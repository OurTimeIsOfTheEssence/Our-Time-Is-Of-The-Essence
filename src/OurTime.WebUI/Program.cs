using System;
using System.Net.Http.Headers;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OurTime.Application;
using OurTime.Infrastructure;
using OurTime.WebUI.Services;
using OurTime.Domain.Entities;
using OurTime.Infrastructure.Persistence;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// 1) Core services, Application + Infrastructure layers
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// 2) Build raw connection string with placeholders replaced by env vars
var rawConn = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("{AZURE_SQL_SERVER}",   Environment.GetEnvironmentVariable("AZURE_SQL_SERVER")!)
    .Replace("{AZURE_SQL_DATABASE}", Environment.GetEnvironmentVariable("AZURE_SQL_DATABASE")!)
    .Replace("{AZURE_SQL_USER}",     Environment.GetEnvironmentVariable("AZURE_SQL_USER")!)
    .Replace("{AZURE_SQL_PASSWORD}", Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD")!);

// 3) EF Core → Azure SQL, migrations in this project, retry + 60s timeout
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(rawConn, sql =>
    {
        sql.MigrationsAssembly("OurTime.WebUI");
        sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
        sql.CommandTimeout(60);
    })
);

// 4) Cookie‐based auth for MVC
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts =>
    {
        opts.LoginPath       = "/Account/Login";
        opts.LogoutPath      = "/Account/Logout";
        opts.ExpireTimeSpan  = TimeSpan.FromHours(1);
    });

// 5) AuthService: simple HttpClient for login + API-key generation
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("REVIEW_ENGINE_URL")!);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// 6) ReviewApiService + handler that injects JWT & API-key on each request
builder.Services.AddTransient<ReviewApiAuthHandler>();
builder.Services.AddHttpClient<ReviewApiService>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("REVIEW_ENGINE_URL")!);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<ReviewApiAuthHandler>();

// 7) MVC + Swagger/OpenAPI
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OurTime.WebUI", Version = "v1" });
});

var app = builder.Build();

// 8) Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 9) Proxy external ReviewEngine swagger JSON
app.MapGet("/swagger-external/swagger.json", async (IHttpClientFactory http) =>
{
    var json = await http
        .CreateClient(nameof(ReviewApiService))
        .GetStringAsync("/v3/api-docs");
    return Results.Content(json, "application/json");
});

// 10) Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",           "OurTime.WebUI v1");
        c.SwaggerEndpoint("/swagger-external/swagger.json",     "ReviewEngine API");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
