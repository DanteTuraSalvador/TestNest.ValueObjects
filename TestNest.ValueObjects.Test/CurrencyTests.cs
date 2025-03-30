using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class CurrencyTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidCurrency_ReturnsCurrencyWithCorrectValues()
    {
        var currency = Currency.Create("USD", "$");

        Assert.Equal("USD", currency.Code);
        Assert.Equal("$", currency.Symbol);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("JPY")]
    public void Parse_ValidCode_ReturnsCorrectCurrency(string code)
    {
        var currency = Currency.Parse(code);
        Assert.Equal(code, currency.Code);
    }

    [Fact]
    public void TryParse_ValidCode_ReturnsTrueAndCorrectCurrency()
    {
        var success = Currency.TryParse("GBP", out var currency);

        Assert.True(success);
        Assert.Equal("GBP", currency.Code);
        Assert.Equal("£", currency.Symbol);
    }

    [Fact]
    public void Default_IsPHP()
    {
        Assert.Equal(Currency.PHP, Currency.Default);
        Assert.Same(Currency.PHP, Currency.Default);
    }

    // Validation tests
    public static TheoryData<string?> InvalidCodes => new()
    {
        null,
        "",
        " ",
        "US",
        "USDD",
        "ABC",
        "usd" // case sensitive
    };

    [Theory]
    [MemberData(nameof(InvalidCodes))]
    public void Create_InvalidCode_ThrowsCurrencyException(string? invalidCode)
    {
        var ex = Assert.Throws<CurrencyException>(() => Currency.Create(invalidCode!, "$"));
        Assert.Equal(CurrencyException.ErrorCode.InvalidCurrencyCode, ex.Code);
    }

    public static TheoryData<string?> InvalidSymbols => new()
    {
        null,
        "",
        " "
    };

    [Theory]
    [MemberData(nameof(InvalidSymbols))]
    public void Create_InvalidSymbol_ThrowsCurrencyException(string? invalidSymbol)
    {
        var ex = Assert.Throws<CurrencyException>(() => Currency.Create("USD", invalidSymbol!));
        Assert.Equal(CurrencyException.ErrorCode.InvalidCurrencySymbol, ex.Code);
    }

    // Edge cases
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Parse_NullOrWhitespace_ReturnsEmpty(string? input)
    {
        var result = Currency.Parse(input!);
        Assert.Equal(Currency.Empty, result);
        Assert.True(result.IsEmpty());
    }

    [Fact]
    public void Parse_InvalidCode_ThrowsCurrencyException()
    {
        var ex = Assert.Throws<CurrencyException>(() => Currency.Parse("XYZ"));
        Assert.Equal(CurrencyException.ErrorCode.InvalidCurrencyCode, ex.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("XYZ")]
    public void TryParse_InvalidInput_ReturnsFalseAndEmpty(string? input)
    {
        var success = Currency.TryParse(input, out var currency);

        Assert.False(success);
        Assert.Equal(Currency.Empty, currency);
    }

    // Equality tests
    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        var currency1 = Currency.Create("USD", "$");
        var currency2 = Currency.Create("USD", "$");

        Assert.Equal(currency1, currency2);
        Assert.True(currency1 == currency2);
        Assert.False(currency1 != currency2);
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var usd = Currency.Create("USD", "$");
        var eur = Currency.Create("EUR", "€");

        Assert.NotEqual(usd, eur);
        Assert.True(usd != eur);
        Assert.False(usd == eur);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var usd = Currency.Create("USD", "$");
        Assert.False(usd.Equals(null));
    }

    // HashCode tests
    [Fact]
    public void GetHashCode_SameValues_ReturnsSameHashCode()
    {
        var currency1 = Currency.Create("USD", "$");
        var currency2 = Currency.Create("USD", "$");

        Assert.Equal(currency1.GetHashCode(), currency2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ReturnsDifferentHashCodes()
    {
        var usd = Currency.Create("USD", "$");
        var eur = Currency.Create("EUR", "€");

        Assert.NotEqual(usd.GetHashCode(), eur.GetHashCode());
    }

    // ToString tests
    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        var usd = Currency.Create("USD", "$");
        Assert.Equal("$ (USD)", usd.ToString());
    }

    [Fact]
    public void ToString_EmptyCurrency_ReturnsEmptyMarker()
    {
        Assert.Equal("[Empty Currency]", Currency.Empty.ToString());
    }

    // Static members tests
    [Fact]
    public void GetValidCurrencyCodes_ReturnsExpectedCodes()
    {
        var expected = new List<string> { "USD", "PHP", "EUR", "GBP", "JPY" };
        var actual = Currency.GetValidCurrencyCodes();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Empty_IsSingleton()
    {
        var empty1 = Currency.Empty;
        var empty2 = Currency.Empty;

        Assert.Same(empty1, empty2);
    }

    // TryCreate tests
    [Fact]
    public void TryCreate_ValidInput_ReturnsTrueAndCurrency()
    {
        var success = Currency.TryCreate("JPY", "¥", out var currency);

        Assert.True(success);
        Assert.Equal("JPY", currency?.Code);
        Assert.Equal("¥", currency?.Symbol);
    }

    [Theory]
    [MemberData(nameof(InvalidCodes))]
    public void TryCreate_InvalidCode_ReturnsFalseAndNull(string? invalidCode)
    {
        var success = Currency.TryCreate(invalidCode!, "$", out var currency);

        Assert.False(success);
        Assert.Null(currency);
    }

    [Theory]
    [MemberData(nameof(InvalidSymbols))]
    public void TryCreate_InvalidSymbol_ReturnsFalseAndNull(string? invalidSymbol)
    {
        var success = Currency.TryCreate("USD", invalidSymbol!, out var currency);

        Assert.False(success);
        Assert.Null(currency);
    }
}