namespace Keycloak.Net.User.Apis.Features.User.Register;

public interface IRegisterUserWithClientRoleCommand
{
    Task<Result> RegisterUserWithClientRole(string clientId, string username, string password, string email, string firstName, string lastName, string[] rolesName, CancellationToken cancellationToken = default);
}