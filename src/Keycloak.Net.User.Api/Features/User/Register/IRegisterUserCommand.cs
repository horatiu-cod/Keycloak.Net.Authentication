using Keycloak.Net.User.Api.Common;

namespace Keycloak.Net.User.Api.Features.User.Register;

internal interface IRegisterUserCommand
{
    Task<Result> Handler(string username, string password, string email, string firstName, string lastName, CancellationToken cancellationToken = default);
}