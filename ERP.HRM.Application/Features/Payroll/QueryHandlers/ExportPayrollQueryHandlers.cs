using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.QueryHandlers
{
    /// <summary>
    /// Handler for GetPayrollExportLinesQuery
    /// </summary>
    public class GetPayrollExportLinesQueryHandler : IRequestHandler<GetPayrollExportLinesQuery, List<PayrollExportLineDto>>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<GetPayrollExportLinesQueryHandler> _logger;

        public GetPayrollExportLinesQueryHandler(
            IPayrollExportService exportService,
            ILogger<GetPayrollExportLinesQueryHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<List<PayrollExportLineDto>> Handle(GetPayrollExportLinesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetPayrollExportLinesQuery for period {PayrollPeriodId}", request.PayrollPeriodId);

            return await _exportService.GetPayrollExportLinesAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetBankTransferExportLinesQuery
    /// </summary>
    public class GetBankTransferExportLinesQueryHandler : IRequestHandler<GetBankTransferExportLinesQuery, List<BankTransferExportDto>>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<GetBankTransferExportLinesQueryHandler> _logger;

        public GetBankTransferExportLinesQueryHandler(
            IPayrollExportService exportService,
            ILogger<GetBankTransferExportLinesQueryHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<List<BankTransferExportDto>> Handle(GetBankTransferExportLinesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetBankTransferExportLinesQuery for period {PayrollPeriodId}", request.PayrollPeriodId);

            return await _exportService.GetBankTransferLinesAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetTaxAuthorityExportLinesQuery
    /// </summary>
    public class GetTaxAuthorityExportLinesQueryHandler : IRequestHandler<GetTaxAuthorityExportLinesQuery, List<TaxAuthorityExportDto>>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<GetTaxAuthorityExportLinesQueryHandler> _logger;

        public GetTaxAuthorityExportLinesQueryHandler(
            IPayrollExportService exportService,
            ILogger<GetTaxAuthorityExportLinesQueryHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<List<TaxAuthorityExportDto>> Handle(GetTaxAuthorityExportLinesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetTaxAuthorityExportLinesQuery for period {PayrollPeriodId}", request.PayrollPeriodId);

            return await _exportService.GetTaxAuthorityExportLinesAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetPayrollExportSummaryQuery
    /// </summary>
    public class GetPayrollExportSummaryQueryHandler : IRequestHandler<GetPayrollExportSummaryQuery, PayrollExportSummaryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPayrollExportSummaryQueryHandler> _logger;

        public GetPayrollExportSummaryQueryHandler(
            IUnitOfWork unitOfWork,
            ILogger<GetPayrollExportSummaryQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PayrollExportSummaryDto> Handle(GetPayrollExportSummaryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetPayrollExportSummaryQuery for period {PayrollPeriodId}", request.PayrollPeriodId);

            var allRecords = await _unitOfWork.PayrollRecordRepository.GetAllAsync();
            var payrollRecords = allRecords
                .Where(pr => pr.PayrollPeriodId == request.PayrollPeriodId)
                .ToList();

            // Filter by department if specified
            if (request.DepartmentId.HasValue)
            {
                payrollRecords = payrollRecords
                    .Where(pr => pr.Employee != null && pr.Employee.DepartmentId == request.DepartmentId.Value)
                    .ToList();
            }

            var summary = new PayrollExportSummaryDto
            {
                PayrollPeriodId = request.PayrollPeriodId,
                TotalEmployees = payrollRecords.Count,
                TotalGrossSalary = payrollRecords.Sum(pr => pr.GrossSalary),
                TotalInsuranceDeduction = payrollRecords.Sum(pr => pr.InsuranceDeduction),
                TotalTaxDeduction = payrollRecords.Sum(pr => pr.TaxDeduction),
                TotalOtherDeductions = payrollRecords.Sum(pr => pr.OtherDeductions),
                TotalNetSalary = payrollRecords.Sum(pr => pr.NetSalary),
                ExportDate = DateTime.Now
            };

            _logger.LogInformation("Payroll export summary: {TotalEmployees} employees, Total Gross: {TotalGross}, Total Net: {TotalNet}",
                summary.TotalEmployees, summary.TotalGrossSalary, summary.TotalNetSalary);

            return summary;
        }
    }
}
