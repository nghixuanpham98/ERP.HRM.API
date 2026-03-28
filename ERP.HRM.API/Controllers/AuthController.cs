using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Auth;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Constants;
using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok(new ApiResponse<string>(true, "Đăng ký thành công", null));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>(true, "Đăng nhập thành công", result));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(new ApiResponse<AuthResponseDto>(true, "Refresh token thành công", result));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest dto)
        {
            await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(new ApiResponse<string>(true, "Đăng xuất thành công", null));
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            await _authService.AssignRoleAsync(dto);
            return Ok(new ApiResponse<string>(
                true, $"Đã gán role '{dto.Role}' cho user '{dto.Username}'", null));
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.Identity?.Name;
            var result = await _authService.GetCurrentUserAsync(username!);
            return Ok(new ApiResponse<object>(true, "Thông tin user hiện tại", result));
        }
    }
}
