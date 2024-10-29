using Keycloak.Net.User.Api.Common;
using Keycloak.Net.User.Api.Features.Role;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Api.Features.Role.UserRole;

internal class AssignClientRoleInternalCommand : IAssignClientRoleInternalCommand
{
    public async Task<Result> AssignClientRolesAsync(string url, string userId, string clientUuid, RoleRepresentation[] roles, HttpClient httpClient, CancellationToken cancellationToken = default)
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
            return Result.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
}
