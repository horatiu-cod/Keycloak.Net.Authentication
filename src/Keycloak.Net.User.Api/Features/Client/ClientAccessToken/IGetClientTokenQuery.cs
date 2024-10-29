using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Token;

namespace Keycloak.Net.User.Api.Features.Client.ClientAccessToken;

internal interface IGetClientTokenQuery
{
    Task<Result<TokenRepresentation?>> GetClientTokenAsync(string url, GetClientTokenRequest client, HttpClient httpClient, CancellationToken cancellationToken = default);
}