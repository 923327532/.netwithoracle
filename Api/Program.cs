using Microsoft.EntityFrameworkCore;
using Taller.Datos;
using Taller.Datos.Repositorios;
using Taller.Negocio;
using Taller.Negocio.Excepciones;
using Taller.Negocio.ReglasNegocio;
using Taller.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Registrar DbContext (PostgreSQL / Neon).
builder.Services.AddDbContext<TallerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Database=lab12;Username=postgres;Password=postgres;"));

// Inyección de dependencias: repositorios, servicios y AutoMapper.
builder.Services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();
builder.Services.AddScoped<IOrdenTrabajoRepositorio, OrdenTrabajoRepositorio>();
builder.Services.AddScoped<IVehiculoServicio, VehiculoServicio>();
builder.Services.AddScoped<IOrdenTrabajoServicio, OrdenTrabajoServicio>();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Taller.Negocio.MappingProfile).Assembly));

builder.Services.AddControllers();

// Swagger / OpenAPI (solo DTOs, nunca entidades).
builder.Services.AddSwaggerGen();

// CORS configurado desde appsettings.json (sin autenticación).
var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "http://129.146.87.179:8080" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors", policy =>
        policy.WithOrigins(corsOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PoliticaCors");

app.MapControllers();

app.Run();
