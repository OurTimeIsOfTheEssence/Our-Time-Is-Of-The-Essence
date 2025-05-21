using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OurTime.WebUI.Data;
using OurTime.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) EF Core mot Azure-databasen
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Cookie-auth för MVC
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// 3) HttpClient för externa Review-API:t
builder.Services.AddHttpClient<ReviewApiService>(client =>
{
    var url = builder.Configuration["ExternalApis:ReviewEngine"];
    client.BaseAddress = new Uri(url);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// 4) Registrera både API-controllers och Razor-views
builder.Services.AddControllers();           // [ApiController]-endpoints
builder.Services.AddControllersWithViews();  // Razor-Views

// 5) Swagger/OpenAPI för Watches API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Dokument-nyckel “v1” => ligger på /swagger/v1/swagger.json
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Watches API V1",
        Version = "v1"
    });
});

var app = builder.Build();

// 6) Middleware: statisk filservering, routing, auth
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 7) Proxy-endpoint för externa ReviewEngine-swagger
app.MapGet("/swagger-external/swagger.json", async (IHttpClientFactory http) =>
{
    var client = http.CreateClient(nameof(ReviewApiService));
    var json = await client.GetStringAsync("/v3/api-docs");
    return Results.Content(json, "application/json");
});

// 8) Swagger + UI (ENDAST i Development)
if (app.Environment.IsDevelopment())
{
    // Standard‐swagger på /swagger/v1/swagger.json
    app.UseSwagger();

    // Swagger UI på /swagger som visar både “v1” och “external”
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Watches API V1");
        c.SwaggerEndpoint("/swagger-external/swagger.json", "ReviewEngine API");
        c.RoutePrefix = "swagger";
    });
}

// 9) Mappa controllers + default MVC-route
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
