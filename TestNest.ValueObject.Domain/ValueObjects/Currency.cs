using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects.Common;

namespace TestNest.ValueObjects.Domain.ValueObjects;

public sealed class Currency : ValueObject
{
    private static readonly HashSet<string> ValidCurrencyCodes = new()
    {
        "USD", "PHP", "EUR", "GBP", "JPY"
    };

    // 1. First define all static currency instances
    public static readonly Currency PHP = new("PHP", "₱");
    public static readonly Currency USD = new("USD", "$");
    public static readonly Currency EUR = new("EUR", "€");
    public static readonly Currency GBP = new("GBP", "£");
    public static readonly Currency JPY = new("JPY", "¥");

    // 2. Then define Default as a property
    public static Currency Default { get; } = PHP;

    // Lazy initialization for Empty
    private static readonly Lazy<Currency> _lazyEmpty =
        new Lazy<Currency>(() => new Currency());
    public static Currency Empty => _lazyEmpty.Value;

    public string Code { get; }
    public string Symbol { get; }

    private Currency()
    {
        Code = string.Empty;
        Symbol = string.Empty;
    }

    private Currency(string code, string symbol)
    {
        Code = code;
        Symbol = symbol;
    }

    public static Currency Create(string code, string symbol)
    {
        ValidateCurrencyCode(code);
        ValidateCurrencySymbol(symbol);
        return new Currency(code, symbol);
    }

    public static bool TryCreate(string code, string symbol, out Currency? currency)
    {
        currency = null;

        if (string.IsNullOrWhiteSpace(code) || code.Length != 3 ||
            !ValidCurrencyCodes.Contains(code))
            return false;

        if (string.IsNullOrWhiteSpace(symbol))
            return false;

        currency = new Currency(code, symbol);
        return true;
    }

    public static Currency Parse(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Empty;

        return code.ToUpperInvariant() switch
        {
            "PHP" => PHP,
            "USD" => USD,
            "EUR" => EUR,
            "GBP" => GBP,
            "JPY" => JPY,
            _ => throw CurrencyException.InvalidCurrencyCode(code, ValidCurrencyCodes)
        };
    }

    public static bool TryParse(string? code, out Currency currency)
    {
        currency = Empty;

        if (string.IsNullOrWhiteSpace(code))
            return false;

        currency = code.ToUpperInvariant() switch
        {
            "PHP" => PHP,
            "USD" => USD,
            "EUR" => EUR,
            "GBP" => GBP,
            "JPY" => JPY,
            _ => Empty
        };

        return currency != Empty;
    }

    public bool IsEmpty() => this == Empty;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Code;
        yield return Symbol;
    }

    public override string ToString() => IsEmpty() ? "[Empty Currency]" : $"{Symbol} ({Code})";

    public static IReadOnlyCollection<string> GetValidCurrencyCodes() =>
        ValidCurrencyCodes.ToList().AsReadOnly();

    private static void ValidateCurrencyCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            throw CurrencyException.InvalidCurrencyCode(code, ValidCurrencyCodes);

        if (!ValidCurrencyCodes.Contains(code))
            throw CurrencyException.InvalidCurrencyCode(code, ValidCurrencyCodes);
    }

    private static void ValidateCurrencySymbol(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw CurrencyException.InvalidCurrencySymbol();
    }
}