using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User
{
    internal interface IUserRequest
    {
        Task<Result<UserRequestDto>> GetUserAsync(string username, string accessToken, HttpClient httpClient, CancellationToken cancellationToken = default);
        Task<Result<string?>> GetUserAsync(string username, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default);
    }
}