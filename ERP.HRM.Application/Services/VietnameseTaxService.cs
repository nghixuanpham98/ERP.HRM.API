using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ERP.HRM.Application.Services
{
    /// <summary>
    /// Vietnamese Personal Income Tax Service (Thuế TNCN)
    /// Implements 7-tier progressive tax system according to Vietnamese tax law
    /// 7 bậc thuế lũy tiến theo pháp luật Việt Nam
    /// </summary>
    public interface IVietnameseTaxService
    {
        /// <summary>
        /// Calculate personal income tax based on taxable income
        /// Tính thuế TNCN dựa trên thu nhập tính thuế
        /// </summary>
        Task<VietnameseTaxCalculationResult> CalculateTaxAsync(decimal taxableIncome, int? employeeId = null);

        /// <summary>
        /// Get all active tax brackets for Vietnam
        /// Lấy tất cả bậc thuế đang hoạt động của Việt Nam
        /// </summary>
        Task<IEnumerable<VietnameseTaxBracketInfo>> GetActiveTaxBracketsAsync();

        /// <summary>
        /// Get applicable tax bracket for income amount
        /// Lấy bậc thuế phù hợp cho mức thu nhập
        /// </summary>
        Task<VietnameseTaxBracketInfo?> GetBracketForIncomeAsync(decimal taxableIncome);

        /// <summary>
        /// Calculate cumulative tax across all brackets
        /// Tính thuế lũy tiến qua tất cả các bậc
        /// </summary>
        decimal CalculateCumulativeTax(decimal taxableIncome);

        /// <summary>
        /// Get the effective tax rate for income
        /// Lấy tỷ lệ thuế thực tế cho mức thu nhập
        /// </summary>
        decimal GetEffectiveTaxRate(decimal taxableIncome);
    }

    /// <summary>
    /// Information about a Vietnamese tax bracket
    /// </summary>
    public class VietnameseTaxBracketInfo
    {
        public int Level { get; set; }  // 1-7
        public decimal MinIncome { get; set; }
        public decimal MaxIncome { get; set; }
        public decimal TaxRate { get; set; }  // 5, 10, 15, 20, 25, 30, 35
        public string Description { get; set; } = null!;
    }

    /// <summary>
    /// Result of Vietnamese tax calculation
    /// </summary>
    public class VietnameseTaxCalculationResult
    {
        public decimal TaxableIncome { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal EffectiveTaxRate { get; set; }
        public decimal NetIncome { get; set; }
        public int ApplicableBracketLevel { get; set; }
        public string TaxBreakdown { get; set; } = null!;
        public DateTime CalculatedAt { get; set; }
    }

    public class VietnameseTaxService : IVietnameseTaxService
    {
        private readonly ILogger<VietnameseTaxService> _logger;

        /// <summary>
        /// Vietnamese tax brackets - 7 levels
        /// Bậc thuế Việt Nam - 7 bậc
        /// 
        /// Level 1: 0 - 5 triệu: 5%
        /// Level 2: 5 - 10 triệu: 10%
        /// Level 3: 10 - 18 triệu: 15%
        /// Level 4: 18 - 32 triệu: 20%
        /// Level 5: 32 - 52 triệu: 25%
        /// Level 6: 52 - 80 triệu: 30%
        /// Level 7: > 80 triệu: 35%
        /// </summary>
        private readonly List<VietnameseTaxBracketInfo> _taxBrackets = new()
        {
            new VietnameseTaxBracketInfo
            {
                Level = 1,
                MinIncome = 0,
                MaxIncome = 5_000_000,  // 5 triệu
                TaxRate = 5,
                Description = "0 - 5 triệu VNĐ: 5%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 2,
                MinIncome = 5_000_000,
                MaxIncome = 10_000_000,  // 10 triệu
                TaxRate = 10,
                Description = "5 - 10 triệu VNĐ: 10%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 3,
                MinIncome = 10_000_000,
                MaxIncome = 18_000_000,  // 18 triệu
                TaxRate = 15,
                Description = "10 - 18 triệu VNĐ: 15%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 4,
                MinIncome = 18_000_000,
                MaxIncome = 32_000_000,  // 32 triệu
                TaxRate = 20,
                Description = "18 - 32 triệu VNĐ: 20%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 5,
                MinIncome = 32_000_000,
                MaxIncome = 52_000_000,  // 52 triệu
                TaxRate = 25,
                Description = "32 - 52 triệu VNĐ: 25%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 6,
                MinIncome = 52_000_000,
                MaxIncome = 80_000_000,  // 80 triệu
                TaxRate = 30,
                Description = "52 - 80 triệu VNĐ: 30%"
            },
            new VietnameseTaxBracketInfo
            {
                Level = 7,
                MinIncome = 80_000_000,
                MaxIncome = decimal.MaxValue,  // > 80 triệu
                TaxRate = 35,
                Description = "> 80 triệu VNĐ: 35%"
            }
        };

        public VietnameseTaxService(ILogger<VietnameseTaxService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<VietnameseTaxCalculationResult> CalculateTaxAsync(decimal taxableIncome, int? employeeId = null)
        {
            _logger.LogInformation(
                "Calculating Vietnamese PIT for {EmployeeId}: Taxable Income: {TaxableIncome:C0}",
                employeeId ?? 0, taxableIncome);

            if (taxableIncome <= 0)
            {
                return new VietnameseTaxCalculationResult
                {
                    TaxableIncome = 0,
                    TaxAmount = 0,
                    EffectiveTaxRate = 0,
                    NetIncome = 0,
                    ApplicableBracketLevel = 0,
                    TaxBreakdown = "No tax (income <= 0)",
                    CalculatedAt = DateTime.UtcNow
                };
            }

            decimal totalTax = 0;
            var breakdown = new StringBuilder();
            int bracketLevel = 0;

            // Calculate cumulative tax across all brackets
            foreach (var bracket in _taxBrackets)
            {
                if (taxableIncome <= bracket.MinIncome)
                    break;

                bracketLevel = bracket.Level;

                decimal incomeInBracket;
                if (taxableIncome > bracket.MaxIncome)
                {
                    incomeInBracket = bracket.MaxIncome - bracket.MinIncome;
                }
                else
                {
                    incomeInBracket = taxableIncome - bracket.MinIncome;
                }

                if (incomeInBracket > 0)
                {
                    decimal taxInBracket = incomeInBracket * bracket.TaxRate / 100;
                    totalTax += taxInBracket;

                    breakdown.AppendLine(
                        $"Level {bracket.Level}: {incomeInBracket:C0} × {bracket.TaxRate}% = {taxInBracket:C0}");
                }
            }

            decimal effectiveTaxRate = taxableIncome > 0 ? (totalTax / taxableIncome) * 100 : 0;

            var result = new VietnameseTaxCalculationResult
            {
                TaxableIncome = taxableIncome,
                TaxAmount = totalTax,
                EffectiveTaxRate = effectiveTaxRate,
                NetIncome = taxableIncome - totalTax,
                ApplicableBracketLevel = bracketLevel,
                TaxBreakdown = breakdown.ToString(),
                CalculatedAt = DateTime.UtcNow
            };

            _logger.LogInformation(
                "PIT Calculation Result - Income: {Income:C0}, Tax: {Tax:C0}, Effective Rate: {Rate:P2}, Net: {Net:C0}",
                result.TaxableIncome, result.TaxAmount, result.EffectiveTaxRate / 100, result.NetIncome);

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<VietnameseTaxBracketInfo>> GetActiveTaxBracketsAsync()
        {
            _logger.LogInformation("Retrieving all Vietnamese tax brackets");
            return await Task.FromResult(_taxBrackets);
        }

        public async Task<VietnameseTaxBracketInfo?> GetBracketForIncomeAsync(decimal taxableIncome)
        {
            var bracket = _taxBrackets.LastOrDefault(b => taxableIncome > b.MinIncome);
            _logger.LogInformation("Tax bracket for income {Income:C0}: Level {Level} ({Rate}%)",
                taxableIncome, bracket?.Level ?? 0, bracket?.TaxRate ?? 0);

            return await Task.FromResult(bracket);
        }

        public decimal CalculateCumulativeTax(decimal taxableIncome)
        {
            if (taxableIncome <= 0)
                return 0;

            decimal totalTax = 0;

            foreach (var bracket in _taxBrackets)
            {
                if (taxableIncome <= bracket.MinIncome)
                    break;

                decimal incomeInBracket = taxableIncome > bracket.MaxIncome
                    ? bracket.MaxIncome - bracket.MinIncome
                    : taxableIncome - bracket.MinIncome;

                if (incomeInBracket > 0)
                {
                    totalTax += incomeInBracket * bracket.TaxRate / 100;
                }
            }

            return totalTax;
        }

        public decimal GetEffectiveTaxRate(decimal taxableIncome)
        {
            if (taxableIncome <= 0)
                return 0;

            decimal tax = CalculateCumulativeTax(taxableIncome);
            return (tax / taxableIncome) * 100;
        }
    }
}
