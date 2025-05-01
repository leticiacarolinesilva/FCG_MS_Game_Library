using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra.Repository;

namespace UserRegistrationAndGameLibrary.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseServiceCollectionExtensions(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IUserAuthorizationRepository, UserAuthorizationRepository>();
            services.AddScoped<IGameLibraryRepository, GameLibraryRepository>();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

            //var serviceProvider = services.BuildServiceProvider();
            //var logger = serviceProvider.GetService<ILogger<UserController>>();
            //services.AddSingleton(typeof(ILogger), logger ?? throw new InvalidOperationException(nameof(logger)));

            return services;
        }
    }
}
