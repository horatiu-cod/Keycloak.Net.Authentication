using ConfigurationSubstitution;
using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Configuration.EnableSubstitutions();

builder.Services
        .AddKeycloakAuthentication()
        .AddKeycloakJwtBearerOptions("Keycloak")
        .AddUma("ResourceClient", configure =>
        {
            configure.AddPolicy("email_verified", configure =>
            {
                configure.RequireClaim("email_verified", "true");
            });
            configure.AddPolicy("name", policy =>
            {
                policy.RequireUserName("hg@g.com");
            });
            configure.AddPolicy("auth", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
            configure.AddPolicy("role", policy =>
            {
                policy.RequireRole("admin-role");
            });
        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


#region Authentication
app.MapGet("api/authenticate", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization();
 
app.MapGet("api/custompolicy", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization("email_verified");

app.MapGet("api/attribute",[Authorize(Roles = "admin-role")] () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"));

app.MapGet("api/role", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization("role");

app.MapGet("api/auth", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization("auth");

app.MapGet("api/name", () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization("name");
#endregion

#region Authorization
// Get the ResourceClient from appsettings.json
app.MapGet("api/uma1", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireUmaAuthorization(resource: "workspace", scope: "write");

// Testing authorization based on ResourceClient roles
app.MapGet("api/uma2", [Permission("workspace", "read", Roles = "write-access")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized")).ResourceClient("backend");

// Testing authorization based ResourceClient Permissions/Policies
app.MapGet("api/uma3", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("urn:workspace:write").ResourceClient("backend");
#endregion
app.MapGet("redirect", (string session_state, string code) =>
{
    Results.Ok($"{code} {session_state}");
});

app.Run();
