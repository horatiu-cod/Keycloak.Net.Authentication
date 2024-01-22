using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization.Common;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authorization.AudienceAccess;

internal class AudienceAccessRequest : IAudienceAccessRequest
{
    private readonly IOptions<JwtBearerValidationOptions> _jwtOptions;
    private readonly IHttpClientFactory _httpClientFactory;

    public AudienceAccessRequest(IOptions<JwtBearerValidationOptions> jwtOptions, IHttpClientFactory httpClientFactory)
    {
        _jwtOptions = jwtOptions;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<string>> VerifyRealmAccess(string clientId, string accessToken, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("uma");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var requestbody = FormUrlEncodedContentBuilder.AudienceAccessRequestBody(clientId);
        var url = $"{_jwtOptions.Value.Authority}/protocol/openid-connect/token";

        var response = await client.PostAsync(url, requestbody, cancellationToken);
        var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
        if (content != null)
        {
            if (response.IsSuccessStatusCode)
            {
                var rpt = (string?)content["access_token"];
                return Result<string>.Success(rpt, response.StatusCode);
            }
            else
            {
                var error = (string?)content["error_description"];
                return Result<string>.Fail(response.StatusCode, error);
            }
        }
        return Result<string>.Fail(System.Net.HttpStatusCode.InternalServerError, "Internal ServerError");
    }

}
