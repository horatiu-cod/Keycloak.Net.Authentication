using Keycloak.Net.User.Apis.Common;
using Keycloak.Net.User.Apis.Features.Client.ClientAccessToken;
using Keycloak.Net.User.Apis.Features.Role.RealmRole;
using Keycloak.Net.User.Apis.Features.Role.UserRole;
using System.Net.Http.Json;

namespace Keycloak.Net.User.Apis.Features.User.RegisterUser;

internal class RegisterUserRequest : IRegisterUserRequest
{
    private IClientTokenRequest ClientTokenRequest {  get; }
    private IRealmRoleRequest RealmRoleRequest { get; }
    private IUserRoleRequest UserRoleRequest { get; }
    private IUserRequest UserRequest { get; }
    private readonly HttpClient httpClient;
    public RegisterUserRequest()
    {
        ClientTokenRequest = new ClientTokenRequest();
        RealmRoleRequest = new RealmRoleRequest();
        UserRoleRequest = new UserRoleRequest();
        UserRequest = new UserRequest();
        httpClient = new HttpClient();
    }

    public async Task<Result> RegisterUserWithRealmRole(string url, string clientId, string clientSecret, string username, string password, string[] roles)
    {
        var tokenResponse = await ClientTokenRequest.GetClientTokenAsync(url, clientId, clientSecret, httpClient);
        if(!tokenResponse.IsSuccess)
            return Result.Fail(tokenResponse.StatusCode, tokenResponse.Error);
        var registerResult = await RegisterUser(username, password,tokenResponse.Content!, url);
        if(!registerResult.IsSuccess)
            return Result.Fail(registerResult.StatusCode, registerResult.Error);
        var userResult = await UserRequest.GetUserAsync(clientId, clientSecret, url, httpClient);
        if(!userResult.IsSuccess)
            return Result.Fail(userResult.StatusCode, registerResult.Error);
        foreach (var role in roles)
        {
            var response = await RealmRoleRequest.GetRealmRoleAsync(tokenResponse.Content!, role, httpClient);
            if(!response.IsSuccess)
                return Result.Fail(response.StatusCode, response.Error);
            var result = await UserRoleRequest.AssignRealmRolesToUserAsync(userResult.Content!, role, tokenResponse.Content!, url, httpClient);
            if(!result.IsSuccess)
                return Result.Fail(result.StatusCode, result.Error);
        }
        return Result.Success(System.Net.HttpStatusCode.Created);
    }
    public async Task RegisterUserWithClientRole(string clientId, string clientSecret, string username, string password, string[] roles)
    {

    }
    public async Task<Result> RegisterUser(string username, string password, string accessToken, string url)
    {
        var credentials = new Credentials(password);
        var user = new RegisterUserDto()
        {
            Email = username,
            UserName = username,
            Credentials = [credentials]
        };
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var response = await httpClient.PostAsJsonAsync(url, user);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
                return Result.Success(response.StatusCode);
            return Result.Fail(response.StatusCode, "Register fail");
        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} Exception for register fail");
        }
    }
}
