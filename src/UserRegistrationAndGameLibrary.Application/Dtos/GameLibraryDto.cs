namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class GameLibraryDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string GameTitle { get; set; } = string.Empty;
    public string GameCoverImageUrl { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public bool IsInstalled { get; set; }
}