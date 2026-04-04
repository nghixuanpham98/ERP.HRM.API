# 📊 PAYROLL SYSTEM - COMPREHENSIVE REVIEW

## REQUIREMENT CHECKLIST

### ✅ REQUIREMENT 1: LƯƠNG THÁNG (Monthly Salary)
**For:** Văn phòng, Cơ khí, Kỹ thuật, Tổ trưởng, etc.  
**Formula:** `(Lương cơ bản / Tổng ngày công kỳ) × Ngày công thực tế + Phụ cấp + Bù giờ tăng ca - Trừ`

#### Implementation Status: ✅ COMPLETE

**Code Files:**
- `ERP.HRM.Application/Features/Payroll/Commands/CalculateMonthlySalaryCommand.cs`
- `ERP.HRM.Application/Features/Payroll/Handlers/CalculateMonthlySalaryCommandHandler.cs`
- `ERP.HRM.Application/Services/PayrollService.cs` → `CalculateMonthlySalaryAsync()`

**Logic Details:**
```csharp
// 1. Lấy thông tin cơ bản
- Employee (nhân viên)
- PayrollPeriod (kỳ lương)
- SalaryConfiguration (cấu hình lương)

// 2. Tính ngày công thực tế
actualWorkingDays = SUM(Attendance.WorkingDays)
→ Được quản lý trong Attendance table

// 3. Tính lương cơ bản theo ngày công
dailySalary = BaseSalary / period.TotalWorkingDays
calculatedBaseSalary = dailySalary × actualWorkingDays

// 4. Cộng phụ cấp
allowance = SalaryConfiguration.Allowance ?? 0

// 5. Cộng bù giờ tăng ca
totalOvertimeHours = SUM(Attendance.OvertimeHours)
overtimeCompensation = (dailySalary / 8) × totalOvertimeHours × overtimeMultiplier

// 6. Tính lương brutto
grossSalary = calculatedBaseSalary + allowance + overtimeCompensation

// 7. Trừ các khoản
insurance = grossSalary × insuranceRate%
tax = grossSalary × taxRate%
totalDeductions = insurance + tax + other

// 8. Tính lương ròng
netSalary = grossSalary - totalDeductions
```

**Database Support:**
- ✅ `Attendances` table - Track ngày công (0, 0.5, 1)
- ✅ `Attendances.OvertimeHours` - Track tăng ca
- ✅ `Attendances.OvertimeMultiplier` - Hệ số tăng ca (mặc định 1.5)
- ✅ `PayrollPeriods.TotalWorkingDays` - Tổng ngày công kỳ
- ✅ `PayrollRecords.Status` - Quản lý trạng thái (Draft, Calculated, Approved, Paid)

**API Endpoints:**
```
POST /api/payroll/calculate-monthly-salary
Body: {
  "employeeId": 1,
  "payrollPeriodId": 1,
  "overrideBaseSalary": null,
  "overrideAllowance": null
}
Response: {
  "payrollRecordId": 1,
  "employeeName": "Nguyễn Văn A",
  "baseSalary": 10000000,
  "allowance": 500000,
  "overtimeCompensation": 200000,
  "grossSalary": 10700000,
  "insuranceDeduction": 856000,
  "taxDeduction": 535000,
  "netSalary": 9309000
}
```

**Validators Applied:**
- ✅ EmployeeId > 0
- ✅ PayrollPeriodId > 0
- ✅ OverrideBaseSalary >= 0 (nếu có)
- ✅ OverrideAllowance >= 0 (nếu có)

---

### ✅ REQUIREMENT 2: LƯƠNG SẢN LƯỢNG (Production-Based Salary)
**For:** Công nhân ở xưởng / factory workers  
**Formula:** `Đơn giá sản phẩm × Sản lượng + Phụ cấp - Trừ`

#### Implementation Status: ✅ COMPLETE

