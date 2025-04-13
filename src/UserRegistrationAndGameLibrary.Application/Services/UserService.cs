using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class UserService : IUserService
{
    private readonly IGameLibraryRepository _gameLibraryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;

    public UserService(IGameLibraryRepository gameLibraryRepository, IUserRepository userRepository, IGameRepository gameRepository)
    {
        _gameLibraryRepository = gameLibraryRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
    }
    
    public async Task<User> RegisterUserAsync(RegisterUserDto userDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new DomainException("Email already exists");
        }
        var emailVo = new Email(userDto.Email);
        var passwordVo = new Password(userDto.Password);
        
        var user = new User(userDto.Name,emailVo, passwordVo);
        await _userRepository.AddAsync(user);
        
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var emailVo = new Email(email);
        return await _userRepository.GetByEmailAsync(emailVo);
        
    }

    public async Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new DomainException("User not found");
        }
        
        var game = await _gameRepository.GetByIdAsync(gameId);
        if (game == null)
        {
            throw new DomainException("Game not found");
        }
        
        var existingLibraryEntries = await _gameLibraryRepository.GetByUserIdAsync(userId);
        if (existingLibraryEntries.Any(gl => gl.GameId == gameId))
        {
            throw new DomainException("User already owns this game");
        }

        var gameLibrary = new GameLibrary(user, game, game.Price);
        await _gameLibraryRepository.AddAsync(gameLibrary);
        
        return gameLibrary;
    }
}