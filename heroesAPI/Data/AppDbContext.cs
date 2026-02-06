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

        // Aquí configuraremos el JSON más adelante cuando descomentes la propiedad en el modelo
        /*
        modelBuilder.Entity<Personaje>()
            .Property(p => p.Rasgos)
            .HasColumnType("jsonb");
        */
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql("Host=localhost;Port=5441;Database=heroesapi;SearchPath=heroescodefirst;Username=heroesuser;Password=Abcd1234");
    }

    public DbSet<Personaje> Personajes { get; set; }

    public DbSet<Guerrero> Guerreros { get; set; }
    public DbSet<Mago> Magos { get; set; }
    public DbSet<Arquero> Arqueros { get; set; }
    public DbSet<Clerigo> Clerigos { get; set; }

}

