using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Application.Interfaces;

public interface IGameLibraryService
{
    Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId);
    Task<IEnumerable<GameLibrary>> GetUserLibraryAsync(Guid userId);
    Task<GameLibrary?> GetLibraryEntryAsync(Guid userId, Guid gameId);
    Task MarkAsInstalledAsync(Guid userId, Guid gameId);
    Task MarkAsUninstalledAsync(Guid userId, Guid gameId);
    Task RemoveFromLibraryAsync(Guid userId, Guid gameId);
}
