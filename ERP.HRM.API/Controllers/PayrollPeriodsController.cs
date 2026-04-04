using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PayrollPeriodsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PayrollPeriodsController> _logger;

        public PayrollPeriodsController(IUnitOfWork unitOfWork, ILogger<PayrollPeriodsController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create a new payroll period
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreatePayrollPeriod([FromBody] CreatePayrollPeriodRequest request)
        {
            try
            {
                _logger.LogInformation("Creating PayrollPeriod for {Month}/{Year}", request.Month, request.Year);

                var period = new PayrollPeriod
                {
                    Year = request.Year,
                    Month = request.Month,
                    PeriodName = request.PeriodName,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    TotalWorkingDays = request.TotalWorkingDays,
                    IsFinalized = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.PayrollPeriodRepository.AddAsync(period);

                _logger.LogInformation("PayrollPeriod created successfully. Id: {PayrollPeriodId}", period.PayrollPeriodId);

                return CreatedAtAction(nameof(GetPayrollPeriod), new { id = period.PayrollPeriodId },
                    new ApiResponse<PayrollPeriodResponse>(true, "Kỳ lương được tạo thành công",
                        new PayrollPeriodResponse
                        {
                            PayrollPeriodId = period.PayrollPeriodId,
                            PeriodName = period.PeriodName,
                            Year = period.Year,
                            Month = period.Month,
                            TotalWorkingDays = period.TotalWorkingDays
                        }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PayrollPeriod");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get payroll period by id
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPayrollPeriod(int id)
        {
            try
            {
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(id);
                if (period == null)
                    return NotFound(new ApiResponse<string>(false, "Kỳ lương không tìm thấy", null));

                return Ok(new ApiResponse<PayrollPeriodResponse>(true, "Thành công",
                    new PayrollPeriodResponse
                    {
                        PayrollPeriodId = period.PayrollPeriodId,
                        PeriodName = period.PeriodName,
                        Year = period.Year,
                        Month = period.Month,
                        TotalWorkingDays = period.TotalWorkingDays,
                        IsFinalized = period.IsFinalized
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting PayrollPeriod");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get all payroll periods
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPayrollPeriods()
        {
            try
            {
                var periods = await _unitOfWork.PayrollPeriodRepository.GetAllAsync();
                var periodResponses = periods.Select(p => new PayrollPeriodResponse
                {
                    PayrollPeriodId = p.PayrollPeriodId,
                    PeriodName = p.PeriodName,
                    Year = p.Year,
                    Month = p.Month,
                    TotalWorkingDays = p.TotalWorkingDays,
                    IsFinalized = p.IsFinalized,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                });

                return Ok(new ApiResponse<IEnumerable<PayrollPeriodResponse>>(true, "Thành công", periodResponses));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all PayrollPeriods");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }

    public class CreatePayrollPeriodRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWorkingDays { get; set; }
    }

    public class PayrollPeriodResponse
    {
        public int PayrollPeriodId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWorkingDays { get; set; }
        public bool IsFinalized { get; set; }
    }
}
