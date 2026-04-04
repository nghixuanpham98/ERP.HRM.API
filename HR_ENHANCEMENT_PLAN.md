# HR Enhancement Plan for Monthly Salary System

## Current State Analysis

### ✅ What Already Exists

#### 1. **Employee Entity** (`ERP.HRM.Domain/Entities/Employee.cs`)
**Current Fields:**
- Basic Info: `EmployeeCode`, `FullName`, `DateOfBirth`, `Gender`, `Email`, `PhoneNumber`, `Address`, `NationalId`
- Work Info: `DepartmentId`, `PositionId`, `ManagerId`, `JobTitle`, `Status`, `HireDate`
- Salary Info: `BaseSalary`, `Allowance`
- **Contract Info (Already Present!):**
  - `ContractType` (string) - Type of contract
  - `ContractStartDate` (DateOnly)
  - `ContractEndDate` (DateOnly)
  - `ProbationEndDate` (DateOnly)
- Relationships: `Department`, `Position`, `Manager`

**Analysis:** 
- ✅ Contract tracking is partially implemented in Employee entity (denormalized)
- ⚠️ Not ideal structure for complex contracts but functional for basic needs

#### 2. **PayrollRecord Entity** (`ERP.HRM.Domain/Entities/PayrollRecord.cs`)
**Current Fields:**
- Salary Components: `BaseSalary`, `Allowance`, `OvertimeCompensation`, `GrossSalary`
- Deduction Breakdown: `InsuranceDeduction`, `TaxDeduction`, `OtherDeductions`, `TotalDeductions`
- Result: `NetSalary`
- Tracking: `Status` (Draft/Calculated/Approved/Paid), `PaymentDate`, `Notes`
- Period Tracking: `WorkingDays`, `ProductionTotal`
- **Relationships: Collection of `PayrollDeduction`** ✅

#### 3. **PayrollDeduction Entity** (`ERP.HRM.Domain/Entities/PayrollDeduction.cs`)
**Current Fields:**
- `DeductionType` (string) - e.g., "BHXH", "Thuế", "Vay nhân viên", "Phạt"
- `Description` (string)
- `Amount` (decimal)
- `Reason` (string)
- **Already designed to track detailed deductions!** ✅

**Analysis:**
- ✅ Allows logging of detailed deduction breakdowns
- ✅ Flexible for custom deduction types
- ✅ Good foundation for enhanced deduction tracking

#### 4. **SalaryConfiguration Entity** (`ERP.HRM.Domain/Entities/SalaryConfiguration.cs`)
- Stores salary type, base rates, insurance %, tax %
- Per-employee configuration
- **Note:** Uses flat percentage rates (simple model)

### ❌ What's Missing

#### 1. **Employment Contract Management**
**Missing:**
- Separate `EmploymentContract` entity for detailed contract tracking
- Contract history (multiple contracts per employee)
- Contract template/type definitions
- Contract amendment tracking

**Current Workaround:** 
- Denormalized in `Employee` entity (ContractType, ContractStartDate, ContractEndDate, ProbationEndDate)
- **Works for basic needs, but not flexible for complex contract management**

#### 2. **Salary Grade / Position Level System**
**Missing:**
- `SalaryGrade` entity (e.g., Level 1, 2, 3, Senior, Manager, etc.)
- Grade salary bands (min, mid, max)
- Grade progression rules
- History of grade changes for an employee
- **Note:** Employee has `PositionId` but Position doesn't define salary grades

**Impact on Payroll:**
- Can't determine salary range validations
- Can't track salary progression decisions
- Can't enforce grade-based salary rules

#### 3. **Family Dependent Deductions**
**Missing:**
- `FamilyDependent` or `Dependent` entity
- Fields: Relationship, DOB, Dependent status
- Dependent tax relief configuration
- Dependent deduction calculation rules

**Vietnamese Tax Context:**
- TNCN (Personal Income Tax) allows "giảm trừ gia cảnh" (family dependent deductions)
- Each dependent typically reduces taxable income
- Missing this feature underestimates actual tax deductions

#### 4. **Insurance Rate Management (BHXH)**
**Current:** 
- Flat percentage in `SalaryConfiguration` (e.g., 8%)

