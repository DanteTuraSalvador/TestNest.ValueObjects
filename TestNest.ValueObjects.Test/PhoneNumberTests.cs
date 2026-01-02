using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class PhoneNumberTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidPhoneNumber_ReturnsPhoneNumberWithCorrectValues()
    {
        var phone = PhoneNumber.Create("+1", "5551234567");

        Assert.Equal("+1", phone.CountryCode);
        Assert.Equal("5551234567", phone.Number);
        Assert.Equal("+15551234567", phone.FullNumber);
    }

    [Theory]
    [InlineData("+1", "5551234567")]
    [InlineData("+44", "7911123456")]
    [InlineData("+63", "9171234567")]
    public void Create_ValidCountryCodesAndNumbers_ReturnsPhoneNumber(string countryCode, string number)
    {
        var phone = PhoneNumber.Create(countryCode, number);

        Assert.Equal(countryCode, phone.CountryCode);
        Assert.Equal(number, phone.Number);
    }

    [Fact]
    public void Create_FromFullNumber_ReturnsPhoneNumber()
    {
        var phone = PhoneNumber.Create("+15551234567");

        Assert.Equal("+1", phone.CountryCode);
        Assert.Equal("5551234567", phone.Number);
    }

    [Fact]
    public void TryCreate_ValidPhoneNumber_ReturnsTrueAndPhoneNumber()
    {
        var success = PhoneNumber.TryCreate("+1", "5551234567", out var phone);

        Assert.True(success);
        Assert.NotNull(phone);
        Assert.Equal("+15551234567", phone.FullNumber);
    }

    [Fact]
    public void Parse_ValidFullNumber_ReturnsPhoneNumber()
    {
        var phone = PhoneNumber.Parse("+15551234567");

        Assert.Equal("+1", phone.CountryCode);
        Assert.Equal("5551234567", phone.Number);
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmpty()
    {
        var phone = PhoneNumber.Parse("");

        Assert.Same(PhoneNumber.Empty, phone);
    }

    // Validation tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyNumber_ThrowsException(string? number)
    {
        Assert.Throws<PhoneNumberException>(() => PhoneNumber.Create("+1", number!));
    }

    [Theory]
    [InlineData("+99")]
    [InlineData("+0")]
    [InlineData("1")]
    public void Create_InvalidCountryCode_ThrowsException(string countryCode)
    {
        Assert.Throws<PhoneNumberException>(() => PhoneNumber.Create(countryCode, "5551234567"));
    }

    [Theory]
    [InlineData("123")]  // too short
    [InlineData("12")]   // too short
    public void Create_InvalidNumber_ThrowsException(string number)
    {
        Assert.Throws<PhoneNumberException>(() => PhoneNumber.Create("+1", number));
    }

    [Fact]
    public void TryCreate_InvalidCountryCode_ReturnsFalse()
    {
        var success = PhoneNumber.TryCreate("+99", "5551234567", out var phone);

        Assert.False(success);
        Assert.Null(phone);
    }

    // Equality tests
    [Fact]
    public void Equals_SamePhoneNumber_ReturnsTrue()
    {
        var phone1 = PhoneNumber.Create("+1", "5551234567");
        var phone2 = PhoneNumber.Create("+1", "5551234567");

        Assert.Equal(phone1, phone2);
        Assert.True(phone1 == phone2);
    }

    [Fact]
    public void Equals_DifferentPhoneNumber_ReturnsFalse()
    {
        var phone1 = PhoneNumber.Create("+1", "5551234567");
        var phone2 = PhoneNumber.Create("+1", "5559876543");

        Assert.NotEqual(phone1, phone2);
        Assert.True(phone1 != phone2);
    }

    [Fact]
    public void Equals_DifferentCountryCode_ReturnsFalse()
    {
        var phone1 = PhoneNumber.Create("+1", "5551234567");
        var phone2 = PhoneNumber.Create("+44", "5551234567");

        Assert.NotEqual(phone1, phone2);
    }

    [Fact]
    public void GetHashCode_SamePhoneNumber_ReturnsSameHashCode()
    {
        var phone1 = PhoneNumber.Create("+1", "5551234567");
        var phone2 = PhoneNumber.Create("+1", "5551234567");

        Assert.Equal(phone1.GetHashCode(), phone2.GetHashCode());
    }

    // Empty instance tests
    [Fact]
    public void Empty_IsSingleton()
    {
        var empty1 = PhoneNumber.Empty;
        var empty2 = PhoneNumber.Empty;

        Assert.Same(empty1, empty2);
    }

    [Fact]
    public void Empty_IsEmpty_ReturnsTrue()
    {
        Assert.True(PhoneNumber.Empty.IsEmpty());
    }

    // ToString tests
    [Fact]
    public void ToString_ValidPhoneNumber_ReturnsFullNumber()
    {
        var phone = PhoneNumber.Create("+1", "5551234567");

        Assert.Equal("+15551234567", phone.ToString());
    }

    [Fact]
    public void ToFormattedString_ValidPhoneNumber_ReturnsFormattedNumber()
    {
        var phone = PhoneNumber.Create("+1", "5551234567");
        var formatted = phone.ToFormattedString();

        Assert.Equal("+1 (555) 123-4567", formatted);
    }

    // Valid country codes tests
    [Fact]
    public void GetValidCountryCodes_ReturnsExpectedCodes()
    {
        var codes = PhoneNumber.GetValidCountryCodes();

        Assert.Contains("+1", codes);
        Assert.Contains("+44", codes);
        Assert.Contains("+63", codes);
    }
}
