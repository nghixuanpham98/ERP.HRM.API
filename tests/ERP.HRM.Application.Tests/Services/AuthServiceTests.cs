using ERP.HRM.Application.DTOs.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockConfiguration = new Mock<IConfiguration>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _service = new AuthService(_mockUserManager.Object, _mockConfiguration.Object, _mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithUniqueUsername_ShouldCreateUser()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.Username))
                .ReturnsAsync((User)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            await _service.RegisterAsync(registerDto);

            // Assert
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingUsername_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Email = "existing@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            var existingUser = new User { UserName = "existinguser" };
            _mockUserManager.Setup(x => x.FindByNameAsync(registerDto.Username))
                .ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(() => _service.RegisterAsync(registerDto));
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "Password123!"
            };

            var user = new User { UserName = "testuser", Email = "test@example.com" };
            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(true);
            _mockUserManager.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "User" });

            _mockConfiguration.Setup(x => x["Jwt:SecretKey"])
                .Returns("this-is-a-very-long-secret-key-for-jwt-token-generation-purpose");
            _mockConfiguration.Setup(x => x["Jwt:ExpirationMinutes"])
                .Returns("60");

            // Act
            var result = await _service.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.AccessToken);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "WrongPassword"
            };

            var user = new User { UserName = "testuser" };
            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginDto.Password))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
        }

        [Fact]
        public async Task LoginAsync_WithNonexistentUser_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "nonexistentuser",
                Password = "Password123!"
            };

            _mockUserManager.Setup(x => x.FindByNameAsync(loginDto.Username))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
        }
    }
}
