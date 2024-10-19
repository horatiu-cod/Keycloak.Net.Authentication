using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.Get
{
    internal interface IGetUserIdQuery
    {
        Task<Result<string?>> GetUserIdAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}