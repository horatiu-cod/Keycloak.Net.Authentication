using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Logout;
public interface ILogoutUserCommand
{
    Task<Result> LogoutUserAsync(string baseAddress, string realmName, string clientId, string clientSecret, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> LogoutUserAsync(string clientId, string refreshToken, CancellationToken cancellationToken = default);
}