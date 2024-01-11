# Keycloak .Net Authentication
[![Build](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/horatiu-cod/Keycloak.Net.Authentication/actions/workflows/build.yml)

Authentication with Keycloak in .NET and ASP.NET Core

Secure your api with Keycloak JWT bearer authentication

Api calls requires auhorization header with an JWT token from Keycloak.
```curl
POST https://yourapi/action HTTP/1.1
Auhorization: Bearer JwtTokenContent
```

## How to use
Add to program.cs of your api
#### Option no.1 (easiest way)
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions("appsettings_section_name");
.....
app.UseAuthentication();
app.UseAuthorization();

```
Add section to the appsettings.{Environment}.json
- Authority is required. If not set will throw exception during app startup
- Set the audience as "Audience", "ValidAudience" or "ValidAudiences"
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
- Authority is required. If not set will throw exception during app startup
- Set the audience as "Audience", "ValidAudience" or "ValidAudiences"
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions(options =>
    {
        options.Authority = "https://{host}/realms/{realm}";
        options.Audience = "<<Audience>>";
    })

```
#### Option no.4
- You have to manually configure the JwtBearerOtions
```csharp
builder.Services
  .AddKeyCloakAuthentication()
  .AddKeyCloakJwtBearerOptions(options =>
    {
        options.Authority = "https://{host}/realms/{realm}";
        options.Audience = "<<Audience>>";
    })
```
### JWT transformation
- Under the hood the Keyclaok JWT is mapped and transformed to Identity JWT
- The Keycloak Realm and Client "roles" claims are mapped and tranformed to ClaimType.Role
- By default the Keycloak "preferred_username" claim is transformed to ClaimType.Name. Yuo can change it by just adding the following:
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













