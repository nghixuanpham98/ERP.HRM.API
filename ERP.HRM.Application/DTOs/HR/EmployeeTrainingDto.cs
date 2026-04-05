namespace ERP.HRM.Application.DTOs.HR
{
    public class EmployeeTrainingDto
    {
        public int EmployeeTrainingId { get; set; }
        public int EmployeeId { get; set; }
        public string TrainingName { get; set; } = null!;
        public string? Provider { get; set; }
        public string? Category { get; set; }
        public decimal DurationHours { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public decimal? GradeObtained { get; set; }
        public bool CertificateObtained { get; set; }
        public string Status { get; set; } = null!;
    }

    public class CreateEmployeeTrainingDto
    {
        public int EmployeeId { get; set; }
        public string TrainingName { get; set; } = null!;
        public string? Provider { get; set; }
        public string? Category { get; set; }
        public decimal DurationHours { get; set; }
        public DateOnly StartDate { get; set; }
        public decimal? TrainingCost { get; set; }
    }

    public class UpdateEmployeeTrainingDto
    {
        public int EmployeeTrainingId { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public decimal? GradeObtained { get; set; }
        public bool? CertificateObtained { get; set; }
        public string? CertificateUrl { get; set; }
        public string? Status { get; set; }
    }
}
