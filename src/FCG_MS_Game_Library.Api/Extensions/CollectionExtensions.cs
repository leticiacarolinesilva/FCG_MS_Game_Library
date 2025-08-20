using FCG_MS_Game_Library.Infra.ExternalClient;
using FCG_MS_Game_Library.Infra.ExternalClient.Interfaces;

using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra.Repository;

namespace UserRegistrationAndGameLibrary.Api.Extensions
{
    public static class CollectionExtensions
    {
        public static IServiceCollection UseCollectionExtensions(this IServiceCollection services)
        {
            services.AddScoped<IGameLibraryRepository, GameLibraryRepository>();
            services.AddScoped<IGameRepository, GameRepository>();

            services.AddScoped<IGameLibraryService, GameLibraryService>();
            services.AddScoped<IGameService, GameService>();

            services.AddHttpClient<IUserClient, UserClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:500/api/user/");
            });

            //services.AddScoped<IUserClient, UserClient>();

            services.AddScoped<IPaymentClient, PaymentClient>();


            return services;
        }
    }
}
