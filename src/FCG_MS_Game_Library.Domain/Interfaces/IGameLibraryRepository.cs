using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Domain.Interfaces;

public interface IGameLibraryRepository
{
    Task<GameLibrary?> GetByUserIdAndGameIdAsync(Guid userId, Guid gameId);
    Task<IEnumerable<GameLibrary>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<GameLibrary>> GetInstalledGamesAsync(Guid userId);
    Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId);
    Task AddAsync(GameLibrary gameLibrary);
    Task UpdateAsync(GameLibrary gameLibrary);
    Task UpdateInstallationStatusAsync(Guid gameLibraryId, bool isInstalled);
    Task RemoveFromLibraryAsync(Guid gameLibraryId);
}
