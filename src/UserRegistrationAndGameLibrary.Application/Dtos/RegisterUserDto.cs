using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class RegisterUserDto
{
    /// <summary>
    /// User's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// User's email address will be used for authentication
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Hashed password 
    /// </summary>
    public string Password { get; set; } = string.Empty;
}