using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace heroesAPI.Models;

[Table("Personajes")]
public abstract class Personaje
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;
    [Range(1, 100)]
    public int Nivel { get; set; }
    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public string? Gremio { get; set; }

    // El campo JSON lo añadiremos posteriormente
}


