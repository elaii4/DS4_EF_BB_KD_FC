// Program.cs
// Punto de entrada y configuración de la API REST ASP.NET Core 8.
// Registra servicios, DbContext en PostgreSQL, Swagger y canal de middleware.
using Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // Inicializa el constructor de la aplicación web

// Asegurar puerto 5144 en Windows para evitar conflicto con svchost/servicios de Windows en el puerto 5000
if (Environment.GetEnvironmentVariable("ASPNETCORE_URLS") == null && builder.Configuration["urls"] == null)
{
    builder.WebHost.UseUrls("http://localhost:5144");
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration["ConnectionStrings__Default"]
    ?? throw new InvalidOperationException("Cadena de conexión 'Default' no configurada.");

builder.Services.AddDbContext<AppDbContext>(options => // Configura el contexto de datos con Npgsql
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();

// Configuración de CORS para permitir solicitudes desde Blazor WebAssembly / navegadores
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CrudWindowsForms API",
        Version = "v1",
        Description = "API REST en 4 capas para la gestion de Personas con EF Core y PostgreSQL."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrudWindowsForms API v1");
    });
}

// Global Exception handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Title = "Error interno del servidor",
            Detail = "Ocurrió un error inesperado al procesar la solicitud.",
            Status = StatusCodes.Status500InternalServerError
        });
    });
});

// Habilitar política CORS antes del enrutamiento de controladores
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
