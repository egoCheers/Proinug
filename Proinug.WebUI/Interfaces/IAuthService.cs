using Proinug.WebUI.Dto;
using Proinug.WebUI.Models;

namespace Proinug.WebUI.Interfaces;

/// <summary>
/// Users authentication service
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    Task<(int Error, TokenDto? Token)> LoginAsync(Credentials credentials);
    
    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<(int Error, TokenDto? Token)> RefreshToken(TokenDto token);
}