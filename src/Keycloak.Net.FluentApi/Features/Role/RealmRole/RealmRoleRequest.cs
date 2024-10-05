using Keycloak.Net.FluentApi.Common;

namespace Keycloak.Net.FluentApi.Features.Role.RealmRole;

internal class RealmRoleRequest : IRealmRoleRequest
{
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
                var roleUuid = (string?)content["uuid"];
                return Result<string?>.Success(roleUuid, result.StatusCode);
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail($"{(int)HttpStatusCode.InternalServerError}: {ex.Message} exception from GetRealmRoleAsync");
        }
    }
}
