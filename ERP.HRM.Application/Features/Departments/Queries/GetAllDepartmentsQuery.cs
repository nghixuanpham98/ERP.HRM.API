using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Department;
using MediatR;

namespace ERP.HRM.Application.Features.Departments.Queries
{
    public class GetAllDepartmentsQuery : IRequest<PagedResult<DepartmentDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
