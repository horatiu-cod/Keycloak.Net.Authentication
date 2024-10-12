using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IPermissionRequest _permissionRequest;

    public PermissionAuthorizationHandler(IPermissionRequest permissionRequest)
    {
        _permissionRequest = permissionRequest;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAttribute requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {

            var client = httpContext.GetEndpoint()?.Metadata.GetMetadata<ClientNameAttribute>()?.ClientName ?? null;
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
                    context.Succeed(requirement);
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
        }
    }
}
