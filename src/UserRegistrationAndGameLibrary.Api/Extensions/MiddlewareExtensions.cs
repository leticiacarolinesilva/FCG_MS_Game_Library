using UserRegistrationAndGameLibrary.teste.Middlewares;

namespace UserRegistrationAndGameLibrary.teste.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewareExtensions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }
}
