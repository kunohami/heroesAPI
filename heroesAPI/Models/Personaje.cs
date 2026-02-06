using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace heroesAPI.Models;

[Table("Personajes")]
public abstract class Personaje
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, 100)]
    public int Nivel { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public string? Gremio { get; set; } 

    [Column(TypeName = "jsonb")]
    public string? Rasgos { get; set; }
}

[Table("Guerreros")]
public class Guerrero : Personaje
{
    public string ArmaPrincipal { get; set; } = string.Empty;
    public int Furia { get; set; }
}

[Table("Magos")]
public class Mago : Personaje
{
    public int Mana { get; set; }
    public string ElementoPrincipal { get; set; } = string.Empty;
}

[Table("Arqueros")]
public class Arquero : Personaje
{
    public double Precision { get; set; }
    public bool TieneMascota { get; set; }
}

[Table("Clerigos")]
public class Clerigo : Personaje
{
    public string Deidad { get; set; } = string.Empty;
    public int PuntosSanacion { get; set; }
}


