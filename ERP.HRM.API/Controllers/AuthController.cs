using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Auth;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _logger.LogInformation("Register called with Username: {Username}", dto.Username);
            await _authService.RegisterAsync(dto);
            _logger.LogInformation("User '{Username}' registered successfully", dto.Username);
            return Ok(new ApiResponse<string>(true, "Đăng ký thành công", null));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Login called with Username: {Username}", dto.Username);
            var result = await _authService.LoginAsync(dto);
            _logger.LogInformation("User '{Username}' logged in successfully", dto.Username);
            return Ok(new ApiResponse<AuthResponseDto>(true, "Đăng nhập thành công", result));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest dto)
        {
            _logger.LogInformation("Refresh token called");
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            _logger.LogInformation("Token refreshed successfully");
            return Ok(new ApiResponse<AuthResponseDto>(true, "Refresh token thành công", result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest dto)
        {
            _logger.LogInformation("Logout called");
            await _authService.LogoutAsync(dto.RefreshToken);
            _logger.LogInformation("User logged out successfully");
            return Ok(new ApiResponse<string>(true, "Đăng xuất thành công", null));
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            _logger.LogInformation("AssignRole called for User: {Username}, Role: {Role}", dto.Username, dto.Role);
            await _authService.AssignRoleAsync(dto);
            _logger.LogInformation("Role '{Role}' assigned to user '{Username}' successfully", dto.Role, dto.Username);
            return Ok(new ApiResponse<string>(
                true, $"Đã gán role '{dto.Role}' cho user '{dto.Username}'", null));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.Identity?.Name;
            _logger.LogInformation("GetCurrentUser called for User: {Username}", username);
            var result = await _authService.GetCurrentUserAsync(username!);
            return Ok(new ApiResponse<object>(true, "Thông tin user hiện tại", result));
        }
    }
}
