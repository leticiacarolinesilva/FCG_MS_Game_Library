using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Domain.Interfaces;

public interface IGameLibraryRepository
{
    Task<GameLibrary?> GetByIdAsync(Guid id);
    Task<IEnumerable<GameLibrary>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<GameLibrary>> GetInstalledGamesAsync(Guid userId);
    Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId);
    Task<GameLibrary> AddAsync(GameLibrary gameLibrary);
    Task UpdateAsync(GameLibrary gameLibrary);
    Task UpdateInstallationStatusAsync(Guid gameLibraryId, bool isInstalled);
    Task RemoveFromLibraryAsync(Guid gameLibraryId);
}