using AutoMapper;
using ERP.HRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ERP.HRM.Application.DTOs.Department;
using ERP.HRM.Application.DTOs.Employee;
using ERP.HRM.Application.DTOs.Position;
using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.DTOs.HR;

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

            // Payroll mappings
            CreateMap<SalaryConfiguration, SalaryConfigurationDto>().ReverseMap();
            CreateMap<PayrollPeriod, PayrollPeriodDto>().ReverseMap();
            CreateMap<Attendance, AttendanceDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductionOutput, ProductionOutputDto>().ReverseMap();
            CreateMap<PayrollRecord, PayrollRecordDto>()
                .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.SalaryType.ToString()))
                .ReverseMap();
            CreateMap<PayrollDeduction, PayrollDeductionDto>().ReverseMap();

            // HR Enhancement mappings
            CreateMap<EmploymentContract, EmploymentContractDto>().ReverseMap();
            CreateMap<EmploymentContract, CreateEmploymentContractDto>().ReverseMap();
            CreateMap<EmploymentContract, UpdateEmploymentContractDto>().ReverseMap();

            CreateMap<SalaryGrade, SalaryGradeDto>().ReverseMap();
            CreateMap<SalaryGrade, CreateSalaryGradeDto>().ReverseMap();
            CreateMap<SalaryGrade, UpdateSalaryGradeDto>().ReverseMap();

            CreateMap<FamilyDependent, FamilyDependentDto>().ReverseMap();
            CreateMap<FamilyDependent, CreateFamilyDependentDto>().ReverseMap();
            CreateMap<FamilyDependent, UpdateFamilyDependentDto>().ReverseMap();

            CreateMap<SalaryAdjustmentDecision, SalaryAdjustmentDecisionDto>().ReverseMap();
            CreateMap<SalaryAdjustmentDecision, CreateSalaryAdjustmentDecisionDto>().ReverseMap();
            CreateMap<SalaryAdjustmentDecision, UpdateSalaryAdjustmentDecisionDto>().ReverseMap();

            CreateMap<TaxBracket, TaxBracketDto>().ReverseMap();
            CreateMap<TaxBracket, CreateTaxBracketDto>().ReverseMap();
            CreateMap<TaxBracket, UpdateTaxBracketDto>().ReverseMap();

            CreateMap<InsuranceTier, InsuranceTierDto>().ReverseMap();
            CreateMap<InsuranceTier, CreateInsuranceTierDto>().ReverseMap();
            CreateMap<InsuranceTier, UpdateInsuranceTierDto>().ReverseMap();
        }
    }
}
