using Microsoft.EntityFrameworkCore;
using heroesAPI.Models;

namespace heroesAPI.Data;
public class AppDbContext : DbContext
{
    public AppDbContext() : base()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("heroescodefirst");

        // configuración del JSON 
        
        modelBuilder.Entity<Personaje>()
            .Property(p => p.Rasgos)
            .HasColumnType("jsonb");

        // Forzar TPT (Table-Per-Type) explícitamente: cada tipo tiene su propia tabla.
        // La tabla base para Personaje y tablas separadas para cada subclase.
        modelBuilder.Entity<Personaje>().ToTable("Personajes");
        modelBuilder.Entity<Guerrero>().ToTable("Guerreros");
        modelBuilder.Entity<Mago>().ToTable("Magos");
        modelBuilder.Entity<Arquero>().ToTable("Arqueros");
        modelBuilder.Entity<Clerigo>().ToTable("Clerigos");

    }

    public DbSet<Personaje> Personajes { get; set; }

    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }

}

