# Unit Tests - ERP.HRM.Application

Bộ test dành cho dự án ERP.HRM với scope bao gồm các services, validators, command handlers, và query handlers.

## 📋 Cấu trúc Test Project

```
tests/ERP.HRM.Application.Tests/
├── Services/
│   ├── DepartmentServiceTests.cs
│   ├── AuthServiceTests.cs
│   └── PayrollServiceTests.cs
├── Validators/
│   ├── CreateEmployeeValidatorTests.cs
│   └── CreateDepartmentValidatorTests.cs
├── Features/
│   └── Handlers/
│       ├── GetAllDepartmentsQueryHandlerTests.cs
│       └── CreatePositionCommandHandlerTests.cs
└── StringValidationExtensionsTests.cs
```

## 🧪 Các Test Case Hiện Có

### 1. Services Tests

#### DepartmentServiceTests
- ✅ `GetAllDepartmentsAsync_ShouldReturnPagedDepartments` - Kiểm tra lấy danh sách phòng ban phân trang
- ✅ `GetDepartmentByIdAsync_WithValidId_ShouldReturnDepartment` - Lấy phòng ban theo ID hợp lệ
- ✅ `GetDepartmentByIdAsync_WithInvalidId_ShouldThrowNotFoundException` - Ném exception khi ID không tồn tại
- ✅ `AddDepartmentAsync_WithValidData_ShouldCreateDepartment` - Thêm phòng ban mới
- ✅ `AddDepartmentAsync_WithDuplicateName_ShouldThrowBusinessRuleException` - Kiểm tra trùng tên
- ✅ `UpdateDepartmentAsync_WithValidData_ShouldUpdateDepartment` - Cập nhật phòng ban
- ✅ `DeleteDepartmentAsync_WithValidId_ShouldDeleteDepartment` - Xóa phòng ban

#### AuthServiceTests
- ✅ `RegisterAsync_WithUniqueUsername_ShouldCreateUser` - Đăng ký user mới
- ✅ `RegisterAsync_WithExistingUsername_ShouldThrowBusinessRuleException` - Kiểm tra user trùng
- ✅ `LoginAsync_WithValidCredentials_ShouldReturnToken` - Đăng nhập thành công
- ✅ `LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedAccessException` - Mật khẩu sai
- ✅ `LoginAsync_WithNonexistentUser_ShouldThrowUnauthorizedAccessException` - User không tồn tại

#### PayrollServiceTests
- ✅ `CalculateMonthlySalaryAsync_WithValidEmployeeAndPeriod_ShouldReturnPayrollRecord` - Tính lương tháng
- ✅ `CalculateMonthlySalaryAsync_WithInvalidEmployee_ShouldThrowNotFoundException` - Employee không tồn tại
- ✅ `CalculateMonthlySalaryAsync_WithInvalidPeriod_ShouldThrowNotFoundException` - Period không tồn tại
- ✅ `CalculateMonthlySalaryAsync_WithOverrideSalary_ShouldUseOverrideValue` - Sử dụng lương override
- ✅ `GetSalarySlipAsync_WithValidIds_ShouldReturnSalarSlip` - Lấy chi tiết lương

### 2. Validators Tests

#### CreateEmployeeValidatorTests
- ✅ `Validate_WithValidData_ShouldSucceed` - Dữ liệu hợp lệ
- ✅ `Validate_WithEmptyEmployeeCode_ShouldFail` - Mã nhân viên trống
- ✅ `Validate_WithTooLongEmployeeCode_ShouldFail` - Mã nhân viên quá dài
- ✅ `Validate_WithEmptyFullName_ShouldFail` - Tên trống
- ✅ `Validate_WithInvalidEmail_ShouldFail` - Email không hợp lệ
- ✅ `Validate_WithInvalidPhoneNumber_ShouldFail` - Số điện thoại không hợp lệ
- ✅ `Validate_WithInvalidDepartmentId_ShouldFail` - ID phòng ban không hợp lệ
- ✅ `Validate_WithInvalidPositionId_ShouldFail` - ID vị trí không hợp lệ
- ✅ `Validate_WithNegativeBaseSalary_ShouldFail` - Lương cơ bản âm
- ✅ `Validate_WithFutureHireDate_ShouldFail` - Ngày tuyển dụng trong tương lai

#### CreateDepartmentValidatorTests
- ✅ `Validate_WithValidData_ShouldSucceed` - Dữ liệu hợp lệ
- ✅ `Validate_WithEmptyName_ShouldFail` - Tên phòng ban trống
- ✅ `Validate_WithNullName_ShouldFail` - Tên phòng ban null
- ✅ `Validate_WithTooLongName_ShouldFail` - Tên phòng ban quá dài

### 3. Feature Handlers Tests

#### GetAllDepartmentsQueryHandlerTests
- ✅ `Handle_WithValidRequest_ShouldReturnPagedDepartments` - Lấy danh sách phòng ban
- ✅ `Handle_WithDifferentPageSize_ShouldReturnCorrectPageSize` - Kiểm tra kích thước trang khác nhau
- ✅ `Handle_WhenRepositoryThrowsException_ShouldThrow` - Xử lý exception

