namespace Proinug.WebUI.Dto;

/// <summary>
/// Token DTO
/// </summary>
public class TokenDto
{
    /// <summary>
    /// Account
    /// </summary>
    public AccountDto Account { get; set; } = new();

    /// <summary>
    /// JSON Web Token
    /// </summary>
    public string Jwt { get; set; } = "";

    /// <summary>
    /// Expiring time
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Refresh token id
    /// </summary>
    public Guid RefreshTokenId { get; set; }

    /// <summary>
    /// Expiring time in ticks
    /// </summary>
    public long ExpiresJsTicks { get; set; }
}