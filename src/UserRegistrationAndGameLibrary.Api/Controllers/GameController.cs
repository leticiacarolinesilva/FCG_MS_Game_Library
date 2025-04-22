using Microsoft.AspNetCore.Mvc;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Api.Controllers;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;


    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Get all available games
    /// </summary>
    /// <returns>List of all games</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();
        var dto = games.Select(g => new GameDto
        {
            Id = g.Id,
            Title = g.Title,
            Description = g.Description,
            Price = g.Price,
            ReleaseDate = g.ReleasedDate,
            Genre = g.Genre.ToString(),
            CoverImageUrl = g.CoverImageUrl,
        });
        
        return Ok(dto);
    }
    
    /// <summary>
    /// Get game by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An specific game</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        var dto = new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDate = game.ReleasedDate,
            Genre = game.Genre.ToString(),
            CoverImageUrl = game.CoverImageUrl
        };

        return Ok(dto);
    }
    
    /// <summary>
    /// Get games by genre
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    [HttpGet("genre/{genre}")]
    [ProducesResponseType(typeof(IEnumerable<GameDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGamesByGenre(string genre)
    {
        
        if (!Enum.TryParse<GameGenre>(genre, out var gameGenre))
        {
            return BadRequest("Invalid game genre");
        }

        var games = await _gameService.GetGamesByGenreAsync(gameGenre);
        var dtos = games.Select(g => new GameDto
        {
            Id = g.Id,
            Title = g.Title,
            Description = g.Description,
            Price = g.Price,
            ReleaseDate = g.ReleasedDate,
            Genre = g.Genre.ToString(),
            CoverImageUrl = g.CoverImageUrl
        });

        return Ok(dtos);
    }    
    
    /// <summary>
    /// Create a new game
    /// </summary>
    /// <param name="dto">Use's a CreateGameDto class</param>
    /// <returns>A new game created</returns>

    [HttpPost]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGame([FromBody] CreateGameDto dto)
    {
        try
        {
            if (!Enum.TryParse<GameGenre>(dto.Genre, out var genre))
            {
                return BadRequest("Invalid game genre");
            }

            var game = await _gameService.CreateGameAsync(
                dto.Title,
                dto.Description,
                dto.Price,
                dto.ReleaseDate,
                genre,
                dto.CoverImageUrl
            );

            var responseDto = new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleasedDate,
                Genre = game.Genre.ToString(),
                CoverImageUrl = game.CoverImageUrl
            };
            
            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, responseDto);


        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    /// <summary>
    /// Update an existing game
    /// </summary>
    /// <param name="id">gameid</param>
    /// <param name="dto">Use's a CreateGameDto class</param>
    /// <returns>A game was updated</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGame(Guid id, [FromBody] UpdateGameDto dto)
    {
        try
        {
            if (!Enum.TryParse<GameGenre>(dto.Genre, out var genre))
            {
                return BadRequest("Invalid game genre");
            }

            await _gameService.UpdateGameAsync(
                id,
                dto.Title,
                dto.Description,
                dto.Price,
                genre,
                dto.CoverImageUrl);

            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    ///  Delete a game
    /// </summary>
    /// <param name="id">GameId</param>
    /// <returns>A game was deleted</returns>
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        try
        {
            await _gameService.DeleteGameAsync(id);
            return NoContent();
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}