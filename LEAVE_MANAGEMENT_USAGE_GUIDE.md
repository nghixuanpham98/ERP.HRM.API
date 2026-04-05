# Leave Management Service - Usage Guide

## Quick Reference

### Dependency Injection Setup

```csharp
// In Program.cs or Startup.cs
services.AddScoped<ILeaveManagementService, LeaveManagementService>();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SubmitLeaveRequestCommand).Assembly));
services.AddAutoMapper(typeof(LeaveManagementService).Assembly);
```

### Interface Reference

```csharp
public interface ILeaveManagementService
{
    // Submission and Approval Workflow
    Task<LeaveRequestDto> SubmitLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default);
    Task<LeaveRequestDto> ApproveLeaveRequestAsync(int leaveRequestId, string? approverNotes = null, CancellationToken cancellationToken = default);
    Task<LeaveRequestDto> RejectLeaveRequestAsync(int leaveRequestId, string rejectionReason, CancellationToken cancellationToken = default);
    Task<LeaveRequestDto> CancelLeaveRequestAsync(int leaveRequestId, string cancelReason, CancellationToken cancellationToken = default);

    // Retrieval Methods
    Task<LeaveRequestDto> GetLeaveRequestAsync(int leaveRequestId, CancellationToken cancellationToken = default);
    Task<PagedResult<LeaveRequestDto>> GetEmployeeLeaveRequestsAsync(int employeeId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<LeaveBalanceDto> GetLeaveBalanceAsync(int employeeId, int year, CancellationToken cancellationToken = default);
    Task<IEnumerable<LeaveBalanceHistoryDto>> GetLeaveHistoryAsync(int employeeId, int year, CancellationToken cancellationToken = default);
    Task<PagedResult<LeaveRequestDto>> GetPendingLeaveRequestsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    // Validation and Calculation
    Task<bool> ValidateLeaveRequestAsync(CreateLeaveRequestDto dto, CancellationToken cancellationToken = default);
    Task<decimal> CalculateRemainingLeaveDaysAsync(int employeeId, int year, CancellationToken cancellationToken = default);
}
```

## Usage Examples

### 1. Submit Leave Request

**Using Service Directly:**
```csharp
[HttpPost("submit")]
public async Task<IActionResult> SubmitLeaveRequest([FromBody] CreateLeaveRequestDto dto)
{
    try
    {
        var result = await _leaveService.SubmitLeaveRequestAsync(dto);
        return Ok(result);
    }
    catch (BusinessRuleException ex)
    {
        return BadRequest(ex.Message);
    }
    catch (NotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
```

**Using MediatR (Preferred):**
```csharp
[HttpPost("submit")]
public async Task<IActionResult> SubmitLeaveRequest([FromBody] CreateLeaveRequestDto dto, int employeeId)
{
    var command = new SubmitLeaveRequestCommand
    {
        EmployeeId = employeeId,
        LeaveRequestDto = dto
    };
    
    var result = await _mediator.Send(command);
    return Ok(result);
}
```

**Expected Request Body:**
```json
{
    "employeeId": 1,
    "startDate": "2024-12-20T00:00:00",
    "endDate": "2024-12-25T00:00:00",
    "leaveType": "Annual",
    "reason": "Family vacation"
}
```

**Expected Response:**
```json
{
    "leaveRequestId": 1,
    "employeeId": 1,
    "requestNumber": "LR-20241215143022",
    "leaveType": "Annual",
    "startDate": "2024-12-20",
    "endDate": "2024-12-25",
    "numberOfDays": 6,
    "reason": "Family vacation",
    "approvalStatus": "Pending",
    "requestDate": "2024-12-15T14:30:22Z"
}
```

### 2. Approve Leave Request

**Using Service:**
```csharp
[HttpPost("{leaveRequestId}/approve")]
public async Task<IActionResult> ApproveLeaveRequest(int leaveRequestId, [FromBody] ApprovalDto approval)
{
    var result = await _leaveService.ApproveLeaveRequestAsync(
        leaveRequestId, 
        approval.ApproverNotes
    );
    return Ok(result);
}
```

**Using MediatR:**
```csharp
var command = new ApproveLeaveRequestCommand
{
    LeaveRequestId = leaveRequestId,
    ApproverId = currentUserId,
    ApproverNotes = "Approved"
};

var result = await _mediator.Send(command);
```

**Expected Response:**
```json
{
    "leaveRequestId": 1,
    "approvalStatus": "Approved",
    "approvedByUserId": 5,
    "approvalDate": "2024-12-15T15:00:00Z",
    "approvalRemarks": "Approved"
}
```

