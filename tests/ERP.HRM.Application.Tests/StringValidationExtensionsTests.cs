using ERP.HRM.Application.Extensions;
using System;
using Xunit;

namespace ERP.HRM.Application.Tests
{
    public class StringValidationExtensionsTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        [InlineData("abc", false)]
        public void IsNullOrEmpty_Works(string value, bool expected)
        {
            Assert.Equal(expected, value.IsNullOrEmpty());
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("<b>hi</b>", "hi")]
        [InlineData("javascript:alert(1)", "alert(1)")]
        [InlineData("  test  ", "test")]
        public void Sanitize_Works(string input, string expected)
        {
            Assert.Equal(expected, input.Sanitize());
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("test@example.com", true)]
        [InlineData("invalid", false)]
        public void IsValidEmail_Works(string input, bool expected)
        {
            Assert.Equal(expected, input.IsValidEmail());
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("+841234567890", true)]
        [InlineData("0123456789", true)]
        [InlineData("123", false)]
        public void IsValidPhoneNumber_Works(string input, bool expected)
        {
            Assert.Equal(expected, input.IsValidPhoneNumber());
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("123456789", true)]
        [InlineData("123456789012", true)]
        [InlineData("1234", false)]
        public void IsValidNationalId_Works(string input, bool expected)
        {
            Assert.Equal(expected, input.IsValidNationalId());
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("select * from users", true)]
        [InlineData("hello world", false)]
        public void ContainsSqlInjectionPatterns_Works(string input, bool expected)
        {
            Assert.Equal(expected, input.ContainsSqlInjectionPatterns());
        }

        [Theory]
        [InlineData("2020-01-01", "2021-01-01", true)]
        [InlineData(null, "2021-01-01", false)]
        [InlineData("2021-01-01", "2020-01-01", false)]
        public void IsValidDateRange_Works(string start, string end, bool expected)
        {
            DateTime? s = start == null ? (DateTime?)null : DateTime.Parse(start);
            DateTime? e = end == null ? (DateTime?)null : DateTime.Parse(end);
            Assert.Equal(expected, DataValidationExtensions.IsValidDateRange(s, e));
        }

        [Theory]
        [InlineData("1990-01-01", true)]
        [InlineData("2010-01-01", false)]
        public void IsValidAge_Works(string dob, bool expected)
        {
            var date = DateTime.Parse(dob);
            Assert.Equal(expected, DataValidationExtensions.IsValidAge(date));
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(-1, false)]
        [InlineData(1000000000, true)]
        [InlineData(1000000001, false)]
        public void IsValidSalary_Works(decimal salary, bool expected)
        {
            Assert.Equal(expected, DataValidationExtensions.IsValidSalary(salary));
        }
    }
}
