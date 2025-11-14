using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class DiarioObra
    {
        [Key]
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public string? Colaboradores { get; set; }
        public string? Atividades { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
        
        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public virtual ICollection<FotoDiario> Fotos { get; set; } = new List<FotoDiario>();
    }
}
