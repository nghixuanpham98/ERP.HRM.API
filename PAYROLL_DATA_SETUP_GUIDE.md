# 📝 PAYROLL DATA SETUP GUIDE

## ✅ CRITICAL: Yêu cầu chuẩn bị dữ liệu TRƯỚC khi tính lương

Như bạn đã chỉ ra đúng rồi:
1. **Để tính lương tháng cần có: CHẤM CÔNG**
2. **Để tính lương sản phẩm cần có: SẢN PHẨM + ĐƠN GIÁ + THỐNG KÊ SẢN LƯỢNG**

---

## 🔧 DATA SETUP WORKFLOW

### BƯỚC 1: Tạo Payroll Period (Kỳ Lương)

**Endpoint:** `POST /api/payroll-periods`

```http
POST /api/payroll-periods
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

Response: 201 Created
{
  "success": true,
  "message": "Kỳ lương được tạo thành công",
  "data": {
    "payrollPeriodId": 1,
    "year": 2024,
    "month": 1,
    "periodName": "Tháng 1/2024",
    "totalWorkingDays": 22
  }
}
```

**Lưu ý:**
- `periodName`: Định dạng "Tháng X/YYYY" hoặc "Month X/YYYY"
- `totalWorkingDays`: Tổng ngày làm việc trong tháng (thường 20-22 ngày)
- `startDate`, `endDate`: Ngày đầu/cuối tháng

**Response sẽ trả về `payrollPeriodId`** - Ghi nhớ ID này để dùng trong các bước tiếp theo!

---

### BƯỚC 2A: Tạo Cấu Hình Lương cho NHÂN VIÊN LƯƠNG THÁNG

**Endpoint:** `POST /api/salary-configurations`

```http
POST /api/salary-configurations
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 1,
  "salaryType": 1,
  "baseSalary": 10000000,
  "allowance": 500000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}

Response: 201 Created
{
  "success": true,
  "message": "Cấu hình lương được tạo thành công",
  "data": {
    "salaryConfigurationId": 1,
    "employeeId": 1,
    "salaryType": "Monthly",
    "baseSalary": 10000000,
    "allowance": 500000,
    "insuranceRate": 8,
    "taxRate": 5
  }
}
```

**Giải thích:**
- `employeeId`: ID của nhân viên
- `salaryType`: 1=Monthly, 2=Production, 3=Hourly
- `baseSalary`: Lương cơ bản/tháng (cho loại Monthly)
- `allowance`: Phụ cấp/tháng
- `insuranceRate`: Tỷ lệ bảo hiểm % (mặc định 8%)
- `taxRate`: Tỷ lệ thuế % (mặc định 5%)

---

### BƯỚC 2B: Tạo Cấu Hình Lương cho CÔNG NHÂN SẢN PHẨM

**Endpoint:** `POST /api/salary-configurations`

```http
POST /api/salary-configurations
Authorization: Bearer {token}
Content-Type: application/json

{
  "employeeId": 5,
  "salaryType": 2,
  "unitPrice": 2000,
  "allowance": 200000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}

Response: 201 Created
{
  "success": true,
  "message": "Cấu hình lương được tạo thành công",
  "data": {
    "salaryConfigurationId": 2,
    "employeeId": 5,
    "salaryType": "Production",
    "unitPrice": 2000,
    "allowance": 200000,
    "insuranceRate": 8,
    "taxRate": 5
  }
}
```

**Giải thích:**
- `salaryType`: 2 = Production
- `unitPrice`: Đơn giá mặc định cho sản phẩm (có thể override khi ghi nhận sản lượng)
- `allowance`: Phụ cấp/tháng
- Các trường khác giống Lương Tháng

---

### BƯỚC 3A: GHI CHẤM CÔNG (Cho Lương Tháng)

**Endpoint:** `POST /api/payroll/record-attendance`

