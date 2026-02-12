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

