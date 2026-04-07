# 🏗️ Kiến Trúc Hệ Thống - Hướng Dẫn Chi Tiết

## 📊 Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                             │
│  (Web Browser, Mobile App, Desktop Client, Third-party Apps)   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                    HTTPS / HTTP Requests
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                    PRESENTATION LAYER                           │
│                   (ERP.HRM.API Project)                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌────────────────────────────────────────────────────────┐   │
│  │              API Controllers                           │   │
│  ├────────────────────────────────────────────────────────┤   │
│  │ • AuthController              • PayrollController     │   │
│  │ • EmployeesController         • LeaveRequestsCtrl     │   │
│  │ • DepartmentController        • InsuranceCtrl         │   │
│  │ • PositionsController         • PersonnelTransferCtrl │   │
│  └────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌────────────────────────────────────────────────────────┐   │
│  │              Middlewares                               │   │
│  ├────────────────────────────────────────────────────────┤   │
│  │ • GlobalExceptionHandling    • RateLimiting           │   │
│  │ • JWTAuthentication          • AuditLogging           │   │
│  │ • RequestResponseLogging     • CORS                   │   │
│  └────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌────────────────────────────────────────────────────────┐   │
│  │              Cross-Cutting Concerns                    │   │
│  ├────────────────────────────────────────────────────────┤   │
│  │ • HealthChecks    • Caching    • Validation           │   │
│  └────────────────────────────────────────────────────────┘   │
│                                                                 │
└────────────────────────────┬────────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                   APPLICATION LAYER                            │
│              (ERP.HRM.Application Project)                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────────────┐       │
│  │         CQRS Pattern (MediatR)                      │       │
│  ├─────────────────────────────────────────────────────┤       │
│  │                                                     │       │
│  │  Commands          │         Queries                │       │
│  │  ────────          │         ───────                │       │
│  │  CreateEmployee    │  GetAllEmployees              │       │
│  │  UpdateEmployee    │  GetEmployeeById              │       │
│  │  DeleteEmployee    │  GetEmployeesByDept           │       │
│  │  CreateDept        │  GetDepartments               │       │
│  │                    │                               │       │
│  │  Handlers          │  QueryHandlers                │       │
│  │  ────────          │  ──────────────                │       │
│  │  • Validation      │  • Filtering                  │       │
│  │  • Business Logic  │  • Sorting                    │       │
│  │  • Side Effects    │  • Pagination                 │       │
│  │                                                     │       │
│  └─────────────────────────────────────────────────────┘       │
│                                                                 │
│  ┌──────────────────┐  ┌──────────────────┐                    │
│  │   DTOs           │  │   Validators     │                    │
│  ├──────────────────┤  ├──────────────────┤                    │
│  │ • CreateXxxDto   │  │ • FluentValidator│                    │
│  │ • UpdateXxxDto   │  │ • Rules          │                    │
│  │ • XxxDto         │  │ • Error Messages │                    │
│  └──────────────────┘  └──────────────────┘                    │
│                                                                 │
│  ┌──────────────────┐  ┌──────────────────┐                    │
│  │   Mappings       │  │   Services       │                    │
│  ├──────────────────┤  ├──────────────────┤                    │
│  │ • AutoMapper     │  │ • AuthService    │                    │
│  │ • Profiles       │  │ • PayrollService │                    │
│  │ • Conventions    │  │ • EmailService   │                    │
│  └──────────────────┘  └──────────────────┘                    │
│                                                                 │
└────────────────────────────┬────────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                    DOMAIN LAYER                                │
│              (ERP.HRM.Domain Project)                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────────────────────────────────────────────┐           │
│  │           Domain Entities                        │           │
│  ├──────────────────────────────────────────────────┤           │
│  │ • User                • SalaryGrade             │           │
│  │ • Employee            • FamilyDependent         │           │
│  │ • Department          • PayrollRecord           │           │
│  │ • Position            • LeaveRequest            │           │
│  │ • EmploymentContract  • InsuranceParticipation  │           │
│  │ • PayrollPeriod       • PersonnelTransfer       │           │
│  │ • Attendance          • ResignationDecision     │           │
│  └──────────────────────────────────────────────────┘           │
│                                                                 │
│  ┌──────────────────────────────────────────────────┐           │
│  │       Repository Interfaces (Contracts)          │           │
│  ├──────────────────────────────────────────────────┤           │
│  │ • IEmployeeRepository      • IPayrollRepository │           │
│  │ • IDepartmentRepository    • ILeaveRepository   │           │
│  │ • IPositionRepository      • IUserRepository    │           │
│  │ • IUnitOfWork              • [More...]          │           │
│  └──────────────────────────────────────────────────┘           │
│                                                                 │
│  ┌──────────────────────────────────────────────────┐           │
│  │    Enums & Value Objects                         │           │
│  ├──────────────────────────────────────────────────┤           │
│  │ • EmployeeStatus      • PayrollStatus          │           │
│  │ • LeaveType           • ContractType           │           │
│  │ • InsuranceType       • PositionLevel          │           │
│  └──────────────────────────────────────────────────┘           │
│                                                                 │
└────────────────────────────┬────────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────────┐
│               INFRASTRUCTURE LAYER                             │
│            (ERP.HRM.Infrastructure Project)                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌────────────────────────────────────────────┐                 │
│  │      Data Access (Entity Framework Core)   │                 │
│  ├────────────────────────────────────────────┤                 │
│  │ • ERPDbContext                             │                 │
│  │ • DbSet definitions                        │                 │
│  │ • Entity Configurations                    │                 │
│  │ • Migrations                               │                 │
│  └────────────────────────────────────────────┘                 │
│                                                                 │
│  ┌────────────────────────────────────────────┐                 │
│  │    Repository Implementations              │                 │
│  ├────────────────────────────────────────────┤                 │
│  │ • EmployeeRepository                       │                 │
│  │ • DepartmentRepository                     │                 │
│  │ • PayrollRecordRepository                  │                 │
│  │ • GenericRepository<T>                     │                 │
│  └────────────────────────────────────────────┘                 │
│                                                                 │
│  ┌────────────────────────────────────────────┐                 │
│  │    Unit of Work Pattern                    │                 │
│  ├────────────────────────────────────────────┤                 │
│  │ • Transactional Control                    │                 │
│  │ • Repository Coordination                  │                 │
│  │ • SaveChanges Orchestration                │                 │
│  └────────────────────────────────────────────┘                 │
│                                                                 │
│  ┌────────────────────────────────────────────┐                 │
│  │    Database Seeding                        │                 │
│  ├────────────────────────────────────────────┤                 │
│  │ • DataSeeder                               │                 │
│  │ • Initial Data Setup                       │                 │
│  └────────────────────────────────────────────┘                 │
│                                                                 │
└────────────────────────────┬────────────────────────────────────┘
                             │
                   SQL Queries / Commands
                             │
