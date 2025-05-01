using UserRegistrationAndGameLibrary.Domain.Enums;

namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class UserAuthorizationDto
{
    public required Guid UserId { get; set; }
    public required AuthorizationPermissions Permission { get; set; }
}
