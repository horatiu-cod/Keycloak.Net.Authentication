using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.ResetPassword
{
    internal interface IResetPasswordCommand
    {
        Task<Result> Handler(string username, string password, CancellationToken cancellationToken = default);
    }
}