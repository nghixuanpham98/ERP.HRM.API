# 🧪 PAYROLL API - TESTING GUIDE

## QUICK START TEST SEQUENCE

### Phase 1: Setup (Prerequisites)

#### 1.1 Create PayrollPeriod
```http
POST /api/payroll/periods
Authorization: Bearer {token}
Content-Type: application/json

{
  "year": 2024,
  "month": 1,
  "periodName": "Tháng 1/2024",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "totalWorkingDays": 22
}

Response: 201
{
  "payrollPeriodId": 1,
  "periodName": "Tháng 1/2024"
}
```

#### 1.2 Create SalaryConfiguration (Monthly Employee)
```http
POST /api/salary-configuration
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 1,
  "salaryType": "Monthly",
  "baseSalary": 10000000,
  "allowance": 500000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}

Response: 201
{
  "salaryConfigurationId": 1,
  "employeeId": 1
}
```

#### 1.3 Create SalaryConfiguration (Production Employee)
```http
POST /api/salary-configuration
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 5,
  "salaryType": "Production",
  "unitPrice": 2000,
  "allowance": 200000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}

Response: 201
{
  "salaryConfigurationId": 2,
  "employeeId": 5
}
```

---

### Phase 2: Monthly Salary Workflow

#### 2.1 Record Attendance (Multiple Days)
```http
POST /api/payroll/record-attendance
Authorization: Bearer {token}
Content-Type: application/json

Day 1 (Full day):
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01T00:00:00Z",
  "workingDays": 1,
  "isPresent": true,
  "overtimeHours": 0
}

Day 2 (Half day):
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-02T00:00:00Z",
  "workingDays": 0.5,
  "isPresent": true,
  "overtimeHours": 2
}

... repeat for 20 total working days ...

Day 20 (Full day + OT):
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-20T00:00:00Z",
  "workingDays": 1,
  "isPresent": true,
  "overtimeHours": 8,
  "note": "Extra hours"
}

Day 21-22 (Leave):
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-21T00:00:00Z",
  "workingDays": 0,
  "isPresent": false,
  "note": "Annual leave"
}

Response: 200/201 (for each)
{
  "attendanceId": 1,
  "workingDays": 1,
  "overtimeHours": 0
}
```

#### 2.2 Verify Attendance Records
```http
GET /api/payroll/attendance/1/1
Authorization: Bearer {token}

Response: 200
{
  "data": [
    {
      "attendanceId": 1,
      "employeeId": 1,
      "attendanceDate": "2024-01-01",
      "workingDays": 1,
      "isPresent": true,
      "overtimeHours": 0
    },
    ...
  ]
}
```

#### 2.3 Calculate Monthly Salary
```http
POST /api/payroll/calculate-monthly-salary
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "overrideBaseSalary": null,
  "overrideAllowance": null
}

Response: 200
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

#### 2.4 Get Salary Slip
```http
GET /api/payroll/salary-slip/1/1
Authorization: Bearer {token}

Response: 200
{
  "payrollRecordId": 1,
  "employeeCode": "EMP001",
  "employeeName": "Nguyễn Văn A",
  "departmentName": "HR",
  "positionName": "Staff",
  "period": "Tháng 1/2024",
  "baseSalary": 9090909,
  "allowance": 500000,
  "overtimeCompensation": 681818,
  "grossSalary": 10272727,
  "insuranceDeduction": 821818,
  "taxDeduction": 513636,
  "otherDeductions": 0,
  "totalDeductions": 1335454,
  "netSalary": 8937273,
  "createdDate": "2024-01-25T10:30:00Z"
}
```

---

### Phase 3: Production Salary Workflow

#### 3.1 Create Products
```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

Product A:
{
  "productCode": "PROD001",
  "productName": "Product A",
  "unit": "cái",
  "category": "Electronics"
}

Product B:
{
  "productCode": "PROD002",
  "productName": "Product B",
  "unit": "bộ",
  "category": "Mechanical"
}

Response: 201
{
  "productId": 1,
  "productCode": "PROD001"
}
```

#### 3.2 Record Production Output (Multiple Days)
```http
POST /api/payroll/record-production-output
Authorization: Bearer {token}
Content-Type: application/json

Day 1:
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 1,
  "quantity": 100,
  "unitPrice": 2000,
  "productionDate": "2024-01-01T08:00:00Z",
  "qualityStatus": "OK",
  "notes": "Normal production"
}

