using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class MoneyTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidMoney_ReturnsMoneyWithCorrectValues()
    {
        var money = Money.Create(100.50m, Currency.USD);

        Assert.Equal(100.50m, money.Amount);
        Assert.Equal(Currency.USD, money.Currency);
    }

    [Fact]
    public void Create_ZeroAmount_ReturnsMoneyWithZero()
    {
        var money = Money.Create(0, Currency.EUR);

        Assert.Equal(0, money.Amount);
        Assert.True(money.IsZero());
    }

    [Fact]
    public void TryCreate_ValidMoney_ReturnsTrueAndMoney()
    {
        var success = Money.TryCreate(50m, Currency.GBP, out var money);

        Assert.True(success);
        Assert.NotNull(money);
        Assert.Equal(50m, money.Amount);
    }

    // Arithmetic tests
    [Fact]
    public void Add_SameCurrency_ReturnsSum()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(50m, Currency.USD);

        var result = money1.Add(money2);

        Assert.Equal(150m, result.Amount);
        Assert.Equal(Currency.USD, result.Currency);
    }

    [Fact]
    public void Add_UsingOperator_ReturnsSum()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(50m, Currency.USD);

        var result = money1 + money2;

        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void Subtract_SameCurrency_ReturnsDifference()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(30m, Currency.USD);

        var result = money1.Subtract(money2);

        Assert.Equal(70m, result.Amount);
    }

    [Fact]
    public void Multiply_ByFactor_ReturnsProduct()
    {
        var money = Money.Create(100m, Currency.USD);

        var result = money.Multiply(1.5m);

        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void Divide_ByDivisor_ReturnsQuotient()
    {
        var money = Money.Create(100m, Currency.USD);

        var result = money.Divide(4);

        Assert.Equal(25m, result.Amount);
    }

    // Validation tests
    [Fact]
    public void Create_NegativeAmount_ThrowsMoneyException()
    {
        var exception = Assert.Throws<MoneyException>(() => Money.Create(-10m, Currency.USD));
        Assert.Equal(MoneyException.ErrorCode.NegativeAmount, exception.Code);
    }

    [Fact]
    public void Create_NullCurrency_ThrowsMoneyException()
    {
        var exception = Assert.Throws<MoneyException>(() => Money.Create(100m, null!));
        Assert.Equal(MoneyException.ErrorCode.NullCurrency, exception.Code);
    }

    [Fact]
    public void Create_EmptyCurrency_ThrowsMoneyException()
    {
        var exception = Assert.Throws<MoneyException>(() => Money.Create(100m, Currency.Empty));
        Assert.Equal(MoneyException.ErrorCode.NullCurrency, exception.Code);
    }

    [Fact]
    public void Add_DifferentCurrency_ThrowsMoneyException()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(50m, Currency.EUR);

        var exception = Assert.Throws<MoneyException>(() => money1.Add(money2));
        Assert.Equal(MoneyException.ErrorCode.CurrencyMismatch, exception.Code);
    }

    [Fact]
    public void Subtract_ResultNegative_ThrowsMoneyException()
    {
        var money1 = Money.Create(50m, Currency.USD);
        var money2 = Money.Create(100m, Currency.USD);

        Assert.Throws<MoneyException>(() => money1.Subtract(money2));
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        var money = Money.Create(100m, Currency.USD);

        Assert.Throws<DivideByZeroException>(() => money.Divide(0));
    }

    [Fact]
    public void TryCreate_NegativeAmount_ReturnsFalse()
    {
        var success = Money.TryCreate(-10m, Currency.USD, out var money);

        Assert.False(success);
        Assert.Null(money);
    }

    // Comparison operators tests
    [Fact]
    public void GreaterThan_LargerAmount_ReturnsTrue()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(50m, Currency.USD);

        Assert.True(money1 > money2);
        Assert.False(money2 > money1);
    }

    [Fact]
    public void LessThan_SmallerAmount_ReturnsTrue()
    {
        var money1 = Money.Create(50m, Currency.USD);
        var money2 = Money.Create(100m, Currency.USD);

        Assert.True(money1 < money2);
    }

    // Equality tests
    [Fact]
    public void Equals_SameAmountAndCurrency_ReturnsTrue()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(100m, Currency.USD);

        Assert.Equal(money1, money2);
        Assert.True(money1 == money2);
    }

    [Fact]
    public void Equals_DifferentAmount_ReturnsFalse()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(50m, Currency.USD);

        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void Equals_DifferentCurrency_ReturnsFalse()
    {
        var money1 = Money.Create(100m, Currency.USD);
        var money2 = Money.Create(100m, Currency.EUR);

        Assert.NotEqual(money1, money2);
    }

    // With* methods tests
    [Fact]
    public void WithAmount_ReturnsNewMoneyWithUpdatedAmount()
    {
        var money = Money.Create(100m, Currency.USD);

        var updated = money.WithAmount(200m);

        Assert.Equal(200m, updated.Amount);
        Assert.Equal(Currency.USD, updated.Currency);
        Assert.Equal(100m, money.Amount); // Original unchanged
    }

    [Fact]
    public void WithCurrency_ReturnsNewMoneyWithUpdatedCurrency()
    {
        var money = Money.Create(100m, Currency.USD);

        var updated = money.WithCurrency(Currency.EUR);

        Assert.Equal(100m, updated.Amount);
        Assert.Equal(Currency.EUR, updated.Currency);
    }

    // ToString tests
    [Fact]
    public void ToString_ValidMoney_ReturnsFormattedString()
    {
        var money = Money.Create(100.50m, Currency.USD);

        Assert.Equal("$100.50", money.ToString());
    }

    [Fact]
    public void ToFullString_ValidMoney_ReturnsDetailedString()
    {
        var money = Money.Create(100.50m, Currency.USD);

        Assert.Equal("$100.50 (USD)", money.ToFullString());
    }

    // Empty and Zero tests
    [Fact]
    public void Zero_IsSingleton()
    {
        var zero1 = Money.Zero;
        var zero2 = Money.Zero;

        Assert.Same(zero1, zero2);
    }

    [Fact]
    public void Empty_IsEmpty_ReturnsTrue()
    {
        Assert.True(Money.Empty.IsEmpty());
    }
}
