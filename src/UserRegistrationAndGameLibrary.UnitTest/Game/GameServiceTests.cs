using Moq;

using UserRegistrationAndGameLibrary.Application.Services;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.UnitTest.Game;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _gameService = new GameService(_gameRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateGameAsync_ShouldThrow_WhenPriceIsNegative()
    {
        // Arrange
        decimal invalidPrice = -10;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() =>
            _gameService.CreateGameAsync("Title", "Desc", invalidPrice, DateTime.UtcNow, GameGenre.Action, "image.png"));

        Assert.Equal("Price cannot be negative", exception.Message);
    }

    [Fact]
    public async Task CreateGameAsync_ShouldThrow_WhenTitleAlreadyExists()
    {
        // Arrange
        var title = "Duplicated";
        var existingGame = new Domain.Entities.Game(title, "Desc", 20, DateTime.UtcNow, GameGenre.RPG, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE");

        _gameRepositoryMock.Setup(r => r.SearchAsync(title))
            .ReturnsAsync(new List<Domain.Entities.Game> { existingGame });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _gameService.CreateGameAsync(title, "Desc", 20, DateTime.UtcNow, GameGenre.RPG, "img.png"));

        Assert.Equal("Game with this title already exists", ex.Message);
    }

    [Fact]
    public async Task CreateGameAsync_ShouldCreate_WhenDataIsValid()
    {
        // Arrange
        var title = "New Game";

        _gameRepositoryMock.Setup(r => r.SearchAsync(title))
            .ReturnsAsync(new List<Domain.Entities.Game>());

        _gameRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Game>()))
            .ReturnsAsync((Domain.Entities.Game g) => g);

        // Act
        var result = await _gameService.CreateGameAsync(
            title, "Descrição", 50, DateTime.UtcNow, GameGenre.Strategy, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE");

        // Assert
        Assert.Equal(title, result.Title);
        Assert.Equal(50, result.Price);
        Assert.Equal(GameGenre.Strategy, result.Genre);
    }

    [Fact]
    public async Task GetGameByIdAsync_ShouldReturnGame_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var game = new Domain.Entities.Game("Game", "Desc", 10, DateTime.UtcNow, GameGenre.Horror, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE");

        _gameRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(game);

        // Act
        var result = await _gameService.GetGameByIdAsync(id);

        // Assert
        Assert.Equal(game, result);
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldThrow_WhenGameNotFound()
    {
        var id = Guid.NewGuid();

        _gameRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Domain.Entities.Game)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _gameService.UpdateGameAsync(id, "Title", "Desc", 20, GameGenre.Sports, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE"));

        Assert.Equal("Game not found", ex.Message);
    }

    [Fact]
    public async Task DeleteGameAsync_ShouldThrow_WhenGameNotFound()
    {
        var id = Guid.NewGuid();

        _gameRepositoryMock.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((Domain.Entities.Game)null!);

        var ex = await Assert.ThrowsAsync<DomainException>(() =>
            _gameService.DeleteGameAsync(id));

        Assert.Equal("Game not found", ex.Message);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ShouldReturnFilteredGames()
    {
        var genre = GameGenre.RPG;
        var list = new List<Domain.Entities.Game>
        {
            new("Game 1", "desc", 10, DateTime.UtcNow, genre, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE"),
            new("Game 2", "desc", 15, DateTime.UtcNow, genre, "https://www.google.com/images&cd=vfe&opi=89978449&ved=0CBQQjRxqFwoTCLiHufG0po0DFQAAAAAdAAAAABAE")
        };

        _gameRepositoryMock.Setup(r => r.GetByGenreAsync(genre)).ReturnsAsync(list);

        var result = await _gameService.GetGamesByGenreAsync(genre);

        Assert.Equal(2, result.Count());
    }
}
