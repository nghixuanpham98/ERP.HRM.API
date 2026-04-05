# 🛠️ CHI TIẾT CÁC HÀNH ĐỘNG CẦN THỰC HIỆN

## PHẦN I: THIẾU CÁC SERVICES BẮT BUỘC (Priority 1)

### 1️⃣ ILeaveManagementService - THIẾU HOÀN TOÀN

#### Interface cần tạo:
```csharp
namespace ERP.HRM.Application.Interfaces;

public interface ILeaveManagementService
{
    // Leave Request Management
    Task<LeaveRequestDto> SubmitLeaveRequestAsync(CreateLeaveRequestDto dto);
    Task<LeaveRequestDto> GetLeaveRequestAsync(int id);
    Task<PagedResult<LeaveRequestDto>> GetEmployeeLeaveRequestsAsync(
        int employeeId, int pageNumber, int pageSize);
    
    // Approval Workflow
    Task<LeaveRequestDto> ApproveLeaveRequestAsync(int id, string approverNotes = "");
    Task<LeaveRequestDto> RejectLeaveRequestAsync(int id, string rejectionReason);
    Task<LeaveRequestDto> CancelLeaveRequestAsync(int id, string cancelReason);
    
    // Leave Balance
    Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year);
    Task<decimal> CalculateRemainingLeaveDaysAsync(int employeeId, int year);
    Task<IEnumerable<LeaveBalanceHistoryDto>> GetLeaveHistoryAsync(int employeeId, int year);
    
    // Validation
    Task<ValidationResult> ValidateLeaveRequestAsync(CreateLeaveRequestDto dto);
}
```

#### Key Business Rules cần implement:
- ✅ Số ngày phép được cấp theo công ty (default 12 ngày/năm)
- ✅ Kiểm tra overlap (không được phép duplicate dates)
- ✅ Kiểm tra leave balance
- ✅ Kiểm tra leave types (annual, sick, unpaid, etc.)
- ✅ Kiểm tra minimum notice period (thường 5-7 ngày)
- ✅ Kiểm tra blackout dates (ngày không được phép, e.g., Tết)
- ✅ Approval chain (Manager → HR Manager → Finance nếu unpaid)
- ✅ Leave balance carryover rules (có thể cộng dồn không quá 5 ngày)

#### Unit Tests cần viết (15-20 tests):
1. Submit valid leave request - status should be Pending
2. Submit leave request with overlap - should fail
3. Submit leave request exceeding balance - should fail
4. Approve leave request - status should be Approved, balance updated
5. Reject leave request - status should be Rejected
6. Get leave balance - should reflect approved leaves
7. Carryover logic - old year balance → new year
8. Blackout date validation
9. Minimum notice period validation
10. Different leave type handling (sick, unpaid, etc.)

---

### 2️⃣ IInsuranceManagementService - THIẾU HOÀN TOÀN

#### Interface cần tạo:
```csharp
namespace ERP.HRM.Application.Interfaces;

public interface IInsuranceManagementService
{
    // Insurance Registration
    Task<InsuranceParticipationDto> RegisterEmployeeInsuranceAsync(
        CreateInsuranceParticipationDto dto);
    Task<InsuranceParticipationDto> UpdateInsuranceAsync(
        int id, UpdateInsuranceParticipationDto dto);
    Task<InsuranceParticipationDto> TerminateInsuranceAsync(
        int employeeId, DateTime effectiveDate);
    
    // Insurance Info
    Task<InsuranceParticipationDto> GetActiveInsuranceAsync(int employeeId);
    Task<IEnumerable<InsuranceParticipationDto>> GetInsuranceHistoryAsync(
        int employeeId);
    
    // Contributions
    Task<InsuranceContributionDto> CalculateInsuranceContributionAsync(
        decimal salary, int insuranceTierId);
    Task<decimal> GetEmployeeContributionAsync(int employeeId, string insuranceType);
    Task<decimal> GetEmployerContributionAsync(int employeeId, string insuranceType);
    
    // Reporting
    Task<InsuranceSummaryReportDto> GenerateMonthlyInsuranceReportAsync(
        int month, int year);
}
```

