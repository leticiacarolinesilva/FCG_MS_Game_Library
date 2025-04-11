using System.Text.RegularExpressions;

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
            throw new ArgumentException("Email cannot beempty.", nameof(value));
        }

        if (!IsValidEmail(value))
        {
            throw new ArgumentException("Invalid email format", nameof(value));
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
        catch (Exception e)
        {
            return false;
        }
    }
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new Email(email);
    
}