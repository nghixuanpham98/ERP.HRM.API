using MediatR;

namespace ERP.HRM.Application.Features.Departments.Commands
{
    public class DeleteDepartmentCommand : IRequest<bool>
    {
        public int DepartmentId { get; set; }
    }
}
