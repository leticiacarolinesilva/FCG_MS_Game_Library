namespace UserRegistrationAndGameLibrary.Application.Dtos;

public class RequestGameLibrary
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
}