using AutoMapper;
using ERP.HRM.API;
using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Interfaces;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<DepartmentDto>> GetAllDepartmentsAsync(int pageNumber, int pageSize)
        {
            var (departments, totalCount) = await _departmentRepository.GetPagedAsync(pageNumber, pageSize);

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
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) throw new NotFoundException($"Department with Id {id} not found");

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> AddDepartmentAsync(CreateDepartmentDto dto)
        {
            if (await _departmentRepository.ExistsByNameAsync(dto.DepartmentName))
                throw new BusinessRuleException("Department name already exists");

            var department = _mapper.Map<Department>(dto);
            await _departmentRepository.AddAsync(department);

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<DepartmentDto> UpdateDepartmentAsync(UpdateDepartmentDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null) throw new NotFoundException($"Department with Id {dto.DepartmentId} not found");

            _mapper.Map(dto, department);
            await _departmentRepository.UpdateAsync(department);

            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null) throw new NotFoundException($"Department with Id {id} not found");

            await _departmentRepository.DeleteAsync(id);
        }
    }
}