┌────────────────────────────▼────────────────────────────────────┐
│                    DATABASE LAYER                              │
│              (SQL Server 2019+)                                │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────┐               │
│  │   Organization Module                       │               │
│  │   ─────────────────                         │               │
│  │   • Departments    • Positions              │               │
│  │   • Employees      • User/Identity          │               │
│  └─────────────────────────────────────────────┘               │
│                                                                 │
│  ┌─────────────────────────────────────────────┐               │
│  │   HR Management Module                      │               │
│  │   ──────────────────                        │               │
│  │   • EmploymentContracts                     │               │
│  │   • SalaryGrades        • FamilyDependents  │               │
│  │   • EmployeeRecords     • LeaveRequests     │               │
│  │   • PersonnelTransfers  • ResignationDecisions              │
│  └─────────────────────────────────────────────┘               │
│                                                                 │
│  ┌─────────────────────────────────────────────┐               │
│  │   Payroll Module                            │               │
│  │   ───────────────                           │               │
│  │   • PayrollPeriods      • PayrollRecords    │               │
│  │   • PayrollDeductions   • SalaryConfigs     │               │
│  │   • TaxBrackets         • SalaryComponents  │               │
│  │   • Attendance          • ProductionOutput  │               │
│  └─────────────────────────────────────────────┘               │
│                                                                 │
│  ┌─────────────────────────────────────────────┐               │
│  │   Insurance Module                          │               │
│  │   ──────────────                            │               │
│  │   • InsuranceTiers                          │               │
│  │   • InsuranceParticipations                 │               │
│  │   • TaxExemptions                           │               │
│  └─────────────────────────────────────────────┘               │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request Processing Flow

