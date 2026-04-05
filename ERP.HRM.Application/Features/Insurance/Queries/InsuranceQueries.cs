using ERP.HRM.Application.DTOs.HR;
using MediatR;

namespace ERP.HRM.Application.Features.Insurance.Queries
{
    // Insurance Participation Queries
    public record GetInsuranceParticipationQuery(int ParticipationId) : IRequest<InsuranceParticipationDto>;

    public record GetEmployeeInsurancesQuery(int EmployeeId) : IRequest<IEnumerable<InsuranceParticipationDto>>;

    public record GetActiveInsurancesQuery() : IRequest<IEnumerable<InsuranceParticipationDto>>;

    // Insurance Tier Queries
    public record GetInsuranceTierQuery(int TierId) : IRequest<InsuranceTierDto>;

    public record GetAllActiveInsuranceTiersQuery() : IRequest<IEnumerable<InsuranceTierDto>>;

    public record GetInsuranceTiersByTypeQuery(string InsuranceType) : IRequest<IEnumerable<InsuranceTierDto>>;

    // Calculation Queries
    public record CalculateInsuranceContributionQuery(int EmployeeId, string InsuranceType, decimal Salary) : IRequest<InsuranceCalculationResultDto>;

    public record CalculateTotalInsuranceContributionQuery(int EmployeeId, decimal TotalSalary) : IRequest<decimal>;
}