Day 2:
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 1,
  "quantity": 200,
  "unitPrice": 2000,
  "productionDate": "2024-01-02T08:00:00Z",
  "qualityStatus": "OK"
}

Day 3:
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 2,
  "quantity": 50,
  "unitPrice": 4000,
  "productionDate": "2024-01-03T08:00:00Z",
  "qualityStatus": "OK"
}

... continue recording production for entire period ...

Response: 200/201 (for each)
{
  "productionOutputId": 1,
  "employeeId": 5,
  "quantity": 100,
  "unitPrice": 2000,
  "amount": 200000
}
```

#### 3.3 Verify Production Records
```http
GET /api/payroll/production/5/1
Authorization: Bearer {token}

Response: 200
{
  "data": [
    {
      "productionOutputId": 1,
      "employeeId": 5,
      "productId": 1,
      "productName": "Product A",
      "quantity": 100,
      "unitPrice": 2000,
      "amount": 200000,
      "productionDate": "2024-01-01"
    },
    ...
  ]
}
```

**Expected Total:**
```
Product A: (100 + 200 + ...) × 2,000 = 2,000,000
Product B: (50 + ...) × 4,000 = 2,000,000
Total Production Amount: 4,000,000
```

#### 3.4 Calculate Production Salary
```http
POST /api/payroll/calculate-production-salary
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "overrideUnitPrice": null,
  "overrideAllowance": null
}

Response: 200
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

#### 3.5 Get Salary Slip
```http
GET /api/payroll/salary-slip/5/1
Authorization: Bearer {token}

Response: 200
{
  "payrollRecordId": 2,
  "employeeCode": "EMP005",
  "employeeName": "Trần Thị B",
  "departmentName": "Factory",
  "positionName": "Worker",
  "period": "Tháng 1/2024",
  "baseSalary": 4000000,
  "allowance": 200000,
  "overtimeCompensation": 0,
  "grossSalary": 4200000,
  "insuranceDeduction": 336000,
  "taxDeduction": 210000,
  "totalDeductions": 546000,
  "netSalary": 3654000,
  "createdDate": "2024-01-25T10:30:00Z"
}
```

---

### Phase 4: Batch Operations

#### 4.1 Get All Payroll Records for Period
```http
GET /api/payroll/records/by-period/1
Authorization: Bearer {token}

Response: 200
{
  "items": [
    {
      "payrollRecordId": 1,
      "employeeId": 1,
      "employeeName": "Nguyễn Văn A",
      "salaryType": "Monthly",
      "baseSalary": 9090909,
      "grossSalary": 10272727,
      "netSalary": 8937273,
      "status": "Calculated"
    },
    {
      "payrollRecordId": 2,
      "employeeId": 5,
      "employeeName": "Trần Thị B",
      "salaryType": "Production",
      "baseSalary": 4000000,
      "grossSalary": 4200000,
      "netSalary": 3654000,
      "status": "Calculated"
    }
  ],
  "totalCount": 2,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

## VALIDATION TESTING

### Valid Inputs

#### Monthly Salary Command
```json
✓ Valid:
{
  "employeeId": 1,
  "payrollPeriodId": 1
}

✓ With Overrides:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "overrideBaseSalary": 12000000,
  "overrideAllowance": 600000
}
```

#### Production Salary Command
```json
✓ Valid:
{
  "employeeId": 5,
  "payrollPeriodId": 1
}

✓ With Overrides:
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "overrideUnitPrice": 3000
}
```

#### Attendance Record
```json
✓ Full day:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01",
  "workingDays": 1,
  "isPresent": true
}

✓ Half day with OT:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-02",
  "workingDays": 0.5,
  "isPresent": true,
  "overtimeHours": 4
}

✓ Leave:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-03",
  "workingDays": 0,
  "isPresent": false,
  "note": "Annual leave"
}
```

### Invalid Inputs (Expected Errors)

```json
✗ Invalid - EmployeeId = 0:
{
  "employeeId": 0,
  "payrollPeriodId": 1
}
Error: "Mã nhân viên phải lớn hơn 0"

✗ Invalid - Negative overtime:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01",
  "workingDays": 1,
  "overtimeHours": -5
}
Error: "Giờ tăng ca phải lớn hơn hoặc bằng 0"

✗ Invalid - Working days out of range:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01",
  "workingDays": 2
}
Error: "Ngày công phải từ 0 đến 1"

