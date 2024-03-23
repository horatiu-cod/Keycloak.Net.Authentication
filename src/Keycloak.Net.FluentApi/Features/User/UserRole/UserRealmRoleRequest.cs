using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.UserRole;

internal class UserRealmRoleRequest : IUserRealmRoleRequest
{
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
