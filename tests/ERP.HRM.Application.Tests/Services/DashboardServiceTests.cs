using Xunit;
using Moq;
using ERP.HRM.Application.Services;
using ERP.HRM.Application.DTOs.Dashboard;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Tests.Services
{
    public class DashboardServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DashboardService>> _mockLogger;
        private readonly DashboardService _dashboardService;

        public DashboardServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DashboardService>>();
            _dashboardService = new DashboardService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        #region Helper Methods

        private List<PayrollRecord> CreateSamplePayrollRecords()
        {
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = 2024,
                Month = 1,
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 1, 31)
            };

            return new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    OvertimeCompensation = 500000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    OtherDeductions = 200000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                },
                new PayrollRecord
                {
                    PayrollRecordId = 2,
                    EmployeeId = 2,
                    PayrollPeriodId = 1,
                    BaseSalary = 12000000m,
                    Allowance = 1500000m,
                    OvertimeCompensation = 300000m,
                    GrossSalary = 13800000m,
                    InsuranceDeduction = 1242000m,
                    TaxDeduction = 1380000m,
                    OtherDeductions = 150000m,
                    TotalDeductions = 2772000m,
                    NetSalary = 11028000m,
                    PayrollPeriod = payrollPeriod
                }
            };
        }

        private List<Employee> CreateSampleEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FullName = "Nguyen Van A",
                    Status = "Active",
                    DepartmentId = 1
                },
                new Employee
                {
                    EmployeeId = 2,
                    FullName = "Tran Thi B",
                    Status = "Active",
                    DepartmentId = 1
                },
                new Employee
                {
                    EmployeeId = 3,
                    FullName = "Le Van C",
                    Status = "Inactive",
                    DepartmentId = 2
                }
            };
        }

        private List<Department> CreateSampleDepartments()
        {
            return new List<Department>
            {
                new Department { DepartmentId = 1, DepartmentName = "IT" },
                new Department { DepartmentId = 2, DepartmentName = "HR" }
            };
        }

        private List<LeaveRequest> CreateSampleLeaveRequests()
        {
            return new List<LeaveRequest>
            {
                new LeaveRequest
                {
                    LeaveRequestId = 1,
                    EmployeeId = 1,
                    StartDate = new DateOnly(2024, 1, 15),
                    EndDate = new DateOnly(2024, 1, 17),
                    ApprovalStatus = "Approved"
                },
                new LeaveRequest
                {
                    LeaveRequestId = 2,
                    EmployeeId = 2,
                    StartDate = new DateOnly(2024, 1, 10),
                    EndDate = new DateOnly(2024, 1, 12),
                    ApprovalStatus = "Approved"
                },
                new LeaveRequest
                {
                    LeaveRequestId = 3,
                    EmployeeId = 1,
                    StartDate = new DateOnly(2024, 1, 20),
                    EndDate = new DateOnly(2024, 1, 21),
                    ApprovalStatus = "Pending"
                }
            };
        }

        private List<PayrollPeriod> CreateSamplePayrollPeriods()
        {
            return new List<PayrollPeriod>
            {
                new PayrollPeriod
                {
                    PayrollPeriodId = 1,
                    Year = 2024,
                    Month = 1,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 1, 31)
                }
            };
        }

        private void SetupRepositoryMocks(List<PayrollRecord> payrollRecords, List<Employee> employees, 
            List<Department> departments, List<LeaveRequest> leaveRequests, List<PayrollPeriod> periods)
        {
            var payrollRepository = new Mock<IPayrollRecordRepository>();
            payrollRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(payrollRecords.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(payrollRepository.Object);

            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(employees.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.EmployeeRepository).Returns(employeeRepository.Object);

            var departmentRepository = new Mock<IDepartmentRepository>();
            departmentRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(departments.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.DepartmentRepository).Returns(departmentRepository.Object);

            var leaveRepository = new Mock<ILeaveRequestRepository>();
            leaveRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.LeaveRequestRepository).Returns(leaveRepository.Object);

            var periodRepository = new Mock<IPayrollPeriodRepository>();
            periodRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(periods.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.PayrollPeriodRepository).Returns(periodRepository.Object);
        }

        #endregion

        #region GetDashboardSummaryAsync Tests

        [Fact]
        public async Task GetDashboardSummaryAsync_WithValidData_ReturnsSummary()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
            };

            var payrollRecords = new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    OvertimeCompensation = 500000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    OtherDeductions = 200000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                },
                new PayrollRecord
                {
                    PayrollRecordId = 2,
                    EmployeeId = 2,
                    PayrollPeriodId = 1,
                    BaseSalary = 12000000m,
                    Allowance = 1500000m,
                    OvertimeCompensation = 300000m,
                    GrossSalary = 13800000m,
                    InsuranceDeduction = 1242000m,
                    TaxDeduction = 1380000m,
                    OtherDeductions = 150000m,
                    TotalDeductions = 2772000m,
                    NetSalary = 11028000m,
                    PayrollPeriod = payrollPeriod
                }
            };

            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod> { payrollPeriod };

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetDashboardSummaryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalEmployees);
            Assert.Equal(2, result.ActiveEmployees);
            Assert.True(result.AverageMonthlySalary > 0);
            Assert.True(result.TotalMonthlyPayroll > 0);
            Assert.Equal(1, result.PendingLeaveRequests);
        }

        [Fact]
        public async Task GetDashboardSummaryAsync_WithNoPayrollData_CalculatesCorrectly()
        {
            // Arrange
            var payrollRecords = new List<PayrollRecord>();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetDashboardSummaryAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalEmployees);
            Assert.Equal(0, result.AverageMonthlySalary);
            Assert.Equal(0, result.TotalMonthlyPayroll);
        }

        #endregion

        #region GetRecentPayrollAsync Tests

        [Fact]
        public async Task GetRecentPayrollAsync_WithValidData_ReturnsRecentPayroll()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetRecentPayrollAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.ProcessedCount);
            Assert.True(result.TotalGrossSalary > 0);
            Assert.True(result.TotalNetSalary > 0);
        }

        [Fact]
        public async Task GetRecentPayrollAsync_WithNoPeriods_ReturnsEmptyPayroll()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod>();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetRecentPayrollAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ProcessedCount);
        }

        #endregion

        #region GetDepartmentBreakdownAsync Tests

        [Fact]
        public async Task GetDepartmentBreakdownAsync_WithValidData_ReturnsDepartmentBreakdown()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetDepartmentBreakdownAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            
            var itDept = result.First(d => d.DepartmentId == 1);
            Assert.Equal("IT", itDept.DepartmentName);
            Assert.Equal(2, itDept.EmployeeCount);
            Assert.Equal(2, itDept.ActiveEmployees);
        }

        [Fact]
        public async Task GetDepartmentBreakdownAsync_WithNoDepartments_ReturnsEmptyList()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = new List<Department>();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetDepartmentBreakdownAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        #endregion

        #region GetSalaryDistributionAsync Tests

        [Fact]
        public async Task GetSalaryDistributionAsync_WithValidData_ReturnsSalaryDistribution()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
            };

            var payrollRecords = new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    OtherDeductions = 200000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                },
                new PayrollRecord
                {
                    PayrollRecordId = 2,
                    EmployeeId = 2,
                    PayrollPeriodId = 1,
                    BaseSalary = 12000000m,
                    Allowance = 1500000m,
                    GrossSalary = 13800000m,
                    InsuranceDeduction = 1242000m,
                    TaxDeduction = 1380000m,
                    OtherDeductions = 150000m,
                    TotalDeductions = 2772000m,
                    NetSalary = 11028000m,
                    PayrollPeriod = payrollPeriod
                }
            };

            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod> { payrollPeriod };

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetSalaryDistributionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            var totalPercentage = result.Sum(d => d.Percentage);
            Assert.True(Math.Abs((double)totalPercentage - 100) < 0.01);
        }

        [Fact]
        public async Task GetSalaryDistributionAsync_DistributionRangesAreCorrect()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
            };

            var payrollRecords = new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                },
                new PayrollRecord
                {
                    PayrollRecordId = 2,
                    EmployeeId = 2,
                    PayrollPeriodId = 1,
                    BaseSalary = 12000000m,
                    Allowance = 1500000m,
                    GrossSalary = 13800000m,
                    InsuranceDeduction = 1242000m,
                    TaxDeduction = 1380000m,
                    TotalDeductions = 2772000m,
                    NetSalary = 11028000m,
                    PayrollPeriod = payrollPeriod
                }
            };

            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod> { payrollPeriod };

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetSalaryDistributionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count); // 5 salary ranges

            var rangeLabels = result.Select(d => d.SalaryRange).ToList();
            Assert.Contains("< 5M", rangeLabels);
            Assert.Contains("5M - 10M", rangeLabels);
            Assert.Contains("10M - 15M", rangeLabels);
            Assert.Contains("15M - 20M", rangeLabels);
            Assert.Contains("> 20M", rangeLabels);
        }

        #endregion

        #region GetPayrollDashboardAsync Tests

        [Fact]
        public async Task GetPayrollDashboardAsync_WithValidData_ReturnsCompleteDashboard()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
            };

            var payrollRecords = new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                }
            };

            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod> { payrollPeriod };

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetPayrollDashboardAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Summary);
            Assert.NotNull(result.RecentPayroll);
            Assert.NotNull(result.DepartmentBreakdown);
            Assert.NotNull(result.LeaveStatistics);
            Assert.NotNull(result.SalaryDistribution);
            Assert.True(result.GeneratedDate > DateTime.Now.AddSeconds(-5));
        }

        [Fact]
        public async Task GetPayrollDashboardAsync_AllComponentsPopulated()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
            };

            var payrollRecords = new List<PayrollRecord>
            {
                new PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 15000000m,
                    Allowance = 2000000m,
                    GrossSalary = 17500000m,
                    InsuranceDeduction = 1575000m,
                    TaxDeduction = 1750000m,
                    TotalDeductions = 3525000m,
                    NetSalary = 13975000m,
                    PayrollPeriod = payrollPeriod
                }
            };

            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = new List<PayrollPeriod> { payrollPeriod };

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _dashboardService.GetPayrollDashboardAsync();

            // Assert
            Assert.Equal(3, result.Summary.TotalEmployees);
            Assert.Equal(2, result.DepartmentBreakdown.Count);
            Assert.Equal(3, result.LeaveStatistics.TotalLeaveRequests);
            Assert.NotEmpty(result.SalaryDistribution);
        }

        #endregion
    }
}
