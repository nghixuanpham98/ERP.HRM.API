using ERP.HRM.Application.DTOs.Department;
using MediatR;

namespace ERP.HRM.Application.Features.Departments.Commands
{
    public class UpdateDepartmentCommand : IRequest<DepartmentDto>
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string? Description { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? HeadOfDepartmentId { get; set; }
    }
}
