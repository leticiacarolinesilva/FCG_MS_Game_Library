using UserRegistrationAndGameLibrary.Api.Controllers;
using UserRegistrationAndGameLibrary.Api.Services;
using UserRegistrationAndGameLibrary.Api.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseServiceCollectionExtensions(this IServiceCollection services)
        {
            services.AddTransient<ICorrelationIdGeneratorService, CorrelationIdGeneratorService>();

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<UserController>>();
            services.AddSingleton(typeof(ILogger), logger ?? throw new InvalidOperationException(nameof(logger)));

            return services;
        }
    }
}
