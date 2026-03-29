using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Exceptions;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Middlewares
{
    /// <summary>
    /// Global exception handling middleware
    /// </summary>
    public class GlobalException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalException> _logger;

        public GlobalException(RequestDelegate next, ILogger<GlobalException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred: {ExceptionMessage}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            string errorCode;
            string message = ex.Message;
            string? detail = null;

            IEnumerable<string>? errors = null;
            if (ex.Data.Contains("Errors"))
            {
                errors = ex.Data["Errors"] as IEnumerable<string>;
            }

            switch (ex)
            {
                case NotFoundException notFound:
                    status = HttpStatusCode.NotFound;
                    errorCode = "NOT_FOUND";
                    _logger.LogWarning("NotFoundException: {Message}", message);
                    detail = $"Resource not found: {notFound.ResourceName ?? "Unknown"}";
                    break;

                case ValidationException validation:
                    status = HttpStatusCode.BadRequest;
                    errorCode = "VALIDATION_ERROR";
                    _logger.LogWarning("ValidationException: {Message}", message);
                    errors = validation.Errors.SelectMany(x => x.Value);
                    break;

                case BusinessRuleException businessRule:
                    status = HttpStatusCode.BadRequest;
                    errorCode = businessRule.Code ?? "BUSINESS_RULE_VIOLATION";
                    _logger.LogWarning("BusinessRuleException ({Code}): {Message}", errorCode, message);
                    break;

                case AccessDeniedException:
                    status = HttpStatusCode.Forbidden;
                    errorCode = "ACCESS_DENIED";
                    _logger.LogWarning("AccessDeniedException: {Message}", message);
                    break;

                case ConflictException:
                    status = HttpStatusCode.Conflict;
                    errorCode = "CONFLICT";
                    _logger.LogWarning("ConflictException: {Message}", message);
                    break;

                case OperationTimeoutException:
                    status = HttpStatusCode.RequestTimeout;
                    errorCode = "OPERATION_TIMEOUT";
                    _logger.LogWarning("OperationTimeoutException: {Message}", message);
                    break;

                case ExternalServiceException externalService:
                    status = HttpStatusCode.ServiceUnavailable;
                    errorCode = "EXTERNAL_SERVICE_ERROR";
                    message = $"External service '{externalService.ServiceName}' is unavailable";
                    _logger.LogError("ExternalServiceException: {Message}", ex.Message);
                    break;

                case ArgumentException:
                    status = HttpStatusCode.BadRequest;
                    errorCode = "INVALID_ARGUMENT";
                    _logger.LogWarning("ArgumentException: {Message}", message);
                    break;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    errorCode = "UNAUTHORIZED";
                    _logger.LogWarning("UnauthorizedAccessException: {Message}", message);
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    errorCode = "INTERNAL_SERVER_ERROR";
                    message = "An internal server error occurred";
                    detail = ex.StackTrace;
                    _logger.LogError(ex, "Unexpected exception: {ExceptionType}: {Message}", ex.GetType().Name, ex.Message);
                    break;
            }

            var response = new ErrorResponse
            {
                Success = false,
                ErrorCode = errorCode,
                ErrorType = ex.GetType().Name,
                Message = message,
                Errors = errors?.ToList(),
                Detail = detail,
                Timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
