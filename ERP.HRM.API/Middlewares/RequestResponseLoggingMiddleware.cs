using System.Text;

namespace ERP.HRM.API.Middlewares
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request
            await LogRequestAsync(context);

            // Store original response stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to capture the response
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);

                    // Log response
                    await LogResponseAsync(context, responseBody);
                }
                finally
                {
                    // Copy the contents of the new memory stream (which contains the response) to the original stream
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

        private async Task LogRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;
            var requestQueryString = context.Request.QueryString;

            string? requestBody = null;
            if (context.Request.ContentLength > 0)
            {
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            _logger.LogInformation(
                "HTTP Request: {Method} {Path}{QueryString} | Body: {Body}",
                requestMethod, requestPath, requestQueryString, requestBody ?? "empty");
        }

        private async Task LogResponseAsync(HttpContext context, MemoryStream responseBody)
        {
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "HTTP Response: {StatusCode} | Path: {Path} | Body: {Body}",
                context.Response.StatusCode, context.Request.Path, responseText ?? "empty");
        }
    }
}