**Code Files:**
- `ERP.HRM.Application/Features/Payroll/Commands/CalculateProductionSalaryCommand.cs`
- `ERP.HRM.Application/Features/Payroll/Handlers/CalculateProductionSalaryCommandHandler.cs`
- `ERP.HRM.Application/Services/PayrollService.cs` → `CalculateProductionSalaryAsync()`

**Logic Details:**
```csharp
// 1. Lấy thông tin cơ bản
- Employee (công nhân)
- PayrollPeriod (kỳ lương)
- SalaryConfiguration (cấu hình lương)

// 2. Tính tổng sản lượng
productionTotal = SUM(ProductionOutput.Amount)
           where Amount = Quantity × UnitPrice

// 3. Lương cơ bản = Tổng sản lượng
calculatedBaseSalary = productionTotal

// 4. Cộng phụ cấp
allowance = SalaryConfiguration.Allowance ?? 0

// 5. Tính lương brutto (không tính tăng ca)
grossSalary = calculatedBaseSalary + allowance

// 6. Trừ các khoản
insurance = grossSalary × insuranceRate%
tax = grossSalary × taxRate%
totalDeductions = insurance + tax + other

// 7. Tính lương ròng
netSalary = grossSalary - totalDeductions
```

**Database Support:**
- ✅ `ProductionOutputs` table - Track sản lượng chi tiết
- ✅ `ProductionOutputs.Quantity` - Sản lượng
- ✅ `ProductionOutputs.UnitPrice` - Đơn giá tại thời điểm sản xuất
- ✅ `ProductionOutputs.Amount` - Tính sẵn (Quantity × UnitPrice)
- ✅ `ProductionOutputs.ProductionDate` - Ngày sản xuất
- ✅ `ProductionOutputs.QualityStatus` - Trạng thái chất lượng (OK, Defective, Rework)
- ✅ `PayrollRecords.ProductionTotal` - Lưu tổng sản lượng

**API Endpoints:**
```
POST /api/payroll/calculate-production-salary
Body: {
  "employeeId": 5,
  "payrollPeriodId": 1,
  "overrideUnitPrice": null,
  "overrideAllowance": null
}
Response: {
  "payrollRecordId": 2,
  "employeeName": "Trần Thị B",
  "baseSalary": 5000000,    // = productionTotal
  "allowance": 200000,
  "overtimeCompensation": 0, // Không có tăng ca
  "grossSalary": 5200000,
  "insuranceDeduction": 416000,
  "taxDeduction": 260000,
  "productionTotal": 5000000,
  "netSalary": 4524000
}
```

**Validators Applied:**
- ✅ EmployeeId > 0
- ✅ PayrollPeriodId > 0
- ✅ OverrideUnitPrice >= 0 (nếu có)
- ✅ OverrideAllowance >= 0 (nếu có)

---

## 📋 DETAILED FEATURE MATRIX

| Feature | Monthly | Production | Status |
|---------|---------|-----------|--------|
| **Basic Calculation** | ✅ | ✅ | Complete |
| **Attendance Tracking** | ✅ | ❌ | N/A for production |
| **Overtime Compensation** | ✅ | ❌ | N/A for production |
| **Production Output Tracking** | ❌ | ✅ | Complete |
| **Allowances** | ✅ | ✅ | Complete |
| **Insurance Deduction** | ✅ | ✅ | Complete |
| **Tax Deduction** | ✅ | ✅ | Complete |
| **Other Deductions** | ✅ | ✅ | Complete |
| **Payroll Record Storage** | ✅ | ✅ | Complete |
| **Salary Slip Generation** | ✅ | ✅ | Complete |
| **Override Salary** | ✅ | ✅ | Complete |

---

## 🗄️ DATABASE SCHEMA

### Core Payroll Tables

#### 1. `PayrollPeriods`
```sql
- PayrollPeriodId (PK)
- Year
- Month
- PeriodName (e.g., "Tháng 1/2024")
- StartDate
- EndDate
- TotalWorkingDays
- IsFinalized
- FinalizedDate
```

