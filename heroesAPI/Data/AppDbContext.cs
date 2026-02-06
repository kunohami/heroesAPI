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

        // Aquí configuraremos el JSON 
        
        modelBuilder.Entity<Personaje>()
            .Property(p => p.Rasgos)
            .HasColumnType("jsonb");
        
    }

    public DbSet<Personaje> Personajes { get; set; }

    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }

}