#### Key Business Rules:
- ✅ Insurance types: BHXH (Social), BHYT (Health), BHTN (Unemployment)
- ✅ Contribution rates (VN 2024):
  - BHXH: Employee 8%, Employer 17.5%
  - BHYT: Employee 1.5%, Employer 2.75%
  - BHTN: Employee 0.5%, Employer 0.5%
- ✅ Salary threshold (300k - 20 triệu)
- ✅ Tiered insurance rates based on salary bands
- ✅ Insurance effective/termination dates
- ✅ Dependent coverage rules

#### Unit Tests (18-25 tests):
1. Register insurance with valid salary - should succeed
2. Register insurance with invalid salary (< 300k) - should fail
3. Update insurance - effective date should be applied
4. Terminate insurance - effective date lock
5. Calculate contributions - verify rates
6. Multiple insurance overlaps - should fail
7. Insurance history - should show all records
8. Tiered insurance calculations
9. Dependent benefits calculations
10. Insurance claim status tracking

---

### 3️⃣ IEmploymentContractService - BÓNG BẠCH

#### Interface cần tạo:
```csharp
namespace ERP.HRM.Application.Interfaces;

public interface IEmploymentContractService
{
    // Contract Management
    Task<EmploymentContractDto> CreateContractAsync(
        CreateEmploymentContractDto dto);
    Task<EmploymentContractDto> RenewContractAsync(
        int contractId, CreateEmploymentContractDto dto);
    Task<EmploymentContractDto> TerminateContractAsync(
        int contractId, string terminationReason, 
        TerminationType type = TerminationType.Resignation);
    
    // Contract Info
    Task<EmploymentContractDto> GetActiveContractAsync(int employeeId);
    Task<EmploymentContractDto> GetContractAsync(int id);
    Task<IEnumerable<EmploymentContractDto>> GetContractHistoryAsync(
        int employeeId);
    
    // Validation & Analysis
    Task<bool> IsContractActiveAsync(int employeeId);
    Task<int> GetRemainingDaysAsync(int contractId);
    Task<IEnumerable<ContractExpiryWarningDto>> GetExpiringContractsAsync(
        int daysThreshold = 60);
    
    // Archive
    Task<EmploymentContractDto> ArchiveContractAsync(int contractId);
}
```

#### Key Business Rules:
- ✅ Contract types: Permanent, Fixed-term, Probation
- ✅ Validate dates: startDate < endDate
- ✅ Prevent overlap contracts
- ✅ Probation contract → Permanent promotion logic
- ✅ Contract renewal logic
- ✅ Termination types: Resignation, Termination, Retirement, Retirement for age
- ✅ Notice period validation

#### Unit Tests (12-15 tests):
1. Create valid contract - should succeed
2. Create contract with invalid dates - should fail
3. Contract overlap detection - should fail
4. Get active contract - should return current one
5. Renew contract - old becomes archive
6. Terminate contract with different types
7. Get expiring contracts - should list correctly
8. Contract status transitions

---

### 4️⃣ IPersonnelTransferService - BÓNG BẠCH

#### Interface cần tạo:
```csharp
namespace ERP.HRM.Application.Interfaces;

public interface IPersonnelTransferService
{
    // Transfer Management
    Task<PersonnelTransferDto> CreateTransferAsync(
        CreatePersonnelTransferDto dto);
    Task<PersonnelTransferDto> ApproveTransferAsync(
        int id, string approverComment = "");
    Task<PersonnelTransferDto> ExecuteTransferAsync(int id);
    Task<PersonnelTransferDto> CancelTransferAsync(
        int id, string cancelReason);
    
    // Transfer Info
    Task<PersonnelTransferDto> GetTransferAsync(int id);
    Task<IEnumerable<PersonnelTransferDto>> GetEmployeeTransferHistoryAsync(
        int employeeId);
    
    // Validation
    Task<ValidationResult> ValidateTransferAsync(
        CreatePersonnelTransferDto dto);
}
```

#### Key Business Rules:
- ✅ Transfer types: Promotion, Lateral, Demotion, Redeployment
- ✅ Effective date must be in future
- ✅ Validate new department/position exist
- ✅ Employee status must be active
- ✅ Department head approval required
- ✅ New department head approval required
- ✅ Update employee department, position, salary grade
- ✅ Create salary adjustment decision if salary changes

