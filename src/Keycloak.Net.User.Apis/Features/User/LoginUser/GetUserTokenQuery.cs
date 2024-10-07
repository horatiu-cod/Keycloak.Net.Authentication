using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.User.LoginUser;

internal class GetUserTokenQuery : IGetUserTokenQuery
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GetUserTokenQuery(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<TokenRepresentation?>> LoginUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string userName, string password, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("kapi");
        var tokenUrl = BaseUrl.TokenUrl(baseAddress, realmName);

        var userRequestDto = new GetUserTokenRequest(clientId, clientSecret, userName, password);
        var requestBody = GetUserTokenRequestBodyBuilder.UserTokenRequestBody(userRequestDto);
        try
        {
            var response = await httpClient.PostAsync(tokenUrl, requestBody, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>();
                return Result<TokenRepresentation>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var token = await response.Content.ReadFromJsonAsync<TokenRepresentation>(cancellationToken);
                return Result<TokenRepresentation?>.Success(token, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<TokenRepresentation>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }

}
