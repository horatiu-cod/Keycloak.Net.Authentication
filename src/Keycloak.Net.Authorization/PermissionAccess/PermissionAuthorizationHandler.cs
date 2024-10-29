using Keycloak.Net.Authorization.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IPermissionRequest _permissionRequest;
    private readonly ClientConfiguration _resourceClient;

    public PermissionAuthorizationHandler(IPermissionRequest permissionRequest, IOptionsMonitor<ClientConfiguration> resourceClient)
    {
        _permissionRequest = permissionRequest;
        _resourceClient = resourceClient.CurrentValue;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAttribute requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {

            var client = httpContext.GetEndpoint()?.Metadata.GetMetadata<ClientNameAttribute>()?.ClientName ?? _resourceClient.ClientId;
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var res = await httpContext.AuthenticateAsync("keycloak");
                if (!res.Succeeded)
                {
                    context.Fail();
                    return;
                }
                var token = await httpContext.GetTokenAsync("access_token");
                if (token is null)
                {
                    context.Fail();
                    return ;
                }
                var response = await _permissionRequest.VerifyPermissionAccessAsync(token!, requirement.Resource, requirement.Scope, client);
                if (response.IsSuccess)
                {
                    var claims = context.User.Claims;
                    const string resourceAccess = "resource_access";
                    var resourceAccessClaim = claims.Single(c => c.Type == resourceAccess);
                    var resource = JsonDocument.Parse(resourceAccessClaim.Value);

                    var audienceResourceExist = resource
                            .RootElement
                            .TryGetProperty(client, out var rolesElement);
                    if (audienceResourceExist)
                    {
                        var claimsIdentity = (ClaimsIdentity)context.User.Identity;
                        var roles = rolesElement.GetProperty("roles").EnumerateArray();
                        foreach (var role in roles)
                        {
                            var value = role.GetString();
                            if (value != null && claimsIdentity is not null && !claimsIdentity.HasClaim(c => c.Equals(value)))
                                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, value));
                        }
                    }
                    context.Succeed(requirement);
                    return ;
                }
                else
                {
                    context.Fail();
                    return;
                }
            }
            else
            {
                context.Fail();
                return;
            }
        }
        else
        {
            context.Fail();
            return;
        }
    }
}
