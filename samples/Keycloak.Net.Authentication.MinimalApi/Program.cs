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
    .AddKeyCloakAuthentication()
    .AddKeyCloakJwtBearerOptions("Appsettings_section_name")


    //.AddKeyCloakJwtBearerOptions("Appsettings_section_name", o =>
    //{
    //    o.Authority = "https://localhost:8843/realms/Test";
    //    //o.Audience = "client-role";
    //    o.SaveToken = true;

    //    o.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(30);
    //})
    //.AddKeyCloakJwtBearerOptions(c =>
    //{
    //    c.Authority = "https://localhost:8843/realms/Test";
    //    c.ValidAudience = "client-role";
    //})
    //.AddJwtBearerOptions(options =>
    //{
    //    options.Authority = "https://localhost:8843/realms/Test";
    //    //options.Audience = "client-role";
    //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    //    {
    //        ValidAudiences = ["client-role"]
    //    };
    //})
    //.AddUma(client =>
    //{
    //    client.ClientId = "client-role";
    //})
    .AddUma("Resource",configure =>
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
    })
    //.AddAuthorization(configure =>
    //{
    //    configure.AddPolicy("email_verified", configure =>
    //    {
    //        configure.RequireClaim("email_verified", "true");
    //    });
    //    configure.AddPolicy("name", policy =>
    //    {
    //        policy.RequireUserName("h@g.com");
    //    });
    //    configure.AddPolicy("auth", policy =>
    //    {
    //        policy.RequireAuthenticatedUser();
    //    });
    //    configure.AddPolicy("role", policy =>
    //    {
    //        policy.RequireRole("user");
    //    });
    //})
    ;


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



app.MapGet("api/authenticate", [ClientName("public-client")] () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization();

app.MapGet("api/authorize", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("email_verified").ForClient("public-client");

app.MapGet("api/attribute",[Authorize(Roles = "admin-role")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized")).ForClient("public-client");

app.MapGet("api/role", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("role").ForClient(clientName: "public-client");

app.MapGet("api/auth", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("auth").ForClient("public-client");

app.MapGet("api/name", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("name").ForClient("public-client");

app.MapGet("api/uma1", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireUmaAuthorization(resource: "workspaces", scope: "all");//.ForClient("webapp");

app.MapGet("api/uma2", [Permission("workspaces", "all", Roles = "admin-role")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized")).ForClient("webapp");

app.MapGet("api/uma3", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("urn:workspaces:all").ForClient("webapp");

app.MapGet("redirect", (string session_state, string code) =>
{
    Results.Ok($"{code} {session_state}");
});

app.MapPost("api/register", () =>
{
    Results.Ok("Registered");
});

app.Run();
public partial class Program { }


