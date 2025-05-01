using UserRegistrationAndGameLibrary.Domain.Enums;

namespace UserRegistrationAndGameLibrary.Domain.Entities;
public class UserAuthorization
{

    public Guid Id { get; private set; }
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Permissions that the user has access to
    /// </summary>
    public AuthorizationPermissions Permission { get; private set; }

    public User User { get; private set; } 

    public UserAuthorization(Guid userId, AuthorizationPermissions permission)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Permission = permission;
    }
}
