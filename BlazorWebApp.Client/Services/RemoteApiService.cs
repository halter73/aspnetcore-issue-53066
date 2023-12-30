using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWebApp.Client.Services;

public class RemoteApiService
{
    private readonly HttpClient _client;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public RemoteApiService(
        HttpClient client,
        AuthenticationStateProvider authenticationStateProvider
        )
    {
        _client = client;
        _authenticationStateProvider = authenticationStateProvider;
    }

    private record Claim(string type, object value);

    public async Task<string> GetData()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "test");
        var response = await _client.SendAsync(request);

        var json = JsonSerializer.Deserialize<IEnumerable<Claim>>(await response.Content.ReadAsStringAsync());
        return JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
    }
}