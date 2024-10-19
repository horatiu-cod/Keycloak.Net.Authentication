using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Token;

namespace Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;

internal interface IGetClientTokenQuery
{
    Task<Result<TokenRepresentation?>> GetClientTokenAsync(string url, GetClientTokenRequest client, HttpClient httpClient, CancellationToken cancellationToken = default);
}