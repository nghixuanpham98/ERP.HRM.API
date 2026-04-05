using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Interface for payroll export service (Excel/PDF)
    /// Supports exports for banks, tax authorities, and general purposes
    /// </summary>
    public interface IPayrollExportService
    {
        /// <summary>
        /// Export payroll records as Excel or PDF
        /// </summary>
        Task<PayrollExportResponseDto> ExportPayrollAsync(PayrollExportRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Export payroll for bank transfer requirements
        /// </summary>
        Task<PayrollExportResponseDto> ExportForBankTransferAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Export payroll for tax authority (PIT - Thuế TNCN)
        /// </summary>
        Task<PayrollExportResponseDto> ExportForTaxAuthorityAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get payroll export lines for a period
        /// </summary>
        Task<List<PayrollExportLineDto>> GetPayrollExportLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get bank transfer lines for export
        /// </summary>
        Task<List<BankTransferExportDto>> GetBankTransferLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get tax authority export lines
        /// </summary>
        Task<List<TaxAuthorityExportDto>> GetTaxAuthorityExportLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Service for exporting payroll data to Excel/PDF formats
    /// Supports bank transfers and tax authority requirements
    /// </summary>
    public class PayrollExportService : IPayrollExportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayrollExportService> _logger;
        private readonly IPayrollService _payrollService;
        private readonly IVietnameseTaxService _taxService;

        public PayrollExportService(
            IUnitOfWork unitOfWork,
            ILogger<PayrollExportService> logger,
            IPayrollService payrollService,
            IVietnameseTaxService taxService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _payrollService = payrollService;
            _taxService = taxService;
        }

        /// <summary>
        /// Export payroll records as Excel or PDF
        /// </summary>
        public async Task<PayrollExportResponseDto> ExportPayrollAsync(PayrollExportRequestDto request, CancellationToken cancellationToken = default)
        {
            // Validate request
            if (request == null)
                throw new BusinessRuleException("Export request cannot be null");

            _logger.LogInformation("Exporting payroll for period {PayrollPeriodId} in {ExportFormat} format", 
                request.PayrollPeriodId, request.ExportFormat);

            if (!request.ExportFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase) &&
                !request.ExportFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                throw new BusinessRuleException("Export format must be 'Excel' or 'PDF'");

            // Get payroll records
            var allRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
            var payrollRecords = allRecords
                .Where(pr => pr.PayrollPeriodId == request.PayrollPeriodId)
                .ToList();

            if (!payrollRecords.Any())
                throw new NotFoundException($"No payroll records found for period {request.PayrollPeriodId}");

            // Filter by department if specified
            if (request.DepartmentId.HasValue)
            {
                payrollRecords = payrollRecords
                    .Where(pr => pr.Employee != null && pr.Employee.DepartmentId == request.DepartmentId.Value)
                    .ToList();
            }

            if (!payrollRecords.Any())
                throw new NotFoundException($"No payroll records found for specified filters");

            _logger.LogInformation("Found {RecordCount} payroll records to export", payrollRecords.Count);

            // Get export lines
            var exportLines = await GetPayrollExportLinesAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);

            // Generate export based on purpose
            byte[] fileContent = request.ExportPurpose.ToLower() switch
            {
                "bank" => GenerateBankExportExcel(exportLines, request.PayrollPeriodId),
                "taxauthority" => GenerateTaxAuthorityExportExcel(exportLines, request.PayrollPeriodId),
                _ => GenerateGeneralPayrollExport(exportLines, request.PayrollPeriodId, request.ExportFormat)
            };

            // Create response
            var response = new PayrollExportResponseDto
            {
                FileName = GenerateFileName(request.ExportFormat, request.ExportPurpose, request.PayrollPeriodId),
                FileContent = fileContent,
                ContentType = GetContentType(request.ExportFormat),
                ExportDate = DateTime.Now,
                TotalRecords = payrollRecords.Count,
                TotalGrossSalary = payrollRecords.Sum(pr => pr.GrossSalary),
                TotalNetSalary = payrollRecords.Sum(pr => pr.NetSalary),
                TotalTaxDeduction = payrollRecords.Sum(pr => pr.TaxDeduction)
            };

            _logger.LogInformation("Payroll export completed: {FileName} ({FileSize} bytes)", 
                response.FileName, fileContent.Length);

            return response;
        }

        /// <summary>
        /// Export payroll for bank transfer requirements
        /// </summary>
        public async Task<PayrollExportResponseDto> ExportForBankTransferAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Exporting payroll for bank transfer: Period {PayrollPeriodId}", payrollPeriodId);

            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = payrollPeriodId,
                ExportFormat = "Excel",
                DepartmentId = departmentId,
                ExportPurpose = "Bank",
                IncludeEmployeeDetails = true,
                IncludeSalaryBreakdown = false,
                IncludeDeductionsBreakdown = false
            };

            return await ExportPayrollAsync(request, cancellationToken);
        }

        /// <summary>
        /// Export payroll for tax authority (PIT - Thuế TNCN)
        /// </summary>
        public async Task<PayrollExportResponseDto> ExportForTaxAuthorityAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Exporting payroll for tax authority: Period {PayrollPeriodId}", payrollPeriodId);

            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = payrollPeriodId,
                ExportFormat = "Excel",
                DepartmentId = departmentId,
                ExportPurpose = "TaxAuthority",
                IncludeEmployeeDetails = true,
                IncludeSalaryBreakdown = true,
                IncludeDeductionsBreakdown = true
            };

            return await ExportPayrollAsync(request, cancellationToken);
        }

        /// <summary>
        /// Get payroll export lines for a period
        /// </summary>
        public async Task<List<PayrollExportLineDto>> GetPayrollExportLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting payroll export lines for period {PayrollPeriodId}", payrollPeriodId);

            var allRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
            var payrollRecords = allRecords
                .Where(pr => pr.PayrollPeriodId == payrollPeriodId)
                .ToList();

            if (!payrollRecords.Any())
                return new List<PayrollExportLineDto>();

            // Filter by department if specified
            if (departmentId.HasValue)
            {
                payrollRecords = payrollRecords
                    .Where(pr => pr.Employee != null && pr.Employee.DepartmentId == departmentId.Value)
                    .ToList();
            }

            // Map to export DTOs
            var exportLines = payrollRecords.Select(pr => new PayrollExportLineDto
            {
                PayrollRecordId = pr.PayrollRecordId,
                EmployeeId = pr.EmployeeId,
                EmployeeCode = pr.Employee?.EmployeeCode ?? "N/A",
                EmployeeName = pr.Employee?.FullName ?? "Unknown",
                DepartmentName = pr.Employee?.Department?.DepartmentName ?? "N/A",
                PositionName = pr.Employee?.Position?.PositionName ?? "N/A",
                BaseSalary = pr.BaseSalary,
                Allowance = pr.Allowance,
                OvertimeCompensation = pr.OvertimeCompensation,
                GrossSalary = pr.GrossSalary,
                InsuranceDeduction = pr.InsuranceDeduction,
                TaxDeduction = pr.TaxDeduction,
                OtherDeductions = pr.OtherDeductions,
                TotalDeductions = pr.TotalDeductions,
                NetSalary = pr.NetSalary,
                PaymentDate = pr.PaymentDate
            }).ToList();

            _logger.LogInformation("Generated {LineCount} export lines", exportLines.Count);

            return exportLines;
        }

        /// <summary>
        /// Get bank transfer lines for export
        /// </summary>
        public async Task<List<BankTransferExportDto>> GetBankTransferLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting bank transfer export lines for period {PayrollPeriodId}", payrollPeriodId);

            var allRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
            var payrollRecords = allRecords
                .Where(pr => pr.PayrollPeriodId == payrollPeriodId)
                .ToList();

            if (!payrollRecords.Any())
                return new List<BankTransferExportDto>();

            // Filter by department if specified
            if (departmentId.HasValue)
            {
                payrollRecords = payrollRecords
                    .Where(pr => pr.Employee != null && pr.Employee.DepartmentId == departmentId.Value)
                    .ToList();
            }

            var bankTransfers = payrollRecords
                .Where(pr => pr.Employee != null)
                .Select(pr => new BankTransferExportDto
                {
                    BankCode = "VCB",
                    BankName = "VietcomBank",
                    EmployeeId = pr.EmployeeId,
                    EmployeeName = pr.Employee?.FullName ?? "Unknown",
                    BankAccountNumber = "N/A",
                    TransferAmount = pr.NetSalary,
                    Description = $"Salary - {pr.Employee?.FullName} - {DateTime.Now:MMMM yyyy}"
                }).ToList();

            _logger.LogInformation("Generated {TransferCount} bank transfer lines", bankTransfers.Count);

            return bankTransfers;
        }

        /// <summary>
        /// Get tax authority export lines (PIT - Thuế TNCN)
        /// </summary>
        public async Task<List<TaxAuthorityExportDto>> GetTaxAuthorityExportLinesAsync(int payrollPeriodId, int? departmentId = null, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting tax authority export lines for period {PayrollPeriodId}", payrollPeriodId);

            var allRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
            var payrollRecords = allRecords
                .Where(pr => pr.PayrollPeriodId == payrollPeriodId)
                .ToList();

            if (!payrollRecords.Any())
                return new List<TaxAuthorityExportDto>();

            // Filter by department if specified
            if (departmentId.HasValue)
            {
                payrollRecords = payrollRecords
                    .Where(pr => pr.Employee != null && pr.Employee.DepartmentId == departmentId.Value)
                    .ToList();
            }

            var taxExports = new List<TaxAuthorityExportDto>();

            foreach (var pr in payrollRecords.Where(p => p.Employee != null))
            {
                // Calculate tax using Vietnamese tax service
                var taxCalculation = await _taxService.CalculateTaxAsync(pr.GrossSalary);

                taxExports.Add(new TaxAuthorityExportDto
                {
                    TaxCode = pr.Employee?.NationalId ?? "N/A",
                    EmployeeId = pr.EmployeeId,
                    EmployeeName = pr.Employee?.FullName ?? "Unknown",
                    EmployeeCode = pr.Employee?.EmployeeCode ?? "N/A",
                    GrossSalary = pr.GrossSalary,
                    TaxableIncome = pr.GrossSalary,
                    TaxAmount = taxCalculation.TaxAmount,
                    EffectiveTaxRate = taxCalculation.EffectiveTaxRate,
                    TaxBracketLevel = $"Level {taxCalculation.ApplicableBracketLevel}",
                    Period = DateTime.Now.ToString("yyyy-MM")
                });
            }

            _logger.LogInformation("Generated {TaxLineCount} tax authority export lines", taxExports.Count);

            return taxExports;
        }

        /// <summary>
        /// Generate bank export in Excel format
        /// </summary>
        private byte[] GenerateBankExportExcel(List<PayrollExportLineDto> lines, int payrollPeriodId)
        {
            _logger.LogInformation("Generating bank export Excel file");

            // Simple CSV format (Excel compatible)
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Mã nhân viên,Tên nhân viên,Số tài khoản ngân hàng,Số tiền chuyển,Mô tả");
            csv.AppendLine($"\"Bảng lương tháng\",\"\",\"\",\"{lines.Sum(l => l.NetSalary)}\",\"\"");
            csv.AppendLine();

            foreach (var line in lines)
            {
                csv.AppendLine($"\"{line.EmployeeCode}\",\"{line.EmployeeName}\",\"{line.EmployeeCode}\",\"{line.NetSalary:F2}\",\"Lương tháng {DateTime.Now:MM/yyyy}\"");
            }

            return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Generate tax authority export in Excel format
        /// </summary>
        private byte[] GenerateTaxAuthorityExportExcel(List<PayrollExportLineDto> lines, int payrollPeriodId)
        {
            _logger.LogInformation("Generating tax authority export Excel file");

            // Simple CSV format (Excel compatible)
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Mã nhân viên,Tên nhân viên,Lương cơ sở,Lương tính thuế,Số thuế,Mức thuế (%)");
            csv.AppendLine($"\"Báo cáo thuế TNCN tháng\",\"\",\"\",\"\",\"{lines.Sum(l => l.TaxDeduction):F2}\",\"\"");
            csv.AppendLine();

            foreach (var line in lines)
            {
                var effectiveRate = line.GrossSalary > 0 
                    ? ((line.TaxDeduction / line.GrossSalary) * 100).ToString("F2", CultureInfo.InvariantCulture)
                    : "0.00";

                csv.AppendLine($"\"{line.EmployeeCode}\",\"{line.EmployeeName}\",\"{line.BaseSalary:F2}\",\"{line.GrossSalary:F2}\",\"{line.TaxDeduction:F2}\",\"{effectiveRate}\"");
            }

            return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Generate general payroll export
        /// </summary>
        private byte[] GenerateGeneralPayrollExport(List<PayrollExportLineDto> lines, int payrollPeriodId, string format)
        {
            _logger.LogInformation("Generating general payroll {Format} export", format);

            if (format.Equals("PDF", StringComparison.OrdinalIgnoreCase))
            {
                return GeneratePdfContent(lines, payrollPeriodId);
            }

            // Excel/CSV format
            return GenerateExcelContent(lines, payrollPeriodId);
        }

        /// <summary>
        /// Generate Excel/CSV content
        /// </summary>
        private byte[] GenerateExcelContent(List<PayrollExportLineDto> lines, int payrollPeriodId)
        {
            var csv = new System.Text.StringBuilder();
            
            // Header
            csv.AppendLine("BẢNG TÍNH LƯƠNG THÁNG");
            csv.AppendLine($"Kỳ: {DateTime.Now:MMMM yyyy}");
            csv.AppendLine();

            // Column headers
            csv.AppendLine("Mã NV,Tên NV,Phòng ban,Chức vụ,Lương cơ sở,Phụ cấp,Lương tăng ca,Tổng lương,BHXH,Thuế,Khác,Tổng khấu trừ,Lương thực lĩnh");

            // Data rows
            foreach (var line in lines)
            {
                csv.AppendLine($"\"{line.EmployeeCode}\",\"{line.EmployeeName}\",\"{line.DepartmentName}\",\"{line.PositionName}\"," +
                    $"\"{line.BaseSalary:F2}\",\"{line.Allowance:F2}\",\"{line.OvertimeCompensation:F2}\",\"{line.GrossSalary:F2}\"," +
                    $"\"{line.InsuranceDeduction:F2}\",\"{line.TaxDeduction:F2}\",\"{line.OtherDeductions:F2}\",\"{line.TotalDeductions:F2}\",\"{line.NetSalary:F2}\"");
            }

            // Summary
            csv.AppendLine();
            csv.AppendLine("TỔNG CỘNG,,,,,,,," +
                $"\"{lines.Sum(l => l.GrossSalary):F2}\"," +
                $"\"{lines.Sum(l => l.InsuranceDeduction):F2}\"," +
                $"\"{lines.Sum(l => l.TaxDeduction):F2}\"," +
                $"\"{lines.Sum(l => l.OtherDeductions):F2}\"," +
                $"\"{lines.Sum(l => l.TotalDeductions):F2}\"," +
                $"\"{lines.Sum(l => l.NetSalary):F2}\"");

            return System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        }

        /// <summary>
        /// Generate PDF content (placeholder - returns basic text)
        /// </summary>
        private byte[] GeneratePdfContent(List<PayrollExportLineDto> lines, int payrollPeriodId)
        {
            var content = new System.Text.StringBuilder();
            
            content.AppendLine("BẢNG TÍNH LƯƠNG THÁNG");
            content.AppendLine($"Kỳ: {DateTime.Now:MMMM yyyy}");
            content.AppendLine("================================================================================");
            content.AppendLine();

            foreach (var line in lines)
            {
                content.AppendLine($"Mã NV: {line.EmployeeCode} | {line.EmployeeName}");
                content.AppendLine($"Phòng ban: {line.DepartmentName} | Chức vụ: {line.PositionName}");
                content.AppendLine($"Lương cơ sở: {line.BaseSalary:F2} | Phụ cấp: {line.Allowance:F2}");
                content.AppendLine($"Tổng lương: {line.GrossSalary:F2}");
                content.AppendLine($"Khấu trừ: BHXH {line.InsuranceDeduction:F2}, Thuế {line.TaxDeduction:F2}");
                content.AppendLine($"Lương thực lĩnh: {line.NetSalary:F2}");
                content.AppendLine("--------------------------------------------------------------------------------");
            }

            content.AppendLine();
            content.AppendLine("TỔNG CỘNG");
            content.AppendLine($"Tổng lương: {lines.Sum(l => l.GrossSalary):F2}");
            content.AppendLine($"Tổng khấu trừ: {lines.Sum(l => l.TotalDeductions):F2}");
            content.AppendLine($"Tổng lương thực lĩnh: {lines.Sum(l => l.NetSalary):F2}");

            return System.Text.Encoding.UTF8.GetBytes(content.ToString());
        }

        /// <summary>
        /// Generate export file name
        /// </summary>
        private string GenerateFileName(string format, string purpose, int payrollPeriodId)
        {
            var ext = format.Equals("PDF", StringComparison.OrdinalIgnoreCase) ? ".pdf" : ".csv";
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var purposePrefix = purpose.ToLower() switch
            {
                "bank" => "BankTransfer",
                "taxauthority" => "TaxAuthority",
                _ => "Payroll"
            };

            return $"{purposePrefix}_P{payrollPeriodId}_{timestamp}{ext}";
        }

        /// <summary>
        /// Get content type for format
        /// </summary>
        private string GetContentType(string format)
        {
            return format.Equals("PDF", StringComparison.OrdinalIgnoreCase) 
                ? "application/pdf" 
                : "text/csv";
        }
    }
}
