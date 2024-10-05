using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword
{
    internal interface IResetPasswordCommand
    {
        Task<Result> ResetPasswordAsync(string baseAddress, string realmName, string clientId, string clientSecret, string userId, string password, CancellationToken cancellationToken);
    }
}