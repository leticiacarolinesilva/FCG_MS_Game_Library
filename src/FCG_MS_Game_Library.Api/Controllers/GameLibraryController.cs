using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
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
    private readonly IGameLibraryService _gameLibraryService;

    public GameLibraryController(IGameLibraryService gameLibraryService)
    {
        _gameLibraryService = gameLibraryService;
    }

    /// <summary>
    /// Get all games in user's library, requires an Admin Token
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <returns>A Game tha matches the UserId and GameId</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameLibraryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
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
    /// Get specific Game Library entry, requires an Admin or User Token
    /// </summary>
    /// <param name="userId">User Id</param>
    /// <param name="gameId">User's information from Game Library database</param>
    /// <returns>Game library data for the userId and entryId</returns>
    [HttpGet("{gameId}")]
    [ProducesResponseType(typeof(GameLibraryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> GetGameLibrary(Guid userId, Guid gameId)
    {
        var libraryData = await _gameLibraryService.GetLibraryEntryAsync(userId, gameId);
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
    /// Add a game to user's library, requires an Admin or User Token
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="gameId">GameId</param>
    /// <returns>A new game purchased</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GameLibraryDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize]
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
    /// Update game installation status, requires an Admin or User Token
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="gameId">GameId register for a specific user from Game Library database</param>
    /// <param name="installationStatus">true = installed, false = no installed</param>
    /// <returns></returns>
    [HttpPatch("{gameId}/installation")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> UpdateInstallationStatus(Guid userId, Guid gameId, bool installationStatus)
    {
        try
        {
            if (installationStatus)
            {
                await _gameLibraryService.MarkAsInstalledAsync(userId, gameId);
            }
            else
            {
                await _gameLibraryService.MarkAsUninstalledAsync(userId, gameId);
            }
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Delete a record from Game Library database by userid and gameid, requires an Admin Token
    /// </summary>
    /// <param name="userId">UserId</param>
    /// <param name="gameId">GameId register for a specific user from Game Library database</param>
    /// <returns>A no content response</returns>
    [HttpDelete("{entryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> RemoveFromLibrary(Guid userId, Guid gameId)
    {
        try
        {
            await _gameLibraryService.RemoveFromLibraryAsync(userId, gameId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            throw;
        }
    }
}
