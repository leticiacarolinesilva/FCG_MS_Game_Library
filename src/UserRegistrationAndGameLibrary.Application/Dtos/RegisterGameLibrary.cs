namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class RegisterGameLibrary
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public bool IsInstalled { get; set; }
}