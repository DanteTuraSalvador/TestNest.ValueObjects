namespace TestNest.ValueObjects.Domain.Exceptions;

public sealed class PriceException : Exception
{
    public enum ErrorCode
    {
        NegativeStandardPrice,
        NegativePeakPrice,
        PeakBelowStandard,
        NullCurrency,
        InvalidPrice
    }

    private static readonly IReadOnlyDictionary<ErrorCode, string> ErrorMessages = new Dictionary<ErrorCode, string>
    {
        [ErrorCode.NegativeStandardPrice] = "Standard price cannot be negative. Value: {0}",
        [ErrorCode.NegativePeakPrice] = "Peak price cannot be negative. Value: {0}",
        [ErrorCode.PeakBelowStandard] = "Peak price ({0}) cannot be less than standard price ({1})",
        [ErrorCode.NullCurrency] = "Currency cannot be null or empty",
        [ErrorCode.InvalidPrice] = "The price is invalid: {0}"
    };

    public ErrorCode Code { get; }

    private PriceException(ErrorCode code, string message)
        : base(message)
    {
        Code = code;
    }

    public static PriceException NegativeStandardPrice(decimal amount)
        => new(ErrorCode.NegativeStandardPrice, string.Format(ErrorMessages[ErrorCode.NegativeStandardPrice], amount));

    public static PriceException NegativePeakPrice(decimal amount)
        => new(ErrorCode.NegativePeakPrice, string.Format(ErrorMessages[ErrorCode.NegativePeakPrice], amount));

    public static PriceException PeakBelowStandard(decimal peakPrice, decimal standardPrice)
        => new(ErrorCode.PeakBelowStandard, string.Format(ErrorMessages[ErrorCode.PeakBelowStandard], peakPrice, standardPrice));

    public static PriceException NullCurrency()
        => new(ErrorCode.NullCurrency, ErrorMessages[ErrorCode.NullCurrency]);

    public static PriceException InvalidPrice(string reason)
        => new(ErrorCode.InvalidPrice, string.Format(ErrorMessages[ErrorCode.InvalidPrice], reason));
}