**Missing:**
- `InsuranceConfiguration` or `InsuranceTier` entity
- Insurance bands based on salary ranges
- Different insurance types (health, unemployment, work injury)
- Employee vs. Employer contributions tracking
- Insurance rate history/changes

**Impact:**
- Real Vietnamese BHXH has tiered rates based on salary
- Can't model complex insurance scenarios
- Over-simplified calculation

#### 5. **Personal Income Tax (PIT / Thuế TNCN) Management**
**Current:**
- Flat percentage in `SalaryConfiguration` (e.g., 5%)

**Missing:**
- `TaxBracket` or `TaxConfiguration` entity
- Tax bracket definitions (income ranges and rates)
- Non-taxable income thresholds
- Tax deduction rules (dependents, insurance, etc.)
- Tax exemption tracking
- Regional tax variations (if applicable)

**Impact:**
- Real Vietnamese PIT has progressive tax brackets
- Simplified 5% flat rate is inaccurate
- Can't model dependent deductions
- No accommodation for tax allowances

#### 6. **Salary Increase/Decrease Decision Tracking**
**Missing:**
- `SalaryDecision` entity to log when and why salary changed
- Decision type (increase, decrease, promotion, etc.)
- Effective date
- Decision approver
- Historical audit trail

**Current Gap:**
- No way to track WHY salary changed
- No approval workflow for decisions
- Can't audit salary changes

---

## Proposed Entities to Create

### Phase 1: High Priority (Core HR)

#### 1. **EmploymentContract** (New Detailed Entity)
```csharp
public class EmploymentContract : BaseEntity
{
    public int EmploymentContractId { get; set; }
    public int EmployeeId { get; set; }
    public string ContractNumber { get; set; } = null!;
    public string ContractType { get; set; } = null!; // "Full-time", "Part-time", "Contract", "Seasonal"
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? ProbationEndDate { get; set; }
    public bool IsActive { get; set; }
    public string? TerminationReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    public virtual Employee Employee { get; set; } = null!;
}
```

#### 2. **SalaryGrade** (New)
```csharp
public class SalaryGrade : BaseEntity
{
    public int SalaryGradeId { get; set; }
    public string GradeName { get; set; } = null!; // "Level 1", "Level 2", "Senior", etc.
    public int GradeLevel { get; set; } // 1, 2, 3... for sorting
    public decimal MinSalary { get; set; }
    public decimal MidSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
    public DateTime EffectiveDate { get; set; }
    
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
```

#### 3. **FamilyDependent** (New)
```csharp
public class FamilyDependent : BaseEntity
{
    public int FamilyDependentId { get; set; }
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = null!;
    public string Relationship { get; set; } = null!; // "Spouse", "Child", "Parent", etc.
    public DateOnly DateOfBirth { get; set; }
    public bool IsQualified { get; set; } = true; // For tax deduction qualification
    public DateOnly? QualificationStartDate { get; set; }
    public DateOnly? QualificationEndDate { get; set; }
    public string? NationalId { get; set; }
    public string? Notes { get; set; }
    
    public virtual Employee Employee { get; set; } = null!;
}
```

### Phase 2: Medium Priority (Tax & Insurance)

#### 4. **TaxBracket** (New)
```csharp
public class TaxBracket : BaseEntity
{
    public int TaxBracketId { get; set; }
    public string BracketName { get; set; } = null!; // e.g., "0-5M", "5M-10M"
    public decimal MinIncome { get; set; }
    public decimal MaxIncome { get; set; }
    public decimal TaxRate { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}
```

#### 5. **InsuranceTier** (New)
```csharp
public class InsuranceTier : BaseEntity
{
    public int InsuranceTierId { get; set; }
    public string TierName { get; set; } = null!; // e.g., "Tier 1", "Tier 2"
    public string InsuranceType { get; set; } = null!; // "Health", "Unemployment", "WorkInjury"
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public decimal EmployeeRate { get; set; } // Employee contribution %
    public decimal EmployerRate { get; set; } // Employer contribution %
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}
```

### Phase 3: Audit & Decisions

