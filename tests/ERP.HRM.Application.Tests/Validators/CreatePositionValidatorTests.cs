using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class CreatePositionValidatorTests
    {
        private readonly CreatePositionValidator _validator;

        public CreatePositionValidatorTests()
        {
            _validator = new CreatePositionValidator();
        }

        [Fact]
        public void Validate_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dto = new CreatePositionDto 
            { 
                PositionCode = "POS001",
                PositionName = "Software Engineer",
                DepartmentId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WithEmptyPositionCode_ShouldFail()
        {
            // Arrange
            var dto = new CreatePositionDto
            {
                PositionCode = "",
                PositionName = "Software Engineer",
                DepartmentId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionCode);
        }

        [Fact]
        public void Validate_WithEmptyPositionName_ShouldFail()
        {
            // Arrange
            var dto = new CreatePositionDto
            {
                PositionCode = "POS001",
                PositionName = "",
                DepartmentId = 1
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PositionName);
        }

        [Fact]
        public void Validate_WithInvalidDepartmentId_ShouldFail()
        {
            // Arrange
            var dto = new CreatePositionDto
            {
                PositionCode = "POS001",
                PositionName = "Software Engineer",
                DepartmentId = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DepartmentId);
        }
    }
}
