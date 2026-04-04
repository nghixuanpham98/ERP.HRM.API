# 🚀 HR Enhancement System - Usage Guide

## Getting Started

### 1. Start the Application

```bash
cd C:\Users\xxxqmfrman\Desktop\Learn\ERP.HRM.API\
dotnet run --project ERP.HRM.API
```

Application starts at: `http://localhost:5000` (or port shown in output)

### 2. Access Swagger UI

Open browser and go to:
```
http://localhost:5000/swagger
```

---

## 📚 Complete Workflow Example

### Step 1: Create Salary Grades

**Endpoint:** `POST /api/salarygrades`

**Request Body:**
```json
{
  "gradeName": "Level 1",
  "gradeLevel": 1,
  "minSalary": 5000000,
  "midSalary": 7500000,
  "maxSalary": 10000000,
  "description": "Entry-level position",
  "effectiveDate": "2024-01-01T00:00:00Z"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Tạo bậc lương thành công",
  "data": {
    "salaryGradeId": 1,
    "gradeName": "Level 1",
    "gradeLevel": 1,
    "minSalary": 5000000,
    "midSalary": 7500000,
    "maxSalary": 10000000,
    "isActive": true,
    "description": "Entry-level position",
    "effectiveDate": "2024-01-01"
  }
}
```

### Step 2: Create Tax Brackets (Vietnamese TNCN)

**Endpoint:** `POST /api/taxbrackets`

**Request Body:**
```json
{
  "bracketName": "10M-20M",
  "minIncome": 10000000,
  "maxIncome": 20000000,
  "taxRate": 15,
  "effectiveDate": "2024-01-01T00:00:00Z"
}
```

### Step 3: Create Insurance Tiers (Vietnamese BHXH)

**Endpoint:** `POST /api/insurancetiers`

**Request Body:**
```json
{
  "tierName": "Tier 2",
  "insuranceType": "Health",
  "minSalary": 15000000,
  "maxSalary": 30000000,
  "employeeRate": 6.0,
  "employerRate": 8.0,
  "effectiveDate": "2024-01-01T00:00:00Z"
}
```

### Step 4: Create Employment Contract

**Endpoint:** `POST /api/employmentcontracts`

**Request Body:**
```json
{
  "employeeId": 1,
  "contractNumber": "HD-2024-001",
  "contractType": "Full-time",
  "startDate": "2024-01-01",
  "endDate": null,
  "probationEndDate": "2024-04-01"
}
```

### Step 5: Add Family Dependents

**Endpoint:** `POST /api/familydependents`

**Request Body:**
```json
{
  "employeeId": 1,
  "fullName": "Nguyễn Thị B",
  "relationship": "Spouse",
  "dateOfBirth": "1990-05-15",
  "qualificationStartDate": "2024-01-01",
  "qualificationEndDate": null,
  "nationalId": "012345678910",
  "notes": "Vợ của nhân viên"
}
```

### Step 6: Create Salary Adjustment Decision

**Endpoint:** `POST /api/salaryadjustmentdecisions`

**Request Body:**
```json
{
  "employeeId": 1,
  "decisionType": "Promotion",
  "oldBaseSalary": 20000000,
  "newBaseSalary": 25000000,
  "effectiveDate": "2024-03-01",
  "reason": "Nâng cao chức vụ từ Staff lên Senior Staff"
}
```

**Response (Pending):**
```json
{
  "salaryAdjustmentDecisionId": 1,
  "status": "Pending",
  "decisionDate": "2024-02-20T10:00:00Z"
}
```

### Step 7: Approve Salary Decision

**Endpoint:** `POST /api/salaryadjustmentdecisions/1/approve`

**Request Body:**
```json
{
  "approvedByUserId": 1,
  "status": "Approved",
  "approvalNotes": "Đã phê duyệt - Hiệu lực từ 01/03/2024"
}
```

**Response (Applied):**
```json
{
  "salaryAdjustmentDecisionId": 1,
  "status": "Applied",
  "approvedDate": "2024-02-20T11:00:00Z",
  "effectiveImplementationDate": "2024-02-20T11:00:00Z"
}
```

---

## 💰 Enhanced Payroll Calculation

### Using Enhanced Payroll Service

The system now supports advanced payroll calculation with Vietnamese compliance:

**Key Features:**
- ✅ Tiered Insurance (BHXH) - Based on salary bands
- ✅ Progressive Tax (TNCN) - Based on tax brackets
- ✅ Dependent Deductions - Per qualified family member
- ✅ Non-taxable Threshold - 11M VND standard
- ✅ Audit Trail - All deductions logged

