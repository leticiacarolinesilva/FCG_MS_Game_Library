using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Domain.Interfaces;

public interface IUserAuthorizationRepository
{
    Task AddAsync(UserAuthorization userAuthorization);
    Task UpdateAsync(UserAuthorization userAuthorization);
    Task<UserAuthorization?> GetByIdAsync(Guid id);
}
