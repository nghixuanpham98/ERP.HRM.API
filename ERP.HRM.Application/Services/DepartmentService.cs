using AutoMapper;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<DepartmentDto>> GetAllDepartmentsAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching all departments. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            var (departments, totalCount) = await _unitOfWork.DepartmentRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<DepartmentDto>
            {
                Items = _mapper.Map<IEnumerable<DepartmentDto>>(departments),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            _logger.LogInformation("Fetching department with Id {DepartmentId}", id);
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Department with Id {DepartmentId} not found", id);
                throw new NotFoundException($"Department with Id {id} not found");
            }

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> AddDepartmentAsync(CreateDepartmentDto dto)
        {
            try
            {
                if (await _unitOfWork.DepartmentRepository.ExistsByNameAsync(dto.DepartmentName))
                {
                    _logger.LogWarning("Department name '{DepartmentName}' already exists", dto.DepartmentName);
                    var ex = new BusinessRuleException("Department name already exists");
                    ex.Data["Errors"] = new List<string> { $"Tên phòng ban '{dto.DepartmentName}' đã tồn tại" };
                    throw ex;
                }

                var department = _mapper.Map<Department>(dto);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Department '{DepartmentName}' created successfully", dto.DepartmentName);

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating department");
                throw;
            }
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto dto)
        {
            try
            {
                _logger.LogInformation("Updating department with Id {DepartmentId}", dto.DepartmentId);
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(dto.DepartmentId);
                if (department == null)
                {
                    _logger.LogWarning("Department with Id {DepartmentId} not found", dto.DepartmentId);
                    throw new NotFoundException($"Department with Id {dto.DepartmentId} not found");
                }

                _mapper.Map(dto, department);
                await _unitOfWork.DepartmentRepository.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Department with Id {DepartmentId} updated successfully", dto.DepartmentId);

                return _mapper.Map<DepartmentDto>(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating department");
                throw;
            }
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting department with Id {DepartmentId}", id);
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
                if (department == null)
                {
                    _logger.LogWarning("Department with Id {DepartmentId} not found", id);
                    throw new NotFoundException($"Department with Id {id} not found");
                }

                await _unitOfWork.DepartmentRepository.SoftDeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Department with Id {DepartmentId} deleted successfully", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department");
                throw;
            }
        }
    }
}
