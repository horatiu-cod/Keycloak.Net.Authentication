using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.LogoutUser
{
    internal interface ILogoutUserCommand
    {
        Task<Result> LogoutUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default);
    }
}