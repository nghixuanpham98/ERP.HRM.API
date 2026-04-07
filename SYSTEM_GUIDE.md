# 📘 Hướng Dẫn Toàn Hệ Thống ERP.HRM.API

**Phiên bản:** 1.0  
**Ngôn ngữ:** C# .NET 8  
**Kiến trúc:** Clean Architecture + CQRS + MediatR  
**Cơ sở dữ liệu:** SQL Server  
**Repository:** https://github.com/nghixuanpham98/ERP.HRM.API

---

## 📑 Mục Lục

1. [Tổng Quan Hệ Thống](#tổng-quan-hệ-thống)
2. [Cấu Trúc Dự Án](#cấu-trúc-dự-án)
3. [Kiến Trúc Ứng Dụng](#kiến-trúc-ứng-dụng)
4. [Hướng Dẫn Cài Đặt](#hướng-dẫn-cài-đặt)
5. [Các Modules Chính](#các-modules-chính)
6. [Hướng Dẫn Phát Triển](#hướng-dẫn-phát-triển)
7. [API Endpoints](#api-endpoints)
8. [Quản Lý Cơ Sở Dữ Liệu](#quản-lý-cơ-sở-dữ-liệu)
9. [Bảo Mật & Xác Thực](#bảo-mật--xác-thực)
10. [Logging & Monitoring](#logging--monitoring)
11. [Testing](#testing)
12. [Deployment](#deployment)
13. [Troubleshooting](#troubleshooting)

---

## 🎯 Tổng Quan Hệ Thống

### Mục Đích
**ERP.HRM.API** là một hệ thống quản lý nhân sự (Human Resource Management) toàn diện được xây dựng trên nền tảng .NET 8. Hệ thống cung cấp các chức năng:

- 👥 **Quản lý nhân viên** (Employees)
- 🏢 **Quản lý bộ phận** (Departments)
- 📝 **Hợp đồng lao động** (Employment Contracts)
- 💰 **Quản lý lương & bảng lương** (Payroll Management)
- 📊 **Báo cáo & thống kê** (Reporting & Analytics)
- 🏥 **Quản lý bảo hiểm** (Insurance Management)
- 🎓 **Quản lý phát triển nhân viên** (Employee Development)
- 📅 **Quản lý nghỉ phép** (Leave Management)
- 🔄 **Chuyên cấp nhân viên** (Personnel Transfer)
- 💼 **Quản lý việc làm sản xuất** (Production Management)

### Công Nghệ Chính
```
Backend:        .NET 8, C#
Framework:      ASP.NET Core 8
Architecture:   Clean Architecture + CQRS + MediatR
Database:       SQL Server (EF Core 8)
Authentication: JWT + ASP.NET Identity
Validation:     FluentValidation
Logging:        Serilog
API Docs:       Swagger/OpenAPI
Testing:        xUnit, Moq
```

### Phiên Bản Hiện Tại
- **Giai đoạn 4:** Payroll Export Service (Xuất dữ liệu lương)
- **Tính năng mới:**
  - Export lương định kỳ
  - Tích hợp bảo hiểm
  - Quản lý chuyên cấp
  - Quản lý từ chức

---

## 📁 Cấu Trúc Dự Án

### Tổng Quan Các Project

```
ERP.HRM.API/
├── ERP.HRM.API/                    # Web API Project (Presentation Layer)
├── ERP.HRM.Application/            # Business Logic Layer
├── ERP.HRM.Domain/                 # Domain Layer (Entities & Interfaces)
├── ERP.HRM.Infrastructure/         # Data Access Layer
├── tests/
│   └── ERP.HRM.Application.Tests/  # Unit Tests
└── docs/                           # Documentation
```

### Chi Tiết Cấu Trúc

#### 1. **ERP.HRM.API** (Presentation Layer)
```
ERP.HRM.API/
├── Controllers/                 # API Controllers
│   ├── AuthController.cs        # Xác thực & đăng nhập
│   ├── EmployeesController.cs   # Quản lý nhân viên
│   ├── DepartmentController.cs  # Quản lý bộ phận
│   ├── PayrollController.cs     # Quản lý lương
│   ├── PayrollExportController.cs # Export lương
│   ├── LeaveRequestsController.cs # Quản lý nghỉ phép
│   ├── InsuranceManagementController.cs
│   └── [Các controller khác...]
├── Configuration/               # Cấu hình ứng dụng
│   ├── CorsConfiguration.cs     # CORS Settings
│   └── ApiVersionConstants.cs   # API Version
├── Middlewares/                 # Middleware
│   ├── GlobalException.cs       # Xử lý ngoại lệ toàn cục
│   ├── RateLimitingMiddleware.cs
│   ├── AuditLoggingMiddleware.cs
│   └── RequestResponseLoggingMiddleware.cs
├── HealthChecks/
│   └── DatabaseHealthCheck.cs   # Kiểm tra sức khỏe DB
├── Attributes/
│   └── CachingAttributes.cs     # Caching decorator
├── Program.cs                   # Startup & DI Configuration
├── appsettings.json            # Cấu hình chung
└── appsettings.Development.json # Cấu hình Dev
```

#### 2. **ERP.HRM.Application** (Business Logic Layer)
```
ERP.HRM.Application/
├── Features/                    # CQRS Features
│   ├── Departments/
│   │   ├── Commands/           # Commands (Tạo, Cập nhật, Xóa)
│   │   ├── Queries/            # Queries (Lấy dữ liệu)
│   │   └── Handlers/           # Command & Query Handlers
│   ├── Employees/
│   ├── Payroll/
│   ├── LeaveRequests/
│   └── [Các features khác...]
├── DTOs/                        # Data Transfer Objects
│   ├── CreateDepartmentDto.cs
│   ├── EmployeeDto.cs
│   ├── PayrollExportDto.cs
│   └── [DTOs khác...]
├── Validators/                  # FluentValidation
│   ├── CreateEmployeeValidator.cs
│   └── [Validators khác...]
├── Mappings/                    # AutoMapper Configuration
│   └── MappingProfile.cs
├── Services/                    # Business Services
│   ├── AuthService.cs
│   ├── PayrollService.cs
│   └── [Services khác...]
├── Interfaces/                  # Contracts
│   ├── IAuthService.cs
│   └── [Interfaces khác...]
└── Extensions/                  # Extension Methods
    └── StringValidationExtensions.cs
```

#### 3. **ERP.HRM.Domain** (Domain Layer)
```
ERP.HRM.Domain/
├── Entities/                    # Domain Models
│   ├── User.cs                 # User Entity
│   ├── Employee.cs             # Employee Entity
│   ├── Department.cs           # Department Entity
│   ├── PayrollRecord.cs        # Payroll Entity
│   ├── SalaryConfiguration.cs  # Salary Config Entity
│   └── [Entities khác...]
├── Interfaces/
│   └── Repositories/           # Repository Contracts
│       ├── IEmployeeRepository.cs
│       ├── IPayrollRecordRepository.cs
│       └── [Interfaces khác...]
├── Enums/                      # Enumerations
│   ├── EmployeeStatus.cs
│   ├── LeaveType.cs
│   └── [Enums khác...]
└── Common/                     # Common Classes
    └── BaseEntity.cs           # Base class cho entities
```

#### 4. **ERP.HRM.Infrastructure** (Data Access Layer)
```
ERP.HRM.Infrastructure/
├── Persistence/
│   ├── ERPDbContext.cs         # DbContext chính
│   └── [DbContext configs...]
├── Repositories/               # Repository Implementations
│   ├── DepartmentRepository.cs
│   ├── EmployeeRepository.cs
│   ├── PayrollRecordRepository.cs
│   └── [Repositories khác...]
├── UnitOfWork/                # Unit of Work Pattern
│   └── UnitOfWork.cs
├── Migrations/                # Database Migrations
│   ├── 20240101000000_Initial.cs
│   ├── 20240115000000_AddPayroll.cs
│   └── [Migrations khác...]
├── Seed/                      # Database Seeding
│   └── DataSeeder.cs
└── Configuration/             # EF Configuration
    └── EntityConfigurations/
        ├── EmployeeConfiguration.cs
        └── [Configs khác...]
```

---

## 🏗️ Kiến Trúc Ứng Dụng

### Clean Architecture Layers

```
┌─────────────────────────────────────────┐
│      API Layer (Controllers)            │ ← ERP.HRM.API
│  Handles HTTP requests/responses        │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│    Application Layer (CQRS/MediatR)     │ ← ERP.HRM.Application
│  Business Logic, Commands, Queries      │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│    Domain Layer (Entities/Interfaces)   │ ← ERP.HRM.Domain
│  Business Rules, Domain Models          │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  Infrastructure Layer (Data Access)     │ ← ERP.HRM.Infrastructure
│  Repositories, DbContext, Migrations    │
└─────────────────────────────────────────┘
```

### CQRS + MediatR Pattern

```
HTTP Request
    ↓
Controller
    ↓
MediatR
    ├─ Command/Query
    ↓
Handler
    ├─ Validation
    ├─ Business Logic
    ├─ Database Operations
    ↓
Response
    ↓
HTTP Response
```

**Ví dụ Flow:**
1. User gửi request tạo nhân viên → `POST /api/employees`
2. Controller nhận request → tạo `CreateEmployeeCommand`
3. MediatR gửi command đến handler
4. Handler validate → gọi service → lưu DB
5. Return `EmployeeDto` → API trả response

### Dependency Injection

```csharp
// Program.cs
// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPayrollRecordRepository, PayrollRecordRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(
        typeof(CreateEmployeeCommand).Assembly));
```

---

## 🚀 Hướng Dẫn Cài Đặt

### Yêu Cầu Hệ Thống
- **OS:** Windows / Linux / macOS
- **Runtime:** .NET 8 SDK
- **Database:** SQL Server 2019+
- **IDE:** Visual Studio 2022+ hoặc VS Code
- **Git:** Để clone repository

### Bước 1: Cài Đặt Môi Trường

#### Cài Đặt .NET 8 SDK
```powershell
# Kiểm tra phiên bản hiện tại
dotnet --version

# Nếu chưa cài, tải từ https://dotnet.microsoft.com/download/dotnet/8.0
```

#### Cài Đặt SQL Server
```
Tải từ: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
Chọn SQL Server Express (miễn phí) hoặc Developer Edition
```

### Bước 2: Clone Repository

```powershell
cd Desktop\Learn
git clone https://github.com/nghixuanpham98/ERP.HRM.API.git
cd ERP.HRM.API
```

### Bước 3: Restore Dependencies

```powershell
# Restore NuGet packages
dotnet restore

# Hoặc qua Visual Studio: Build → Restore NuGet Packages
```

### Bước 4: Cấu Hình Connection String

**File:** `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ERP_HRM_DB;User Id=sa;Password=YourPassword123;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**Lưu ý:** Thay `YourPassword123` bằng password SQL Server của bạn

### Bước 5: Tạo Cơ Sở Dữ Liệu

```powershell
# Sử dụng Entity Framework CLI
cd ERP.HRM.Infrastructure

# Tạo database từ migration mới nhất
dotnet ef database update --project ../ERP.HRM.API

# Hoặc qua Visual Studio Package Manager Console:
# Update-Database
```

### Bước 6: Seed Dữ Liệu (Tùy Chọn)

```powershell
# Tệp DataSeeder.cs tự động chạy nếu database trống
# Sẽ tạo:
# - Admin user (admin@example.com)
# - Các bộ phận mẫu
# - Nhân viên mẫu
```

### Bước 7: Chạy Ứng Dụng

```powershell
# Từ thư mục gốc
dotnet run --project ERP.HRM.API

# Hoặc qua Visual Studio:
# Bấm F5 hoặc Debug → Start Debugging
```

**Output dự kiến:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to exit.
```

### Bước 8: Truy Cập Ứng Dụng

- **API URL:** https://localhost:5001
- **Swagger UI:** https://localhost:5001/swagger
- **Health Check:** https://localhost:5001/health

---

## 📦 Các Modules Chính

### 1. Authentication & Authorization Module

**Tập tin chính:**
- `Controllers/AuthController.cs`
- `Services/AuthService.cs`
- `Features/Auth/Commands/`
- `Features/Auth/Queries/`

**Chức năng:**
- ✅ Đăng ký người dùng (Register)
- ✅ Đăng nhập (Login)
- ✅ Refresh Token
- ✅ Gán quyền (Assign Roles)
- ✅ Quản lý phòng

**Endpoints:**
```
POST   /api/auth/register         # Đăng ký
POST   /api/auth/login            # Đăng nhập
POST   /api/auth/refresh-token    # Làm mới token
POST   /api/auth/assign-role      # Gán quyền
```

**Ví dụ Request Login:**
```json
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "Admin123!@#"
}

Response:
{
  "message": "Login successful",
  "data": {
    "token": "eyJhbGc...",
    "refreshToken": "eyJhbGc...",
    "expiresIn": 3600
  }
}
```

---

### 2. Employee Management Module

**Tập tin chính:**
- `Controllers/EmployeesController.cs`
- `Features/Employees/`
- `Entities/Employee.cs`

**Chức năng:**
- ✅ Tạo nhân viên (Create)
- ✅ Cập nhật thông tin (Update)
- ✅ Xóa nhân viên (Delete)
- ✅ Lấy danh sách (Get List)
- ✅ Tìm kiếm & lọc

**Endpoints:**
```
GET    /api/employees              # Lấy danh sách
GET    /api/employees/{id}         # Lấy chi tiết
POST   /api/employees              # Tạo mới
PUT    /api/employees/{id}         # Cập nhật
DELETE /api/employees/{id}         # Xóa
```

**Entity Employee:**
```csharp
public class Employee
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmployeeCode { get; set; }
    public DateTime JoiningDate { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid PositionId { get; set; }
    public EmployeeStatus Status { get; set; }
    public decimal BaseSalary { get; set; }
    // ... properties khác
}
```

---

### 3. Department Management Module

**Endpoints:**
```
GET    /api/departments            # Lấy danh sách
GET    /api/departments/{id}       # Lấy chi tiết
POST   /api/departments            # Tạo mới
PUT    /api/departments/{id}       # Cập nhật
DELETE /api/departments/{id}       # Xóa
```

---

### 4. Payroll Management Module

**Tập tin chính:**
- `Controllers/PayrollController.cs`
- `Controllers/PayrollExportController.cs`
- `Features/Payroll/`
- `Entities/PayrollRecord.cs`
- `Entities/PayrollDeduction.cs`

**Chức năng:**
- ✅ Tính toán lương
- ✅ Quản lý kỳ lương
- ✅ Quản lý khoản trừ
- ✅ Export bảng lương
- ✅ Báo cáo lương

**Endpoints:**
```
GET    /api/payroll/periods              # Kỳ lương
GET    /api/payroll/records              # Bản ghi lương
POST   /api/payroll/calculate            # Tính lương
POST   /api/payroll/export              # Export Excel
```

**PayrollRecord Entity:**
```csharp
public class PayrollRecord
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid PayrollPeriodId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowances { get; set; }
    public decimal TotalDeductions { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime CreatedDate { get; set; }
    public PayrollStatus Status { get; set; }
}
```

---

### 5. Leave Management Module

**Endpoints:**
```
GET    /api/leave-requests              # Danh sách
POST   /api/leave-requests              # Tạo yêu cầu
PUT    /api/leave-requests/{id}         # Cập nhật
POST   /api/leave-requests/{id}/approve # Phê duyệt
POST   /api/leave-requests/{id}/reject  # Từ chối
```

---

### 6. Insurance Management Module

**Endpoints:**
```
GET    /api/insurance-management         # Danh sách
GET    /api/insurance-participations    # Tham gia bảo hiểm
POST   /api/insurance-participations    # Thêm tham gia
```

---

### 7. Personnel Transfer Module

**Endpoints:**
```
GET    /api/personnel-transfers          # Danh sách chuyên cấp
POST   /api/personnel-transfers          # Tạo chuyên cấp
```

---

### 8. Resignation Decision Module

**Endpoints:**
```
GET    /api/resignation-decisions        # Danh sách từ chức
POST   /api/resignation-decisions        # Tạo quyết định
```

---

### 9. Dashboard & Reporting Module

**Endpoints:**
```
GET    /api/dashboard/metrics            # Thống kê chung
GET    /api/reporting/employee-summary   # Tóm tắt nhân viên
GET    /api/reporting/payroll-summary    # Tóm tắt lương
```

---

## 👨‍💻 Hướng Dẫn Phát Triển

### Coding Standards

#### Naming Conventions
```csharp
// Classes - PascalCase
public class EmployeeService { }

// Methods - PascalCase
public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto) { }

// Properties - PascalCase
public string FullName { get; set; }

// Private fields - _camelCase
private readonly IEmployeeRepository _employeeRepository;

// Local variables - camelCase
var employeeList = await _employeeRepository.GetAllAsync();

// Constants - UPPER_CASE
public const string DEFAULT_ROLE = "User";
```

#### Async/Await
```csharp
// ✅ Đúng
public async Task<IActionResult> GetEmployees()
{
    var employees = await _employeeRepository.GetAllAsync();
    return Ok(employees);
}

// ❌ Sai - không nên block thread
public IActionResult GetEmployees()
{
    var employees = _employeeRepository.GetAllAsync().Result;
    return Ok(employees);
}
```

#### Error Handling
```csharp
// ✅ Đúng
try
{
    var employee = await _employeeRepository.GetByIdAsync(id);
    if (employee == null)
        throw new NotFoundException("Employee not found");
    
    return Ok(employee);
}
catch (NotFoundException ex)
{
    Log.Warning(ex, "Employee not found: {Id}", id);
    return NotFound(new { message = ex.Message });
}
catch (Exception ex)
{
    Log.Error(ex, "Error retrieving employee: {Id}", id);
    return StatusCode(500, new { message = "Internal server error" });
}
```

### Thêm Một Feature Mới

#### Step 1: Định Nghĩa Entity

**File:** `ERP.HRM.Domain/Entities/Certificate.cs`

```csharp
using ERP.HRM.Domain.Common;
using System;

namespace ERP.HRM.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificateNumber { get; set; }
        public Employee Employee { get; set; }
    }
}
```

#### Step 2: Thêm DbSet vào DBContext

**File:** `ERP.HRM.Infrastructure/Persistence/ERPDbContext.cs`

```csharp
public virtual DbSet<Certificate> Certificates { get; set; }
```

#### Step 3: Tạo Repository Interface

**File:** `ERP.HRM.Domain/Interfaces/Repositories/ICertificateRepository.cs`

```csharp
using ERP.HRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.Domain.Interfaces.Repositories
{
    public interface ICertificateRepository
    {
        Task<Certificate> GetByIdAsync(Guid id);
        Task<IEnumerable<Certificate>> GetByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<Certificate>> GetAllAsync();
        Task<Certificate> AddAsync(Certificate certificate);
        Task<Certificate> UpdateAsync(Certificate certificate);
        Task<bool> DeleteAsync(Guid id);
    }
}
```

#### Step 4: Implement Repository

**File:** `ERP.HRM.Infrastructure/Repositories/CertificateRepository.cs`

```csharp
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.HRM.Infrastructure.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ERPDbContext _context;

        public CertificateRepository(ERPDbContext context)
        {
            _context = context;
        }

        public async Task<Certificate> GetByIdAsync(Guid id)
            => await _context.Certificates.FindAsync(id);

        public async Task<IEnumerable<Certificate>> GetByEmployeeIdAsync(Guid employeeId)
            => await _context.Certificates
                .Where(c => c.EmployeeId == employeeId)
                .ToListAsync();

        public async Task<IEnumerable<Certificate>> GetAllAsync()
            => await _context.Certificates.ToListAsync();

        public async Task<Certificate> AddAsync(Certificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();
            return certificate;
        }

        public async Task<Certificate> UpdateAsync(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
            await _context.SaveChangesAsync();
            return certificate;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var certificate = await GetByIdAsync(id);
            if (certificate == null) return false;

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
```

#### Step 5: Tạo DTOs

**File:** `ERP.HRM.Application/DTOs/CertificateDto.cs`

```csharp
using System;

namespace ERP.HRM.Application.DTOs
{
    public class CertificateDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificateNumber { get; set; }
    }

    public class CreateCertificateDto
    {
        public Guid EmployeeId { get; set; }
        public string CertificateName { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CertificateNumber { get; set; }
    }

    public class UpdateCertificateDto
    {
        public string CertificateName { get; set; }
        public string IssuingOrganization { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
```

#### Step 6: Tạo Validators

**File:** `ERP.HRM.Application/Validators/CreateCertificateValidator.cs`

```csharp
using ERP.HRM.Application.DTOs;
using FluentValidation;

namespace ERP.HRM.Application.Validators
{
    public class CreateCertificateValidator : AbstractValidator<CreateCertificateDto>
    {
        public CreateCertificateValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required");

            RuleFor(x => x.CertificateName)
                .NotEmpty().WithMessage("Certificate name is required")
                .MaximumLength(200).WithMessage("Certificate name cannot exceed 200 characters");

            RuleFor(x => x.IssuingOrganization)
                .NotEmpty().WithMessage("Issuing organization is required");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required")
                .LessThanOrEqualTo(System.DateTime.Now)
                .WithMessage("Issue date cannot be in the future");
        }
    }
}
```

#### Step 7: Tạo CQRS Commands & Queries

**File:** `ERP.HRM.Application/Features/Certificates/Commands/CreateCertificateCommand.cs`

```csharp
using ERP.HRM.Application.DTOs;
using MediatR;

namespace ERP.HRM.Application.Features.Certificates.Commands
{
    public class CreateCertificateCommand : IRequest<CertificateDto>
    {
        public CreateCertificateDto Certificate { get; set; }
    }
}
```

**File:** `ERP.HRM.Application/Features/Certificates/Handlers/CreateCertificateHandler.cs`

```csharp
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Features.Certificates.Commands;
using ERP.HRM.Domain.Entities;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Features.Certificates.Handlers
{
    public class CreateCertificateHandler : IRequestHandler<CreateCertificateCommand, CertificateDto>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public CreateCertificateHandler(
            ICertificateRepository certificateRepository,
            IMapper mapper)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<CertificateDto> Handle(
            CreateCertificateCommand request,
            CancellationToken cancellationToken)
        {
            var certificate = _mapper.Map<Certificate>(request.Certificate);
            var createdCertificate = await _certificateRepository.AddAsync(certificate);
            return _mapper.Map<CertificateDto>(createdCertificate);
        }
    }
}
```

#### Step 8: Tạo Query

**File:** `ERP.HRM.Application/Features/Certificates/Queries/GetCertificatesByEmployeeQuery.cs`

```csharp
using ERP.HRM.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace ERP.HRM.Application.Features.Certificates.Queries
{
    public class GetCertificatesByEmployeeQuery : IRequest<IEnumerable<CertificateDto>>
    {
        public Guid EmployeeId { get; set; }
    }
}
```

**File:** `ERP.HRM.Application/Features/Certificates/Handlers/GetCertificatesByEmployeeHandler.cs`

```csharp
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Features.Certificates.Queries;
using ERP.HRM.Domain.Interfaces.Repositories;
using MediatR;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Features.Certificates.Handlers
{
    public class GetCertificatesByEmployeeHandler 
        : IRequestHandler<GetCertificatesByEmployeeQuery, IEnumerable<CertificateDto>>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public GetCertificatesByEmployeeHandler(
            ICertificateRepository certificateRepository,
            IMapper mapper)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CertificateDto>> Handle(
            GetCertificatesByEmployeeQuery request,
            CancellationToken cancellationToken)
        {
            var certificates = await _certificateRepository
                .GetByEmployeeIdAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
        }
    }
}
```

#### Step 9: Tạo Controller

**File:** `ERP.HRM.API/Controllers/CertificatesController.cs`

```csharp
using ERP.HRM.Application.DTOs;
using ERP.HRM.Application.Features.Certificates.Commands;
using ERP.HRM.Application.Features.Certificates.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.HRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CertificatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CertificateDto>> CreateCertificate(
            CreateCertificateDto dto)
        {
            var command = new CreateCertificateCommand { Certificate = dto };
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCertificatesByEmployee), 
                new { employeeId = dto.EmployeeId }, result);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<CertificateDto>>> GetCertificatesByEmployee(
            Guid employeeId)
        {
            var query = new GetCertificatesByEmployeeQuery { EmployeeId = employeeId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
```

#### Step 10: Register in DI Container

**File:** `ERP.HRM.API/Program.cs`

```csharp
// Repositories
builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
```

#### Step 11: Tạo Database Migration

```powershell
# Chạy từ thư mục gốc
dotnet ef migrations add AddCertificateEntity --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API

# Apply migration
dotnet ef database update --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

---

## 📡 API Endpoints

### Tổng Hợp Endpoints

#### Authentication
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| POST | `/api/auth/register` | Đăng ký tài khoản |
| POST | `/api/auth/login` | Đăng nhập |
| POST | `/api/auth/refresh-token` | Làm mới token |
| POST | `/api/auth/assign-role` | Gán quyền cho user |

#### Employees
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/employees` | Lấy danh sách nhân viên |
| GET | `/api/employees/{id}` | Lấy chi tiết nhân viên |
| POST | `/api/employees` | Tạo nhân viên |
| PUT | `/api/employees/{id}` | Cập nhật nhân viên |
| DELETE | `/api/employees/{id}` | Xóa nhân viên |
| GET | `/api/employees/department/{deptId}` | Lấy nhân viên theo bộ phận |

#### Departments
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/departments` | Lấy danh sách bộ phận |
| GET | `/api/departments/{id}` | Lấy chi tiết bộ phận |
| POST | `/api/departments` | Tạo bộ phận |
| PUT | `/api/departments/{id}` | Cập nhật bộ phận |
| DELETE | `/api/departments/{id}` | Xóa bộ phận |

#### Payroll
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/payroll/periods` | Lấy danh sách kỳ lương |
| GET | `/api/payroll/records` | Lấy bản ghi lương |
| POST | `/api/payroll/calculate` | Tính toán lương |
| POST | `/api/payroll/export` | Export bảng lương |
| POST | `/api/payroll-export/generate-export` | Tạo export lương |

#### Leave Requests
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/leave-requests` | Danh sách yêu cầu nghỉ phép |
| POST | `/api/leave-requests` | Tạo yêu cầu nghỉ phép |
| PUT | `/api/leave-requests/{id}` | Cập nhật yêu cầu |
| POST | `/api/leave-requests/{id}/approve` | Phê duyệt |
| POST | `/api/leave-requests/{id}/reject` | Từ chối |

#### Insurance
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/insurance-management` | Danh sách bảo hiểm |
| GET | `/api/insurance-participations` | Tham gia bảo hiểm |
| POST | `/api/insurance-participations` | Thêm tham gia bảo hiểm |

#### Personnel Transfer
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/personnel-transfers` | Danh sách chuyên cấp |
| POST | `/api/personnel-transfers` | Tạo chuyên cấp |

#### Resignation
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/resignation-decisions` | Danh sách từ chức |
| POST | `/api/resignation-decisions` | Tạo quyết định từ chức |

#### Dashboard & Reporting
| Phương Thức | Endpoint | Mô Tả |
|---|---|---|
| GET | `/api/dashboard/metrics` | Thống kê chung |
| GET | `/api/reporting/employee-summary` | Tóm tắt nhân viên |
| GET | `/api/reporting/payroll-summary` | Tóm tắt lương |

### Ví Dụ Requests

#### Create Employee
```json
POST /api/employees
Content-Type: application/json
Authorization: Bearer <token>

{
  "fullName": "Nguyễn Văn A",
  "email": "nguyen.van.a@example.com",
  "phoneNumber": "0912345678",
  "dateOfBirth": "1990-05-15",
  "employeeCode": "EMP001",
  "joiningDate": "2024-01-01",
  "departmentId": "550e8400-e29b-41d4-a716-446655440000",
  "positionId": "550e8400-e29b-41d4-a716-446655440001",
  "baseSalary": 10000000
}
```

#### Get Employees with Pagination
```
GET /api/employees?pageNumber=1&pageSize=10&search=Nguyễn
Authorization: Bearer <token>
```

#### Create Leave Request
```json
POST /api/leave-requests
Content-Type: application/json
Authorization: Bearer <token>

{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "leaveType": "Annual",
  "startDate": "2024-02-01",
  "endDate": "2024-02-05",
  "reason": "Personal leave"
}
```

#### Export Payroll
```json
POST /api/payroll-export/generate-export
Content-Type: application/json
Authorization: Bearer <token>

{
  "payrollPeriodId": "550e8400-e29b-41d4-a716-446655440000",
  "exportFormat": "Excel",
  "includeDeductions": true
}

Response:
{
  "message": "Export created successfully",
  "data": {
    "exportId": "550e8400-e29b-41d4-a716-446655440002",
    "fileName": "Payroll_2024_01.xlsx",
    "downloadUrl": "/api/payroll-export/download/550e8400-e29b-41d4-a716-446655440002"
  }
}
```

---

## 🗄️ Quản Lý Cơ Sở Dữ Liệu

### Database Schema Overview

```sql
-- Chính những bảng chính

-- Users & Identity
Users                    -- Người dùng hệ thống
UserRefreshTokens        -- Refresh tokens

-- Organization
Departments              -- Bộ phận
Positions                -- Vị trí công việc
Employees                -- Nhân viên

-- HR Management
EmploymentContracts      -- Hợp đồng lao động
SalaryGrades             -- Bậc lương
FamilyDependents         -- Người phụ thuộc

-- Payroll
PayrollPeriods           -- Kỳ lương
PayrollRecords           -- Bản ghi lương
PayrollDeductions        -- Khoản trừ
SalaryConfigurations     -- Cấu hình lương
Attendance               -- Chấm công

-- Insurance
InsuranceTiers           -- Tầng bảo hiểm
InsuranceParticipations  -- Tham gia bảo hiểm

-- Leave Management
LeaveRequests            -- Yêu cầu nghỉ phép
LeaveBalances            -- Số dư nghỉ phép

-- Employee Records
EmployeeRecords          -- Hồ sơ nhân viên
PersonnelTransfers       -- Chuyên cấp
ResignationDecisions     -- Quyết định từ chức

-- Tax & Insurance
TaxBrackets              -- Khung thuế
TaxExemptions            -- Miễn trừ thuế

-- Production
Products                 -- Sản phẩm
ProductionOutputs        -- Sản lượng
ProductionStages         -- Giai đoạn sản xuất
```

### Entity Relationships

```
Employee
├─ Department
├─ Position
├─ User (1:1)
├─ EmploymentContract (1:*)
├─ PayrollRecord (1:*)
├─ LeaveRequest (1:*)
├─ FamilyDependent (1:*)
├─ EmployeeRecord (1:*)
├─ PersonnelTransfer (1:*)
└─ InsuranceParticipation (1:*)

PayrollRecord
├─ Employee (*)
├─ PayrollPeriod (*)
├─ PayrollDeduction (1:*)
└─ SalaryComponent (1:*)

LeaveRequest
├─ Employee (*)
└─ LeaveType
```

### Migrations

#### Xem danh sách migrations
```powershell
dotnet ef migrations list --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

#### Tạo migration mới
```powershell
# Cú pháp: dotnet ef migrations add <MigrationName>
dotnet ef migrations add AddEmployeeTable --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

#### Apply migrations
```powershell
# Apply tất cả pending migrations
dotnet ef database update --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API

# Apply tới migration cụ thể
dotnet ef database update AddEmployeeTable --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

#### Rollback migration
```powershell
# Rollback tới migration trước
dotnet ef database update <PreviousMigrationName> --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

#### Remove migration (chưa apply)
```powershell
dotnet ef migrations remove --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API
```

### Database Seeding

**File:** `ERP.HRM.Infrastructure/Seed/DataSeeder.cs`

```csharp
public static async Task SeedDataAsync(this ModelBuilder modelBuilder)
{
    // Seed Department
    var departments = new[]
    {
        new Department { Id = Guid.NewGuid(), Name = "IT", Description = "Information Technology" },
        new Department { Id = Guid.NewGuid(), Name = "HR", Description = "Human Resources" },
        new Department { Id = Guid.NewGuid(), Name = "Finance", Description = "Finance" }
    };
    modelBuilder.Entity<Department>().HasData(departments);

    // Seed Position
    var positions = new[]
    {
        new Position { Id = Guid.NewGuid(), Name = "Developer", DepartmentId = departments[0].Id },
        new Position { Id = Guid.NewGuid(), Name = "Manager", DepartmentId = departments[1].Id }
    };
    modelBuilder.Entity<Position>().HasData(positions);
}
```

### Backup & Restore

#### Backup Database
```powershell
# Sử dụng SQL Server Management Studio (SSMS)
# Hoặc qua PowerShell:

$server = "localhost"
$database = "ERP_HRM_DB"
$backupPath = "C:\Backups\ERP_HRM_DB_$(Get-Date -Format 'yyyyMMdd_HHmmss').bak"

Invoke-SqlCmd -ServerInstance $server -Database $database `
  -Query "BACKUP DATABASE [$database] TO DISK = N'$backupPath'"
```

#### Restore Database
```powershell
$backupPath = "C:\Backups\ERP_HRM_DB_20240115_120000.bak"

Invoke-SqlCmd -ServerInstance "localhost" `
  -Query "RESTORE DATABASE [ERP_HRM_DB] FROM DISK = N'$backupPath'"
```

---

## 🔐 Bảo Mật & Xác Thực

### JWT Authentication

#### Cấu Hình JWT
**File:** `appsettings.json`

```json
{
  "JwtSettings": {
    "Secret": "your-very-long-secret-key-at-least-32-characters",
    "Issuer": "erp.hrm.api",
    "Audience": "erp.hrm.api.users",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

#### Login Flow
```
1. User sends credentials
2. API validates credentials
3. Generate JWT token (60 minutes)
4. Generate Refresh token (7 days)
5. Return tokens to client
6. Client uses JWT for subsequent requests
```

#### Using JWT Token
```http
GET /api/employees
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Role-Based Access Control (RBAC)

#### Defined Roles
```csharp
public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Employee = "Employee";
    public const string HR = "HR";
    public const string Finance = "Finance";
}
```

#### Using Authorization in Controllers
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]  // Require authentication
public class EmployeesController : ControllerBase
{
    // Accessible to authenticated users
    [HttpGet]
    public async Task<IActionResult> GetEmployees() { }

    // Only Admin can access
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid id) { }

    // Admin or HR can access
    [Authorize(Roles = "Admin,HR")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeDto dto) { }
}
```

### Password Policy

```csharp
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    // Yêu cầu chữ số
    options.Password.RequireDigit = true;
    
    // Độ dài tối thiểu
    options.Password.RequiredLength = 8;
    
    // Yêu cầu ký tự đặc biệt
    options.Password.RequireNonAlphanumeric = true;
    
    // Yêu cầu chữ hoa
    options.Password.RequireUppercase = true;
    
    // Khóa tài khoản sau 5 lần đăng nhập sai
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});
```

**Yêu cầu mật khẩu:**
- ✓ Có ít nhất 1 chữ số (0-9)
- ✓ Có ít nhất 1 ký tự đặc biệt (!@#$%^&*)
- ✓ Có ít nhất 1 chữ hoa (A-Z)
- ✓ Tối thiểu 8 ký tự

**Ví dụ mật khẩu hợp lệ:** `Admin123!@#`

### Data Validation

#### FluentValidation Usage
```csharp
public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0).WithMessage("Salary must be greater than 0");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");
    }
}
```

### CORS Configuration

**File:** `Configuration/CorsConfiguration.cs`

```csharp
public static void AddCorsConfiguration(this IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

        options.AddPolicy("AllowSpecific", builder =>
            builder
                .WithOrigins("https://localhost:3000", "https://app.example.com")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });
}
```

---

## 📝 Logging & Monitoring

### Serilog Configuration

**File:** `Program.cs`

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "ERP.HRM.API")
    .WriteTo.Console()
    .WriteTo.File(
        "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
```

### Log Levels
- **Fatal** - Lỗi nghiêm trọng, ứng dụng sắp dừng
- **Error** - Lỗi xảy ra
- **Warning** - Cảnh báo, có thể gây vấn đề
- **Information** - Thông tin tổng quát
- **Debug** - Thông tin debug
- **Verbose** - Thông tin chi tiết nhất

### Logging Examples

```csharp
using Serilog;

public class EmployeeService
{
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(ILogger<EmployeeService> logger)
    {
        _logger = logger;
    }

    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        _logger.LogInformation("Creating employee: {EmployeeName}", dto.FullName);
        
        try
        {
            // Business logic
            var employee = new Employee { ... };
            
            _logger.LogInformation("Employee created successfully: {EmployeeId}", employee.Id);
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee: {EmployeeName}", dto.FullName);
            throw;
        }
    }
}
```

### Health Checks

**Endpoint:** `GET /health`

```csharp
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthCheckService _healthCheckService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _healthCheckService.CheckHealthAsync();
        return Ok(new { status = result });
    }
}
```

### Application Insights (Optional)

```json
"ApplicationInsights": {
  "InstrumentationKey": "your-key-here"
}
```

---

## 🧪 Testing

### Unit Testing Setup

**File:** `tests/ERP.HRM.Application.Tests/ERP.HRM.Application.Tests.csproj`

```xml
<ItemGroup>
    <PackageReference Include="xunit" Version="2.7.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.7.0" />
    <PackageReference Include="Moq" Version="4.20.0" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
</ItemGroup>
```

### Test Examples

#### Employee Service Tests
```csharp
public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new EmployeeService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateEmployee_WithValidData_ReturnsCreatedEmployee()
    {
        // Arrange
        var createDto = new CreateEmployeeDto
        {
            FullName = "John Doe",
            Email = "john@example.com"
        };

        var employee = new Employee { Id = Guid.NewGuid(), ...createDto };
        var employeeDto = new EmployeeDto { ...employee };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Employee>()))
            .ReturnsAsync(employee);
        _mockMapper.Setup(m => m.Map<Employee>(It.IsAny<CreateEmployeeDto>()))
            .Returns(employee);
        _mockMapper.Setup(m => m.Map<EmployeeDto>(It.IsAny<Employee>()))
            .Returns(employeeDto);

        // Act
        var result = await _service.CreateEmployeeAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.FullName.Should().Be("John Doe");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(invalidId))
            .ReturnsAsync((Employee)null);

        // Act
        var result = await _service.GetEmployeeByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }
}
```

### Running Tests

```powershell
# Chạy tất cả tests
dotnet test

# Chạy tests trong project cụ thể
dotnet test tests/ERP.HRM.Application.Tests

# Chạy test với filter
dotnet test --filter "EmployeeServiceTests"

# Chạy với verbose output
dotnet test -v detailed

# Tạo code coverage report
dotnet test /p:CollectCoverage=true
```

---

## 🚀 Deployment

### Environment Preparation

#### Production Checklist
- [ ] Database backup & security review
- [ ] Appsettings configuration
- [ ] SSL certificate setup
- [ ] Firewall rules
- [ ] Load balancer configuration
- [ ] Monitoring setup
- [ ] Logging infrastructure
- [ ] Secrets management (Azure Key Vault)

### Deployment Methods

#### 1. Publish as Self-Contained App

```powershell
# Build release version
dotnet publish -c Release -o ./publish

# Run the published app
cd publish
./ERP.HRM.API.exe
```

#### 2. Docker Deployment

**File:** `Dockerfile`

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY . .
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=runtime /app/out .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "ERP.HRM.API.dll"]
```

**Build & Run:**
```bash
docker build -t erp-hrm-api:latest .
docker run -p 5000:80 -e ConnectionStrings__DefaultConnection="your-connection-string" erp-hrm-api:latest
```

#### 3. IIS Deployment

1. Publish to folder: `dotnet publish -c Release -o ./iis-publish`
2. Copy folder to IIS server
3. Create Application Pool (.NET CLR version: No Managed Code)
4. Create website pointing to published folder
5. Configure bindings (HTTP/HTTPS)

#### 4. Azure App Service

```powershell
# Login to Azure
az login

# Create Resource Group
az group create --name erp-hrm-rg --location eastasia

# Create App Service Plan
az appservice plan create --name erp-hrm-plan --resource-group erp-hrm-rg --sku B2

# Create Web App
az webapp create --name erp-hrm-api --resource-group erp-hrm-rg --plan erp-hrm-plan

# Deploy
az webapp up --name erp-hrm-api --resource-group erp-hrm-rg --plan erp-hrm-plan
```

### Configuration Management

#### Development
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ERP_HRM_Dev;User Id=sa;Password=dev123;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

#### Production
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-sql-server;Database=ERP_HRM_Prod;User Id=sql-user;Password=*****;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "JwtSettings": {
    "ExpirationMinutes": 60
  }
}
```

---

## 🐛 Troubleshooting

### Common Issues & Solutions

#### 1. Connection String Error
```
Error: A network-related or instance-specific error occurred while 
establishing a connection to SQL Server
```

**Giải pháp:**
- Kiểm tra SQL Server đang chạy
- Kiểm tra connection string trong appsettings
- Kiểm tra SQL Server authentication mode (Mixed Mode)
- Kiểm tra firewall rules

```powershell
# Kiểm tra SQL Server service
Get-Service -Name MSSQLSERVER

# Khởi động service nếu dừng
Start-Service -Name MSSQLSERVER
```

#### 2. Migration Not Applied
```
Error: The migration 'AddEmployeeTable' has not been applied to the database
```

**Giải pháp:**
```powershell
# Update database to latest migration
dotnet ef database update

# Hoặc specify migration
dotnet ef database update AddEmployeeTable
```

#### 3. JWT Token Expired
```json
{
  "error": "The token is expired"
}
```

**Giải pháp:**
- Sử dụng refresh token để lấy token mới
- Hoặc đăng nhập lại

```http
POST /api/auth/refresh-token
{
  "refreshToken": "your-refresh-token"
}
```

#### 4. Unauthorized Access
```json
{
  "error": "Unauthorized"
}
```

**Giải pháp:**
- Kiểm tra JWT token trong header
- Kiểm tra token còn hạn không
- Kiểm tra user có quyền không

#### 5. Null Reference Exception

**Giải pháp:**
- Kiểm tra Dependency Injection configuration trong Program.cs
- Kiểm tra nullable reference types

```csharp
// Enable nullable reference types
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
```

#### 6. CORS Error
```
Access to XMLHttpRequest at 'http://api.com/...' from origin 'http://localhost:3000'
has been blocked by CORS policy
```

**Giải pháp:**
Cấu hình CORS trong Program.cs
```csharp
app.UseCors("AllowSpecific");
```

### Performance Issues

#### Database Query Too Slow
```csharp
// ❌ N+1 Problem
var employees = await _context.Employees.ToListAsync();
foreach (var emp in employees)
{
    var dept = await _context.Departments
        .FirstOrDefaultAsync(d => d.Id == emp.DepartmentId);
}

// ✅ Use Include
var employees = await _context.Employees
    .Include(e => e.Department)
    .ToListAsync();
```

#### High Memory Usage
```csharp
// ❌ Load all data
var allRecords = await _context.PayrollRecords.ToListAsync();

// ✅ Use pagination
var page = await _context.PayrollRecords
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### Debug Tips

#### Enable Debug Logging
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

#### Use Breakpoints in Visual Studio
1. Click on line number → đặt breakpoint (đỏ)
2. F5 để start debugging
3. Khi breakpoint được hit → F10 để step, F11 để step into
4. Hover trên variable để xem giá trị

---

## 📚 Tài Liệu Tham Khảo

### Documentation Files
- `docs/PHASE4_README.md` - Quick start guide
- `docs/FINAL_REPORT.md` - Final project report
- `docs/PAYROLL_EXPORT_API.md` - API documentation
- `docs/VISUAL_SUMMARY.md` - Visual overview

### External Resources
- [Microsoft .NET 8 Docs](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Best Practices](https://learn.microsoft.com/en-us/aspnet/core/)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Getting Help
- 📧 Email: support@example.com
- 💬 GitHub Issues: https://github.com/nghixuanpham98/ERP.HRM.API/issues
- 📖 Wiki: https://github.com/nghixuanpham98/ERP.HRM.API/wiki

---

## 🎯 Quick Reference

### Essential Commands

```powershell
# Clone repository
git clone https://github.com/nghixuanpham98/ERP.HRM.API.git

# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Create migration
dotnet ef migrations add MigrationName --project ERP.HRM.Infrastructure

# Apply migrations
dotnet ef database update

# Run application
dotnet run --project ERP.HRM.API

# Publish for deployment
dotnet publish -c Release

# Format code
dotnet format
```

### File Structure Quick Map

| Tệp/Thư mục | Mục đích |
|---|---|
| `Program.cs` | Startup & DI |
| `appsettings.json` | Cấu hình chung |
| `Controllers/` | API endpoints |
| `Features/` | CQRS features |
| `Services/` | Business logic |
| `Entities/` | Domain models |
| `Repositories/` | Data access |
| `Migrations/` | Database history |

---

## ✅ Checklist for New Developers

Khi mới bắt đầu với project:

- [ ] Clone repository
- [ ] Cài đặt .NET 8 SDK
- [ ] Cài đặt SQL Server
- [ ] Restore dependencies (`dotnet restore`)
- [ ] Cấu hình connection string
- [ ] Tạo database (`dotnet ef database update`)
- [ ] Chạy application (`dotnet run`)
- [ ] Truy cập Swagger UI
- [ ] Đọc documentation
- [ ] Chạy unit tests
- [ ] Làm quen với codebase

---

**Cập nhật lần cuối:** 2024  
**Phiên bản tài liệu:** 1.0  
**Tác giả:** Development Team  

---

*Happy Coding! 🚀*
