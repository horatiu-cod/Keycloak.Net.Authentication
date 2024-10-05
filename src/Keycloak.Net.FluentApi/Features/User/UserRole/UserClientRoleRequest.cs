using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.User.UserRole;

internal class UserClientRoleRequest : IUserClientRoleRequest
{
    public async Task<Result> AssignClientRolesToUserAsync(string userId, string clientUuid, string role, string accessToken, string url, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.PostAsJsonAsync($"{url}/users/{userId}/role-mappings/clients/{clientUuid}", role, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return Result.Fail(result.StatusCode, $"{(int)result.StatusCode} from AssignClientRolesToUserAsync");
            }
            return Result.Success(result.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail($"{ex.Message} from AssignClientRolesToUserAsync");
        }
    }
}
