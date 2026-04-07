# 📡 API Reference & Integration Guide

## 🎯 Base URL

```
Development:  https://localhost:5001
Production:   https://api.example.com
```

## 🔐 Authentication

Tất cả endpoints (except `/api/auth/register` và `/api/auth/login`) yêu cầu JWT token.

### Lấy Token

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@example.com",
  "password": "Admin123!@#"
}

HTTP/1.1 200 OK
{
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600
  }
}
```

### Sử Dụng Token

```http
GET /api/employees
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Refresh Token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

HTTP/1.1 200 OK
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600
  }
}
```

---

## 👤 Authentication Endpoints

### Register User

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "SecurePass123!@#",
  "fullName": "John Doe"
}

Response:
{
  "message": "User registered successfully",
  "data": {
    "userId": "550e8400-e29b-41d4-a716-446655440000",
    "email": "john.doe@example.com",
    "fullName": "John Doe"
  }
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "SecurePass123!@#"
}

Response:
{
  "message": "Login successful",
  "data": {
    "token": "eyJhbGc...",
    "refreshToken": "eyJhbGc...",
    "expiresIn": 3600
  }
}
```

### Assign Role

```http
POST /api/auth/assign-role
Content-Type: application/json
Authorization: Bearer <admin-token>

{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "roleName": "HR"
}

Response:
{
  "message": "Role assigned successfully"
}
```

---

## 👥 Employee Endpoints

### Get All Employees

```http
GET /api/employees?pageNumber=1&pageSize=10&search=Nguyen
Authorization: Bearer <token>

Response:
{
  "message": "Employees retrieved successfully",
  "data": {
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "fullName": "Nguyễn Văn A",
        "email": "nguyen.van.a@example.com",
        "phoneNumber": "0912345678",
        "dateOfBirth": "1990-05-15",
        "employeeCode": "EMP001",
        "joiningDate": "2024-01-01",
        "departmentId": "550e8400-e29b-41d4-a716-446655440001",
        "departmentName": "IT",
        "positionId": "550e8400-e29b-41d4-a716-446655440002",
        "positionName": "Developer",
        "baseSalary": 10000000,
        "status": "Active"
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10,
      "totalCount": 150,
      "totalPages": 15
    }
  }
}
```

**Query Parameters:**
- `pageNumber` (int) - Trang hiện tại (mặc định: 1)
- `pageSize` (int) - Số records per trang (mặc định: 10)
- `search` (string) - Tìm kiếm theo tên hoặc email

### Get Employee By ID

```http
GET /api/employees/{id}
Authorization: Bearer <token>

Response:
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "fullName": "Nguyễn Văn A",
    "email": "nguyen.van.a@example.com",
    "phoneNumber": "0912345678",
    "dateOfBirth": "1990-05-15",
    "employeeCode": "EMP001",
    "joiningDate": "2024-01-01",
    "departmentId": "550e8400-e29b-41d4-a716-446655440001",
    "departmentName": "IT",
    "positionId": "550e8400-e29b-41d4-a716-446655440002",
    "positionName": "Developer",
    "baseSalary": 10000000,
    "status": "Active"
  }
}
```

### Create Employee

```http
POST /api/employees
Content-Type: application/json
Authorization: Bearer <token>

{
  "fullName": "Trần Thị B",
  "email": "tran.thi.b@example.com",
  "phoneNumber": "0987654321",
  "dateOfBirth": "1995-03-20",
  "employeeCode": "EMP002",
  "joiningDate": "2024-02-01",
  "departmentId": "550e8400-e29b-41d4-a716-446655440001",
  "positionId": "550e8400-e29b-41d4-a716-446655440003",
  "baseSalary": 12000000
}

Response:
HTTP/1.1 201 Created
{
  "message": "Employee created successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440004",
    "fullName": "Trần Thị B",
    "email": "tran.thi.b@example.com",
    ...
  }
}
```

### Update Employee

```http
PUT /api/employees/{id}
Content-Type: application/json
Authorization: Bearer <token>

{
  "fullName": "Trần Thị B Updated",
  "phoneNumber": "0987654322",
  "baseSalary": 13000000
}

Response:
{
  "message": "Employee updated successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440004",
    "fullName": "Trần Thị B Updated",
    ...
  }
}
```

