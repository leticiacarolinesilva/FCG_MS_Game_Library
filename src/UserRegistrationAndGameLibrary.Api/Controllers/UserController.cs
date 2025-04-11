using Microsoft.AspNetCore.Mvc;
using UserRegistrationAndGameLibrary.teste.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.teste.Controllers;

/// <summary>
/// Handles HTTP requests related to user operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private ICorrelationIdGeneratorService _correlationIdGenerator;
    public ILogger Logger;

    public UserController(
        ICorrelationIdGeneratorService idGeneratorService,
        ILogger logger)
    {
        _correlationIdGenerator = idGeneratorService ?? throw new InvalidOperationException(nameof(idGeneratorService));
        Logger = logger ?? throw new InvalidOperationException(nameof(logger));
    }
  
    /// <summary>
    /// User registration.
    /// </summary>
    /// <returns> </returns>
    [HttpPost]
    public IActionResult UserRegistration([FromBody] string request)
    {
        Logger.LogInformation($"Start process user registration.");

        string result = string.Empty;

        Logger.LogInformation($"Fished process user registration.");

        return Ok(new { msg = result });
    }
}
