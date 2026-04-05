using ERP.HRM.Application.Features.Reporting.Queries;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Reporting
{
    /// <summary>
    /// Validator for GetMonthlyPayrollSummaryQuery
    /// </summary>
    public class GetMonthlyPayrollSummaryQueryValidator : AbstractValidator<GetMonthlyPayrollSummaryQuery>
    {
        public GetMonthlyPayrollSummaryQueryValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");

            RuleFor(x => x.Month)
                .NotEmpty().WithMessage("Month is required")
                .GreaterThanOrEqualTo(1).WithMessage("Month must be between 1 and 12")
                .LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12");
        }
    }

    /// <summary>
    /// Validator for GetTaxSummaryReportQuery
    /// </summary>
    public class GetTaxSummaryReportQueryValidator : AbstractValidator<GetTaxSummaryReportQuery>
    {
        public GetTaxSummaryReportQueryValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");

            RuleFor(x => x.Month)
                .NotEmpty().WithMessage("Month is required")
                .GreaterThanOrEqualTo(1).WithMessage("Month must be between 1 and 12")
                .LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12");
        }
    }

    /// <summary>
    /// Validator for GetInsuranceSummaryReportQuery
    /// </summary>
    public class GetInsuranceSummaryReportQueryValidator : AbstractValidator<GetInsuranceSummaryReportQuery>
    {
        public GetInsuranceSummaryReportQueryValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");

            RuleFor(x => x.Month)
                .NotEmpty().WithMessage("Month is required")
                .GreaterThanOrEqualTo(1).WithMessage("Month must be between 1 and 12")
                .LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12");
        }
    }

    /// <summary>
    /// Validator for GetLeaveSummaryReportQuery
    /// </summary>
    public class GetLeaveSummaryReportQueryValidator : AbstractValidator<GetLeaveSummaryReportQuery>
    {
        public GetLeaveSummaryReportQueryValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");

            RuleFor(x => x.Month)
                .NotEmpty().WithMessage("Month is required")
                .GreaterThanOrEqualTo(1).WithMessage("Month must be between 1 and 12")
                .LessThanOrEqualTo(12).WithMessage("Month must be between 1 and 12");
        }
    }

    /// <summary>
    /// Validator for GetHRMetricsQuery
    /// </summary>
    public class GetHRMetricsQueryValidator : AbstractValidator<GetHRMetricsQuery>
    {
        public GetHRMetricsQueryValidator()
        {
            RuleFor(x => x.Year)
                .NotEmpty().WithMessage("Year is required")
                .GreaterThanOrEqualTo(2020).WithMessage("Year must be 2020 or later")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year cannot be in the future");
        }
    }
}
