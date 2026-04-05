# Phase 1.1 - Leave Management API Quick Reference

## 📋 Base URL
```
http://localhost:5000/api/leaverequests
```

## 🔐 Authorization
All endpoints require:
- Bearer token in `Authorization` header
- Format: `Authorization: Bearer {token}`

Some endpoints additionally require:
- `Admin` or `HR` role for sensitive operations

---

## 📌 Endpoints Summary

### 1. Get All Leave Requests
```
GET /api/leaverequests
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "message": "Leave requests retrieved successfully",
  "data": [
    {
      "leaveRequestId": 1,
      "employeeId": 1,
      "leaveType": "Annual",
      "startDate": "2024-06-01",
      "endDate": "2024-06-05",
      "numberOfDays": 5,
      "reason": "Vacation",
      "approvalStatus": "Approved",
      "requestDate": "2024-05-15"
    }
  ]
}
```

---

### 2. Get Specific Leave Request
```
GET /api/leaverequests/{id}
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "message": "Leave request retrieved successfully",
  "data": {
    "leaveRequestId": 1,
    "employeeId": 1,
    "leaveType": "Annual",
    "startDate": "2024-06-01",
    "endDate": "2024-06-05",
    "numberOfDays": 5,
    "reason": "Vacation",
    "approvalStatus": "Approved"
  }
}
```

---

### 3. Get Employee's Leave Requests
```
GET /api/leaverequests/employee/{employeeId}?pageNumber=1&pageSize=10
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "message": "Employee leave requests retrieved",
  "data": {
    "items": [...],
    "totalCount": 5,
    "pageNumber": 1,
    "pageSize": 10
  }
}
```

---

### 4. Get Pending Leave Requests (Paginated)
```
GET /api/leaverequests/pending?pageNumber=1&pageSize=10
Authorization: Bearer {token}
Role: Admin or HR

Response: 200 OK
{
  "success": true,
  "message": "Pending leave requests retrieved",
  "data": {
    "items": [...],
    "totalCount": 3,
    "pageNumber": 1,
    "pageSize": 10
  }
}
```

---

### 5. Get Approved Leave Requests
```
GET /api/leaverequests/approved
Authorization: Bearer {token}

Response: 200 OK
{
  "success": true,
  "message": "Approved leave requests retrieved",
  "data": [...]
}
```

---

### 6. Get Leave Balance
```
GET /api/leaverequests/balance/{employeeId}/{year}
Authorization: Bearer {token}

Example: /api/leaverequests/balance/1/2024

Response: 200 OK
{
  "success": true,
  "message": "Leave balance retrieved",
  "data": {
    "leaveBalanceId": 1,
    "employeeId": 1,
    "year": 2024,
    "leaveType": "Annual",
    "allocatedDays": 20,
    "usedDays": 5,
    "remainingDays": 15
  }
}
```

---

### 7. Get Remaining Leave Days
```
GET /api/leaverequests/remaining/{employeeId}/{year}
Authorization: Bearer {token}

Example: /api/leaverequests/remaining/1/2024

Response: 200 OK
{
  "success": true,
  "message": "Remaining leave days calculated",
  "data": 15
}
```

---

### 8. Get Leave History
```
GET /api/leaverequests/history/{employeeId}/{year}
Authorization: Bearer {token}

Example: /api/leaverequests/history/1/2024

Response: 200 OK
{
  "success": true,
  "message": "Leave history retrieved",
  "data": [
    {
      "leaveRequestId": 1,
      "leaveType": "Annual",
      "startDate": "2024-06-01",
      "endDate": "2024-06-05",
      "numberOfDays": 5,
      "approvalStatus": "Approved"
    }
  ]
}
```

---

### 9. Submit New Leave Request
```
POST /api/leaverequests
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "employeeId": 1,
  "leaveType": "Annual",
  "startDate": "2024-06-01",
  "endDate": "2024-06-05",
  "numberOfDays": 5,
  "reason": "Vacation",
  "emergencyContact": "1234567890"
}

Response: 201 Created
{
  "success": true,
  "message": "Leave request submitted successfully",
  "data": {
    "leaveRequestId": 1,
    "employeeId": 1,
    "leaveType": "Annual",
    "approvalStatus": "Pending",
    "requestDate": "2024-05-15"
  }
}
```

