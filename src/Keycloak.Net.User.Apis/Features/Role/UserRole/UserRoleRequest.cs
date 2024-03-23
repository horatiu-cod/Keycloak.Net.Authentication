using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole;

internal class UserRoleRequest : IUserRoleRequest
{
    private readonly IRequestUrlBuilder? _url;

    public UserRoleRequest() { }
    public UserRoleRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result> AssignClientRolesToUserAsync(string userId, string clientUuid, RoleDto[] roles, string accessToken, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            HttpResponseMessage? HttpResult = null;
            foreach (var role in roles)
            {
                var result = await httpClient.PostAsJsonAsync($"{_url.AdminApi}/users/{userId}/role-mappings/clients/{clientUuid}", role, cancellationToken);

                if (!result.IsSuccessStatusCode)
                {
                    return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from AssignClientRolesToUserAsync");
                }
                HttpResult = result;
            }
            return Result.Success(HttpResult!.StatusCode);

        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} from AssignClientRolesToUserAsync");
        }
    }

    public async Task<Result> AssignRealmRolesToUserAsync(string userId, RoleDto[] roles, string accessToken, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            HttpResponseMessage? HttpResult = null;
            foreach (var role in roles)
            {
                var result = await httpClient.PostAsJsonAsync($"{_url.AdminApi}/users/{userId}/role-mappings/realm", role, cancellationToken);

                if (!result.IsSuccessStatusCode)
                {
                    return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from AssignRealmRolesToUserAsync");
                }
                HttpResult = result;
            }
            return Result.Success(HttpResult!.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} from AssignRealmRolesToUserAsync");
        }
    }
    public async Task<Result> AssignRealmRolesToUserAsync(string userId, string role, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var result = await httpClient.PostAsJsonAsync($"{url}/users/{userId}/role-mappings/realm", role, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from AssignRealmRolesToUserAsync");
            }
            return Result.Success(result.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} from AssignRealmRolesToUserAsync");
        }
    }

}