```
1. HTTP Request arrives
   ↓
2. ASP.NET Core Pipeline
   ├─ CORS Middleware
   ├─ Authentication Middleware (JWT)
   ├─ Authorization Middleware
   ├─ Request/Response Logging
   └─ Rate Limiting
   ↓
3. Routing
   └─ URL → Controller Action
   ↓
4. Controller Action
   ├─ Parse Request Body
   ├─ Model Binding
   └─ Create Command/Query
   ↓
5. FluentValidation
   ├─ Validate DTO
   └─ Return errors if invalid
   ↓
6. MediatR Dispatch
   ├─ Route to Handler
   └─ Execute Handler
   ↓
7. Handler Execution
   ├─ Apply Business Logic
   ├─ Access Repository/UnitOfWork
   ├─ Database Operations
   └─ Return Result
   ↓
8. AutoMapper
   └─ Map Entity → DTO
   ↓
9. Response Serialization
   ├─ Convert to JSON
   └─ Add Headers
   ↓
10. HTTP Response
    └─ Status Code + Body
```

---

## 🗂️ Feature Structure (Per Module)

### Standard Feature Organization

```
Features/
├── [FeatureName]/
│   ├── Commands/
│   │   ├── Create[Feature]Command.cs
│   │   ├── Update[Feature]Command.cs
│   │   └── Delete[Feature]Command.cs
│   │
│   ├── Queries/
│   │   ├── GetAll[Features]Query.cs
│   │   ├── GetBy[Feature]Query.cs
│   │   └── [Feature]SearchQuery.cs
│   │
│   ├── Handlers/
│   │   ├── Commands/
│   │   │   ├── Create[Feature]Handler.cs
│   │   │   ├── Update[Feature]Handler.cs
│   │   │   └── Delete[Feature]Handler.cs
│   │   │
│   │   └── Queries/
│   │       ├── GetAll[Features]Handler.cs
│   │       ├── GetBy[Feature]Handler.cs
│   │       └── [Feature]SearchHandler.cs
│   │
│   └── Events/ (Optional - for Event Sourcing)
│       ├── [Feature]CreatedEvent.cs
│       └── [Feature]UpdatedEvent.cs
```

### Example: Employee Feature

```
Features/
└── Employees/
    ├── Commands/
    │   ├── CreateEmployeeCommand.cs
    │   ├── UpdateEmployeeCommand.cs
    │   ├── DeleteEmployeeCommand.cs
    │   └── AssignDepartmentCommand.cs
    │
    ├── Queries/
    │   ├── GetAllEmployeesQuery.cs
    │   ├── GetEmployeeByIdQuery.cs
    │   ├── GetEmployeesByDepartmentQuery.cs
    │   ├── SearchEmployeesQuery.cs
    │   └── GetEmployeeDetailsQuery.cs
    │
    └── Handlers/
        ├── Commands/
        │   ├── CreateEmployeeHandler.cs
        │   ├── UpdateEmployeeHandler.cs
        │   ├── DeleteEmployeeHandler.cs
        │   └── AssignDepartmentHandler.cs
        │
        └── Queries/
            ├── GetAllEmployeesHandler.cs
            ├── GetEmployeeByIdHandler.cs
            ├── GetEmployeesByDepartmentHandler.cs
            ├── SearchEmployeesHandler.cs
            └── GetEmployeeDetailsHandler.cs
```

---

## 🔌 Dependency Injection Container

### Service Registration in Program.cs

```csharp
// Phase 1: Database & Identity
builder.Services.AddDbContext<ERPDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ERPDbContext>();

// Phase 2: JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => { /* JWT config */ });

// Phase 3: Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeValidator).Assembly);

// Phase 4: MediatR (CQRS)
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly));

// Phase 5: AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Phase 6: Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
// ... more repositories

// Phase 7: Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Phase 8: Business Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
// ... more services

// Phase 9: Health Checks
builder.Services.AddHealthChecks();

// Phase 10: Swagger/OpenAPI
builder.Services.AddSwaggerGen();

// Phase 11: Logging
builder.Host.UseSerilog();

// Phase 12: CORS
builder.Services.AddCors(options => { /* CORS config */ });
```

---

## 📊 Entity Relationship Diagram (Simplified)

