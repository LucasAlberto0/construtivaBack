
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public class Documento
{
    public int Id { get; set; }
    public int ObraId { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Path { get; set; } = string.Empty;
    public string Pasta { get; set; } = "Geral"; // Ex: Contratos, Projetos, Relatórios
    public int Versao { get; set; } = 1;
    public bool IsDeleted { get; set; } = false; // Soft delete

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
}
