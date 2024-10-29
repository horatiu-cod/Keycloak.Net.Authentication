using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;

namespace Keycloak.Net.User.Api.Features.Role.UserRole;

internal class AssignRealmRoleInternalCommand : IAssignRealmRoleInternalCommand
{
    public async Task<Result> AssignRealmRolesAsync(string url, string userId, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/users/{userId}/role-mappings/realm", roles, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result.Fail(response.StatusCode, (string?)content!["error"]);
            }
            return Result.Success(response.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}
