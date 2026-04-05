using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class CreateEmployeeValidatorTests
    {
        private readonly CreateEmployeeValidator _validator;

        public CreateEmployeeValidatorTests()
        {
            _validator = new CreateEmployeeValidator();
        }

        [Fact]
        public void Validate_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "0912345678",
                DepartmentId = 1,
                PositionId = 1,
                BaseSalary = 1000000,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptyEmployeeCode_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "",
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "0912345678",
                DepartmentId = 1,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmployeeCode);
        }

        [Fact]
        public void Validate_WithTooLongEmployeeCode_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = new string('A', 51),
                FullName = "John Doe",
                Email = "john@example.com",
                DepartmentId = 1,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EmployeeCode);
        }

        [Fact]
        public void Validate_WithEmptyFullName_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "",
                Email = "john@example.com",
                DepartmentId = 1,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        [Fact]
        public void Validate_WithInvalidEmail_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "invalid-email",
                DepartmentId = 1,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validate_WithInvalidPhoneNumber_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "123",
                DepartmentId = 1,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Validate_WithInvalidDepartmentId_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                DepartmentId = 0,
                PositionId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
        }

        [Fact]
        public void Validate_WithInvalidPositionId_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                DepartmentId = 1,
                PositionId = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionId);
        }

        [Fact]
        public void Validate_WithNegativeBaseSalary_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                DepartmentId = 1,
                PositionId = 1,
                BaseSalary = -100
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BaseSalary);
        }

        [Fact]
        public void Validate_WithFutureHireDate_ShouldFail()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                EmployeeCode = "EMP001",
                FullName = "John Doe",
                Email = "john@example.com",
                DepartmentId = 1,
                PositionId = 1,
                HireDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.HireDate);
        }
    }
}