### Delete Employee

```http
DELETE /api/employees/{id}
Authorization: Bearer <token>

Response:
{
  "message": "Employee deleted successfully"
}
```

---

## 🏢 Department Endpoints

### Get All Departments

```http
GET /api/departments
Authorization: Bearer <token>

Response:
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440001",
      "name": "IT",
      "description": "Information Technology",
      "createdDate": "2024-01-01",
      "employeeCount": 25
    },
    {
      "id": "550e8400-e29b-41d4-a716-446655440005",
      "name": "HR",
      "description": "Human Resources",
      "createdDate": "2024-01-01",
      "employeeCount": 10
    }
  ]
}
```

### Create Department

```http
POST /api/departments
Content-Type: application/json
Authorization: Bearer <token>

{
  "name": "Finance",
  "description": "Finance Department"
}

Response:
HTTP/1.1 201 Created
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440006",
    "name": "Finance",
    "description": "Finance Department"
  }
}
```

### Update Department

```http
PUT /api/departments/{id}
Content-Type: application/json
Authorization: Bearer <token>

{
  "name": "Finance & Accounting",
  "description": "Updated Finance Department"
}

Response:
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440006",
    "name": "Finance & Accounting",
    ...
  }
}
```

### Delete Department

```http
DELETE /api/departments/{id}
Authorization: Bearer <token>

Response:
{
  "message": "Department deleted successfully"
}
```

---

## 💰 Payroll Endpoints

### Get Payroll Periods

```http
GET /api/payroll/periods?pageNumber=1&pageSize=10
Authorization: Bearer <token>

Response:
{
  "data": {
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440007",
        "periodName": "January 2024",
        "startDate": "2024-01-01",
        "endDate": "2024-01-31",
        "status": "Closed",
        "totalRecords": 150
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10,
      "totalCount": 12,
      "totalPages": 2
    }
  }
}
```

### Get Payroll Records

```http
GET /api/payroll/records?periodId={periodId}&pageNumber=1&pageSize=10
Authorization: Bearer <token>

Response:
{
  "data": {
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440008",
        "employeeId": "550e8400-e29b-41d4-a716-446655440000",
        "employeeName": "Nguyễn Văn A",
        "payrollPeriodId": "550e8400-e29b-41d4-a716-446655440007",
        "basicSalary": 10000000,
        "allowances": 2000000,
        "totalDeductions": 1500000,
        "netSalary": 10500000,
        "status": "Processed",
        "createdDate": "2024-02-01"
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10,
      "totalCount": 150,
      "totalPages": 15
    }
  }
}
```

### Calculate Payroll

```http
POST /api/payroll/calculate
Content-Type: application/json
Authorization: Bearer <token>

{
  "payrollPeriodId": "550e8400-e29b-41d4-a716-446655440007",
  "employeeIds": [
    "550e8400-e29b-41d4-a716-446655440000",
    "550e8400-e29b-41d4-a716-446655440004"
  ]
}

Response:
{
  "message": "Payroll calculated successfully",
  "data": {
    "calculatedRecords": 2,
    "totalBaseSalary": 22000000,
    "totalAllowances": 4000000,
    "totalDeductions": 3300000,
    "totalNetSalary": 22700000
  }
}
```

### Export Payroll

```http
POST /api/payroll-export/generate-export
Content-Type: application/json
Authorization: Bearer <token>

{
  "payrollPeriodId": "550e8400-e29b-41d4-a716-446655440007",
  "exportFormat": "Excel",
  "includeDeductions": true,
  "includeAllowances": true
}

Response:
HTTP/1.1 201 Created
{
  "message": "Payroll export generated successfully",
  "data": {
    "exportId": "550e8400-e29b-41d4-a716-446655440009",
    "fileName": "Payroll_202401_20240202_120000.xlsx",
    "downloadUrl": "/api/payroll-export/download/550e8400-e29b-41d4-a716-446655440009",
    "createdDate": "2024-02-02T12:00:00",
    "exportedRecords": 150
  }
}
```

