using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Newtonsoft.Json;

using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Domain.Enums;

using Xunit;

namespace UserRegistrationAndGameLibrary.IntegrationTest.Game;

public class GameControllerTests : BaseIntegrationTests
{
    private const string BaseUrl = "http://localhost:5209/api/games";

    [Fact]
    public async Task CreateGame_ShouldReturnCreated_WhenValid()
    {
        var game = new CreateGameDto
        {
            Title = "Game Teste",
            Description = "Jogo de teste",
            Price = 59.90m,
            ReleaseDate = DateTime.UtcNow,
            Genre = GameGenre.Action.ToString(),
            CoverImageUrl = "https://cdn.imagens.com/capa.jpg"
        };

        var response = await HttpClient.PostAsJsonAsync(BaseUrl, game);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<GameDto>(content);

        Assert.NotNull(created);
        Assert.Equal(game.Title, created.Title);
    }

    [Fact]
    public async Task CreateGame_ShouldReturnBadRequest_WhenGenreIsInvalid()
    {
        var game = new CreateGameDto
        {
            Title = "Game Gênero Inválido",
            Description = "Falha de gênero",
            Price = 30,
            ReleaseDate = DateTime.UtcNow,
            Genre = "Invalido",
            CoverImageUrl = "https://cdn.img.com/teste.jpg"
        };

        var response = await HttpClient.PostAsJsonAsync(BaseUrl, game);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllGames_ShouldReturnGames()
    {
        var response = await HttpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var games = JsonConvert.DeserializeObject<List<GameDto>>(content);

        Assert.NotNull(games);
        Assert.True(games.Count >= 0);
    }

    [Fact]
    public async Task GetGameById_ShouldReturnGame_WhenExists()
    {
        // cria um jogo
        var createDto = new CreateGameDto
        {
            Title = "Jogo Para Consulta",
            Description = "Descrição",
            Price = 20,
            ReleaseDate = DateTime.UtcNow,
            Genre = GameGenre.Simulation.ToString(),
            CoverImageUrl = "https://cdn.img.com/consulta.jpg"
        };

        var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto);
        createResponse.EnsureSuccessStatusCode();

        var created = JsonConvert.DeserializeObject<GameDto>(await createResponse.Content.ReadAsStringAsync());

        var response = await HttpClient.GetAsync($"{BaseUrl}/{created.Id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var game = JsonConvert.DeserializeObject<GameDto>(content);

        Assert.NotNull(game);
        Assert.Equal(created.Id, game.Id);
    }

    [Fact]
    public async Task GetGamesByGenre_ShouldReturnFilteredList()
    {
        var genre = GameGenre.Horror.ToString();

        var response = await HttpClient.GetAsync($"{BaseUrl}/genre/{genre}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var games = JsonConvert.DeserializeObject<List<GameDto>>(content);

        Assert.NotNull(games);
    }

    [Fact]
    public async Task UpdateGame_ShouldReturnNoContent_WhenSuccessful()
    {
        var createDto = new CreateGameDto
        {
            Title = "Game Para Atualizar",
            Description = "Descrição original",
            Price = 40,
            ReleaseDate = DateTime.UtcNow,
            Genre = GameGenre.Horror.ToString(),
            CoverImageUrl = "https://img.com/original.jpg"
        };

        var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto);
        createResponse.EnsureSuccessStatusCode();

        var game = JsonConvert.DeserializeObject<GameDto>(await createResponse.Content.ReadAsStringAsync());

        var updateDto = new UpdateGameDto
        {
            Title = "Título Atualizado",
            Description = "Descrição atualizada",
            Price = 45,
            Genre = GameGenre.Horror.ToString(),
            CoverImageUrl = "https://img.com/atualizada.jpg"
        };

        var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{game.Id}", updateDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGame_ShouldReturnNoContent_WhenSuccessful()
    {
        var createDto = new CreateGameDto
        {
            Title = "Jogo Para Deletar",
            Description = "Será excluído",
            Price = 10,
            ReleaseDate = DateTime.UtcNow,
            Genre = GameGenre.Sports.ToString(),
            CoverImageUrl = "https://img.com/deletar.jpg"
        };

        var createResponse = await HttpClient.PostAsJsonAsync(BaseUrl, createDto);
        createResponse.EnsureSuccessStatusCode();

        var game = JsonConvert.DeserializeObject<GameDto>(await createResponse.Content.ReadAsStringAsync());

        var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/{game.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var check = await HttpClient.GetAsync($"{BaseUrl}/{game.Id}");
        Assert.Equal(HttpStatusCode.NotFound, check.StatusCode);
    }
}
