using ERP.HRM.Application.Common;
using ERP.HRM.Domain.Exceptions;
using System.Net;

namespace ERP.HRM.API.Middlewares
{
    public class GlobalException
    {
        private readonly RequestDelegate _next;

        public GlobalException(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
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
                    break;
                case ValidationException:
                    status = HttpStatusCode.BadRequest;
                    break;
                case BusinessRuleException:
                    status = HttpStatusCode.Conflict;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
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
