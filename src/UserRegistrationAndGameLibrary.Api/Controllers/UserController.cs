using Microsoft.AspNetCore.Mvc;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Api.Services.Interfaces;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

/// <summary>
/// API for user management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private ICorrelationIdGeneratorService _correlationIdGenerator;
    private readonly IUserService _uservice;

    public UserController(
        ICorrelationIdGeneratorService idGeneratorService,
        IUserService uservice)
    {
        _correlationIdGenerator = idGeneratorService;
        _uservice = uservice;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">ser registration data</param>
    /// <returns>The newly created user</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(request?.Name))
            {
                var user = await _uservice.RegisterUserAsync(request);
            
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);                
            }
            return BadRequest();
        }
        catch (DomainException ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromQuery] string email)
    {
        var user = await _uservice.GetUserByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }

        var response = new ResponseUserDto()
        {
            Name = user.Name,
            Email = user.Email,
        };
        return Ok(response);
    }
}