### 3. Reject Leave Request

```csharp
[HttpPost("{leaveRequestId}/reject")]
public async Task<IActionResult> RejectLeaveRequest(int leaveRequestId, [FromBody] RejectionDto rejection)
{
    var result = await _leaveService.RejectLeaveRequestAsync(
        leaveRequestId, 
        rejection.RejectionReason
    );
    return Ok(result);
}
```

### 4. Cancel Leave Request

```csharp
[HttpPost("{leaveRequestId}/cancel")]
public async Task<IActionResult> CancelLeaveRequest(int leaveRequestId, [FromBody] CancellationDto cancellation)
{
    var result = await _leaveService.CancelLeaveRequestAsync(
        leaveRequestId, 
        cancellation.CancelReason
    );
    return Ok(result);
}
```

### 5. Get Employee Leave Requests (Paginated)

```csharp
[HttpGet("employee/{employeeId}")]
public async Task<IActionResult> GetEmployeeLeaveRequests(int employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    var result = await _leaveService.GetEmployeeLeaveRequestsAsync(employeeId, pageNumber, pageSize);
    return Ok(result);
}
```

**Expected Response:**
```json
{
    "items": [
        {
            "leaveRequestId": 1,
            "employeeId": 1,
            "requestNumber": "LR-20241215143022",
            "leaveType": "Annual",
            "startDate": "2024-12-20",
            "endDate": "2024-12-25",
            "numberOfDays": 6,
            "reason": "Family vacation",
            "approvalStatus": "Approved",
            "requestDate": "2024-12-15T14:30:22Z"
        }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10
}
```

### 6. Get Leave Balance

```csharp
[HttpGet("balance/{employeeId}/{year}")]
public async Task<IActionResult> GetLeaveBalance(int employeeId, int year)
{
    var result = await _leaveService.GetLeaveBalanceAsync(employeeId, year);
    return Ok(result);
}
```

**Expected Response:**
```json
{
    "leaveBalanceId": 1,
    "employeeId": 1,
    "year": 2024,
    "leaveType": "Annual",
    "allocatedDays": 20,
    "usedDays": 6,
    "remainingDays": 14,
    "carriedOverDays": 0,
    "carryOverLimit": 5,
    "expiryDate": "2024-12-31T00:00:00",
    "isActive": true,
    "notes": "Annual leave"
}
```

### 7. Get Pending Leave Requests (Admin/Manager View)

```csharp
[HttpGet("pending")]
[Authorize(Roles = "Manager,HR")]
public async Task<IActionResult> GetPendingLeaveRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    var result = await _leaveService.GetPendingLeaveRequestsAsync(pageNumber, pageSize);
    return Ok(result);
}
```

### 8. Validate Leave Request

```csharp
[HttpPost("validate")]
public async Task<IActionResult> ValidateLeaveRequest([FromBody] CreateLeaveRequestDto dto)
{
    try
    {
        var isValid = await _leaveService.ValidateLeaveRequestAsync(dto);
        return Ok(new { isValid = true });
    }
    catch (BusinessRuleException ex)
    {
        return BadRequest(new { isValid = false, error = ex.Message });
    }
}
```

### 9. Calculate Remaining Leave Days

```csharp
[HttpGet("remaining/{employeeId}/{year}")]
public async Task<IActionResult> GetRemainingLeaveDays(int employeeId, int year)
{
    var remaining = await _leaveService.CalculateRemainingLeaveDaysAsync(employeeId, year);
    return Ok(new { employeeId, year, remainingDays = remaining });
}
```

## Validation and Error Handling

### Validation Rules

**Date Validation:**
- Start date must be in the future or today
- Start date must be before end date
- Cannot submit requests for past dates

**Employee Validation:**
- Employee must exist in system
- Employee ID must be greater than 0

**Balance Validation:**
- Employee must have leave balance for the year
- Requested days must not exceed available balance
- Automatic calculation of number of days between dates

### Exception Handling

```csharp
try
{
    var result = await _leaveService.SubmitLeaveRequestAsync(dto);
}
catch (NotFoundException ex)
{
    // Employee or leave balance not found
    return NotFound(new { error = ex.Message });
}
catch (BusinessRuleException ex)
{
    // Validation failed (insufficient balance, invalid dates, etc.)
    return BadRequest(new { error = ex.Message });
}
catch (Exception ex)
{
    // Unexpected error
    _logger.LogError(ex, "Error processing leave request");
    return StatusCode(500, new { error = "An unexpected error occurred" });
}
```

