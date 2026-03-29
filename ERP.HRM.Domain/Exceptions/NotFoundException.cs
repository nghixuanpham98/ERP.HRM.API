namespace ERP.HRM.Domain.Exceptions
{
    /// <summary>
    /// Thrown when a requested resource is not found
    /// </summary>
    public class NotFoundException : Exception
    {
        public string? ResourceName { get; set; }
        public object? ResourceId { get; set; }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string resourceName, object resourceId)
            : base($"{resourceName} with ID '{resourceId}' was not found.")
        {
            ResourceName = resourceName;
            ResourceId = resourceId;
        }
    }

    /// <summary>
    /// Thrown when validation rules are violated
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

        public ValidationException(string message) : base(message) { }

        public ValidationException(string propertyName, string errorMessage)
            : base(errorMessage)
        {
            Errors[propertyName] = new[] { errorMessage };
        }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("Validation failed.")
        {
            Errors = errors;
        }
    }

    /// <summary>
    /// Thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public string? Code { get; set; }

        public BusinessRuleException(string message) : base(message) { }

        public BusinessRuleException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }

    /// <summary>
    /// Thrown when an operation is not allowed
    /// </summary>
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message = "Access denied.") : base(message) { }
    }

    /// <summary>
    /// Thrown when a conflict occurs (e.g., duplicate key)
    /// </summary>
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    /// <summary>
    /// Thrown when an operation times out
    /// </summary>
    public class OperationTimeoutException : Exception
    {
        public OperationTimeoutException(string message = "Operation timed out.") : base(message) { }
    }

    /// <summary>
    /// Thrown when external service integration fails
    /// </summary>
    public class ExternalServiceException : Exception
    {
        public string? ServiceName { get; set; }

        public ExternalServiceException(string message) : base(message) { }

        public ExternalServiceException(string serviceName, string message)
            : base($"External service '{serviceName}' error: {message}")
        {
            ServiceName = serviceName;
        }
    }
}
