
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class ManutencaoDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public DateTime DataRegistro { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public List<string> Imagens { get; set; } = new();
}

public class CreateManutencaoDto
{
    [Required]
    public DateTime DataRegistro { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