**Validation Rules**:
- Start date must be after today
- End date must be after start date
- Leave balance must be sufficient
- Emergency contact is required

---

### 10. Approve Leave Request
```
POST /api/leaverequests/{id}/approve
Authorization: Bearer {token}
Role: Admin or HR
Content-Type: application/json

Request Body:
{
  "approverId": 10,
  "approverNotes": "Approved. Have a good vacation!"
}

Response: 200 OK
{
  "success": true,
  "message": "Leave request approved successfully",
  "data": {
    "leaveRequestId": 1,
    "approvalStatus": "Approved",
    "approvedDate": "2024-05-15",
    "approverNotes": "Approved. Have a good vacation!"
  }
}
```

---

### 11. Reject Leave Request
```
POST /api/leaverequests/{id}/reject
Authorization: Bearer {token}
Role: Admin or HR
Content-Type: application/json

Request Body:
{
  "rejecterId": 10,
  "rejectionReason": "Insufficient coverage during period"
}

Response: 200 OK
{
  "success": true,
  "message": "Leave request rejected successfully",
  "data": {
    "leaveRequestId": 1,
    "approvalStatus": "Rejected",
    "rejectionReason": "Insufficient coverage during period"
  }
}
```

---

### 12. Cancel Leave Request
```
POST /api/leaverequests/{id}/cancel
Authorization: Bearer {token}
Content-Type: application/json

Request Body:
{
  "employeeId": 1,
  "cancelReason": "Changed plans"
}

Response: 200 OK
{
  "success": true,
  "message": "Leave request cancelled successfully",
  "data": {
    "leaveRequestId": 1,
    "approvalStatus": "Cancelled",
    "cancelReason": "Changed plans"
  }
}
```

---

## ❌ Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Insufficient leave balance. Required: 6 days, Available: 0 days",
  "data": null
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Leave request with Id 999 not found",
  "data": null
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Unauthorized",
  "data": null
}
```

### 403 Forbidden
```json
{
  "success": false,
  "message": "You don't have permission to perform this action",
  "data": null
}
```

---

## 🔍 Common Use Cases

### Scenario 1: Submit and Approve Leave
```
1. POST /api/leaverequests → Submit request (status: Pending)
2. GET /api/leaverequests/{id} → Check status
3. POST /api/leaverequests/{id}/approve → Approve (role: HR/Admin)
```

### Scenario 2: Check Leave Balance
```
1. GET /api/leaverequests/balance/{empId}/{year} → Full balance details
2. GET /api/leaverequests/remaining/{empId}/{year} → Just remaining days
3. POST /api/leaverequests → Submit within available balance
```

### Scenario 3: View Leave History
```
1. GET /api/leaverequests/employee/{empId} → All requests
2. GET /api/leaverequests/history/{empId}/{year} → Year-specific history
3. GET /api/leaverequests/approved → Approved requests only
```

---

## 🧪 Testing with Postman

### Setup Steps:
1. Import the Swagger specification from `/swagger/v1/swagger.json`
2. Add `Authorization` header with Bearer token to environment variables
3. Use the endpoints above as templates

### Sample Tokens:
Replace `{token}` with your actual JWT token from authentication endpoint

---

## 📊 Response Structure

All responses follow the standard format:
```json
{
  "success": boolean,
  "message": string,
  "data": object | array | null
}
```

- **success**: `true` if request succeeded, `false` otherwise
- **message**: User-friendly message (English)
- **data**: Response payload (null if error)

---

## 🔗 Related Services

- **Authentication**: Obtain JWT token from auth endpoint
- **Employee Management**: Verify employee exists before creating leave request
- **Payroll**: Used for salary calculations when leave affects payment

---

*Last Updated: Phase 1.1 Completion*  
*API Version: 1.0*  
*Framework: ASP.NET Core 8.0*
