
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class AditivoDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public string? AnexoPath { get; set; }
    public bool Aprovado { get; set; } = false;
    public DateTime? DataAprovacao { get; set; }
    public string? AprovadoPorUserId { get; set; }
}

public class CreateAditivoDto
{
    [Required]
    public string Descricao { get; set; } = string.Empty;
    // Anexo será enviado via outro endpoint ou como IFormFile
}

public class AprovarAditivoDto
{
    [Required]
    public bool Aprovar { get; set; }
}
