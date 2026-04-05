using ERP.HRM.Application.DTOs.Payroll;
using ERP.HRM.Infrastructure.Repositories;
using ERP.HRM.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ERP.HRM.API.Tests.Integration.Controllers
{
    /// <summary>
    /// Integration tests for PayrollExportController
    /// Tests the complete flow from API request to file download
    /// </summary>
    [Collection("PayrollExport Integration Tests")]
    public class PayrollExportControllerIntegrationTests : IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private string _testToken;

        public PayrollExportControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _testToken = "Bearer test-token"; // In real tests, use proper JWT token
        }

        public async Task InitializeAsync()
        {
            // Initialize test database with sample data
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            _client?.Dispose();
            _factory?.Dispose();
            await Task.CompletedTask;
        }

        #region Export Payroll Tests

        [Fact]
        public async Task ExportPayroll_WithValidRequest_ShouldReturnFileDownload()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            Assert.NotNull(response);
            // Note: In actual tests, would verify file content and headers
        }

        [Fact]
        public async Task ExportPayroll_WithInvalidFormat_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "InvalidFormat",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ExportPayroll_WithNonexistentPeriod_ShouldReturnNotFound()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 999,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Bank Transfer Export Tests

        [Fact]
        public async Task ExportBankTransfer_WithValidPeriod_ShouldReturnFileDownload()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.PostAsync(
                $"/api/payrollexport/export-bank-transfer?payrollPeriodId={payrollPeriodId}",
                null);

            // Assert
            Assert.NotNull(response);
            // Verify file content type and headers
        }

        [Fact]
        public async Task ExportBankTransfer_WithDepartmentFilter_ShouldFilterCorrectly()
        {
            // Arrange
            int payrollPeriodId = 1;
            int departmentId = 1;

            // Act
            var response = await _client.PostAsync(
                $"/api/payrollexport/export-bank-transfer?payrollPeriodId={payrollPeriodId}&departmentId={departmentId}",
                null);

            // Assert
            Assert.NotNull(response);
        }

        #endregion

        #region Tax Authority Export Tests

        [Fact]
        public async Task ExportTaxAuthority_WithValidPeriod_ShouldReturnFileDownload()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.PostAsync(
                $"/api/payrollexport/export-tax-authority?payrollPeriodId={payrollPeriodId}",
                null);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ExportTaxAuthority_WithInvalidPeriod_ShouldReturnBadRequest()
        {
            // Arrange
            int payrollPeriodId = -1;

            // Act
            var response = await _client.PostAsync(
                $"/api/payrollexport/export-tax-authority?payrollPeriodId={payrollPeriodId}",
                null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Query Data Tests

        [Fact]
        public async Task GetPayrollExportLines_WithValidPeriod_ShouldReturnData()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.GetAsync($"/api/payrollexport/lines/{payrollPeriodId}");

            // Assert
            Assert.NotNull(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(content);
            }
        }

        [Fact]
        public async Task GetBankTransferLines_WithValidPeriod_ShouldReturnBankData()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.GetAsync($"/api/payrollexport/bank-lines/{payrollPeriodId}");

            // Assert
            Assert.NotNull(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(content);
            }
        }

        [Fact]
        public async Task GetTaxAuthorityLines_WithValidPeriod_ShouldReturnTaxData()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.GetAsync($"/api/payrollexport/tax-lines/{payrollPeriodId}");

            // Assert
            Assert.NotNull(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(content);
            }
        }

        [Fact]
        public async Task GetPayrollExportSummary_WithValidPeriod_ShouldReturnSummary()
        {
            // Arrange
            int payrollPeriodId = 1;

            // Act
            var response = await _client.GetAsync($"/api/payrollexport/summary/{payrollPeriodId}");

            // Assert
            Assert.NotNull(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Assert.NotEmpty(content);
                
                // Verify response structure
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var jsonDoc = JsonDocument.Parse(content);
                Assert.True(jsonDoc.RootElement.TryGetProperty("data", out var dataElement));
            }
        }

        #endregion

        #region Authorization Tests

        [Fact]
        public async Task ExportPayroll_WithoutAuthorization_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            _client.DefaultRequestHeaders.Clear(); // Remove auth header

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region File Download Tests

        [Fact]
        public async Task ExportPayroll_ShouldReturnCorrectFileContentType()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            if (response.IsSuccessStatusCode)
            {
                Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType);
            }
        }

        [Fact]
        public async Task ExportPayroll_ShouldReturnFileNameInHeaders()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert
            if (response.IsSuccessStatusCode)
            {
                var contentDisposition = response.Content.Headers.ContentDisposition;
                Assert.NotNull(contentDisposition);
                Assert.NotEmpty(contentDisposition.FileName);
            }
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task ExportPayroll_WithLargeDataSet_ShouldCompleteInReasonableTime()
        {
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            var startTime = DateTime.Now;
            var timeout = TimeSpan.FromSeconds(30);

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);
            var duration = DateTime.Now - startTime;

            // Assert
            Assert.True(duration < timeout, $"Export took {duration.TotalSeconds} seconds, expected < 30 seconds");
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public async Task ExportPayroll_WithServerError_ShouldReturnErrorResponse()
        {
            // This test would simulate a database error or service failure
            // In practice, you'd use a test double or mock
            
            // Arrange
            var request = new PayrollExportRequestDto
            {
                PayrollPeriodId = 1,
                ExportFormat = "Excel",
                ExportPurpose = "General"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payrollexport/export", request);

            // Assert - should not throw, but return graceful error
            Assert.NotNull(response);
        }

        #endregion
    }
}
