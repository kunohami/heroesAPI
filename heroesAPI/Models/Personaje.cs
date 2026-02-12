using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace heroesAPI.Models;

/// <summary>
/// Representa un personaje base en el sistema.
/// </summary>
/// <remarks>
/// Esta clase es abstracta y sirve como base para las clases derivadas.
/// </remarks>
/// <author>Silvia Balsmaseda</author>

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


/// <summary>
/// Tablasderivadas para cada tipo de personaje (Guerrero, Mago, Arquero, Clerigo).
/// </summary>
/// <remarks>
/// Se han definido las clases derivadas (Guerrero, Mago, Arquero, Clerigo)
/// para representar diferentes tipos de personajes con sus propias propiedades específicas. Cada clase hija hereda de la clase base "Personaje", 
/// lo que permite compartir las propiedades comunes y agregar características únicas a cada tipo de personaje. 
/// </remarks>
/// <author>Rafael Robles</author>
// se pueden quitar las etiquetas table de cada clase hija, ya que se ha forzado el TPT en el OnModelCreating del DbContext,

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


