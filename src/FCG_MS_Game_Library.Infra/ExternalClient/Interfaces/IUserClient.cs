using FCG_MS_Game_Library.Infra.ExternalClient.Responses;

namespace FCG_MS_Game_Library.Infra.ExternalClient.Interfaces;

public interface IUserClient
{
    Task<UserResponse> GetByIdAsync(Guid userId);
}
