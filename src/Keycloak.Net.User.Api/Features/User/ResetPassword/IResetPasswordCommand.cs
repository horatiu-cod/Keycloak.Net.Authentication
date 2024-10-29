using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.ResetPassword
{
    internal interface IResetPasswordCommand
    {
        Task<Result> Handler(string username, string password, CancellationToken cancellationToken = default);
    }
}