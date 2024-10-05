using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User
{
    internal interface IGetUserQuery
    {
        Task<Result<GetUserResponse?>> GetUserAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<string?>> GetUserIdAsync(string url, string username, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}