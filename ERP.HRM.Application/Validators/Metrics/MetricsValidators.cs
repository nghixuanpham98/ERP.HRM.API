using ERP.HRM.Application.Features.Metrics.Queries;
using FluentValidation;

namespace ERP.HRM.Application.Validators.Metrics
{
    /// <summary>
    /// Validator for GetDepartmentMetricsQuery
    /// </summary>
    public class GetDepartmentMetricsQueryValidator : AbstractValidator<GetDepartmentMetricsQuery>
    {
        public GetDepartmentMetricsQueryValidator()
        {
            RuleFor(q => q.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Department ID must be greater than 0");
        }
    }

    /// <summary>
    /// Validator for GetDepartmentComparisonQuery
    /// </summary>
    public class GetDepartmentComparisonQueryValidator : AbstractValidator<GetDepartmentComparisonQuery>
    {
        public GetDepartmentComparisonQueryValidator()
        {
            // No specific validation needed for this query
        }
    }

    /// <summary>
    /// Validator for GetEmployeeDistributionQuery
    /// </summary>
    public class GetEmployeeDistributionQueryValidator : AbstractValidator<GetEmployeeDistributionQuery>
    {
        public GetEmployeeDistributionQueryValidator()
        {
            // No specific validation needed for this query
        }
    }

    /// <summary>
    /// Validator for GetDepartmentPayrollTrendQuery
    /// </summary>
    public class GetDepartmentPayrollTrendQueryValidator : AbstractValidator<GetDepartmentPayrollTrendQuery>
    {
        public GetDepartmentPayrollTrendQueryValidator()
        {
            RuleFor(q => q.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Department ID must be greater than 0");
        }
    }

    /// <summary>
    /// Validator for GetDepartmentAnalyticsQuery
    /// </summary>
    public class GetDepartmentAnalyticsQueryValidator : AbstractValidator<GetDepartmentAnalyticsQuery>
    {
        public GetDepartmentAnalyticsQueryValidator()
        {
            RuleFor(q => q.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Department ID must be greater than 0");
        }
    }
}
