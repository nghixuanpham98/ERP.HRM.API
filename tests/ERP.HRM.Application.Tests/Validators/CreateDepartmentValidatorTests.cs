using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class CreateDepartmentValidatorTests
    {
        private readonly CreateDepartmentValidator _validator;

        public CreateDepartmentValidatorTests()
        {
            _validator = new CreateDepartmentValidator();
        }

        [Fact]
        public void Validate_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dto = new CreateDepartmentDto { DepartmentName = "IT Department" };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptyName_ShouldFail()
        {
            // Arrange
            var dto = new CreateDepartmentDto { DepartmentName = "" };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentName);
        }

        [Fact]
        public void Validate_WithNullName_ShouldFail()
        {
            // Arrange
            var dto = new CreateDepartmentDto { DepartmentName = null };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentName);
        }

        [Fact]
        public void Validate_WithTooLongName_ShouldFail()
        {
            // Arrange
            var dto = new CreateDepartmentDto { DepartmentName = new string('A', 201) };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentName);
        }
    }
}
