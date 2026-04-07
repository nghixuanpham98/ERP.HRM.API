# ⚡ Quick Start & Troubleshooting Guide

## 🚀 Quick Start (5 phút)

### Prerequisites Check

```powershell
# Kiểm tra .NET 8 SDK
dotnet --version
# Output: 8.0.x hoặc cao hơn

# Kiểm tra SQL Server
sqlcmd -S localhost -U sa
# Hoặc dùng SSMS
```

### 1️⃣ Clone & Setup

```powershell
# Clone repository
git clone https://github.com/nghixuanpham98/ERP.HRM.API.git
cd ERP.HRM.API

# Restore packages
dotnet restore
```

### 2️⃣ Configure Database

**File:** `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ERP_HRM_DB;User Id=sa;Password=YourPassword123;TrustServerCertificate=true;"
  }
}
```

> 🔑 **Thay `YourPassword123` bằng password SQL Server của bạn**

### 3️⃣ Create Database

```powershell
cd ERP.HRM.Infrastructure

# Tạo database từ migration mới nhất
dotnet ef database update --project . --startup-project ../ERP.HRM.API

# Kiểm tra: Mở SSMS → Object Explorer → Servers → Databases
# Bạn sẽ thấy database "ERP_HRM_DB"
```

### 4️⃣ Run Application

```powershell
# Từ thư mục gốc
dotnet run --project ERP.HRM.API

# Output:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:5001
# info: Microsoft.Hosting.Lifetime[0]
#       Application started.
```

### 5️⃣ Test API

Truy cập: **https://localhost:5001/swagger**

**Test Login:**
```json
POST /api/auth/login
{
  "email": "admin@example.com",
  "password": "Admin123!@#"
}
```

✅ **Done!** Hệ thống đang chạy!

---

## 🐛 Troubleshooting

### ❌ Problem 1: "Connection string validation failed"

**Error Message:**
```
A network-related or instance-specific error occurred while establishing a 
connection to SQL Server. The server was not found or was not accessible.
```

**Giải pháp:**

1️⃣ **Kiểm tra SQL Server đang chạy**
```powershell
# Xem danh sách services
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Kết quả:
# Status   Name                DisplayName
# ------   ----                -----------
# Running  MSSQLSERVER        SQL Server (MSSQLSERVER)
# Running  SQLSERVERAGENT     SQL Server Agent (MSSQLSERVER)

# Nếu dừng, khởi động:
Start-Service -Name MSSQLSERVER
```

2️⃣ **Kiểm tra connection string**
```powershell
# Test connection qua sqlcmd
sqlcmd -S localhost -U sa -P YourPassword123

# Hoặc test trong Visual Studio:
# View → SQL Server Object Explorer → Add SQL Server
```

3️⃣ **Nếu vẫn lỗi:**
- Kiểm tra SQL Server đang nghe port 1433
- Kiểm tra Windows Firewall rules
- Thử restart SQL Server service

---

### ❌ Problem 2: "Migration not found"

**Error Message:**
```
The migration 'AddEmployeeTable' has not been applied to the database. 
Apply pending migrations before accessing the service.
```

**Giải pháp:**

```powershell
# Cách 1: Update database to latest migration
dotnet ef database update --project ERP.HRM.Infrastructure --startup-project ERP.HRM.API

# Cách 2: Xem danh sách migrations
dotnet ef migrations list --project ERP.HRM.Infrastructure

# Cách 3: Xem pending migrations
dotnet ef migrations pending --project ERP.HRM.Infrastructure

# Cách 4: Reset database (⚠️ DELETE ALL DATA)
dotnet ef database drop --force --project ERP.HRM.Infrastructure
dotnet ef database update --project ERP.HRM.Infrastructure
```

**Verify:**
- Mở SSMS → Database → Tables
- Bạn sẽ thấy tables: Employees, Departments, Users, etc.

---

### ❌ Problem 3: "401 Unauthorized"

