using FCG_MS_Game_Library.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;

using UserRegistrationAndGameLibrary.Api.Filters;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

public class GameSearchController : ControllerBase
{
    private readonly IGameSearchRepository _gameSearchRepository;

    public GameSearchController(IGameSearchRepository gameSearchRepository)
    {
        _gameSearchRepository = gameSearchRepository;
    }

    /// <summary>
    /// Create index for game, requires an Admin token
    /// </summary>
    /// <returns>Jogo indexado com sucesso!</returns>
    [HttpPost("index")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin)]
    public async Task<IActionResult> IndexGame([FromBody] CreateGameDto gameDto)
    {
        if (!Enum.TryParse<GameGenre>(gameDto.Genre, out var genre))
        {
            return BadRequest("Invalid game genre");
        }

        var game = new Game(
            gameDto.Title,
            gameDto.Description,
            gameDto.Price,
            gameDto.ReleaseDate,
            genre,
            gameDto.CoverImageUrl);

        await _gameSearchRepository.IndexGameAsync(game);

        return Ok("Jogo indexado com sucesso!");
    }

    /// <summary>
    /// Get all available games by title, requires an Admin or User token
    /// </summary>
    /// <returns>List of all games by title</returns>
    [HttpGet("search/title")]
    [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> SearchByTitle([FromQuery] string title)
    {
        var results = await _gameSearchRepository.SearchByTitleAsync(title);
        return Ok(results);
    }

    /// <summary>
    /// Get all available games by genre, requires an Admin or User token
    /// </summary>
    /// <returns>List of all games by genre</returns>
    [HttpGet("search/genre")]
    [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> SearchByGenre([FromQuery] string genre)
    {
        var results = await _gameSearchRepository.SearchByGenreAsync(genre);
        return Ok(results);
    }

    /// <summary>
    /// Metrics by price
    /// </summary>
    /// <returns>Return metrics by price</returns>
    [HttpGet("stats/prices")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [UserAuthorizeAtribute(AuthorizationPermissions.Admin, AuthorizationPermissions.User)]
    public async Task<IActionResult> GetPriceStats()
    {
        var stats = await _gameSearchRepository.GetPriceStatisticsAsync();
        return Ok(stats);
    }
}
