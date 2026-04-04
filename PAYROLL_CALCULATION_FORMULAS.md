# 💰 PAYROLL CALCULATION FORMULAS - DETAILED REFERENCE

## FORMULA COMPARISON

### 1️⃣ MONTHLY SALARY FORMULA (Lương Tháng)

#### **Complete Formula:**
```
NetSalary = GrossSalary - TotalDeductions

Where:
  GrossSalary = BaseSalary + Allowance + OvertimeCompensation
  
  BaseSalary = (BaseSalaryPerMonth / TotalWorkingDaysInPeriod) × ActualWorkingDays
  
  OvertimeCompensation = (DailySalary / 8) × TotalOvertimeHours × OvertimeMultiplier
  
  TotalDeductions = InsuranceDeduction + TaxDeduction + OtherDeductions
  
  InsuranceDeduction = GrossSalary × InsuranceRate%
  TaxDeduction = GrossSalary × TaxRate%
```

#### **Variables:**
| Variable | Type | Description | Example |
|----------|------|-------------|---------|
| `BaseSalaryPerMonth` | decimal | Base salary per month from config | 10,000,000 |
| `TotalWorkingDaysInPeriod` | int | Total working days in month | 22 |
| `ActualWorkingDays` | decimal | Days actually worked (0, 0.5, 1) | 20 |
| `OvertimeHours` | decimal | Total overtime hours in period | 8 |
| `OvertimeMultiplier` | decimal | OT rate multiplier | 1.5x |
| `Allowance` | decimal | Monthly allowance | 500,000 |
| `InsuranceRate` | decimal | Insurance % of gross | 8% |
| `TaxRate` | decimal | Tax % of gross | 5% |

#### **Step-by-Step Calculation:**

**Step 1: Calculate Daily Salary**
```
DailySalary = 10,000,000 / 22 = 454,545.45 VND/day
```

**Step 2: Calculate Base Salary (by working days)**
```
BaseSalary = 454,545.45 × 20 = 9,090,909 VND
(Employee worked 20 days, had 2 days leave)
```

**Step 3: Calculate Overtime Compensation**
```
OvertimeCompensation = (454,545.45 / 8) × 8 hours × 1.5 = 681,818 VND
(Employee worked 1 extra day = 8 hours)
```

**Step 4: Add Allowance**
```
Subtotal = 9,090,909 + 500,000 + 681,818 = 10,272,727 VND
```

**Step 5: Calculate Deductions**
```
Insurance = 10,272,727 × 8% = 821,818 VND
Tax = 10,272,727 × 5% = 513,636 VND
TotalDeductions = 821,818 + 513,636 = 1,335,454 VND
```

**Step 6: Calculate Net Salary**
```
NetSalary = 10,272,727 - 1,335,454 = 8,937,273 VND
```

#### **Payroll Record Fields (Monthly):**
```csharp
PayrollRecordId = AUTO
EmployeeId = 1
PayrollPeriodId = 1
SalaryType = "Monthly"
BaseSalary = 9,090,909         // Step 2
Allowance = 500,000
OvertimeCompensation = 681,818  // Step 3
GrossSalary = 10,272,727        // Step 4
InsuranceDeduction = 821,818    // Step 5
TaxDeduction = 513,636          // Step 5
OtherDeductions = 0
TotalDeductions = 1,335,454     // Step 5
NetSalary = 8,937,273           // Step 6
WorkingDays = 20
ProductionTotal = NULL
Status = "Calculated"
```

---

### 2️⃣ PRODUCTION SALARY FORMULA (Lương Sản Lượng)

#### **Complete Formula:**
```
NetSalary = GrossSalary - TotalDeductions

Where:
  GrossSalary = ProductionTotal + Allowance
  
  ProductionTotal = SUM(Quantity × UnitPrice) for all products
  
  TotalDeductions = InsuranceDeduction + TaxDeduction + OtherDeductions
  
  InsuranceDeduction = GrossSalary × InsuranceRate%
  TaxDeduction = GrossSalary × TaxRate%
```

