using Keycloak.Net.Authorization.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    const string POLICY_PREFIX = "Permission";
    public DefaultAuthorizationPolicyProvider FallBackPolicyProvider { get; set; }

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallBackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallBackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallBackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX) && !string.IsNullOrEmpty(policyName.Substring(POLICY_PREFIX.Length)))
        {
            var permission = policyName.Substring(POLICY_PREFIX.Length+1);
            var parts = permission.Split(',');
            var resource = parts[0];
            var scope = parts[1];
            var policy = new AuthorizationPolicyBuilder();
            policy.AddAuthenticationSchemes([JwtBearerDefaults.AuthenticationScheme]);
            policy.Requirements.Add(new PermissionRequirement(resource, scope));
            return Task.FromResult(policy?.Build());
        }
        else
        {
            return FallBackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
