using heroesAPI.Data;
using heroesAPI.Models;
using Microsoft.AspNetCore.Mvc; // Necesario para [FromBody]
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de Servicios (Swagger y DB)
// Aquí se configuran los servicios que la aplicación necesita, como Swagger para la documentación
// y el DbContext para interactuar con la base de datos.
builder.Services.AddEndpointsApiExplorer();
object value = builder.Services.AddSwaggerGen();

// Configuración de la conexión a la base de datos
// La cadena de conexión se lee desde el archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// 2. Configuración del Pipeline HTTP
// Aquí se configuran los middleware que manejarán las solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    // Swagger solo se habilita en el entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(); // Interfaz gráfica para explorar la API
}

app.UseHttpsRedirection(); // Redirige automáticamente las solicitudes HTTP a HTTPS

// 3. DEFINICIÓN DE ENDPOINTS (lógica que normalmente estaría en controllers)
// Aquí se definen los endpoints de la API. Cada endpoint maneja una ruta específica y una acción.

// --- GET: Obtener todos los personajes (Polimorfismo: trae magos, guerreros, etc.) ---
// Este endpoint devuelve todos los personajes, sin importar su tipo.
app.MapGet("/api/personajes", async (AppDbContext db) =>
{
    return await db.Personajes.ToListAsync();
})
.WithName("ObtenerTodosPersonajes");

// --- GET: Obtener un personaje por ID ---
// Este endpoint busca un personaje específico por su ID.
app.MapGet("/api/personajes/{id}", async (int id, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    return personaje is not null ? Results.Ok(personaje) : Results.NotFound();
})
.WithName("ObtenerPersonajePorId");

// --- POST: Crear un Guerrero ---
// Este endpoint permite crear un nuevo Guerrero.
app.MapPost("/api/personajes/guerrero", async (Guerrero guerrero, AppDbContext db) =>
{
    db.Guerreros.Add(guerrero);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{guerrero.Id}", guerrero);
});

// --- POST: Crear un Mago ---
// Este endpoint permite crear un nuevo Mago.
app.MapPost("/api/personajes/mago", async (Mago mago, AppDbContext db) =>
{
    db.Magos.Add(mago);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{mago.Id}", mago);
});

// --- POST: Crear un Arquero ---
// Este endpoint permite crear un nuevo Arquero.
app.MapPost("/api/personajes/arquero", async (Arquero arquero, AppDbContext db) =>
{
    db.Arqueros.Add(arquero);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{arquero.Id}", arquero);
});

// --- POST: Crear un Clérigo ---
// Este endpoint permite crear un nuevo Clérigo.
app.MapPost("/api/personajes/clerigo", async (Clerigo clerigo, AppDbContext db) =>
{
    db.Clerigos.Add(clerigo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/personajes/{clerigo.Id}", clerigo);
});

// --- PUT: Actualizar un personaje (Incluyendo el JSON) ---
// Este endpoint permite actualizar los datos de un personaje existente, incluyendo su campo JSON "Rasgos".
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

// --- DELETE: Borrar un personaje ---
// Este endpoint permite eliminar un personaje por su ID.
app.MapDelete("/api/personajes/{id}", async (int id, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    if (personaje is null) return Results.NotFound();

    db.Personajes.Remove(personaje);
    await db.SaveChangesAsync();
    return Results.NoContent();
});



// 4. CONSULTAS COMPLEJAS 
// Aquí se definen consultas avanzadas que cumplen con los requisitos del enunciado.

// Opción A: Buscar personajes que tengan un miedo específico en el JSON
// Este endpoint busca personajes cuyo campo JSON "Rasgos" contenga un miedo específico.
app.MapGet("/api/consultas/buscar-miedo", async (string miedo, AppDbContext db) =>
{
    // Use FromSqlInterpolated para pasar el parámetro de forma segura
    var result = await db.Personajes
        .FromSqlInterpolated($@"SELECT * FROM ""heroescodefirst"".""Personajes""
                               WHERE ""Rasgos"" ->> 'MiedoA' = {miedo}")
        .ToListAsync();

    return result.Any() ? Results.Ok(result) : Results.NotFound("No se encontraron personajes con ese miedo.");
});

// Opción B: Obtener Magos y Clérigos de alto nivel (Multitabla)
// Este endpoint devuelve una lista de Magos y Clérigos con nivel mayor a 50, ordenados por nivel.
app.MapGet("/api/consultas/magos-clerigos-top", async (AppDbContext db) =>
{
    var magos = await db.Magos.Where(m => m.Nivel > 50).Cast<Personaje>().ToListAsync();
    var clerigos = await db.Clerigos.Where(c => c.Nivel > 50).Cast<Personaje>().ToListAsync();

    var union = magos.Concat(clerigos)
        .OrderByDescending(p => p.Nivel)
        .ToList();

    return Results.Ok(union);
});

app.Run();

