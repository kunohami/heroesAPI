using Microsoft.EntityFrameworkCore;
using heroesAPI.Models;

namespace heroesAPI.Data;

// Clase que representa el contexto de la base de datos
// Aquí se configuran las entidades y su mapeo a las tablas de la base de datos
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

    // Método para configurar el modelo de datos y las relaciones
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

    // DbSet representa una tabla en la base de datos
    // Tabla base para todos los personajes
    public DbSet<Personaje> Personajes { get; set; }

    // Tablas específicas para cada tipo de personaje
    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }
}

