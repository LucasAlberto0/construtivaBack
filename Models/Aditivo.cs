using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class Aditivo
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
    }
}