**Error Message:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Authorization header is missing or invalid"
}
```

**Giải pháp:**

1️⃣ **Đăng nhập trước**
```http
POST https://localhost:5001/api/auth/login
{
  "email": "admin@example.com",
  "password": "Admin123!@#"
}

Response:
{
  "data": {
    "token": "eyJhbGc...",
    "refreshToken": "eyJhbGc..."
  }
}
```

2️⃣ **Copy token và thêm vào header**
```http
GET https://localhost:5001/api/employees
Authorization: Bearer eyJhbGc...
```

3️⃣ **Hoặc sử dụng Swagger**
- Mở https://localhost:5001/swagger
- Click "Authorize"
- Paste token
- Click "Authorize"

---

### ❌ Problem 4: "Password does not meet complexity requirements"

**Error Message:**
```json
{
  "error": "Passwords must have at least one non-alphanumeric character"
}
```

**Giải pháp:**

Password phải có:
- ✅ Ít nhất 8 ký tự
- ✅ Ít nhất 1 chữ số (0-9)
- ✅ Ít nhất 1 chữ hoa (A-Z)
- ✅ Ít nhất 1 ký tự đặc biệt (!@#$%^&*)

**Valid passwords:**
- ✅ `Admin123!@#`
- ✅ `User@Password123`
- ✅ `Test$1234`

**Invalid passwords:**
- ❌ `admin123` (no uppercase, no special)
- ❌ `Admin` (too short, no number)
- ❌ `123456789` (no letter, no special)

---

### ❌ Problem 5: "Port 5001 is already in use"

**Error Message:**
```
System.IO.IOException: Failed to bind to address http://127.0.0.1:5001: 
Address already in use.
```

**Giải pháp:**

```powershell
# Tìm process sử dụng port 5001
netstat -ano | findstr :5001
# Output: TCP    127.0.0.1:5001         LISTENING       12345

# Kill process
taskkill /PID 12345 /F

# Hoặc chạy trên port khác
dotnet run --project ERP.HRM.API --urls https://localhost:5002
```

---

### ❌ Problem 6: "NullReferenceException in Dependency Injection"

**Error Message:**
```
System.NullReferenceException: Object reference not set to an instance of an object.
   at ERP.HRM.API.Controllers.EmployeesController.cctor() in EmployeesController.cs
```

**Giải pháp:**

1️⃣ **Kiểm tra service được register trong Program.cs**
```csharp
// ❌ Sai - service không được register
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    // Exception!
}

// ✅ Đúng - service được register
// Trong Program.cs:
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
```

2️⃣ **Kiểm tra constructor injection**
```csharp
// ✅ Đúng - sử dụng constructor injection
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;
    
    public EmployeesController(IEmployeeRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
```

3️⃣ **Debug:**
```powershell
# Chạy với debug logging
dotnet run --project ERP.HRM.API --verbosity Debug
```

---

### ❌ Problem 7: "CORS error when calling from frontend"

**Error Message:**
```
Access to XMLHttpRequest at 'https://localhost:5001/api/employees' from origin 
'http://localhost:3000' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header
```

**Giải pháp:**

Trong `Program.cs`, cấu hình CORS:

```csharp
// Thêm CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
        builder
            .WithOrigins("http://localhost:3000", "https://yourdomain.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// ...

// Sử dụng CORS
app.UseCors("AllowFrontend");
```

---

### ❌ Problem 8: "Build failed - syntax error"

**Error Message:**
```
error CS1002: ; expected
error CS1513: } expected
```

**Giải pháp:**

```powershell
# 1. Clean solution
dotnet clean

# 2. Restore packages
dotnet restore

# 3. Build lại
dotnet build

# 4. Xem detailed errors
dotnet build --verbosity detailed

# 5. Nếu vẫn lỗi, check file
# Mở file có lỗi → kiểm tra syntax
```

---

### ❌ Problem 9: "Test failures"

**Error Message:**
```
FAILED Test.UnitTests [100ms]
System.NullReferenceException: Object reference not set to an instance.
```

**Giải pháp:**

