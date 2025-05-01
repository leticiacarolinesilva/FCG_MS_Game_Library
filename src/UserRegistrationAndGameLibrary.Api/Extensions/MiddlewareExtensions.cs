using UserRegistrationAndGameLibrary.Api.Middlewares;

namespace UserRegistrationAndGameLibrary.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMiddlewareExtensions(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<RequestLoggingMiddleware>();
        builder.UseMiddleware<ExceptionHandlingMiddleware>();

        return builder;
    }
}
