
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public class Aditivo
{
    public int Id { get; set; }
    public int ObraId { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;
    public string? AnexoPath { get; set; } // RN003: Cada aditivo deve ter anexo
    public bool Aprovado { get; set; } = false;
    public DateTime? DataAprovacao { get; set; }
    public string? AprovadoPorUserId { get; set; }

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
    public virtual ApplicationUser? AprovadoPorUser { get; set; }
}