```csharp
// ❌ Sai - mock không setup
[Fact]
public async Task TestMethod()
{
    var mockRepo = new Mock<IEmployeeRepository>();
    // Exception - method không được setup!
}

// ✅ Đúng - mock được setup
[Fact]
public async Task TestMethod()
{
    var mockRepo = new Mock<IEmployeeRepository>();
    mockRepo.Setup(r => r.GetAllAsync())
        .ReturnsAsync(new List<Employee>());
    
    var result = await mockRepo.Object.GetAllAsync();
    Assert.NotNull(result);
}
```

**Chạy tests:**
```powershell
# Chạy tất cả tests
dotnet test

# Chạy test cụ thể
dotnet test --filter "TestMethod"

# Verbose output
dotnet test --verbosity detailed
```

---

### ❌ Problem 10: "Logging not working"

**Error Message:**
```
No log files are created in logs/ directory
```

**Giải pháp:**

Kiểm tra Serilog configuration trong `Program.cs`:

```csharp
// Kiểm tra logs folder tồn tại
// Nếu không, tạo:
Directory.CreateDirectory("logs");

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/log-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();
```

**Verify logs:**
```powershell
# Xem log files
Get-ChildItem -Path "logs" -Filter "log-*.txt" | Select-Object Name, LastWriteTime

# Xem nội dung log
Get-Content "logs/log-20240115.txt" -Tail 20
```

---

## 📋 Checklist: Before Going to Production

- [ ] Database backup taken
- [ ] Connection string updated
- [ ] SSL certificate installed
- [ ] Firewall rules configured
- [ ] Logging configured
- [ ] Monitoring setup
- [ ] Backup strategy in place
- [ ] Security headers added
- [ ] CORS properly configured
- [ ] Rate limiting enabled
- [ ] All tests passing
- [ ] Code reviewed
- [ ] Documentation updated

---

## 🔍 Common Commands Reference

### Development

```powershell
# Build
dotnet build

# Run
dotnet run --project ERP.HRM.API

# Run specific port
dotnet run --project ERP.HRM.API --urls https://localhost:5002

# Watch mode (auto reload on file changes)
dotnet watch --project ERP.HRM.API
```

### Testing

```powershell
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "EmployeeServiceTests"

# Run with code coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover

# Watch mode for tests
dotnet watch test
```

### Database

```powershell
# Create migration
dotnet ef migrations add MigrationName --project ERP.HRM.Infrastructure

# List migrations
dotnet ef migrations list --project ERP.HRM.Infrastructure

# Pending migrations
dotnet ef migrations pending --project ERP.HRM.Infrastructure

# Apply migrations
dotnet ef database update --project ERP.HRM.Infrastructure

# Rollback
dotnet ef database update PreviousMigrationName --project ERP.HRM.Infrastructure

# Drop database
dotnet ef database drop --force --project ERP.HRM.Infrastructure

# Get SQL script
dotnet ef migrations script --project ERP.HRM.Infrastructure
```

### Code Quality

```powershell
# Format code
dotnet format

# Analyze
dotnet build -c Debug

# Clean
dotnet clean
```

### Publishing

```powershell
# Publish release
dotnet publish -c Release -o ./publish

# Self-contained (includes runtime)
dotnet publish -c Release -r win-x64 --self-contained

# Docker
docker build -t erp-hrm-api:latest .
docker run -p 5001:80 erp-hrm-api:latest
```

---

## 🚀 Performance Tuning Tips

### 1. Database Query Optimization

```csharp
// ❌ Slow - N+1 problem
var employees = await _context.Employees.ToListAsync();
foreach (var emp in employees)
{
    var department = await _context.Departments.FindAsync(emp.DepartmentId);
}

// ✅ Fast - Include relationships
var employees = await _context.Employees
    .Include(e => e.Department)
    .ToListAsync();

// ✅ Fast - Select only needed columns
var employees = await _context.Employees
    .Select(e => new { e.Id, e.FullName, e.Department.Name })
    .ToListAsync();
```

### 2. Implement Caching

```csharp
// Cache employee list for 1 hour
[Caching(Duration = 3600)]
[HttpGet]
public async Task<IActionResult> GetEmployees()
{
    return Ok(await _employeeService.GetAllAsync());
}
```