#### 2. `SalaryConfigurations`
```sql
- SalaryConfigurationId (PK)
- EmployeeId (FK)
- SalaryType (Monthly=1, Production=2, Hourly=3)
- BaseSalary
- UnitPrice (for production)
- HourlyRate (for hourly)
- Allowance
- InsuranceRate (%) default 8
- TaxRate (%) default 5
- EffectiveFrom
- EffectiveTo
- IsActive
```

#### 3. `Attendances` (For Monthly Salary)
```sql
- AttendanceId (PK)
- EmployeeId (FK)
- PayrollPeriodId (FK)
- AttendanceDate
- WorkingDays (0, 0.5, 1)
- IsPresent
- OvertimeHours
- OvertimeMultiplier (default 1.5)
- Note (reason: "Sick", "Leave", etc.)
- UNIQUE INDEX: (EmployeeId, PayrollPeriodId, AttendanceDate)
```

#### 4. `Products` (For Production Salary)
```sql
- ProductId (PK)
- ProductCode
- ProductName
- Description
- Unit (e.g., "cái", "bộ")
- Category
- Status (Active/Inactive)
```

#### 5. `ProductionOutputs` (For Production Salary)
```sql
- ProductionOutputId (PK)
- EmployeeId (FK)
- PayrollPeriodId (FK)
- ProductId (FK)
- Quantity
- UnitPrice (at time of production)
- Amount (Quantity × UnitPrice)
- ProductionDate
- QualityStatus (OK, Defective, Rework)
- Notes
- INDEX: (EmployeeId, PayrollPeriodId)
```

#### 6. `PayrollRecords` (Salary Slip)
```sql
- PayrollRecordId (PK)
- EmployeeId (FK)
- PayrollPeriodId (FK)
- SalaryType (Monthly, Production, Hourly)
- BaseSalary (calculated based on type)
- Allowance
- OvertimeCompensation (0 for production)
- GrossSalary = BaseSalary + Allowance + OvertimeCompensation
- InsuranceDeduction
- TaxDeduction
- OtherDeductions
- TotalDeductions
- NetSalary = GrossSalary - TotalDeductions
- WorkingDays (for monthly)
- ProductionTotal (for production)
- Status (Draft, Calculated, Approved, Paid)
- PaymentDate
- Notes
- UNIQUE INDEX: (EmployeeId, PayrollPeriodId)
```

#### 7. `PayrollDeductions`
```sql
- PayrollDeductionId (PK)
- PayrollRecordId (FK)
- DeductionType (BHXH, Thuế, Vay, Phạt, etc.)
- Description
- Amount
- Reason
```

---

## 📊 WORKFLOW OVERVIEW

### Monthly Salary Flow
```
1. Employee creates/updates Attendance records
   - RecordAttendanceCommand
   - POST /api/payroll/record-attendance
   
2. HR calculates monthly salary
   - CalculateMonthlySalaryCommand
   - POST /api/payroll/calculate-monthly-salary
   - System fetches: Attendance, SalaryConfiguration, PayrollPeriod
   - Calculates: BaseSalary, Allowance, OvertimeCompensation, Deductions
   - Stores: PayrollRecord
   
3. Generate salary slip
   - GetSalarySlipQuery
   - GET /api/payroll/salary-slip/{employeeId}/{payrollPeriodId}
   - Returns: SalarySlipDto with all details
```

### Production Salary Flow
```
1. Employee records production output
   - RecordProductionOutputCommand
   - POST /api/payroll/record-production-output
   - Stores: Quantity, UnitPrice, ProductionDate
   
2. HR calculates production salary
   - CalculateProductionSalaryCommand
   - POST /api/payroll/calculate-production-salary
   - System fetches: ProductionOutputs, SalaryConfiguration
   - Calculates: ProductionTotal, Allowance, Deductions
   - Stores: PayrollRecord with ProductionTotal
   
3. Generate salary slip
   - Same as monthly
```

---

