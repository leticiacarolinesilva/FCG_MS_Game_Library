using Microsoft.Extensions.Primitives;
using UserRegistrationAndGameLibrary.Api.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.Api.Middlewares;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationIdHeader = "x-correlation-id";

    public CorrelationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, ICorrelationIdGeneratorService correlationIdGenerator)
    {
        var correlationId = GetCorrelationId(context, correlationIdGenerator);
        AddCorrelationIdHeaderToResponse(context, correlationId);

        await _next(context);
    }

    private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGeneratorService correlationIdGenerator)
    {
        if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
        {
            correlationIdGenerator.Set(correlationId);
            return correlationId;
        }
        else
        {
            correlationId = Guid.NewGuid().ToString();
            correlationIdGenerator.Set(correlationId);
            return correlationId;
        }
    }

    private static void AddCorrelationIdHeaderToResponse(HttpContext context, StringValues correlationId)
        => context.Response.OnStarting(() =>
   {
       context.Response.Headers[_correlationIdHeader] = new[] { correlationId.ToString() };
       return Task.CompletedTask;
   });
}
