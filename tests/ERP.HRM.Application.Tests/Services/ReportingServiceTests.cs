using Xunit;
using Moq;
using ERP.HRM.Application.Services;
using ERP.HRM.Application.DTOs.Reporting;
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
    public class ReportingServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<ReportingService>> _mockLogger;
        private readonly ReportingService _reportingService;

        public ReportingServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<ReportingService>>();
            _reportingService = new ReportingService(_mockUnitOfWork.Object, _mockLogger.Object);
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

            var employee1 = new Employee
            {
                EmployeeId = 1,
                FullName = "Nguyen Van A",
                Status = "Active",
                DepartmentId = 1
            };

            var employee2 = new Employee
            {
                EmployeeId = 2,
                FullName = "Tran Thi B",
                Status = "Active",
                DepartmentId = 1
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
                    Employee = employee1,
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
                    Employee = employee2,
                    PayrollPeriod = payrollPeriod
                }
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

        private void SetupPayrollRecordsRepository(List<PayrollRecord> records)
        {
            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(records.AsEnumerable());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);
        }

        private void SetupLeaveRequestRepository(List<LeaveRequest> records)
        {
            var mockRepository = new Mock<ILeaveRequestRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(records.AsEnumerable());

            _mockUnitOfWork.Setup(u => u.LeaveRequestRepository).Returns(mockRepository.Object);
        }

        private void SetupEmployeeRepository(List<Employee> records)
        {
            var mockRepository = new Mock<IEmployeeRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(records.AsEnumerable());

            _mockUnitOfWork.Setup(u => u.EmployeeRepository).Returns(mockRepository.Object);
        }

        #endregion

        #region GetMonthlyPayrollSummaryAsync Tests

        [Fact]
        public async Task GetMonthlyPayrollSummaryAsync_WithValidData_ReturnsSummary()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetMonthlyPayrollSummaryAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.TotalEmployees);
            Assert.True(result.TotalGrossSalary > 0);
            Assert.True(result.TotalNetSalary > 0);
            Assert.True(result.AverageSalary > 0);
        }

        [Fact]
        public async Task GetMonthlyPayrollSummaryAsync_WithNoData_ReturnsEmptySummary()
        {
            // Arrange
            SetupPayrollRecordsRepository(new List<PayrollRecord>());

            // Act
            var result = await _reportingService.GetMonthlyPayrollSummaryAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(0, result.TotalEmployees);
            Assert.Equal(0, result.TotalGrossSalary);
        }

        #endregion

        #region GetTaxSummaryReportAsync Tests

        [Fact]
        public async Task GetTaxSummaryReportAsync_WithValidData_ReturnsTaxSummary()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetTaxSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.TotalEmployees);
            Assert.True(result.TotalTaxDeducted > 0);
            Assert.True(result.AverageTaxPerEmployee > 0);
        }

        [Fact]
        public async Task GetTaxSummaryReportAsync_WithValidData_CalculatesTaxCorrectly()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetTaxSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            var expectedTotalTax = payrollRecords.Sum(p => p.TaxDeduction);
            Assert.Equal(expectedTotalTax, result.TotalTaxDeducted);
            var expectedTaxableIncome = payrollRecords.Sum(p => p.GrossSalary - p.InsuranceDeduction);
            Assert.Equal(expectedTaxableIncome, result.TotalTaxableIncome);
        }

        #endregion

        #region GetInsuranceSummaryReportAsync Tests

        [Fact]
        public async Task GetInsuranceSummaryReportAsync_WithValidData_ReturnsInsuranceSummary()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetInsuranceSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.TotalEmployees);
            Assert.True(result.TotalInsuranceDeduction > 0);
        }

        [Fact]
        public async Task GetInsuranceSummaryReportAsync_WithValidData_CalculatesInsuranceCorrectly()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetInsuranceSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            var expectedTotalInsurance = payrollRecords.Sum(p => p.InsuranceDeduction);
            Assert.Equal(expectedTotalInsurance, result.TotalInsuranceDeduction);
            var expectedAverageInsurance = payrollRecords.Average(p => p.InsuranceDeduction);
            Assert.Equal(expectedAverageInsurance, result.AverageInsurancePerEmployee);
        }

        #endregion

        #region GetLeaveSummaryReportAsync Tests

        [Fact]
        public async Task GetLeaveSummaryReportAsync_WithValidData_ReturnsLeaveSummary()
        {
            // Arrange
            var leaveRequests = CreateSampleLeaveRequests();
            SetupLeaveRequestRepository(leaveRequests);

            // Act
            var result = await _reportingService.GetLeaveSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(2, result.ApprovedLeaveRequests);
            Assert.Equal(1, result.PendingLeaveRequests);
            Assert.Equal(0, result.RejectedLeaveRequests);
        }

        [Fact]
        public async Task GetLeaveSummaryReportAsync_WithNoData_ReturnsEmptyLeaveSummary()
        {
            // Arrange
            SetupLeaveRequestRepository(new List<LeaveRequest>());

            // Act
            var result = await _reportingService.GetLeaveSummaryReportAsync(2024, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(1, result.Month);
            Assert.Equal(0, result.ApprovedLeaveRequests);
            Assert.Equal(0, result.PendingLeaveRequests);
            Assert.Equal(0, result.RejectedLeaveRequests);
        }

        #endregion

        #region GetHRMetricsAsync Tests

        [Fact]
        public async Task GetHRMetricsAsync_WithValidData_ReturnsHRMetrics()
        {
            // Arrange
            var employees = CreateSampleEmployees();
            var payrollRecords = CreateSamplePayrollRecords();
            
            SetupEmployeeRepository(employees);
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetHRMetricsAsync(2024);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2024, result.Year);
            Assert.Equal(3, result.TotalEmployees);
            Assert.Equal(2, result.ActiveEmployees);
            Assert.Equal(1, result.InactiveEmployees);
            Assert.True(result.AverageSalary > 0);
            Assert.True(result.TotalPayrollCost > 0);
        }

        [Fact]
        public async Task GetHRMetricsAsync_WithValidData_CalculatesMetricsCorrectly()
        {
            // Arrange
            var employees = CreateSampleEmployees();
            var payrollRecords = CreateSamplePayrollRecords();
            
            SetupEmployeeRepository(employees);
            SetupPayrollRecordsRepository(payrollRecords);

            // Act
            var result = await _reportingService.GetHRMetricsAsync(2024);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.TotalEmployees);
            
            var yearPayroll = payrollRecords.Where(p => p.PayrollPeriod.Year == 2024).ToList();
            var expectedAvgSalary = yearPayroll.Average(p => p.NetSalary);
            var expectedTotalPayroll = yearPayroll.Sum(p => p.GrossSalary);
            var expectedCostPerEmployee = expectedTotalPayroll / 2; // 2 active employees
            
            Assert.Equal(expectedAvgSalary, result.AverageSalary);
            Assert.Equal(expectedTotalPayroll, result.TotalPayrollCost);
            Assert.Equal(expectedCostPerEmployee, result.CostPerEmployee);
        }

        #endregion
    }
}
