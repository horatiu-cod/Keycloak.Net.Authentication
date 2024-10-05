using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization.Common;
using Keycloak.Net.Authorization.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionRequest : IPermissionRequest
{
    private readonly IOptions<JwtBearerValidationOptions> _jwtOptions;
    private readonly IOptions<ClientConfiguration> _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public PermissionRequest(IOptions<JwtBearerValidationOptions> jwtOptions, IOptions<ClientConfiguration> options, IHttpClientFactory httpClientFactory)
    {
        _jwtOptions = jwtOptions;
        _options = options;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<string>> VerifyPermissionAccessAsync(string accessToken, string resource, string scope, string? client = default, CancellationToken cancellationToken = default)
    {
        var clientId = client ?? _options.Value.ClientId;  
        var httpClient = _httpClientFactory.CreateClient("uma");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var requestBody = FormUrlEncodedContentBuilder.PermissionRequestBody(clientId, resource, scope);
        var url = $"{_jwtOptions.Value.Authority}/protocol/openid-connect/token";

        var response = await httpClient.PostAsync(url, requestBody, cancellationToken);
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