#### Unit Tests (10-12 tests):
1. Create transfer - status Pending
2. Transfer to invalid position - fail
3. Transfer inactive employee - fail
4. Approve transfer - status Approved
5. Execute transfer - employee updated
6. Salary adjustment on promotion
7. Transfer history tracking

---

### 5️⃣ IResignationManagementService - BÓNG BẠCH

#### Interface cần tạo:
```csharp
namespace ERP.HRM.Application.Interfaces;

public interface IResignationManagementService
{
    // Resignation Process
    Task<ResignationDecisionDto> CreateResignationAsync(
        CreateResignationDecisionDto dto);
    Task<ResignationDecisionDto> ApproveResignationAsync(
        int id, string approverComment = "");
    Task<ResignationDecisionDto> ProcessResignationAsync(int id);
    
    // Resignation Info
    Task<ResignationDecisionDto> GetResignationAsync(int id);
    Task<IEnumerable<ResignationDecisionDto>> GetEmployeeResignationsAsync(
        int employeeId);
    
    // Settlement Calculation
    Task<ResignationSettlementDto> CalculateSettlementAsync(int employeeId);
    Task<decimal> CalculateFinalPayAsync(int employeeId);
    Task<IEnumerable<DeductionDto>> CalculateDeductionsAsync(int employeeId);
}
```

#### Key Business Rules:
- ✅ Resignation types: Voluntary, Involuntary, Retirement
- ✅ Notice period: 30 ngày (Vietnamese law)
- ✅ Effective date = application date + notice period
- ✅ Settlement calculation:
  - Final salary (current month + proportional)
  - Unused annual leave
  - Bonuses (if applicable)
  - Deductions: advance salary, damages
- ✅ Clearance check (all leaves closed, all debts settled)
- ✅ Update employee status to Resigned
- ✅ Archive employee

#### Unit Tests (12-15 tests):
1. Create resignation - status Submitted
2. Approve resignation
3. Calculate settlement - verify amounts
4. Process resignation - employee status changed
5. Unused leave calculation
6. Notice period validation

---

## PHẦN II: BỔ SUNG VÀO EXISTING SERVICES

### 6️⃣ Extend IPayrollService

**Thêm methods**:
```csharp
// Trong PayrollService, thêm:
Task<PayrollRecordDto> CalculateBackpayAsync(int employeeId, int startMonth, int endMonth);
Task<PayrollRecordDto> CalculateSeverancePayAsync(int employeeId);
Task<BonusCalculationDto> CalculateBonusAsync(int employeeId, int year, decimal bonusRate);
Task<PayrollReconciliationDto> ReconcilePayrollAsync(int payrollPeriodId);
Task<byte[]> ExportPayrollAsync(int payrollPeriodId, ExportFormat format);
Task<PayrollValidationResultDto> ValidateSalaryComputationAsync(int payrollPeriodId);
```

**Unit Tests cần viết**: 10-12 tests

---

### 7️⃣ Complete IEnhancedPayrollService Implementation

**Hiện tại chỉ có interface, cần implement logic**:
```csharp
public class EnhancedPayrollService : IEnhancedPayrollService
{
    // Implement all interface methods with:
    
    // 1. CalculateTieredInsuranceAsync
    // - Get insurance tier based on salary
    // - Calculate deduction: salary * tier.rate
    
    // 2. CalculateProgressiveTaxAsync
    // - Get tax bracket based on salary
    // - Apply progressive formula
    // - Deduct dependent allowances
    // - Calculate TNCN tax
    
    // 3. GetDependentDeductionAsync
    // - Get family dependents count
    // - Apply dependent allowance per dependent
    // - Total deduction = count * allowance
}
```

**Unit Tests cần viết**: 15-20 tests

---

## PHẦN III: API ENDPOINTS CẦN THÊM/HOÀN THIỆN

### Trong các Controllers, cần thêm endpoints:

#### LeaveRequestsController - Thêm:
```
PUT    /api/leave-requests/{id}/approve
PUT    /api/leave-requests/{id}/reject
DELETE /api/leave-requests/{id}
GET    /api/leave-requests/{employeeId}/balance
GET    /api/leave-requests/{employeeId}/history
```

