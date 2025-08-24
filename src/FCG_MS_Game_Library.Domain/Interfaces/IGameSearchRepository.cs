using UserRegistrationAndGameLibrary.Domain.Entities;

namespace FCG_MS_Game_Library.Domain.Interfaces;

public interface IGameSearchRepository
{
    Task IndexGameAsync(Game game);
    Task<IReadOnlyCollection<Game>> SearchByTitleAsync(string title);
    Task<IReadOnlyCollection<Game>> SearchByGenreAsync(string genre);
    Task<object> GetPriceStatisticsAsync();
}
