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
        public Clima Clima { get; set; }
        public int QuantidadeColaboradores { get; set; }
        public string DescricaoAtividades { get; set; }
        public string? Observacoes { get; set; }
        public byte[]? Foto { get; set; }
        public string? FotoMimeType { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
        
        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
