# TestNest.ValueObjects

A .NET 8.0 implementation of the Value Object pattern for Domain-Driven Design. This library provides a foundation for creating self-validating, immutable domain types that eliminate primitive obsession.

## Features

- **Immutable by design** - All properties are read-only after creation
- **Value-based equality** - Objects are compared by their values, not references
- **Self-validating** - Validation rules are encapsulated within the value object
- **Thread-safe** - Lazy initialization ensures safe singleton patterns
- **Zero dependencies** - No external packages required

## Installation

Clone the repository and add a reference to `TestNest.ValueObjects.Domain` in your project.

```bash
git clone https://github.com/DanteTuraSalvador/TestNest.ValueObjects.git
```

## Quick Start

### Currency

```csharp
// Use predefined currency instances
var usd = Currency.USD;
var eur = Currency.EUR;

// Parse from string
var currency = Currency.Parse("USD");

// Safe parsing with TryParse
if (Currency.TryParse("GBP", out var gbp))
{
    Console.WriteLine(gbp.Symbol); // £
}

// Create with validation
var php = Currency.Create("PHP", "₱");
```

### Email

```csharp
// Create with validation
var email = Email.Create("user@example.com");
Console.WriteLine(email.LocalPart); // user
Console.WriteLine(email.Domain);    // example.com

// Safe creation
if (Email.TryCreate("contact@company.org", out var result))
{
    Console.WriteLine(result.Address);
}

// Parse (returns Empty for invalid input)
var parsed = Email.Parse("info@test.com");
```

### PhoneNumber

```csharp
// Create with country code and number
var phone = PhoneNumber.Create("+1", "5551234567");
Console.WriteLine(phone.FullNumber);        // +15551234567
Console.WriteLine(phone.ToFormattedString()); // +1 (555) 123-4567

// Create from full number string
var phone2 = PhoneNumber.Create("+441onal234567890");

// Supported country codes: +1, +44, +63, +81, +86, +91, +49, +33, +39, +34
```

### Money

```csharp
// Create money with amount and currency
var price = Money.Create(99.99m, Currency.USD);
var tax = Money.Create(8.50m, Currency.USD);

// Arithmetic operations
var total = price + tax;           // $108.49
var discount = price * 0.10m;      // $9.99
var perUnit = price / 3;           // $33.33

// Comparison operators
if (price > tax) { /* ... */ }

// Display formatting
Console.WriteLine(price.ToString());     // $99.99
Console.WriteLine(price.ToFullString()); // $99.99 (USD)
```

### Address

```csharp
// Create full address
var address = Address.Create(
    street: "123 Main Street",
    city: "New York",
    state: "NY",
    postalCode: "10001",
    country: "USA"
);

// Create minimal address (street, city, country)
var simple = Address.Create("456 Oak Ave", "London", "UK");

// Immutable updates with With* methods
var updated = address.WithCity("Brooklyn").WithPostalCode("11201");

// Display formatting
Console.WriteLine(address.ToSingleLineString());
// 123 Main Street, New York, NY, 10001, USA

Console.WriteLine(address.ToMultiLineString());
// 123 Main Street
// New York, NY 10001
// USA
```

### Price

```csharp
// Create with standard and peak pricing
var price = Price.Create(
    standardPrice: 100.00m,
    peakPrice: 150.00m,
    currency: Currency.USD
);

// Create uniform price (no peak pricing)
var flat = Price.Create(50.00m, Currency.EUR);

// Apply discount/markup
var discounted = price.ApplyDiscount(20);  // 20% off
var marked = price.ApplyMarkup(15);        // 15% increase

// Price analysis
Console.WriteLine(price.HasPeakPricing());         // True
Console.WriteLine(price.GetPriceDifference());     // 50.00
Console.WriteLine(price.GetPeakPremiumPercentage()); // 50.00%

// Display
Console.WriteLine(price.ToString()); // $100.00 / $150.00 (Peak)
```

## Value Equality

All value objects support value-based equality:

```csharp
var email1 = Email.Create("user@example.com");
var email2 = Email.Create("USER@EXAMPLE.COM");

Console.WriteLine(email1 == email2); // True - normalized to lowercase
Console.WriteLine(email1.GetHashCode() == email2.GetHashCode()); // True
```

## Creating Custom Value Objects

Inherit from the `ValueObject` base class and implement `GetAtomicValues()`:

```csharp
public sealed class ProductCode : ValueObject
{
    public string Category { get; }
    public int Number { get; }

    private ProductCode(string category, int number)
    {
        Category = category;
        Number = number;
    }

    public static ProductCode Create(string category, int number)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category is required");
        if (number <= 0)
            throw new ArgumentException("Number must be positive");

        return new ProductCode(category.ToUpper(), number);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Category;
        yield return Number;
    }
}
```

## API Reference

### ValueObject (Base Class)

| Member | Description |
|--------|-------------|
| `GetAtomicValues()` | Abstract method returning values that define equality |
| `Equals(ValueObject?)` | Value-based equality comparison |
| `GetHashCode()` | Hash code based on atomic values |
| `operator ==` / `!=` | Equality operators |

### Currency

| Member | Description |
|--------|-------------|
| `Code` | ISO 4217 currency code (e.g., "USD") |
| `Symbol` | Currency symbol (e.g., "$") |
| `Create(code, symbol)` | Factory method with validation |
| `TryCreate(code, symbol, out currency)` | Safe factory method |
| `Parse(code)` | Parse from currency code |
| `TryParse(code, out currency)` | Safe parsing |
| `IsEmpty()` | Check if this is the empty instance |
| `Empty` | Singleton empty instance |
| `Default` | Default currency (PHP) |
| `USD`, `EUR`, `GBP`, `JPY`, `PHP` | Predefined instances |

