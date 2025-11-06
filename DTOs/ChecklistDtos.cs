
using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs;

public class ChecklistItemDto
{
    public int Id { get; set; }
    public int ChecklistId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Concluido { get; set; } = false;
}

public class ChecklistDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public ChecklistTipo Tipo { get; set; }
    public List<ChecklistItemDto> Itens { get; set; } = new();
}

public class CreateChecklistDto
{
    [Required]
    public ChecklistTipo Tipo { get; set; }
    public List<string> ItensDescricao { get; set; } = new(); // Descrições dos itens a serem criados
}

public class UpdateChecklistItemDto
{
    public bool Concluido { get; set; }
}
