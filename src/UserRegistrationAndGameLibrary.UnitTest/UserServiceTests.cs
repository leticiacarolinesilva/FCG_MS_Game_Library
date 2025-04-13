using UserRegistrationAndGameLibrary.Domain.Interfaces;
using Moq;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;
using Xunit;
namespace UserRegistrationAndGameLibrary.UnitTest;

public class UserServiceTests
{
    private readonly Mock<IGameLibraryRepository> _gameLibraryRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _gameLibraryRepositoryMock = new Mock<IGameLibraryRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        
        _userService = new UserService(
            _gameLibraryRepositoryMock.Object, 
            _userRepositoryMock.Object, 
            _gameRepositoryMock.Object
        );
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        var existingEmail = "test@gmail.com";
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(existingEmail))
            .ReturnsAsync(new User("Existing", new Email(existingEmail), new Password("Pass123!")));

        var dto = new RegisterUserDto
        {
            Name = "Test",
            Email = existingEmail,
            Password = "Pass123!"
        };
        await Assert.ThrowsAsync<DomainException>(() =>
            _userService.RegisterUserAsync(dto));

    }

    [Fact]
    public async Task RegisterUserAsync_ShouldCreateUser_WhenValid()
    {

        var dto = new RegisterUserDto
        {
            Name = "Test user",
            Email = "test@gmail.com",
            Password = "ValidPass123!"
        };
        
        _userRepositoryMock.Setup(e => e.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        
        _userRepositoryMock.Setup(add => add.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        
        var user = await _userService.RegisterUserAsync(dto);
        
        Assert.Equal(dto.Name, user.Name);
        Assert.Equal(dto.Email, user.Email.Value);
        _userRepositoryMock.Verify(add => add.AddAsync(It.IsAny<User>()), Times.Once);

    }

    [Fact]
    public async Task AddGameToLibraryAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        
        _userRepositoryMock.Setup(u => u.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);
        
        await Assert.ThrowsAsync<DomainException>(() =>
            _userService.AddGameToLibraryAsync(userId, gameId));
    }
    
}