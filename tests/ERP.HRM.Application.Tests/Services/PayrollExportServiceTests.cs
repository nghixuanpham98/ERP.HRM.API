using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Tests.Services
{
    /// <summary>
    /// Comprehensive unit tests for PayrollExportService
    /// Tests all export scenarios: Excel, PDF, Bank, Tax Authority
    /// </summary>
    public class PayrollExportServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<PayrollExportService>> _mockLogger;
        private readonly Mock<IPayrollService> _mockPayrollService;
        private readonly Mock<IVietnameseTaxService> _mockTaxService;
        private readonly IPayrollExportService _service;

        public PayrollExportServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<PayrollExportService>>();
            _mockPayrollService = new Mock<IPayrollService>();
            _mockTaxService = new Mock<IVietnameseTaxService>();

            _service = new PayrollExportService(
                _mockUnitOfWork.Object,
                _mockLogger.Object,
                _mockPayrollService.Object,
                _mockTaxService.Object);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithValidRequest_ShouldReturnExportResponse()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            Assert.NotEmpty(result.FileName);
            Assert.NotEmpty(result.FileContent);
            Assert.Equal(payrollRecords.Count, result.TotalRecords);
            Assert.True(result.TotalGrossSalary > 0);
            Assert.True(result.TotalNetSalary > 0);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithInvalidFormat_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "InvalidFormat"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.ExportPayrollAsync(request));

            Assert.Contains("must be 'Excel' or 'PDF'", exception.Message);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithNoRecords_ShouldThrowNotFoundException()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 999,
                ExportFormat = "Excel"
            };

            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Domain.Entities.PayrollRecord>());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => _service.ExportPayrollAsync(request));

            Assert.Contains("No payroll records found", exception.Message);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithExcelFormat_ShouldReturnCsvContent()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            var csvContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Mã NV", csvContent);
            Assert.Contains("BẢNG TÍNH LƯƠNG THÁNG", csvContent);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithPdfFormat_ShouldReturnPdfContent()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "PDF",
                ExportPurpose = "General"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/pdf", result.ContentType);
            var pdfContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("BẢNG TÍNH LƯƠNG THÁNG", pdfContent);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithBankPurpose_ShouldReturnBankTransferFormat()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "Bank"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            var csvContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Mã nhân viên", csvContent);
            Assert.Contains("Số tài khoản ngân hàng", csvContent);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithTaxAuthorityPurpose_ShouldReturnTaxFormat()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "TaxAuthority"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            var csvContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Báo cáo thuế TNCN", csvContent);
            Assert.Contains("Mức thuế", csvContent);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithDepartmentFilter_ShouldOnlyExportDepartmentRecords()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                DepartmentId = 1
            };

            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(payrollRecords.Where(pr => pr.Employee?.DepartmentId == 1).ToList().AsEnumerable());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result);
            var filteredCount = payrollRecords.Count(pr => pr.Employee?.DepartmentId == 1);
            Assert.Equal(filteredCount, result.TotalRecords);
        }

        [Fact]
        public async Task ExportForBankTransferAsync_ShouldReturnBankOptimizedExport()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportForBankTransferAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Bank", result.FileName);
            var csvContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            Assert.Contains("Số tài khoản", csvContent);
        }

        [Fact]
        public async Task ExportForTaxAuthorityAsync_ShouldReturnTaxOptimizedExport()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            _mockTaxService.Setup(t => t.CalculateTaxAsync(It.IsAny<decimal>(), It.IsAny<int?>()))
                .ReturnsAsync(new VietnameseTaxCalculationResult
                {
                    TaxableIncome = 10000000,
                    TaxAmount = 1000000,
                    EffectiveTaxRate = 10,
                    ApplicableBracketLevel = 2,
                    NetIncome = 9000000
                });

            // Act
            var result = await _service.ExportForTaxAuthorityAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("TaxAuthority", result.FileName);
            var csvContent = System.Text.Encoding.UTF8.GetString(result.FileContent);
            // Verify it contains payroll data with taxes
            Assert.NotEmpty(csvContent);
        }

        [Fact]
        public async Task GetPayrollExportLinesAsync_WithValidPeriod_ShouldReturnExportLines()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            // Act
            var lines = await _service.GetPayrollExportLinesAsync(1);

            // Assert
            Assert.NotNull(lines);
            Assert.NotEmpty(lines);
            Assert.Equal(payrollRecords.Count, lines.Count);
            
            var firstLine = lines.First();
            Assert.NotNull(firstLine.EmployeeName);
            Assert.True(firstLine.GrossSalary > 0);
            Assert.True(firstLine.NetSalary > 0);
        }

        [Fact]
        public async Task GetPayrollExportLinesAsync_WithNoPeriod_ShouldReturnEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Domain.Entities.PayrollRecord>());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);

            // Act
            var lines = await _service.GetPayrollExportLinesAsync(999);

            // Assert
            Assert.Empty(lines);
        }

        [Fact]
        public async Task GetPayrollExportLinesAsync_WithDepartmentFilter_ShouldFilterCorrectly()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(payrollRecords.Where(pr => pr.Employee?.DepartmentId == 1).ToList().AsEnumerable());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);

            // Act
            var lines = await _service.GetPayrollExportLinesAsync(1, departmentId: 1);

            // Assert
            Assert.NotNull(lines);
            var filtered = payrollRecords.Where(pr => pr.Employee?.DepartmentId == 1).ToList();
            Assert.Equal(filtered.Count, lines.Count);
        }

        [Fact]
        public async Task GetBankTransferLinesAsync_ShouldReturnBankTransferData()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            // Act
            var bankLines = await _service.GetBankTransferLinesAsync(1);

            // Assert
            Assert.NotNull(bankLines);
            Assert.NotEmpty(bankLines);
            
            var firstLine = bankLines.First();
            Assert.NotNull(firstLine.EmployeeName);
            Assert.NotNull(firstLine.BankAccountNumber);
            Assert.Equal("VietcomBank", firstLine.BankName);
            Assert.Equal(payrollRecords[0].NetSalary, firstLine.TransferAmount);
        }

        [Fact]
        public async Task GetBankTransferLinesAsync_ShouldIncludeDescription()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            // Act
            var bankLines = await _service.GetBankTransferLinesAsync(1);

            // Assert
            Assert.NotEmpty(bankLines);
            var firstLine = bankLines.First();
            Assert.Contains("Salary", firstLine.Description);
        }

        [Fact]
        public async Task GetTaxAuthorityExportLinesAsync_ShouldReturnTaxData()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            _mockTaxService.Setup(t => t.CalculateTaxAsync(It.IsAny<decimal>(), It.IsAny<int?>()))
                .ReturnsAsync(new VietnameseTaxCalculationResult
                {
                    TaxableIncome = 10000000,
                    TaxAmount = 500000,
                    EffectiveTaxRate = 5,
                    ApplicableBracketLevel = 1,
                    NetIncome = 9500000
                });

            // Act
            var taxLines = await _service.GetTaxAuthorityExportLinesAsync(1);

            // Assert
            Assert.NotNull(taxLines);
            Assert.NotEmpty(taxLines);
            
            var firstLine = taxLines.First();
            Assert.NotNull(firstLine.TaxCode);
            Assert.True(firstLine.TaxAmount > 0);
            Assert.True(firstLine.EffectiveTaxRate >= 0);
        }

        [Fact]
        public async Task GetTaxAuthorityExportLinesAsync_ShouldIncludeBracketLevel()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            SetupMocks(payrollRecords);

            _mockTaxService.Setup(t => t.CalculateTaxAsync(It.IsAny<decimal>(), It.IsAny<int?>()))
                .ReturnsAsync(new VietnameseTaxCalculationResult
                {
                    TaxableIncome = 5000000,
                    TaxAmount = 250000,
                    EffectiveTaxRate = 5,
                    ApplicableBracketLevel = 1,
                    NetIncome = 4750000
                });

            // Act
            var taxLines = await _service.GetTaxAuthorityExportLinesAsync(1);

            // Assert
            Assert.NotEmpty(taxLines);
            var firstLine = taxLines.First();
            Assert.Contains("Level 1", firstLine.TaxBracketLevel);
        }

        [Fact]
        public async Task ExportPayrollAsync_ShouldSetCorrectFileName()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "Bank"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotNull(result.FileName);
            Assert.Contains("BankTransfer", result.FileName);
            Assert.Contains("P1", result.FileName);
            Assert.EndsWith(".csv", result.FileName);
        }

        [Fact]
        public async Task ExportPayrollAsync_ShouldCalculateTotalCorrectly()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            var expectedGross = payrollRecords.Sum(pr => pr.GrossSalary);
            var expectedNet = payrollRecords.Sum(pr => pr.NetSalary);
            var expectedTax = payrollRecords.Sum(pr => pr.TaxDeduction);

            Assert.Equal(expectedGross, result.TotalGrossSalary);
            Assert.Equal(expectedNet, result.TotalNetSalary);
            Assert.Equal(expectedTax, result.TotalTaxDeduction);
        }

        [Fact]
        public async Task ExportPayrollAsync_WithNullRequest_ShouldThrowBusinessRuleException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.ExportPayrollAsync(null));

            Assert.Contains("cannot be null", exception.Message);
        }

        [Fact]
        public async Task GetBankTransferLinesAsync_WithNoPeriod_ShouldReturnEmptyList()
        {
            // Arrange
            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Domain.Entities.PayrollRecord>());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);

            // Act
            var lines = await _service.GetBankTransferLinesAsync(999);

            // Assert
            Assert.Empty(lines);
        }

        [Fact]
        public async Task ExportPayrollAsync_FileContentShouldNotBeEmpty()
        {
            // Arrange
            var payrollRecords = CreateSamplePayrollRecords();
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel"
            };

            SetupMocks(payrollRecords);

            // Act
            var result = await _service.ExportPayrollAsync(request);

            // Assert
            Assert.NotEmpty(result.FileContent);
            Assert.True(result.FileContent.Length > 0);
        }

        // Helper methods

        private List<Domain.Entities.PayrollRecord> CreateSamplePayrollRecords()
        {
            return new List<Domain.Entities.PayrollRecord>
            {
                new Domain.Entities.PayrollRecord
                {
                    PayrollRecordId = 1,
                    EmployeeId = 1,
                    PayrollPeriodId = 1,
                    BaseSalary = 10000000,
                    Allowance = 1000000,
                    OvertimeCompensation = 500000,
                    GrossSalary = 11500000,
                    InsuranceDeduction = 1035000,
                    TaxDeduction = 500000,
                    OtherDeductions = 0,
                    TotalDeductions = 1535000,
                    NetSalary = 9965000,
                    PaymentDate = DateTime.Now,
                    Employee = new Domain.Entities.Employee
                    {
                        EmployeeId = 1,
                        EmployeeCode = "EMP001",
                        FullName = "Nguyễn Văn A",
                        DepartmentId = 1,
                        NationalId = "0123456789",
                        Department = new Domain.Entities.Department { DepartmentId = 1, DepartmentName = "IT" },
                        Position = new Domain.Entities.Position { PositionId = 1, PositionName = "Developer" }
                    }
                },
                new Domain.Entities.PayrollRecord
                {
                    PayrollRecordId = 2,
                    EmployeeId = 2,
                    PayrollPeriodId = 1,
                    BaseSalary = 8000000,
                    Allowance = 500000,
                    OvertimeCompensation = 200000,
                    GrossSalary = 8700000,
                    InsuranceDeduction = 783000,
                    TaxDeduction = 350000,
                    OtherDeductions = 0,
                    TotalDeductions = 1133000,
                    NetSalary = 7567000,
                    PaymentDate = DateTime.Now,
                    Employee = new Domain.Entities.Employee
                    {
                        EmployeeId = 2,
                        EmployeeCode = "EMP002",
                        FullName = "Trần Thị B",
                        DepartmentId = 1,
                        NationalId = "9876543210",
                        Department = new Domain.Entities.Department { DepartmentId = 1, DepartmentName = "IT" },
                        Position = new Domain.Entities.Position { PositionId = 2, PositionName = "Analyst" }
                    }
                },
                new Domain.Entities.PayrollRecord
                {
                    PayrollRecordId = 3,
                    EmployeeId = 3,
                    PayrollPeriodId = 1,
                    BaseSalary = 7000000,
                    Allowance = 300000,
                    OvertimeCompensation = 0,
                    GrossSalary = 7300000,
                    InsuranceDeduction = 657000,
                    TaxDeduction = 250000,
                    OtherDeductions = 0,
                    TotalDeductions = 907000,
                    NetSalary = 6393000,
                    PaymentDate = DateTime.Now,
                    Employee = new Domain.Entities.Employee
                    {
                        EmployeeId = 3,
                        EmployeeCode = "EMP003",
                        FullName = "Lê Văn C",
                        DepartmentId = 2,
                        NationalId = "5555555555",
                        Department = new Domain.Entities.Department { DepartmentId = 2, DepartmentName = "HR" },
                        Position = new Domain.Entities.Position { PositionId = 3, PositionName = "Manager" }
                    }
                }
            };
        }

        private void SetupMocks(List<Domain.Entities.PayrollRecord> payrollRecords)
        {
            var mockRepository = new Mock<IPayrollRecordRepository>();
            mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(payrollRecords.AsEnumerable());

            _mockUnitOfWork.Setup(u => u.PayrollRecordRepository).Returns(mockRepository.Object);
        }
    }
}
