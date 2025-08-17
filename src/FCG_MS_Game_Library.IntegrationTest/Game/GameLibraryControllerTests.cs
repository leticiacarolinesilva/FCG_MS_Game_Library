//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;

//using Newtonsoft.Json;

//using UserRegistrationAndGameLibrary.Application.Dtos;

//using Xunit;

//namespace UserRegistrationAndGameLibrary.IntegrationTest.GameLibrary;

//public class GameLibraryControllerTests : BaseIntegrationTests
//{
//    private const string BaseUrl = "http://localhost:5209/api";

//    [Fact]
//    public async Task GetUserLibrary_ShouldReturnLibrary_WhenExists()
//    {
//        var (userId, gameId) = await SetupUserAndGameAsync();
//        await HttpClient.PostAsync($"{BaseUrl}/users/{userId}/library?gameId={gameId}", null);

//        var response = await HttpClient.GetAsync($"{BaseUrl}/users/{userId}/library");

//        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        var content = await response.Content.ReadAsStringAsync();
//        var library = JsonConvert.DeserializeObject<List<GameLibraryDto>>(content);

//        Assert.NotNull(library);
//        Assert.Single(library);
//        Assert.Equal(gameId, library[0].GameId);
//    }

//    [Fact]
//    public async Task GetGameLibrary_ShouldReturnEntry_WhenExists()
//    {
//        var (userId, gameId) = await SetupUserAndGameAsync();
//        await HttpClient.PostAsync($"{BaseUrl}/users/{userId}/library?gameId={gameId}", null);

//        var response = await HttpClient.GetAsync($"{BaseUrl}/users/{userId}/library/{gameId}");

//        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

//        var content = await response.Content.ReadAsStringAsync();
//        var entry = JsonConvert.DeserializeObject<GameLibraryDto>(content);

//        Assert.NotNull(entry);
//        Assert.Equal(gameId, entry!.GameId);
//    }

//    [Fact]
//    public async Task UpdateInstallationStatus_ShouldUpdate_WhenInstalled()
//    {
//        var (userId, gameId) = await SetupUserAndGameAsync();
//        await HttpClient.PostAsync($"{BaseUrl}/users/{userId}/library?gameId={gameId}", null);

//        var response = await HttpClient.PatchAsync($"{BaseUrl}/users/{userId}/library/{gameId}/installation?installationStatus=true", null);

//        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//    }

//    [Fact]
//    public async Task UpdateInstallationStatus_ShouldUpdate_WhenUninstalled()
//    {
//        var (userId, gameId) = await SetupUserAndGameAsync();
//        await HttpClient.PostAsync($"{BaseUrl}/users/{userId}/library?gameId={gameId}", null);

//        // instala
//        await HttpClient.PatchAsync($"{BaseUrl}/users/{userId}/library/{gameId}/installation?installationStatus=true", null);

//        // desinstala
//        var response = await HttpClient.PatchAsync($"{BaseUrl}/users/{userId}/library/{gameId}/installation?installationStatus=false", null);

//        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//    }

//    [Fact]
//    public async Task RemoveFromLibrary_ShouldReturnNoContent_WhenValid()
//    {
//        var (userId, gameId) = await SetupUserAndGameAsync();
//        await HttpClient.PostAsync($"{BaseUrl}/users/{userId}/library?gameId={gameId}", null);

//        var response = await HttpClient.DeleteAsync($"{BaseUrl}/users/{userId}/library/{Guid.NewGuid()}?gameId={gameId}");

//        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

//        var getResponse = await HttpClient.GetAsync($"{BaseUrl}/users/{userId}/library/{gameId}");
//        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
//    }

//    //private async Task<(Guid userId, Guid gameId)> SetupUserAndGameAsync()
//    //{
//    //    // Cria usuário
//    //    var user = new RegisterUserDto
//    //    {
//    //        Name = "Usuário Lib",
//    //        Email = Guid.NewGuid() + "@lib.com",
//    //        Password = "Senha123!",
//    //        ConfirmationPassword = "Senha123!"
//    //    };

//    //    await HttpClient.PostAsJsonAsync($"{BaseUrl}/user/register", user);

//    //    var userResponse = await HttpClient.GetAsync($"{BaseUrl}/user?email={user.Email}");
//    //    var userContent = await userResponse.Content.ReadAsStringAsync();
//    //    var users = JsonConvert.DeserializeObject<List<ResponseUserDto>>(userContent);
//    //    var userId = users.FirstOrDefault().Id;

//    //    // Cria jogo
//    //    var game = new CreateGameDto
//    //    {
//    //        Title = $"Jogo Teste",
//    //        Description = "Jogo Teste",
//    //        Price = 39.99m,
//    //        ReleaseDate = DateTime.UtcNow,
//    //        Genre = GameGenre.Horror.ToString(),
//    //        CoverImageUrl = "https://cdn.img.com/teste.jpg"
//    //    };

//    //    var gameResponse = await HttpClient.PostAsJsonAsync($"{BaseUrl}/games", game);
//    //    var gameContent = await gameResponse.Content.ReadAsStringAsync();
//    //    var gameDto = JsonConvert.DeserializeObject<GameDto>(gameContent);
//    //    var gameId = gameDto!.Id;

//    //    return (userId, gameId);
//    //}
//}
