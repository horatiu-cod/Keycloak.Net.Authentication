# Keycloak .Net Authentication and Authorization (UMA)
[![Build](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml)
[![Build](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/codeql-analysis.yml)

## Authentication and Authorization with Keycloak in .NET and ASP.NET Core. Secure your api with Keycloak UMA authorization and JWT bearer authentication.

Add the [ Keycloak.Net.Authentification ](https://www.nuget.org/packages/Keycloak.Net.Authentication) and [ Keycloak.Net.Authorization ](https://www.nuget.org/packages/Keycloak.Net.Authorization) nuget packages to your project.

Api calls requires auhorization header with an JWT token from Keycloak.
```curl
POST https://yourapi/action HTTP/1.1
Auhorization: Bearer JwtTokenContent
```
### How to use

### Authentication

Add to program.cs of your api
#### Option no.1 (easiest way)
```csharp
using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization;
new code 👆

.
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("appsettings_section_name");
.....
app.UseAuthentication();
app.UseAuthorization();

```
Add section to the appsettings.{Environment}.json
- Authority is required. If not set will throw exception during app startup.
- Set the audience as "Audience", "ValidAudience" or "ValidAudiences". If not set will throw exception during app startup.
```json
 {
    "KeycloakUrl": "<<FROM_USER_SECRET>>",
    "RealmName": "<<FROM_USER_SECRET>>",
    
    "appsettings_section_name": {
          "Authority": "{KeycloakUrl}{RealmName}",
          "Audience": "<<Audience>>"
       }
// or
    "appsettings_section_name": {
          "Authority": "{KeycloakUrl}{RealmName}",
          "ValidAudience": "<<Audience>>"
       }
//or
    "appsettings_section_name": {
          "Authority": "{KeycloakUrl}{RealmName}",
          "ValidAudiences": ["<<Audience>>"]
       }
 }
```
#### Option no.2 
- Variation of first option. Additional configuration of JwtBearerOptions available.
- If set will override the value of same property defined in appsettings.{Environment}.json
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("appsettings_section_name", options =>
  {
    options.Audience = "<<Audience>>";
    options.SaveToken = true;

    options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
  });
```
#### Option no.3
- If you want to get auth options from other source, not from appsettings.{Environment}.json, you can pass Action\<JwtBearerValidationOptions\> instead.
- Authority is required. If not set will throw exception during app startup.
- Set the audience as "Audience", "ValidAudience" or "ValidAudiences".If not set will throw exception during app startup.
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions(options =>
    {
        options.KeycloakAuthority = "https://{host}/realms/{realm}";
        options.KeycloakAudience = "<<Audience>>";
    })

```
#### Option no.4
- You have to manually configure the JwtBearerOtions.
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddJwtBearerOptions(options =>
    {
        options.Authority = "https://{host}/realms/{realm}";
        options.Audience = "<<Audience>>";
        ......
        options.TokenValidationParameters = new TokenValidationParameters( options =>
        {
          options.ClockSkew = TimeSpan.FromSeconds(30);
          .......
        });
    });
```
#### JWT transformation
- Under the hood the Keyclaok JWT is mapped and transformed to Identity JWT
- The Keycloak Realm and Client "roles" claims are mapped and transformed to ClaimType.Role
- By default the Keycloak "preferred_username" claim is transformed to ClaimType.Name. You can change it by just adding the following:
```js
{
    "KeycloakUrl": "<<FROM_USER_SECRET>>",
    "RealmName": "<<FROM_USER_SECRET>>",
    
    "appsettings_section_name": {
          "Authority": "{KeycloakUrl}{RealmName}",
          "Audience": "<<Audience>>",
          "NameClaim: "<<NameOfClaimWhichShouldBeSetToNameClaim>>"
       }
}
```
### Authorization {#authorization}

#### Add to program.cs of your api

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












