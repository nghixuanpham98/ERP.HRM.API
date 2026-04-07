# 🚀 Cheat Sheet - ERP.HRM.API (Quick Reference)

**In this page if needed!**

---

## ⚡ 5-Minute Setup

```powershell
# Clone
git clone https://github.com/nghixuanpham98/ERP.HRM.API.git
cd ERP.HRM.API

# Setup
dotnet restore
dotnet ef database update --project ERP.HRM.Infrastructure

# Run
dotnet run --project ERP.HRM.API

# Open: https://localhost:5001/swagger
```

---

## 🔑 Default Credentials

**Admin Account:**
```
Email:    admin@example.com
Password: Admin123!@#
```

---

## 🗂️ Project Structure

```
ERP.HRM.API/                 ← API & Controllers
ERP.HRM.Application/         ← Business Logic & CQRS
ERP.HRM.Domain/              ← Entities & Interfaces
ERP.HRM.Infrastructure/      ← Database & Repositories
tests/                       ← Unit Tests
```

---

## 📡 Main Endpoints

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/auth/login` | POST | Login |
| `/api/employees` | GET/POST | Employee CRUD |
| `/api/departments` | GET/POST | Department CRUD |
| `/api/payroll/calculate` | POST | Calculate payroll |
| `/api/payroll-export/generate-export` | POST | Export payroll |
| `/api/leave-requests` | GET/POST | Leave requests |
| `/api/dashboard/metrics` | GET | Dashboard stats |

---

## 🔐 API Auth

```bash
# 1. Login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"Admin123!@#"}'

# 2. Copy token from response

# 3. Use token
curl -H "Authorization: Bearer <token>" \
  https://localhost:5001/api/employees
```

---

## 🛠️ Essential Commands

### Build & Test
```bash
dotnet build              # Build
dotnet test              # Run tests
dotnet clean             # Clean
dotnet format            # Format code
```

### Database
```bash
# Create migration
dotnet ef migrations add NameOfMigration --project ERP.HRM.Infrastructure

# Apply migrations
dotnet ef database update --project ERP.HRM.Infrastructure

# Rollback
dotnet ef database update PreviousMigration
```

### Run
```bash
# Debug
dotnet run --project ERP.HRM.API

# Watch mode (auto-reload)
dotnet watch --project ERP.HRM.API

# Custom port
dotnet run --project ERP.HRM.API --urls https://localhost:5002
```

### Deploy
```bash
# Release build
dotnet publish -c Release -o ./publish

