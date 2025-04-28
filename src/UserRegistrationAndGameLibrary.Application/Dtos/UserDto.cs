using System.ComponentModel.DataAnnotations;

namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class UserDto
{
    [Required]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