### Example Calculation

**Employee Data:**
```
Name: Nguyễn Văn A
Base Salary: 30,000,000 VND
Working Days: 22/22
Allowance: 1,000,000 VND
Dependents: 2 children (qualified)
```

**Calculation Process:**

1. **Gross Salary**
   ```
   = (30,000,000 / 22) × 22 + 1,000,000
   = 30,000,000 + 1,000,000
   = 31,000,000 VND
   ```

2. **Insurance (BHXH) - Tiered**
   ```
   Salary Range: 30M-40M → 8% employee rate
   = 31,000,000 × 8%
   = 2,480,000 VND
   ```

3. **Tax (TNCN) - Progressive with Dependents**
   ```
   Taxable Income = 31,000,000 - 2,480,000 - 11,000,000 = 17,520,000
   Dependent Deduction = 2 × 800,000 = 1,600,000
   Taxable After Dependents = 17,520,000 - 1,600,000 = 15,920,000
   
   Tax Bracket: 10M-20M → 15% rate
   = 15,920,000 × 15%
   = 2,388,000 VND
   ```

4. **Net Salary**
   ```
   = 31,000,000 - 2,480,000 - 2,388,000
   = 26,132,000 VND
   ```

---

## 📋 API Reference

### Authentication

All endpoints require Bearer Token:

```http
Authorization: Bearer <your_jwt_token>
```

### Common Response Format

**Success Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* entity data */ }
}
```

**Error Response:**
```json
{
  "success": false,
  "message": "Error description",
  "data": null
}
```

### Salary Grades

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/salarygrades` | Get all salary grades |
| GET | `/api/salarygrades/{id}` | Get specific grade |
| POST | `/api/salarygrades` | Create new grade |
| PUT | `/api/salarygrades/{id}` | Update grade |
| DELETE | `/api/salarygrades/{id}` | Delete grade |

### Employment Contracts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/employmentcontracts` | Get all contracts |
| GET | `/api/employmentcontracts/employee/{employeeId}` | Get contracts for employee |
| GET | `/api/employmentcontracts/employee/{employeeId}/active` | Get active contract |
| POST | `/api/employmentcontracts` | Create new contract |
| PUT | `/api/employmentcontracts/{id}` | Update contract |
| DELETE | `/api/employmentcontracts/{id}` | Delete contract |

### Family Dependents

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/familydependents` | Get all dependents |
| GET | `/api/familydependents/employee/{employeeId}` | Get employee's dependents |
| GET | `/api/familydependents/employee/{employeeId}/qualified` | Get qualified for tax |
| POST | `/api/familydependents` | Create new dependent |
| PUT | `/api/familydependents/{id}` | Update dependent |
| DELETE | `/api/familydependents/{id}` | Delete dependent |

### Salary Adjustment Decisions

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/salaryadjustmentdecisions` | Get all decisions |
| GET | `/api/salaryadjustmentdecisions/employee/{employeeId}` | Get employee decisions |
| GET | `/api/salaryadjustmentdecisions/status/pending` | Get pending decisions |
| POST | `/api/salaryadjustmentdecisions` | Create new decision |
| POST | `/api/salaryadjustmentdecisions/{id}/approve` | Approve/reject decision |
| PUT | `/api/salaryadjustmentdecisions/{id}` | Update decision |
| DELETE | `/api/salaryadjustmentdecisions/{id}` | Delete decision |

### Tax Brackets

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/taxbrackets` | Get all brackets |
| GET | `/api/taxbrackets/active` | Get active brackets |
| POST | `/api/taxbrackets` | Create new bracket |
| PUT | `/api/taxbrackets/{id}` | Update bracket |
| DELETE | `/api/taxbrackets/{id}` | Delete bracket |

### Insurance Tiers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/insurancetiers` | Get all tiers |
| GET | `/api/insurancetiers/active` | Get active tiers |
| GET | `/api/insurancetiers/type/{type}` | Get tiers by type |
| POST | `/api/insurancetiers` | Create new tier |
| PUT | `/api/insurancetiers/{id}` | Update tier |
| DELETE | `/api/insurancetiers/{id}` | Delete tier |

---

## 🔐 Authorization Levels

### Public (No Auth Required)
- None (all endpoints require authentication)

### All Authenticated Users
- GET endpoints (view data)

### HR Role
- Create salary grades
- Create employment contracts
- Create family dependents
- Create salary decisions
- Update salary decisions (before approval)

