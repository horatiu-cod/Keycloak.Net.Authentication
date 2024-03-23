using Keycloak.Net.User.Apis.Common;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Keycloak.Net.User.Apis.Features.Role.RealmRole;

internal class RealmRoleRequest : IRealmRoleRequest
{
    private readonly IRequestUrlBuilder? _url;

    public RealmRoleRequest() { }
    public RealmRoleRequest(IRequestUrlBuilder url) => _url = url;

    public async Task<Result<RoleDto>> GetRealmRoleAsync(string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.GetAsync($"{_url.AdminApi}/clients/roles/{roleName}", cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetRealmRoleAsync");
            }
            else if (!result.IsSuccessStatusCode)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetRealmRoleAsync");
            }
            else if (result.StatusCode != HttpStatusCode.OK)
            {
                return Result<RoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetRealmRoleAsync");
            }
            else
            {
                var keycloakRoleDto = await result.Content.ReadFromJsonAsync<RoleDto>(cancellationToken);
                return Result<RoleDto>.Success(keycloakRoleDto, result.StatusCode);
            }

        }
        catch (Exception ex)
        {
            return Result<RoleDto>.Fail($"{(int)HttpStatusCode.InternalServerError}: {ex.Message} exception from GetRealmRoleAsync");
        }
    }
    public async Task<Result<string?>> GetRealmRoleAsync(string url, string accessToken, string roleName, HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        try
        {
            var result = await httpClient.GetAsync($"{url}/clients/roles/{roleName}", cancellationToken);
            if (!result.IsSuccessStatusCode)
            {
                return Result<string>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetRealmRoleAsync");
            }
            else
            {
                var content = await result.Content.ReadFromJsonAsync<JsonObject>(cancellationToken);
                if (content == null)
                    return Result<string>.Fail(result.StatusCode, $"{(int)result.StatusCode} from GetRealmRoleAsync");
                var role = (string?)content["uuid"];
                return Result<string?>.Success(role, result.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail($"{(int)HttpStatusCode.InternalServerError}: {ex.Message} exception from GetRealmRoleAsync");
        }
    }

}