## Data Flow Diagram

```
REQUEST RECEIVED
    ↓
DTOs Validated (FluentValidation)
    ↓
Service.SubmitLeaveRequestAsync()
    ↓
Business Rules Validated
    ├─ Employee exists?
    ├─ Valid date range?
    ├─ Sufficient balance?
    └─ Dates in future?
    ↓
Entity Created (LeaveRequest)
    ↓
Repository.AddAsync()
    ↓
UnitOfWork.SaveChangesAsync()
    ↓
AutoMapper → LeaveRequestDto
    ↓
RESPONSE RETURNED
```

## MediatR Command/Query Flow

**Command Example (SubmitLeaveRequest):**
```
Client Request
    ↓
SubmitLeaveRequestCommand created
    ↓
MediatR Mediator.Send()
    ↓
SubmitLeaveRequestCommandHandler.Handle()
    ↓
Service Business Logic
    ↓
Repository Operations
    ↓
LeaveRequestDto returned
```

**Query Example (GetLeaveBalance):**
```
Client Request
    ↓
GetLeaveBalanceQuery created
    ↓
MediatR Mediator.Send()
    ↓
GetLeaveBalanceQueryHandler.Handle()
    ↓
Repository.GetAllByEmployeeAsync()
    ↓
Filter by year
    ↓
LeaveBalanceDto returned
```

## Common Scenarios

### Scenario 1: Submit and Approve Leave
```csharp
// Step 1: Submit
var submitCmd = new SubmitLeaveRequestCommand { EmployeeId = 1, LeaveRequestDto = dto };
var submitted = await _mediator.Send(submitCmd);

// Step 2: Approve
var approveCmd = new ApproveLeaveRequestCommand { 
    LeaveRequestId = submitted.LeaveRequestId,
    ApproverId = 5
};
var approved = await _mediator.Send(approveCmd);
```

### Scenario 2: Check Balance Before Submitting
```csharp
// Get remaining days
var remaining = await _leaveService.CalculateRemainingLeaveDaysAsync(employeeId, 2024);

// Validate request
var isValid = await _leaveService.ValidateLeaveRequestAsync(dto);

// If valid, submit
if (isValid)
{
    var result = await _leaveService.SubmitLeaveRequestAsync(dto);
}
```

### Scenario 3: Manager Reviews Pending
```csharp
// Get all pending for approval
var pending = await _leaveService.GetPendingLeaveRequestsAsync(1, 10);

// For each pending request, manager can:
// - Approve with notes
// - Reject with reason
// - View employee details
```

## Testing

### Unit Test Examples

```csharp
[Fact]
public async Task SubmitLeaveRequest_WithValidData_ReturnsLeaveRequestDto()
{
    // Arrange
    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var mockMapper = new Mock<IMapper>();
    var service = new LeaveManagementService(mockUnitOfWork.Object, mockMapper.Object, _logger);
    
    var dto = new CreateLeaveRequestDto
    {
        EmployeeId = 1,
        StartDate = DateTime.Now.AddDays(1),
        EndDate = DateTime.Now.AddDays(5),
        LeaveType = "Annual",
        Reason = "Vacation"
    };

    // Act
    var result = await service.SubmitLeaveRequestAsync(dto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Pending", result.ApprovalStatus);
}

[Fact]
public async Task SubmitLeaveRequest_WithPastDate_ThrowsBusinessRuleException()
{
    // Arrange
    var dto = new CreateLeaveRequestDto
    {
        StartDate = DateTime.Now.AddDays(-1),
        EndDate = DateTime.Now.AddDays(5),
        // ...
    };

    // Act & Assert
    await Assert.ThrowsAsync<BusinessRuleException>(() => service.SubmitLeaveRequestAsync(dto));
}
```

## Performance Considerations

- **Paging**: Always use pagination for list endpoints to prevent large result sets
- **Caching**: Consider caching leave balances (typically stable for the year)
- **Indexing**: Ensure database indexes on EmployeeId, ApprovalStatus, RequestDate
- **Lazy Loading**: Be careful with navigation properties on Leave entities

## Security Considerations

- Authorize all endpoints (except public info endpoints)
- Verify employee authorization before returning personal leave data
- Log all approvals/rejections for audit trail
- Prevent unauthorized cancellations (must be employee or admin)

---

**Ready to integrate with API Controllers and Swagger documentation!**
