# 📋 Payroll Export REST API Documentation

## Overview
The Payroll Export API provides endpoints to export payroll data in multiple formats (Excel/PDF) for different purposes (General, Bank Transfer, Tax Authority).

## Base URL
```
https://api.example.com/api/payrollexport
```

## Authentication
All endpoints require Bearer token authentication:
```
Authorization: Bearer {token}
```

## Endpoints

### 1. POST /export
**Export payroll data in Excel or PDF format**

#### Request
```json
{
  "payrollPeriodId": 1,
  "exportFormat": "Excel",  // "Excel" or "PDF"
  "exportPurpose": "General",  // "General", "Bank", "TaxAuthority"
  "departmentId": null,  // Optional
  "includeEmployeeDetails": true,
  "includeSalaryBreakdown": true,
  "includeDeductionsBreakdown": true
}
```

#### Response
- **200 OK**: File download
  - Content-Type: `text/csv` or `application/pdf`
  - File name: `{purpose}_P{periodId}_{timestamp}.csv`
- **400 Bad Request**: Invalid parameters
- **404 Not Found**: No payroll records found
- **401 Unauthorized**: Missing or invalid token

#### Example cURL
```bash
curl -X POST "https://api.example.com/api/payrollexport/export" \
  -H "Authorization: Bearer your-token" \
  -H "Content-Type: application/json" \
  -d '{
    "payrollPeriodId": 1,
    "exportFormat": "Excel",
    "exportPurpose": "General"
  }'
```

---

### 2. POST /export-bank-transfer
**Export payroll optimized for bank transfer**

#### Request
```
POST /api/payrollexport/export-bank-transfer?payrollPeriodId=1&departmentId=null
```

#### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| payrollPeriodId | int | Yes | Payroll period to export |
| departmentId | int? | No | Filter by department |

#### Response
- **200 OK**: Bank transfer file (Excel format)
- **400 Bad Request**: Invalid period
- **404 Not Found**: No records found
- **401 Unauthorized**: Not authenticated

#### Example cURL
```bash
curl -X POST "https://api.example.com/api/payrollexport/export-bank-transfer?payrollPeriodId=1" \
  -H "Authorization: Bearer your-token"
```

---

### 3. POST /export-tax-authority
**Export payroll for tax authority (PIT - Thuế TNCN)**

#### Request
```
POST /api/payrollexport/export-tax-authority?payrollPeriodId=1&departmentId=null
```

#### Query Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| payrollPeriodId | int | Yes | Payroll period to export |
| departmentId | int? | No | Filter by department |

#### Response
- **200 OK**: Tax authority file (Excel format)
- **400 Bad Request**: Invalid parameters
- **404 Not Found**: No records found
- **401 Unauthorized**: Not authenticated

#### Example cURL
```bash
curl -X POST "https://api.example.com/api/payrollexport/export-tax-authority?payrollPeriodId=1" \
  -H "Authorization: Bearer your-token"
```

---

### 4. GET /lines/{payrollPeriodId}
**Get payroll export lines (preview data)**

#### Request
```
GET /api/payrollexport/lines/1?departmentId=null
```

#### Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| payrollPeriodId | int | Yes | Payroll period ID |
| departmentId | int? | No | Filter by department |

#### Response (200 OK)
```json
{
  "success": true,
  "message": "Retrieved 10 payroll export lines",
  "data": [
    {
      "payrollRecordId": 1,
      "employeeId": 1,
      "employeeCode": "EMP001",
      "employeeName": "Nguyễn Văn A",
      "departmentName": "IT",
      "positionName": "Developer",
      "baseSalary": 10000000,
      "allowance": 1000000,
      "overtimeCompensation": 500000,
      "grossSalary": 11500000,
      "insuranceDeduction": 1035000,
      "taxDeduction": 500000,
      "otherDeductions": 0,
      "totalDeductions": 1535000,
      "netSalary": 9965000,
      "paymentDate": "2024-01-31"
    }
  ]
}
```

#### Example cURL
```bash
curl -X GET "https://api.example.com/api/payrollexport/lines/1" \
  -H "Authorization: Bearer your-token"
```

---

### 5. GET /bank-lines/{payrollPeriodId}
**Get bank transfer export lines**

#### Request
```
GET /api/payrollexport/bank-lines/1?departmentId=null
```

#### Response (200 OK)
```json
{
  "success": true,
  "message": "Retrieved 10 bank transfer lines",
  "data": [
    {
      "bankCode": "VCB",
      "bankName": "VietcomBank",
      "employeeId": 1,
      "employeeName": "Nguyễn Văn A",
      "bankAccountNumber": "1234567890",
      "transferAmount": 9965000,
      "description": "Salary - Nguyễn Văn A - January 2024"
    }
  ]
}
```

