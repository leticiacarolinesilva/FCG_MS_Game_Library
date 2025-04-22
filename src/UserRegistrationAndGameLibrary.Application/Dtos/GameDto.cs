namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class GameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
}