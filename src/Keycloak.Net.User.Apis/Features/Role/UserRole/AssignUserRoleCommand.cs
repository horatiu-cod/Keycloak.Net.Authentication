using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.Role.UserRole;

internal class AssignUserRoleCommand : IAssignUserRoleCommand
{
    public async Task<Result> AssignClientRolesToUserAsync(string url, string userId, string clientUuid, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/users/{userId}/role-mappings/clients/{clientUuid}", roles, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result.Fail(response.StatusCode, (string?)content!["error"]);

            }
            return Result.Success(response.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail(System.Net.HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }


    public async Task<Result> AssignRealmRolesToUserAsync(string url, string userId, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/users/{userId}/role-mappings/realm", roles, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            return Result.Success(response.StatusCode);
        }
        catch (Exception ex)
        {
            return Result.Fail(System.Net.HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}
