using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.UnitTest.User;

public class PasswordTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Password_ShouldThrowException_WhenEmpty(string password)
    {
        Assert.Throws<DomainException>(() => new Password(password));
    }
    
    [Theory]
    [InlineData("short1!")]
    [InlineData("longenoughwithoutnumbers")]
    [InlineData("1234567890")]
    [InlineData("noSpecialChars123")] 
    [InlineData("noSpecialChars123")] 
    public void Password_ShouldThrowException_WhenNotSecured(string password)
    {
        Assert.Throws<DomainException>(() => new Password(password));
    }
}