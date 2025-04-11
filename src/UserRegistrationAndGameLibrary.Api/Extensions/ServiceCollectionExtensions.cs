using UserRegistrationAndGameLibrary.teste.Controllers;
using UserRegistrationAndGameLibrary.teste.Services;
using UserRegistrationAndGameLibrary.teste.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.teste.Extensions
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
