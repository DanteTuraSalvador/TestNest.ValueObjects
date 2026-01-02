using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class AddressTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidAddress_ReturnsAddressWithCorrectValues()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("10001", address.PostalCode);
        Assert.Equal("USA", address.Country);
    }

    [Fact]
    public void Create_MinimalAddress_ReturnsAddress()
    {
        var address = Address.Create("123 Main St", "Manila", "Philippines");

        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("Manila", address.City);
        Assert.Equal("Philippines", address.Country);
        Assert.Equal(string.Empty, address.State);
        Assert.Equal(string.Empty, address.PostalCode);
    }

    [Fact]
    public void TryCreate_ValidAddress_ReturnsTrueAndAddress()
    {
        var success = Address.TryCreate("123 Main St", "New York", "NY", "10001", "USA", out var address);

        Assert.True(success);
        Assert.NotNull(address);
        Assert.Equal("123 Main St", address.Street);
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        var address = Address.Create("  123 Main St  ", "  New York  ", "  NY  ", "  10001  ", "  USA  ");

        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("10001", address.PostalCode);
        Assert.Equal("USA", address.Country);
    }

    // Validation tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyStreet_ThrowsAddressException(string? street)
    {
        var exception = Assert.Throws<AddressException>(() =>
            Address.Create(street!, "New York", "NY", "10001", "USA"));
        Assert.Equal(AddressException.ErrorCode.EmptyStreet, exception.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyCity_ThrowsAddressException(string? city)
    {
        var exception = Assert.Throws<AddressException>(() =>
            Address.Create("123 Main St", city!, "NY", "10001", "USA"));
        Assert.Equal(AddressException.ErrorCode.EmptyCity, exception.Code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_EmptyCountry_ThrowsAddressException(string? country)
    {
        var exception = Assert.Throws<AddressException>(() =>
            Address.Create("123 Main St", "New York", "NY", "10001", country!));
        Assert.Equal(AddressException.ErrorCode.EmptyCountry, exception.Code);
    }

    [Fact]
    public void TryCreate_EmptyStreet_ReturnsFalse()
    {
        var success = Address.TryCreate("", "New York", "NY", "10001", "USA", out var address);

        Assert.False(success);
        Assert.Null(address);
    }

    // Equality tests
    [Fact]
    public void Equals_SameAddress_ReturnsTrue()
    {
        var address1 = Address.Create("123 Main St", "New York", "NY", "10001", "USA");
        var address2 = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        Assert.Equal(address1, address2);
        Assert.True(address1 == address2);
    }

    [Fact]
    public void Equals_DifferentStreet_ReturnsFalse()
    {
        var address1 = Address.Create("123 Main St", "New York", "NY", "10001", "USA");
        var address2 = Address.Create("456 Oak Ave", "New York", "NY", "10001", "USA");

        Assert.NotEqual(address1, address2);
        Assert.True(address1 != address2);
    }

    [Fact]
    public void GetHashCode_SameAddress_ReturnsSameHashCode()
    {
        var address1 = Address.Create("123 Main St", "New York", "NY", "10001", "USA");
        var address2 = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
    }

    // With* methods tests
    [Fact]
    public void WithStreet_ReturnsNewAddressWithUpdatedStreet()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var updated = address.WithStreet("456 Oak Ave");

        Assert.Equal("456 Oak Ave", updated.Street);
        Assert.Equal("New York", updated.City);
        Assert.Equal("123 Main St", address.Street); // Original unchanged
    }

    [Fact]
    public void WithCity_ReturnsNewAddressWithUpdatedCity()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var updated = address.WithCity("Los Angeles");

        Assert.Equal("Los Angeles", updated.City);
    }

    [Fact]
    public void WithState_ReturnsNewAddressWithUpdatedState()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var updated = address.WithState("CA");

        Assert.Equal("CA", updated.State);
    }

    [Fact]
    public void WithPostalCode_ReturnsNewAddressWithUpdatedPostalCode()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var updated = address.WithPostalCode("90210");

        Assert.Equal("90210", updated.PostalCode);
    }

    [Fact]
    public void WithCountry_ReturnsNewAddressWithUpdatedCountry()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var updated = address.WithCountry("Canada");

        Assert.Equal("Canada", updated.Country);
    }

    // Empty instance tests
    [Fact]
    public void Empty_IsSingleton()
    {
        var empty1 = Address.Empty;
        var empty2 = Address.Empty;

        Assert.Same(empty1, empty2);
    }

    [Fact]
    public void Empty_IsEmpty_ReturnsTrue()
    {
        Assert.True(Address.Empty.IsEmpty());
    }

    [Fact]
    public void Empty_HasEmptyValues()
    {
        Assert.Equal(string.Empty, Address.Empty.Street);
        Assert.Equal(string.Empty, Address.Empty.City);
        Assert.Equal(string.Empty, Address.Empty.State);
        Assert.Equal(string.Empty, Address.Empty.PostalCode);
        Assert.Equal(string.Empty, Address.Empty.Country);
    }

    // ToString tests
    [Fact]
    public void ToString_ValidAddress_ReturnsSingleLineFormat()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        Assert.Equal("123 Main St, New York, NY, 10001, USA", address.ToString());
    }

    [Fact]
    public void ToSingleLineString_ValidAddress_ReturnsSingleLineFormat()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        Assert.Equal("123 Main St, New York, NY, 10001, USA", address.ToSingleLineString());
    }

    [Fact]
    public void ToMultiLineString_ValidAddress_ReturnsMultiLineFormat()
    {
        var address = Address.Create("123 Main St", "New York", "NY", "10001", "USA");

        var expected = $"123 Main St{Environment.NewLine}New York, NY 10001{Environment.NewLine}USA";
        Assert.Equal(expected, address.ToMultiLineString());
    }

    [Fact]
    public void ToString_Empty_ReturnsEmptyString()
    {
        Assert.Equal("[Empty Address]", Address.Empty.ToString());
    }
}
