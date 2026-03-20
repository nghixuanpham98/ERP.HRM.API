using ERP.HRM.API;
using ERP.HRM.Application.DTOs.Auth;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;
        private readonly IPermissionRepository _permissionRepository;

        public AuthService(
            UserManager<User> userManager,
            IConfiguration configuration,
            IUserRefreshTokenRepository refreshTokenRepository,
            IPermissionRepository permissionRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                throw new BusinessRuleException("User đã tồn tại");

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
                var ex = new BusinessRuleException("Đăng ký thất bại");
                ex.Data["Errors"] = errors;
                throw ex;
            }

            // Gán role mặc định
            await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                throw new BusinessRuleException("Sai mật khẩu");

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

            await _refreshTokenRepository.AddAsync(refreshEntity);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var refreshEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (refreshEntity == null || refreshEntity.ExpiryDate <= DateTime.UtcNow || refreshEntity.IsRevoked)
                throw new BusinessRuleException("Refresh token không hợp lệ");

            var user = await _userManager.FindByIdAsync(refreshEntity.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User không tồn tại");

            // revoke old token
            await _refreshTokenRepository.RevokeAsync(refreshEntity);

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

            await _refreshTokenRepository.AddAsync(newEntity);

            return new AuthResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var refreshEntity = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (refreshEntity != null)
            {
                await _refreshTokenRepository.RevokeAsync(refreshEntity);
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
                var rolePermissions = await _permissionRepository.GetPermissionsByRoleNameAsync(role);
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
    }
}