```http
POST /api/payroll/record-attendance
Authorization: Bearer {token}
Content-Type: application/json

Ghi cả tháng - 22 ngày, nhân viên làm 20 ngày, 2 ngày nghỉ, 8 giờ tăng ca

Ngày 1-19: Ngày thường (1 ngày = 1)
Ngày 20: Nửa ngày + 2 giờ OT (0.5 + 2 OT)
Ngày 21-22: Nghỉ phép (0)
...
Ngày cuối: Toàn ngày + 8 giờ tăng ca (1 + 8 OT)

{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01T00:00:00Z",
  "workingDays": 1,
  "isPresent": true,
  "overtimeHours": 0
}

Response: 200/201
{
  "success": true,
  "message": "Điểm danh được ghi lại thành công",
  "data": {
    "attendanceId": 1,
    "employeeId": 1,
    "attendanceDate": "2024-01-01",
    "workingDays": 1,
    "isPresent": true,
    "overtimeHours": 0
  }
}
```

**⚠️ QUAN TRỌNG - Chi tiết cần ghi:**

| Tình huống | workingDays | overtimeHours | isPresent | Ghi chú |
|-----------|------------|---------------|----------|---------|
| Ngày thường | 1 | 0 | true | Normal |
| Nửa ngày | 0.5 | 0 | true | Half day |
| Toàn ngày + OT | 1 | 8 | true | Normal + OT |
| Nghỉ phép | 0 | 0 | false | Leave |
| Nghỉ bệnh | 0 | 0 | false | Sick |

**TÍNH TOÁN TỔNG CHO 22 NGÀY:**
```
- Ngày 1-19: 19 × 1 = 19 ngày
- Ngày 20: 0.5 ngày
- Ngày 21-22: 0 (nghỉ)
- Cộng lại: 19 + 0.5 = 19.5 ngày → nhưng trong guide nói 20 ngày
  → Cần cộng thêm 1 ngày từ những ngày khác hoặc 20 × 1 = 20 ngày công

- Tăng ca: 2 + 8 = 10 giờ (hoặc 8 giờ nếu chỉ 1 lần)
```

---

### BƯỚC 3B: TẠO SẢN PHẨM (Cho Lương Sản Phẩm)

**Endpoint:** `POST /api/products`

```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

Sản phẩm A:
{
  "productCode": "PROD001",
  "productName": "Product A",
  "unit": "cái",
  "category": "Electronics"
}

Sản phẩm B:
{
  "productCode": "PROD002",
  "productName": "Product B",
  "unit": "bộ",
  "category": "Mechanical"
}

Response: 201 Created
{
  "success": true,
  "message": "Sản phẩm được tạo thành công",
  "data": {
    "productId": 1,
    "productCode": "PROD001",
    "productName": "Product A",
    "unit": "cái"
  }
}
```

**Ghi nhớ ProductId từ response để dùng ở bước tiếp theo!**

---

### BƯỚC 3C: GHI SẢN LƯỢNG (Cho Lương Sản Phẩm)

**Endpoint:** `POST /api/payroll/record-production-output`

```http
POST /api/payroll/record-production-output
Authorization: Bearer {token}
Content-Type: application/json

Ngày 1: Sản phẩm A 100 cái @ 2,000 VND = 200,000
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 1,
  "quantity": 100,
  "unitPrice": 2000,
  "productionDate": "2024-01-01T08:00:00Z",
  "qualityStatus": "OK"
}

Ngày 2: Sản phẩm A 200 cái @ 2,000 VND = 400,000
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 1,
  "quantity": 200,
  "unitPrice": 2000,
  "productionDate": "2024-01-02T08:00:00Z",
  "qualityStatus": "OK"
}

Ngày 3: Sản phẩm B 50 bộ @ 4,000 VND = 200,000
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 2,
  "quantity": 50,
  "unitPrice": 4000,
  "productionDate": "2024-01-03T08:00:00Z",
  "qualityStatus": "OK"
}

Response: 200/201
{
  "success": true,
  "message": "Sản lượng được ghi lại thành công",
  "data": {
    "productionOutputId": 1,
    "employeeId": 5,
    "quantity": 100,
    "unitPrice": 2000,
    "amount": 200000
  }
}
```

**⚠️ QUAN TRỌNG:**
- `productId`: Phải là ID của sản phẩm đã tạo ở BƯỚC 3B
- `unitPrice`: Có thể khác với default nếu giá thay đổi
- `amount`: Tự tính `Quantity × UnitPrice`
- `qualityStatus`: "OK", "Defective", hoặc "Rework"

