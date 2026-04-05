using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Application.Services;
using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Tests.Services
{
    /// <summary>
    /// Unit tests for LeaveManagementService
    /// Tests cover all major functionality including validation, authorization, and error handling
    /// </summary>
    public class LeaveManagementServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILeaveRequestRepository> _leaveRequestRepositoryMock;
        private readonly Mock<ILeaveBalanceRepository> _leaveBalanceRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<LeaveManagementService>> _loggerMock;
        private readonly ILeaveManagementService _service;

        public LeaveManagementServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _leaveRequestRepositoryMock = new Mock<ILeaveRequestRepository>();
            _leaveBalanceRepositoryMock = new Mock<ILeaveBalanceRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<LeaveManagementService>>();

            // Setup UnitOfWork mocks
            _unitOfWorkMock.Setup(x => x.LeaveRequestRepository).Returns(_leaveRequestRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.LeaveBalanceRepository).Returns(_leaveBalanceRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.EmployeeRepository).Returns(_employeeRepositoryMock.Object);

            _service = new LeaveManagementService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        #region SubmitLeaveRequest Tests

        [Fact]
        public async Task SubmitLeaveRequestAsync_WithValidData_ShouldSucceed()
        {
            // Arrange
            var employeeId = 1;
            var dto = new CreateLeaveRequestDto
            {
                EmployeeId = employeeId,
                LeaveType = "Annual",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(10),
                NumberOfDays = 5,
                Reason = "Vacation",
                EmergencyContact = "1234567890"
            };

            var employee = new Employee { EmployeeId = employeeId, FullName = "John Doe" };
            var leaveRequest = new LeaveRequest
            {
                LeaveRequestId = 1,
                EmployeeId = employeeId,
                LeaveType = dto.LeaveType,
                StartDate = DateOnly.FromDateTime(dto.StartDate),
                EndDate = DateOnly.FromDateTime(dto.EndDate),
                NumberOfDays = dto.NumberOfDays,
                Reason = dto.Reason,
                ApprovalStatus = "Pending"
            };

            var resultDto = new LeaveRequestDto
            {
                LeaveRequestId = 1,
                EmployeeId = employeeId,
                LeaveType = dto.LeaveType,
                ApprovalStatus = "Pending"
            };

            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Setup leave balance with sufficient days  
            var leaveBalance = new LeaveBalance 
            { 
                LeaveBalanceId = 1,
                EmployeeId = employeeId, 
                Year = DateTime.UtcNow.Year,
                LeaveType = dto.LeaveType,
                AllocatedDays = 20, 
                UsedDays = 0, 
                RemainingDays = 20
            };

            // Mock GetAllByEmployeeAsync which is called by CalculateRemainingLeaveDaysAsync
            _leaveBalanceRepositoryMock.Setup(x => x.GetAllByEmployeeAsync(employeeId))
                .ReturnsAsync(new List<LeaveBalance> { leaveBalance });

            _leaveRequestRepositoryMock.Setup(x => x.AddAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
                .Returns(resultDto);

            // Act
            var result = await _service.SubmitLeaveRequestAsync(dto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            Assert.Equal("Pending", result.ApprovalStatus);
            _leaveRequestRepositoryMock.Verify(x => x.AddAsync(It.IsAny<LeaveRequest>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SubmitLeaveRequestAsync_WithInvalidEmployee_ShouldThrow()
        {
            // Arrange
            var dto = new CreateLeaveRequestDto
            {
                EmployeeId = 999,
                LeaveType = "Annual",
                StartDate = DateTime.UtcNow.AddDays(5),
                EndDate = DateTime.UtcNow.AddDays(10),
                NumberOfDays = 5,
                Reason = "Vacation"
            };

            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<ERP.HRM.Domain.Exceptions.NotFoundException>(
                () => _service.SubmitLeaveRequestAsync(dto, CancellationToken.None));
        }

        [Fact]
        public async Task SubmitLeaveRequestAsync_WithInvalidDates_ShouldThrow()
        {
            // Arrange
            var employeeId = 1;
            var dto = new CreateLeaveRequestDto
            {
                EmployeeId = employeeId,
                LeaveType = "Annual",
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(5), // EndDate before StartDate
                NumberOfDays = 5,
                Reason = "Vacation"
            };

            var employee = new Employee { EmployeeId = employeeId, FullName = "John Doe" };
            _employeeRepositoryMock.Setup(x => x.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);

            // Act & Assert
            await Assert.ThrowsAsync<ERP.HRM.Domain.Exceptions.BusinessRuleException>(
                () => _service.SubmitLeaveRequestAsync(dto, CancellationToken.None));
        }

        #endregion

        #region ApproveLeaveRequest Tests

        [Fact]
        public async Task ApproveLeaveRequestAsync_WithValidId_ShouldSucceed()
        {
            // Arrange
            var leaveRequestId = 1;
            var approverNotes = "Approved";

            var leaveRequest = new LeaveRequest
            {
                LeaveRequestId = leaveRequestId,
                EmployeeId = 1,
                ApprovalStatus = "Pending",
                LeaveType = "Annual"
            };

            var resultDto = new LeaveRequestDto
            {
                LeaveRequestId = leaveRequestId,
                ApprovalStatus = "Approved"
            };

            _leaveRequestRepositoryMock.Setup(x => x.GetByIdAsync(leaveRequestId))
                .ReturnsAsync(leaveRequest);
            _leaveRequestRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
                .Returns(resultDto);

            // Act
            var result = await _service.ApproveLeaveRequestAsync(leaveRequestId, approverNotes, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Approved", result.ApprovalStatus);
            _leaveRequestRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<LeaveRequest>()), Times.Once);
        }

        [Fact]
        public async Task ApproveLeaveRequestAsync_WithInvalidId_ShouldThrow()
        {
            // Arrange
            _leaveRequestRepositoryMock.Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((LeaveRequest)null);

            // Act & Assert
            await Assert.ThrowsAsync<ERP.HRM.Domain.Exceptions.NotFoundException>(
                () => _service.ApproveLeaveRequestAsync(999, "Notes", CancellationToken.None));
        }

        #endregion

        #region RejectLeaveRequest Tests

        [Fact]
        public async Task RejectLeaveRequestAsync_WithValidId_ShouldSucceed()
        {
            // Arrange
            var leaveRequestId = 1;
            var rejectionReason = "Insufficient balance";

            var leaveRequest = new LeaveRequest
            {
                LeaveRequestId = leaveRequestId,
                ApprovalStatus = "Pending"
            };

            var resultDto = new LeaveRequestDto
            {
                LeaveRequestId = leaveRequestId,
                ApprovalStatus = "Rejected"
            };

            _leaveRequestRepositoryMock.Setup(x => x.GetByIdAsync(leaveRequestId))
                .ReturnsAsync(leaveRequest);
            _leaveRequestRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
                .Returns(resultDto);

            // Act
            var result = await _service.RejectLeaveRequestAsync(leaveRequestId, rejectionReason, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Rejected", result.ApprovalStatus);
        }

        #endregion

        #region CancelLeaveRequest Tests

        [Fact]
        public async Task CancelLeaveRequestAsync_WithValidId_ShouldSucceed()
        {
            // Arrange
            var leaveRequestId = 1;
            var cancelReason = "Personal reasons";

            var leaveRequest = new LeaveRequest
            {
                LeaveRequestId = leaveRequestId,
                ApprovalStatus = "Approved"
            };

            var resultDto = new LeaveRequestDto
            {
                LeaveRequestId = leaveRequestId,
                ApprovalStatus = "Cancelled"
            };

            _leaveRequestRepositoryMock.Setup(x => x.GetByIdAsync(leaveRequestId))
                .ReturnsAsync(leaveRequest);
            _leaveRequestRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<LeaveRequest>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(x => x.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
                .Returns(resultDto);

            // Act
            var result = await _service.CancelLeaveRequestAsync(leaveRequestId, cancelReason, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cancelled", result.ApprovalStatus);
        }

        #endregion

        #region Retrieval Tests

        [Fact]
        public async Task GetLeaveRequestAsync_WithValidId_ShouldReturnDto()
        {
            // Arrange
            var leaveRequestId = 1;
            var leaveRequest = new LeaveRequest { LeaveRequestId = leaveRequestId };
            var resultDto = new LeaveRequestDto { LeaveRequestId = leaveRequestId };

            _leaveRequestRepositoryMock.Setup(x => x.GetByIdAsync(leaveRequestId))
                .ReturnsAsync(leaveRequest);
            _mapperMock.Setup(x => x.Map<LeaveRequestDto>(It.IsAny<LeaveRequest>()))
                .Returns(resultDto);

            // Act
            var result = await _service.GetLeaveRequestAsync(leaveRequestId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(leaveRequestId, result.LeaveRequestId);
        }

        [Fact]
        public async Task GetEmployeeLeaveRequestsAsync_WithValidId_ShouldReturnList()
        {
            // Arrange
            var employeeId = 1;
            var leaveRequests = new List<LeaveRequest>
            {
                new LeaveRequest { LeaveRequestId = 1, EmployeeId = employeeId },
                new LeaveRequest { LeaveRequestId = 2, EmployeeId = employeeId }
            };

            var resultDtos = new PagedResult<LeaveRequestDto>
            {
                Items = new List<LeaveRequestDto>
                {
                    new LeaveRequestDto { LeaveRequestId = 1, EmployeeId = employeeId },
                    new LeaveRequestDto { LeaveRequestId = 2, EmployeeId = employeeId }
                },
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            };

            _leaveRequestRepositoryMock.Setup(x => x.GetByEmployeeIdAsync(employeeId))
                .ReturnsAsync(leaveRequests);
            _mapperMock.Setup(x => x.Map<IEnumerable<LeaveRequestDto>>(It.IsAny<IEnumerable<LeaveRequest>>()))
                .Returns(resultDtos.Items);

            // Act
            var result = await _service.GetEmployeeLeaveRequestsAsync(employeeId, 1, 10, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
        }

        #endregion
    }
}