```
┌─────────────────────┐
│   Organization      │
├─────────────────────┤
│ User (Identity)     │◄──────────┐
│ Department          │           │
│ Position            │           │
└──────────┬──────────┘           │
           │ 1:*                  │
           │                      │ 1:1
┌──────────▼──────────┐           │
│   HR Management     │           │
├─────────────────────┤           │
│ Employee            ├───────────┘
│ EmploymentContract  │
│ SalaryGrade         │
│ FamilyDependent     │
│ EmployeeRecord      │
│ PersonnelTransfer   │
│ ResignationDecision │
└──────────┬──────────┘
           │ 1:*
┌──────────▼──────────┐
│   Payroll           │
├─────────────────────┤
│ PayrollPeriod       │
│ PayrollRecord       │
│ PayrollDeduction    │
│ SalaryComponent     │
│ Attendance          │
│ ProductionOutput    │
└─────────────────────┘
```

---

## 🔐 Security Architecture

```
┌─────────────────────────────────────────┐
│       Client Application                │
└──────────────────┬──────────────────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  TLS/SSL (HTTPS)     │
        │ End-to-End Encryption│
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  API Gateway         │
        │ - Rate Limiting      │
        │ - Request Validation │
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  Authentication      │
        │  JWT Verification    │
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  Authorization       │
        │  RBAC/Claims Check   │
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  Business Logic      │
        │  with Validation     │
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │  Database            │
        │ - Connection Pool    │
        │ - Parameterized SQL  │
        │ - Row-Level Security │
        └──────────────────────┘
```

---

## 📈 Data Flow for Common Operations

### Employee Creation Flow

```
1. POST /api/employees
   ├─ Request: CreateEmployeeDto
   │
2. Model Validation
   ├─ FluentValidation rules
   ├─ Email uniqueness check
   └─ Data format validation
   │
3. CreateEmployeeCommand created
   │
4. MediatR dispatches to handler
   │
5. CreateEmployeeHandler
   ├─ Validate business rules
   ├─ Check department exists
   ├─ Check position exists
   ├─ Map DTO → Entity
   │
6. Repository.AddAsync(Employee)
   ├─ Entity added to DbContext
   │
7. UnitOfWork.SaveChangesAsync()
   ├─ SQL INSERT executed
   ├─ DB generates ID
   │
8. Map Entity → EmployeeDto
   │
9. HTTP 201 Created
   └─ Response: EmployeeDto
```

### Payroll Calculation Flow

```
1. POST /api/payroll/calculate
   ├─ Request: PayrollCalculationRequest
   │
2. Validation
   ├─ Check payroll period exists
   ├─ Check employees in period
   │
3. For each employee:
   │
   a) Get BasicSalary
   │
   b) Calculate Allowances
   │  ├─ Housing allowance
   │  ├─ Transportation
   │  └─ Other allowances
   │
   c) Get Attendance
   │  ├─ Days worked
   │  ├─ Overtime hours
   │
   d) Calculate Deductions
   │  ├─ Health insurance
   │  ├─ Social insurance
   │  ├─ Tax withholding
   │  └─ Personal deductions
   │
   e) Calculate GrossSalary
   │  └─ BaseSalary + Allowances
   │
   f) Calculate NetSalary
   │  └─ GrossSalary - Deductions
   │
   g) Create/Update PayrollRecord
   │
4. Save all PayrollRecords
   │
5. Publish PayrollCalculatedEvent
   │
6. Return PayrollSummaryDto
```

---

## 🧪 Testing Architecture

```
┌─────────────────────────────────────────┐
│     Test Pyramid                        │
├─────────────────────────────────────────┤
│                                         │
│              ▲                          │
│             / \                         │
│            /   \                        │
│           /  E2E \ (10%)                │
│          /________\                     │
│         /          \                    │
│        /            \                   │
│       /  Integration \ (30%)            │
│      /________________\                 │
│     /                  \                │
│    /                    \               │
│   /      Unit Tests      \ (60%)        │
│  /__________________________\            │
│                                         │
└─────────────────────────────────────────┘

Unit Tests
├─ Service Tests
├─ Repository Tests
├─ Validator Tests
└─ Handler Tests

Integration Tests
├─ Database Tests
├─ API Route Tests
├─ Middleware Tests
└─ DI Container Tests

E2E Tests
├─ Complete Workflows
├─ API Contract Tests
└─ Performance Tests
```