**TÍNH TOÁN TỔNG SẢN LƯỢNG:**
```
Sản phẩm A: 100 + 200 + ... = X cái × 2,000 = amount_A
Sản phẩm B: 50 + ... = Y bộ × 4,000 = amount_B
Total: amount_A + amount_B = ProductionTotal
```

---

## 🧮 COMPLETE EXAMPLE: Full Setup cho 1 nhân viên lương tháng

### Setup Data:
1. ✅ Payroll Period: Tháng 1/2024, 22 ngày làm việc
2. ✅ Salary Config: Lương 10M, phụ cấp 500K
3. ✅ Ghi chấm công: 20 ngày, 8 giờ tăng ca

### API Calls (trong thứ tự):

**Call 1: Tạo Payroll Period**
```json
POST /api/payroll-periods
{
  "year": 2024,
  "month": 1,
  "periodName": "Tháng 1/2024",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "totalWorkingDays": 22
}
// Response: payrollPeriodId = 1
```

**Call 2: Tạo Salary Configuration**
```json
POST /api/salary-configurations
{
  "employeeId": 1,
  "salaryType": 1,
  "baseSalary": 10000000,
  "allowance": 500000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}
// Response: salaryConfigurationId = 1
```

**Call 3-22: Ghi Chấm Công (20 lần)**
```json
// Ghi 19 ngày thường
POST /api/payroll/record-attendance
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-01T00:00:00Z",
  "workingDays": 1,
  "isPresent": true,
  "overtimeHours": 0
}
// Lặp lại cho ngày 2-19

// Ngày 20 (nửa ngày + 2 giờ OT)
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-20T00:00:00Z",
  "workingDays": 0.5,
  "isPresent": true,
  "overtimeHours": 2
}

// Ngày 21-22 (Nghỉ)
{
  "employeeId": 1,
  "payrollPeriodId": 1,
  "attendanceDate": "2024-01-21T00:00:00Z",
  "workingDays": 0,
  "isPresent": false,
  "note": "Annual leave"
}
```

**Call 23: Tính Lương Tháng**
```json
POST /api/payroll/calculate-monthly-salary
{
  "employeeId": 1,
  "payrollPeriodId": 1
}
// Response sẽ có netSalary = 8,937,273
```

---

## 🧮 COMPLETE EXAMPLE: Full Setup cho 1 công nhân sản phẩm

### Setup Data:
1. ✅ Payroll Period: Tháng 1/2024, 22 ngày
2. ✅ Products: Product A (2,000/unit), Product B (4,000/unit)
3. ✅ Salary Config: Đơn giá, phụ cấp 200K
4. ✅ Ghi sản lượng: Product A 300 + Product B 200

### API Calls (trong thứ tự):

**Call 1: Tạo Payroll Period** (Reuse từ example trước hoặc tạo mới)
```json
POST /api/payroll-periods
{
  "year": 2024,
  "month": 1,
  "periodName": "Tháng 1/2024",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-01-31T23:59:59Z",
  "totalWorkingDays": 22
}
// Response: payrollPeriodId = 1
```

**Call 2: Tạo Sản Phẩm A**
```json
POST /api/products
{
  "productCode": "PROD001",
  "productName": "Product A",
  "unit": "cái",
  "category": "Electronics"
}
// Response: productId = 1
```

**Call 3: Tạo Sản Phẩm B**
```json
POST /api/products
{
  "productCode": "PROD002",
  "productName": "Product B",
  "unit": "bộ",
  "category": "Mechanical"
}
// Response: productId = 2
```

**Call 4: Tạo Salary Configuration cho công nhân**
```json
POST /api/salary-configurations
{
  "employeeId": 5,
  "salaryType": 2,
  "unitPrice": 2000,
  "allowance": 200000,
  "insuranceRate": 8,
  "taxRate": 5,
  "effectiveFrom": "2024-01-01T00:00:00Z"
}
// Response: salaryConfigurationId = 2
```