#### 6. **SalaryAdjustmentDecision** (New)
```csharp
public class SalaryAdjustmentDecision : BaseEntity
{
    public int SalaryAdjustmentDecisionId { get; set; }
    public int EmployeeId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? ApprovedByUserId { get; set; }
    public string DecisionType { get; set; } = null!; // "Increase", "Decrease", "Promotion", "Demotion"
    public decimal OldBaseSalary { get; set; }
    public decimal NewBaseSalary { get; set; }
    public decimal SalaryChange { get; set; } // NewBaseSalary - OldBaseSalary
    public DateOnly EffectiveDate { get; set; }
    public string Reason { get; set; } = null!;
    public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected", "Applied"
    public DateTime DecisionDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? EffectiveImplementationDate { get; set; }
    public string? ApprovalNotes { get; set; }
    
    public virtual Employee Employee { get; set; } = null!;
}
```

---

## Integration Points with Monthly Salary Calculation

### Current Calculation (Simple):
```
GrossSalary = (BaseSalary / TotalWorkingDays) × ActualWorkingDays + Allowance + OT

Deductions:
- Insurance = GrossSalary × 8%
- Tax = GrossSalary × 5%

NetSalary = GrossSalary - Insurance - Tax - OtherDeductions
```

### Enhanced Calculation (With HR Integration):

```
1. **Determine Base Salary:**
   - Use current effective SalaryAdjustmentDecision (if any)
   - Or use Employee.BaseSalary
   - Cross-reference with SalaryGrade for validation

2. **Calculate Gross Salary:**
   - DailySalary = BaseSalary / TotalWorkingDays
   - GrossSalary = (DailySalary × ActualWorkingDays) 
                   + Allowance 
                   + CalculateOvertimeCompensation()

3. **Calculate Insurance Deduction (BHXH):**
   - Lookup InsuranceTier based on GrossSalary
   - InsuranceDeduction = GrossSalary × EmployeeRate%
   - Log separately in PayrollDeduction

4. **Calculate Tax Deduction (TNCN):**
   a. Calculate Taxable Income:
      - Start with GrossSalary
      - Subtract InsuranceDeduction (BHXH contribution is deductible)
      - Subtract NonTaxableThreshold (e.g., 11M VND in Vietnam)
   
   b. Lookup TaxBracket based on taxable income
   
   c. Calculate Dependent Deductions:
      - Count FamilyDependents where IsQualified = true
      - Apply dependent deduction formula (e.g., 9.6M/dependent/year)
   
   d. Apply Tax = TaxBracket calculation - Dependent deductions
   
   e. Log in PayrollDeduction

5. **Other Deductions:**
   - Loans, fines, etc.
   - Log each in PayrollDeduction

6. **Net Salary:**
   - NetSalary = GrossSalary - Insurance - Tax - OtherDeductions
```

---

## Implementation Roadmap

### **Step 1: Create New Entities** (Phase 1)
- [ ] Create `EmploymentContract` entity
- [ ] Create `SalaryGrade` entity
- [ ] Create `FamilyDependent` entity
- [ ] Create `SalaryAdjustmentDecision` entity
- [ ] Update `Employee` entity to add `SalaryGradeId` foreign key (keep contract fields for backward compatibility)

### **Step 2: Database Migrations**
- [ ] Create EF Core migration for all new entities
- [ ] Add foreign keys and constraints
- [ ] Add seed data for initial SalaryGrades
- [ ] Update database

### **Step 3: Create Repositories & DTOs**
- [ ] Add repositories for each new entity
- [ ] Create DTOs for API operations (Create, Update, Read)
- [ ] Add validators for DTOs

### **Step 4: Create API Controllers**
- [ ] `EmploymentContractsController` - CRUD for contracts
- [ ] `SalaryGradesController` - CRUD for grades
- [ ] `FamilyDependentsController` - CRUD for dependents
- [ ] `SalaryAdjustmentDecisionsController` - Create and approve decisions

### **Step 5: Enhance PayrollService**
- [ ] Modify `CalculateMonthlySalaryAsync()` to use new entities
- [ ] Implement `CalculateInsuranceDeduction()` with tiers
- [ ] Implement `CalculateTaxDeduction()` with brackets and dependents
- [ ] Update `CalculateDeductionsAsync()` to log detailed deductions
- [ ] Add validation against SalaryGrade bands

