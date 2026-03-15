using AutoMapper;
using ERP.HRM.Application.DTOs;
using ERP.HRM.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ERP.HRM.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Department, CreateDepartmentDto>().ReverseMap();
            CreateMap<Department, UpdateDepartmentDto>().ReverseMap();

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();

            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<Position, CreatePositionDto>().ReverseMap();
            CreateMap<Position, UpdatePositionDto>().ReverseMap();
        }
    }
}
