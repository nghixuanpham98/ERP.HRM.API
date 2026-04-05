using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Insurance.Queries;
using ERP.HRM.Application.Services;
using MediatR;

namespace ERP.HRM.Application.Features.Insurance.Handlers
{
    public class InsuranceQueryHandlers :
        IRequestHandler<GetInsuranceParticipationQuery, InsuranceParticipationDto>,
        IRequestHandler<GetEmployeeInsurancesQuery, IEnumerable<InsuranceParticipationDto>>,
        IRequestHandler<GetActiveInsurancesQuery, IEnumerable<InsuranceParticipationDto>>,
        IRequestHandler<GetInsuranceTierQuery, InsuranceTierDto>,
        IRequestHandler<GetAllActiveInsuranceTiersQuery, IEnumerable<InsuranceTierDto>>,
        IRequestHandler<GetInsuranceTiersByTypeQuery, IEnumerable<InsuranceTierDto>>,
        IRequestHandler<CalculateInsuranceContributionQuery, InsuranceCalculationResultDto>,
        IRequestHandler<CalculateTotalInsuranceContributionQuery, decimal>
    {
        private readonly IInsuranceManagementService _insuranceService;

        public InsuranceQueryHandlers(IInsuranceManagementService insuranceService)
        {
            _insuranceService = insuranceService ?? throw new ArgumentNullException(nameof(insuranceService));
        }

        public async Task<InsuranceParticipationDto> Handle(GetInsuranceParticipationQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetInsuranceParticipationAsync(request.ParticipationId, cancellationToken);
        }

        public async Task<IEnumerable<InsuranceParticipationDto>> Handle(GetEmployeeInsurancesQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetEmployeeInsurancesAsync(request.EmployeeId, cancellationToken);
        }

        public async Task<IEnumerable<InsuranceParticipationDto>> Handle(GetActiveInsurancesQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetActiveInsurancesAsync(cancellationToken);
        }

        public async Task<InsuranceTierDto> Handle(GetInsuranceTierQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetInsuranceTierAsync(request.TierId, cancellationToken);
        }

        public async Task<IEnumerable<InsuranceTierDto>> Handle(GetAllActiveInsuranceTiersQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetAllActiveInsuranceTiersAsync(cancellationToken);
        }

        public async Task<IEnumerable<InsuranceTierDto>> Handle(GetInsuranceTiersByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.GetInsuranceTiersByTypeAsync(request.InsuranceType, cancellationToken);
        }

        public async Task<InsuranceCalculationResultDto> Handle(CalculateInsuranceContributionQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.CalculateInsuranceContributionAsync(request.EmployeeId, request.InsuranceType, request.Salary, cancellationToken);
        }

        public async Task<decimal> Handle(CalculateTotalInsuranceContributionQuery request, CancellationToken cancellationToken)
        {
            return await _insuranceService.CalculateTotalInsuranceContributionAsync(request.EmployeeId, request.TotalSalary, cancellationToken);
        }
    }
}