### **Step 6: Testing & Validation**
- [ ] Unit tests for calculation logic with examples
- [ ] Integration tests for full payroll flow
- [ ] Test with various dependent counts
- [ ] Test with various salary ranges

### **Step 7: Documentation & Training**
- [ ] Update API documentation
- [ ] Create usage examples
- [ ] Document Vietnamese payroll rules
- [ ] Create migration guide for existing data

---

## Vietnamese Payroll Context (Rules to Implement)

### **BHXH (Insurance) - Current Implementation**
- ✅ Payroll already tracks `InsuranceDeduction`
- ❌ Using flat 8% rate
- 📋 **To Fix:** Implement tiered rates based on salary bands

### **TNCN (Personal Income Tax) - Current Implementation**
- ✅ Payroll already tracks `TaxDeduction`
- ❌ Using flat 5% rate
- 📋 **To Fix:** 
  - Implement tax brackets (typically 5%, 10%, 20%, 35%)
  - Account for non-taxable threshold (~11M VND/month)
  - Implement dependent deductions (~9.6M/dependent/year)
  - Subtract insurance contribution from taxable income

### **Giảm Trừ Gia Cảnh (Dependent Deductions) - Current Implementation**
- ❌ Not implemented
- 📋 **To Add:** 
  - FamilyDependent entity
  - Dependent deduction logic in tax calculation
  - Qualification tracking

---

## Data Structure Summary

### New Tables Needed:
1. `EmploymentContracts` - Detailed contract history
2. `SalaryGrades` - Grade definitions with salary bands
3. `FamilyDependents` - Employee family members
4. `SalaryAdjustmentDecisions` - Audit trail of salary changes
5. `TaxBrackets` - Tax rate brackets (optional - could be config)
6. `InsuranceTiers` - Insurance rate tiers (optional - could be config)

### Existing Tables to Enhance:
1. `Employees` - Add `SalaryGradeId` FK (optional, keep ContractType fields)
2. `PayrollRecords` - Already has structure for detailed deductions
3. `PayrollDeductions` - Already supports detailed deduction logging ✅

---

## Implementation Notes

### What Works Well (Reuse)
- ✅ `PayrollRecord` structure is excellent for tracking salary components
- ✅ `PayrollDeduction` table is perfect for detailed deduction logging
- ✅ CQRS pattern with MediatR enables clean service integration
- ✅ Clean Architecture allows new features without breaking existing code

### What Needs Change
- 🔄 `PayrollService.CalculateMonthlySalaryAsync()` - Enhance with new logic
- 🔄 `CalculateMonthlySalaryCommandHandler` - Update to use new entities
- 🔄 DTOs for salary calculation - Add contract, grade, dependent info

### Backward Compatibility
- ✅ All new entities are additions - no existing code breaks
- ✅ Existing `Employee` fields can coexist with `SalaryGradeId` FK
- ⚠️ Need migration script to populate `SalaryGradeId` from existing data
- ⚠️ `PayrollService` changes are internal - API remains compatible

---

## Success Criteria

- ✅ Monthly salary calculation matches Vietnamese BHXH and TNCN regulations
- ✅ Dependent deductions properly reduce taxable income
- ✅ Insurance and tax deductions are logged with detail in `PayrollDeduction`
- ✅ Salary changes are auditable via `SalaryAdjustmentDecision`
- ✅ Grade progression is tracked and validated
- ✅ Build succeeds with no errors
- ✅ Existing tests still pass
- ✅ New features have test coverage

---

## Next Steps Recommendation

**Immediate Priority:**
1. Review this plan and confirm entities and calculations
2. Create the new entities (Step 1)
3. Add database migrations (Step 2)
4. Start with Phase 1 entities (Contract, Grade, Dependent)

**Would you like me to:**
- [ ] Create the new entity classes?
- [ ] Create the EF Core migration?
- [ ] Create the DTOs and validators?
- [ ] Modify the PayrollService with enhanced calculations?
- [ ] Create the API controllers?
- [ ] Create unit tests for new calculation logic?

Let me know which part you'd like to tackle first!
