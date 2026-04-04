using FluentValidation;
using ERP.HRM.Application.DTOs.Production;

namespace ERP.HRM.Application.Validators.Production
{
    /// <summary>
    /// Validator for RecordProductionOutputV2Dto
    /// </summary>
    public class RecordProductionOutputV2Validator : AbstractValidator<RecordProductionOutputV2Dto>
    {
        public RecordProductionOutputV2Validator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be valid");

            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0).WithMessage("Payroll period ID must be valid");

            RuleFor(x => x.ProductionStageId)
                .GreaterThan(0).WithMessage("Production stage ID must be valid");

            RuleFor(x => x.ProductionJobId)
                .GreaterThan(0).WithMessage("Production job ID must be valid");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be valid");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.JobComplexityMultiplier)
                .GreaterThan(0).WithMessage("Job complexity multiplier must be greater than 0");

            RuleFor(x => x.WorkerSkillMultiplier)
                .GreaterThan(0).WithMessage("Worker skill multiplier must be greater than 0");

            RuleFor(x => x.ProductionDate)
                .NotEmpty().WithMessage("Production date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Production date cannot be in the future");

            RuleFor(x => x.QualityStatus)
                .Must(x => new[] { "OK", "Defective", "Rework", "Rejected" }.Contains(x))
                .WithMessage("Invalid quality status");

            RuleFor(x => x.QualityAdjustmentPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("Quality adjustment percentage cannot be negative")
                .LessThanOrEqualTo(1).WithMessage("Quality adjustment percentage cannot exceed 100%");
        }
    }
}