### Email

| Member | Description |
|--------|-------------|
| `Address` | Full email address (normalized to lowercase) |
| `LocalPart` | Part before @ symbol |
| `Domain` | Part after @ symbol |
| `Create(address)` | Factory method with validation |
| `TryCreate(address, out email)` | Safe factory method |
| `Parse(address)` | Parse (returns Empty for invalid) |
| `TryParse(address, out email)` | Safe parsing |
| `IsEmpty()` | Check if this is the empty instance |
| `Empty` | Singleton empty instance |

### PhoneNumber

| Member | Description |
|--------|-------------|
| `CountryCode` | Country code with + prefix |
| `Number` | Phone number digits |
| `FullNumber` | Complete number (CountryCode + Number) |
| `Create(countryCode, number)` | Factory with separate parts |
| `Create(fullNumber)` | Factory with full number string |
| `TryCreate(...)` | Safe factory methods |
| `Parse(fullNumber)` | Parse (returns Empty for invalid) |
| `TryParse(fullNumber, out phone)` | Safe parsing |
| `ToFormattedString()` | Formatted display: +1 (555) 123-4567 |
| `GetValidCountryCodes()` | List of supported country codes |
| `IsEmpty()` | Check if this is the empty instance |
| `Empty` | Singleton empty instance |

### Money

| Member | Description |
|--------|-------------|
| `Amount` | Decimal amount value |
| `Currency` | Associated Currency value object |
| `Create(amount, currency)` | Factory method with validation |
| `TryCreate(amount, currency, out money)` | Safe factory method |
| `Add(other)` / `operator +` | Add two Money values |
| `Subtract(other)` / `operator -` | Subtract Money values |
| `Multiply(factor)` / `operator *` | Multiply by decimal factor |
| `Divide(divisor)` / `operator /` | Divide by decimal divisor |
| `operator >`, `<`, `>=`, `<=` | Comparison operators |
| `WithAmount(newAmount)` | Create copy with new amount |
| `WithCurrency(newCurrency)` | Create copy with new currency |
| `IsEmpty()` | Check if this is the empty instance |
| `IsZero()` | Check if amount is zero |
| `Zero` | Singleton zero instance |
| `Empty` | Singleton empty instance |

### Address

| Member | Description |
|--------|-------------|
| `Street` | Street address |
| `City` | City name |
| `State` | State/province (optional) |
| `PostalCode` | Postal/ZIP code (optional) |
| `Country` | Country name |
| `Create(street, city, state, postalCode, country)` | Full factory |
| `Create(street, city, country)` | Minimal factory |
| `TryCreate(...)` | Safe factory method |
| `WithStreet(...)`, `WithCity(...)`, etc. | Immutable update methods |
| `ToSingleLineString()` | Single line format |
| `ToMultiLineString()` | Multi-line format |
| `IsEmpty()` | Check if this is the empty instance |
| `Empty` | Singleton empty instance |

### Price

| Member | Description |
|--------|-------------|
| `StandardPrice` | Regular price amount |
| `PeakPrice` | Peak/premium price amount |
| `Currency` | Associated Currency value object |
| `Create(standard, peak, currency)` | Factory with both prices |
| `Create(standard, currency)` | Factory with uniform price |
| `TryCreate(...)` | Safe factory method |
| `WithStandardPrice(...)`, `WithPeakPrice(...)` | Immutable updates |
| `ApplyDiscount(percentage)` | Apply percentage discount |
| `ApplyMarkup(percentage)` | Apply percentage markup |
| `HasPeakPricing()` | Check if peak > standard |
| `GetPriceDifference()` | Peak minus standard |
| `GetPeakPremiumPercentage()` | Premium as percentage |
| `ToDetailedString()` | Detailed format with both prices |
| `IsEmpty()` | Check if this is the empty instance |
| `IsZero()` | Check if both prices are zero |
| `Zero` | Singleton zero instance |
| `Empty` | Singleton empty instance |

## Project Structure

```
TestNest.ValueObjects/
├── TestNest.ValueObjects.Domain/
│   ├── ValueObjects/
│   │   ├── Common/
│   │   │   └── ValueObject.cs          # Abstract base class
│   │   ├── Currency.cs                 # Currency value object
│   │   ├── Email.cs                    # Email value object
│   │   ├── PhoneNumber.cs              # Phone number value object
│   │   ├── Money.cs                    # Money value object
│   │   ├── Address.cs                  # Address value object
│   │   └── Price.cs                    # Price value object
│   └── Exceptions/
│       ├── CurrencyException.cs        # Currency domain exception
│       ├── EmailException.cs           # Email domain exception
│       ├── PhoneNumberException.cs     # Phone number domain exception
│       ├── MoneyException.cs           # Money domain exception
│       ├── AddressException.cs         # Address domain exception
│       └── PriceException.cs           # Price domain exception
├── TestNest.ValueObjects.Console/
│   └── Program.cs                      # Demo application
└── TestNest.ValueObjects.Test/
    ├── CurrencyTests.cs                # Currency unit tests
    ├── EmailTests.cs                   # Email unit tests
    ├── PhoneNumberTests.cs             # Phone number unit tests
    ├── MoneyTests.cs                   # Money unit tests
    ├── AddressTests.cs                 # Address unit tests
    └── PriceTests.cs                   # Price unit tests
```

## Why Value Objects?

| Problem | Solution |
|---------|----------|
| Primitive obsession | Encapsulates related values into a single type |
| Scattered validation | Self-validating objects ensure consistency |
| Ambiguous parameters | Strongly-typed values prevent mistakes |
| Reference equality bugs | Value-based comparison works correctly |

## Running Tests

```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Ensure all tests pass
4. Submit a pull request

## License

This project is open-source and available under the MIT License.
