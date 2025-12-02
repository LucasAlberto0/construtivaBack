using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public class Manutencao
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataManutencao { get; set; }
        public string Descricao { get; set; }
        public byte[]? Foto { get; set; }
        public string? FotoMimeType { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
    }
}
