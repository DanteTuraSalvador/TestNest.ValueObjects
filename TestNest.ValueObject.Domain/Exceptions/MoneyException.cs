namespace TestNest.ValueObjects.Domain.Exceptions;

public sealed class MoneyException : Exception
{
    public enum ErrorCode
    {
        NegativeAmount,
        NullCurrency,
        CurrencyMismatch,
        InvalidAmount
    }

    private static readonly IReadOnlyDictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>
    {
        [ErrorCode.NegativeAmount] = "Money amount cannot be negative. Value: {0}",
        [ErrorCode.NullCurrency] = "Currency cannot be null",
        [ErrorCode.CurrencyMismatch] = "Cannot perform operation with different currencies: {0} and {1}",
        [ErrorCode.InvalidAmount] = "The amount '{0}' is invalid"
    };

    public ErrorCode Code { get; }

    private MoneyException(ErrorCode code, string message)
        : base(message)
    {
        Code = code;
    }

    public static MoneyException NegativeAmount(decimal amount)
        => new(ErrorCode.NegativeAmount, string.Format(ErrorMessages[ErrorCode.NegativeAmount], amount));

    public static MoneyException NullCurrency()
        => new(ErrorCode.NullCurrency, ErrorMessages[ErrorCode.NullCurrency]);

    public static MoneyException CurrencyMismatch(string currency1, string currency2)
        => new(ErrorCode.CurrencyMismatch, string.Format(ErrorMessages[ErrorCode.CurrencyMismatch], currency1, currency2));

    public static MoneyException InvalidAmount(string? amount)
        => new(ErrorCode.InvalidAmount, string.Format(ErrorMessages[ErrorCode.InvalidAmount], amount ?? "null"));
}
