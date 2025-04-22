namespace UserRegistrationAndGameLibrary.Domain.Entities;

public class GameLibrary
{
    /// <summary>
    /// Unique identifier for the library entry
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// User who owns the game
    /// </summary>
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    /// <summary>
    /// Game that is owned
    /// </summary>
    public Guid GameId { get; private set; }
    public Game Game { get; private set; }
    /// <summary>
    /// Date when the game was added to the library
    /// </summary>
    public DateTime PurchaseDate { get; private set; }
    /// <summary>
    /// Price at which the game was purchased
    /// </summary>
    public decimal PurchasePrice { get; private set; }
    /// <summary>
    /// Whether the game is currently installed
    /// </summary>
    public bool IsInstalled { get; private set; }
    /// <summary>
    /// Used for EF Core
    /// </summary>
    private GameLibrary(){}
    
    /// <summary>
    /// Constructor used to set new GameLibrary
    /// </summary>
    /// <param name="user">User who owns the game</param>
    /// <param name="game">Game that is owned</param>
    /// <param name="purchasePrice">Price at which the game was purchased</param>
    public GameLibrary(User user, Game game,decimal purchasePrice)
    {
        Id = Guid.NewGuid();
        UserId = user.Id;
        User = user;
        GameId = game.Id;
        Game = game;
        PurchaseDate = DateTime.UtcNow;
        PurchasePrice = purchasePrice;
        IsInstalled = false;
    }
    
    public void MarkAsInstalled() => IsInstalled = true;
    public void MarkAsUninstalled() => IsInstalled = false;
    public void SetInstalledStatus(bool isInstalled) => IsInstalled = isInstalled;
    
    
}