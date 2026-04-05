# 🚀 PHASE 1 IMPLEMENTATION GUIDE - CHI TIẾT TỪNG BƯỚC

## 📌 OVERVIEW

Hướng dẫn chi tiết cách triển khai **Phase 1 (MVP)** trong 4-6 tuần.

---

## 🎯 MỤC TIÊU PHASE 1

### Bắt buộc hoàn thành:
- ✅ 5 critical services
- ✅ 82 unit tests
- ✅ 8 workflow handlers
- ✅ Concurrency handling
- ✅ MVP deployable

---

## 📋 TASK BREAKDOWN

### WEEK 1-2: FOUNDATION SERVICES

#### Task 1.1: ILeaveManagementService
**Người phụ trách**: Developer 1  
**Thời gian**: 5-6 ngày

**Step 1: Create Interface** (2 giờ)
```
File: ERP.HRM.Application/Interfaces/ILeaveManagementService.cs
- 10+ methods (submit, approve, reject, balance, etc.)
- Reference: DETAILED_ACTION_ITEMS.md "ILeaveManagementService"
```

**Step 2: Create DTOs** (2 giờ)
```
Files:
- CreateLeaveRequestDto
- LeaveRequestDto
- LeaveBalanceDto
- UpdateLeaveRequestDto
- Reference models in Domain/Entities/LeaveRequest.cs
```

**Step 3: Write Unit Tests** (8 giờ) ⭐ START HERE!
```
File: tests/ERP.HRM.Application.Tests/Services/LeaveManagementServiceTests.cs
20 test methods:
1. SubmitLeaveRequest_ValidRequest_ReturnsSuccess
2. SubmitLeaveRequest_ExceedingBalance_ReturnsFails
3. SubmitLeaveRequest_OverlapDates_ReturnsFails
4. ... (17 more)

Template: See TEST_TEMPLATES.md section "LeaveManagementService"
```

**Step 4: Implement Service** (8 giờ)
```
File: ERP.HRM.Application/Services/LeaveManagementService.cs
- Implement all interface methods
- Pass all 20 tests
- Handle all edge cases
```

**Step 5: Register in DI** (0.5 giờ)
```
File: ERP.HRM.API/Program.cs
- Add: builder.Services.AddScoped<ILeaveManagementService, LeaveManagementService>();
```

**Step 6: Create Handlers** (4 giờ)
```
Files: 
- Features/Leaves/Commands/SubmitLeaveRequestCommand.cs
- Features/Leaves/Handlers/SubmitLeaveRequestCommandHandler.cs
- Features/Leaves/Queries/GetLeaveBalanceQuery.cs
- Features/Leaves/Handlers/GetLeaveBalanceQueryHandler.cs
```

**Total Effort**: 6 giờ + 4 giờ = **~24-28 giờ** (3-4 ngày)

---

#### Task 1.2: IInsuranceManagementService
**Người phụ trách**: Developer 2  
**Thời gian**: 5-6 ngày

**Tương tự như Task 1.1:**
- Create interface (2 giờ)
- Create DTOs (2 giờ)
- Write 25 tests (10 giờ)
- Implement service (10 giờ)
- Register in DI (0.5 giờ)
- Create handlers (5 giờ)

**Total Effort**: **~28-30 giờ** (3-4 ngày)

**Key Business Rules**:
```
- Contribution rates: BHXH 8%, BHYT 1.5%, BHTN 0.5%
- Salary range: 300k - 20 triệu
- Tiered insurance based on salary bands
- Must validate dates and existing records
```

---

#### Task 1.3: Validators for Leave & Insurance
**Người phụ trách**: Developer 1 (sau Task 1.1)  
**Thời gian**: 1 ngày

**Create**:
```
Files:
- CreateLeaveRequestValidator
- UpdateLeaveRequestValidator
- CreateInsuranceParticipationValidator
- UpdateInsuranceParticipationValidator
```

**Total Effort**: **~8 giờ**

---

### WEEK 3-4: WORKFLOWS & ADDITIONAL SERVICES

#### Task 2.1: Leave Request Workflow Handlers
**Người phụ trách**: Developer 1  
**Thời gian**: 2-3 ngày

**Create MediatR Handlers**:
```
1. ApproveLeaveRequestCommandHandler
2. RejectLeaveRequestCommandHandler
3. CancelLeaveRequestCommandHandler
4. UpdateLeaveBalanceHandler (automatic)
```

