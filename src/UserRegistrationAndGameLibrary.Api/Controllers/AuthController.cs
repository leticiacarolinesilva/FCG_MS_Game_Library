using Microsoft.AspNetCore.Mvc;

using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

/// <summary>
/// API for authentication management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Get token for user
    /// </summary>
    /// <param name="request">user data</param>
    /// <returns>Token JWT</returns>
    [HttpPost]
    public async Task<IActionResult> GetToken([FromBody] UserDto request)
    {
        try
        {
            if (!string.IsNullOrEmpty(request?.Email!))
            {
                var response = await _authService.GetToken(request);

                return Ok(response);
            }

            return BadRequest();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
