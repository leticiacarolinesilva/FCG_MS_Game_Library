using Nest;

using UserRegistrationAndGameLibrary.Domain.Entities;

namespace FCG_MS_Game_Library.Infra;

public static class ElasticSearchClientFactory
{
    public static ElasticClient CreateClient(string uri, string? username, string? password)
    {
        if (string.IsNullOrEmpty(uri))
        {
            throw new ArgumentException("URI cannot be null or empty.", nameof(uri));
        }

        var settings = new ConnectionSettings(new Uri(uri))
            .DefaultIndex("games")
                .PrettyJson()
                .DisableDirectStreaming()
                .EnableApiVersioningHeader()
                .DefaultMappingFor<Game>(m => m
                    .IdProperty(g => g.Id)
                    .PropertyName(g => g.Title, "title")
                    .PropertyName(g => g.Description, "description")
                    .PropertyName(g => g.Price, "price")
                    .PropertyName(g => g.ReleasedDate, "releasedDate")
                    .PropertyName(g => g.Genre, "genre")
                    .PropertyName(g => g.CoverImageUrl, "coverImageUrl")
        );

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            settings = settings.BasicAuthentication(username, password);
        }

        return new ElasticClient(settings);
    }
}