**Key Logic**:
- Approve: Update status → Deduct balance → Send notification
- Reject: Update status → Keep balance → Send notification
- Cancel: Verify not already processed → Restore balance

**Total Effort**: **~12-16 giờ**

---

#### Task 2.2: Personnel Transfer Workflow
**Người phụ trách**: Developer 2  
**Thời gian**: 2-3 ngày

**Create**:
```
Files:
- IPersonnelTransferService
- PersonnelTransferServiceTests (12 tests)
- PersonnelTransferService
- Transfer workflow handlers (Approve, Execute, Cancel)
```

**Key Steps in Workflow**:
1. Create transfer (pending)
2. Manager approves
3. New manager approves
4. Execute transfer:
   - Update employee department
   - Update employee position
   - Update salary grade if different
   - Create salary adjustment if needed
   - Archive old transfer
   - Send notifications

**Total Effort**: **~24-28 giờ** (3 ngày)

---

#### Task 2.3: Employment Contract Service
**Người phụ trách**: Developer 1 (after Task 2.1)  
**Thời gian**: 2 ngày

**Create**:
```
Files:
- IEmploymentContractService
- EmploymentContractServiceTests (15 tests)
- EmploymentContractService
- Contract renewal & termination handlers
```

**Total Effort**: **~16-20 giờ**

---

#### Task 2.4: Resignation Management Service
**Người phụ trách**: Developer 2  
**Thời gian**: 2-3 ngày

**Create**:
```
Files:
- IResignationManagementService
- ResignationManagementServiceTests (15 tests)
- ResignationManagementService
- Settlement calculation logic
- Resignation workflow handlers
```

**Complex Logic**:
- Calculate final paycheck
- Calculate unused leave payout
- Calculate bonuses
- Apply deductions (advance salary, damages)
- Update employee status (Resigned)
- Archive employee

**Total Effort**: **~20-24 giờ** (3 ngày)

---

### WEEK 5-6: POLISH & INTEGRATION

#### Task 3.1: Concurrency Handling
**Người phụ trách**: Tech Lead / Developer 1  
**Thời gian**: 1 ngày

**Add RowVersion to Entities**:
```csharp
// Add to affected entities:
public class LeaveRequest
{
    // ... existing properties
    
    [Timestamp]
    public byte[] RowVersion { get; set; }
}
```

**Add Concurrency Exception Handling**:
```
- Create ConcurrencyException
- Add try-catch in handlers
- Implement retry logic
- Update service methods
```

**Total Effort**: **~6-8 giờ**

---

#### Task 3.2: Edge Cases & Error Handling
**Người phụ trách**: Developer 2  
**Thời gian**: 1-2 ngày

**Handle**:
- Multiple overlapping requests
- Simultaneous approvals
- Status transition violations
- Invalid state combinations
- Negative balance scenarios

**Total Effort**: **~12-16 giờ**

---

#### Task 3.3: Integration Testing & QA
**Người phụ trách**: QA Engineer  
**Thời gian**: 2 days

**Create**:
- Integration test suite (50+ tests)
- End-to-end workflow tests
- Database transaction tests
- API endpoint tests

**Total Effort**: **~20-24 giờ**

---

## 📊 TIMELINE VISUALIZATION

```
WEEK 1:
  Mon-Tue:  Task 1.1 Interface & DTOs + Task 1.2 Interface & DTOs
  Wed-Thu:  Task 1.1 Tests (write first!) + Task 1.2 Tests
  Fri:      Task 1.1 Implementation start

WEEK 2:
  Mon-Tue:  Task 1.1 Complete + Task 1.2 Implementation
  Wed-Thu:  Task 1.2 Complete + Task 1.3 Validators
  Fri:      Code review & fixes

WEEK 3:
  Mon-Tue:  Task 2.1 Handlers + Task 2.2 Start
  Wed-Thu:  Task 2.2 Continue + Task 2.3 Start
  Fri:      Code review

WEEK 4:
  Mon-Tue:  Task 2.3 Complete + Task 2.4 Start
  Wed-Thu:  Task 2.4 Continue
  Fri:      Code review & fixes

WEEK 5:
  Mon-Tue:  Task 3.1 Concurrency + Task 3.2 Edge cases
  Wed-Thu:  Task 3.3 Integration testing
  Fri:      Final QA & bug fixes

WEEK 6:
  Mon-Tue:  Performance testing & optimization
  Wed-Thu:  Documentation & deployment prep
  Fri:      MVP Release! 🎉
```

