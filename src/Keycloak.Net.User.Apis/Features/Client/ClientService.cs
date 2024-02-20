using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace Keycloak.Net.User.Apis.Features.Client;

internal class ClientService
{
    public async Task<Result<ClientDTO?>> GetClientAsync(string ClientId, string AccessToken, string Realm, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await httpClient.GetAsync($"/admin/realms/{Realm}/clients/?clientId={ClientId}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Result<ClientDTO?>.Fail(response.StatusCode, $"{(int)response.StatusCode} {response.ReasonPhrase} from GetClientAsync");
        if (!response.IsSuccessStatusCode)
            return Result<ClientDTO?>.Fail(response.StatusCode, $"{(int)response.StatusCode} {response.ReasonPhrase} from GeClientAsync");
        if (response.StatusCode != HttpStatusCode.OK)
            return Result<ClientDTO?>.Fail(response.StatusCode, $"{(int)response.StatusCode} {response.ReasonPhrase} from GetClientAsync");
        var results = await response.Content.ReadFromJsonAsync<ClientDTO[]>(cancellationToken);
        if (results is null)
            return Result<ClientDTO?>.Fail(HttpStatusCode.NotFound, $"{(int)HttpStatusCode.NotFound} {HttpStatusCode.NotFound} from GetClientAsync");
        var result = results.FirstOrDefault();
        return Result<ClientDTO?>.Success(result, response.StatusCode);

    }
}
