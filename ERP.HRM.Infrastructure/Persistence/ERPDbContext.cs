using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERP.HRM.API
{
    public partial class ERPDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ERPDbContext()
        {
        }

        public ERPDbContext(DbContextOptions<ERPDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public virtual DbSet<SalaryConfiguration> SalaryConfigurations { get; set; }

        public virtual DbSet<PayrollPeriod> PayrollPeriods { get; set; }

        public virtual DbSet<Attendance> Attendances { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductionOutput> ProductionOutputs { get; set; }

        public virtual DbSet<PayrollRecord> PayrollRecords { get; set; }

        public virtual DbSet<PayrollDeduction> PayrollDeductions { get; set; }

        public virtual DbSet<EmploymentContract> EmploymentContracts { get; set; }

        public virtual DbSet<SalaryGrade> SalaryGrades { get; set; }

        public virtual DbSet<FamilyDependent> FamilyDependents { get; set; }

        public virtual DbSet<SalaryAdjustmentDecision> SalaryAdjustmentDecisions { get; set; }

        public virtual DbSet<TaxBracket> TaxBrackets { get; set; }

        public virtual DbSet<InsuranceTier> InsuranceTiers { get; set; }

        public virtual DbSet<EmployeeRecord> EmployeeRecords { get; set; }

        public virtual DbSet<InsuranceParticipation> InsuranceParticipations { get; set; }

        public virtual DbSet<PersonnelTransfer> PersonnelTransfers { get; set; }

        public virtual DbSet<ResignationDecision> ResignationDecisions { get; set; }

        public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

        public virtual DbSet<ProductionStage> ProductionStages { get; set; }

        public virtual DbSet<ProductionJob> ProductionJobs { get; set; }

        public virtual DbSet<JobProductPricing> JobProductPricings { get; set; }

        public virtual DbSet<ProductionOutputV2> ProductionOutputV2 { get; set; }

        public virtual DbSet<SalaryComponent> SalaryComponents { get; set; }

        // New DbSets for enhanced HR and Payroll features
        public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }

        public virtual DbSet<LeaveApprovalWorkflow> LeaveApprovalWorkflows { get; set; }

        public virtual DbSet<ContractHistory> ContractHistories { get; set; }

        public virtual DbSet<SalaryComponentType> SalaryComponentTypes { get; set; }

        public virtual DbSet<SalaryComponentValue> SalaryComponentValues { get; set; }

        public virtual DbSet<SalaryHistory> SalaryHistories { get; set; }

        public virtual DbSet<TaxExemption> TaxExemptions { get; set; }

        public virtual DbSet<CumulativeTaxRecord> CumulativeTaxRecords { get; set; }

        public virtual DbSet<PayrollException> PayrollExceptions { get; set; }

        public virtual DbSet<ProductionInspection> ProductionInspections { get; set; }

        public virtual DbSet<EmployeeCertification> EmployeeCertifications { get; set; }

        public virtual DbSet<EmployeeTraining> EmployeeTrainings { get; set; }

        public virtual DbSet<EmployeeSkillMatrix> EmployeeSkillMatrices { get; set; }

        public virtual DbSet<PerformanceAppraisal> PerformanceAppraisals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BED6739090C");

                entity.HasIndex(e => e.DepartmentCode, "UQ__Departme__6EA8896DA18388CE").IsUnique();

                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.DepartmentCode)
                    .HasMaxLength(50)
                    .HasDefaultValue("DPT");
                entity.Property(e => e.DepartmentName).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.ModifiedBy).HasMaxLength(100);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F11318FD90B");

                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Allowance)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.BaseSalary)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.ContractType).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.EmployeeCode).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.JobTitle).HasMaxLength(100);
                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.ModifiedBy).HasMaxLength(100);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NationalId).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employees_Departments");

                entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK_Employees_Manager");

                entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Employees_Positions");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId).HasName("PK__Position__60BB9A7967EFCAD1");

                entity.HasIndex(e => e.PositionCode, "UQ__Position__83745B02724EC467").IsUnique();

                entity.Property(e => e.Allowance)
                    .HasDefaultValue(0m)
                    .HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Level).HasDefaultValue(1);
                entity.Property(e => e.ModifiedBy).HasMaxLength(100);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.PositionCode)
                    .HasMaxLength(50)
                    .HasDefaultValue("POS");
                entity.Property(e => e.PositionName).HasMaxLength(100);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC078EACC68D");

                entity.HasIndex(e => e.UserName, "UQ__Users__536C85E4BDC76937").IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.RefreshToken).HasMaxLength(255);
                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            // Configure Payroll Entities
            modelBuilder.Entity<PayrollPeriod>(entity =>
            {
                entity.HasKey(e => e.PayrollPeriodId);

                entity.Property(e => e.PeriodName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.FinalizedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendanceId);

                entity.Property(e => e.AttendanceDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.WorkingDays)
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.OvertimeHours)
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.OvertimeMultiplier)
                    .HasColumnType("decimal(3, 2)")
                    .HasDefaultValue(1.5m);

                entity.Property(e => e.Note)
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Attendances_Employees");

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany()
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_Attendances_PayrollPeriods");

                entity.HasIndex(e => new { e.EmployeeId, e.PayrollPeriodId, e.AttendanceDate })
                    .IsUnique()
                    .HasDatabaseName("UQ_Attendance_EmployeePeriodDate");
            });

            modelBuilder.Entity<SalaryConfiguration>(entity =>
            {
                entity.HasKey(e => e.SalaryConfigurationId);

                entity.Property(e => e.SalaryType)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.BaseSalary)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValue(0m);

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HourlyRate)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Allowance)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InsuranceRate)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValue(8m);

                entity.Property(e => e.TaxRate)
                    .HasColumnType("decimal(5, 2)")
                    .HasDefaultValue(5m);

                entity.Property(e => e.EffectiveFrom)
                    .HasColumnType("datetime");

                entity.Property(e => e.EffectiveTo)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_SalaryConfigurations_Employees");
            });

            modelBuilder.Entity<PayrollRecord>(entity =>
            {
                entity.HasKey(e => e.PayrollRecordId);

                entity.Property(e => e.SalaryType)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.BaseSalary)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Allowance)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OvertimeCompensation)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GrossSalary)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InsuranceDeduction)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxDeduction)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.OtherDeductions)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalDeductions)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NetSalary)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WorkingDays)
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ProductionTotal)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Draft");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_PayrollRecords_Employees");

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany()
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_PayrollRecords_PayrollPeriods");

                entity.HasIndex(e => new { e.EmployeeId, e.PayrollPeriodId })
                    .IsUnique()
                    .HasDatabaseName("UQ_PayrollRecord_EmployeePeriod");
            });

            modelBuilder.Entity<PayrollDeduction>(entity =>
            {
                entity.HasKey(e => e.PayrollDeductionId);

                entity.Property(e => e.DeductionType)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reason)
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.PayrollRecord)
                    .WithMany(p => p.Deductions)
                    .HasForeignKey(d => d.PayrollRecordId)
                    .HasConstraintName("FK_PayrollDeductions_PayrollRecords");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.Unit)
                    .HasMaxLength(20)
                    .HasDefaultValue("cái");

                entity.Property(e => e.Category)
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);
            });

            modelBuilder.Entity<ProductionOutput>(entity =>
            {
                entity.HasKey(e => e.ProductionOutputId);

                entity.Property(e => e.Quantity)
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductionDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.QualityStatus)
                    .HasMaxLength(50)
                    .HasDefaultValue("OK");

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasOne(d => d.Employee)
                    .WithMany()
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_ProductionOutputs_Employees");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductionOutputs_Products");

                entity.HasOne(d => d.PayrollPeriod)
                    .WithMany()
                    .HasForeignKey(d => d.PayrollPeriodId)
                    .HasConstraintName("FK_ProductionOutputs_PayrollPeriods");

                entity.HasIndex(e => new { e.EmployeeId, e.PayrollPeriodId })
                    .HasDatabaseName("IDX_ProductionOutput_EmployeePeriod");
            });

            OnModelCreatingPartial(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}