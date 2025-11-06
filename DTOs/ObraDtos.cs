
using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs;

public class ObraDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Localizacao { get; set; } = string.Empty;
    public string Contratante { get; set; } = string.Empty;
    public string? Contrato { get; set; }
    public string? OrdemDeServico { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateObraDto
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Localizacao { get; set; } = string.Empty;

    [Required]
    public string Contratante { get; set; } = string.Empty;

    public string? Contrato { get; set; }
    public string? OrdemDeServico { get; set; }
    
    public ObraStatus Status { get; set; } = ObraStatus.EmAndamento;
}
