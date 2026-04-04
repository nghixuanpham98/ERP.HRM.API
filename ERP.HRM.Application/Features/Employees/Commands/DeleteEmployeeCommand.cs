using MediatR;

namespace ERP.HRM.Application.Features.Employees.Commands
{
    /// <summary>
    /// Command to delete (soft delete) an employee
    /// </summary>
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public int EmployeeId { get; set; }

        public DeleteEmployeeCommand(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
