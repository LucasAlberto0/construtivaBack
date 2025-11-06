
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class DiarioObraDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public DateTime Data { get; set; }
    public string? Clima { get; set; }
    public string? EquipePresente { get; set; }
    public string? AtividadesRealizadas { get; set; }
    public string? ComentariosTecnicos { get; set; }
    public List<string> Fotos { get; set; } = new();
}

public class CreateDiarioObraDto
{
    [Required]
    public DateTime Data { get; set; }
    public string? Clima { get; set; }
    public string? EquipePresente { get; set; }
    public string? AtividadesRealizadas { get; set; }
    public string? ComentariosTecnicos { get; set; }
}
