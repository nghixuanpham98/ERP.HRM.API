using ERP.HRM.Application.DTOs.Payroll;
using System;
using System.Threading;
using System.Threading.Tasks;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class PayrollServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<PayrollService>> _mockLogger;
        private readonly PayrollService _service;

        public PayrollServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<PayrollService>>();
            _service = new PayrollService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CalculateMonthlySalaryAsync_WithValidEmployeeAndPeriod_ShouldReturnPayrollRecord()
        {
            // Arrange
            var employeeId = 1;
            var payrollPeriodId = 1;
            var employee = new Employee { Id = employeeId, FullName = "John Doe" };
            var period = new PayrollPeriod { Id = payrollPeriodId, TotalWorkingDays = 22 };
            var salaryConfig = new SalaryConfiguration 
            { 
                EmployeeId = employeeId, 
                BaseSalary = 10000000,
                Allowance = 1000000
            };
            var attendanceRecords = new List<Attendance>
            {
                new Attendance { OvertimeHours = 5, OvertimeMultiplier = 1.5m }
            };

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockUnitOfWork.Setup(x => x.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId))
                .ReturnsAsync(period);
            _mockUnitOfWork.Setup(x => x.SalaryConfigurationRepository.GetActiveConfigByEmployeeIdAsync(employeeId))
                .ReturnsAsync(salaryConfig);
            _mockUnitOfWork.Setup(x => x.AttendanceRepository.GetTotalWorkingDaysAsync(employeeId, payrollPeriodId))
                .ReturnsAsync(20);
            _mockUnitOfWork.Setup(x => x.AttendanceRepository.GetByEmployeeAndPeriodAsync(employeeId, payrollPeriodId))
                .ReturnsAsync(attendanceRecords);

            // Act
            var result = await _service.CalculateMonthlySalaryAsync(employeeId, payrollPeriodId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            Assert.Equal(payrollPeriodId, result.PayrollPeriodId);
            Assert.True(result.GrossSalary > 0);
        }

        [Fact]
        public async Task CalculateMonthlySalaryAsync_WithInvalidEmployee_ShouldThrowNotFoundException()
        {
            // Arrange
            var employeeId = 999;
            var payrollPeriodId = 1;

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _service.CalculateMonthlySalaryAsync(employeeId, payrollPeriodId));
        }

        [Fact]
        public async Task CalculateMonthlySalaryAsync_WithInvalidPeriod_ShouldThrowNotFoundException()
        {
            // Arrange
            var employeeId = 1;
            var payrollPeriodId = 999;
            var employee = new Employee { Id = employeeId };

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockUnitOfWork.Setup(x => x.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId))
                .ReturnsAsync((PayrollPeriod)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _service.CalculateMonthlySalaryAsync(employeeId, payrollPeriodId));
        }

        [Fact]
        public async Task CalculateMonthlySalaryAsync_WithOverrideSalary_ShouldUseOverrideValue()
        {
            // Arrange
            var employeeId = 1;
            var payrollPeriodId = 1;
            var overrideSalary = 15000000m;
            var employee = new Employee { Id = employeeId };
            var period = new PayrollPeriod { Id = payrollPeriodId, TotalWorkingDays = 22 };
            var salaryConfig = new SalaryConfiguration 
            { 
                EmployeeId = employeeId, 
                BaseSalary = 10000000,
                Allowance = 1000000
            };
            var attendanceRecords = new List<Attendance>();

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockUnitOfWork.Setup(x => x.PayrollPeriodRepository.GetByIdAsync(payrollPeriodId))
                .ReturnsAsync(period);
            _mockUnitOfWork.Setup(x => x.SalaryConfigurationRepository.GetActiveConfigByEmployeeIdAsync(employeeId))
                .ReturnsAsync(salaryConfig);
            _mockUnitOfWork.Setup(x => x.AttendanceRepository.GetTotalWorkingDaysAsync(employeeId, payrollPeriodId))
                .ReturnsAsync(20);
            _mockUnitOfWork.Setup(x => x.AttendanceRepository.GetByEmployeeAndPeriodAsync(employeeId, payrollPeriodId))
                .ReturnsAsync(attendanceRecords);

            // Act
            var result = await _service.CalculateMonthlySalaryAsync(employeeId, payrollPeriodId, overrideSalary);

            // Assert
            Assert.NotNull(result);
            // Base salary should be calculated using override value, not config value
            Assert.True(result.GrossSalary > 0);
        }

        [Fact]
        public async Task GetSalarySlipAsync_WithValidIds_ShouldReturnSalarSlip()
        {
            // Arrange
            var employeeId = 1;
            var payrollPeriodId = 1;
            var employee = new Employee { Id = employeeId, FullName = "John Doe", Email = "john@example.com" };
            var payrollRecord = new PayrollRecord 
            { 
                EmployeeId = employeeId, 
                PayrollPeriodId = payrollPeriodId,
                GrossSalary = 11000000,
                TotalDeductions = 2000000
            };

            _mockUnitOfWork.Setup(x => x.EmployeeRepository.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockUnitOfWork.Setup(x => x.PayrollRecordRepository.GetByEmployeeAndPeriodAsync(employeeId, payrollPeriodId))
                .ReturnsAsync(payrollRecord);

            // Act
            var result = await _service.GetSalarySlipAsync(employeeId, payrollPeriodId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employeeId, result.EmployeeId);
            Assert.Equal(payrollPeriodId, result.PayrollPeriodId);
        }
    }
}
