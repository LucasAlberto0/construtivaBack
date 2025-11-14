using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        
        [Required]
        public string AutorId { get; set; }
        [ForeignKey("AutorId")]
        public virtual ApplicationUser Autor { get; set; }
        
        public int? DiarioObraId { get; set; }
        [ForeignKey("DiarioObraId")]
        public virtual DiarioObra? DiarioObra { get; set; }
    }
}
