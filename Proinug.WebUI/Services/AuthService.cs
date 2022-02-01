using Proinug.WebUI.Dto;
using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Models;

namespace Proinug.WebUI.Services;

/// <summary>
/// Users authentication service
/// </summary>
public class AuthService: IAuthService
{
    private readonly HttpClient _client;
    private readonly ILogger<AuthService> _logger;

    public AuthService(HttpClient client, ILogger<AuthService> logger)
    {
        _client = client;
        _logger = logger;
    }
    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    public Task<(int Error, TokenDto? Token)> LoginAsync(Credentials credentials)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<(int Error, TokenDto? Token)> RefreshToken(TokenDto token)
    {
        throw new NotImplementedException();
    }
}