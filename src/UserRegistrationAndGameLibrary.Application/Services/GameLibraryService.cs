using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class GameLibraryService
{
    private readonly IGameLibraryRepository _gameLibraryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;

    public GameLibraryService(IGameLibraryRepository gameLibraryRepository, IUserRepository userRepository, IGameRepository gameRepository)
    {
        _gameLibraryRepository = gameLibraryRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
    }
    public async Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId)
    {
        var user = await _userRepository.GetByIdAsync(userId) 
                   ?? throw new DomainException("User not found");
            
        var game = await _gameRepository.GetByIdAsync(gameId) 
                   ?? throw new DomainException("Game not found");

        if (await _gameLibraryRepository.UserOwnsGameAsync(userId, gameId))
            throw new DomainException("User already owns this game");

        var gameLibrary = new GameLibrary(user, game, game.Price);
        return await _gameLibraryRepository.AddAsync(gameLibrary);
    }

    public async Task<IEnumerable<GameLibrary>> GetUserLibraryAsync(Guid userId)
    {
        return await _gameLibraryRepository.GetByUserIdAsync(userId);
    }

    public async Task<GameLibrary?> GetLibraryEntryAsync(Guid entryId, Guid userId)
    {
        var entry = await _gameLibraryRepository.GetByIdAsync(entryId);
        return entry?.UserId == userId ? entry : null;
    }

    public async Task MarkAsInstalledAsync(Guid entryId, Guid userId)
    {
        var entry = await GetLibraryEntryAsync(entryId, userId);
        if (entry == null) throw new DomainException("Library entry not found");
        entry.MarkAsInstalled();
        await _gameLibraryRepository.UpdateAsync(entry);
    }
 
    public async Task MarkAsUninstalledAsync(Guid entryId, Guid userId)
    {
        var entry = await GetLibraryEntryAsync(entryId, userId);
        if (entry == null) throw new DomainException("Library entry not found");
        entry.MarkAsUninstalled();
        await _gameLibraryRepository.UpdateAsync(entry);
    }

    public async Task RemoveFromLibraryAsync(Guid entryId, Guid userId)
    {
        var entry = await GetLibraryEntryAsync(entryId, userId);
        if (entry == null) throw new DomainException("Library entry not found");
        await _gameLibraryRepository.RemoveFromLibraryAsync(entryId);
    }
    
}