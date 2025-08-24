using FCG_MS_Game_Library.Domain.Interfaces;

using Nest;

using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace FCG_MS_Game_Library.Infra.Repository;

public class GameSearchRepository : IGameSearchRepository
{
    private readonly IElasticClient _elasticClient;

    public GameSearchRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task IndexGameAsync(Game game)
    {
        var response = await _elasticClient.IndexDocumentAsync(game);

        if (!response.IsValid)
        {
            throw new DomainException($"Erro ao indexar jogo: {response.DebugInformation}");
        }
    }

    public async Task<IReadOnlyCollection<Game>> SearchByTitleAsync(string title)
    {
        var response = await _elasticClient.SearchAsync<Game>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Title)
                    .Query(title)
                )
            )
        );

        return response.Documents;
    }

    public async Task<IReadOnlyCollection<Game>> SearchByGenreAsync(string genre)
    {
        var response = await _elasticClient.SearchAsync<Game>(s => s
            .Query(q => q
                .Term(t => t.Genre, genre)
            )
        );

        return response.Documents;
    }

    public async Task<object> GetPriceStatisticsAsync()
    {
        var response = await _elasticClient.SearchAsync<Game>(s => s
            .Aggregations(a => a
                .Stats("price_stats", st => st.Field(f => f.Price))
            )
        );

        return response.Aggregations.Stats("price_stats");
    }

    public async Task DeleteGameAsync(Guid id)
    {
        await _elasticClient.DeleteAsync<Game>(id);
    }
}
