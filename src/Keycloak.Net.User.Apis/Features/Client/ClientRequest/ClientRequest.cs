using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace Keycloak.Net.User.Apis.Features.Client.ClientRequest;

internal class ClientRequest
{
    private readonly IRequestUrlBuilder _url;

    public ClientRequest(IRequestUrlBuilder url)
    {
        _url = url;
    }

    public async Task<Result<ClientRequestDto?>> GetClientAsync(string ClientId, string AccessToken, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await httpClient.GetAsync($"{_url.AdminApi}/clients/?clientId={ClientId}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Result<ClientRequestDto?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GetClientAsync");
        if (!response.IsSuccessStatusCode)
            return Result<ClientRequestDto?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GeClientAsync");
        if (response.StatusCode != HttpStatusCode.OK)
            return Result<ClientRequestDto?>.Fail(response.StatusCode, $"{(int)response.StatusCode} from GetClientAsync");
        var results = await response.Content.ReadFromJsonAsync<ClientRequestDto[]>(cancellationToken);
        if (results is null)
            return Result<ClientRequestDto?>.Fail(HttpStatusCode.NotFound, $"{(int)HttpStatusCode.NotFound} {HttpStatusCode.NotFound} from GetClientAsync");
        var result = results.FirstOrDefault();
        return Result<ClientRequestDto?>.Success(result, response.StatusCode);

    }
}
