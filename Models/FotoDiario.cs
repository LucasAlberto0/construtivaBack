using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class FotoDiario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        
        public int DiarioObraId { get; set; }
        [ForeignKey("DiarioObraId")]
        public virtual DiarioObra DiarioObra { get; set; }
    }
}
