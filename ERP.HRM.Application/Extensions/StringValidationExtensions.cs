using System.Text.RegularExpressions;

namespace ERP.HRM.Application.Extensions
{
    /// <summary>
    /// String validation and sanitization extensions
    /// </summary>
    public static class StringValidationExtensions
    {
        /// <summary>
        /// Check if string is empty or null
        /// </summary>
        public static bool IsNullOrEmpty(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Sanitize string by removing potential XSS characters
        /// </summary>
        public static string Sanitize(this string? value)
        {
            if (value.IsNullOrEmpty())
                return string.Empty;

            // Remove HTML tags
            value = Regex.Replace(value, @"<[^>]*>", string.Empty);

            // Remove script tags
            value = Regex.Replace(value, @"javascript:", string.Empty, RegexOptions.IgnoreCase);

            // Trim whitespace
            return value.Trim();
        }

        /// <summary>
        /// Validate email format
        /// </summary>
        public static bool IsValidEmail(this string? value)
        {
            if (value.IsNullOrEmpty())
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate Vietnamese phone number
        /// </summary>
        public static bool IsValidPhoneNumber(this string? value)
        {
            if (value.IsNullOrEmpty())
                return false;

            // Vietnamese phone number format
            var pattern = @"^(0|\+84)(1|2|3|4|5|6|7|8|9)\d{8,9}$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// Validate Vietnamese national ID
        /// </summary>
        public static bool IsValidNationalId(this string? value)
        {
            if (value.IsNullOrEmpty())
                return false;

            // 9 or 12 digits
            var pattern = @"^\d{9}$|^\d{12}$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// Check if string contains SQL injection patterns
        /// </summary>
        public static bool ContainsSqlInjectionPatterns(this string? value)
        {
            if (value.IsNullOrEmpty())
                return false;

            var patterns = new[]
            {
                "(?i)(union|select|insert|update|delete|drop|create|alter|exec|execute|script)",
                @"(-{2}|/\*|\*/|xp_|sp_)"
            };

            return patterns.Any(pattern => Regex.IsMatch(value, pattern));
        }
    }

    /// <summary>
    /// Data validation extensions
    /// </summary>
    public static class DataValidationExtensions
    {
        /// <summary>
        /// Validate date range
        /// </summary>
        public static bool IsValidDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
                return false;

            return startDate < endDate;
        }

        /// <summary>
        /// Validate age
        /// </summary>
        public static bool IsValidAge(DateTime dateOfBirth, int minAge = 18, int maxAge = 65)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age >= minAge && age <= maxAge;
        }

        /// <summary>
        /// Validate salary range
        /// </summary>
        public static bool IsValidSalary(decimal salary, decimal minSalary = 0, decimal maxSalary = 1000000000)
        {
            return salary >= minSalary && salary <= maxSalary;
        }
    }
}
