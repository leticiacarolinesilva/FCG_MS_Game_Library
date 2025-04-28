using Microsoft.AspNetCore.Mvc;

using UserRegistrationAndGameLibrary.Api.Filters;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

/// <summary>
/// API for managing user's game library
/// </summary>
[ApiController]
[Route("api/users/{userId}/library")]
public class GameLibraryController: ControllerBase
{
    private readonly GameLibraryService _gameLibraryService;

    public GameLibraryController(GameLibraryService gameLibraryService)
    {
        _gameLibraryService = gameLibraryService;
    }

    /// <summary>
    /// Get all games in user's library
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <returns>A Game tha matches the UserId and GameId</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameLibraryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> GetUserLibrary(Guid userId)
    {
        var librayData = await _gameLibraryService.GetUserLibraryAsync(userId);
        var dtos = librayData.Select(gl => new GameLibraryDto
        {
            Id = gl.Id,
            GameId = gl.GameId,
            GameTitle = gl.Game.Title,
            GameCoverImageUrl = gl.Game.CoverImageUrl,
            PurchaseDate = gl.PurchaseDate,
            PurchasePrice = gl.PurchasePrice,
            IsInstalled = gl.IsInstalled,
        });
        
        return Ok(dtos);
    }

    /// <summary>
    /// Get specific Game Library entry
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="entryId">User's information from Game Library database</param>
    /// <returns>Game library data for the userId and entryId</returns>
    [HttpGet("{entryId}")]
    [ProducesResponseType(typeof(GameLibraryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(404)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> GetGameLibrary(Guid userId, Guid entryId)
    {
        var libraryData = await _gameLibraryService.GetLibraryEntryAsync(entryId, userId);
        if (libraryData == null)
        {
            return NotFound();
        }

        var dto = new GameLibraryDto
        {
            Id = libraryData.Id,
            GameId = libraryData.GameId,
            GameTitle = libraryData.Game.Title,
            GameCoverImageUrl = libraryData.Game.CoverImageUrl,
            PurchaseDate = libraryData.PurchaseDate,
            PurchasePrice = libraryData.PurchasePrice,
            IsInstalled = libraryData.IsInstalled,
        };

        return Ok(dto);
    }    

    /// <summary>
    /// Add a game to user's library
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="gameId">GameId</param>
    /// <returns>A new game purchased</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GameLibraryDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> PurchaseGame(Guid userId, Guid gameId)
    {
        try
        {
            var librayData = await _gameLibraryService.AddGameToLibraryAsync(userId, gameId);

            var responseDto = new GameLibraryDto
            {
                Id = librayData.Id,
                GameId = librayData.GameId,
                GameTitle = librayData.Game.Title,
                GameCoverImageUrl = librayData.Game.CoverImageUrl,
                PurchaseDate = librayData.PurchaseDate,
                PurchasePrice = librayData.PurchasePrice,
                IsInstalled = librayData.IsInstalled,
            };
            
            return CreatedAtAction(nameof(GetUserLibrary), new { userId = userId, gameId = gameId }, responseDto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update game installation status
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="entryId">User's information from Game Library database</param>
    /// <param name="installationStatus">true = installed, false = no installed</param>
    /// <returns></returns>
    [HttpPatch("{entryId}/installation")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> UpdateInstallationStatus(Guid userId, Guid entryId, bool installationStatus)
    {
        try
        {
            if (installationStatus)
            {
                await _gameLibraryService.MarkAsInstalledAsync(userId, entryId);
            }
            else
            {
                await _gameLibraryService.MarkAsUninstalledAsync(userId, entryId);
            }
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("{entryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> RemoveFromLibrary(Guid userId, Guid entryId)
    {
        try
        {
            await _gameLibraryService.RemoveFromLibraryAsync(entryId, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            throw;
        }
    }
}
