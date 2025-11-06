
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public enum ChecklistTipo
{
    InicioObra,
    EntregaObra
}

public class Checklist
{
    public int Id { get; set; }
    public int ObraId { get; set; }

    [Required]
    public ChecklistTipo Tipo { get; set; }

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
    public virtual ICollection<ChecklistItem> Itens { get; set; } = new List<ChecklistItem>();
}

public class ChecklistItem
{
    public int Id { get; set; }
    public int ChecklistId { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;
    public bool Concluido { get; set; } = false;

    // Navigation Properties
    public virtual Checklist? Checklist { get; set; }
}
