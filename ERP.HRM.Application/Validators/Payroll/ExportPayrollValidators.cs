using ERP.HRM.Application.Features.Payroll.Commands;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Payroll
{
    /// <summary>
    /// Validator for ExportPayrollCommand
    /// </summary>
    public class ExportPayrollCommandValidator : AbstractValidator<ExportPayrollCommand>
    {
        public ExportPayrollCommandValidator()
        {
            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Payroll Period ID must be greater than 0");

            RuleFor(x => x.ExportFormat)
                .NotEmpty()
                .WithMessage("Export format is required")
                .Must(x => x.Equals("Excel", StringComparison.OrdinalIgnoreCase) || 
                           x.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Export format must be 'Excel' or 'PDF'");

            RuleFor(x => x.ExportPurpose)
                .NotEmpty()
                .WithMessage("Export purpose is required")
                .Must(x => x.Equals("Bank", StringComparison.OrdinalIgnoreCase) ||
                           x.Equals("TaxAuthority", StringComparison.OrdinalIgnoreCase) ||
                           x.Equals("General", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Export purpose must be 'Bank', 'TaxAuthority', or 'General'");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .When(x => x.DepartmentId.HasValue)
                .WithMessage("Department ID must be greater than 0 if specified");
        }
    }

    /// <summary>
    /// Validator for ExportPayrollForBankCommand
    /// </summary>
    public class ExportPayrollForBankCommandValidator : AbstractValidator<ExportPayrollForBankCommand>
    {
        public ExportPayrollForBankCommandValidator()
        {
            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Payroll Period ID must be greater than 0");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .When(x => x.DepartmentId.HasValue)
                .WithMessage("Department ID must be greater than 0 if specified");
        }
    }

    /// <summary>
    /// Validator for ExportPayrollForTaxAuthorityCommand
    /// </summary>
    public class ExportPayrollForTaxAuthorityCommandValidator : AbstractValidator<ExportPayrollForTaxAuthorityCommand>
    {
        public ExportPayrollForTaxAuthorityCommandValidator()
        {
            RuleFor(x => x.PayrollPeriodId)
                .GreaterThan(0)
                .WithMessage("Payroll Period ID must be greater than 0");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .When(x => x.DepartmentId.HasValue)
                .WithMessage("Department ID must be greater than 0 if specified");
        }
    }
}
