using System.Text;
using System.Text.Json;

namespace Proinug.WebUI.Extensions;

public static class HttpClientExtensions
{
    private static readonly Random Rnd = new Random();
    
    /// <summary>
    /// Send data (object) as json
    /// </summary>
    /// <param name="client"></param>
    /// <param name="method"></param>
    /// <param name="content"></param>
    /// <param name="uri"></param>
    /// <param name="jwt"></param>
    /// <param name="requestId"></param>
    /// <returns></returns>
    public static Task<HttpResponseMessage> SendAsJson(this HttpClient client, HttpMethod method, 
        object? content, string uri, string jwt = "", string requestId = "")
    {
        var httpRequest = new HttpRequestMessage(method, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"),
        };

        if (!string.IsNullOrEmpty(jwt))
        {
            httpRequest.Headers.Add("Authorization", "Bearer " + jwt);
        }

        if (!string.IsNullOrEmpty(requestId))
        {
            httpRequest.Headers.Add("Request-id", requestId);
        }

        return client.SendAsync(httpRequest);
    }
    
    /// <summary>
    /// Read response json data as object
    /// </summary>
    /// <param name="responseContent"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> ReadAs<T>(this HttpContent responseContent) where T : class
    {
        var jsonStream = await responseContent.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(
            jsonStream, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <summary>
    /// Generate request id
    /// </summary>
    /// <returns></returns>
    public static string GenerateRequestId(this HttpClient client)
    {
        var rndBuff = new byte[8];
        Rnd.NextBytes(rndBuff);
        return Convert.ToBase64String(rndBuff);
    }
}