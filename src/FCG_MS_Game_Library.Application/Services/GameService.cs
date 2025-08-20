using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    public async Task<Game> CreateGameAsync(
        string title, 
        string description, 
        decimal price, 
        DateTime releaseDate, 
        GameGenre genre, 
        string coverImageUrl)
    {
        // Validate price
        if (price < 0)
            throw new DomainException("Price cannot be negative");

        // Check if game with same title exists
        var existing = (await _gameRepository.SearchAsync(title))
            .FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            throw new DomainException("Game with this title already exists");

        var game = new Game(title, description, price, releaseDate, genre, coverImageUrl);
        return await _gameRepository.AddAsync(game);
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
        {
            throw new DomainException("Game not found");
        }

        game.SetTitle(title);
        game.SetDescription(description);
        game.SetPrice(price);
        //TODO
        //game.Genre = genre;
        game.SetCoverImageUrl(coverImageUrl);

        await _gameRepository.UpdateAsync(game);
    }

    public async Task DeleteGameAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            throw new DomainException("Game not found");
        }

        await _gameRepository.DeleteAsync(game);
    }

    public async Task<IEnumerable<Game>> GetGamesByGenreAsync(GameGenre genre)
    {
        return await _gameRepository.GetByGenreAsync(genre);
    }
}
