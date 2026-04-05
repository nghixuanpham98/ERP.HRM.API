using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests.Services
{
    public class InsuranceManagementServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<InsuranceManagementService>> _loggerMock;
        private readonly InsuranceManagementService _service;

        public InsuranceManagementServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<InsuranceManagementService>>();
            _service = new InsuranceManagementService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        #region Insurance Participation Tests

        [Fact]
        public async Task EnrollEmployeeInInsurance_WithValidData_ShouldSucceed()
        {
            // Arrange
            var employeeId = 1;
            var dto = new CreateInsuranceParticipationDto
            {
                EmployeeId = employeeId,
                InsuranceType = "Health",
                InsuranceNumber = "INS001",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = null,
                ContributionBaseSalary = 10000,
                EmployeeContributionRate = 2,
                EmployerContributionRate = 3
            };

            var employee = new Employee { EmployeeId = employeeId };
            var participation = new InsuranceParticipation
            {
                InsuranceParticipationId = 1,
                EmployeeId = employeeId,
                InsuranceType = dto.InsuranceType,
                InsuranceNumber = dto.InsuranceNumber,
                Status = "Active"
            };

            _unitOfWorkMock.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.GetActiveInsuranceByEmployeeAndTypeAsync(employeeId, "Health"))
                .ReturnsAsync((InsuranceParticipation)null!);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.AddAsync(It.IsAny<InsuranceParticipation>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<InsuranceParticipationDto>(It.IsAny<InsuranceParticipation>()))
                .Returns(new InsuranceParticipationDto { InsuranceParticipationId = 1, EmployeeId = employeeId });

            // Act
            var result = await _service.EnrollEmployeeInInsuranceAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EnrollEmployeeInInsurance_WithInvalidEmployee_ShouldThrowNotFoundException()
        {
            // Arrange
            var dto = new CreateInsuranceParticipationDto
            {
                EmployeeId = 999,
                InsuranceType = "Health",
                InsuranceNumber = "INS001",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ContributionBaseSalary = 10000,
                EmployeeContributionRate = 2,
                EmployerContributionRate = 3
            };

            _unitOfWorkMock.Setup(x => x.EmployeeRepository.GetByIdAsync(999))
                .ReturnsAsync((Employee)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.EnrollEmployeeInInsuranceAsync(dto));
        }

        [Fact]
        public async Task UpdateInsuranceParticipation_WithValidData_ShouldSucceed()
        {
            // Arrange
            var participationId = 1;
            var dto = new UpdateInsuranceParticipationDto
            {
                InsuranceNumber = "INS002",
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = null,
                Status = "Active",
                ContributionBaseSalary = 12000,
                EmployeeContributionRate = 2.5m,
                EmployerContributionRate = 3.5m
            };

            var participation = new InsuranceParticipation
            {
                InsuranceParticipationId = participationId,
                Status = "Active"
            };

            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.GetByIdAsync(participationId))
                .ReturnsAsync(participation);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.UpdateAsync(It.IsAny<InsuranceParticipation>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<InsuranceParticipationDto>(It.IsAny<InsuranceParticipation>()))
                .Returns(new InsuranceParticipationDto { InsuranceParticipationId = participationId });

            // Act
            var result = await _service.UpdateInsuranceParticipationAsync(participationId, dto);

            // Assert
            Assert.NotNull(result);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task TerminateInsuranceParticipation_WithValidId_ShouldSucceed()
        {
            // Arrange
            var participationId = 1;
            var participation = new InsuranceParticipation
            {
                InsuranceParticipationId = participationId,
                Status = "Active"
            };

            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.GetByIdAsync(participationId))
                .ReturnsAsync(participation);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.UpdateAsync(It.IsAny<InsuranceParticipation>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<InsuranceParticipationDto>(It.IsAny<InsuranceParticipation>()))
                .Returns(new InsuranceParticipationDto { InsuranceParticipationId = participationId, Status = "Terminated" });

            // Act
            var result = await _service.TerminateInsuranceParticipationAsync(participationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Terminated", result.Status);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region Insurance Tier Tests

        [Fact]
        public async Task CreateInsuranceTier_WithValidData_ShouldSucceed()
        {
            // Arrange
            var dto = new CreateInsuranceTierDto
            {
                TierName = "Tier 1",
                InsuranceType = "Health",
                MinSalary = 0,
                MaxSalary = 10000,
                EmployeeRate = 2,
                EmployerRate = 3,
                EffectiveDate = DateTime.UtcNow
            };

            var tier = new InsuranceTier
            {
                InsuranceTierId = 1,
                TierName = dto.TierName,
                InsuranceType = dto.InsuranceType
            };

            _unitOfWorkMock.Setup(x => x.InsuranceTierRepository.AddAsync(It.IsAny<InsuranceTier>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<InsuranceTierDto>(It.IsAny<InsuranceTier>()))
                .Returns(new InsuranceTierDto { InsuranceTierId = 1, TierName = dto.TierName });

            // Act
            var result = await _service.CreateInsuranceTierAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.TierName, result.TierName);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateInsuranceTier_WithInvalidSalaryRange_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreateInsuranceTierDto
            {
                TierName = "Tier 1",
                InsuranceType = "Health",
                MinSalary = 10000,
                MaxSalary = 5000,  // Invalid: max < min
                EmployeeRate = 2,
                EmployerRate = 3,
                EffectiveDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.CreateInsuranceTierAsync(dto));
        }

        [Fact]
        public async Task GetInsuranceTiersByType_WithValidType_ShouldReturnTiers()
        {
            // Arrange
            var insuranceType = "Health";
            var tiers = new List<InsuranceTier>
            {
                new InsuranceTier { InsuranceTierId = 1, InsuranceType = insuranceType },
                new InsuranceTier { InsuranceTierId = 2, InsuranceType = insuranceType }
            };

            _unitOfWorkMock.Setup(x => x.InsuranceTierRepository.GetByTypeAsync(insuranceType))
                .ReturnsAsync(tiers);
            _mapperMock.Setup(x => x.Map<IEnumerable<InsuranceTierDto>>(It.IsAny<IEnumerable<InsuranceTier>>()))
                .Returns(new List<InsuranceTierDto>
                {
                    new InsuranceTierDto { InsuranceTierId = 1 },
                    new InsuranceTierDto { InsuranceTierId = 2 }
                });

            // Act
            var result = await _service.GetInsuranceTiersByTypeAsync(insuranceType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        #endregion

        #region Calculation Tests

        [Fact]
        public async Task CalculateInsuranceContribution_WithValidData_ShouldSucceed()
        {
            // Arrange
            var employeeId = 1;
            var insuranceType = "Health";
            var salary = 10000;

            var employee = new Employee { EmployeeId = employeeId };
            var participation = new InsuranceParticipation
            {
                InsuranceParticipationId = 1,
                EmployeeId = employeeId,
                InsuranceType = insuranceType,
                Status = "Active"
            };
            var tier = new InsuranceTier
            {
                InsuranceTierId = 1,
                MinSalary = 0,
                MaxSalary = 15000,
                EmployeeRate = 2,
                EmployerRate = 3
            };

            _unitOfWorkMock.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.GetActiveInsuranceByEmployeeAndTypeAsync(employeeId, insuranceType))
                .ReturnsAsync(participation);
            _unitOfWorkMock.Setup(x => x.InsuranceTierRepository.GetTierForSalaryAsync(salary, insuranceType, It.IsAny<DateTime>()))
                .ReturnsAsync(tier);

            // Act
            var result = await _service.CalculateInsuranceContributionAsync(employeeId, insuranceType, salary);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            Assert.Equal(200, result.EmployeeContributionAmount);  // 10000 * 2%
            Assert.Equal(300, result.EmployerContributionAmount);  // 10000 * 3%
            Assert.Equal(500, result.TotalContributionAmount);     // 200 + 300
        }

        [Fact]
        public async Task CalculateInsuranceContribution_WithInvalidEmployee_ShouldThrowNotFoundException()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.EmployeeRepository.GetByIdAsync(999))
                .ReturnsAsync((Employee)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.CalculateInsuranceContributionAsync(999, "Health", 10000));
        }

        [Fact]
        public async Task CalculateTotalInsuranceContribution_WithMultipleInsurances_ShouldSucceed()
        {
            // Arrange
            var employeeId = 1;
            var totalSalary = 10000;

            var employee = new Employee { EmployeeId = employeeId };
            var insurances = new List<InsuranceParticipation>
            {
                new InsuranceParticipation { InsuranceParticipationId = 1, InsuranceType = "Health", Status = "Active" },
                new InsuranceParticipation { InsuranceParticipationId = 2, InsuranceType = "Unemployment", Status = "Active" }
            };
            var healthTier = new InsuranceTier { EmployeeRate = 2, EmployerRate = 3 };
            var unemploymentTier = new InsuranceTier { EmployeeRate = 0.5m, EmployerRate = 0.5m };

            _unitOfWorkMock.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _unitOfWorkMock.Setup(x => x.InsuranceParticipationRepository.GetByEmployeeIdAsync(employeeId))
                .ReturnsAsync(insurances);
            _unitOfWorkMock.Setup(x => x.InsuranceTierRepository.GetTierForSalaryAsync(totalSalary, "Health", It.IsAny<DateTime>()))
                .ReturnsAsync(healthTier);
            _unitOfWorkMock.Setup(x => x.InsuranceTierRepository.GetTierForSalaryAsync(totalSalary, "Unemployment", It.IsAny<DateTime>()))
                .ReturnsAsync(unemploymentTier);

            // Act
            var result = await _service.CalculateTotalInsuranceContributionAsync(employeeId, totalSalary);

            // Assert
            Assert.NotNull(result);
            // Health: (2+3)% = 5% of 10000 = 500
            // Unemployment: (0.5+0.5)% = 1% of 10000 = 100
            // Total: 600
            Assert.Equal(600, result);
        }

        #endregion
    }
}
