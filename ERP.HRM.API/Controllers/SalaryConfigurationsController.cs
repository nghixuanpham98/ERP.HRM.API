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
    public class SalaryConfigurationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SalaryConfigurationsController> _logger;

        public SalaryConfigurationsController(IUnitOfWork unitOfWork, ILogger<SalaryConfigurationsController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Create a new salary configuration for an employee
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> CreateSalaryConfiguration([FromBody] CreateSalaryConfigurationRequest request)
        {
            try
            {
                _logger.LogInformation("Creating SalaryConfiguration for Employee: {EmployeeId}", request.EmployeeId);

                // Verify employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    return NotFound(new ApiResponse<string>(false, "Nhân viên không tìm thấy", null));

                var config = new SalaryConfiguration
                {
                    EmployeeId = request.EmployeeId,
                    SalaryType = (SalaryType)request.SalaryType,
                    BaseSalary = request.BaseSalary ?? 0,
                    UnitPrice = request.UnitPrice,
                    HourlyRate = request.HourlyRate,
                    Allowance = request.Allowance,
                    InsuranceRate = request.InsuranceRate ?? 8,
                    TaxRate = request.TaxRate ?? 5,
                    EffectiveFrom = request.EffectiveFrom,
                    EffectiveTo = request.EffectiveTo,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.SalaryConfigurationRepository.AddAsync(config);

                _logger.LogInformation("SalaryConfiguration created successfully. Id: {SalaryConfigurationId}", config.SalaryConfigurationId);

                return CreatedAtAction(nameof(GetSalaryConfiguration), new { id = config.SalaryConfigurationId },
                    new ApiResponse<SalaryConfigurationResponse>(true, "Cấu hình lương được tạo thành công",
                        new SalaryConfigurationResponse
                        {
                            SalaryConfigurationId = config.SalaryConfigurationId,
                            EmployeeId = config.EmployeeId,
                            SalaryType = config.SalaryType.ToString(),
                            BaseSalary = config.BaseSalary,
                            UnitPrice = config.UnitPrice,
                            Allowance = config.Allowance,
                            InsuranceRate = config.InsuranceRate,
                            TaxRate = config.TaxRate
                        }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating SalaryConfiguration");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get salary configuration by id
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSalaryConfiguration(int id)
        {
            try
            {
                var config = await _unitOfWork.SalaryConfigurationRepository.GetByIdAsync(id);
                if (config == null)
                    return NotFound(new ApiResponse<string>(false, "Cấu hình lương không tìm thấy", null));

                return Ok(new ApiResponse<SalaryConfigurationResponse>(true, "Thành công",
                    new SalaryConfigurationResponse
                    {
                        SalaryConfigurationId = config.SalaryConfigurationId,
                        EmployeeId = config.EmployeeId,
                        SalaryType = config.SalaryType.ToString(),
                        BaseSalary = config.BaseSalary,
                        UnitPrice = config.UnitPrice,
                        HourlyRate = config.HourlyRate,
                        Allowance = config.Allowance,
                        InsuranceRate = config.InsuranceRate,
                        TaxRate = config.TaxRate,
                        IsActive = config.IsActive
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting SalaryConfiguration");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get salary configuration by employee id
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        [Authorize]
        public async Task<IActionResult> GetSalaryConfigurationByEmployeeId(int employeeId)
        {
            try
            {
                var config = await _unitOfWork.SalaryConfigurationRepository.GetActiveConfigByEmployeeIdAsync(employeeId);
                if (config == null)
                    return NotFound(new ApiResponse<string>(false, "Cấu hình lương cho nhân viên này không tìm thấy", null));

                return Ok(new ApiResponse<SalaryConfigurationResponse>(true, "Thành công",
                    new SalaryConfigurationResponse
                    {
                        SalaryConfigurationId = config.SalaryConfigurationId,
                        EmployeeId = config.EmployeeId,
                        SalaryType = config.SalaryType.ToString(),
                        BaseSalary = config.BaseSalary,
                        UnitPrice = config.UnitPrice,
                        HourlyRate = config.HourlyRate,
                        Allowance = config.Allowance,
                        InsuranceRate = config.InsuranceRate,
                        TaxRate = config.TaxRate,
                        IsActive = config.IsActive
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting SalaryConfiguration by EmployeeId");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update salary configuration
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> UpdateSalaryConfiguration(int id, [FromBody] UpdateSalaryConfigurationRequest request)
        {
            try
            {
                var config = await _unitOfWork.SalaryConfigurationRepository.GetByIdAsync(id);
                if (config == null)
                    return NotFound(new ApiResponse<string>(false, "Cấu hình lương không tìm thấy", null));

                config.BaseSalary = request.BaseSalary ?? config.BaseSalary;
                config.UnitPrice = request.UnitPrice ?? config.UnitPrice;
                config.HourlyRate = request.HourlyRate ?? config.HourlyRate;
                config.Allowance = request.Allowance ?? config.Allowance;
                config.InsuranceRate = request.InsuranceRate ?? config.InsuranceRate;
                config.TaxRate = request.TaxRate ?? config.TaxRate;
                config.EffectiveTo = request.EffectiveTo ?? config.EffectiveTo;
                config.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SalaryConfigurationRepository.UpdateAsync(config);

                _logger.LogInformation("SalaryConfiguration updated successfully. Id: {SalaryConfigurationId}", id);

                return Ok(new ApiResponse<SalaryConfigurationResponse>(true, "Cập nhật thành công",
                    new SalaryConfigurationResponse
                    {
                        SalaryConfigurationId = config.SalaryConfigurationId,
                        EmployeeId = config.EmployeeId,
                        SalaryType = config.SalaryType.ToString(),
                        BaseSalary = config.BaseSalary,
                        UnitPrice = config.UnitPrice,
                        Allowance = config.Allowance,
                        InsuranceRate = config.InsuranceRate,
                        TaxRate = config.TaxRate
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating SalaryConfiguration");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }

    public class CreateSalaryConfigurationRequest
    {
        public int EmployeeId { get; set; }
        public int SalaryType { get; set; } // 1=Monthly, 2=Production, 3=Hourly
        public decimal? BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class UpdateSalaryConfigurationRequest
    {
        public decimal? BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class SalaryConfigurationResponse
    {
        public int SalaryConfigurationId { get; set; }
        public int EmployeeId { get; set; }
        public string SalaryType { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? Allowance { get; set; }
        public decimal? InsuranceRate { get; set; }
        public decimal? TaxRate { get; set; }
        public bool IsActive { get; set; }
    }
}
