# Keycloak .Net Authorization
[![Build](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml)
[![Build](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/codeql-analysis.yml)

Authentication and Authorization with Keycloak in .NET and ASP.NET Core. Secure your api with Keycloak UMA authorization and JWT bearer authentication.

Add the [ Keycloak.Net.Authorization ](https://www.nuget.org/packages/Keycloak.Net.Authorization)  nuget package to your project. 

Api calls requires auhorization header with an JWT token from Keycloak.
```curl
POST https://yourapi/action HTTP/1.1
Auhorization: Bearer JwtTokenContent
```
## How to use
### Add to program.cs of your api
Add and configure Keycloak.Net.Authentication services - see [README.md](https://github.com/horatiu-cod/Keycloak.Net.Authentication/blob/main/src/Keycloak.Net.Authentication/README.md)

```csharp
using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization;
new code 👆

.....
👇new code
builder.Services
  // Keycloak.Net.Authentication services 
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("appsettings_section_name");
.....
app.UseAuthentication();
app.UseAuthorization();

```
#### Add and configure Keycloak.Net.Authorization 
Configure using the `Action<ClientConfiguration>`

```csharp
builder.Services
  // Keycloak.Net.Authentication services 
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("appsettings_section_name");
  .AddUma(client =>
    {
        client.ClientId = "client-role";
    });
new code 👆
.....
👇new code 
app.UseUma();

app.UseAuthentication();
app.UseAuthorization();

```
Configure by `appsettings.{Environment}.json`

```csharp
builder.Services
  // Keycloak.Net.Authentication services 
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("Appsettings_Section_Name")
  .AddUma("Client_Section_Name);
new code 👆
.....

👇new code 
app.UseUma();

app.UseAuthentication();
app.UseAuthorization();

```
Add to your `appsettings.{Environment}.json`

 ```json
{
  "Client_Section_Name": {
    "ClientId": "<CLIENT_NAME>"
}

```

### Add to your endpoints
#### MinimalAPI

Via custom extenxion method
```csharp
app.MapGet("api/example", () =>
    Results.Ok()
    .RequireUmaAuthorization(resource: "<<resource>>", scope: "<<scope>>");

```
Via Attribute
```csharp
app.MapGet("api/example", [Permission(Resource = "<<resource>>", Scope = "<<scope>>")] () =>
    Results.Ok();

```
Via ASP.NET extension method. The policy string format is: Permission:<<resource>>,<<scope>>
```csharp
app.MapGet("api/example", () =>
    Results.Ok()
    .RequireAuthorization("Permission:<<resource>>,<<scope>>");

```

## How it works

The `UseUMA` middleware exchange the JWT of the request with a RPT received from Keycloak auth server after validating the realm access permission.
The RPT contains the permission granted by the auth server, and is used to autorize access of the resources.







