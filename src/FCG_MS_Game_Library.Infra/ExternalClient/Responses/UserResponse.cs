namespace FCG_MS_Game_Library.Infra.ExternalClient.Responses;
public class UserResponse
{
    /// <summary>
    /// User Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User Permission
    /// </summary>
    public string Permission { get; set; } = string.Empty;
}
