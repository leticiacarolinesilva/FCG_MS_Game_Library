using System.Collections.Specialized;

using Elasticsearch.Net;

using Nest;

namespace FCG_MS_Game_Library.Infra
{
    public static class ElasticSearchClientFactory
    {
        public static ElasticClient CreateClient(string uri, string xApiKey)
        {
            if (string.IsNullOrEmpty(uri))
                throw new ArgumentException("URI cannot be null or empty.", nameof(uri));

            if (string.IsNullOrEmpty(xApiKey))
                throw new ArgumentException("API Key cannot be null or empty.", nameof(xApiKey));

            var pool = new SingleNodeConnectionPool(new Uri(uri));

            // Headers globais (usando NameValueCollection)
            var headers = new NameValueCollection
            {
                { "Authorization", $"ApiKey {xApiKey}" }
            };

            var settings = new ConnectionSettings(pool)
                .DefaultIndex("games")
                .PrettyJson()
                .DisableDirectStreaming()
                .GlobalHeaders(headers);

            return new ElasticClient(settings);
        }
    }
}
