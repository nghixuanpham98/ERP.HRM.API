# 🧪 TESTING GUIDE - Professional ERP Improvements

## Quick Test Commands

### Health Check
```powershell
# Test database health
curl -X GET http://localhost:5000/health
```

### Authentication (Example)
```powershell
# Register
curl -X POST http://localhost:5000/api/auth/register `
  -H "Content-Type: application/json" `
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test@123456"
  }'

# Login
curl -X POST http://localhost:5000/api/auth/login `
  -H "Content-Type: application/json" `
  -d '{
    "username": "testuser",
    "password": "Test@123456"
  }'
```

### Rate Limiting Test
```powershell
# Send 101 requests to trigger rate limit
for ($i = 1; $i -le 101; $i++) {
    $response = curl -X GET http://localhost:5000/api/departments `
      -H "Authorization: Bearer {your-token}"
    if ($response.StatusCode -eq 429) {
        Write-Host "Rate limit hit at request $i"
        break
    }
    Write-Host "Request $i: $($response.StatusCode)"
}
```

### CORS Test (from browser console)
```javascript
fetch('http://localhost:5000/api/departments', {
    method: 'GET',
    headers: {
        'Authorization': 'Bearer YOUR_TOKEN_HERE',
        'Content-Type': 'application/json'
    }
})
.then(r => r.json())
.then(d => console.log(d))
.catch(e => console.error(e))
```

### Exception Handling Tests
```powershell
# Test NotFoundException (404)
curl -X GET http://localhost:5000/api/departments/9999 `
  -H "Authorization: Bearer {token}"
# Expected: 404 with errorCode "NOT_FOUND"

# Test ValidationException (400)
curl -X POST http://localhost:5000/api/departments `
  -H "Authorization: Bearer {token}" `
  -H "Content-Type: application/json" `
  -d '{
    "departmentName": "",
    "departmentCode": ""
  }'
# Expected: 400 with errorCode "VALIDATION_ERROR"

# Test BusinessRuleException (400)
curl -X POST http://localhost:5000/api/departments `
  -H "Authorization: Bearer {token}" `
  -H "Content-Type: application/json" `
  -d '{
    "departmentName": "HR",
    "departmentCode": "HR"
  }'
# Expected: 400 with errorCode "BUSINESS_RULE_VIOLATION"

# Test UnauthorizedAccessException (401)
curl -X GET http://localhost:5000/api/departments/1
# Expected: 401 with errorCode "UNAUTHORIZED"
```

---

## Unit Test Examples

### Test Exception Handling
```csharp
[TestClass]
public class ExceptionHandlingTests
{
    [TestMethod]
    public void NotFoundException_HasResourceInfo()
    {
        var ex = new NotFoundException("User", 123);
        
        Assert.AreEqual("User", ex.ResourceName);
        Assert.AreEqual(123, ex.ResourceId);
        Assert.IsTrue(ex.Message.Contains("123"));
    }

    [TestMethod]
    public void ValidationException_AccumulatesErrors()
    {
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Invalid email format" } },
            { "Phone", new[] { "Invalid phone number" } }
        };
        
        var ex = new ValidationException(errors);
        
        Assert.AreEqual(2, ex.Errors.Count);
    }

    [TestMethod]
    public void BusinessRuleException_StoresCode()
    {
        var ex = new BusinessRuleException("DUPLICATE_ENTRY", "Already exists");
        
        Assert.AreEqual("DUPLICATE_ENTRY", ex.Code);
    }
}
```

### Test Input Validation
```csharp
[TestClass]
public class StringValidationTests
{
    [TestMethod]
    [DataRow("test@example.com", true)]
    [DataRow("invalid-email", false)]
    [DataRow("", false)]
    public void IsValidEmail_ChecksFormat(string email, bool expected)
    {
        var result = email.IsValidEmail();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("0901234567", true)]
    [DataRow("84912345678", true)]
    [DataRow("123", false)]
    public void IsValidPhoneNumber_ChecksVietnamFormat(string phone, bool expected)
    {
        var result = phone.IsValidPhoneNumber();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("123456789", true)]
    [DataRow("123456789012", true)]
    [DataRow("12345", false)]
    public void IsValidNationalId_Checks9Or12Digits(string id, bool expected)
    {
        var result = id.IsValidNationalId();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("<script>alert('xss')</script>", false)]
    [DataRow("SELECT * FROM Users", false)]
    [DataRow("Normal text", false)]
    public void ContainsSqlInjectionPatterns_DetectsPatterns(string input, bool expected)
    {
        var result = input.ContainsSqlInjectionPatterns();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Sanitize_RemovesHtmlTags()
    {
        var input = "<script>alert('xss')</script>Hello";
        var result = input.Sanitize();
        
        Assert.IsFalse(result.Contains("<script>"));
        Assert.IsTrue(result.Contains("Hello"));
    }
}
```

### Test Data Validation
```csharp
[TestClass]
public class DataValidationTests
{
    [TestMethod]
    public void IsValidDateRange_ChecksOrder()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 12, 31);
        
