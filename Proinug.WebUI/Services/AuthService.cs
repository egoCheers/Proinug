using System.Net;
using Proinug.WebUI.Dto;
using Proinug.WebUI.Extensions;
using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Models;

namespace Proinug.WebUI.Services;

/// <summary>
/// Users authentication service
/// </summary>
public class AuthService: IAuthService
{
    private const string API = "api";
    private const string LOGIN_ENDPOINT = "Token/signin";
    private const string REFRESH_ENDPOINT = "Token/refreshId=";
    
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
    public async Task<(int Error, TokenDto? Token)> LoginAsync(Credentials credentials)
    {
        var uri = $"{API}/{LOGIN_ENDPOINT}";
        try
        {
            var response = await _client.SendAsJson(HttpMethod.Post, new 
                { 
                    name = credentials.Username, 
                    password = credentials.Password 
                },
                uri);

            if (response.StatusCode != HttpStatusCode.OK) return ((int) response.StatusCode, null);
            var tokenDto = await response.Content.ReadAs<TokenDto>();
            return (0, tokenDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while login");
            return (1000, null);
        }
    }

    /// <summary>
    /// Refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<(int Error, TokenDto? Token)> RefreshToken(TokenDto token)
    {
        var uri = $"{API}/{REFRESH_ENDPOINT}{token.RefreshTokenId}";

        try
        {
            var response = await _client.SendAsJson(HttpMethod.Post, null, uri, token.Jwt);

            if (response.StatusCode != HttpStatusCode.OK) return ((int) response.StatusCode, null);
            var tokenDto = await response.Content.ReadAs<TokenDto>();
            return (0, tokenDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while refresh token");
            return (1000, null);
        }
    }
}