#### CreatePositionCommandHandlerTests
- ✅ `Handle_WithValidCommand_ShouldCreatePosition` - Tạo vị trí mới
- ✅ `Handle_WithDuplicatePositionCode_ShouldThrowConflictException` - Kiểm tra mã vị trí trùng
- ✅ `Handle_WhenSaveChangesFails_ShouldThrow` - Xử lý lỗi lưu

### 4. Extension Tests

#### StringValidationExtensionsTests
- ✅ `IsNullOrEmpty_Works` - Kiểm tra chuỗi rỗng/null
- ✅ `Sanitize_Works` - Sanitize chuỗi XSS
- ✅ `IsValidEmail_Works` - Kiểm tra email hợp lệ
- ✅ `IsValidPhoneNumber_Works` - Kiểm tra số điện thoại VN
- ✅ `IsValidNationalId_Works` - Kiểm tra CMND/CCCD
- ✅ `ContainsSqlInjectionPatterns_Works` - Kiểm tra SQL injection
- ✅ `IsValidDateRange_Works` - Kiểm tra khoảng ngày hợp lệ
- ✅ `IsValidAge_Works` - Kiểm tra tuổi hợp lệ
- ✅ `IsValidSalary_Works` - Kiểm tra lương hợp lệ

## 🛠️ Công Nghệ Sử Dụng

- **Framework**: xUnit
- **Mocking**: Moq
- **Validation Testing**: FluentValidation.TestHelper
- **.NET Version**: .NET 8

## 📦 Dependencies

```xml
<ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.4.0" />
    <PackageReference Include="xunit" Version="2.5.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
    <PackageReference Include="Moq" Version="4.20.1" />
    <PackageReference Include="FluentValidation" Version="11.8.0" />
</ItemGroup>
```

## 🚀 Chạy Tests

### Visual Studio
1. Mở Test Explorer (Test > Test Explorer hoặc Ctrl+E, T)
2. Click "Run All Tests" hoặc chọn test cụ thể

### Command Line
```bash
# Chạy tất cả tests
dotnet test tests/ERP.HRM.Application.Tests/

# Chạy test cụ thể
dotnet test tests/ERP.HRM.Application.Tests/ --filter "DepartmentServiceTests"

# Chạy và hiển thị coverage
dotnet test tests/ERP.HRM.Application.Tests/ /p:CollectCoverageMetrics=true
```

## 📝 Hướng Dẫn Viết Test Mới

### Pattern cơ bản
```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedBehavior()
{
    // Arrange - Chuẩn bị dữ liệu
    var mockService = new Mock<IService>();
    
    // Act - Thực thi code cần test
    var result = await service.MethodAsync();
    
    // Assert - Kiểm tra kết quả
    Assert.NotNull(result);
    mockService.Verify(x => x.SomeMethod(), Times.Once);
}
```

### Naming Convention
- **Fact**: Test đơn lẻ không có tham số
- **Theory**: Test có tham số (InlineData, MemberData, etc.)
- **Naming Format**: `MethodName_Scenario_ExpectedBehavior`

## 📊 Test Coverage Goals

- **Services**: ≥ 90%
- **Validators**: ≥ 95%
- **Handlers**: ≥ 85%
- **Overall**: ≥ 80%

## ✨ Best Practices

1. **Isolate Tests**: Mỗi test chỉ test một điều
2. **Use Mocks**: Mock external dependencies
3. **Clear Names**: Tên test phải mô tả rõ ràng
4. **Arrange-Act-Assert**: Luôn tuân thủ AAA pattern
5. **No Test Dependencies**: Tests độc lập, không phụ thuộc lẫn nhau
6. **Fast Execution**: Tests phải chạy nhanh
7. **Deterministic**: Kết quả test phải giống nhau mỗi lần chạy

## 🎯 Các Component Cần Thêm Unit Test

Dưới đây là danh sách các component nên có unit test bổ sung:

### 🔴 Ưu tiên Cao (Priority 1)
- [ ] `PositionService` - Các CRUD operations
- [ ] `EmployeeService` - Các CRUD operations  
- [ ] `EmploymentContractValidator` - Validation rules
- [ ] `LeaveRequestValidator` - Validation rules
- [ ] `DeleteDepartmentCommandHandler`
- [ ] `UpdatePositionCommandHandler`
- [ ] `UpdateEmployeeCommandHandler`
- [ ] `RecordAttendanceCommandHandler`

### 🟡 Ưu tiên Trung Bình (Priority 2)
- [ ] `EnhancedPayrollService` - Complex payroll logic
- [ ] `SalaryAdjustmentDecisionValidator`
- [ ] `InsuranceParticipationValidator`
- [ ] `GetPositionByIdQueryHandler`
- [ ] `GetEmployeeByIdQueryHandler`
- [ ] `GetPayrollRecordsByPeriodQueryHandler`

### 🟢 Ưu tiên Thấp (Priority 3)
- [ ] `CreateDepartmentCommandHandler`
- [ ] `GetAllPositionsQueryHandler`
- [ ] Các repository methods
- [ ] Middleware tests
- [ ] Controller tests

## 📞 Liên Hệ

Nếu có câu hỏi hoặc muốn thêm test, vui lòng liên hệ team development.

---

**Last Updated**: 2024
**Version**: 1.0