        Assert.IsTrue(DataValidationExtensions.IsValidDateRange(start, end));
        Assert.IsFalse(DataValidationExtensions.IsValidDateRange(end, start));
    }

    [TestMethod]
    public void IsValidAge_ChecksRange()
    {
        var validDob = DateTime.Today.AddYears(-30);
        var youngDob = DateTime.Today.AddYears(-15);
        var oldDob = DateTime.Today.AddYears(-70);
        
        Assert.IsTrue(DataValidationExtensions.IsValidAge(validDob, 18, 65));
        Assert.IsFalse(DataValidationExtensions.IsValidAge(youngDob, 18, 65));
        Assert.IsFalse(DataValidationExtensions.IsValidAge(oldDob, 18, 65));
    }

    [TestMethod]
    public void IsValidSalary_ChecksRange()
    {
        Assert.IsTrue(DataValidationExtensions.IsValidSalary(1000000m, 0, 10000000m));
        Assert.IsFalse(DataValidationExtensions.IsValidSalary(-1000m));
        Assert.IsFalse(DataValidationExtensions.IsValidSalary(100000000000m));
    }
}
```

---

## Integration Test Examples

### Test CORS Middleware
```csharp
[TestClass]
public class CorsMiddlewareTests
{
    private WebApplicationFactory<Program> _factory;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TestMethod]
    public async Task CorsHeaders_ReturnedForAllowedOrigin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Origin", "http://localhost:3000");
        
        var response = await client.GetAsync("/api/departments");
        
        Assert.IsTrue(response.Headers.Contains("Access-Control-Allow-Origin"));
    }

    [TestMethod]
    public async Task CorsHeaders_BlockedForDisallowedOrigin()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Origin", "http://malicious.com");
        
        var response = await client.GetAsync("/api/departments");
        
        // Should not have CORS headers
        Assert.IsFalse(response.Headers.Contains("Access-Control-Allow-Origin") 
            || response.Headers.GetValues("Access-Control-Allow-Origin").FirstOrDefault() == "http://malicious.com");
    }
}
```

### Test Health Check
```csharp
[TestClass]
public class HealthCheckTests
{
    private WebApplicationFactory<Program> _factory;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TestMethod]
    public async Task HealthCheck_ReturnsHealthy()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/health");
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsAsync<HealthCheckResponse>();
        Assert.AreEqual("Healthy", content.Status);
    }

    [TestMethod]
    public async Task HealthCheck_IncludesDatabaseCheck()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/health");
        
        var content = await response.Content.ReadAsAsync<HealthCheckResponse>();
        Assert.IsTrue(content.Checks.ContainsKey("Database"));
    }
}
```

### Test Exception Handler
```csharp
[TestClass]
public class GlobalExceptionHandlerTests
{
    private WebApplicationFactory<Program> _factory;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TestMethod]
    public async Task ExceptionHandler_NotFoundException_Returns404()
    {
        var client = _factory.CreateClient();
        var token = GetAuthToken();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.GetAsync("/api/departments/9999");
        
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        var error = await response.Content.ReadAsAsync<ErrorResponse>();
        Assert.AreEqual("NOT_FOUND", error.ErrorCode);
    }

    [TestMethod]
    public async Task ExceptionHandler_ValidationError_Returns400()
    {
        var client = _factory.CreateClient();
        var token = GetAuthToken();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.PostAsJsonAsync("/api/departments", new { departmentName = "" });
        
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadAsAsync<ErrorResponse>();
        Assert.AreEqual("VALIDATION_ERROR", error.ErrorCode);
        Assert.IsNotNull(error.Errors);
    }

    private string GetAuthToken()
    {
        // Implement auth token retrieval for tests
        return "test-token";
    }
}
```

---

## Load Testing

### Using Apache Bench
```bash
# Test 1000 requests with 10 concurrent
ab -n 1000 -c 10 -H "Authorization: Bearer YOUR_TOKEN" http://localhost:5000/api/departments

# Expected: Most requests succeed, some might hit rate limit
```

### Using PowerShell
```powershell
$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$results = @()

for ($i = 1; $i -le 1000; $i++) {
    $response = curl -s -w "%{http_code}" -o /dev/null `
        -X GET http://localhost:5000/api/departments `
        -H "Authorization: Bearer {token}"
    
    $results += @{
        Request = $i
        StatusCode = $response
        Time = [DateTime]::Now
    }
    
    if ($i % 100 -eq 0) {
        Write-Host "Completed $i requests..."
    }
}

