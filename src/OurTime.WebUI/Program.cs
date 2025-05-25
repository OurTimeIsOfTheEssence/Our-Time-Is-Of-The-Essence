using System;
using System.Net.Http.Headers;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OurTime.Application;
using OurTime.Infrastructure;
using OurTime.WebUI.Data;
using OurTime.WebUI.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// 1) Core‐tjänster + EF Core
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var rawConn = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("{AZURE_SQL_SERVER}", Environment.GetEnvironmentVariable("AZURE_SQL_SERVER")!)
    .Replace("{AZURE_SQL_DATABASE}", Environment.GetEnvironmentVariable("AZURE_SQL_DATABASE")!)
    .Replace("{AZURE_SQL_USER}", Environment.GetEnvironmentVariable("AZURE_SQL_USER")!)
    .Replace("{AZURE_SQL_PASSWORD}", Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD")!);

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(rawConn, sql =>
    {
        sql.MigrationsAssembly("OurTime.WebUI");
        sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        sql.CommandTimeout(60); // 60 sekunders timeout
    })
);

// 2) Cookie‐baserad autentisering
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opts =>
    {
        opts.LoginPath = "/Account/Login";
        opts.LogoutPath = "/Account/Logout";
        opts.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

// 3) AuthService (för inloggning + API‐key)
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("REVIEW_ENGINE_URL")!);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// 4) ReviewApiService + JWT/API‐key‐handler
builder.Services.AddTransient<ReviewApiAuthHandler>();

builder.Services.AddHttpClient<ReviewApiService>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("REVIEW_ENGINE_URL")!);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<ReviewApiAuthHandler>();

// 5) MVC + Swagger
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OurTime.WebUI", Version = "v1" })
);

var app = builder.Build();

// 6) Middleware‐pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 7) Proxy för externa ReviewEngine-swagger
app.MapGet("/swagger-external/swagger.json", async (IHttpClientFactory http) =>
{
    var json = await http
                 .CreateClient(nameof(ReviewApiService))
                 .GetStringAsync("/v3/api-docs");
    return Results.Content(json, "application/json");
});

// 8) Swagger UI (endast i Dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OurTime.WebUI v1");
        c.SwaggerEndpoint("/swagger-external/swagger.json", "ReviewEngine API");
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
