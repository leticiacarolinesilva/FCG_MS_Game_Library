using UserRegistrationAndGameLibrary.Api.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.Api.Services;

public class CorrelationIdGeneratorService : ICorrelationIdGeneratorService
{
    private static string _requestId;

    public string Get() => _requestId;

    public void Set(string requestId) => _requestId = requestId;
}
