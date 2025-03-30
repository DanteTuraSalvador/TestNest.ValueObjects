

using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using TestNest.ValueObjects.Domain.ValueObjects.Common;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Currency Value Object Demo ===");
        Console.WriteLine();

        DemoValueObjectBaseFunctionality();
        DemoCurrencyCreation();
        DemoCurrencyParsing();
        DemoEqualityComparisons();
        DemoEdgeCases();

        Console.WriteLine("\nAll demonstrations completed!");

        Console.ReadLine();
    }

    static void DemoValueObjectBaseFunctionality()
    {
        Console.WriteLine("1. ValueObject Base Class Functionality");
        Console.WriteLine("--------------------------------------");

        var money1 = new Money(100, "USD");
        var money2 = new Money(100, "USD");
        var money3 = new Money(200, "EUR");

        Console.WriteLine($"Money 1: {money1}");
        Console.WriteLine($"Money 2: {money2}");
        Console.WriteLine($"Money 3: {money3}");

        Console.WriteLine($"\nMoney1 == Money2: {money1 == money2} (expected true)");
        Console.WriteLine($"Money1.Equals(Money2): {money1.Equals(money2)} (expected true)");
        Console.WriteLine($"Money1 == Money3: {money1 == money3} (expected false)");
        Console.WriteLine($"Money1.GetHashCode() == Money2.GetHashCode(): {money1.GetHashCode() == money2.GetHashCode()} (expected true)");
        Console.WriteLine($"Money1.GetHashCode() == Money3.GetHashCode(): {money1.GetHashCode() == money3.GetHashCode()} (expected false)");
    }

    static void DemoCurrencyCreation()
    {
        Console.WriteLine("\n2. Currency Creation");
        Console.WriteLine("--------------------");

        var usd = Currency.Create("USD", "$");
        Console.WriteLine($"Created USD: {usd}");

        try
        {
            var invalid = Currency.Create("XYZ", "!");
            Console.WriteLine("This line won't execute");
        }
        catch (CurrencyException ex)
        {
            Console.WriteLine($"Failed to create invalid currency: {ex.Message}");
        }

        if (Currency.TryCreate("EUR", "€", out var eur))
        {
            Console.WriteLine($"Successfully created EUR: {eur}");
        }
    }

    static void DemoCurrencyParsing()
    {
        Console.WriteLine("\n3. Currency Parsing");
        Console.WriteLine("--------------------");

        var jpy = Currency.Parse("JPY");
        Console.WriteLine($"Parsed JPY: {jpy}");

        try
        {
            var invalid = Currency.Parse("ABC");
            Console.WriteLine("This line won't execute");
        }
        catch (CurrencyException ex)
        {
            Console.WriteLine($"Failed to parse invalid code: {ex.Message}");
        }

        if (Currency.TryParse("GBP", out var gbp))
        {
            Console.WriteLine($"Successfully parsed GBP: {gbp}");
        }

        var empty = Currency.Parse("");
        Console.WriteLine($"Parsed empty: {empty} (IsEmpty: {empty.IsEmpty()})");
    }

    static void DemoEqualityComparisons()
    {
        Console.WriteLine("\n4. Currency Equality");
        Console.WriteLine("--------------------");

        var php1 = Currency.PHP;
        var php2 = Currency.Create("PHP", "₱");
        var usd = Currency.USD;

        Console.WriteLine($"PHP1: {php1}");
        Console.WriteLine($"PHP2: {php2}");
        Console.WriteLine($"USD: {usd}");

        Console.WriteLine($"\nPHP1 == PHP2: {php1 == php2} (expected true)");
        Console.WriteLine($"PHP1.Equals(PHP2): {php1.Equals(php2)} (expected true)");
        Console.WriteLine($"PHP1 == USD: {php1 == usd} (expected false)");
        Console.WriteLine($"Default == PHP: {Currency.Default == Currency.PHP} (expected true)");
    }

    static void DemoEdgeCases()
    {
        Console.WriteLine("\n5. Edge Cases");
        Console.WriteLine("-------------");

        Console.WriteLine($"Currency.Empty: {Currency.Empty}");
        Console.WriteLine($"Currency.Default: {Currency.Default}");

        var empty1 = Currency.Empty;
        var empty2 = Currency.Parse(null!);
        Console.WriteLine($"empty1 == empty2: {empty1 == empty2} (expected true)");

        Console.WriteLine("\nValid Currency Codes:");
        foreach (var code in Currency.GetValidCurrencyCodes())
        {
            Console.WriteLine($"- {code}");
        }
    }
}

// Sample Money class to demonstrate ValueObject base functionality
public class Money : ValueObject
{
    public decimal Amount { get; }
    public string CurrencyCode { get; }

    public Money(decimal amount, string currencyCode)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return CurrencyCode;
    }

    public override string ToString() => $"{Amount} {CurrencyCode}";
}