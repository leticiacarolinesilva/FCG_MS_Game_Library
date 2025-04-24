using System.Text.RegularExpressions;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Domain.ValueObjects;

/// <summary>
/// Represents an email address with validation
/// </summary>
public class Email
{
    public string Value { get;}
    
    public Email(string value)
    {
        Value = value;
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException("Email cannot beempty.");
        }

        if (!IsValidEmail(value))
        {
            throw new DomainException("Invalid email format");
        }
        
        Value = value.Trim().ToLower();
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new Email(email);
    
}