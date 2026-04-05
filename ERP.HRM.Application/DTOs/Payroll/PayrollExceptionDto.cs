namespace ERP.HRM.Application.DTOs.Payroll
{
    public class PayrollExceptionDto
    {
        public int PayrollExceptionId { get; set; }
        public int PayrollPeriodId { get; set; }
        public int? EmployeeId { get; set; }
        public string ExceptionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid? ResolvedByUserId { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? ResolutionNotes { get; set; }
        public bool IsBlocking { get; set; }
    }

    public class CreatePayrollExceptionDto
    {
        public int PayrollPeriodId { get; set; }
        public int? EmployeeId { get; set; }
        public string ExceptionType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Severity { get; set; } = "Error";
        public string PayrollImpact { get; set; } = "None";
        public bool IsBlocking { get; set; }
    }

    public class ResolvePayrollExceptionDto
    {
        public int PayrollExceptionId { get; set; }
        public string Status { get; set; } = null!;
        public string? ResolutionNotes { get; set; }
    }
}
