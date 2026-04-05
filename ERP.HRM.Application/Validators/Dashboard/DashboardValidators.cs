using ERP.HRM.Application.Features.Dashboard.Queries;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Dashboard
{
    /// <summary>
    /// Validator for GetPayrollDashboardQuery
    /// </summary>
    public class GetPayrollDashboardQueryValidator : AbstractValidator<GetPayrollDashboardQuery>
    {
        public GetPayrollDashboardQueryValidator()
        {
            RuleFor(q => q.Year)
                .GreaterThanOrEqualTo(2020)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .When(q => q.Year.HasValue)
                .WithMessage("Year must be between 2020 and current year");

            RuleFor(q => q.Month)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12)
                .When(q => q.Month.HasValue)
                .WithMessage("Month must be between 1 and 12");
        }
    }

    /// <summary>
    /// Validator for GetDashboardSummaryQuery
    /// </summary>
    public class GetDashboardSummaryQueryValidator : AbstractValidator<GetDashboardSummaryQuery>
    {
        public GetDashboardSummaryQueryValidator()
        {
            // No specific validation needed for this query
        }
    }

    /// <summary>
    /// Validator for GetRecentPayrollQuery
    /// </summary>
    public class GetRecentPayrollQueryValidator : AbstractValidator<GetRecentPayrollQuery>
    {
        public GetRecentPayrollQueryValidator()
        {
            // No specific validation needed for this query
        }
    }

    /// <summary>
    /// Validator for GetDepartmentBreakdownQuery
    /// </summary>
    public class GetDepartmentBreakdownQueryValidator : AbstractValidator<GetDepartmentBreakdownQuery>
    {
        public GetDepartmentBreakdownQueryValidator()
        {
            // No specific validation needed for this query
        }
    }

    /// <summary>
    /// Validator for GetSalaryDistributionQuery
    /// </summary>
    public class GetSalaryDistributionQueryValidator : AbstractValidator<GetSalaryDistributionQuery>
    {
        public GetSalaryDistributionQueryValidator()
        {
            // No specific validation needed for this query
        }
    }
}
