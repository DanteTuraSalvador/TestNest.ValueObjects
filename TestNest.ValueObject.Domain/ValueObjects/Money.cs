using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects.Common;

namespace TestNest.ValueObjects.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    private static readonly Lazy<Money> _lazyZero = new(() => new Money(0, Currency.Default));
    public static Money Zero => _lazyZero.Value;

    private static readonly Lazy<Money> _lazyEmpty = new(() => new Money());
    public static Money Empty => _lazyEmpty.Value;

    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money()
    {
        Amount = 0;
        Currency = Currency.Empty;
    }

    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, Currency currency)
    {
        ValidateAmount(amount);
        ValidateCurrency(currency);
        return new Money(amount, currency);
    }

    public static bool TryCreate(decimal amount, Currency? currency, out Money? money)
    {
        money = null;

        if (amount < 0)
            return false;

        if (currency is null || currency.IsEmpty())
            return false;

        money = new Money(amount, currency);
        return true;
    }

    public Money Add(Money other)
    {
        ValidateSameCurrency(this, other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        ValidateSameCurrency(this, other);
        var result = Amount - other.Amount;
        if (result < 0)
            throw MoneyException.NegativeAmount(result);
        return new Money(result, Currency);
    }

    public Money Multiply(decimal factor)
    {
        var result = Amount * factor;
        if (result < 0)
            throw MoneyException.NegativeAmount(result);
        return new Money(result, Currency);
    }

    public Money Divide(decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide money by zero");
        var result = Amount / divisor;
        if (result < 0)
            throw MoneyException.NegativeAmount(result);
        return new Money(result, Currency);
    }

    public Money WithAmount(decimal newAmount)
        => Create(newAmount, Currency);

    public Money WithCurrency(Currency newCurrency)
        => Create(Amount, newCurrency);

    public bool IsEmpty() => this == Empty || Currency.IsEmpty();

    public bool IsZero() => Amount == 0;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
    {
        if (IsEmpty()) return "[Empty Money]";
        return $"{Currency.Symbol}{Amount:N2}";
    }

    public string ToFullString()
    {
        if (IsEmpty()) return "[Empty Money]";
        return $"{Currency.Symbol}{Amount:N2} ({Currency.Code})";
    }

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money money, decimal factor) => money.Multiply(factor);
    public static Money operator /(Money money, decimal divisor) => money.Divide(divisor);

    public static bool operator >(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount >= right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount <= right.Amount;
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount < 0)
            throw MoneyException.NegativeAmount(amount);
    }

    private static void ValidateCurrency(Currency? currency)
    {
        if (currency is null || currency.IsEmpty())
            throw MoneyException.NullCurrency();
    }

    private static void ValidateSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw MoneyException.CurrencyMismatch(left.Currency.Code, right.Currency.Code);
    }
}
