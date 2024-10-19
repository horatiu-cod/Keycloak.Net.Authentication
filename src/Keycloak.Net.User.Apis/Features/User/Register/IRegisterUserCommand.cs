using Keycloak.Net.User.Apis.Common;

namespace Keycloak.Net.User.Apis.Features.User.Register;

internal interface IRegisterUserCommand
{
    Task<Result> RegisterUser(string username, string password, string email, string firstName, string lastName, CancellationToken cancellationToken =default);
}