### Download Export File

```http
GET /api/payroll-export/download/{exportId}
Authorization: Bearer <token>

Response:
HTTP/1.1 200 OK
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename=Payroll_202401_20240202_120000.xlsx

[Binary Excel file content]
```

---

## 📝 Leave Request Endpoints

### Get Leave Requests

```http
GET /api/leave-requests?status=Pending&pageNumber=1&pageSize=10
Authorization: Bearer <token>

Response:
{
  "data": {
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440010",
        "employeeId": "550e8400-e29b-41d4-a716-446655440000",
        "employeeName": "Nguyễn Văn A",
        "leaveType": "Annual",
        "startDate": "2024-03-01",
        "endDate": "2024-03-05",
        "numberOfDays": 5,
        "reason": "Personal leave",
        "status": "Pending",
        "approvedBy": null,
        "approvalDate": null,
        "createdDate": "2024-02-15"
      }
    ],
    "pagination": {
      "pageNumber": 1,
      "pageSize": 10,
      "totalCount": 25,
      "totalPages": 3
    }
  }
}
```

**Query Parameters:**
- `status` - "Pending", "Approved", "Rejected", "Cancelled"
- `employeeId` - Filter by employee
- `leaveType` - "Annual", "Sick", "Unpaid", etc.

### Create Leave Request

```http
POST /api/leave-requests
Content-Type: application/json
Authorization: Bearer <token>

{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "leaveType": "Annual",
  "startDate": "2024-03-10",
  "endDate": "2024-03-12",
  "reason": "Family visit"
}

Response:
HTTP/1.1 201 Created
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440011",
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "leaveType": "Annual",
    "startDate": "2024-03-10",
    "endDate": "2024-03-12",
    "numberOfDays": 3,
    "status": "Pending"
  }
}
```

### Approve Leave Request

```http
POST /api/leave-requests/{id}/approve
Content-Type: application/json
Authorization: Bearer <token>

{
  "approvalNotes": "Approved by manager"
}

Response:
{
  "message": "Leave request approved successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440011",
    "status": "Approved",
    "approvedBy": "550e8400-e29b-41d4-a716-446655440001",
    "approvalDate": "2024-02-16"
  }
}
```

### Reject Leave Request

```http
POST /api/leave-requests/{id}/reject
Content-Type: application/json
Authorization: Bearer <token>

{
  "rejectionReason": "No coverage available"
}

Response:
{
  "message": "Leave request rejected successfully",
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440011",
    "status": "Rejected",
    "rejectionReason": "No coverage available"
  }
}
```

---

## 🏥 Insurance Endpoints

### Get Insurance Management

```http
GET /api/insurance-management
Authorization: Bearer <token>

Response:
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440012",
      "name": "Health Insurance",
      "type": "Health",
      "provider": "Insurance Provider A",
      "coverage": 500000,
      "monthlyPremium": 150000,
      "isActive": true
    },
    {
      "id": "550e8400-e29b-41d4-a716-446655440013",
      "name": "Social Insurance",
      "type": "Social",
      "provider": "Government",
      "coverage": 1000000,
      "monthlyPremium": 0,
      "isActive": true
    }
  ]
}
```

### Get Insurance Participations

```http
GET /api/insurance-participations?employeeId={employeeId}
Authorization: Bearer <token>

Response:
{
  "data": [
    {
      "id": "550e8400-e29b-41d4-a716-446655440014",
      "employeeId": "550e8400-e29b-41d4-a716-446655440000",
      "insuranceId": "550e8400-e29b-41d4-a716-446655440012",
      "insuranceName": "Health Insurance",
      "startDate": "2024-01-01",
      "endDate": "2024-12-31",
      "enrollmentStatus": "Active",
      "premiumAmount": 150000
    }
  ]
}
```

### Add Insurance Participation