## 🔧 CONFIGURATION DETAILS

### Employee Salary Configuration
**Entity:** `SalaryConfiguration`

Must set before calculating salary:
```csharp
{
  "employeeId": 1,
  "salaryType": "Monthly",          // or Production
  "baseSalary": 10000000,           // Monthly only
  "unitPrice": null,                // Production only
  "allowance": 500000,
  "insuranceRate": 8,               // %
  "taxRate": 5,                     // %
  "effectiveFrom": "2024-01-01"
}
```

### Deduction Rates (Configurable per employee)
- **Insurance (BHXH):** Default 8% of GrossSalary
- **Tax (Thuế TNCN):** Default 5% of GrossSalary
- **Other:** Additional custom deductions as needed

---

## 📈 CALCULATION EXAMPLES

### Example 1: Monthly Salary (Office Employee)
```
Employee: Nguyễn Văn A
Period: Tháng 1/2024 (22 working days)

Input:
- BaseSalary: 10,000,000
- Allowance: 500,000
- WorkingDays: 20 (2 days leave)
- OvertimeHours: 8 (1 day × 8 hours)
- OvertimeMultiplier: 1.5

Calculation:
- DailySalary = 10,000,000 / 22 = 454,545.45
- CalculatedBaseSalary = 454,545.45 × 20 = 9,090,909
- OvertimeCompensation = (454,545.45 / 8) × 8 × 1.5 = 681,818.25
- GrossSalary = 9,090,909 + 500,000 + 681,818.25 = 10,272,727.25
- Insurance (8%) = 821,818.18
- Tax (5%) = 513,636.36
- TotalDeductions = 1,335,454.54
- NetSalary = 10,272,727.25 - 1,335,454.54 = 8,937,272.71
```

### Example 2: Production Salary (Factory Worker)
```
Employee: Trần Thị B
Period: Tháng 1/2024

Production Output:
- Product A: Quantity 1000 × UnitPrice 2,000 = 2,000,000
- Product B: Quantity 500 × UnitPrice 4,000 = 2,000,000
- Total Amount: 4,000,000

Input:
- ProductionTotal: 4,000,000
- Allowance: 200,000

Calculation:
- CalculatedBaseSalary = 4,000,000
- GrossSalary = 4,000,000 + 200,000 = 4,200,000
- Insurance (8%) = 336,000
- Tax (5%) = 210,000
- TotalDeductions = 546,000
- NetSalary = 4,200,000 - 546,000 = 3,654,000
```

---

## ✅ VALIDATION RULES

### CalculateMonthlySalaryCommand Validation
```csharp
✓ EmployeeId > 0
✓ PayrollPeriodId > 0
✓ OverrideBaseSalary >= 0 (if provided)
✓ OverrideAllowance >= 0 (if provided)
✓ Employee exists
✓ PayrollPeriod exists
✓ SalaryConfiguration exists and is active
```

### CalculateProductionSalaryCommand Validation
```csharp
✓ EmployeeId > 0
✓ PayrollPeriodId > 0
✓ OverrideUnitPrice >= 0 (if provided)
✓ OverrideAllowance >= 0 (if provided)
✓ Employee exists
✓ PayrollPeriod exists
✓ SalaryConfiguration exists and is active
```

### RecordAttendanceCommand Validation
```csharp
✓ EmployeeId > 0
✓ PayrollPeriodId > 0
✓ AttendanceDate not in future
✓ WorkingDays between 0 and 1 (0, 0.5, 1)
✓ OvertimeHours >= 0 (if provided)
```

---

## 🚀 API ENDPOINTS SUMMARY

### Monthly Salary Endpoints
```
POST   /api/payroll/calculate-monthly-salary          [Admin, HR]
GET    /api/payroll/records/by-period/{periodId}     [Authorized]
GET    /api/payroll/salary-slip/{empId}/{periodId}   [Authorized]
```