### Test Structure

```
tests/
└── ERP.HRM.Application.Tests/
    ├── Unit/
    │   ├── Services/
    │   ├── Repositories/
    │   ├── Validators/
    │   └── Handlers/
    │
    ├── Integration/
    │   ├── Database/
    │   └── API/
    │
    └── Fixtures/
        ├── MockRepositories.cs
        ├── TestDatabaseFixture.cs
        └── TestData.cs
```

---

## 🚀 Deployment Architecture

```
┌──────────────────────────────────────────────┐
│        Load Balancer / Reverse Proxy         │
│        (nginx / IIS / Azure Front Door)      │
└────────────┬─────────────────────────────────┘
             │
    ┌────────┼────────┐
    │        │        │
    ▼        ▼        ▼
┌────────┐┌────────┐┌────────┐
│ App 1  ││ App 2  ││ App 3  │  API Instances
└──┬─────┘└──┬─────┘└──┬─────┘
   │         │         │
   └────────┬┴────────┬┘
            │
    ┌───────▼────────┐
    │  Connection    │
    │  Pool          │
    └───────┬────────┘
            │
    ┌───────▼────────────┐
    │  Database Cluster  │
    ├────────────────────┤
    │  Primary (Write)   │
    │  Replica 1 (Read)  │
    │  Replica 2 (Read)  │
    │  Backup            │
    └────────────────────┘
```

---

## 📚 Knowledge Base Structure

```
Documentation/
├── Architecture/
│   ├── System Design
│   ├── Data Flow
│   └── Security Model
│
├── Development/
│   ├── Coding Standards
│   ├── Feature Development
│   ├── Testing Guide
│   └── Git Workflow
│
├── Deployment/
│   ├── Environment Setup
│   ├── CI/CD Pipeline
│   ├── Monitoring
│   └── Disaster Recovery
│
├── Operations/
│   ├── Backup & Restore
│   ├── Performance Tuning
│   ├── Security Patching
│   └── Troubleshooting
│
└── API/
    ├── Endpoint Reference
    ├── Request/Response Examples
    ├── Error Handling
    └── Rate Limiting
```

---

## 🎯 Development Workflow

```
1. Feature Request / Bug Report
   ↓
2. Create Feature Branch
   └─ git checkout -b feature/employee-update
   ↓
3. Develop Feature
   ├─ Write domain entity
   ├─ Create repository interface/implementation
   ├─ Add database migrations
   ├─ Create DTOs
   ├─ Create validators
   ├─ Create CQRS commands/queries
   ├─ Create handlers
   ├─ Create controller endpoints
   └─ Write unit tests
   ↓
4. Verify Quality
   ├─ Run tests: dotnet test
   ├─ Check code: dotnet build
   ├─ Format code: dotnet format
   └─ Code review
   ↓
5. Commit & Push
   └─ git commit -m "feat: add employee update feature"
   ├─ git push origin feature/employee-update
   ↓
6. Create Pull Request
   ├─ Add description
   ├─ Link to issue
   ├─ Assign reviewers
   ↓
7. Code Review
   ├─ Fix feedback
   ├─ Update PR
   ↓
8. Merge
   └─ Squash & merge to main
   ↓
9. Deploy
   ├─ To staging environment
   ├─ Run E2E tests
   ├─ To production
   └─ Monitor
```

---

## 📝 File Locations Quick Reference

| Feature | Main Files | Location |
|---------|-----------|----------|
| API Endpoint | Controller | `ERP.HRM.API/Controllers/` |
| Business Logic | Handler | `ERP.HRM.Application/Features/*/Handlers/` |
| Data Model | Entity | `ERP.HRM.Domain/Entities/` |
| Data Access | Repository | `ERP.HRM.Infrastructure/Repositories/` |
| Database | DbContext | `ERP.HRM.Infrastructure/Persistence/` |
| Migration | Migration | `ERP.HRM.Infrastructure/Migrations/` |
| DTO | Data Transfer Object | `ERP.HRM.Application/DTOs/` |
| Validation | Validator | `ERP.HRM.Application/Validators/` |
| Configuration | Config Class | `ERP.HRM.API/Configuration/` |
| Middleware | Middleware | `ERP.HRM.API/Middlewares/` |

---

**Cập nhật lần cuối:** 2024  
**Phiên bản:** 1.0