```http
POST /api/insurance-participations
Content-Type: application/json
Authorization: Bearer <token>

{
  "employeeId": "550e8400-e29b-41d4-a716-446655440000",
  "insuranceId": "550e8400-e29b-41d4-a716-446655440013",
  "startDate": "2024-03-01",
  "endDate": "2024-12-31"
}

Response:
HTTP/1.1 201 Created
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440015",
    "employeeId": "550e8400-e29b-41d4-a716-446655440000",
    "insuranceId": "550e8400-e29b-41d4-a716-446655440013",
    "enrollmentStatus": "Active"
  }
}
```

---

## 🔄 Personnel Transfer Endpoints

### Get Personnel Transfers

```http
GET /api/personnel-transfers?status=Pending&pageNumber=1&pageSize=10
Authorization: Bearer <token>

Response:
{
  "data": {
    "items": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440016",
        "employeeId": "550e8400-e29b-41d4-a716-446655440000",
        "employeeName": "Nguyễn Văn A",
        "fromDepartmentId": "550e8400-e29b-41d4-a716-446655440001",
        "fromDepartmentName": "IT",
        "toDepartmentId": "550e8400-e29b-41d4-a716-446655440005",
        "toDepartmentName": "HR",
        "transferDate": "2024-03-15",
        "reason": "Career development",
        "status": "Approved",
        "approvalDate": "2024-02-20"
      }
    ]
  }
}
```

### Create Personnel Transfer

```http
POST /api/personnel-transfers
Content-Type: application/json
Authorization: Bearer <token>

{
  "employeeId": "550e8400-e29b-41d4-a716-446655440004",
  "toDepartmentId": "550e8400-e29b-41d4-a716-446655440006",
  "transferDate": "2024-04-01",
  "reason": "Promotion to Finance Manager"
}

Response:
HTTP/1.1 201 Created
{
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440017",
    "employeeId": "550e8400-e29b-41d4-a716-446655440004",
    "toDepartmentId": "550e8400-e29b-41d4-a716-446655440006",
    "status": "Pending"
  }
}
```

---

## 📊 Dashboard Endpoints

### Get Dashboard Metrics

```http
GET /api/dashboard/metrics
Authorization: Bearer <token>

Response:
{
  "data": {
    "totalEmployees": 250,
    "activeEmployees": 240,
    "onLeaveToday": 15,
    "totalDepartments": 8,
    "averageSalary": 11500000,
    "turnoverRate": 5.2,
    "attendanceRate": 94.3,
    "pendingLeaveRequests": 8,
    "pendingTransfers": 3,
    "hireThisMonth": 5,
    "resignThisMonth": 2
  }
}
```

### Get Employee Summary Report

```http
GET /api/reporting/employee-summary?departmentId={deptId}&status=Active
Authorization: Bearer <token>

Response:
{
  "data": {
    "generatedDate": "2024-02-20",
    "summary": {
      "totalEmployees": 250,
      "byDepartment": [
        {
          "departmentName": "IT",
          "count": 45,
          "averageSalary": 12000000
        },
        {
          "departmentName": "HR",
          "count": 15,
          "averageSalary": 10000000
        }
      ],
      "byStatus": [
        {
          "status": "Active",
          "count": 240
        },
        {
          "status": "On Leave",
          "count": 10
        }
      ]
    }
  }
}
```

### Get Payroll Summary Report

```http
GET /api/reporting/payroll-summary?periodId={periodId}
Authorization: Bearer <token>

Response:
{
  "data": {
    "periodName": "January 2024",
    "summary": {
      "totalEmployees": 150,
      "totalBaseSalary": 1500000000,
      "totalAllowances": 300000000,
      "totalDeductions": 225000000,
      "totalNetSalary": 1575000000,
      "averageNetSalary": 10500000,
      "minimumSalary": 5000000,
      "maximumSalary": 25000000
    }
  }
}
```

---

## ⚠️ Error Responses

### Validation Error

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "FullName": ["Full name is required"],
    "Email": ["Invalid email format"],
    "BaseSalary": ["Salary must be greater than 0"]
  }
}
```

### Unauthorized

```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Authorization header is missing or invalid"
}
```

### Not Found

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Employee with ID 550e8400-e29b-41d4-a716-446655440000 not found"
}
```

