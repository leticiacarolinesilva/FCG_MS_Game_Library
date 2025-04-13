using Microsoft.AspNetCore.Mvc;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.teste.Controllers;

/// <summary>
/// API for managing user's game library
/// </summary>
[ApiController]
[Route("api/users/[controller]")]
public class GameLibraryController: ControllerBase
{
    private readonly IUserService _userService;

    public GameLibraryController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddGameToLibraryAsync([FromRoute] Guid userId, [FromBody] Guid gameId)
    {
        try
        {
            var gameLibrary = await _userService.AddGameToLibraryAsync(userId, gameId);
            
            return Ok(gameLibrary);
            //TODO
            //return CreatedAtAction(nameof(GetGameLibraby), new { userId = userId, gameId = gameLibrary.Id }, gameLibrary);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message );
        }
    }
    

}