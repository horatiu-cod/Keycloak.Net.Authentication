using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Client;

internal class ClientRequest : IClientRequest
{
    public async Task<Result<string?>> GetClientUuidAsync(string ClientId, string AccessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await httpClient.GetAsync($"{url}/clients/?clientId={ClientId}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Result<string?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GetClientAsync");
        if (!response.IsSuccessStatusCode)
            return Result<string?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GeClientAsync");
        if (response.StatusCode != HttpStatusCode.OK)
            return Result<string?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GetClientAsync");
        var results = await response.Content.ReadFromJsonAsync<JsonObject[]>(cancellationToken);
        if (results is not null && results.Length != 0)
        {
            var uuid = results.FirstOrDefault()?["uuid"]?.ToString();
            return Result<string?>.Success(uuid, response.StatusCode);
        }
        return Result<string?>.Fail(HttpStatusCode.NotFound, $"{(int)HttpStatusCode.NotFound} {HttpStatusCode.NotFound} from GetClientAsync");
    }
}
