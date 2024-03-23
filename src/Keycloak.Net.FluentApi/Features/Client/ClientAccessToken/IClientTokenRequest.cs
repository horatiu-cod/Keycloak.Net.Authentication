using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Client.ClientAccessToken
{
    internal interface IClientTokenRequest
    {
        Task<Result<string?>> GetClientTokenAsync(string url, string clientId, string clientSecret, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}