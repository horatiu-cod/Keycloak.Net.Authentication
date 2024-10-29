using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Get
{
    internal interface IGetUserIdQuery
    {
        Task<Result<string?>> GetUserIdAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}