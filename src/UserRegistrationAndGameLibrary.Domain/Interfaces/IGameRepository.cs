using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Domain.Interfaces;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(Guid id);
    Task<IEnumerable<Game>> GetAllAsync();
    Task AddAsync(Game game);
}