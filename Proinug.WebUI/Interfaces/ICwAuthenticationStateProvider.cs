using Microsoft.AspNetCore.Components.Authorization;
using Proinug.WebUI.Dto;
using Proinug.WebUI.Models;

namespace Proinug.WebUI.Interfaces;

public interface ICwAuthenticationStateProvider
{
    TokenDto? TokenDto { get; }

    /// <summary>
    /// Returns authentication state
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<AuthenticationState> GetAuthenticationStateAsync();

    /// <summary>
    /// Login async
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    Task<int> LoginAsync(Credentials credentials);

    /// <summary>
    /// Logout async
    /// </summary>
    Task LogoutAsync();

    event AuthenticationStateChangedHandler? AuthenticationStateChanged;
}