using ERP.HRM.Application.DTOs.Auth;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP.HRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            try
            {
                _logger.LogInformation("Register attempt for Username: {Username}", dto.Username);
                var existingUser = await _userManager.FindByNameAsync(dto.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Register failed: User '{Username}' already exists", dto.Username);
                    throw new BusinessRuleException("User đã tồn tại");
                }

                var user = new User
                {
                    UserName = dto.Username,
                    Email = dto.Email,
                    CreatedDate = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Register failed for Username: {Username}. Errors: {Errors}", dto.Username, string.Join(", ", errors));
                    var ex = new BusinessRuleException("Đăng ký thất bại");
                    ex.Data["Errors"] = errors;
                    throw ex;
                }

                await _userManager.AddToRoleAsync(user, RoleConstants.Employee);
                _logger.LogInformation("User '{Username}' registered successfully", dto.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for Username: {Username}", dto.Username);
                throw;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            try
            {
                _logger.LogInformation("Login attempt for Username: {Username}", dto.Username);
                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: User '{Username}' not found", dto.Username);
                    throw new NotFoundException("User không tồn tại");
                }

                var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (!passwordValid)
                {
                    _logger.LogWarning("Login failed: Invalid password for Username: {Username}", dto.Username);
                    throw new BusinessRuleException("Sai mật khẩu");
                }

                var token = await GenerateJwtToken(user);
                var refreshToken = Guid.NewGuid().ToString();

                var refreshEntity = new UserRefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };

                await _unitOfWork.UserRefreshTokenRepository.AddAsync(refreshEntity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("User '{Username}' logged in successfully", dto.Username);

                return new AuthResponseDto
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for Username: {Username}", dto.Username);
                throw;
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("RefreshToken attempt");
                var refreshEntity = await _unitOfWork.UserRefreshTokenRepository.GetByTokenAsync(refreshToken);

                if (refreshEntity == null || refreshEntity.ExpiryDate <= DateTime.UtcNow || refreshEntity.IsRevoked)
                {
                    _logger.LogWarning("RefreshToken failed: Invalid or expired token");
                    throw new BusinessRuleException("Refresh token không hợp lệ");
                }

                var user = await _userManager.FindByIdAsync(refreshEntity.UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarning("RefreshToken failed: User not found for UserId: {UserId}", refreshEntity.UserId);
                    throw new NotFoundException("User không tồn tại");
                }

                // revoke old token
                await _unitOfWork.UserRefreshTokenRepository.RevokeAsync(refreshEntity);

                // issue new token
                var newToken = await GenerateJwtToken(user);
                var newRefreshToken = Guid.NewGuid().ToString();

                var newEntity = new UserRefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };

                await _unitOfWork.UserRefreshTokenRepository.AddAsync(newEntity);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Token refreshed successfully for Username: {Username}", user.UserName);

                return new AuthResponseDto
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                throw;
            }
        }

        public async Task LogoutAsync(string refreshToken)
        {
            try
            {
                _logger.LogInformation("Logout attempt");
                var refreshEntity = await _unitOfWork.UserRefreshTokenRepository.GetByTokenAsync(refreshToken);
                if (refreshEntity != null)
                {
                    await _unitOfWork.UserRefreshTokenRepository.RevokeAsync(refreshEntity);
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation("User logged out successfully");
                }
                else
                {
                    _logger.LogWarning("Logout: Refresh token not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                throw;
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var key = _configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key not configured");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            // Lấy roles từ Identity
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                // Lấy permissions từ repository theo Role
                var rolePermissions = await _unitOfWork.PermissionRepository.GetPermissionsByRoleNameAsync(role);
                foreach (var perm in rolePermissions)
                {
                    claims.Add(new Claim("permissions", perm));
                }
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task AssignRoleAsync(AssignRoleDto dto)
        {
            try
            {
                _logger.LogInformation("AssignRole attempt for Username: {Username}, Role: {Role}", dto.Username, dto.Role);
                if (!RoleConstants.AllRoles.Contains(dto.Role))
                {
                    _logger.LogWarning("AssignRole failed: Invalid role '{Role}'", dto.Role);
                    throw new ValidationException($"Role '{dto.Role}' không hợp lệ. " +
                        $"Chỉ chấp nhận: {string.Join(", ", RoleConstants.AllRoles)}");
                }

                var user = await _userManager.FindByNameAsync(dto.Username);
                if (user == null)
                {
                    _logger.LogWarning("AssignRole failed: User '{Username}' not found", dto.Username);
                    throw new NotFoundException($"User '{dto.Username}' không tồn tại");
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, dto.Role);
                _logger.LogInformation("Role '{Role}' assigned to Username: {Username} successfully", dto.Role, dto.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role '{Role}' to Username: {Username}", dto.Role, dto.Username);
                throw;
            }
        }

        public async Task<object> GetCurrentUserAsync(string username)
        {
            try
            {
                _logger.LogInformation("GetCurrentUser for Username: {Username}", username);
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning("GetCurrentUser failed: User '{Username}' not found", username);
                    throw new NotFoundException("User không tồn tại");
                }

                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation("GetCurrentUser retrieved successfully for Username: {Username}", username);

                return new
                {
                    user.UserName,
                    user.Email,
                    Roles = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user for Username: {Username}", username);
                throw;
            }
        }
    }
}