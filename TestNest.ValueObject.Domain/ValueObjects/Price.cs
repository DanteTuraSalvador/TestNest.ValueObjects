using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects.Common;

namespace TestNest.ValueObjects.Domain.ValueObjects;

public sealed class Price : ValueObject
{
    private static readonly Lazy<Price> _lazyEmpty = new(() => new Price());
    public static Price Empty => _lazyEmpty.Value;

    private static readonly Lazy<Price> _lazyZero = new(() => new Price(0, 0, Currency.Default));
    public static Price Zero => _lazyZero.Value;

    public decimal StandardPrice { get; }
    public decimal PeakPrice { get; }
    public Currency Currency { get; }

    private Price()
    {
        StandardPrice = 0;
        PeakPrice = 0;
        Currency = Currency.Empty;
    }

    private Price(decimal standardPrice, decimal peakPrice, Currency currency)
    {
        StandardPrice = standardPrice;
        PeakPrice = peakPrice;
        Currency = currency;
    }

    public static Price Create(decimal standardPrice, decimal peakPrice, Currency currency)
    {
        ValidateStandardPrice(standardPrice);
        ValidatePeakPrice(peakPrice);
        ValidatePeakNotBelowStandard(peakPrice, standardPrice);
        ValidateCurrency(currency);

        return new Price(standardPrice, peakPrice, currency);
    }

    public static Price Create(decimal standardPrice, Currency currency)
        => Create(standardPrice, standardPrice, currency);

    public static bool TryCreate(
        decimal standardPrice,
        decimal peakPrice,
        Currency? currency,
        out Price? price)
    {
        price = null;

        if (standardPrice < 0)
            return false;

        if (peakPrice < 0)
            return false;

        if (peakPrice < standardPrice)
            return false;

        if (currency is null || currency.IsEmpty())
            return false;

        price = new Price(standardPrice, peakPrice, currency);
        return true;
    }

    public Price WithStandardPrice(decimal newStandardPrice)
        => Create(newStandardPrice, PeakPrice, Currency);

    public Price WithPeakPrice(decimal newPeakPrice)
        => Create(StandardPrice, newPeakPrice, Currency);

    public Price WithCurrency(Currency newCurrency)
        => Create(StandardPrice, PeakPrice, newCurrency);

    public Price ApplyDiscount(decimal discountPercentage)
    {
        if (discountPercentage < 0 || discountPercentage > 100)
            throw PriceException.InvalidPrice($"Discount percentage must be between 0 and 100. Value: {discountPercentage}");

        var factor = 1 - (discountPercentage / 100);
        return new Price(
            Math.Round(StandardPrice * factor, 2),
            Math.Round(PeakPrice * factor, 2),
            Currency
        );
    }

    public Price ApplyMarkup(decimal markupPercentage)
    {
        if (markupPercentage < 0)
            throw PriceException.InvalidPrice($"Markup percentage cannot be negative. Value: {markupPercentage}");

        var factor = 1 + (markupPercentage / 100);
        return new Price(
            Math.Round(StandardPrice * factor, 2),
            Math.Round(PeakPrice * factor, 2),
            Currency
        );
    }

    public bool IsEmpty() => this == Empty || Currency.IsEmpty();

    public bool IsZero() => StandardPrice == 0 && PeakPrice == 0;

    public bool HasPeakPricing() => PeakPrice > StandardPrice;

    public decimal GetPriceDifference() => PeakPrice - StandardPrice;

    public decimal GetPeakPremiumPercentage()
    {
        if (StandardPrice == 0) return 0;
        return Math.Round((PeakPrice - StandardPrice) / StandardPrice * 100, 2);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return StandardPrice;
        yield return PeakPrice;
        yield return Currency;
    }

    public override string ToString()
    {
        if (IsEmpty()) return "[Empty Price]";

        if (HasPeakPricing())
            return $"{Currency.Symbol}{StandardPrice:N2} / {Currency.Symbol}{PeakPrice:N2} (Peak)";

        return $"{Currency.Symbol}{StandardPrice:N2}";
    }

    public string ToDetailedString()
    {
        if (IsEmpty()) return "[Empty Price]";

        return $"Standard: {Currency.Symbol}{StandardPrice:N2}, Peak: {Currency.Symbol}{PeakPrice:N2} ({Currency.Code})";
    }

    private static void ValidateStandardPrice(decimal standardPrice)
    {
        if (standardPrice < 0)
            throw PriceException.NegativeStandardPrice(standardPrice);
    }

    private static void ValidatePeakPrice(decimal peakPrice)
    {
        if (peakPrice < 0)
            throw PriceException.NegativePeakPrice(peakPrice);
    }

    private static void ValidatePeakNotBelowStandard(decimal peakPrice, decimal standardPrice)
    {
        if (peakPrice < standardPrice)
            throw PriceException.PeakBelowStandard(peakPrice, standardPrice);
    }

    private static void ValidateCurrency(Currency? currency)
    {
        if (currency is null || currency.IsEmpty())
            throw PriceException.NullCurrency();
    }
}
