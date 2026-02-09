using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc; 
using System.Text.Json;
using heroesAPI.Data;
using heroesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuraci�n de Servicios (Swagger y DB)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectar el DbContext leyendo la conexi�n del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// 2. Configuraci�n del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. DEFINICI�N DE ENDPOINTS (Aqu� est� toda la l�gica, sin Controllers)

// --- GET: Obtener todos (Polimorfismo: trae magos, guerreros, etc.) ---
app.MapGet("/api/personajes", async (AppDbContext db) =>
{
    return await db.Personajes.ToListAsync();
})
.WithName("ObtenerTodosPersonajes");

// --- GET: Obtener por ID ---
app.MapGet("/api/personajes/{id}", async (int id, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    return personaje is not null ? Results.Ok(personaje) : Results.NotFound();
})
.WithName("ObtenerPersonajePorId");

// --- POST: Crear Guerrero ---
app.MapPost("/api/personajes/guerrero", async (Guerrero guerrero, AppDbContext db) =>
{
    db.Guerreros.Add(guerrero);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{guerrero.Id}", guerrero);
});

// --- POST: Crear Mago ---
app.MapPost("/api/personajes/mago", async (Mago mago, AppDbContext db) =>
{
    db.Magos.Add(mago);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{mago.Id}", mago);
});

// --- POST: Crear Arquero ---
app.MapPost("/api/personajes/arquero", async (Arquero arquero, AppDbContext db) =>
{
    db.Arqueros.Add(arquero);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{arquero.Id}", arquero);
});

// --- POST: Crear Clerigo ---
app.MapPost("/api/personajes/clerigo", async (Clerigo clerigo, AppDbContext db) =>
{
    db.Clerigos.Add(clerigo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{clerigo.Id}", clerigo);
});

// --- PUT: Actualizar (Incluyendo el JSON) ---
app.MapPut("/api/personajes/{id}", async (int id, [FromBody] JsonElement body, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    if (personaje is null) return Results.NotFound();

    // Actualizamos campos simples si existen en el JSON enviado
    if (body.TryGetProperty("nombre", out var nombreVal))
        personaje.Nombre = nombreVal.GetString()!;

    if (body.TryGetProperty("nivel", out var nivelVal))
        personaje.Nivel = nivelVal.GetInt32();

    // Actualizamos el JSON (Rasgos)
    // Convertimos el objeto JSON recibido a String para guardarlo en la propiedad string
    if (body.TryGetProperty("rasgos", out var rasgosJson))
    {
        personaje.Rasgos = rasgosJson.GetRawText();
    }

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// --- DELETE: Borrar ---
app.MapDelete("/api/personajes/{id}", async (int id, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    if (personaje is null) return Results.NotFound();

    db.Personajes.Remove(personaje);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
