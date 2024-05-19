using ConfigurationSubstitution;
using Keycloak.Net.Authentication;
using Keycloak.Net.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    .AddUma(configure =>
    {
        configure.AddPolicy("email_verified", configure =>
        {
            configure.RequireClaim("email_verified", "true");
        });
        configure.AddPolicy("name", policy =>
        {
            policy.RequireUserName("h@g.com");
        });
        configure.AddPolicy("auth", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
        configure.AddPolicy("role", policy =>
        {
            policy.RequireRole("user");
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
app.UseUma();

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("api/authenticate", [ClientName("client-role")] () =>
    Results.Ok($"{HttpStatusCode.OK} authenticated"))
    .RequireAuthorization();

app.MapGet("api/authorize", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("email_verified").ForClient("client-role");

app.MapGet("api/attribute",[Authorize(Roles = "user")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized")).ForClient("client-role");

app.MapGet("api/role", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("role").ForClient(clientName: "client-role");

app.MapGet("api/auth", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("auth").ForClient("client-role");

app.MapGet("api/name", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("name").ForClient("client-role");

app.MapGet("api/uma1", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireUmaAuthorization(resource: "user-resource", scope: "user-scope").ForClient("client-role");

app.MapGet("api/uma2", [Permission(Resource = "user-resource", Scope = "user-scope", Roles = "user")] () =>
    Results.Ok($"{HttpStatusCode.OK} authorized")).ForClient("client-role");

app.MapGet("api/uma3", () =>
    Results.Ok($"{HttpStatusCode.OK} authorized"))
    .RequireAuthorization("Permission:user-resource:user-scope").ForClient("client-role");

app.MapGet("redirect", (string session_state, string code) =>
{
    Results.Ok($"{code} {session_state}");
});

app.MapPost("api/register", async () =>
{
});

app.Run();
public partial class Program { }


