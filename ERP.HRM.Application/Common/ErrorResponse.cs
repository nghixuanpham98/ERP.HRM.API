using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.HRM.Application.Common
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string ErrorType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string>? Errors { get; set; }
        public string? Detail { get; set; }
    }
}