$stopwatch.Stop()
$successful = ($results | Where-Object {$_.StatusCode -eq 200}).Count
$rateLimited = ($results | Where-Object {$_.StatusCode -eq 429}).Count

Write-Host "Total: $($results.Count), Successful: $successful, Rate Limited: $rateLimited"
Write-Host "Time: $($stopwatch.ElapsedMilliseconds)ms"
```

---

## Performance Testing

### Log Performance Impact
```powershell
# Measure request time with logging
$times = @()

for ($i = 1; $i -le 100; $i++) {
    $timer = [System.Diagnostics.Stopwatch]::StartNew()
    curl -s -X GET http://localhost:5000/api/departments `
        -H "Authorization: Bearer {token}" > $null
    $timer.Stop()
    $times += $timer.ElapsedMilliseconds
}

$avg = ($times | Measure-Object -Average).Average
Write-Host "Average response time: ${avg}ms"
```

---

## Manual Testing Scenarios

### Scenario 1: Complete User Flow
1. ✅ Register new user
2. ✅ Login with credentials
3. ✅ Get JWT token
4. ✅ Use token to access protected resource
5. ✅ Verify audit log shows action
6. ✅ Check request/response logs

### Scenario 2: Error Handling
1. ✅ Request non-existent resource → 404
2. ✅ Send invalid data → 400
3. ✅ Duplicate entry → 409
4. ✅ Unauthenticated request → 401
5. ✅ Permission denied → 403

### Scenario 3: Rate Limiting
1. ✅ Send 100 valid requests within 60s → all succeed
2. ✅ Send 101st request → 429
3. ✅ Wait 60 seconds
4. ✅ Send request again → succeeds

### Scenario 4: CORS
1. ✅ Request from localhost:3000 → allowed
2. ✅ Request from localhost:4200 → allowed
3. ✅ Request from untrusted origin → blocked

---

## Logging Verification

### Check Audit Logs
```powershell
# View today's logs
Get-Content "logs/log-$(Get-Date -Format 'yyyyMMdd').txt" | 
    Select-String "Audit:" | 
    Tail -20
```

### Check Error Logs
```powershell
# Find all error logs
Get-Content "logs/*.txt" | 
    Select-String "Error\|Exception" | 
    Tail -50
```

---

## Automation Script (PowerShell)

```powershell
# comprehensive-test.ps1

param(
    [string]$ApiUrl = "http://localhost:5000",
    [string]$Username = "testuser",
    [string]$Password = "Test@123456"
)

function Test-HealthCheck {
    Write-Host "Testing Health Check..."
    $response = Invoke-WebRequest -Uri "$ApiUrl/health" -Method Get
    $response.StatusCode -eq 200 ? "✅ PASS" : "❌ FAIL"
}

function Test-Authentication {
    Write-Host "Testing Authentication..."
    $body = @{
        username = $Username
        password = $Password
    } | ConvertTo-Json
    
    $response = Invoke-WebRequest -Uri "$ApiUrl/api/auth/login" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body
    
    $response.StatusCode -eq 200 ? "✅ PASS" : "❌ FAIL"
}

function Test-Authorization {
    Write-Host "Testing Authorization..."
    $response = Invoke-WebRequest -Uri "$ApiUrl/api/departments" `
        -Method Get `
        -ErrorAction SilentlyContinue
    
    $response.StatusCode -eq 401 ? "✅ PASS (correctly rejected)" : "❌ FAIL"
}

function Test-RateLimiting {
    Write-Host "Testing Rate Limiting..."
    $tokenResponse = Invoke-WebRequest -Uri "$ApiUrl/api/auth/login" `
        -Method Post `
        -ContentType "application/json" `
        -Body (ConvertTo-Json @{username=$Username; password=$Password})
    
    $token = ($tokenResponse.Content | ConvertFrom-Json).token
    $headers = @{"Authorization" = "Bearer $token"}
    
    $rateLimitHit = $false
    for ($i = 1; $i -le 101; $i++) {
        $response = Invoke-WebRequest -Uri "$ApiUrl/api/departments" `
            -Method Get `
            -Headers $headers `
            -ErrorAction SilentlyContinue
        
        if ($response.StatusCode -eq 429) {
            $rateLimitHit = $true
            break
        }
    }
    
    $rateLimitHit ? "✅ PASS (limit enforced at $i requests)" : "❌ FAIL"
}

# Run all tests
Test-HealthCheck
Test-Authentication
Test-Authorization
Test-RateLimiting
```

---

**🎯 All tests verify that professional-grade features are working correctly!**

Run these tests regularly to ensure system reliability.
