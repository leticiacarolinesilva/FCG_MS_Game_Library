using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

using UserRegistrationAndGameLibrary.Application.Dtos;

using Xunit;

namespace UserRegistrationAndGameLibrary.IntegrationTest.User;

public class UsersController : BaseIntegrationTests
{
    private const string BASE_URL = "http://localhost:5209/api/User";

	[Fact]
	public async Task Can_Create_Client()
	{
    	Assert.NotNull(HttpClient); // Verify setup worked
	}
    [Fact]
    public async Task RegisterUser_ShouldReturnCreated_WhenValid()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!"
        };
        
        var response = await HttpClient.PostAsJsonAsync($"{BASE_URL}/register", request);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location;
        Assert.NotNull(location);
        
    }
    
    /*[Fact]
    public async Task RegisterUser_ShouldReturnUserAlreadyCreate_WhenThereIsAlreadyUser()
    {
        var request = new RegisterUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "ValidPass1!"
        };
        
        var response = await HttpClient.PostAsJsonAsync($"{BASE_URL}/register", request);
        
        var newResponse = await HttpClient.PostAsJsonAsync($"{BASE_URL}/register", request);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var location = response.Headers.Location;
        Assert.NotNull(location);
        
    } */   

    [Fact]
    public async Task RegisterUser_ShouldReturnNotFound_WhenIsNotRegisteredYet()
    {
        var email = "notexist@test.com";
        
        var response = await HttpClient.GetAsync($"{BASE_URL}/?email={email}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var location = response.Headers.Location;
        Assert.Null(location);
    }

}
