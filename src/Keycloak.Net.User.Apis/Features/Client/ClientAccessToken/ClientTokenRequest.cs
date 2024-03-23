using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;

internal class ClientTokenRequest : IClientTokenRequest
{
    private readonly IRequestUrlBuilder _url;

    public ClientTokenRequest()
    {
        
    }
    public ClientTokenRequest(IRequestUrlBuilder url)
    {
        _url = url;
    }

    public async Task<Result<TokenResponseDto?>> GetClientTokenAsync(ClientTokenRequestDto client, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        var requestBody = ClientTokenRequestBodyBuilder.ClientTokenRequestBody(client);
		try
		{
            var response = await httpClient.PostAsync(_url.TokenApi, requestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<TokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} from GetClientTokenAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<TokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} from GetClientTokenAsync");
            }
            else
            {
                var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                return Result<TokenResponseDto?>.Success(keycloakTokenResponseDto);
            }
        }
        catch (BadHttpRequestException ex)
		{
            return Result<TokenResponseDto>.Fail($"{ex.Message} Exception from GetClientTokenAsync");
        }
    }
    public async Task<Result<string?>> GetClientTokenAsync(string url,string clientId, string clientSecret, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        var client = new ClientTokenRequestDto { ClientId = clientId, ClientSecret = clientSecret };
        var requestBody = ClientTokenRequestBodyBuilder.ClientTokenRequestBody(client);
        try
        {
            var response = await httpClient.PostAsync(url, requestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<string>.Fail(response.StatusCode, $"{response.StatusCode} from GetClientTokenAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<string>.Fail(response.StatusCode, $"{response.StatusCode} from GetClientTokenAsync");
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                if (content is not null) {
                    var rpt = (string?)content["access_token"];
                    return Result<string?>.Success(rpt, response.StatusCode);
                }
                return Result<string>.Fail( $"Access token not found from GetClientTokenAsync");
            }
        }
        catch (BadHttpRequestException ex)
        {
            return Result<string>.Fail($"{ex.Message} Exception from GetClientTokenAsync");
        }
    }

}
