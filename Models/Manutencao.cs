
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public class Manutencao
{
    public int Id { get; set; }
    public int ObraId { get; set; }

    [Required]
    public DateTime DataRegistro { get; set; }
    public string Descricao { get; set; } = string.Empty;

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
    public virtual ICollection<string> Imagens { get; set; } = new List<string>(); // Paths para as imagens
}
