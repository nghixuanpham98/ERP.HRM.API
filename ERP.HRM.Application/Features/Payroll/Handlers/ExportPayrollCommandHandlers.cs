using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Application.Services;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for ExportPayrollCommand
    /// </summary>
    public class ExportPayrollCommandHandler : IRequestHandler<ExportPayrollCommand, PayrollExportResponseDto>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<ExportPayrollCommandHandler> _logger;

        public ExportPayrollCommandHandler(
            IPayrollExportService exportService,
            ILogger<ExportPayrollCommandHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<PayrollExportResponseDto> Handle(ExportPayrollCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ExportPayrollCommand for period {PayrollPeriodId}", request.PayrollPeriodId);

            var exportRequest = new PayrollExportRequestDto
            {
                PayrollPeriodId = request.PayrollPeriodId,
                ExportFormat = request.ExportFormat,
                DepartmentId = request.DepartmentId,
                IncludeEmployeeDetails = request.IncludeEmployeeDetails,
                IncludeSalaryBreakdown = request.IncludeSalaryBreakdown,
                IncludeDeductionsBreakdown = request.IncludeDeductionsBreakdown,
                ExportPurpose = request.ExportPurpose
            };

            return await _exportService.ExportPayrollAsync(exportRequest, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for ExportPayrollForBankCommand
    /// </summary>
    public class ExportPayrollForBankCommandHandler : IRequestHandler<ExportPayrollForBankCommand, PayrollExportResponseDto>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<ExportPayrollForBankCommandHandler> _logger;

        public ExportPayrollForBankCommandHandler(
            IPayrollExportService exportService,
            ILogger<ExportPayrollForBankCommandHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<PayrollExportResponseDto> Handle(ExportPayrollForBankCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ExportPayrollForBankCommand for period {PayrollPeriodId}", request.PayrollPeriodId);

            return await _exportService.ExportForBankTransferAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for ExportPayrollForTaxAuthorityCommand
    /// </summary>
    public class ExportPayrollForTaxAuthorityCommandHandler : IRequestHandler<ExportPayrollForTaxAuthorityCommand, PayrollExportResponseDto>
    {
        private readonly IPayrollExportService _exportService;
        private readonly ILogger<ExportPayrollForTaxAuthorityCommandHandler> _logger;

        public ExportPayrollForTaxAuthorityCommandHandler(
            IPayrollExportService exportService,
            ILogger<ExportPayrollForTaxAuthorityCommandHandler> logger)
        {
            _exportService = exportService;
            _logger = logger;
        }

        public async Task<PayrollExportResponseDto> Handle(ExportPayrollForTaxAuthorityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling ExportPayrollForTaxAuthorityCommand for period {PayrollPeriodId}", request.PayrollPeriodId);

            return await _exportService.ExportForTaxAuthorityAsync(request.PayrollPeriodId, request.DepartmentId, cancellationToken);
        }
    }
}
