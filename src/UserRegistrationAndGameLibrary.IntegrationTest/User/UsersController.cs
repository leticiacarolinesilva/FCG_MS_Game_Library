using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Newtonsoft.Json;

using UserRegistrationAndGameLibrary.Application.Dtos;

using Xunit;

namespace UserRegistrationAndGameLibrary.IntegrationTest.User;

public class UsersController : BaseIntegrationTests
{
    private const string BaseUrl = "http://localhost:5209/api/User";

    [Fact]
    public async Task RegisterUser_ShouldReturnCreated_WhenValid()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!",
            ConfirmationPassword = "ValidPass1!"
        };

        var response = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var location = response.Headers.Location;
        Assert.NotNull(location);

    }

    [Theory]
    [InlineData("Test User", "not-an-email", "ValidPass1!")]
    [InlineData("Test User", "test@test.com", "not-an-password")]
    [InlineData("", "test@test.com", "ValidPass1!")]
    public async Task RegisterUser_ShouldReturnBadRequest_WhenNameOrEmailOrPasswordAreInvalid(string name, string email, string password)
    {
        var invalidUser = new RegisterUserDto
        {
            Name = name,
            Email = email,
            Password = password,
            ConfirmationPassword = password
        };

        var response = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", invalidUser);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task RegisterUser_ShouldReturnBadRequest_WhenThereIsAlreadyUser()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!",
            ConfirmationPassword = "ValidPass1!"
        };

        await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        var response = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        //TODO improve the return to show Notification instead of exceptions
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    [Fact]
    public async Task RegisterUser_ShouldReturnNotFound_WhenIsNotRegisteredYet()
    {
        var email = "notexist@test.com";

        var response = await HttpClient.GetAsync($"{BaseUrl}/?email={email}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var location = response.Headers.Location;
        Assert.Null(location);
    }

    [Fact]
    public async Task RegisterUser_WhenSendEmailAndName_ShouldReturnBadRequest_WhenThereIsAlreadyUser()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!",
            ConfirmationPassword = "ValidPass1!"
        };

        var responseCreation = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        var response = await HttpClient.GetAsync($"{BaseUrl}?email={request.Email}&name={request.Name}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseCreation.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        var users =  JsonConvert.DeserializeObject<List<ResponseUserDto>>(content);

        Assert.Contains(users, x => x.Name == request.Name && x.Email == request.Email);
    }

    [Fact]
    public async Task RegisterUser_WhenSendOnlyEmail_ShouldReturnBadRequest_WhenThereIsAlreadyUser()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!",
            ConfirmationPassword = "ValidPass1!"
        };

        var responseCreation = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        var response = await HttpClient.GetAsync($"{BaseUrl}?email={request.Email}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseCreation.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<ResponseUserDto>>(content);

        Assert.Contains(users, x => x.Name == request.Name && x.Email == request.Email);
    }

    [Fact]
    public async Task RegisterUser_WhenSendOnlyName_ShouldReturnBadRequest_WhenThereIsAlreadyUser()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!",
            ConfirmationPassword = "ValidPass1!"
        };

        var responseCreation = await HttpClient.PostAsJsonAsync($"{BaseUrl}/register", request);

        var response = await HttpClient.GetAsync($"{BaseUrl}?name={request.Name}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseCreation.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<ResponseUserDto>>(content);

        Assert.Contains(users, x => x.Name == request.Name && x.Email == request.Email);
    }
}
