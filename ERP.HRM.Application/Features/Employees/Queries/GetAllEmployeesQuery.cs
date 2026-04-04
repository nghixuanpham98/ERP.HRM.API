using ERP.HRM.Application.Common;
using ERP.HRM.Application.DTOs.Employee;
using MediatR;

namespace ERP.HRM.Application.Features.Employees.Queries
{
    /// <summary>
    /// Query to get all employees with pagination
    /// </summary>
    public class GetAllEmployeesQuery : IRequest<PagedResult<EmployeeDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string? Status { get; set; }
    }
}
