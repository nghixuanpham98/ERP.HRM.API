using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.HRM.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ERP.HRM.Application.Tests.Services
{
    /// <summary>
    /// Test Vietnamese Tax Service (Thuế TNCN)
    /// </summary>
    public class VietnameseTaxServiceTests
    {
        private readonly Mock<ILogger<VietnameseTaxService>> _loggerMock;
        private readonly VietnameseTaxService _service;

        public VietnameseTaxServiceTests()
        {
            _loggerMock = new Mock<ILogger<VietnameseTaxService>>();
            _service = new VietnameseTaxService(_loggerMock.Object);
        }

        #region Tax Bracket Tests

        [Fact]
        public async Task GetActiveTaxBrackets_ShouldReturn7Levels()
        {
            // Act
            var brackets = await _service.GetActiveTaxBracketsAsync();

            // Assert
            Assert.NotNull(brackets);
            Assert.Equal(7, brackets.Count());
            
            // Verify bracket details
            var bracketList = brackets.ToList();
            Assert.Equal(1, bracketList[0].Level);
            Assert.Equal(7, bracketList[6].Level);
        }

        [Fact]
        public async Task GetBracketForIncome_WithLevel1Income_ShouldReturnCorrectBracket()
        {
            // Act - Income: 3 triệu (Level 1: 0-5M @ 5%)
            var bracket = await _service.GetBracketForIncomeAsync(3_000_000);

            // Assert
            Assert.NotNull(bracket);
            Assert.Equal(1, bracket.Level);
            Assert.Equal(5, bracket.TaxRate);
        }

        [Fact]
        public async Task GetBracketForIncome_WithLevel4Income_ShouldReturnCorrectBracket()
        {
            // Act - Income: 25 triệu (Level 4: 18-32M @ 20%)
            var bracket = await _service.GetBracketForIncomeAsync(25_000_000);

            // Assert
            Assert.NotNull(bracket);
            Assert.Equal(4, bracket.Level);
            Assert.Equal(20, bracket.TaxRate);
        }

        [Fact]
        public async Task GetBracketForIncome_WithLevel7Income_ShouldReturnHighestBracket()
        {
            // Act - Income: 100 triệu (Level 7: >80M @ 35%)
            var bracket = await _service.GetBracketForIncomeAsync(100_000_000);

            // Assert
            Assert.NotNull(bracket);
            Assert.Equal(7, bracket.Level);
            Assert.Equal(35, bracket.TaxRate);
        }

        #endregion

        #region Simple Tax Calculation Tests (Single Bracket)

        [Fact]
        public async Task CalculateTax_WithLevel1Income_3Million_ShouldCalculateCorrectly()
        {
            // Act - Income: 3 triệu
            // Tax = 3,000,000 × 5% = 150,000
            var result = await _service.CalculateTaxAsync(3_000_000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3_000_000, result.TaxableIncome);
            Assert.Equal(150_000, result.TaxAmount);
            Assert.Equal(5, result.EffectiveTaxRate);  // 150,000 / 3,000,000 = 5%
            Assert.Equal(2_850_000, result.NetIncome);
            Assert.Equal(1, result.ApplicableBracketLevel);
        }

        [Fact]
        public async Task CalculateTax_WithZeroIncome_ShouldReturnZero()
        {
            // Act
            var result = await _service.CalculateTaxAsync(0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TaxAmount);
            Assert.Equal(0, result.EffectiveTaxRate);
            Assert.Equal(0, result.NetIncome);
        }

        #endregion

        #region Cumulative Tax Calculation Tests (Multiple Brackets)

        [Fact]
        public async Task CalculateTax_WithCumulativeIncome_6Million_ShouldCalculateAcrossTwoBrackets()
        {
            // Act - Income: 6 triệu (across 2 brackets)
            // Bracket 1: 5,000,000 × 5% = 250,000
            // Bracket 2: (6,000,000 - 5,000,000) × 10% = 1,000,000 × 10% = 100,000
            // Total Tax: 350,000
            var result = await _service.CalculateTaxAsync(6_000_000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6_000_000, result.TaxableIncome);
            Assert.Equal(350_000, result.TaxAmount);
            Assert.Equal(5_650_000, result.NetIncome);
            Assert.Equal(2, result.ApplicableBracketLevel);

            // Effective rate: 350,000 / 6,000,000 = 5.833%
            var expectedEffectiveRate = (350_000m / 6_000_000m) * 100;
            Assert.Equal(expectedEffectiveRate, result.EffectiveTaxRate, 2);
        }

        [Fact]
        public async Task CalculateTax_WithCumulativeIncome_20Million_ShouldCalculateAcrossThreeBrackets()
        {
            // Act - Income: 20 triệu (across 4 brackets)
            // Bracket 1: 5,000,000 × 5% = 250,000
            // Bracket 2: 5,000,000 × 10% = 500,000
            // Bracket 3: 8,000,000 × 15% = 1,200,000
            // Bracket 4: (20,000,000 - 18,000,000) × 20% = 2,000,000 × 20% = 400,000
            // Total Tax: 2,350,000
            var result = await _service.CalculateTaxAsync(20_000_000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(20_000_000, result.TaxableIncome);
            Assert.Equal(2_350_000, result.TaxAmount);
            Assert.Equal(17_650_000, result.NetIncome);
            Assert.Equal(4, result.ApplicableBracketLevel);

            // Effective rate: 2,350,000 / 20,000,000 = 11.75%
            Assert.Equal(11.75m, result.EffectiveTaxRate, 2);
        }

        [Fact]
        public async Task CalculateTax_WithHighIncome_100Million_ShouldCalculateCorrectly()
        {
            // Act - Income: 100 triệu
            // Bracket 1: 5,000,000 × 5% = 250,000
            // Bracket 2: 5,000,000 × 10% = 500,000
            // Bracket 3: 8,000,000 × 15% = 1,200,000
            // Bracket 4: 14,000,000 × 20% = 2,800,000
            // Bracket 5: 20,000,000 × 25% = 5,000,000
            // Bracket 6: 28,000,000 × 30% = 8,400,000
            // Bracket 7: (100,000,000 - 80,000,000) × 35% = 20,000,000 × 35% = 7,000,000
            // Total Tax: 24,950,000
            var result = await _service.CalculateTaxAsync(100_000_000);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100_000_000, result.TaxableIncome);
            
            decimal expectedTax = 250_000 + 500_000 + 1_200_000 + 2_800_000 + 5_000_000 + 8_400_000 + 7_000_000;
            Assert.Equal(expectedTax, result.TaxAmount);
            
            Assert.Equal(100_000_000 - expectedTax, result.NetIncome);
            Assert.Equal(7, result.ApplicableBracketLevel);

            // Effective rate: 24,950,000 / 100,000,000 = 24.95%
            var expectedRate = (expectedTax / 100_000_000m) * 100;
            Assert.Equal(expectedRate, result.EffectiveTaxRate, 2);
        }

        #endregion

        #region Effective Tax Rate Tests

        [Fact]
        public void GetEffectiveTaxRate_WithLevel1Income_ShouldReturn5Percent()
        {
            // Act
            var rate = _service.GetEffectiveTaxRate(3_000_000);

            // Assert
            Assert.Equal(5m, rate);
        }

        [Fact]
        public void GetEffectiveTaxRate_WithCumulativeIncome_ShouldReturnCorrectRate()
        {
            // Act - 6 triệu income
            // Tax: 350,000 / Effective rate: 350,000 / 6,000,000 = 5.833%
            var rate = _service.GetEffectiveTaxRate(6_000_000);

            // Assert
            var expected = (350_000m / 6_000_000m) * 100;
            Assert.Equal(expected, rate, 2);
        }

        [Fact]
        public void GetEffectiveTaxRate_WithZeroIncome_ShouldReturnZero()
        {
            // Act
            var rate = _service.GetEffectiveTaxRate(0);

            // Assert
            Assert.Equal(0m, rate);
        }

        #endregion

        #region Cumulative Tax Calculation Tests

        [Fact]
        public void CalculateCumulativeTax_With3Million_ShouldReturn150000()
        {
            // Act
            var tax = _service.CalculateCumulativeTax(3_000_000);

            // Assert
            Assert.Equal(150_000, tax);
        }

        [Fact]
        public void CalculateCumulativeTax_With6Million_ShouldReturn350000()
        {
            // Act - 5M × 5% + 1M × 10%
            var tax = _service.CalculateCumulativeTax(6_000_000);

            // Assert
            Assert.Equal(350_000, tax);
        }

        [Fact]
        public void CalculateCumulativeTax_WithZeroIncome_ShouldReturnZero()
        {
            // Act
            var tax = _service.CalculateCumulativeTax(0);

            // Assert
            Assert.Equal(0, tax);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public async Task CalculateTax_WithExactlyBracketBoundary_5Million_ShouldCalculateCorrectly()
        {
            // Act - Exactly at boundary: 5 triệu
            var result = await _service.CalculateTaxAsync(5_000_000);

            // Assert - Should use Level 1: 5,000,000 × 5% = 250,000
            Assert.Equal(5_000_000, result.TaxableIncome);
            Assert.Equal(250_000, result.TaxAmount);
            Assert.Equal(1, result.ApplicableBracketLevel);
        }

        [Fact]
        public async Task CalculateTax_JustAboveBracketBoundary_5M1_ShouldMoveToLevel2()
        {
            // Act - Just above Level 1 boundary
            var result = await _service.CalculateTaxAsync(5_000_001);

            // Assert
            // Level 1: 5,000,000 × 5% = 250,000
            // Level 2: 1 × 10% = 0.10
            // Total: 250,000.10
            Assert.Equal(2, result.ApplicableBracketLevel);
            Assert.True(result.TaxAmount > 250_000);
        }

        [Fact]
        public async Task CalculateTax_At80MillionBoundary_ShouldUseLevel6()
        {
            // Act - Exactly at 80 triệu (boundary between Level 6 and 7)
            var result = await _service.CalculateTaxAsync(80_000_000);

            // Assert
            Assert.Equal(6, result.ApplicableBracketLevel);
        }

        [Fact]
        public async Task CalculateTax_JustAbove80Million_ShouldUseLevel7()
        {
            // Act - Just above 80 triệu
            var result = await _service.CalculateTaxAsync(80_000_001);

            // Assert
            Assert.Equal(7, result.ApplicableBracketLevel);
            Assert.True(result.TaxAmount > 0);
        }

        #endregion

        #region Real-world Scenarios

        [Theory]
        [InlineData(4_500_000, 225_000)]      // Level 1: 4.5M × 5% = 225K
        [InlineData(7_000_000, 450_000)]      // L1: 5M × 5% = 250K, L2: 2M × 10% = 200K, Total = 450K
        [InlineData(15_000_000, 1_500_000)]   // L1: 5M × 5% = 250K, L2: 5M × 10% = 500K, L3: 5M × 15% = 750K, Total = 1.5M
        [InlineData(50_000_000, 9_250_000)]   // Full calculation across brackets
        public async Task CalculateTax_VariousIncomes_ShouldCalculateCorrectly(
            decimal income, decimal expectedTax)
        {
            // Act
            var result = await _service.CalculateTaxAsync(income);

            // Assert
            Assert.Equal(expectedTax, result.TaxAmount);
        }

        #endregion
    }
}
