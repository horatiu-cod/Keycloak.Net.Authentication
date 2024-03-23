using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User
{
    internal interface IUserRequest
    {
        Task<Result<string?>> GetUserAsync(string username, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}