### Admin Role
- All CRUD operations
- Approve/reject salary decisions
- Delete any record
- Manage tax brackets and insurance tiers

---

## ⚙️ Configuration

### Non-Taxable Threshold (TNCN)
Located in `EnhancedPayrollService.cs`:
```csharp
private const decimal NON_TAXABLE_THRESHOLD = 11_000_000m; // Vietnamese standard
```

### Dependent Deduction (Annual)
```csharp
private const decimal DEPENDENT_DEDUCTION_ANNUAL = 9_600_000m; // Per dependent per year
private const decimal DEPENDENT_DEDUCTION_MONTHLY = DEPENDENT_DEDUCTION_ANNUAL / 12; // Monthly
```

### Fallback Insurance Rate (if no tier found)
```csharp
return grossSalary * 0.08m; // 8% default
```

### Fallback Tax Rate (if no bracket found)
```csharp
return grossSalary * 0.05m; // 5% default
```

---

## 🧪 Testing Scenarios

### Scenario 1: Employee with No Dependents
- Create employee with no dependents
- Create contract
- Process payroll
- Verify tax calculation without dependent deductions

### Scenario 2: Employee with Dependents
- Create employee
- Add 2-3 family dependents
- Create contract
- Process payroll
- Verify dependent deductions applied

### Scenario 3: Salary Increase Decision
- Create initial salary decision (Pending)
- View pending decisions
- Approve decision
- Verify employee salary updated
- Verify decision marked as Applied

### Scenario 4: High Salary (Multiple Tax Brackets)
- Create employee with high salary (50M+)
- Verify tiered insurance applied
- Verify progressive tax in higher bracket
- Verify deductions calculated correctly

---

## 🐛 Troubleshooting

### No Insurance Tier Found
- **Cause:** No tier defined for salary range
- **Solution:** Create InsuranceTier covering the salary range
- **Fallback:** Uses 8% default rate

### No Tax Bracket Found
- **Cause:** No bracket defined for income range
- **Solution:** Create TaxBracket covering the income range
- **Fallback:** Uses 5% default rate

### Salary Adjustment Not Applied
- **Cause:** Decision status not "Approved"
- **Solution:** Use `/approve` endpoint to approve first
- **Verify:** Check Status field = "Applied"

### Dependent Not Counted for Tax
- **Cause:** Dependent IsQualified = false or outside qualification dates
- **Solution:** Update dependent IsQualified = true and set valid qualification dates
- **Verify:** Use `/qualified` endpoint to check

---

## 📊 Database Queries (Advanced)

### Get Employees with Salary Decisions

```sql
SELECT e.EmployeeCode, e.FullName, sad.DecisionType, 
       sad.OldBaseSalary, sad.NewBaseSalary, sad.Status
FROM Employees e
JOIN SalaryAdjustmentDecisions sad ON e.EmployeeId = sad.EmployeeId
WHERE sad.Status = 'Applied'
ORDER BY sad.EffectiveImplementationDate DESC;
```

### Get Employees with Dependents for Tax Relief

```sql
SELECT e.EmployeeCode, e.FullName, COUNT(fd.FamilyDependentId) as DependentCount,
       COUNT(fd.FamilyDependentId) * 800000 as MonthlyDeduction
FROM Employees e
JOIN FamilyDependents fd ON e.EmployeeId = fd.EmployeeId
WHERE fd.IsQualified = 1 
  AND (fd.QualificationStartDate IS NULL OR fd.QualificationStartDate <= CAST(GETDATE() AS DATE))
  AND (fd.QualificationEndDate IS NULL OR fd.QualificationEndDate >= CAST(GETDATE() AS DATE))
GROUP BY e.EmployeeId, e.EmployeeCode, e.FullName;
```

### Get Active Employment Contracts

```sql
SELECT e.EmployeeCode, e.FullName, ec.ContractNumber, 
       ec.ContractType, ec.StartDate, ec.EndDate, ec.IsActive
FROM Employees e
JOIN EmploymentContracts ec ON e.EmployeeId = ec.EmployeeId
WHERE ec.IsActive = 1
  AND ec.StartDate <= CAST(GETDATE() AS DATE)
  AND (ec.EndDate IS NULL OR ec.EndDate >= CAST(GETDATE() AS DATE));
```

---

## 📞 Support

For issues or questions:
1. Check this guide first
2. Check `HR_ENHANCEMENT_COMPLETE.md` for implementation details
3. Check Swagger documentation at `/swagger`
4. Check application logs in console

---

**Happy Payroll Processing!** 💼

*Last Updated: [Today's Date]*