#### **Variables:**
| Variable | Type | Description | Example |
|----------|------|-------------|---------|
| `ProductionTotal` | decimal | Sum of all production amounts | 4,000,000 |
| `Quantity` | decimal | Items produced | 1000 |
| `UnitPrice` | decimal | Price per unit | 2,000 |
| `Allowance` | decimal | Monthly allowance | 200,000 |
| `InsuranceRate` | decimal | Insurance % of gross | 8% |
| `TaxRate` | decimal | Tax % of gross | 5% |

#### **Step-by-Step Calculation:**

**Step 1: Calculate Production Total**
```
Product A: 1000 units × 2,000 = 2,000,000 VND
Product B: 500 units × 4,000 = 2,000,000 VND
ProductionTotal = 2,000,000 + 2,000,000 = 4,000,000 VND
```

**Step 2: Add Allowance**
```
GrossSalary = 4,000,000 + 200,000 = 4,200,000 VND
```

**Step 3: Calculate Deductions**
```
Insurance = 4,200,000 × 8% = 336,000 VND
Tax = 4,200,000 × 5% = 210,000 VND
TotalDeductions = 336,000 + 210,000 = 546,000 VND
```

**Step 4: Calculate Net Salary**
```
NetSalary = 4,200,000 - 546,000 = 3,654,000 VND
```

#### **Payroll Record Fields (Production):**
```csharp
PayrollRecordId = AUTO
EmployeeId = 5
PayrollPeriodId = 1
SalaryType = "Production"
BaseSalary = 4,000,000         // ProductionTotal
Allowance = 200,000
OvertimeCompensation = 0        // No OT for production
GrossSalary = 4,200,000         // Step 2
InsuranceDeduction = 336,000    // Step 3
TaxDeduction = 210,000          // Step 3
OtherDeductions = 0
TotalDeductions = 546,000       // Step 3
NetSalary = 3,654,000           // Step 4
WorkingDays = NULL
ProductionTotal = 4,000,000     // Step 1
Status = "Calculated"
```

---

## 🔀 SIDE-BY-SIDE COMPARISON

| Component | Monthly | Production |
|-----------|---------|-----------|
| **Base Calculation** | `DailySalary × WorkingDays` | `SUM(Qty × Price)` |
| **Overtime** | ✅ Supported | ❌ Not applicable |
| **Allowance** | ✅ Included | ✅ Included |
| **Deductions** | ✅ Insurance, Tax, Other | ✅ Insurance, Tax, Other |
| **Data Source** | Attendance table | ProductionOutput table |
| **Key Field** | `WorkingDays` | `ProductionTotal` |
| **Typical For** | Office, Technical, Manager | Factory, Workers |
| **Variability** | Depends on attendance | Depends on output |

---

## 📊 DATABASE RECORD TRACKING

### For Monthly Salary

**Attendance Records (Daily):**
```
AttendanceId | EmployeeId | Date       | WorkingDays | OvertimeHours | Note
1            | 1          | 2024-01-01 | 1           | 0             | Regular
2            | 1          | 2024-01-02 | 1           | 0             | Regular
3            | 1          | 2024-01-03 | 0.5         | 2             | Half day + OT
...
22           | 1          | 2024-01-22 | 0           | 0             | Leave
```

**Payroll Record (Monthly):**
```
PayrollRecordId | EmployeeId | Period | Type    | BaseSalary | Gross  | Net
1               | 1          | 202401 | Monthly | 9,090,909  | 10,272,727 | 8,937,273
```

---

### For Production Salary

**Production Output Records (Daily):**
```
ProductionOutputId | EmployeeId | Date       | Product | Qty | UnitPrice | Amount
1                  | 5          | 2024-01-01 | Prod_A  | 100 | 2,000     | 200,000
2                  | 5          | 2024-01-02 | Prod_A  | 200 | 2,000     | 400,000
3                  | 5          | 2024-01-03 | Prod_B  | 50  | 4,000     | 200,000
...
20                 | 5          | 2024-01-20 | Prod_B  | 150 | 4,000     | 600,000
```

**Payroll Record (Monthly):**
```
PayrollRecordId | EmployeeId | Period | Type       | BaseSalary | Gross  | Net       | ProductionTotal
2               | 5          | 202401 | Production | 4,000,000  | 4,200,000 | 3,654,000 | 4,000,000
```

---

## 🧮 CALCULATION CODE FLOW

### Monthly Salary Calculation Flow

