using Microsoft.EntityFrameworkCore;
using FootballStats.Api.Data;
using FootballStats.Api.Endpoints;
using FootballStats.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow Vue.js front-end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:3000" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Entity Framework Core with PostgreSQL
// Supports DATABASE_URL env var (used by Render/Supabase) or appsettings connection string
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Force IPv4 resolution to avoid IPv6 unreachable errors on Render
        var connStr = databaseUrl;
        if (!connStr.Contains("Resolve=", StringComparison.OrdinalIgnoreCase))
        {
            connStr = connStr.TrimEnd(';') + ";Resolve=ipv4";
        }
        options.UseNpgsql(connStr);
    }
    else
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
});

// Register service layer via native DI container
builder.Services.AddScoped<IJogadorService, JogadorService>();
builder.Services.AddScoped<IRegistroPartidaService, RegistroPartidaService>();
builder.Services.AddScoped<IEstatisticaService, EstatisticaService>();

var app = builder.Build();

// Auto-apply migrations on startup (safe for single-instance deployments)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Global exception handler middleware - returns JSON without exposing internal details
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { mensagem = "Erro interno do servidor" });
    });
});

app.UseCors("AllowFrontend");

// Map API endpoint groups
app.MapJogadoresEndpoints();
app.MapRegistrosEndpoints();
app.MapEstatisticasEndpoints();
app.MapGet("/", () => Results.Ok(new { mensagem = "Football Stats Tracker API" }));

app.Run();

// Make the implicit Program class accessible for integration tests
public partial class Program { }
