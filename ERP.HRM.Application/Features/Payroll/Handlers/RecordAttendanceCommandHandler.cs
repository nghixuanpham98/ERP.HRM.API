using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Application.Features.Payroll.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Exceptions;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Features.Payroll.Handlers
{
    /// <summary>
    /// Handler for recording employee attendance
    /// </summary>
    public class RecordAttendanceCommandHandler : IRequestHandler<RecordAttendanceCommand, AttendanceDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecordAttendanceCommandHandler> _logger;

        public RecordAttendanceCommandHandler(IUnitOfWork unitOfWork, ILogger<RecordAttendanceCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AttendanceDto> Handle(RecordAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handling RecordAttendanceCommand for Employee: {EmployeeId}, Period: {PayrollPeriodId}", 
                    request.EmployeeId, request.PayrollPeriodId);

                // Validate employee exists
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
                if (employee == null)
                    throw new NotFoundException("Employee", request.EmployeeId);

                // Validate payroll period exists
                var period = await _unitOfWork.PayrollPeriodRepository.GetByIdAsync(request.PayrollPeriodId);
                if (period == null)
                    throw new NotFoundException("Payroll Period", request.PayrollPeriodId);

                // Check if attendance record already exists
                var existingAttendances = await _unitOfWork.AttendanceRepository.GetByEmployeeAndPeriodAsync(
                    request.EmployeeId, request.PayrollPeriodId);

                var recordOnDate = existingAttendances?.FirstOrDefault();

                Attendance attendance;
                if (recordOnDate != null)
                    {
                        // Update existing record
                        recordOnDate.WorkingDays = request.WorkingDays;
                        recordOnDate.IsPresent = request.IsPresent;
                        recordOnDate.OvertimeHours = request.OvertimeHours ?? 0;
                        recordOnDate.Note = request.Note;
                        recordOnDate.ModifiedDate = DateTime.UtcNow;

                        await _unitOfWork.AttendanceRepository.UpdateAsync(recordOnDate);
                        attendance = recordOnDate;
                        _logger.LogInformation("Updated existing attendance record for Employee: {EmployeeId}", request.EmployeeId);
                    }
                else
                {
                    // Create new record
                    attendance = new Attendance
                    {
                        EmployeeId = request.EmployeeId,
                        PayrollPeriodId = request.PayrollPeriodId,
                        AttendanceDate = request.AttendanceDate,
                        WorkingDays = request.WorkingDays,
                        IsPresent = request.IsPresent,
                        OvertimeHours = request.OvertimeHours ?? 0,
                        OvertimeMultiplier = 1.5m,
                        Note = request.Note,
                        CreatedDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow
                    };

                    await _unitOfWork.AttendanceRepository.AddAsync(attendance);
                    _logger.LogInformation("Created new attendance record for Employee: {EmployeeId}", request.EmployeeId);
                }

                await _unitOfWork.SaveChangesAsync();

                var dto = new AttendanceDto
                {
                    AttendanceId = attendance.AttendanceId,
                    EmployeeId = attendance.EmployeeId,
                    PayrollPeriodId = attendance.PayrollPeriodId,
                    AttendanceDate = attendance.AttendanceDate,
                    WorkingDays = attendance.WorkingDays,
                    IsPresent = attendance.IsPresent,
                    OvertimeHours = attendance.OvertimeHours,
                    Note = attendance.Note
                };

                _logger.LogInformation("Attendance recorded successfully for Employee: {EmployeeId}", request.EmployeeId);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling RecordAttendanceCommand");
                throw;
            }
        }
    }
}
