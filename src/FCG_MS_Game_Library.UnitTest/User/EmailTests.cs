using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.UnitTest.User;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Email_ShouldThrowException_WhenEmpty(string email)
    {
        Assert.Throws<DomainException>(() => new Email(email));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("invalid.com")]
    [InlineData("@invalid@.com")]
    public void Email_ShouldThrowException_WhenInvalidFormat(string email)
    {
        Assert.Throws<DomainException>(() => new Email(email));
    }

    [Theory]
    [InlineData("valid@example.com")]
    [InlineData("valid.name@example.com")]
    [InlineData("valid+alias@example.com")]
    public void Email_ShoulBeCreated_WhenValid(string email)
    {
        var validEmail = new Email(email);
        Assert.Equal(email.Trim().ToLower(), validEmail.Value);
    }
}