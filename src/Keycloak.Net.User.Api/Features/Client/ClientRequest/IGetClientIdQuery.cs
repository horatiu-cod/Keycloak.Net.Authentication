using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.Client.ClientRequest
{
    internal interface IGetClientIdQuery
    {
        //Task<Result<GetClientIdResponse?>> GetClientIdAsync(string ClientId, string AccessToken, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<GetClientIdResponse?>> GetClientIdAsync(string url, string ClientId, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}