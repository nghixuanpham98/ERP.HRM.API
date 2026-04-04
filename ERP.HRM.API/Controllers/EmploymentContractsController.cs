using AutoMapper;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.HR;
using ERP.HRM.Domain.Constants;
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
    public class EmploymentContractsController : ControllerBase
    {
        private readonly IEmploymentContractRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EmploymentContractsController> _logger;

        public EmploymentContractsController(
            IEmploymentContractRepository repository,
            IMapper mapper,
            ILogger<EmploymentContractsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all employment contracts
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all employment contracts");
                var contracts = await _repository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<EmploymentContractDto>>(contracts);
                return Ok(new ApiResponse<IEnumerable<EmploymentContractDto>>(true, "Lấy danh sách hợp đồng thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employment contracts");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get contracts by employee
        /// </summary>
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting contracts for employee {EmployeeId}", employeeId);
                var contracts = await _repository.GetByEmployeeIdAsync(employeeId);
                var dtos = _mapper.Map<IEnumerable<EmploymentContractDto>>(contracts);
                return Ok(new ApiResponse<IEnumerable<EmploymentContractDto>>(true, "Lấy danh sách hợp đồng thành công", dtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contracts");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get active contract for employee
        /// </summary>
        [HttpGet("employee/{employeeId}/active")]
        public async Task<IActionResult> GetActiveContract(int employeeId)
        {
            try
            {
                _logger.LogInformation("Getting active contract for employee {EmployeeId}", employeeId);
                var contract = await _repository.GetActiveContractByEmployeeIdAsync(employeeId);
                if (contract == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hợp đồng hoạt động", null));

                var dto = _mapper.Map<EmploymentContractDto>(contract);
                return Ok(new ApiResponse<EmploymentContractDto>(true, "Lấy hợp đồng hoạt động thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active contract");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Get contract by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Getting contract {Id}", id);
                var contract = await _repository.GetByIdAsync(id);
                if (contract == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hợp đồng", null));

                var dto = _mapper.Map<EmploymentContractDto>(contract);
                return Ok(new ApiResponse<EmploymentContractDto>(true, "Lấy hợp đồng thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting contract");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Create a new employment contract
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Create([FromBody] CreateEmploymentContractDto createDto)
        {
            try
            {
                _logger.LogInformation("Creating new contract for employee {EmployeeId}", createDto.EmployeeId);
                
                var contract = new EmploymentContract
                {
                    EmployeeId = createDto.EmployeeId,
                    ContractNumber = createDto.ContractNumber,
                    ContractType = createDto.ContractType,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    ProbationEndDate = createDto.ProbationEndDate,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(contract);
                var dto = _mapper.Map<EmploymentContractDto>(contract);
                return CreatedAtAction(nameof(GetById), new { id = contract.EmploymentContractId }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contract");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Update an employment contract
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.HR}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmploymentContractDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating contract {Id}", id);
                
                var contract = await _repository.GetByIdAsync(id);
                if (contract == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hợp đồng", null));

                contract.ContractNumber = updateDto.ContractNumber;
                contract.ContractType = updateDto.ContractType;
                contract.StartDate = updateDto.StartDate;
                contract.EndDate = updateDto.EndDate;
                contract.ProbationEndDate = updateDto.ProbationEndDate;
                contract.IsActive = updateDto.IsActive;
                contract.TerminationReason = updateDto.TerminationReason;
                contract.ModifiedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(contract);
                var dto = _mapper.Map<EmploymentContractDto>(contract);
                return Ok(new ApiResponse<EmploymentContractDto>(true, "Cập nhật hợp đồng thành công", dto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contract");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }

        /// <summary>
        /// Delete an employment contract
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting contract {Id}", id);
                
                var contract = await _repository.GetByIdAsync(id);
                if (contract == null)
                    return NotFound(new ApiResponse<string>(false, "Không tìm thấy hợp đồng", null));

                await _repository.DeleteAsync(id);
                return Ok(new ApiResponse<string>(true, "Xóa hợp đồng thành công", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contract");
                return BadRequest(new ApiResponse<string>(false, ex.Message, null));
            }
        }
    }
}
