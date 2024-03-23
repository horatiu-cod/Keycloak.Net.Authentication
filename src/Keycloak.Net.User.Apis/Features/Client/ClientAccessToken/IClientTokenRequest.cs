using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken
{
    internal interface IClientTokenRequest
    {
        Task<Result<TokenResponseDto?>> GetClientTokenAsync(ClientTokenRequestDto client, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<string?>> GetClientTokenAsync(string url, string clientId, string clientSecret, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}