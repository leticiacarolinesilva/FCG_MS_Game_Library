namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class ResponseUserDto
{
    /// <summary>
    /// User's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// User's email address will be used for authentication
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
