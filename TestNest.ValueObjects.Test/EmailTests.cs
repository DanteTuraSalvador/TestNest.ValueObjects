using TestNest.ValueObjects.Domain.Exceptions;
using TestNest.ValueObjects.Domain.ValueObjects;
using Xunit;

namespace TestNest.ValueObjects.Domain.Tests.ValueObjects;

public class EmailTests
{
    // Happy path tests
    [Fact]
    public void Create_ValidEmail_ReturnsEmailWithCorrectValues()
    {
        var email = Email.Create("test@example.com");

        Assert.Equal("test@example.com", email.Address);
        Assert.Equal("test", email.LocalPart);
        Assert.Equal("example.com", email.Domain);
    }

    [Theory]
    [InlineData("user@domain.com")]
    [InlineData("user.name@domain.com")]
    [InlineData("user+tag@domain.com")]
    [InlineData("user@sub.domain.com")]
    public void Create_ValidEmails_ReturnsEmail(string address)
    {
        var email = Email.Create(address);
        Assert.Equal(address.ToLowerInvariant(), email.Address);
    }

    [Fact]
    public void Create_EmailWithUpperCase_NormalizesToLowerCase()
    {
        var email = Email.Create("Test@EXAMPLE.COM");

        Assert.Equal("test@example.com", email.Address);
    }

    [Fact]
    public void TryCreate_ValidEmail_ReturnsTrueAndEmail()
    {
        var success = Email.TryCreate("test@example.com", out var email);

        Assert.True(success);
        Assert.NotNull(email);
        Assert.Equal("test@example.com", email.Address);
    }

    [Fact]
    public void Parse_ValidEmail_ReturnsEmail()
    {
        var email = Email.Parse("test@example.com");

        Assert.Equal("test@example.com", email.Address);
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmpty()
    {
        var email = Email.Parse("");

        Assert.Same(Email.Empty, email);
        Assert.True(email.IsEmpty());
    }

    // Validation tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmpty_ThrowsEmailException(string? address)
    {
        var exception = Assert.Throws<EmailException>(() => Email.Create(address!));
        Assert.Equal(EmailException.ErrorCode.EmptyEmail, exception.Code);
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@")]
    [InlineData("@nodomain")]
    [InlineData("no@domain")]
    public void Create_InvalidFormat_ThrowsEmailException(string address)
    {
        var exception = Assert.Throws<EmailException>(() => Email.Create(address));
        Assert.True(
            exception.Code == EmailException.ErrorCode.InvalidFormat ||
            exception.Code == EmailException.ErrorCode.InvalidDomain
        );
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("no@tld")]
    [InlineData("")]
    [InlineData(null)]
    public void TryCreate_InvalidEmail_ReturnsFalse(string? address)
    {
        var success = Email.TryCreate(address, out var email);

        Assert.False(success);
        Assert.Null(email);
    }

    // Equality tests
    [Fact]
    public void Equals_SameAddress_ReturnsTrue()
    {
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
    }

    [Fact]
    public void Equals_DifferentCase_ReturnsTrue()
    {
        var email1 = Email.Create("Test@Example.com");
        var email2 = Email.Create("test@example.com");

        Assert.Equal(email1, email2);
    }

    [Fact]
    public void Equals_DifferentAddress_ReturnsFalse()
    {
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        Assert.NotEqual(email1, email2);
        Assert.True(email1 != email2);
    }

    [Fact]
    public void GetHashCode_SameAddress_ReturnsSameHashCode()
    {
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    // Empty instance tests
    [Fact]
    public void Empty_IsSingleton()
    {
        var empty1 = Email.Empty;
        var empty2 = Email.Empty;

        Assert.Same(empty1, empty2);
    }

    [Fact]
    public void Empty_IsEmpty_ReturnsTrue()
    {
        Assert.True(Email.Empty.IsEmpty());
    }

    [Fact]
    public void Empty_HasEmptyValues()
    {
        Assert.Equal(string.Empty, Email.Empty.Address);
        Assert.Equal(string.Empty, Email.Empty.LocalPart);
        Assert.Equal(string.Empty, Email.Empty.Domain);
    }

    // ToString tests
    [Fact]
    public void ToString_ValidEmail_ReturnsAddress()
    {
        var email = Email.Create("test@example.com");

        Assert.Equal("test@example.com", email.ToString());
    }

    [Fact]
    public void ToString_Empty_ReturnsEmptyString()
    {
        Assert.Equal("[Empty Email]", Email.Empty.ToString());
    }
}
