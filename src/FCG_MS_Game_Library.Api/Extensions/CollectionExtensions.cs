using FCG_MS_Game_Library.Domain.Interfaces;
using FCG_MS_Game_Library.Infra.ExternalClient;
using FCG_MS_Game_Library.Infra.ExternalClient.Interfaces;
using FCG_MS_Game_Library.Infra.Repository;

using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra.Repository;

namespace UserRegistrationAndGameLibrary.Api.Extensions
{
    public static class CollectionExtensions
    {
        public static IServiceCollection UseCollectionExtensions(this IServiceCollection services, string uri)
        {
            services.AddScoped<IGameLibraryRepository, GameLibraryRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameSearchRepository, GameSearchRepository>();

            services.AddScoped<IGameLibraryService, GameLibraryService>();
            services.AddScoped<IGameService, GameService>();

            services.AddHttpClient<IUserClient, UserClient>(client =>
            {
                client.BaseAddress = new Uri(uri);
            });

            services.AddScoped<IPaymentClient, PaymentClient>();

            return services;
        }
    }
}
