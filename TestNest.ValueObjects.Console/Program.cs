using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using TestNest.ValueObjects.Domain.ValueObjects.Common;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== TestNest.ValueObjects Demo Application ===");
        Console.WriteLine("Demonstrating the Value Object pattern in .NET 8.0");
        Console.WriteLine(new string('=', 50));

        DemoCurrency();
        DemoEmail();
        DemoPhoneNumber();
        DemoMoney();
        DemoAddress();
        DemoPrice();
        DemoValueEquality();

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("All demonstrations completed!");
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }

    static void DemoCurrency()
    {
        Console.WriteLine("\n1. CURRENCY VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Predefined instances
        Console.WriteLine("Predefined currencies:");
        Console.WriteLine($"  USD: {Currency.USD} ({Currency.USD.Symbol})");
        Console.WriteLine($"  EUR: {Currency.EUR} ({Currency.EUR.Symbol})");
        Console.WriteLine($"  PHP: {Currency.PHP} ({Currency.PHP.Symbol})");

        // Create with validation
        var gbp = Currency.Create("GBP", "£");
        Console.WriteLine($"\nCreated: {gbp}");

        // Parse from code
        var jpy = Currency.Parse("JPY");
        Console.WriteLine($"Parsed JPY: {jpy}");

        // Safe parsing
        if (Currency.TryParse("EUR", out var eur))
        {
            Console.WriteLine($"TryParse EUR: {eur}");
        }

        // Error handling
        try
        {
            var invalid = Currency.Create("INVALID", "!");
        }
        catch (CurrencyException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoEmail()
    {
        Console.WriteLine("\n2. EMAIL VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Create with validation
        var email = Email.Create("User@Example.COM");
        Console.WriteLine($"Created: {email.Address}");
        Console.WriteLine($"  Local Part: {email.LocalPart}");
        Console.WriteLine($"  Domain: {email.Domain}");

        // Safe creation
        if (Email.TryCreate("contact@company.org", out var contact))
        {
            Console.WriteLine($"TryCreate: {contact!.Address}");
        }

        // Parse returns Empty for invalid
        var empty = Email.Parse("");
        Console.WriteLine($"Parse empty: IsEmpty = {empty.IsEmpty()}");

        // Error handling
        try
        {
            var invalid = Email.Create("not-an-email");
        }
        catch (EmailException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoPhoneNumber()
    {
        Console.WriteLine("\n3. PHONENUMBER VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Create with country code and number
        var usPhone = PhoneNumber.Create("+1", "5551234567");
        Console.WriteLine($"Created: {usPhone.FullNumber}");
        Console.WriteLine($"  Formatted: {usPhone.ToFormattedString()}");

        // Create from full number
        var ukPhone = PhoneNumber.Create("+447911123456");
        Console.WriteLine($"UK Phone: {ukPhone.FullNumber}");
        Console.WriteLine($"  Country Code: {ukPhone.CountryCode}");
        Console.WriteLine($"  Number: {ukPhone.Number}");

        // Valid country codes
        Console.WriteLine("\nSupported country codes:");
        Console.WriteLine($"  {string.Join(", ", PhoneNumber.GetValidCountryCodes())}");

        // Error handling
        try
        {
            var invalid = PhoneNumber.Create("+99", "1234567");
        }
        catch (PhoneNumberException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoMoney()
    {
        Console.WriteLine("\n4. MONEY VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Create money
        var price = Money.Create(99.99m, Currency.USD);
        var tax = Money.Create(8.50m, Currency.USD);
        Console.WriteLine($"Price: {price}");
        Console.WriteLine($"Tax: {tax}");

        // Arithmetic operations
        var total = price + tax;
        Console.WriteLine($"Total (price + tax): {total}");

        var discount = price * 0.10m;
        Console.WriteLine($"10% Discount: {discount}");

        var perUnit = price / 3;
        Console.WriteLine($"Per unit (price / 3): {perUnit}");

        // Comparison
        Console.WriteLine($"Price > Tax: {price > tax}");

        // Full string format
        Console.WriteLine($"Full format: {price.ToFullString()}");

        // Error handling
        try
        {
            var negative = Money.Create(-50m, Currency.USD);
        }
        catch (MoneyException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoAddress()
    {
        Console.WriteLine("\n5. ADDRESS VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Create full address
        var address = Address.Create(
            street: "123 Main Street",
            city: "New York",
            state: "NY",
            postalCode: "10001",
            country: "USA"
        );
        Console.WriteLine("Full address:");
        Console.WriteLine($"  Single line: {address.ToSingleLineString()}");
        Console.WriteLine("  Multi-line:");
        foreach (var line in address.ToMultiLineString().Split(Environment.NewLine))
        {
            Console.WriteLine($"    {line}");
        }

        // Create minimal address
        var simple = Address.Create("456 Oak Ave", "London", "UK");
        Console.WriteLine($"\nMinimal address: {simple}");

        // Immutable updates
        var updated = address.WithCity("Brooklyn").WithPostalCode("11201");
        Console.WriteLine($"Updated address: {updated}");

        // Error handling
        try
        {
            var invalid = Address.Create("", "City", "Country");
        }
        catch (AddressException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoPrice()
    {
        Console.WriteLine("\n6. PRICE VALUE OBJECT");
        Console.WriteLine(new string('-', 30));

        // Create with standard and peak pricing
        var price = Price.Create(100.00m, 150.00m, Currency.USD);
        Console.WriteLine($"Price with peak: {price}");
        Console.WriteLine($"  Detailed: {price.ToDetailedString()}");
        Console.WriteLine($"  Has peak pricing: {price.HasPeakPricing()}");
        Console.WriteLine($"  Price difference: ${price.GetPriceDifference()}");
        Console.WriteLine($"  Peak premium: {price.GetPeakPremiumPercentage()}%");

        // Create uniform price
        var flat = Price.Create(50.00m, Currency.EUR);
        Console.WriteLine($"\nFlat price: {flat}");
        Console.WriteLine($"  Has peak pricing: {flat.HasPeakPricing()}");

        // Apply discount
        var discounted = price.ApplyDiscount(20);
        Console.WriteLine($"\nAfter 20% discount: {discounted}");

        // Apply markup
        var markedUp = price.ApplyMarkup(15);
        Console.WriteLine($"After 15% markup: {markedUp}");

        // Error handling
        try
        {
            var invalid = Price.Create(100.00m, 50.00m, Currency.USD);
        }
        catch (PriceException ex)
        {
            Console.WriteLine($"Expected error: {ex.Message}");
        }
    }

    static void DemoValueEquality()
    {
        Console.WriteLine("\n7. VALUE-BASED EQUALITY");
        Console.WriteLine(new string('-', 30));

        // Email normalization
        var email1 = Email.Create("user@example.com");
        var email2 = Email.Create("USER@EXAMPLE.COM");
        Console.WriteLine($"Email1: {email1}");
        Console.WriteLine($"Email2: {email2}");
        Console.WriteLine($"  email1 == email2: {email1 == email2}");
        Console.WriteLine($"  Same hash code: {email1.GetHashCode() == email2.GetHashCode()}");

        // Currency equality
        var usd1 = Currency.USD;
        var usd2 = Currency.Create("USD", "$");
        Console.WriteLine($"\nUSD1 (predefined): {usd1}");
        Console.WriteLine($"USD2 (created): {usd2}");
        Console.WriteLine($"  usd1 == usd2: {usd1 == usd2}");

        // Money equality
        var money1 = Money.Create(100.00m, Currency.USD);
        var money2 = Money.Create(100.00m, Currency.USD);
        var money3 = Money.Create(100.00m, Currency.EUR);
        Console.WriteLine($"\nMoney1: {money1.ToFullString()}");
        Console.WriteLine($"Money2: {money2.ToFullString()}");
        Console.WriteLine($"Money3: {money3.ToFullString()}");
        Console.WriteLine($"  money1 == money2: {money1 == money2}");
        Console.WriteLine($"  money1 == money3: {money1 == money3}");
    }
}
