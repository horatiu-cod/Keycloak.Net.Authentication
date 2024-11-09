using Keycloak.Net.FluentApi.Interfaces;

namespace Keycloak.Net.FluentApi;

public interface IRegisterUser : IUserName, IUserPassword, IUserEmail, IUserFirstName, IUserLastName, IServerUrl, IAuthClientId, IAuthClientSecret;