```
CalculateMonthlySalaryCommand
    ↓
CalculateMonthlySalaryCommandHandler.Handle()
    ↓
PayrollService.CalculateMonthlySalaryAsync()
    ├─ Get Employee
    ├─ Get PayrollPeriod (TotalWorkingDays)
    ├─ Get SalaryConfiguration (BaseSalary, Allowance, rates)
    ├─ Query Attendance table
    │   ├─ SUM(WorkingDays) → actualWorkingDays
    │   └─ SUM(OvertimeHours) → totalOvertimeHours
    ├─ Calculate DailySalary = BaseSalary / TotalWorkingDays
    ├─ Calculate BaseSalary = DailySalary × actualWorkingDays
    ├─ Calculate OvertimeCompensation = (DailySalary / 8) × totalOvertimeHours × multiplier
    ├─ Add Allowance
    ├─ Calculate GrossSalary
    ├─ Calculate Deductions
    ├─ Create PayrollRecord
    ├─ Save to database
    └─ Return PayrollRecordDto
```

### Production Salary Calculation Flow

```
CalculateProductionSalaryCommand
    ↓
CalculateProductionSalaryCommandHandler.Handle()
    ↓
PayrollService.CalculateProductionSalaryAsync()
    ├─ Get Employee
    ├─ Get PayrollPeriod
    ├─ Get SalaryConfiguration (Allowance, rates)
    ├─ Query ProductionOutput table
    │   └─ SUM(Quantity × UnitPrice) → ProductionTotal
    ├─ Calculate BaseSalary = ProductionTotal
    ├─ Add Allowance
    ├─ Calculate GrossSalary
    ├─ Calculate Deductions (no OT)
    ├─ Create PayrollRecord
    ├─ Save to database
    └─ Return PayrollRecordDto
```

---

## 📋 INPUT/OUTPUT EXAMPLES

### Monthly Salary Example

**Input (Command):**
```json
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "overrideBaseSalary": null,
  "overrideAllowance": null
}
```

**Processing:**
- Query Attendance: 20 working days + 8 OT hours
- Query SalaryConfig: 10M base, 500K allowance, 8% insurance, 5% tax
- Query Period: 22 total working days

**Output (Response):**
```json
{
  "payrollRecordId": 1,
  "employeeId": 1,
  "employeeName": "Nguyễn Văn A",
  "payrollPeriodId": 1,
  "salaryType": "Monthly",
  "baseSalary": 9090909,
  "allowance": 500000,
  "overtimeCompensation": 681818,
  "grossSalary": 10272727,
  "insuranceDeduction": 821818,
  "taxDeduction": 513636,
  "otherDeductions": 0,
  "totalDeductions": 1335454,
  "netSalary": 8937273,
  "workingDays": 20,
  "status": "Calculated"
}
```

---

### Production Salary Example

**Input (Command):**
```json
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "overrideUnitPrice": null,
  "overrideAllowance": null
}
```

**Processing:**
- Query ProductionOutput: 4,000,000 total amount
- Query SalaryConfig: 200K allowance, 8% insurance, 5% tax

**Output (Response):**
```json
{
  "payrollRecordId": 2,
  "employeeId": 5,
  "employeeName": "Trần Thị B",
  "payrollPeriodId": 1,
  "salaryType": "Production",
  "baseSalary": 4000000,
  "allowance": 200000,
  "overtimeCompensation": 0,
  "grossSalary": 4200000,
  "insuranceDeduction": 336000,
  "taxDeduction": 210000,
  "otherDeductions": 0,
  "totalDeductions": 546000,
  "netSalary": 3654000,
  "productionTotal": 4000000,
  "status": "Calculated"
}
```

---

## 🎯 KEY DIFFERENCES SUMMARY

### Monthly Salary (Văn phòng, Cơ khí, Kỹ thuật, etc.)
```
✓ Fixed monthly base salary
✓ Scaled by working days (0, 0.5, 1)
✓ Includes overtime compensation
✓ Less variable, more predictable
✓ Source: Attendance records
```

### Production Salary (Công nhân xưởng)
```
✓ Variable based on output
✓ Calculated from quantity × unit price
✓ No overtime (already in amount)
✓ More variable, performance-based
✓ Source: Production output records
```

---

**Both systems fully implemented and ready for use! ✅**
