using Keycloak.Net.Authorization.Common;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Keycloak.Net.Authorization.PermissionAccess;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method , AllowMultiple = true, Inherited = true)]
[DebuggerDisplay("{ToString(),nq}")]
public class PermissionAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "Permission";
    public PermissionAttribute()
    {
        
    }
    public PermissionAttribute(string resource, string scope)
    {
        Resource = resource;
        Scope = scope;
        Policy = $"{POLICY_PREFIX}:{resource},{scope}";
    }

    public string? Resource 
    {
        get 
        {
            if (!string.IsNullOrEmpty(Resource))
            {
                var rsx = Policy.Substring(POLICY_PREFIX.Length).Split(",")[0];
                return rsx;
            }
            return default(string);
        }
        set { }
    }
    public string? Scope
    {
        get
        {
            if (!string.IsNullOrEmpty(Resource))
            {
                var rsx = Policy.Substring(POLICY_PREFIX.Length).Split(",")[1];
                return rsx;
            }
            return default;
        }
        set { }
    }

    public override string ToString()
    {
        return DebuggerHelpers.GetDebugText(Resource, nameof(Resource), Scope, nameof(Scope), includeNullValues: false, prefix: POLICY_PREFIX);
    }
}
