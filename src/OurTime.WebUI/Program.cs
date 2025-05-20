using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using OurTime.WebUI.Data;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using dotenv.net; // För .env-stöd

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(); // Ladda miljövariabler från .env

// Lägg till EF Core med connection string från appsettings.json + .env-ersättning
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
            .Replace("{AZURE_SQL_USER}", Environment.GetEnvironmentVariable("AZURE_SQL_USER"))
            .Replace("{AZURE_SQL_PASSWORD}", Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD"))
            .Replace("{AZURE_SQL_SERVER}", Environment.GetEnvironmentVariable("AZURE_SQL_SERVER"))
            .Replace("{AZURE_SQL_DATABASE}", Environment.GetEnvironmentVariable("AZURE_SQL_DATABASE"))
    ));

// Lägg till cookie-baserad inloggning
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

// Lägg till MVC och Swagger
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Review API V1",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Review API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
