using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Proinug.WebUI.Dto;
using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Models;

namespace Proinug.WebUI.Services;

/// <summary>
/// Custom authentication state provider
/// </summary>
public class CwAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string USER_SESSION_OBJECT_KEY = "user_session_obj";
    
    public TokenDto? TokenDto { get; private set; }

    private readonly IAuthService _authService;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly ISystemClock _systemClock;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CwAuthenticationStateProvider> _logger;
    
    public CwAuthenticationStateProvider(IAuthService authService,
        ProtectedLocalStorage protectedLocalStorage,
        ISystemClock systemClock,
        IConfiguration configuration,
        ILogger<CwAuthenticationStateProvider> logger)
    {
        _authService = authService;
        _protectedLocalStorage = protectedLocalStorage;
        _systemClock = systemClock;
        _configuration = configuration;
        _logger = logger;
    }
    
    /// <summary>
    /// Returns authentication state
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        TokenDto = await GetUserSessionAsync();
        if (TokenDto == null) return GenerateEmptyAuthenticationState();
        if (TokenIsValid()) return GenerateAuthenticationState(TokenDto);
        if (TokenDto.Expires > _systemClock.UtcNow)
        {
            var (error, token) = await _authService.RefreshToken(TokenDto);
            if (error == 0 && token != null)
            {
                await _protectedLocalStorage.SetAsync(USER_SESSION_OBJECT_KEY, JsonSerializer.Serialize(token));
                TokenDto = token;
                return GenerateAuthenticationState(TokenDto);
            }
        }
        await LogoutAsync();
        return GenerateEmptyAuthenticationState();
    }
    
    /// <summary>
    /// Login async
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    public async Task<int> LoginAsync(Credentials credentials)
    {
        var (error, token) = await _authService.LoginAsync(credentials);
        if (error != 0) return error;
        if (token == null) return 1000;
        try
        {
            await _protectedLocalStorage.SetAsync(USER_SESSION_OBJECT_KEY, JsonSerializer.Serialize(token));
            NotifyAuthenticationStateChanged(Task.FromResult(GenerateAuthenticationState(token)));
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred writing data to protected browser local storage");
        }
        return 1000;
    }
    
    /// <summary>
    /// Logout async
    /// </summary>
    public async Task LogoutAsync()
    {
        TokenDto = null;
        try
        {
            await _protectedLocalStorage.DeleteAsync(USER_SESSION_OBJECT_KEY);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting data from protected browser local storage");
        }
        NotifyAuthenticationStateChanged(Task.FromResult(GenerateEmptyAuthenticationState()));
    }
    
    /// <summary>
    /// Token is valid
    /// </summary>
    /// <returns></returns>
    private bool TokenIsValid()
    {
        if (TokenDto == null) return false;
            
        var validTimeRemains = TokenDto.Expires - _systemClock.UtcNow;
        _logger.LogDebug("User token valid time remains {TimeRemains}", validTimeRemains);
            
        if(int.TryParse(_configuration["TokenValidMinutesRemains"], out var timeRemains))
            return validTimeRemains > TimeSpan.Zero &&
                   validTimeRemains > TimeSpan.FromMinutes(timeRemains);
            
        return false;
    }
    
    /// <summary>
    /// Get user session async
    /// </summary>
    /// <returns></returns>
    private async Task<TokenDto?> GetUserSessionAsync()
    {
        if (TokenDto != null) return TokenDto;
        try
        {
            var tokenDto = await _protectedLocalStorage.GetAsync<string>(USER_SESSION_OBJECT_KEY);
            return string.IsNullOrEmpty(tokenDto.Value) ? null : 
                JsonSerializer.Deserialize<TokenDto>(
                    tokenDto.Value, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting data from protected browser local storage");
            return null;
        }
    }
    
    private AuthenticationState GenerateAuthenticationState(TokenDto tokenDto)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, tokenDto.Account.Name),
            new Claim(ClaimTypes.Role, tokenDto.Account.Role.ToString()),
        }, "auth");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return new AuthenticationState(claimsPrincipal);
    }
    
    private static AuthenticationState GenerateEmptyAuthenticationState() =>
        new AuthenticationState(new ClaimsPrincipal());
}