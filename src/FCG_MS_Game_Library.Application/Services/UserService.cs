using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Interfaces;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.Application.Services;

public class UserService : IUserService
{
    private readonly IGameLibraryRepository _gameLibraryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserAuthorizationRepository _userAuthorizationRepository;

    public UserService(
        IGameLibraryRepository gameLibraryRepository,
        IUserRepository userRepository,
        IGameRepository gameRepository,
        IUserAuthorizationRepository userAuthorizationRepository)
    {
        _gameLibraryRepository = gameLibraryRepository;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _userAuthorizationRepository = userAuthorizationRepository;
    }

    public async Task<ResponseUserDto> RegisterUserAsync(RegisterUserDto userDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);
        if (existingUser != null)
        {
            throw new DomainException("Email already exists");
        }
        var emailVo = new Email(userDto.Email);
        var passwordVo = new Password(userDto.Password);

        var user = new User(userDto.Name, emailVo, passwordVo);
        await _userRepository.AddAsync(user);

        var userAuthorization = new UserAuthorization(user.Id, AuthorizationPermissions.User);
        await _userAuthorizationRepository.AddAsync(userAuthorization);

        var userReponseDto = new ResponseUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Permission = AuthorizationPermissions.User.ToString(),
        };

        return userReponseDto;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var emailVo = new Email(email);
        return await _userRepository.GetByEmailAsync(emailVo.Value);
    }

    public async Task<GameLibrary> AddGameToLibraryAsync(Guid userId, Guid gameId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
                   ?? throw new DomainException("User not found");

        var game = await _gameRepository.GetByIdAsync(gameId)
                   ?? throw new DomainException("Game not found");


        var existingEntries = await _gameLibraryRepository.GetByUserIdAsync(userId);
        if (existingEntries.Any(gl => gl.GameId == gameId))
        {
            throw new DomainException("User already owns this game");
        }

        var gameLibrary = new GameLibrary(user.Id, game.Id, game.Price);
        await _gameLibraryRepository.AddAsync(gameLibrary);
        return gameLibrary;
    }

    public async Task<List<ResponseUserDto>> SearchUsersAsync(string email, string name)
    {
        var users = await _userRepository.SearchUsersAsync(email, name);

        var responseUserDto = new List<ResponseUserDto>();

        foreach (var user in users)
        {
            var response = new ResponseUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Permission = user.Authorization.Permission.ToString(),
            };
            responseUserDto.Add(response);
        }

        return responseUserDto;
    }

    public async Task<string?> UpdateAsync(UpdateUserDto userDto)
    {
        if (userDto == null)
            return "Request invalid is not allowed to be null";

        var userResponse = await GetUserByIdAsync(userDto.UserId);

        if (userResponse == null)
            return "UserId does not exist";

        var isEmailExist = await _userRepository.SearchUsersAsync(userDto.Email, name: null);

        if (isEmailExist.Count > 0) return "Email indicated already exists in the database";

        userResponse.SetName(userDto.Name);

        if(userDto.Email != null)
            userResponse.SetEmail(userDto.Email);

        await _userRepository.UpdateAsync(userResponse);

        var updatedUser = await GetUserByIdAsync(userResponse.Id);

        return "Usuario Atualizado com sucesso";
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await GetUserByIdAsync(userId);

        if (user == null)
            return;

        await _userRepository.DeleteAsync(user);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
}
