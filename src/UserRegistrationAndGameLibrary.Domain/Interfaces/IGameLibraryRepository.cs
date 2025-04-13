using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Domain.Interfaces;

public interface IGameLibraryRepository
{
    Task<GameLibrary?> GetByIdAsync(Guid id);
    Task<IEnumerable<GameLibrary>> GetByUserIdAsync(Guid userId);
    Task AddAsync(GameLibrary gameLibrary);
    Task UpdateAsync(GameLibrary gameLibrary);
}