---

### 6. GET /tax-lines/{payrollPeriodId}
**Get tax authority export lines**

#### Request
```
GET /api/payrollexport/tax-lines/1?departmentId=null
```

#### Response (200 OK)
```json
{
  "success": true,
  "message": "Retrieved 10 tax authority export lines",
  "data": [
    {
      "taxCode": "0123456789",
      "employeeId": 1,
      "employeeName": "Nguyễn Văn A",
      "employeeCode": "EMP001",
      "grossSalary": 11500000,
      "taxableIncome": 11500000,
      "taxAmount": 500000,
      "effectiveTaxRate": 4.35,
      "taxBracketLevel": "Level 1",
      "period": "2024-01"
    }
  ]
}
```

---

### 7. GET /summary/{payrollPeriodId}
**Get payroll export summary (totals and statistics)**

#### Request
```
GET /api/payrollexport/summary/1?departmentId=null
```

#### Response (200 OK)
```json
{
  "success": true,
  "message": "Payroll export summary retrieved successfully",
  "data": {
    "payrollPeriodId": 1,
    "totalEmployees": 10,
    "totalGrossSalary": 115000000,
    "totalInsuranceDeduction": 10350000,
    "totalTaxDeduction": 5000000,
    "totalOtherDeductions": 0,
    "totalNetSalary": 99650000,
    "exportDate": "2024-01-31T10:30:00"
  }
}
```

---

## Authorization Levels

### Admin / Manager
- All endpoints
- No department restrictions

### Finance / Accounting
- `/export-bank-transfer`
- `/export-tax-authority`
- All GET endpoints

### HR
- `/export`
- `/lines/{periodId}`
- `/summary/{periodId}`

---

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Export format must be 'Excel' or 'PDF'",
  "data": null
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "No payroll records found for period 5",
  "data": null
}
```

### 401 Unauthorized
```json
{
  "success": false,
  "message": "Unauthorized access",
  "data": null
}
```

---

## File Formats

### Excel (CSV)
- Format: Comma-separated values
- Headers: Employee info, Salary components, Deductions
- Summary: Totals row at bottom
- Encoding: UTF-8

### PDF
- Format: Text-based (not binary PDF)
- Content: Similar to Excel but formatted for printing
- Encoding: UTF-8

---

## Rate Limiting
- 100 requests per hour per user
- Large exports may take 10-30 seconds depending on record count

---

## Examples

### Example 1: Export General Payroll (Excel)
```bash
curl -X POST "https://api.example.com/api/payrollexport/export" \
  -H "Authorization: Bearer eyJhbGc..." \
  -H "Content-Type: application/json" \
  -d '{
    "payrollPeriodId": 1,
    "exportFormat": "Excel",
    "exportPurpose": "General",
    "includeEmployeeDetails": true,
    "includeSalaryBreakdown": true,
    "includeDeductionsBreakdown": true
  }' \
  -o payroll_export.csv
```

### Example 2: Export Bank Transfer (Department Filter)
```bash
curl -X POST "https://api.example.com/api/payrollexport/export-bank-transfer?payrollPeriodId=1&departmentId=1" \
  -H "Authorization: Bearer eyJhbGc..." \
  -o bank_transfer_P1.csv
```

### Example 3: Export Tax Authority (PDF)
```bash
curl -X POST "https://api.example.com/api/payrollexport/export-tax-authority?payrollPeriodId=1" \
  -H "Authorization: Bearer eyJhbGc..." \
  -o tax_authority_P1.pdf
```

### Example 4: Preview Payroll Lines
```bash
curl -X GET "https://api.example.com/api/payrollexport/lines/1?departmentId=1" \
  -H "Authorization: Bearer eyJhbGc..."
```

### Example 5: Get Export Summary
```bash
curl -X GET "https://api.example.com/api/payrollexport/summary/1" \
  -H "Authorization: Bearer eyJhbGc..."
```

---

## Status Codes

| Code | Meaning |
|------|---------|
| 200 | Success |
| 400 | Bad Request (invalid parameters) |
| 401 | Unauthorized (invalid/missing token) |
| 403 | Forbidden (insufficient permissions) |
| 404 | Not Found (no records) |
| 500 | Server Error |

---

## Version History

### v1.0 (Current)
- Initial release with 7 endpoints
- Support for Excel/PDF formats
- Bank transfer and tax authority exports
- Query endpoints for data preview
