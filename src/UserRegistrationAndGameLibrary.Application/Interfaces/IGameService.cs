using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;

namespace UserRegistrationAndGameLibrary.Application.Interfaces;

public interface IGameService
{
    Task<Game> CreateGameAsync(string title, string description, decimal price, 
        DateTime releaseDate, GameGenre genre, string coverImageUrl);
    Task<Game?> GetGameByIdAsync(Guid id);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<IEnumerable<Game>> GetGamesByGenreAsync(GameGenre genre);
    Task UpdateGameAsync(Guid id, string title, string description, 
        decimal price, GameGenre genre, string coverImageUrl);
    Task DeleteGameAsync(Guid id);
}
