using UserRegistrationAndGameLibrary.Api.Middlewares;

namespace UserRegistrationAndGameLibrary.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewareExtensions(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }
}