### Conflict

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "title": "Conflict",
  "status": 409,
  "detail": "Employee with email 'test@example.com' already exists"
}
```

### Server Error

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred. Please try again later."
}
```

---

## 🔗 Common Integration Scenarios

### Scenario 1: Integrate with Frontend

```typescript
// TypeScript example
async function loginUser(email: string, password: string) {
  const response = await fetch('/api/auth/login', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });

  const data = await response.json();
  const token = data.data.token;
  
  // Store token
  localStorage.setItem('authToken', token);
  return data;
}

async function getEmployees() {
  const token = localStorage.getItem('authToken');
  const response = await fetch('/api/employees', {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });

  return await response.json();
}
```

### Scenario 2: Integrate with Excel Export

The API returns Excel files that can be:
- Downloaded directly from the download endpoint
- Processed by desktop applications
- Used in data analysis workflows

```powershell
# PowerShell example
$headers = @{
    'Authorization' = 'Bearer <token>'
}

$response = Invoke-RestMethod `
    -Uri "https://localhost:5001/api/payroll-export/download/<exportId>" `
    -Headers $headers `
    -OutFile "payroll_export.xlsx"
```

### Scenario 3: Integrate with Third-Party HR Systems

```csharp
// C# example for third-party integration
var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync(
    "https://api.example.com/api/employees?pageNumber=1&pageSize=100"
);

var json = await response.Content.ReadAsStringAsync();
var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(json);
```

### Scenario 4: Batch Operations

```http
POST /api/employees/batch-import
Content-Type: application/json
Authorization: Bearer <token>

{
  "employees": [
    {
      "fullName": "Employee 1",
      "email": "emp1@example.com",
      "departmentId": "...",
      "positionId": "...",
      "baseSalary": 10000000
    },
    {
      "fullName": "Employee 2",
      "email": "emp2@example.com",
      "departmentId": "...",
      "positionId": "...",
      "baseSalary": 12000000
    }
  ]
}

Response:
{
  "message": "Batch import completed",
  "data": {
    "successful": 2,
    "failed": 0,
    "results": [...]
  }
}
```

---

## 📚 SDK Examples

### C# Client

```csharp
public class HRMApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string _token;

    public HRMApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var request = new { email, password };
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/api/auth/login", request);
        
        var content = await response.Content.ReadAsAsync<ApiResponse<LoginResponse>>();
        _token = content.Data.Token;
        return content.Data;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync(int page = 1, int size = 10)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _token);
        
        var response = await _httpClient.GetAsync(
            $"{_baseUrl}/api/employees?pageNumber={page}&pageSize={size}");
        
        var content = await response.Content.ReadAsAsync<ApiResponse<PaginatedResult<EmployeeDto>>>();
        return content.Data.Items.ToList();
    }
}
```

### JavaScript/TypeScript Client

```typescript
class HRMApiClient {
    private baseUrl: string;
    private token: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
    }

    async login(email: string, password: string): Promise<LoginResponse> {
        const response = await fetch(`${this.baseUrl}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });

        const data = await response.json();
        this.token = data.data.token;
        return data.data;
    }

    async getEmployees(page: number = 1, size: number = 10): Promise<EmployeeDto[]> {
        const response = await fetch(
            `${this.baseUrl}/api/employees?pageNumber=${page}&pageSize=${size}`,
            {
                headers: {
                    'Authorization': `Bearer ${this.token}`
                }
            }
        );

        const data = await response.json();
        return data.data.items;
    }
}
```

---

## 📞 Rate Limiting

The API implements rate limiting to prevent abuse:

```
Limit: 100 requests per minute
Per IP: 1000 requests per day
```

When rate limit exceeded:

```json
{
  "status": 429,
  "title": "Too Many Requests",
  "detail": "Rate limit exceeded. Try again after 60 seconds.",
  "retryAfter": 60
}
```

---

## 🔄 Webhooks (Future)

Coming soon - webhooks for:
- `employee.created`
- `employee.updated`
- `payroll.processed`
- `leave.approved`

---

**API Version:** 1.0  
**Last Updated:** 2024  
**Documentation Version:** 1.0
