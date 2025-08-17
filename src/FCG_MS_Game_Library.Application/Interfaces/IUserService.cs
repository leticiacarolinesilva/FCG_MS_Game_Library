using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Domain.Entities;

namespace UserRegistrationAndGameLibrary.Application.Interfaces;

public interface IUserService
{
    Task<ResponseUserDto> RegisterUserAsync(RegisterUserDto dto);
    Task<User?> GetUserByEmailAsync(string email);
    Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId);
    Task<List<ResponseUserDto>> SearchUsersAsync(string email, string name);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<string> UpdateAsync(UpdateUserDto user);
    Task DeleteAsync(Guid userId);
}
