
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public enum ObraStatus
{
    EmAndamento,
    Manutencao,
    Suspenso,
    Finalizado
}

public class Obra
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Localizacao { get; set; } = string.Empty;

    [Required]
    public string Contratante { get; set; } = string.Empty;

    public string? Contrato { get; set; }
    public string? OrdemDeServico { get; set; }

    public ObraStatus Status { get; set; }

    // Navigation Properties
    public virtual ICollection<ApplicationUser> Equipe { get; set; } = new List<ApplicationUser>();
    public virtual ICollection<DiarioObra> Diarios { get; set; } = new List<DiarioObra>();
    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
    public virtual ICollection<Manutencao> Manutencoes { get; set; } = new List<Manutencao>();
}
