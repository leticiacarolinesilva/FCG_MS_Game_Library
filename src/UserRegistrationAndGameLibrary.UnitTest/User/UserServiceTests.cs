using UserRegistrationAndGameLibrary.Domain.Interfaces;
using Moq;
using UserRegistrationAndGameLibrary.Application.Dtos;
using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.UnitTest.User;

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
                _gameRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldCreateUser_WhenValid()
        {
            var dto = new RegisterUserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "ValidPass1!"
            };

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((Domain.Entities.User)null);
            
            _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.User>()))
                .Returns(Task.CompletedTask);
            
            var result = await _userService.RegisterUserAsync(dto);
            
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Email, result.Email.Value);
            
            _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrow_WhenEmailExists()
        {
            var dto = new RegisterUserDto
            {
                Name = "Test User",
                Email = "existing@example.com",
                Password = "ValidPass1!"
            };

            var existingUser = new Domain.Entities.User("Existing", new Email(dto.Email), new Password(dto.Password));
            
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);
            
            await Assert.ThrowsAsync<DomainException>(() => 
                _userService.RegisterUserAsync(dto));
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenExists()
        {
            
            const string email = "test@example.com";
            var expectedUser = new Domain.Entities.User("Test", new Email(email), new Password("ValidPass1!"));

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(expectedUser);
            
            var result = await _userService.GetUserByEmailAsync(email);

            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            const string email = "nonexistent@example.com";

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync((Domain.Entities.User)null);

            // Act
            var result = await _userService.GetUserByEmailAsync(email);

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task AddGameToLibraryAsync_ShouldCreateValidLibraryEntry_WhenInputIsValid()
        {

            var user = new Domain.Entities.User("Test User", 
                new Email("test@example.com"), 
                new Password("ValidPass1!")); 
    
            var game = new Game(
                "Test Game", 
                "Description", 
                29.99m, 
                DateTime.UtcNow, 
                GameGenre.RPG, 
                "https://test.com/cover.jpg"); 
            
            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);
    
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(game);
            
            GameLibrary createdLibrary = null;
            _gameLibraryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<GameLibrary>()))
                .Callback<GameLibrary>(gl => createdLibrary = gl)
                .ReturnsAsync((GameLibrary gl) => gl);
            
            var result = await _userService.AddGameToLibraryAsync(Guid.NewGuid(), Guid.NewGuid());
            
            Assert.NotNull(createdLibrary);
            Assert.Equal(createdLibrary.Id, result.Id);
            Assert.Equal(createdLibrary.UserId, result.UserId);
            Assert.Equal(createdLibrary.GameId, result.GameId);
    
            _gameLibraryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<GameLibrary>()), Times.Once);
        }

        [Fact]
        public async Task AddGameToLibraryAsync_ShouldThrow_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync((Domain.Entities.User)null);
            
            await Assert.ThrowsAsync<DomainException>(() => 
                _userService.AddGameToLibraryAsync(userId, gameId));
        }

        [Fact]
        public async Task AddGameToLibraryAsync_ShouldThrow_WhenGameNotFound()
        {

            var userId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var user = new Domain.Entities.User("Test", new Email("test@example.com"), new Password("ValidPass1!"));

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(gameId))
                .ReturnsAsync((Game)null);
            
            await Assert.ThrowsAsync<DomainException>(() => 
                _userService.AddGameToLibraryAsync(userId, gameId));
        }

        [Fact]
        public async Task AddGameToLibraryAsync_ShouldThrow_WhenUserAlreadyOwnsGame()
        {
            
            var user = new Domain.Entities.User("Test", 
                new Email("test@example.com"), 
                new Password("ValidPass1!"));
            
            var game = new Game("Test Game", 
                "Description", 
                29.99m, 
                DateTime.UtcNow, 
                GameGenre.RPG, 
                "https://example.com/test.jpg");
            
            var existingEntry = new GameLibrary(user, game, game.Price);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(user);
            
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(game.Id))
                .ReturnsAsync(game);
            
            _gameLibraryRepositoryMock.Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<GameLibrary> { existingEntry });
            
            var exception = await Assert.ThrowsAsync<DomainException>(() => 
                _userService.AddGameToLibraryAsync(Guid.NewGuid(), game.Id));
            
            Assert.Equal("User already owns this game", exception.Message);            
        }
    }
