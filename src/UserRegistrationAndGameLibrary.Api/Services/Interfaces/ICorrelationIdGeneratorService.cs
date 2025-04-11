namespace UserRegistrationAndGameLibrary.teste.Services.Interfaces;

public interface ICorrelationIdGeneratorService
{
    string Get();
    void Set(string requestId);
}