# Docker
docker build -t erp-hrm-api .
docker run -p 5001:80 erp-hrm-api
```

---

## 🗂️ Add New Feature (18 Steps)

```
1. Create Entity           → Domain/Entities/YourEntity.cs
2. Create Repository       → Domain/Interfaces & Infrastructure/Repositories
3. Add DbSet              → Infrastructure/Persistence/ERPDbContext.cs
4. Create DTO             → Application/DTOs/YourEntityDto.cs
5. Create Validator       → Application/Validators/CreateYourValidator.cs
6. Create Commands        → Application/Features/YourFeature/Commands
7. Create Queries         → Application/Features/YourFeature/Queries
8. Create Handlers        → Application/Features/YourFeature/Handlers
9. Create Controller      → API/Controllers/YourController.cs
10. Register DI           → Program.cs (AddScoped)
11. Create Migration      → dotnet ef migrations add
12. Apply Migration       → dotnet ef database update
13. Create Unit Tests     → tests/YourEntity.Tests.cs
14. Test Locally          → dotnet test
15. Build Check           → dotnet build
16. Code Review           → Create PR
17. Merge                 → Squash & merge
18. Deploy                → Follow deployment guide
```

---

## 🐛 Common Errors & Fixes

| Error | Fix |
|-------|-----|
| "Connection string validation failed" | Check SQL Server running, update connection string |
| "401 Unauthorized" | Login first, add token to header: `Authorization: Bearer <token>` |
| "Password does not meet requirements" | Use: Uppercase + number + special char + 8+ chars (e.g., Admin123!@#) |
| "Port 5001 in use" | Kill process: `netstat -ano \| findstr :5001` then `taskkill /PID <id> /F` |
| "Migration not applied" | Run: `dotnet ef database update` |
| "Build failed" | Run: `dotnet clean` then `dotnet restore` then `dotnet build` |

---

## 📊 Database Quick Reference

### Main Tables
```
Users              - User accounts
Employees          - Employee records
Departments        - Departments
Positions          - Job positions
PayrollRecords     - Payroll data
PayrollPeriods     - Payroll cycles
LeaveRequests      - Leave applications
InsuranceTiers     - Insurance types
```

### Key Relationships
```
Employee → Department (many-to-one)
Employee → Position (many-to-one)
Employee → User (one-to-one)
Employee → PayrollRecord (one-to-many)
PayrollRecord → PayrollPeriod (many-to-one)
```

---

## 🔒 Security Checklist

- [ ] Use HTTPS (not HTTP)
- [ ] Store JWT tokens securely
- [ ] Never commit secrets/passwords
- [ ] Use parameterized queries (EF Core default)
- [ ] Validate all inputs (FluentValidation done)
- [ ] Check authorization on endpoints
- [ ] Enable audit logging
- [ ] Regular security scans
- [ ] Keep dependencies updated
- [ ] Implement CORS properly

---

## 📈 File Locations Cheat Sheet

| What | Where |
|------|-------|
| API Route | `ERP.HRM.API/Controllers/*.cs` |
| Business Logic | `ERP.HRM.Application/Features/*/Handlers/*.cs` |
| Database Model | `ERP.HRM.Domain/Entities/*.cs` |
| DB Access | `ERP.HRM.Infrastructure/Repositories/*.cs` |
| Config | `ERP.HRM.API/appsettings*.json` |
| Migrations | `ERP.HRM.Infrastructure/Migrations/` |
| Tests | `tests/ERP.HRM.Application.Tests/` |

---

## 🎯 Coding Patterns

### CQRS (Command Query)
```csharp
// Command (Create/Update/Delete)
public class CreateEmployeeCommand : IRequest<EmployeeDto> { }

// Query (Read)
public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>> { }

// Handler
public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto> { }

// Use in Controller
var result = await _mediator.Send(command);
```

### Repository Pattern
```csharp
// Interface
public interface IEmployeeRepository {
    Task<Employee> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee> AddAsync(Employee employee);
}

// Implementation
public class EmployeeRepository : IEmployeeRepository {
    private readonly ERPDbContext _context;
    // ... implementation
}

// Use
var employee = await _employeeRepository.GetByIdAsync(id);
```

### Dependency Injection
```csharp
// Register in Program.cs
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Use in Controller
public class EmployeesController : ControllerBase {
    private readonly IMediator _mediator;
    
    public EmployeesController(IMediator mediator) {
        _mediator = mediator;
    }
}
```

---

## 🚀 Performance Tips

```csharp
// ✅ Use async/await
public async Task<List<Employee>> GetEmployeesAsync()
    => await _context.Employees.ToListAsync();

// ✅ Include related data
var employees = await _context.Employees
    .Include(e => e.Department)
    .ToListAsync();

// ✅ Pagination
var page = await _context.Employees
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

// ✅ Select only needed columns
var employees = await _context.Employees
    .Select(e => new { e.Id, e.FullName })
    .ToListAsync();

// ❌ Avoid N+1 problem
// Don't do: foreach { await _context.X.Find(...) }
```

---

## 📱 API Request Examples

### Login
```json
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "Admin123!@#"
}
```

### Create Employee
```json
POST /api/employees
Authorization: Bearer <token>
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "dateOfBirth": "1990-05-15",
  "departmentId": "550e8400-...",
  "baseSalary": 10000000
}
```

### Calculate Payroll
```json
POST /api/payroll/calculate
Authorization: Bearer <token>
{
  "payrollPeriodId": "550e8400-...",
  "employeeIds": ["550e8400-...", "550e8400-..."]
}
```

---

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "EmployeeServiceTests"

# With code coverage
dotnet test /p:CollectCoverage=true

# Verbose output
dotnet test -v detailed
```

### Test Pattern
```csharp
[Fact]
public async Task Should_CreateEmployee_WithValidData() {
    // Arrange
    var mockRepository = new Mock<IEmployeeRepository>();
    mockRepository.Setup(r => r.AddAsync(It.IsAny<Employee>()))
        .ReturnsAsync(new Employee { Id = Guid.NewGuid() });
    
    // Act
    var result = await _service.CreateAsync(dto);
    
    // Assert
    Assert.NotNull(result);
    mockRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
}
```

---

## 🚀 Deployment

### Local Publish
```bash
dotnet publish -c Release -o ./publish
cd publish
./ERP.HRM.API.exe
```

### Docker
```bash
docker build -t erp-hrm-api:latest .
docker run -p 5001:80 \
  -e ConnectionStrings__DefaultConnection="your-connection-string" \
  erp-hrm-api:latest
```

### IIS
1. Publish: `dotnet publish -c Release -o ./iis-publish`
2. Copy to IIS folder
3. Create Application Pool
4. Configure website

---

## 📚 Documentation Map

| Need | Document | Time |
|------|----------|------|
| Full setup | SYSTEM_GUIDE.md | 2 hours |
| Quick start | QUICK_START_TROUBLESHOOTING.md | 10 min |
| API details | API_REFERENCE.md | 30 min |
| Architecture | ARCHITECTURE_GUIDE.md | 1 hour |
| Business info | EXECUTIVE_SUMMARY.md | 15 min |

---

## 🆘 Get Help

### Quick Search
- Project structure → SYSTEM_GUIDE.md (Cấu trúc)
- API endpoint → API_REFERENCE.md (search endpoint name)
- Error fix → QUICK_START.md (Troubleshooting)
- Architecture → ARCHITECTURE_GUIDE.md
- Setup → QUICK_START.md (Quick Start)

### Support
- GitHub: https://github.com/nghixuanpham98/ERP.HRM.API/issues
- Docs: https://github.com/nghixuanpham98/ERP.HRM.API/wiki

---

## 📋 Daily Workflow

### Morning
```
1. Pull latest: git pull
2. Build: dotnet build
3. Tests: dotnet test
4. Read PRs & issues
```

### Development
```
1. Create branch: git checkout -b feature/name
2. Develop feature
3. Test locally: dotnet run
4. Run tests: dotnet test
5. Commit: git commit -m "feat: description"
```

### Afternoon
```
1. Push: git push origin feature/name
2. Create PR
3. Code review
4. Merge: git checkout main && git pull
5. Deploy to staging
```

---

## ✅ Pre-Deployment Checklist

- [ ] Code reviewed & approved
- [ ] All tests passing
- [ ] No compiler warnings
- [ ] Migration created & tested
- [ ] Database backup taken
- [ ] Performance tested
- [ ] Security scanned
- [ ] Documentation updated
- [ ] Deployment plan ready
- [ ] Rollback plan ready

---

## 🎯 Remember

```
1. Always use async/await
2. Always validate inputs (FluentValidation)
3. Always use DI for dependencies
4. Always write tests
5. Always follow the architecture
6. Always reference documentation
7. Always commit with clear messages
8. Always review before merge
9. Always test before deploy
10. Always keep documentation updated
```

---

## 📞 Key Contacts

| Role | Name | Email | Phone |
|------|------|-------|-------|
| Tech Lead | [Name] | [email] | [phone] |
| PM | [Name] | [email] | [phone] |
| DevOps | [Name] | [email] | [phone] |
| QA Lead | [Name] | [email] | [phone] |

---

## 🔗 Useful Links

- GitHub Repo: https://github.com/nghixuanpham98/ERP.HRM.API
- Swagger UI: https://localhost:5001/swagger (local)
- .NET Docs: https://learn.microsoft.com/dotnet/
- ASP.NET Core: https://learn.microsoft.com/aspnet/core/
- Entity Framework: https://learn.microsoft.com/ef/core/

---

**Print or bookmark this page for quick reference!**

**Version:** 1.0 | **Last Updated:** 2024 | **Status:** ✅ Production Ready
