# 🚀 EMPLOYEE & POSITION CQRS - QUICK START

## 📦 What Was Created

### Employee Module (10 files)
```
Features/Employees/
├── Commands/
│   ├── CreateEmployeeCommand.cs
│   ├── UpdateEmployeeCommand.cs
│   └── DeleteEmployeeCommand.cs
├── Queries/
│   ├── GetEmployeeByIdQuery.cs
│   └── GetAllEmployeesQuery.cs
└── Handlers/
    ├── CreateEmployeeCommandHandler.cs
    ├── UpdateEmployeeCommandHandler.cs
    ├── DeleteEmployeeCommandHandler.cs
    ├── GetEmployeeByIdQueryHandler.cs
    └── GetAllEmployeesQueryHandler.cs
```

### Position Module (10 files)
```
Features/Positions/
├── Commands/
│   ├── CreatePositionCommand.cs
│   ├── UpdatePositionCommand.cs
│   └── DeletePositionCommand.cs
├── Queries/
│   ├── GetPositionByIdQuery.cs
│   └── GetAllPositionsQuery.cs
└── Handlers/
    ├── CreatePositionCommandHandler.cs
    ├── UpdatePositionCommandHandler.cs
    ├── DeletePositionCommandHandler.cs
    ├── GetPositionByIdQueryHandler.cs
    └── GetAllPositionsQueryHandler.cs
```

---

## ✨ Key Features

### Employee
- ✅ Full CRUD operations via CQRS
- ✅ Email & phone validation
- ✅ Age validation (18-65)
- ✅ Salary validation
- ✅ Duplicate code detection
- ✅ Pagination with search/filters
- ✅ Soft delete support

### Position
- ✅ Full CRUD operations via CQRS
- ✅ Duplicate code detection
- ✅ Level-based filtering
- ✅ Status filtering
- ✅ Pagination with search
- ✅ Soft delete support

---

## 🎯 Usage Quick Reference

### Create Employee
```csharp
var command = new CreateEmployeeCommand
{
    EmployeeCode = "EMP001",
    FullName = "John Doe",
    Email = "john@example.com",
    PhoneNumber = "0901234567",
    BaseSalary = 15000000,
    DepartmentId = 1,
    PositionId = 1
};
var result = await mediator.Send(command);
```

### Get All Employees (with filtering)
```csharp
var query = new GetAllEmployeesQuery
{
    PageNumber = 1,
    PageSize = 10,
    SearchTerm = "John",
    DepartmentId = 1,
    Status = "Active"
};
var result = await mediator.Send(query);
```

### Get Employee By ID
```csharp
var query = new GetEmployeeByIdQuery { EmployeeId = 1 };
var result = await mediator.Send(query);
```

### Update Employee
```csharp
var command = new UpdateEmployeeCommand
{
    EmployeeId = 1,
    EmployeeCode = "EMP001",
    FullName = "Jane Doe",
    BaseSalary = 16000000
};
var result = await mediator.Send(command);
```

### Delete Employee
```csharp
var command = new DeleteEmployeeCommand(1);
var success = await mediator.Send(command);
```

---

### Create Position
```csharp
var command = new CreatePositionCommand
{
    PositionName = "Senior Manager",
    PositionCode = "SM001",
    Level = 5,
    Allowance = 5000000,
    Status = "Active"
};
var result = await mediator.Send(command);
```

### Get All Positions
```csharp
var query = new GetAllPositionsQuery
{
    PageNumber = 1,
    PageSize = 10,
    SearchTerm = "Manager",
    Level = 5
};
var result = await mediator.Send(query);
```

---

## 🔍 Response Format

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {
    "id": 1,
    "code": "EMP001",
    "name": "John Doe",
    "...": "..."
  }
}
```

### Error Response
```json
{
  "success": false,
  "errorCode": "VALIDATION_ERROR",
  "message": "Email format is invalid",
  "errors": ["Email must be valid"],
  "timestamp": "2024-01-15T10:30:00Z"
}
```

---

## 📊 Pagination

### Query Parameters
- `pageNumber`: Current page (default: 1)
- `pageSize`: Items per page (1-100, default: 10)

### Response
```json
{
  "items": [],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10
}
```

---

## 🧪 Controller Example

```csharp
[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllEmployeesQuery query)
        => Ok(await _mediator.Send(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _mediator.Send(new GetEmployeeByIdQuery { EmployeeId = id }));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeCommand command)
    {
        command.EmployeeId = id;
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
        => Ok(await _mediator.Send(new DeleteEmployeeCommand(id)));
}
```

---

## ✅ Error Handling

### Exceptions Thrown
- `NotFoundException` - Resource not found (404)
- `ValidationException` - Validation failed (400)
- `ConflictException` - Duplicate/conflict (409)

### Example
```csharp
try
{
    var result = await mediator.Send(command);
}
catch (NotFoundException ex)
{
    // Handle 404
}
catch (ValidationException ex)
{
    // Handle 400
}
catch (ConflictException ex)
{
    // Handle 409
}
```

---

## 📝 Validation Rules

### Employee
| Field | Required | Rules |
|-------|----------|-------|
| EmployeeCode | Yes | Unique, max 50 chars |
| FullName | Yes | Max 100 chars |
| Email | No | Valid format |
| Phone | No | Vietnamese format |
| DateOfBirth | No | Age 18-65 |
| BaseSalary | No | Valid range |

### Position
| Field | Required | Rules |
|-------|----------|-------|
| PositionName | Yes | Max 100 chars |
| PositionCode | Yes | Unique, max 50 chars |
| Level | No | Integer value |
| Allowance | No | Decimal value |

---

## 🔗 Dependencies

All required services are already configured in `Program.cs`:
- ✅ MediatR (for CQRS)
- ✅ AutoMapper (for DTO mapping)
- ✅ IUnitOfWork (for repositories)
- ✅ Logging (via ILogger)

No additional configuration needed!

---

## 🎯 Next Steps

1. **Add to Controllers** - Implement endpoints
2. **Test with Postman/Swagger** - Test all operations
3. **Check Logs** - Verify logging works
4. **Deploy** - Roll to production

---

## 📖 Files Reference

- See `EMPLOYEE_POSITION_CQRS_COMPLETE.md` for full documentation
- All files located in `ERP.HRM.Application/Features/`
- Follow Department pattern for consistency

---

**🎉 Ready to use! No additional setup needed.**

Build Status: ✅ Successful
