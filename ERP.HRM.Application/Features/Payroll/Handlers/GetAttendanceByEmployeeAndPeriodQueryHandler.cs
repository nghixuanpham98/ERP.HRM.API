using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Queries;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for getting attendance records by employee and period
    /// </summary>
    public class GetAttendanceByEmployeeAndPeriodQueryHandler : IRequestHandler<GetAttendanceByEmployeeAndPeriodQuery, IEnumerable<AttendanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAttendanceByEmployeeAndPeriodQueryHandler> _logger;

        public GetAttendanceByEmployeeAndPeriodQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAttendanceByEmployeeAndPeriodQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AttendanceDto>> Handle(GetAttendanceByEmployeeAndPeriodQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling GetAttendanceByEmployeeAndPeriodQuery for Employee: {EmployeeId}, Period: {PayrollPeriodId}", 
                    request.EmployeeId, request.PayrollPeriodId);

                // Validate employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", request.EmployeeId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Get attendance records
                var attendanceRecords = await _unitOfWork.AttendanceRepository.GetByEmployeeAndPeriodAsync(
                    request.EmployeeId, request.PayrollPeriodId);

                _logger.LogInformation("Retrieved {Count} attendance records for Employee: {EmployeeId}, Period: {PayrollPeriodId}",
                    attendanceRecords.Count(), request.EmployeeId, request.PayrollPeriodId);

                // Map to DTOs
                var dtos = attendanceRecords.Select(a => new AttendanceDto
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeId = a.EmployeeId,
                    PayrollPeriodId = a.PayrollPeriodId,
                    AttendanceDate = a.AttendanceDate,
                    WorkingDays = a.WorkingDays,
                    IsPresent = a.IsPresent,
                    OvertimeHours = a.OvertimeHours,
                    Note = a.Note
                }).OrderBy(a => a.AttendanceDate).ToList();

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling GetAttendanceByEmployeeAndPeriodQuery");
                throw;
            }
        }
    }
}