#### ResignationDecisionsController - Thêm:
```
PUT    /api/resignations/{id}/approve
PUT    /api/resignations/{id}/process
GET    /api/resignations/{employeeId}/settlement
GET    /api/resignations/{employeeId}/final-pay
```

#### PersonnelTransfersController - Thêm:
```
PUT    /api/transfers/{id}/approve
PUT    /api/transfers/{id}/execute
PUT    /api/transfers/{id}/cancel
GET    /api/transfers/{employeeId}/history
```

#### EmploymentContractsController - Thêm:
```
POST   /api/contracts/{id}/renew
PUT    /api/contracts/{id}/terminate
GET    /api/contracts/{employeeId}/active
GET    /api/contracts/expiring?days=60
PUT    /api/contracts/{id}/archive
```

#### InsuranceParticipationsController - Thêm:
```
PUT    /api/insurance/{id}/terminate
GET    /api/insurance/{employeeId}/history
GET    /api/insurance/{employeeId}/contribution-detail
```

---

## PHẦN IV: WORKFLOW HANDLERS CẦN TẠO (MediatR)

### Quy trình Leave Request:

**Create Handlers**:
```
1. SubmitLeaveRequestCommandHandler
2. ApproveLeaveRequestCommandHandler
3. RejectLeaveRequestCommandHandler
4. CancelLeaveRequestCommandHandler
5. GetLeaveBalanceQueryHandler
```

**Create Queries**:
```
1. GetLeaveRequestQuery
2. GetEmployeeLeaveRequestsQuery
3. GetLeaveBalanceQuery
4. GetLeaveHistoryQuery
```

### Quy trình Resignation:

**Create Handlers**:
```
1. CreateResignationCommandHandler
2. ApproveResignationCommandHandler
3. ProcessResignationCommandHandler
4. CalculateSettlementCommandHandler
```

### Tương tự cho Transfer, Contract, Insurance

---

## PHẦN V: VALIDATORS CẦN TẠOE

```csharp
// Thêm validators:
CreateLeaveRequestValidator
UpdateLeaveRequestValidator
CreateInsuranceParticipationValidator
UpdateInsuranceParticipationValidator
CreateEmploymentContractValidator
RenewEmploymentContractValidator
CreatePersonnelTransferValidator
ApprovePersonnelTransferValidator
CreateResignationDecisionValidator
```

---

## PHẦN VI: ERROR HANDLING & EDGE CASES

### Add Custom Exception Classes:
```csharp
public class LeaveBalanceExceededException : BusinessException { }
public class LeaveOverlapException : BusinessException { }
public class ContractOverlapException : BusinessException { }
public class InsuranceValidationException : BusinessException { }
public class PayrollProcessingException : BusinessException { }
public class ConcurrencyException : BusinessException { }
```

### Add Concurrency Token:
```csharp
// Add to affected entities:
[Timestamp]
public byte[] RowVersion { get; set; }
```

---

## PHẦN VII: UNIT TEST PRIORITY

### Existing Tests (58 tests) - ✅ Hoàn tất

### NEW Priority 1 Tests (87 new tests) - CẦN VIẾT NGAY

1. **ILeaveManagementService** (20 tests)
   - Submission, approval, rejection, balance calculation
   - Overlap detection, blackout dates, notice periods

2. **IInsuranceManagementService** (25 tests)
   - Registration, termination, contribution calculation
   - Tiered rates, dependent benefits, contribution tracking

3. **IEmploymentContractService** (15 tests)
   - Creation, renewal, termination
   - Overlap detection, expiry warnings

4. **IPersonnelTransferService** (12 tests)
   - Transfer creation, approval, execution
   - Department/position updates

5. **IResignationManagementService** (15 tests)
   - Resignation submission, approval, settlement
   - Final payment calculation

### Priority 2 Tests (75 tests) - Sau

### Priority 3 Tests (127 tests) - Sau

---

## 🎯 RECOMMENDED NEXT STEPS

1. **Tuần 1-2**: Tạo interfaces & unit tests cho Phase 1 services
2. **Tuần 2-3**: Implement service logic
3. **Tuần 3-4**: Tạo MediatR handlers & validators
4. **Tuần 4-5**: Tạo API controllers & endpoints
5. **Tuần 5-6**: Integration testing & bug fixes

**Total Effort**: 4-6 tuần để hoàn thành Phase 1