### 3. Use Async/Await Properly

```csharp
// ✅ Correct
public async Task<List<Employee>> GetEmployeesAsync()
{
    return await _repository.GetAllAsync();
}

// ❌ Wrong - Sync over async (deadlock!)
public List<Employee> GetEmployees()
{
    return _repository.GetAllAsync().Result;
}
```

### 4. Pagination

```csharp
// Always paginate large result sets
var pagedEmployees = await _repository.GetAllAsync(
    pageNumber: 1,
    pageSize: 10
);
```

### 5. Connection Pooling

```csharp
// Already configured in EF Core
// Connection pool size: 100 (default)
// Connection lifetime: 5 minutes
```

---

## 📊 Monitoring & Health Checks

### Health Check Endpoint

```powershell
# Check API health
curl https://localhost:5001/health

# Response:
{
  "status": "Healthy",
  "checks": {
    "database": "Healthy",
    "memory": "Healthy"
  }
}
```

### Log Monitoring

```powershell
# Real-time tail
Get-Content "logs/log-20240115.txt" -Wait -Tail 20

# Filter errors
Select-String -Path "logs/log-*.txt" -Pattern "ERROR"

# Count log entries
(Get-Content "logs/log-20240115.txt" | Measure-Object -Line).Lines
```

### Performance Metrics

```csharp
// View in Application Insights or console
// Track:
// - API response time
// - Database query time
// - Memory usage
// - Error rate
```

---

## 🔒 Security Quick Tips

### JWT Token Management
- ✅ Store token in secure HTTP-only cookie (frontend)
- ✅ Use HTTPS only (never HTTP)
- ✅ Set token expiration to 1 hour
- ✅ Refresh token expiration to 7 days
- ❌ Don't store sensitive data in JWT payload

### Password Security
- ✅ Enforce minimum 8 characters
- ✅ Require uppercase, number, special character
- ✅ Use bcrypt hashing (ASP.NET Identity does this)
- ✅ Implement account lockout after failed attempts

### API Security
- ✅ Enable HTTPS/TLS
- ✅ Add security headers
- ✅ Implement rate limiting
- ✅ Validate all inputs (FluentValidation)
- ✅ Use parameterized queries (EF Core does this)

---

## 📞 Support & Resources

### When You Need Help

1. **Check Logs**
   ```powershell
   Get-Content "logs/log-20240115.txt" | Select-String "ERROR"
   ```

2. **Search Documentation**
   - `SYSTEM_GUIDE.md` - Comprehensive guide
   - `ARCHITECTURE_GUIDE.md` - Architecture details
   - `docs/` folder - Additional docs

3. **Check GitHub Issues**
   - https://github.com/nghixuanpham98/ERP.HRM.API/issues

4. **Review Code Examples**
   - Sample requests in `docs/PAYROLL_EXPORT_API.md`
   - Postman collection in `docs/PayrollExportAPI.postman_collection.json`

---

## ✅ Verification Checklist

After setup, verify everything works:

```powershell
# 1. Database connection
sqlcmd -S localhost -U sa -P YourPassword123

# 2. Build
dotnet build
# ✅ Expected: Build succeeded

# 3. Run tests
dotnet test
# ✅ Expected: X passed, 0 failed

# 4. Start app
dotnet run --project ERP.HRM.API
# ✅ Expected: Now listening on https://localhost:5001

# 5. API health
curl https://localhost:5001/health
# ✅ Expected: {"status":"Healthy"}

# 6. Swagger
# Open https://localhost:5001/swagger in browser
# ✅ Expected: Swagger UI loads with all endpoints

# 7. Login
# POST /api/auth/login with admin credentials
# ✅ Expected: Receive JWT token

# 8. Get employees
# GET /api/employees with token in header
# ✅ Expected: List of employees returned
```

---

**🎉 Nếu tất cả ✅ thì bạn đã setup thành công!**

---

**Cập nhật lần cuối:** 2024  
**Phiên bản:** 1.0
