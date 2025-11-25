using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class ChecklistItem
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } 
        public bool Concluido { get; set; }
        public string? Observacao { get; set; }
        
        public int ChecklistId { get; set; }
        [ForeignKey("ChecklistId")]
        public virtual Checklist Checklist { get; set; }
    }
}
