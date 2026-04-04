using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.HRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionHierarchyAndSalaryComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create ProductionStages table
            migrationBuilder.CreateTable(
                name: "ProductionStages",
                columns: table => new
                {
                    ProductionStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StageCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    EstimatedHours = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionStages", x => x.ProductionStageId);
                    table.ForeignKey(
                        name: "FK_ProductionStages_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                });

            // Create ProductionJobs table
            migrationBuilder.CreateTable(
                name: "ProductionJobs",
                columns: table => new
                {
                    ProductionJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionStageId = table.Column<int>(type: "int", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    JobCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplexityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Medium"),
                    ComplexityMultiplier = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 1.0m),
                    EstimatedTimePerUnit = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionJobs", x => x.ProductionJobId);
                    table.ForeignKey(
                        name: "FK_ProductionJobs_ProductionStages_ProductionStageId",
                        column: x => x.ProductionStageId,
                        principalTable: "ProductionStages",
                        principalColumn: "ProductionStageId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create JobProductPricings table
            migrationBuilder.CreateTable(
                name: "JobProductPricings",
                columns: table => new
                {
                    JobProductPricingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionJobId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BaseUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    QualityStandard = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProductPricings", x => x.JobProductPricingId);
                    table.UniqueConstraint("UK_JobProductPricings_JobProduct", x => new { x.ProductionJobId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_JobProductPricings_ProductionJobs_ProductionJobId",
                        column: x => x.ProductionJobId,
                        principalTable: "ProductionJobs",
                        principalColumn: "ProductionJobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobProductPricings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create ProductionOutputV2 table
            migrationBuilder.CreateTable(
                name: "ProductionOutputV2",
                columns: table => new
                {
                    ProductionOutputV2Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayrollPeriodId = table.Column<int>(type: "int", nullable: false),
                    ProductionStageId = table.Column<int>(type: "int", nullable: false),
                    ProductionJobId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AppliedUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JobComplexityMultiplier = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 1.0m),
                    WorkerSkillMultiplier = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 1.0m),
                    ProductionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QualityStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "OK"),
                    QualityAdjustmentPercentage = table.Column<decimal>(type: "decimal(5,3)", nullable: false, defaultValue: 1.0m),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ApprovedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOutputV2", x => x.ProductionOutputV2Id);
                    table.ForeignKey(
                        name: "FK_ProductionOutputV2_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputV2_PayrollPeriods_PayrollPeriodId",
                        column: x => x.PayrollPeriodId,
                        principalTable: "PayrollPeriods",
                        principalColumn: "PayrollPeriodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputV2_ProductionStages_ProductionStageId",
                        column: x => x.ProductionStageId,
                        principalTable: "ProductionStages",
                        principalColumn: "ProductionStageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputV2_ProductionJobs_ProductionJobId",
                        column: x => x.ProductionJobId,
                        principalTable: "ProductionJobs",
                        principalColumn: "ProductionJobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionOutputV2_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create SalaryComponents table
            migrationBuilder.CreateTable(
                name: "SalaryComponents",
                columns: table => new
                {
                    SalaryComponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ComponentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EffectiveStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EffectiveEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ApplicablePeriod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Monthly"),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    ApprovedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryComponents", x => x.SalaryComponentId);
                    table.ForeignKey(
                        name: "FK_SalaryComponents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_ProductionStages_StageCode",
                table: "ProductionStages",
                column: "StageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionStages_DepartmentId",
                table: "ProductionStages",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionJobs_JobCode",
                table: "ProductionJobs",
                column: "JobCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionJobs_ProductionStageId",
                table: "ProductionJobs",
                column: "ProductionStageId");

            migrationBuilder.CreateIndex(
                name: "IX_JobProductPricings_ProductId",
                table: "JobProductPricings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputV2_EmployeeId_PayrollPeriodId",
                table: "ProductionOutputV2",
                columns: new[] { "EmployeeId", "PayrollPeriodId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputV2_ProductionJobId",
                table: "ProductionOutputV2",
                column: "ProductionJobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOutputV2_ProductionDate",
                table: "ProductionOutputV2",
                column: "ProductionDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_EmployeeId",
                table: "SalaryComponents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_ComponentType",
                table: "SalaryComponents",
                column: "ComponentType");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryComponents_ApprovalStatus",
                table: "SalaryComponents",
                column: "ApprovalStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SalaryComponents");
            migrationBuilder.DropTable(name: "ProductionOutputV2");
            migrationBuilder.DropTable(name: "JobProductPricings");
            migrationBuilder.DropTable(name: "ProductionJobs");
            migrationBuilder.DropTable(name: "ProductionStages");
        }
    }
}
