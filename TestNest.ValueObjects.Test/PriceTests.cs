using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class PriceTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidPrice_ReturnsPriceWithCorrectValues()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal(100m, price.StandardPrice);
        Assert.Equal(150m, price.PeakPrice);
        Assert.Equal(Currency.USD, price.Currency);
    }

    [Fact]
    public void Create_SinglePrice_SetsStandardAndPeakEqual()
    {
        var price = Price.Create(100m, Currency.USD);

        Assert.Equal(100m, price.StandardPrice);
        Assert.Equal(100m, price.PeakPrice);
    }

    [Fact]
    public void Create_ZeroPrices_ReturnsZeroPrice()
    {
        var price = Price.Create(0m, 0m, Currency.USD);

        Assert.Equal(0m, price.StandardPrice);
        Assert.Equal(0m, price.PeakPrice);
        Assert.True(price.IsZero());
    }

    [Fact]
    public void TryCreate_ValidPrice_ReturnsTrueAndPrice()
    {
        var success = Price.TryCreate(100m, 150m, Currency.USD, out var price);

        Assert.True(success);
        Assert.NotNull(price);
        Assert.Equal(100m, price.StandardPrice);
        Assert.Equal(150m, price.PeakPrice);
    }

    // Validation tests
    [Fact]
    public void Create_NegativeStandardPrice_ThrowsPriceException()
    {
        var exception = Assert.Throws<PriceException>(() =>
            Price.Create(-10m, 100m, Currency.USD));
        Assert.Equal(PriceException.ErrorCode.NegativeStandardPrice, exception.Code);
    }

    [Fact]
    public void Create_NegativePeakPrice_ThrowsPriceException()
    {
        var exception = Assert.Throws<PriceException>(() =>
            Price.Create(100m, -10m, Currency.USD));
        Assert.Equal(PriceException.ErrorCode.NegativePeakPrice, exception.Code);
    }

    [Fact]
    public void Create_PeakBelowStandard_ThrowsPriceException()
    {
        var exception = Assert.Throws<PriceException>(() =>
            Price.Create(100m, 50m, Currency.USD));
        Assert.Equal(PriceException.ErrorCode.PeakBelowStandard, exception.Code);
    }

    [Fact]
    public void Create_NullCurrency_ThrowsPriceException()
    {
        var exception = Assert.Throws<PriceException>(() =>
            Price.Create(100m, 150m, null!));
        Assert.Equal(PriceException.ErrorCode.NullCurrency, exception.Code);
    }

    [Fact]
    public void Create_EmptyCurrency_ThrowsPriceException()
    {
        var exception = Assert.Throws<PriceException>(() =>
            Price.Create(100m, 150m, Currency.Empty));
        Assert.Equal(PriceException.ErrorCode.NullCurrency, exception.Code);
    }

    [Fact]
    public void TryCreate_NegativeStandardPrice_ReturnsFalse()
    {
        var success = Price.TryCreate(-10m, 100m, Currency.USD, out var price);

        Assert.False(success);
        Assert.Null(price);
    }

    [Fact]
    public void TryCreate_PeakBelowStandard_ReturnsFalse()
    {
        var success = Price.TryCreate(100m, 50m, Currency.USD, out var price);

        Assert.False(success);
        Assert.Null(price);
    }

    // With* methods tests
    [Fact]
    public void WithStandardPrice_ReturnsNewPriceWithUpdatedStandardPrice()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        var updated = price.WithStandardPrice(80m);

        Assert.Equal(80m, updated.StandardPrice);
        Assert.Equal(150m, updated.PeakPrice);
        Assert.Equal(100m, price.StandardPrice); // Original unchanged
    }

    [Fact]
    public void WithPeakPrice_ReturnsNewPriceWithUpdatedPeakPrice()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        var updated = price.WithPeakPrice(200m);

        Assert.Equal(200m, updated.PeakPrice);
        Assert.Equal(100m, updated.StandardPrice);
    }

    [Fact]
    public void WithCurrency_ReturnsNewPriceWithUpdatedCurrency()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        var updated = price.WithCurrency(Currency.EUR);

        Assert.Equal(Currency.EUR, updated.Currency);
        Assert.Equal(100m, updated.StandardPrice);
        Assert.Equal(150m, updated.PeakPrice);
    }

    // Discount and markup tests
    [Fact]
    public void ApplyDiscount_ValidPercentage_ReducesPrices()
    {
        var price = Price.Create(100m, 200m, Currency.USD);

        var discounted = price.ApplyDiscount(10);

        Assert.Equal(90m, discounted.StandardPrice);
        Assert.Equal(180m, discounted.PeakPrice);
    }

    [Fact]
    public void ApplyDiscount_InvalidPercentage_ThrowsPriceException()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Throws<PriceException>(() => price.ApplyDiscount(-10));
        Assert.Throws<PriceException>(() => price.ApplyDiscount(110));
    }

    [Fact]
    public void ApplyMarkup_ValidPercentage_IncreasesPrices()
    {
        var price = Price.Create(100m, 200m, Currency.USD);

        var marked = price.ApplyMarkup(25);

        Assert.Equal(125m, marked.StandardPrice);
        Assert.Equal(250m, marked.PeakPrice);
    }

    [Fact]
    public void ApplyMarkup_NegativePercentage_ThrowsPriceException()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Throws<PriceException>(() => price.ApplyMarkup(-10));
    }

    // Peak pricing tests
    [Fact]
    public void HasPeakPricing_PeakGreaterThanStandard_ReturnsTrue()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.True(price.HasPeakPricing());
    }

    [Fact]
    public void HasPeakPricing_PeakEqualsStandard_ReturnsFalse()
    {
        var price = Price.Create(100m, Currency.USD);

        Assert.False(price.HasPeakPricing());
    }

    [Fact]
    public void GetPriceDifference_ReturnsDifference()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal(50m, price.GetPriceDifference());
    }

    [Fact]
    public void GetPeakPremiumPercentage_ReturnsCorrectPercentage()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal(50m, price.GetPeakPremiumPercentage());
    }

    // Equality tests
    [Fact]
    public void Equals_SamePrice_ReturnsTrue()
    {
        var price1 = Price.Create(100m, 150m, Currency.USD);
        var price2 = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal(price1, price2);
        Assert.True(price1 == price2);
    }

    [Fact]
    public void Equals_DifferentStandardPrice_ReturnsFalse()
    {
        var price1 = Price.Create(100m, 150m, Currency.USD);
        var price2 = Price.Create(90m, 150m, Currency.USD);

        Assert.NotEqual(price1, price2);
        Assert.True(price1 != price2);
    }

    [Fact]
    public void Equals_DifferentCurrency_ReturnsFalse()
    {
        var price1 = Price.Create(100m, 150m, Currency.USD);
        var price2 = Price.Create(100m, 150m, Currency.EUR);

        Assert.NotEqual(price1, price2);
    }

    [Fact]
    public void GetHashCode_SamePrice_ReturnsSameHashCode()
    {
        var price1 = Price.Create(100m, 150m, Currency.USD);
        var price2 = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal(price1.GetHashCode(), price2.GetHashCode());
    }

    // Empty and Zero tests
    [Fact]
    public void Empty_IsSingleton()
    {
        var empty1 = Price.Empty;
        var empty2 = Price.Empty;

        Assert.Same(empty1, empty2);
    }

    [Fact]
    public void Empty_IsEmpty_ReturnsTrue()
    {
        Assert.True(Price.Empty.IsEmpty());
    }

    [Fact]
    public void Zero_IsZero_ReturnsTrue()
    {
        Assert.True(Price.Zero.IsZero());
    }

    // ToString tests
    [Fact]
    public void ToString_WithPeakPricing_ReturnsFormattedString()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal("$100.00 / $150.00 (Peak)", price.ToString());
    }

    [Fact]
    public void ToString_NoPeakPricing_ReturnsSimpleFormat()
    {
        var price = Price.Create(100m, Currency.USD);

        Assert.Equal("$100.00", price.ToString());
    }

    [Fact]
    public void ToDetailedString_ReturnsDetailedFormat()
    {
        var price = Price.Create(100m, 150m, Currency.USD);

        Assert.Equal("Standard: $100.00, Peak: $150.00 (USD)", price.ToDetailedString());
    }

    [Fact]
    public void ToString_Empty_ReturnsEmptyString()
    {
        Assert.Equal("[Empty Price]", Price.Empty.ToString());
    }
}
