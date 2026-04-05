using ERP.HRM.Application.DTOs.Reporting;
using ERP.HRM.Application.Features.Reporting.Queries;
using ERP.HRM.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Reporting.Handlers
{
    /// <summary>
    /// Handler for GetMonthlyPayrollSummaryQuery
    /// </summary>
    public class GetMonthlyPayrollSummaryQueryHandler : IRequestHandler<GetMonthlyPayrollSummaryQuery, MonthlyPayrollSummaryDto>
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<GetMonthlyPayrollSummaryQueryHandler> _logger;

        public GetMonthlyPayrollSummaryQueryHandler(IReportingService reportingService, ILogger<GetMonthlyPayrollSummaryQueryHandler> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }

        public async Task<MonthlyPayrollSummaryDto> Handle(GetMonthlyPayrollSummaryQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetMonthlyPayrollSummaryQuery for {Year}-{Month}", request.Year, request.Month);
            return await _reportingService.GetMonthlyPayrollSummaryAsync(request.Year, request.Month, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetTaxSummaryReportQuery
    /// </summary>
    public class GetTaxSummaryReportQueryHandler : IRequestHandler<GetTaxSummaryReportQuery, TaxSummaryReportDto>
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<GetTaxSummaryReportQueryHandler> _logger;

        public GetTaxSummaryReportQueryHandler(IReportingService reportingService, ILogger<GetTaxSummaryReportQueryHandler> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }

        public async Task<TaxSummaryReportDto> Handle(GetTaxSummaryReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetTaxSummaryReportQuery for {Year}-{Month}", request.Year, request.Month);
            return await _reportingService.GetTaxSummaryReportAsync(request.Year, request.Month, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetInsuranceSummaryReportQuery
    /// </summary>
    public class GetInsuranceSummaryReportQueryHandler : IRequestHandler<GetInsuranceSummaryReportQuery, InsuranceSummaryReportDto>
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<GetInsuranceSummaryReportQueryHandler> _logger;

        public GetInsuranceSummaryReportQueryHandler(IReportingService reportingService, ILogger<GetInsuranceSummaryReportQueryHandler> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }

        public async Task<InsuranceSummaryReportDto> Handle(GetInsuranceSummaryReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetInsuranceSummaryReportQuery for {Year}-{Month}", request.Year, request.Month);
            return await _reportingService.GetInsuranceSummaryReportAsync(request.Year, request.Month, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetLeaveSummaryReportQuery
    /// </summary>
    public class GetLeaveSummaryReportQueryHandler : IRequestHandler<GetLeaveSummaryReportQuery, LeaveSummaryReportDto>
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<GetLeaveSummaryReportQueryHandler> _logger;

        public GetLeaveSummaryReportQueryHandler(IReportingService reportingService, ILogger<GetLeaveSummaryReportQueryHandler> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }

        public async Task<LeaveSummaryReportDto> Handle(GetLeaveSummaryReportQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetLeaveSummaryReportQuery for {Year}-{Month}", request.Year, request.Month);
            return await _reportingService.GetLeaveSummaryReportAsync(request.Year, request.Month, cancellationToken);
        }
    }

    /// <summary>
    /// Handler for GetHRMetricsQuery
    /// </summary>
    public class GetHRMetricsQueryHandler : IRequestHandler<GetHRMetricsQuery, HRMetricsDto>
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger<GetHRMetricsQueryHandler> _logger;

        public GetHRMetricsQueryHandler(IReportingService reportingService, ILogger<GetHRMetricsQueryHandler> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }

        public async Task<HRMetricsDto> Handle(GetHRMetricsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetHRMetricsQuery for {Year}", request.Year);
            return await _reportingService.GetHRMetricsAsync(request.Year, cancellationToken);
        }
    }
}
