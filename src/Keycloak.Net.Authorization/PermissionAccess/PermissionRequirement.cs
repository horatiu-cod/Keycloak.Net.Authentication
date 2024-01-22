using Keycloak.Net.Authorization.Common;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace Keycloak.Net.Authorization.PermissionAccess;

internal class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string resource, string scope)
    {
        Resource = resource;
        Scope = scope;
    }

    public string Resource { get; private set; }
    public string Scope { get; private set; }

}
