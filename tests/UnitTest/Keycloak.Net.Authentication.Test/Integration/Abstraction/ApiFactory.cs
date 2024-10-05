using DotNet.Testcontainers.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Keycloak;

namespace Keycloak.Net.Authentication.Test.Integration.Abstraction;

public class ApiFactory : WebApplicationFactory<Program>
{
}
