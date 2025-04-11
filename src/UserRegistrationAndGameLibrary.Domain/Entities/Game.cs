using System.Text.RegularExpressions;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Exceptions;

namespace UserRegistrationAndGameLibrary.Domain.Entities;

/// <summary>
/// Represents a game in the platform
/// </summary>
public class Game
{
    /// <summary>
    /// Unique identifier for the game
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Title of the game
    /// </summary>
    public string Title { get; private set; }
    /// <summary>
    /// Description of the game
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// Price of the game
    /// </summary>
    public decimal Price { get; private set; }
    /// <summary>
    /// Release date of the game
    /// </summary>
    public DateTime ReleasedDate { get; private set; }
    /// <summary>
    /// Genre of the game
    /// </summary>
    public GameGenre Genre { get; private set; }
    /// <summary>
    /// URL for the game's cover image
    /// </summary>
    public string CoverImageUrl { get; private set; }
    
    /// <summary>
    /// Used for EF Core
    /// </summary>
    private Game(){}

    /// <summary>
    /// Constructor used to set new Game
    /// </summary>
    /// <param name="title">Title of the game</param>
    /// <param name="description">Description of the game</param>
    /// <param name="price">Price of the game</param>
    /// <param name="releasedDate">Release date of the game</param>
    /// <param name="genre">Genre of the game</param>
    /// <param name="coverImageUrl">URL for the game's cover image</param>
    public Game(
        string title, string description, decimal price, DateTime releasedDate, 
        GameGenre genre, string coverImageUrl
    )
    {
        Id = Guid.NewGuid();
        SetTitle(title);
        SetDescription(description);
        SetPrice(price);
        ReleasedDate = releasedDate;
        Genre = genre;
        SetCoverImageUrl(coverImageUrl);
    }
    
    /// <summary>
    /// Sets the game title (validates for non-empty and max length)
    /// </summary>
    /// <param name="title">Title of the game</param>
    /// <exception cref="DomainException">Thrown if title is invalid.</exception>
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("User name cannot be empty");
            
        if (title.Length > 100)
            throw new DomainException("User name is too long");
            
        Title = title.Trim();
    }
    /// <summary>
    /// Sets the game price (validates for non-negative value).
    /// </summary>
    /// <param name="price">Price of the game</param>
    /// <exception cref="DomainException">Thrown if price is invalid</exception>
    public void SetPrice(decimal price)
    {
        if (price < 0 )
            throw new DomainException("Price cannot be negative");

        Price = price;
    }
    /// <summary>
    /// Sets the game description (validates for non-empty and specific length).
    /// </summary>
    /// <param name="description">Description of the game</param>
    /// <exception cref="DomainException">Thrown if description is invalid</exception>
    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("User name cannot be empty");
            
        if (description.Length > 500)
            throw new DomainException("User name is too long");
            
        Description = description.Trim();
    }
    /// <summary>
    /// Sets the cover image URL (validates format if needed).
    /// </summary>
    /// <param name="url">URL for the game's cover image</param>
    /// <exception cref="DomainException">Thrown if url is invalid</exception>
    public void SetCoverImageUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Cover image URL cannot be empty.");

        //URL format validation 
        var urlRegex = new Regex(@"^(https?|ftp):\/\/[^\s\/$.?#].[^\s]*$");
        if (!urlRegex.IsMatch(url))
            throw new DomainException("Invalid cover image URL format.");

        CoverImageUrl = url.Trim();
    }
}