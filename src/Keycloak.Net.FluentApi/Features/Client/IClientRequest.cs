using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Client
{
    internal interface IClientRequest
    {
        Task<Result<string?>> GetClientUuidAsync(string ClientId, string AccessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}