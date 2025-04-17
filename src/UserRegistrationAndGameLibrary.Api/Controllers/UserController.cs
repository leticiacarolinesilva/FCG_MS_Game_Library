using Microsoft.AspNetCore.Mvc;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

/// <summary>
/// API for user management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _uservice;

    public UserController(IUserService uservice)
    {
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
            var user = await _uservice.RegisterUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
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
        return Ok(user);
    }
        
    
}