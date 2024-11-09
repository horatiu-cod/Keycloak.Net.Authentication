using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Client.ClientAccessToken;

internal class ClientTokenRequest : IClientTokenRequest
{
    public async Task<Result<string?>> GetClientTokenAsync(string url, string clientId, string clientSecret, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        var client = new ClientTokenRequestDto { ClientId = clientId, ClientSecret = clientSecret };
        var requestBody = ClientTokenRequestBodyBuilder.ClientTokenRequestBody(client);
        var tokenUrl = $"{url}protocol/openid-connect/token";
        try
        {
            var response = await httpClient.PostAsync(tokenUrl, requestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized && !response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<string>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<string>.Fail(response.StatusCode, $"{response.StatusCode} from GetClientTokenAsync");
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                if (content is not null)
                {
                    var rpt = (string?)content["access_token"];
                    return Result<string?>.Success(rpt, response.StatusCode);
                }
                return Result<string?>.Fail(HttpStatusCode.NotFound,$"Access token not found from GetClientTokenAsync");
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail(HttpStatusCode.InternalServerError,$"{ex.Message}");
        }
    }
}
