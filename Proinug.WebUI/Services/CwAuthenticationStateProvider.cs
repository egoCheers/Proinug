using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Proinug.WebUI.Services;

/// <summary>
/// Custom authentication state provider
/// </summary>
public class CwAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string USER_SESSION_OBJECT_KEY = "user_session_obj";
    
    /// <summary>
    /// Returns 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}