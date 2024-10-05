using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.Role.ClientRole;

internal class GetClientRoleQuery : IGetClientRoleQuery
{
    public async Task<Result<RoleRepresentation?>> GetClientRoleAsync(string url, string clientUuid, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<RoleRepresentation?>.Fail(response.StatusCode, (string?)content!["error"]);
            }
            else
            {
                var role = await response.Content.ReadFromJsonAsync<RoleRepresentation>(cancellationToken);
                if (role is not null)
                {
                    return Result<RoleRepresentation?>.Success(role, response.StatusCode);
                }
                else
                {
                    return Result<RoleRepresentation?>.Fail(HttpStatusCode.NotFound, $"{roleName} was not found");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<RoleRepresentation>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }
    public async Task<Result<RoleRepresentation[]?>> GetClientRolesAsync(string url, string clientUuid, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/clients/{clientUuid}/roles", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                return Result<RoleRepresentation[]>.Fail(response.StatusCode, (string?)content!["error_description"]);
            }
            else
            {
                var roles = await response.Content.ReadFromJsonAsync<RoleRepresentation[]>(cancellationToken);
                if (roles is not null && roles.Length !=0)
                {
                    return Result<RoleRepresentation[]?>.Success(roles, response.StatusCode);
                }
                else
                {
                    return Result<RoleRepresentation[]>.Fail(HttpStatusCode.NotFound, $"No was not found");
                }
            }
        }
        catch (Exception ex)
        {
            return Result<RoleRepresentation[]>.Fail(HttpStatusCode.InternalServerError, $"Something went wrong //br{ex.Message}");
        }
    }

}

