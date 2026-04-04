using ERP.HRM.Application.DTOs.Employee;
using MediatR;

namespace ERP.HRM.Application.Features.Employees.Queries
{
    /// <summary>
    /// Query to get employee by ID
    /// </summary>
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public int EmployeeId { get; set; }
    }
}
