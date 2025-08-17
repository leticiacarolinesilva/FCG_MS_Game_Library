namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class UpdateGameDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
}