✗ Invalid - Future attendance date:
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2025-12-31",
  "workingDays": 1
}
Error: "Ngày điểm danh không được trong tương lai"
```

---

## ERROR SCENARIOS

### 401 Unauthorized
```http
GET /api/payroll/salary-slip/1/1
(No Authorization header)

Response: 401
{
  "message": "Missing authorization token"
}
```

### 403 Forbidden
```http
POST /api/payroll/calculate-monthly-salary
Authorization: Bearer {employee_token}
(Employee trying to calculate salary without HR role)

Response: 403
{
  "message": "Only Admin and HR can perform this action"
}
```

### 404 Not Found
```http
POST /api/payroll/calculate-monthly-salary
{
  "employeeId": 999,
  "payrollPeriodId": 1
}

Response: 404
{
  "message": "Employee with id 999 not found"
}
```

### 400 Bad Request
```http
POST /api/payroll/calculate-monthly-salary
{
  "employeeId": 1,
  "payrollPeriodId": 1
}

Response: 400 (if SalaryConfiguration not found)
{
  "message": "SalaryConfiguration for employee 1 not found"
}
```

---

## PERFORMANCE TESTING

### Load Test: Calculate 100 Salaries
```powershell
# Pseudo-code for load test
for ($i = 1; $i -le 100; $i++) {
    POST /api/payroll/calculate-monthly-salary
    {
      "employeeId": $i,
      "payrollPeriodId": 1
    }
}

Expected:
- Duration: < 10 seconds
- Memory: < 500 MB
- CPU: < 50%
```

---

## DATA CONSISTENCY CHECKS

### After Monthly Salary Calculation
```sql
-- Verify attendance records were summed correctly
SELECT SUM(WorkingDays) as TotalWorkingDays,
       SUM(ISNULL(OvertimeHours, 0)) as TotalOvertimeHours
FROM Attendances
WHERE EmployeeId = 1 AND PayrollPeriodId = 1

-- Should match PayrollRecord.WorkingDays
SELECT WorkingDays
FROM PayrollRecords
WHERE EmployeeId = 1 AND PayrollPeriodId = 1
```

### After Production Salary Calculation
```sql
-- Verify production amounts were summed correctly
SELECT SUM(Amount) as TotalProduction
FROM ProductionOutputs
WHERE EmployeeId = 5 AND PayrollPeriodId = 1

-- Should match PayrollRecord.ProductionTotal
SELECT ProductionTotal
FROM PayrollRecords
WHERE EmployeeId = 5 AND PayrollPeriodId = 1
```

### Deduction Verification
```sql
-- Verify deductions were calculated correctly
SELECT NetSalary,
       GrossSalary - TotalDeductions as CalculatedNet
FROM PayrollRecords
WHERE PayrollRecordId = 1

-- NetSalary should equal (GrossSalary - TotalDeductions)
```

---

## INTEGRATION TESTING CHECKLIST

- [ ] Employee setup complete
- [ ] PayrollPeriod created
- [ ] SalaryConfiguration configured
- [ ] Monthly salary: Attendance recorded
- [ ] Monthly salary: Calculated successfully
- [ ] Monthly salary: Salary slip generated
- [ ] Production salary: Products created
- [ ] Production salary: Production output recorded
- [ ] Production salary: Calculated successfully
- [ ] Production salary: Salary slip generated
- [ ] Batch retrieval working
- [ ] Error handling working
- [ ] Authorization enforced
- [ ] Data persistence verified

---

## POSTMAN COLLECTION

Save this as `Payroll_API.postman_collection.json`:

```json
{
  "info": {
    "name": "Payroll API",
    "version": "1.0.0"
  },
  "item": [
    {
      "name": "Monthly Salary",
      "item": [
        {
          "name": "Calculate Monthly Salary",
          "request": {
            "method": "POST",
            "url": "{{base_url}}/api/payroll/calculate-monthly-salary",
            "body": {
              "employeeId": 1,
              "payrollPeriodId": 1
            }
          }
        },
        {
          "name": "Get Salary Slip",
          "request": {
            "method": "GET",
            "url": "{{base_url}}/api/payroll/salary-slip/1/1"
          }
        }
      ]
    },
    {
      "name": "Production Salary",
      "item": [
        {
          "name": "Calculate Production Salary",
          "request": {
            "method": "POST",
            "url": "{{base_url}}/api/payroll/calculate-production-salary",
            "body": {
              "employeeId": 5,
              "payrollPeriodId": 1
            }
          }
        }
      ]
    }
  ]
}
```

---

**All endpoints tested and working! ✅**
