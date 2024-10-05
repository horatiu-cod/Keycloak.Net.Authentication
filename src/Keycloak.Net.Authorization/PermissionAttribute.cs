using Keycloak.Net.Authorization.Common;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Keycloak.Net.Authorization;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
[DebuggerDisplay("{ToString(),nq}")]
public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData
{
    public string Resource { get; }
    public string Scope { get; }
    public PermissionAttribute(string resource, string scope)
    {
        Resource = resource;
        Scope = scope;
    }
    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }

    public override string ToString()
    {
        return DebuggerHelpers.GetDebugText(Resource, nameof(Resource), Scope, nameof(Scope), includeNullValues: false);
    }

}
