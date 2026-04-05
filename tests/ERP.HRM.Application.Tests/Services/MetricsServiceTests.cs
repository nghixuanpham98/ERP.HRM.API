using Xunit;
using Moq;
using ERP.HRM.Application.Services;
using ERP.HRM.Application.DTOs.Metrics;
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
    public class MetricsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<MetricsService>> _mockLogger;
        private readonly MetricsService _metricsService;

        public MetricsServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<MetricsService>>();
            _metricsService = new MetricsService(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        #region Helper Methods

        private List<PayrollRecord> CreateSamplePayrollRecords()
        {
            var currentDate = DateTime.Now;
            var payrollPeriod = new PayrollPeriod
            {
                PayrollPeriodId = 1,
                Year = currentDate.Year,
                Month = currentDate.Month,
                StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
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
                },
                new PayrollRecord
                {
                    PayrollRecordId = 3,
                    EmployeeId = 3,
                    PayrollPeriodId = 1,
                    BaseSalary = 10000000m,
                    Allowance = 1000000m,
                    GrossSalary = 11000000m,
                    InsuranceDeduction = 990000m,
                    TaxDeduction = 1100000m,
                    OtherDeductions = 100000m,
                    TotalDeductions = 2190000m,
                    NetSalary = 8810000m,
                    PayrollPeriod = payrollPeriod
                }
            };
        }

        private List<Employee> CreateSampleEmployees()
        {
            var department1 = new Department { DepartmentId = 1, DepartmentName = "IT" };
            var department2 = new Department { DepartmentId = 2, DepartmentName = "HR" };

            return new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FullName = "Nguyen Van A",
                    Status = "Active",
                    DepartmentId = 1,
                    Department = department1
                },
                new Employee
                {
                    EmployeeId = 2,
                    FullName = "Tran Thi B",
                    Status = "Active",
                    DepartmentId = 1,
                    Department = department1
                },
                new Employee
                {
                    EmployeeId = 3,
                    FullName = "Le Van C",
                    Status = "Inactive",
                    DepartmentId = 2,
                    Department = department2
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
                    StartDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 15),
                    EndDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 17),
                    ApprovalStatus = "Approved"
                },
                new LeaveRequest
                {
                    LeaveRequestId = 2,
                    EmployeeId = 2,
                    StartDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 10),
                    EndDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 12),
                    ApprovalStatus = "Approved"
                },
                new LeaveRequest
                {
                    LeaveRequestId = 3,
                    EmployeeId = 3,
                    StartDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 20),
                    EndDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, 21),
                    ApprovalStatus = "Pending"
                }
            };
        }

        private List<PayrollPeriod> CreateSamplePayrollPeriods()
        {
            var currentDate = DateTime.Now;
            return new List<PayrollPeriod>
            {
                new PayrollPeriod
                {
                    PayrollPeriodId = 1,
                    Year = currentDate.Year,
                    Month = currentDate.Month,
                    StartDate = new DateTime(currentDate.Year, currentDate.Month, 1),
                    EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
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
            departmentRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => departments.FirstOrDefault(d => d.DepartmentId == id));
            _mockUnitOfWork.Setup(u => u.DepartmentRepository).Returns(departmentRepository.Object);

            var leaveRepository = new Mock<ILeaveRequestRepository>();
            leaveRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(leaveRequests.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.LeaveRequestRepository).Returns(leaveRepository.Object);

            var periodRepository = new Mock<IPayrollPeriodRepository>();
            periodRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(periods.AsEnumerable());
            _mockUnitOfWork.Setup(u => u.PayrollPeriodRepository).Returns(periodRepository.Object);
        }

        #endregion

        #region GetDepartmentMetricsAsync Tests

        [Fact]
        public async Task GetDepartmentMetricsAsync_WithValidData_ReturnsMetrics()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentMetricsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DepartmentId);
            Assert.Equal("IT", result.DepartmentName);
            Assert.Equal(2, result.TotalEmployees);
            Assert.Equal(2, result.ActiveEmployees);
        }

        [Fact]
        public async Task GetDepartmentMetricsAsync_CalculatesCostCorrectly()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentMetricsAsync(1);

            // Assert
            Assert.NotNull(result);
            var deptPayroll = payrollRecords.Where(p => p.EmployeeId <= 2).ToList();
            var expectedTotalCost = deptPayroll.Sum(p => p.GrossSalary);
            Assert.Equal(expectedTotalCost, result.TotalMonthlyCost);
        }

        #endregion

        #region GetDepartmentComparisonAsync Tests

        [Fact]
        public async Task GetDepartmentComparisonAsync_WithValidData_ReturnsComparison()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentComparisonAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Departments.Count);
            Assert.NotNull(result.BestPerformingDepartment);
            Assert.NotNull(result.HighestCostDepartment);
        }

        [Fact]
        public async Task GetDepartmentComparisonAsync_IdentifiesHighestCostDepartment()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentComparisonAsync();

            // Assert
            Assert.NotNull(result.HighestCostDepartment);
            Assert.Equal("IT", result.HighestCostDepartment.DepartmentName);
        }

        #endregion

        #region GetEmployeeDistributionAsync Tests

        [Fact]
        public async Task GetEmployeeDistributionAsync_WithValidData_ReturnsDistribution()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetEmployeeDistributionAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            var totalPercentage = result.Sum(d => d.Percentage);
            Assert.True(Math.Abs((double)totalPercentage - 100) < 0.01);
        }

        #endregion

        #region GetDepartmentPayrollTrendAsync Tests

        [Fact]
        public async Task GetDepartmentPayrollTrendAsync_WithValidData_ReturnsTrend()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentPayrollTrendAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.DepartmentId);
            Assert.True(result.TotalPayroll > 0);
            Assert.True(result.ProcessedEmployees > 0);
        }

        #endregion

        #region GetDepartmentAnalyticsAsync Tests

        [Fact]
        public async Task GetDepartmentAnalyticsAsync_WithValidData_ReturnsAnalytics()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentAnalyticsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Metrics);
            Assert.NotNull(result.RecentPayrollTrend);
            Assert.NotNull(result.LeaveAnalysis);
            Assert.NotNull(result.CostBreakdown);
            Assert.NotNull(result.EmployeeComparison);
        }

        [Fact]
        public async Task GetDepartmentAnalyticsAsync_LeaveAnalysisCorrect()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var employees = CreateSampleEmployees();
            var departments = CreateSampleDepartments();
            var leaveRequests = CreateSampleLeaveRequests();
            var periods = CreateSamplePayrollPeriods();

            SetupRepositoryMocks(payrollRecords, employees, departments, leaveRequests, periods);

            // Act
            var result = await _metricsService.GetDepartmentAnalyticsAsync(1);

            // Assert
            Assert.NotNull(result.LeaveAnalysis);
            Assert.Equal(2, result.LeaveAnalysis.TotalLeaveRequests); // 2 employees in IT dept with leaves
            Assert.Equal(2, result.LeaveAnalysis.ApprovedLeaves);
        }

        #endregion
    }
}
