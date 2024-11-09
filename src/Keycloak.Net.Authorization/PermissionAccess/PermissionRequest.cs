using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization.Common;
using Keycloak.Net.Authorization.Configuration;
using Microsoft.Extensions.Options;
using Polly.Registry;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionRequest : IPermissionRequest
{
    private readonly IOptions<JwtBearerValidationOptions> _jwtOptions;
    private readonly IOptions<ClientConfiguration> _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;

    public PermissionRequest(IOptions<JwtBearerValidationOptions> jwtOptions, IOptions<ClientConfiguration> options, IHttpClientFactory httpClientFactory, ResiliencePipelineProvider<string> pipelineProvider)
    {
        _jwtOptions = jwtOptions;
        _options = options;
        _httpClientFactory = httpClientFactory;
        _pipelineProvider = pipelineProvider;
    }

    public async Task<Result<string>> VerifyPermissionAccessAsync(string accessToken, string resource, string scope, string? client = default, CancellationToken cancellationToken = default)
    {
        var clientId = client ?? _options.Value.ClientId;  
        var httpClient = _httpClientFactory.CreateClient("uma");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var requestBody = FormUrlEncodedContentBuilder.PermissionRequestBody(clientId, resource, scope);
        var url = $"{(_jwtOptions.Value.Authority!.EndsWith('/')? _jwtOptions.Value.Authority.TrimEnd('/') : _jwtOptions.Value.Authority)}/protocol/openid-connect/token";


        var pipeline = _pipelineProvider.GetPipeline("keycloak");

        try
        {
            var response = await pipeline.ExecuteAsync(async cancelationToken => await httpClient.PostAsync(url, requestBody, cancellationToken), cancellationToken);

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
                    var error = (string?)content["error_description"] ?? (string?)content["error"];
                    return Result<string>.Fail(response.StatusCode, error);
                }
            }
            else 
            {
                return Result<string>.Fail(System.Net.HttpStatusCode.NotFound, "RPT cannot be found");
            }
        }
        catch (Exception)
        {
            return Result<string>.Fail(System.Net.HttpStatusCode.InternalServerError, "Something went wrong");
            throw;
        }
    }
}
