using UserRegistrationAndGameLibrary.Application.Dtos;

namespace UserRegistrationAndGameLibrary.Application.Interfaces;
public interface IAuthService
{
    public Task<string> GetToken(UserDto request);
}
