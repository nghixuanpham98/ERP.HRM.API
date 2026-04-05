using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Features.Insurance.Commands;
using ERP.HRM.Application.Services;
using MediatR;

namespace ERP.HRM.Application.Features.Insurance.Handlers
{
    public class InsuranceCommandHandlers :
        IRequestHandler<EnrollEmployeeInInsuranceCommand, InsuranceParticipationDto>,
        IRequestHandler<UpdateInsuranceParticipationCommand, InsuranceParticipationDto>,
        IRequestHandler<TerminateInsuranceParticipationCommand, InsuranceParticipationDto>,
        IRequestHandler<CreateInsuranceTierCommand, InsuranceTierDto>,
        IRequestHandler<UpdateInsuranceTierCommand, InsuranceTierDto>,
        IRequestHandler<DeleteInsuranceTierCommand, bool>
    {
        private readonly IInsuranceManagementService _insuranceService;

        public InsuranceCommandHandlers(IInsuranceManagementService insuranceService)
        {
            _insuranceService = insuranceService ?? throw new ArgumentNullException(nameof(insuranceService));
        }

        public async Task<InsuranceParticipationDto> Handle(EnrollEmployeeInInsuranceCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceService.EnrollEmployeeInInsuranceAsync(request.InsuranceData, cancellationToken);
        }

        public async Task<InsuranceParticipationDto> Handle(UpdateInsuranceParticipationCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceService.UpdateInsuranceParticipationAsync(request.ParticipationId, request.InsuranceData, cancellationToken);
        }

        public async Task<InsuranceParticipationDto> Handle(TerminateInsuranceParticipationCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceService.TerminateInsuranceParticipationAsync(request.ParticipationId, cancellationToken);
        }

        public async Task<InsuranceTierDto> Handle(CreateInsuranceTierCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceService.CreateInsuranceTierAsync(request.TierData, cancellationToken);
        }

        public async Task<InsuranceTierDto> Handle(UpdateInsuranceTierCommand request, CancellationToken cancellationToken)
        {
            return await _insuranceService.UpdateInsuranceTierAsync(request.TierId, request.TierData, cancellationToken);
        }

        public async Task<bool> Handle(DeleteInsuranceTierCommand request, CancellationToken cancellationToken)
        {
            // For deletion, we typically soft-delete by marking IsActive = false
            // Get the tier first
            var tier = await _insuranceService.GetInsuranceTierAsync(request.TierId, cancellationToken);
            var updateDto = new UpdateInsuranceTierDto
            {
                TierName = tier.TierName,
                MinSalary = tier.MinSalary,
                MaxSalary = tier.MaxSalary,
                EmployeeRate = tier.EmployeeRate,
                EmployerRate = tier.EmployerRate,
                EndDate = DateTime.UtcNow,
                IsActive = false
            };

            var result = await _insuranceService.UpdateInsuranceTierAsync(request.TierId, updateDto, cancellationToken);
            return result != null;
        }
    }
}
