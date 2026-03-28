using ERP.HRM.Application.DTOs.Department;
using MediatR;

namespace ERP.HRM.Application.Features.Departments.Queries
{
    public class GetDepartmentByIdQuery : IRequest<DepartmentDto>
    {
        public int DepartmentId { get; set; }
    }
}
