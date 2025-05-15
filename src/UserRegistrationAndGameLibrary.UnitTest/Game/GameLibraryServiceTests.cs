using Moq;

using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Domain.ValueObjects;

namespace UserRegistrationAndGameLibrary.UnitTest.GameLibrary;

public class GameLibraryServiceTests
{
    private readonly Mock<IGameLibraryRepository> _libraryRepoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IGameRepository> _gameRepoMock;
    private readonly GameLibraryService _service;

    public GameLibraryServiceTests()
    {
        _libraryRepoMock = new Mock<IGameLibraryRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _gameRepoMock = new Mock<IGameRepository>();

        _service = new GameLibraryService(
            _libraryRepoMock.Object,
            _userRepoMock.Object,
            _gameRepoMock.Object);
    }

    [Fact]
    public async Task AddGameToLibraryAsync_ShouldThrow_WhenUserNotFound()
    {
        var userId = Guid.NewGuid();
        _userRepoMock.Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((Domain.Entities.User)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _service.AddGameToLibraryAsync(userId, Guid.NewGuid()));

        Assert.Equal("User not found", ex.Message);
    }

    [Fact]
    public async Task AddGameToLibraryAsync_ShouldThrow_WhenGameNotFound()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var user = new Domain.Entities.User("Test", new Email("test@example.com"), new Password("ValidPass1!"));

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gameRepoMock.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync((Domain.Entities.Game)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _service.AddGameToLibraryAsync(userId, gameId));

        Assert.Equal("Game not found", ex.Message);
    }

    [Fact]
    public async Task AddGameToLibraryAsync_ShouldThrow_WhenGameAlreadyOwned()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var user = new Domain.Entities.User("Test", new Email("test@example.com"), new Password("ValidPass1!"));
        var game = new Domain.Entities.Game("Jogo", "desc", 15, DateTime.UtcNow, GameGenre.Horror, "C:\\IMG_3083.jpg");

        _userRepoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gameRepoMock.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);
        _libraryRepoMock.Setup(r => r.UserOwnsGameAsync(userId, gameId)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _service.AddGameToLibraryAsync(userId, gameId));

        Assert.Equal("User already owns this game", ex.Message);
    }

    [Fact]
    public async Task AddGameToLibraryAsync_ShouldAdd_WhenValid()
    {
        var user = new Domain.Entities.User("Test", new Email("test@example.com"), new Password("ValidPass1!"));
        var game = new Domain.Entities.Game("Jogo", "desc", 15, DateTime.UtcNow, GameGenre.Horror, "C:\\IMG_3083.jpg");

        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _gameRepoMock.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);
        _libraryRepoMock.Setup(r => r.UserOwnsGameAsync(user.Id, game.Id)).ReturnsAsync(false);
        _libraryRepoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.GameLibrary>()))
            .ReturnsAsync((Domain.Entities.GameLibrary lib) => lib);

        var result = await _service.AddGameToLibraryAsync(user.Id, game.Id);

        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(result.GameId, game.Id);
        Assert.Equal(game.Price, result.PurchasePrice);
    }

    [Fact]
    public async Task MarkAsInstalledAsync_ShouldThrow_WhenEntryNotFound()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        _libraryRepoMock.Setup(r => r.GetByUserIdAndGameIdAsync(userId, gameId))
            .ReturnsAsync((Domain.Entities.GameLibrary)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _service.MarkAsInstalledAsync(userId, gameId));

        Assert.Equal("Library entry not found", ex.Message);
    }

    [Fact]
    public async Task MarkAsInstalledAsync_ShouldUpdateEntry()
    {
        var entry = new Domain.Entities.GameLibrary(Guid.NewGuid(), Guid.NewGuid(), 20);

        _libraryRepoMock.Setup(r => r.GetByUserIdAndGameIdAsync(entry.UserId, entry.GameId))
            .ReturnsAsync(entry);

        await _service.MarkAsInstalledAsync(entry.UserId, entry.GameId);

        Assert.True(entry.IsInstalled);
        _libraryRepoMock.Verify(r => r.UpdateAsync(entry), Times.Once);
    }

    [Fact]
    public async Task MarkAsUninstalledAsync_ShouldUpdateEntry()
    {
        var entry = new Domain.Entities.GameLibrary(Guid.NewGuid(), Guid.NewGuid(), 20);
        entry.MarkAsInstalled(); // primeiro instala

        _libraryRepoMock.Setup(r => r.GetByUserIdAndGameIdAsync(entry.UserId, entry.GameId))
            .ReturnsAsync(entry);

        await _service.MarkAsUninstalledAsync(entry.UserId, entry.GameId);

        Assert.False(entry.IsInstalled);
        _libraryRepoMock.Verify(r => r.UpdateAsync(entry), Times.Once);
    }

    [Fact]
    public async Task RemoveFromLibraryAsync_ShouldThrow_WhenEntryNotFound()
    {
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        _libraryRepoMock.Setup(r => r.GetByUserIdAndGameIdAsync(userId, gameId))
            .ReturnsAsync((Domain.Entities.GameLibrary)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _service.RemoveFromLibraryAsync(userId, gameId));

        Assert.Equal("Library entry not found", ex.Message);
    }

    [Fact]
    public async Task RemoveFromLibraryAsync_ShouldCallRepo_WhenValid()
    {
        var entry = new Domain.Entities.GameLibrary(Guid.NewGuid(), Guid.NewGuid(), 20);

        _libraryRepoMock.Setup(r => r.GetByUserIdAndGameIdAsync(entry.UserId, entry.GameId))
            .ReturnsAsync(entry);

        await _service.RemoveFromLibraryAsync(entry.UserId, entry.GameId);

        _libraryRepoMock.Verify(r => r.RemoveFromLibraryAsync(entry.GameId), Times.Once);
    }
}
