using FCG_MS_Game_Library.Infra.ExternalClient.Interfaces;

using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class GameLibraryService : IGameLibraryService
{
    private readonly IGameLibraryRepository _gameLibraryRepository;
    private readonly IUserClient _userClient;
    private readonly IGameRepository _gameRepository;

    public GameLibraryService(IGameLibraryRepository gameLibraryRepository,
        IUserClient userClient,
        IGameRepository gameRepository)
    {
        _gameLibraryRepository = gameLibraryRepository;
        _userClient = userClient;
        _gameRepository = gameRepository;
    }
    public async Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId)
    {
        var user = await _userClient.GetByIdAsync(userId)
                   ?? throw new DomainException("User not found");

        var game = await _gameRepository.GetByIdAsync(gameId)
                   ?? throw new DomainException("Game not found");

        if (await _gameLibraryRepository.UserOwnsGameAsync(userId, gameId))
            throw new DomainException("User already owns this game");

        var gameLibrary = new GameLibrary(user.Id, game.Id, game.Price);

        await _gameLibraryRepository.AddAsync(gameLibrary);

        var gameLibraryNew = await _gameLibraryRepository.GetByUserIdAndGameIdAsync(user.Id, gameId);

        if (gameLibraryNew is null)
        {
            throw new DomainException("An error occurred when linking game to user ");
        }

        return gameLibraryNew;
    }

    public async Task<IEnumerable<GameLibrary>> GetUserLibraryAsync(Guid userId)
    {
        return await _gameLibraryRepository.GetByUserIdAsync(userId);
    }

    public async Task<GameLibrary?> GetLibraryEntryAsync(Guid userId, Guid gameId)
    {
        var entry = await _gameLibraryRepository.GetByUserIdAndGameIdAsync(userId, gameId);
        return entry?.UserId == userId ? entry : null;
    }

    public async Task MarkAsInstalledAsync(Guid userId, Guid gameId)
    {
        var entry = await GetLibraryEntryAsync(userId, gameId);
        if (entry == null) throw new DomainException("Library entry not found");
        entry.MarkAsInstalled();
        await _gameLibraryRepository.UpdateAsync(entry);
    }

    public async Task MarkAsUninstalledAsync(Guid userId, Guid gameId)
    {
        var entry = await GetLibraryEntryAsync(userId, gameId);
        if (entry == null) throw new DomainException("Library entry not found");
        entry.MarkAsUninstalled();
        await _gameLibraryRepository.UpdateAsync(entry);
    }

    public async Task RemoveFromLibraryAsync(Guid userId, Guid gameId)
    {
        var entry = await GetLibraryEntryAsync(userId, gameId);
        if (entry == null) throw new DomainException("Library entry not found");
        await _gameLibraryRepository.RemoveFromLibraryAsync(entry.Id);
    }

}
