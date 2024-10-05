﻿namespace Keycloak.Net.User.Apis.Common;

internal static class BaseUrl
{
    public static string AdminUrl(string BaseAddress, string RealmName) => $"{(BaseAddress.EndsWith('/') ? BaseAddress.TrimEnd('/') : BaseAddress)}/admin/realms/{RealmName}";
    public static string RealmUrl(string BaseAddress, string RealmName) => $"{(BaseAddress.EndsWith('/') ? BaseAddress.TrimEnd('/') : BaseAddress)}/realms/{RealmName}";
    public static string TokenUrl(string BaseAddress, string RealmName) => $"{RealmUrl(BaseAddress, RealmName)}/protocol/openid-connect/token";
}