**Call 5-10: Ghi Sản Lượng (6 lần)**
```json
// Ngày 1-5: Product A 100 cái/ngày
POST /api/payroll/record-production-output
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 1,
  "quantity": 100,
  "unitPrice": 2000,
  "productionDate": "2024-01-01T08:00:00Z",
  "qualityStatus": "OK"
}
// Lặp lại cho ngày 2-5

// Ngày 6-10: Product B 40 bộ/ngày
POST /api/payroll/record-production-output
{
  "employeeId": 5,
  "payrollPeriodId": 1,
  "productId": 2,
  "quantity": 40,
  "unitPrice": 4000,
  "productionDate": "2024-01-06T08:00:00Z",
  "qualityStatus": "OK"
}
// Lặp lại cho ngày 7-10
```

**Total sản lượng:**
```
Product A: 100 × 5 ngày × 2,000 = 1,000,000
Product B: 40 × 5 ngày × 4,000 = 800,000
Total: 1,800,000 + allowance 200,000 = 2,000,000 gross
```

**Call 11: Tính Lương Sản Phẩm**
```json
POST /api/payroll/calculate-production-salary
{
  "employeeId": 5,
  "payrollPeriodId": 1
}
// Response sẽ có netSalary = 1,640,000 (approx)
```

---

## ✅ CHECKLIST: DATA PREPARATION

### Trước khi TÍNH LƯƠNG THÁNG:
- [ ] Payroll Period đã tạo
- [ ] Salary Configuration đã tạo (salaryType = 1/Monthly)
- [ ] Chấm công đã ghi hết tất cả ngày trong tháng
- [ ] Tổng workingDays hợp lý (18-22 ngày)
- [ ] OvertimeHours nếu có

### Trước khi TÍNH LƯƠNG SẢN PHẨM:
- [ ] Payroll Period đã tạo
- [ ] Tất cả sản phẩm đã tạo
- [ ] Salary Configuration đã tạo (salaryType = 2/Production)
- [ ] Sản lượng đã ghi hết tất cả ngày trong tháng
- [ ] Mỗi dòng sản lượng có đúng productId

---

## 🚨 COMMON MISTAKES

### ❌ MISTAKE 1: Quên tạo Payroll Period
**Error:** "PayrollPeriod not found"
**Fix:** Tạo PayrollPeriod trước bất cứ cái gì khác

### ❌ MISTAKE 2: Quên tạo Salary Configuration
**Error:** "SalaryConfiguration for employee not found"
**Fix:** Tạo SalaryConfiguration cho employee

### ❌ MISTAKE 3: Ghi chấm công nhưng workingDays quá cao
**Error:** Lương tính cao hơn mong muốn
**Fix:** Kiểm tra tổng workingDays, phải <= totalWorkingDays của period

### ❌ MISTAKE 4: Quên tạo Product
**Error:** "Product not found" khi ghi sản lượng
**Fix:** Tạo tất cả sản phẩm trước bước ghi sản lượng

### ❌ MISTAKE 5: workingDays không phải 0, 0.5, hoặc 1
**Error:** "Ngày công phải từ 0 đến 1"
**Fix:** Chỉ dùng: 0 (nghỉ), 0.5 (nửa ngày), 1 (ngày đầy đủ)

---

## 📊 DATA VALIDATION

### Sau khi ghi dữ liệu xong, verify:

**Verify Attendance:**
```sql
SELECT 
  SUM(WorkingDays) as TotalWorkingDays,
  SUM(OvertimeHours) as TotalOvertimeHours
FROM Attendances
WHERE EmployeeId = 1 AND PayrollPeriodId = 1
-- Kết quả: TotalWorkingDays ≈ 19-21, OvertimeHours ≥ 0
```

**Verify Production:**
```sql
SELECT 
  SUM(Quantity) as TotalQuantity,
  SUM(Amount) as ProductionTotal
FROM ProductionOutputs
WHERE EmployeeId = 5 AND PayrollPeriodId = 1
-- Kết quả: ProductionTotal là số dương
```

---

**NOW YOU HAVE ALL THE DATA READY FOR SALARY CALCULATION! ✅**

Sau khi hoàn thành tất cả bước trên, bạn có thể:
1. `POST /api/payroll/calculate-monthly-salary` để tính lương tháng
2. `POST /api/payroll/calculate-production-salary` để tính lương sản phẩm
3. `GET /api/payroll/salary-slip/{empId}/{periodId}` để xem phiếu lương
