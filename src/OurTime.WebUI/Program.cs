using System;
using dotenv.net;                             // För .env-stöd (om du vill lagra hemligheter lokalt)
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;          // Viktigt för UseSqlServer()
using Microsoft.OpenApi.Models;
using OurTime.Infrastructure;
using OurTime.WebUI.Data;
using OurTime.WebUI.Services;
using OurTime.Application;


DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();


// 1) EF Core mot Azure-databasen
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(
        // Hämta connection string från appsettings.json,
        // ersätt sedan platshållare med miljövariabler.
        (builder.Configuration.GetConnectionString("DefaultConnection") ?? "")
            .Replace("{AZURE_SQL_USER}", Environment.GetEnvironmentVariable("AZURE_SQL_USER") ?? "")
            .Replace("{AZURE_SQL_PASSWORD}", Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD") ?? "")
            .Replace("{AZURE_SQL_SERVER}", Environment.GetEnvironmentVariable("AZURE_SQL_SERVER") ?? "")
            .Replace("{AZURE_SQL_DATABASE}", Environment.GetEnvironmentVariable("AZURE_SQL_DATABASE") ?? "")
    )
);


// 2) Cookie‐autentisering för MVC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

// 3) HttpClient för externa Review-API:t
builder.Services.AddHttpClient<ReviewApiService>(client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("REVIEW_ENGINE_URL"));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("X-Api-Key", Environment.GetEnvironmentVariable("REVIEW_ENGINE_API_KEY"));
});

// 4) Registrera API‐controllers + Razor‐views
builder.Services.AddControllers();          // [ApiController]–endpoints
builder.Services.AddControllersWithViews(); // Razor‐views

// 5) Swagger/OpenAPI för Watches API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Watches API V1",
        Version = "v1"
    });
});

var app = builder.Build();

// 6) Middleware: statiska filer, routing, auth
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 7) Proxy‐endpoint för externa ReviewEngine‐swagger

app.MapGet("/swagger-external/swagger.json", async (IHttpClientFactory http) =>
{
    var client = http.CreateClient(nameof(ReviewApiService));
    var json = await client.GetStringAsync("/v3/api-docs");
    return Results.Content(json, "application/json");
});

// 8) Swagger + UI (ENDAST i Development)

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Watches API V1");
        c.SwaggerEndpoint("/swagger-external/swagger.json", "ReviewEngine API");
        c.RoutePrefix = "swagger";
    });
}


// 9) Map controllers + standard MVC‐route
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();