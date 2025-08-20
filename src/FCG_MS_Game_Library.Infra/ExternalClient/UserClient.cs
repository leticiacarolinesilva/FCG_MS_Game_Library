using System.Net.Http.Json;

using FCG_MS_Game_Library.Infra.ExternalClient.Interfaces;
using FCG_MS_Game_Library.Infra.ExternalClient.Responses;

using Microsoft.AspNetCore.Http;

using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace FCG_MS_Game_Library.Infra.ExternalClient;

public class UserClient : IUserClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public UserClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;

        GetJwtToken();
    }

    public async Task<UserResponse> GetByIdAsync(Guid userId)
    {
        var response = await _httpClient.GetAsync($"id?Id={userId}");

        var responseUser = await response.Content.ReadFromJsonAsync<UserResponse>();

        if (responseUser == null)
        {
            throw new DomainException("Failed to deserialize response from user service.");
        }

        return responseUser;
    }

    private void GetJwtToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
        }
    }
}
