using Keycloak.Net.Authorization.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider//IAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _authorizationOptions;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options): base(options)
    {
        _authorizationOptions = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        const string POLICY_PREFIX = "urn";
        var policy = await base.GetPolicyAsync(policyName);
        if (policy == null && policyName.StartsWith(POLICY_PREFIX) && !string.IsNullOrEmpty(policyName.Substring(POLICY_PREFIX.Length)))
        {
            var permission = policyName.Substring(POLICY_PREFIX.Length + 1);
            var parts = permission.Split(':');
            var resource = parts[0];
            var scope = parts[1];
            var policyBuilder = new AuthorizationPolicyBuilder();
            policyBuilder.AddAuthenticationSchemes("keycloak");
            var attr = new PermissionAttribute(resource, scope);
            foreach (var requirement in attr.GetRequirements())
            {
                policyBuilder.AddRequirements(requirement);
            }
            policy = policyBuilder.Build();
            _authorizationOptions.AddPolicy(policyName, policy);
        }
        return policy;
    }
    //const string POLICY_PREFIX = "urn";
    ////const string POLICY_PREFIX = "Permission";

    //public DefaultAuthorizationPolicyProvider FallBackPolicyProvider { get; set; }

    //public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    //{
    //    FallBackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //}
    //public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallBackPolicyProvider.GetDefaultPolicyAsync();

    //public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallBackPolicyProvider.GetFallbackPolicyAsync();

    //public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    //{
    //    if (policyName.StartsWith(POLICY_PREFIX) && !string.IsNullOrEmpty(policyName.Substring(POLICY_PREFIX.Length)))
    //    {
    //        var permission = policyName.Substring(POLICY_PREFIX.Length+1);
    //        var parts = permission.Split(':');
    //        var resource = parts[0];
    //        var scope = parts[1];
    //        var policy = new AuthorizationPolicyBuilder();
    //        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
    //        var attr = new PermissionAttribute(resource, scope);
    //        foreach (var requirement in attr.GetRequirements())
    //        {
    //            policy.AddRequirements(requirement);
    //        }
    //        //policy.AddRequirements(attr.GetRequirements().ToList().FirstOrDefault());
    //        //policy.AddRequirements(new PermissionRequirement(resource, scope));
    //        return Task.FromResult(policy?.Build());
    //    }
    //    else
    //    {
    //        return FallBackPolicyProvider.GetPolicyAsync(policyName);
    //    }
    //}
}