### Production Salary Endpoints
```
POST   /api/payroll/calculate-production-salary       [Admin, HR]
POST   /api/payroll/record-production-output          [Admin, HR]
GET    /api/payroll/production/{empId}/{periodId}    [Authorized]
```

### Common Endpoints
```
POST   /api/payroll/record-attendance                 [Admin, HR]
GET    /api/payroll/attendance/{empId}/{periodId}    [Authorized]
```

---

## 🎯 IMPLEMENTATION CHECKLIST

### Core Features
- ✅ Monthly salary calculation (by working days)
- ✅ Production salary calculation (by output)
- ✅ Attendance tracking (0, 0.5, 1 day)
- ✅ Overtime compensation
- ✅ Deduction management (insurance, tax, other)
- ✅ Salary slip generation
- ✅ Production output recording
- ✅ Payroll period management

### Database
- ✅ PayrollPeriods table
- ✅ SalaryConfigurations table
- ✅ Attendances table (with unique index)
- ✅ PayrollRecords table (with unique index)
- ✅ PayrollDeductions table
- ✅ Products table
- ✅ ProductionOutputs table (with index)
- ✅ Foreign key relationships
- ✅ Soft delete support
- ✅ Audit trail (CreatedDate, ModifiedDate)

### Validation
- ✅ Input validation (FluentValidation)
- ✅ Business logic validation
- ✅ Duplicate prevention (unique indexes)

### API
- ✅ Commands for calculation
- ✅ Queries for retrieval
- ✅ Authorization (Admin, HR roles)
- ✅ Error handling
- ✅ Logging

### DI Configuration
- ✅ PayrollService registered
- ✅ All repositories registered
- ✅ Database context configured

---

## 🔍 MISSING FEATURES (Optional Enhancements)

1. **Batch Calculation**
   - Calculate salary for all employees in a period at once
   - `CalculateBatchSalariesCommand`

2. **Salary Advance**
   - Allow employees to request advance salary
   - Deduct from next payroll

3. **Tax Calculation**
   - Current: Simple percentage-based
   - Suggested: Progressive tax based on gross salary

4. **Bonus/Penalty Management**
   - Customizable bonuses
   - Penalties for violations

5. **Multi-level Deductions**
   - Primary deduction: Insurance
   - Secondary deduction: Tax
   - Tertiary deduction: Custom

6. **Payroll Approval Workflow**
   - Draft → Pending Approval → Approved → Paid
   - Approval by manager/HR

7. **Export Payroll Report**
   - Export to Excel/PDF
   - Bank transfer file generation

8. **Year-to-Date (YTD) Calculation**
   - Track cumulative salary
   - Track cumulative deductions

---

## 📝 SUMMARY

| Aspect | Status | Notes |
|--------|--------|-------|
| **Monthly Salary** | ✅ Complete | By working days (0, 0.5, 1) |
| **Production Salary** | ✅ Complete | By unit price × quantity |
| **Attendance Tracking** | ✅ Complete | Supports partial days & overtime |
| **Deduction Management** | ✅ Complete | Insurance, Tax, Other |
| **Database Schema** | ✅ Complete | Fully normalized with FK & indexes |
| **API Endpoints** | ✅ Complete | All CRUD operations |
| **Validation** | ✅ Complete | FluentValidation implemented |
| **Error Handling** | ✅ Complete | Try-catch with logging |
| **Authorization** | ✅ Complete | Role-based access control |
| **DI Configuration** | ✅ Complete | All services registered |

---

## 🎓 READY FOR PRODUCTION? ✅ YES

**All required features for both salary types are implemented, tested, and ready to use.**

### Quick Start
1. Create PayrollPeriod for month
2. Set SalaryConfiguration for each employee
3. Record Attendance (for monthly) or ProductionOutput (for production)
4. Calculate salary using CalculateMonthlySalaryCommand or CalculateProductionSalaryCommand
5. Generate salary slip with GetSalarySlipQuery

---

**Generated:** 2024-2026  
**Version:** 1.0.0  
**Status:** Production Ready ✅
