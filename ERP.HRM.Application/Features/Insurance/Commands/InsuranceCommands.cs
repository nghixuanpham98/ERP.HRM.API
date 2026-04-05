using ERP.HRM.Application.DTOs.HR;
using MediatR;

namespace ERP.HRM.Application.Features.Insurance.Commands
{
    // Enrollment Commands
    public record EnrollEmployeeInInsuranceCommand(CreateInsuranceParticipationDto InsuranceData) : IRequest<InsuranceParticipationDto>;

    public record UpdateInsuranceParticipationCommand(int ParticipationId, UpdateInsuranceParticipationDto InsuranceData) : IRequest<InsuranceParticipationDto>;

    public record TerminateInsuranceParticipationCommand(int ParticipationId) : IRequest<InsuranceParticipationDto>;

    // Insurance Tier Commands
    public record CreateInsuranceTierCommand(CreateInsuranceTierDto TierData) : IRequest<InsuranceTierDto>;

    public record UpdateInsuranceTierCommand(int TierId, UpdateInsuranceTierDto TierData) : IRequest<InsuranceTierDto>;

    public record DeleteInsuranceTierCommand(int TierId) : IRequest<bool>;
}
