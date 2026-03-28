using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Exceptions;
using System.Net;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.API.Middlewares
{
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
            string errorType = ex.GetType().Name;
            string message = ex.Message;
            string? detail = ex.InnerException?.Message ?? ex.StackTrace;

            IEnumerable<string>? errors = null;
            if (ex.Data.Contains("Errors"))
            {
                errors = ex.Data["Errors"] as IEnumerable<string>;
            }

            switch (ex)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    _logger.LogWarning("NotFoundException: {Message}", message);
                    break;
                case ValidationException:
                    status = HttpStatusCode.BadRequest;
                    _logger.LogWarning("ValidationException: {Message}", message);
                    break;
                case BusinessRuleException:
                    status = HttpStatusCode.Conflict;
                    _logger.LogWarning("BusinessRuleException: {Message}", message);
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    _logger.LogError("Unexpected exception: {ExceptionType}: {Message}", errorType, ex.Message);
                    break;
            }

            var response = new ErrorResponse
            {
                Success = false,
                ErrorType = errorType,
                Message = message,
                Errors = errors,
                Detail = detail
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