---

## 💡 BEST PRACTICES

### 1. TDD (Test-Driven Development)
```
For each feature:
1. Write failing test
2. Implement minimum code to pass
3. Refactor for quality
4. Commit
```

### 2. Git Workflow
```
1. Create feature branch: git checkout -b feature/leave-management
2. Commit frequently: git commit -m "feat: implement leave submission"
3. Push daily: git push origin feature/leave-management
4. Create PR for code review
5. Merge after approval
```

### 3. Code Review Checklist
```
- [ ] Tests written & passing
- [ ] Code follows patterns
- [ ] No hardcoded values
- [ ] Error handling complete
- [ ] Documentation updated
- [ ] Performance acceptable
```

### 4. Daily Standup Template
```
What did I do yesterday?
- Completed Task X
- Wrote Y tests

What am I doing today?
- Working on Task Z
- Code review

Any blockers?
- (List if any)
```

---

## 🧪 TESTING STRATEGY

### Unit Test Targets
```
Phase 1 Total: 82 new tests
- LeaveManagementService: 20
- InsuranceManagementService: 25
- EmploymentContractService: 15
- PersonnelTransferService: 12
- ResignationManagementService: 15
```

### Coverage Goals
```
Week 1-2: 40% coverage (58 existing + 82 new)
Week 3-4: 60% coverage (add integration tests)
Week 5-6: 80% coverage (+ edge cases)
```

### Test Running
```
Daily:
  dotnet test tests/ERP.HRM.Application.Tests/

Before commit:
  dotnet test --filter "FullyQualifiedName~FeatureYouChanged"

Before merge:
  dotnet test --no-build
```

---

## 📋 DEPLOYMENT CHECKLIST

### Pre-Release (End of Week 6)
```
- [ ] All 82 tests passing
- [ ] 80% code coverage
- [ ] No breaking changes
- [ ] Database migrations reviewed
- [ ] Performance acceptable
- [ ] Security review passed
- [ ] Documentation updated
- [ ] Deployment guide written
```

### Release to Internal
```
- [ ] Build verified
- [ ] Database backups taken
- [ ] Migrations applied
- [ ] Initial data seeded
- [ ] Smoke tests passed
- [ ] Team notified
- [ ] Version tagged
```

---

## 📞 COMMON ISSUES & SOLUTIONS

### Issue 1: "EF Core migration conflicts"
**Solution**:
```
1. Delete last migration file
2. Create new migration: dotnet ef migrations add FixedMigration
3. Verify SQL is correct
4. Update database: dotnet ef database update
```

### Issue 2: "Tests failing due to mocking"
**Solution**:
```
1. Verify Moq setup is correct
2. Check mocked method names match exactly
3. Ensure async/await handled properly
4. Debug with .Verify() calls
```

### Issue 3: "Null reference exceptions in tests"
**Solution**:
```
1. Initialize all mocked dependencies in [SetUp]
2. Use MockBehavior.Strict to catch issues
3. Add null checks in production code
4. Use nullable reference types
```

### Issue 4: "Database lock issues during testing"
**Solution**:
```
1. Ensure test database cleanup [TearDown]
2. Use InMemory database for unit tests
3. Use SQLite for integration tests
4. Don't share database between tests
```

---

## 🎯 SUCCESS CRITERIA

### By End of Week 2
- ✅ ILeaveManagementService complete & tested
- ✅ IInsuranceManagementService complete & tested
- ✅ 45 new tests passing
- ✅ Code review passed

### By End of Week 4
- ✅ All 5 services implemented
- ✅ All handlers created
- ✅ 100+ tests passing
- ✅ Workflows functioning

### By End of Week 6
- ✅ MVP deployable
- ✅ 82+ tests passing
- ✅ 80%+ coverage
- ✅ All acceptance criteria met

---

## 📚 REFERENCE DOCUMENTS

- DETAILED_ACTION_ITEMS.md - Code examples for each service
- TEST_TEMPLATES.md - Unit test templates (next file)
- TASK_BREAKDOWN.md - Detailed task list with estimates
- CODE_REVIEW_CHECKLIST.md - What to check in reviews

---

**Start with Task 1.1 on Monday! 🚀**
