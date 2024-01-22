using Keycloak.Net.Authorization.AudienceAccess;
using Keycloak.Net.Authorization.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionRequest _permissionRequest;
    private readonly IOptions<ClientConfiguration> _clientConfiguration;

    public PermissionAuthorizationHandler(IPermissionRequest permissionRequest, IOptions<ClientConfiguration> clientConfiguration)
    {
        _permissionRequest = permissionRequest;
        _clientConfiguration = clientConfiguration;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {


            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var res = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                if (!res.Succeeded)
                    context.Fail();
                var token = await httpContext.GetTokenAsync("access_token");
                if (token is null)
                    context.Fail();
                var response = await _permissionRequest.VerifyPermissionAccessAsync(token!, requirement.Resource, requirement.Scope);
                if (response.IsSuccess)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
        else
        {
            context.Fail();
        }
    }
}
