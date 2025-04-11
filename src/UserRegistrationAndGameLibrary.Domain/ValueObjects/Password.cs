using System.Text.RegularExpressions;

namespace UserRegistrationAndGameLibrary.Domain.ValueObjects;
/// <summary>
/// Represents a secure password with validation
/// </summary>
public sealed class Password
{
   public string HasedValue { get; }

   public Password(string value)
   {
      if (string.IsNullOrWhiteSpace(value))
      {
         throw new ArgumentException("Password cannot be empty", nameof(value));
      }

      if (!IsSecuredPassword(value))
      {
         throw new ArgumentException("Password must be at least 8 characters long and contain numbers, letters and special characters");
      }
      
      HasedValue = value;
   }

   private static bool IsSecuredPassword(string password)
   {
      if (password.Length < 8)
      {
         return false;
      }
      var hashNumber = new Regex(@"[0-9]+");
      var hasLetter = new Regex(@"[a-zA-Z]+");
      var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
      
      return hashNumber.IsMatch(password) && 
             hasLetter.IsMatch(password) && 
             hasSpecialChar.IsMatch(password);
   }
}