using Microsoft.EntityFrameworkCore;

namespace heroesAPI.Data;
public class AppDbContext : DbContext
{
    public AppDbContext() : base()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // OnModelCreating


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql("Host=localhost;Port=5441;Database=heroesapi;SearchPath=heroescodefirst;Username=heroesuser;Password=Abcd1234");

    }

    // public DbSet<Personaje> Personajes { get; set; }


}

