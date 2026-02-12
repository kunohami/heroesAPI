using Microsoft.EntityFrameworkCore;
using heroesAPI.Models;

namespace heroesAPI.Data;


/// <summary>
/// Contexto de la base de datos para la aplicación HeroesAPI.
/// Configura las entidades y su mapeo a las tablas de la base de datos.
/// </summary>
/// <author>Silvia Balmaseda & Rafael Robles</author>

public class AppDbContext : DbContext
{
    // Constructor sin parámetros (usado en pruebas o configuraciones manuales)
    public AppDbContext() : base()
    {
    }

    // Constructor que recibe opciones de configuración (usado en la inyección de dependencias)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Guarda los cambios en la base de datos y captura excepciones específicas de EF Core.
    /// </summary>
    /// <returns>El número de registros escritos en la base de datos.</returns>
    /// <exception cref="Exception">Lanza una excepción genérica con detalles del error original.</exception>
    /// <author>Rafael Robles</author>
    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            // Captura errores de base de datos (Unique constraints, FKs, etc.)
            throw new Exception($"Error de base de datos al guardar cambios: {ex.InnerException?.Message ?? ex.Message}", ex);
        }
        catch (Exception ex)
        {
            // Captura cualquier otro error inesperado
            throw new Exception($"Error inesperado al guardar datos: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Versión asíncrona de SaveChanges con manejo de excepciones.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El número de registros escritos en la base de datos.</returns>
    /// <author>Rafael Robles</author>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception($"Error de base de datos al guardar cambios (Async): {ex.InnerException?.Message ?? ex.Message}", ex);
        }
        catch (OperationCanceledException)
        {
            // Si la operación fue cancelada, es normal relanzarla.
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inesperado al guardar datos (Async): {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Configura el modelo de datos y las relaciones entre entidades.
    /// </summary>
    /// <author>Rafael Robles</author>

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Establece el esquema predeterminado para las tablas
        modelBuilder.HasDefaultSchema("heroescodefirst");

        // Configuración para que la propiedad "Rasgos" sea almacenada como JSONB en PostgreSQL
        modelBuilder.Entity<Personaje>()
            .Property(p => p.Rasgos)
            .HasColumnType("jsonb");

        // Configuración explícita para usar TPT (Table-Per-Type)
        // Cada subclase de "Personaje" tendrá su propia tabla
        modelBuilder.Entity<Personaje>().ToTable("Personajes");
        modelBuilder.Entity<Guerrero>().ToTable("Guerreros");
        modelBuilder.Entity<Mago>().ToTable("Magos");
        modelBuilder.Entity<Arquero>().ToTable("Arqueros");
        modelBuilder.Entity<Clerigo>().ToTable("Clerigos");
    }


    /// <summary>
    /// DbSets que representan las tablas de la base de datos para cada entidad.
    /// </summary>
    /// <author>Silvia Balmaseda</author>

    public DbSet<Personaje> Personajes { get; set; }

    // Tablas específicas para cada tipo de personaje
    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }
}

