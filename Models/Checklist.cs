using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public enum TipoChecklist
    {
        InicioObra,
        EntregaObra
    }

    public class Checklist
    {
        [Key]
        public int Id { get; set; }
        public TipoChecklist Tipo { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
        
        public virtual ICollection<ChecklistItem> Itens { get; set; } = new List<ChecklistItem>();
    }
}
