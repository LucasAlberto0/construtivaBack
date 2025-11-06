
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public class DiarioObra
{
    public int Id { get; set; }
    public int ObraId { get; set; }

    [Required]
    public DateTime Data { get; set; }
    public string? Clima { get; set; }
    public string? EquipePresente { get; set; } // Simplificado por enquanto
    public string? AtividadesRealizadas { get; set; }
    public string? ComentariosTecnicos { get; set; }

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
    public virtual ICollection<string> Fotos { get; set; } = new List<string>(); // Paths para as fotos
}
