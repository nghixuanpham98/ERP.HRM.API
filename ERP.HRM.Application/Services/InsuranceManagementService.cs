using AutoMapper;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Service for managing insurance participation and tiers
    /// Provides business logic for insurance enrollment, updates, and calculations
    /// </summary>
    public interface IInsuranceManagementService
    {
        // Insurance Participation Operations
        Task<InsuranceParticipationDto> EnrollEmployeeInInsuranceAsync(CreateInsuranceParticipationDto dto, CancellationToken cancellationToken = default);
        Task<InsuranceParticipationDto> UpdateInsuranceParticipationAsync(int participationId, UpdateInsuranceParticipationDto dto, CancellationToken cancellationToken = default);
        Task<InsuranceParticipationDto> TerminateInsuranceParticipationAsync(int participationId, CancellationToken cancellationToken = default);
        Task<InsuranceParticipationDto> GetInsuranceParticipationAsync(int participationId, CancellationToken cancellationToken = default);
        Task<IEnumerable<InsuranceParticipationDto>> GetEmployeeInsurancesAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<InsuranceParticipationDto>> GetActiveInsurancesAsync(CancellationToken cancellationToken = default);

        // Insurance Tier Operations
        Task<InsuranceTierDto> CreateInsuranceTierAsync(CreateInsuranceTierDto dto, CancellationToken cancellationToken = default);
        Task<InsuranceTierDto> UpdateInsuranceTierAsync(int tierId, UpdateInsuranceTierDto dto, CancellationToken cancellationToken = default);
        Task<InsuranceTierDto> GetInsuranceTierAsync(int tierId, CancellationToken cancellationToken = default);
        Task<IEnumerable<InsuranceTierDto>> GetAllActiveInsuranceTiersAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<InsuranceTierDto>> GetInsuranceTiersByTypeAsync(string insuranceType, CancellationToken cancellationToken = default);

        // Calculation Operations
        Task<InsuranceCalculationResultDto> CalculateInsuranceContributionAsync(int employeeId, string insuranceType, decimal salary, CancellationToken cancellationToken = default);
        Task<decimal> CalculateTotalInsuranceContributionAsync(int employeeId, decimal totalSalary, CancellationToken cancellationToken = default);
    }

    public class InsuranceManagementService : IInsuranceManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InsuranceManagementService> _logger;

        public InsuranceManagementService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<InsuranceManagementService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<InsuranceParticipationDto> EnrollEmployeeInInsuranceAsync(CreateInsuranceParticipationDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Enrolling employee {dto.EmployeeId} in {dto.InsuranceType} insurance");

            // Validate employee exists
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {dto.EmployeeId} not found");

            // Validate no duplicate active insurance of same type
            var existing = await _unitOfWork.InsuranceParticipationRepository.GetActiveInsuranceByEmployeeAndTypeAsync(dto.EmployeeId, dto.InsuranceType);
            if (existing != null)
                throw new BusinessRuleException($"Employee already has active {dto.InsuranceType} insurance");

            // Validate dates
            if (dto.EndDate.HasValue && dto.EndDate <= dto.StartDate)
                throw new BusinessRuleException("End date must be after start date");

            // Create new insurance participation
            var participation = new InsuranceParticipation
            {
                EmployeeId = dto.EmployeeId,
                InsuranceType = dto.InsuranceType,
                InsuranceNumber = dto.InsuranceNumber,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = "Active",
                ContributionBaseSalary = dto.ContributionBaseSalary,
                EmployeeContributionRate = dto.EmployeeContributionRate,
                EmployerContributionRate = dto.EmployerContributionRate
            };

            await _unitOfWork.InsuranceParticipationRepository.AddAsync(participation);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully enrolled employee {dto.EmployeeId} in insurance");
            return _mapper.Map<InsuranceParticipationDto>(participation);
        }

        public async Task<InsuranceParticipationDto> UpdateInsuranceParticipationAsync(int participationId, UpdateInsuranceParticipationDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating insurance participation {participationId}");

            var participation = await _unitOfWork.InsuranceParticipationRepository.GetByIdAsync(participationId);
            if (participation == null)
                throw new NotFoundException($"Insurance participation with Id {participationId} not found");

            // Validate dates if being updated
            if (dto.EndDate.HasValue && dto.EndDate <= dto.StartDate)
                throw new BusinessRuleException("End date must be after start date");

            // Update fields
            participation.InsuranceNumber = dto.InsuranceNumber;
            participation.StartDate = dto.StartDate;
            participation.EndDate = dto.EndDate;
            participation.Status = dto.Status;
            participation.ContributionBaseSalary = dto.ContributionBaseSalary;
            participation.EmployeeContributionRate = dto.EmployeeContributionRate;
            participation.EmployerContributionRate = dto.EmployerContributionRate;
            participation.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.InsuranceParticipationRepository.UpdateAsync(participation);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated insurance participation {participationId}");
            return _mapper.Map<InsuranceParticipationDto>(participation);
        }

        public async Task<InsuranceParticipationDto> TerminateInsuranceParticipationAsync(int participationId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Terminating insurance participation {participationId}");

            var participation = await _unitOfWork.InsuranceParticipationRepository.GetByIdAsync(participationId);
            if (participation == null)
                throw new NotFoundException($"Insurance participation with Id {participationId} not found");

            if (participation.Status == "Terminated")
                throw new BusinessRuleException("Insurance participation is already terminated");

            participation.Status = "Terminated";
            participation.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            participation.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.InsuranceParticipationRepository.UpdateAsync(participation);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully terminated insurance participation {participationId}");
            return _mapper.Map<InsuranceParticipationDto>(participation);
        }

        public async Task<InsuranceParticipationDto> GetInsuranceParticipationAsync(int participationId, CancellationToken cancellationToken = default)
        {
            var participation = await _unitOfWork.InsuranceParticipationRepository.GetByIdAsync(participationId);
            if (participation == null)
                throw new NotFoundException($"Insurance participation with Id {participationId} not found");

            return _mapper.Map<InsuranceParticipationDto>(participation);
        }

        public async Task<IEnumerable<InsuranceParticipationDto>> GetEmployeeInsurancesAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {employeeId} not found");

            var insurances = await _unitOfWork.InsuranceParticipationRepository.GetByEmployeeIdAsync(employeeId);
            return _mapper.Map<IEnumerable<InsuranceParticipationDto>>(insurances);
        }

        public async Task<IEnumerable<InsuranceParticipationDto>> GetActiveInsurancesAsync(CancellationToken cancellationToken = default)
        {
            var insurances = await _unitOfWork.InsuranceParticipationRepository.GetActiveInsurancesAsync();
            return _mapper.Map<IEnumerable<InsuranceParticipationDto>>(insurances);
        }

        public async Task<InsuranceTierDto> CreateInsuranceTierAsync(CreateInsuranceTierDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Creating insurance tier {dto.TierName} for {dto.InsuranceType}");

            // Validate salary range
            if (dto.MaxSalary <= dto.MinSalary)
                throw new BusinessRuleException("Maximum salary must be greater than minimum salary");

            // Validate rates
            if (dto.EmployeeRate < 0 || dto.EmployerRate < 0)
                throw new BusinessRuleException("Contribution rates cannot be negative");

            var tier = new InsuranceTier
            {
                TierName = dto.TierName,
                InsuranceType = dto.InsuranceType,
                MinSalary = dto.MinSalary,
                MaxSalary = dto.MaxSalary,
                EmployeeRate = dto.EmployeeRate,
                EmployerRate = dto.EmployerRate,
                EffectiveDate = dto.EffectiveDate,
                EndDate = null,
                IsActive = true
            };

            await _unitOfWork.InsuranceTierRepository.AddAsync(tier);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully created insurance tier {dto.TierName}");
            return _mapper.Map<InsuranceTierDto>(tier);
        }

        public async Task<InsuranceTierDto> UpdateInsuranceTierAsync(int tierId, UpdateInsuranceTierDto dto, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Updating insurance tier {tierId}");

            var tier = await _unitOfWork.InsuranceTierRepository.GetByIdAsync(tierId);
            if (tier == null)
                throw new NotFoundException($"Insurance tier with Id {tierId} not found");

            // Validate salary range if being updated
            if (dto.MaxSalary <= dto.MinSalary)
                throw new BusinessRuleException("Maximum salary must be greater than minimum salary");

            // Validate rates
            if (dto.EmployeeRate < 0 || dto.EmployerRate < 0)
                throw new BusinessRuleException("Contribution rates cannot be negative");

            tier.TierName = dto.TierName;
            tier.MinSalary = dto.MinSalary;
            tier.MaxSalary = dto.MaxSalary;
            tier.EmployeeRate = dto.EmployeeRate;
            tier.EmployerRate = dto.EmployerRate;
            tier.EndDate = dto.EndDate;
            tier.IsActive = dto.IsActive;

            await _unitOfWork.InsuranceTierRepository.UpdateAsync(tier);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated insurance tier {tierId}");
            return _mapper.Map<InsuranceTierDto>(tier);
        }

        public async Task<InsuranceTierDto> GetInsuranceTierAsync(int tierId, CancellationToken cancellationToken = default)
        {
            var tier = await _unitOfWork.InsuranceTierRepository.GetByIdAsync(tierId);
            if (tier == null)
                throw new NotFoundException($"Insurance tier with Id {tierId} not found");

            return _mapper.Map<InsuranceTierDto>(tier);
        }

        public async Task<IEnumerable<InsuranceTierDto>> GetAllActiveInsuranceTiersAsync(CancellationToken cancellationToken = default)
        {
            var tiers = await _unitOfWork.InsuranceTierRepository.GetActiveTiersAsync(DateTime.UtcNow);
            return _mapper.Map<IEnumerable<InsuranceTierDto>>(tiers);
        }

        public async Task<IEnumerable<InsuranceTierDto>> GetInsuranceTiersByTypeAsync(string insuranceType, CancellationToken cancellationToken = default)
        {
            var tiers = await _unitOfWork.InsuranceTierRepository.GetByTypeAsync(insuranceType);
            return _mapper.Map<IEnumerable<InsuranceTierDto>>(tiers);
        }

        public async Task<InsuranceCalculationResultDto> CalculateInsuranceContributionAsync(int employeeId, string insuranceType, decimal salary, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Calculating {insuranceType} insurance contribution for employee {employeeId} with salary {salary}");

            // Validate employee exists
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {employeeId} not found");

            // Get active insurance participation
            var participation = await _unitOfWork.InsuranceParticipationRepository.GetActiveInsuranceByEmployeeAndTypeAsync(employeeId, insuranceType);
            if (participation == null)
                throw new NotFoundException($"Employee does not have active {insuranceType} insurance");

            // Get applicable tier
            var tier = await _unitOfWork.InsuranceTierRepository.GetTierForSalaryAsync(salary, insuranceType, DateTime.UtcNow);
            if (tier == null)
                throw new BusinessRuleException($"No applicable insurance tier found for salary {salary} and type {insuranceType}");

            // Calculate contributions
            decimal employeeContribution = (salary * tier.EmployeeRate) / 100;
            decimal employerContribution = (salary * tier.EmployerRate) / 100;
            decimal totalContribution = employeeContribution + employerContribution;

            var result = new InsuranceCalculationResultDto
            {
                EmployeeId = employeeId,
                InsuranceType = insuranceType,
                BaseSalary = salary,
                EmployeeContributionRate = tier.EmployeeRate,
                EmployerContributionRate = tier.EmployerRate,
                EmployeeContributionAmount = employeeContribution,
                EmployerContributionAmount = employerContribution,
                TotalContributionAmount = totalContribution,
                CalculatedAt = DateTime.UtcNow
            };

            _logger.LogInformation($"Calculated contribution: Employee={employeeContribution}, Employer={employerContribution}, Total={totalContribution}");
            return result;
        }

        public async Task<decimal> CalculateTotalInsuranceContributionAsync(int employeeId, decimal totalSalary, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Calculating total insurance contribution for employee {employeeId}");

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new NotFoundException($"Employee with Id {employeeId} not found");

            var insurances = await _unitOfWork.InsuranceParticipationRepository.GetByEmployeeIdAsync(employeeId);
            decimal totalContribution = 0;

            foreach (var insurance in insurances)
            {
                if (insurance.Status != "Active")
                    continue;

                var tier = await _unitOfWork.InsuranceTierRepository.GetTierForSalaryAsync(totalSalary, insurance.InsuranceType, DateTime.UtcNow);
                if (tier != null)
                {
                    totalContribution += (totalSalary * (tier.EmployeeRate + tier.EmployerRate)) / 100;
                }
            }

            _logger.LogInformation($"Total insurance contribution calculated: {totalContribution}");
            return totalContribution;
        }
    }
}
