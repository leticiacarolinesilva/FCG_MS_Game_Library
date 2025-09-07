using FCG_MS_Game_Library.Application.Helpers;
using FCG_MS_Game_Library.Domain.Interfaces;

using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGameSearchRepository _gameSearchRepository;

    public GameService(
        IGameRepository gameRepository,
        IGameSearchRepository gameSearchRepository)
    {
        _gameRepository = gameRepository;
        _gameSearchRepository = gameSearchRepository;
    }

    public async Task<Game> CreateGameAsync(
        string title,
        string description,
        decimal price,
        DateTime releaseDate,
        GameGenre genre,
        string coverImageUrl)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative");

        var existing = (await _gameRepository.SearchAsync(title))
            .FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            throw new DomainException("Game with this title already exists");

        var game = new Game(title, description, price, releaseDate, genre, coverImageUrl);

        var createdGame = await _gameRepository.AddAsync(game);

        if (EnviromentHelper.IsProductionOrDevelopment())
            await _gameSearchRepository.IndexGameAsync(createdGame);

        return createdGame;
    }

    public async Task<Game?> GetGameByIdAsync(Guid id)
    {
        return await _gameRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllAsync();
    }

    public async Task UpdateGameAsync(
        Guid id,
        string title,
        string description,
        decimal price,
        GameGenre genre,
        string coverImageUrl)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
            throw new DomainException("Game not found");

        game.SetTitle(title);
        game.SetDescription(description);
        game.SetPrice(price);
        game.SetCoverImageUrl(coverImageUrl);

        await _gameRepository.UpdateAsync(game);

        if (EnviromentHelper.IsProductionOrDevelopment())
            await _gameSearchRepository.IndexGameAsync(game);
    }

    public async Task DeleteGameAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
            throw new DomainException("Game not found");

        await _gameRepository.DeleteAsync(game);

        if (EnviromentHelper.IsProductionOrDevelopment())
            await _gameSearchRepository.DeleteGameAsync(id);
    }

    public async Task<IEnumerable<Game>> GetGamesByGenreAsync(GameGenre genre)
    {
        return await _gameRepository.GetByGenreAsync(genre);
    }
}
