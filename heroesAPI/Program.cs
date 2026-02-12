using heroesAPI.Data;
using heroesAPI.Models;
using Microsoft.AspNetCore.Mvc; // Necesario para [FromBody]
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

/// <summary>
/// Configuración principal de la aplicación HeroesAPI.
/// Este archivo define los servicios, el pipeline HTTP y los endpoints de la API.
/// </summary>
/// <author>Silvia Balmaseda & Rafael Robles</author>

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configuración de servicios necesarios para la aplicación.
/// Incluye la configuración de Swagger para la documentación y el DbContext para la base de datos.
/// </summary>
/// <author>Silvia Balmaseda</author>

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



/// <summary>
/// Endpoints para obtener todos los personajes registrados y para añadir nuevos personajes de cada tipo (Guerrero, Mago, Arquero, Clérigo).
/// Estos endpoints devuelven una lista polimórfica que incluye todas las subclases de Personaje.
/// (lógica que normalmente estaría en controllers)
/// </summary>
/// <returns>Lista de personajes (Guerreros, Magos, Arqueros, Clérigos).</returns>
/// <author>Silvia Balmaseda</author>


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

/// <summary>
/// Put: Endpoint para actualizar los datos de un personaje existente.
/// Permite modificar campos simples y el campo JSON "Rasgos".
/// </summary>
/// <param name="id">ID del personaje a actualizar.</param>
/// <param name="body">Objeto JSON con los datos a actualizar.</param>
/// <returns>Código 204 si la actualización fue exitosa, o 404 si el personaje no existe.</returns>
/// <author>Silvia Balmaseda</author>


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

/// <summary>
/// Endpoint para eliminar un personaje por su ID.
/// </summary>
/// <param name="id">ID del personaje a eliminar.</param>
/// <returns>Código 204 si la eliminación fue exitosa, o 404 si el personaje no existe.</returns>
/// <author>Rafael Robles</author>

app.MapDelete("/api/personajes/{id}", async (int id, AppDbContext db) =>
{
    var personaje = await db.Personajes.FindAsync(id);
    if (personaje is null) return Results.NotFound();

    db.Personajes.Remove(personaje);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// 4. CONSULTAS COMPLEJAS 

/// <summary>
/// Endpoint para buscar personajes cuyo campo JSON "Rasgos" contenga un miedo específico.
/// </summary>
/// <param name="miedo">El miedo a buscar en el campo JSON "Rasgos".</param>
/// <returns>Lista de personajes que tienen el miedo especificado, o un código 404 si no se encuentran coincidencias.</returns>
/// <author>Rafael Robles</author>

app.MapGet("/api/consultas/buscar-miedo", async (string miedo, AppDbContext db) =>
{
    // PASO 1: Obtener solo los IDs usando SQL directo (Consultando el JSONB)
    // Usamos SqlQuery<int> porque devolver un tipo simple (int) SÍ está permitido
    var ids = await db.Database
        .SqlQuery<int>($@"
            SELECT ""Id""
            FROM ""heroescodefirst"".""Personajes""
            WHERE ""Rasgos"" ->> 'MiedoA' = {miedo}")
        .ToListAsync();

    // Si no hay nadie, terminamos rápido
    if (!ids.Any()) return Results.NotFound("No se encontraron personajes con ese miedo.");

    // PASO 2: Usar EF Core para cargar los objetos completos (Polimorfismo TPT)
    // Al usar 'Contains', EF Core generará automáticamente los JOINS necesarios
    var result = await db.Personajes
        .Where(p => ids.Contains(p.Id))
        .ToListAsync();

    return Results.Ok(result);
});

/// <summary>
/// Endpoint para obtener Magos y Clérigos con nivel mayor a 50.
/// Los resultados se ordenan de forma descendente por nivel.
/// </summary>
/// <returns>Lista de Magos y Clérigos de alto nivel.</returns>
/// <author>Rafael Robles</author>

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

