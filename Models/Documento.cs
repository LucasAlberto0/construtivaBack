using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace construtivaBack.Models
{
    public enum TipoPasta
    {
        Contratos,
        Projetos,
        Relatorios,
        Outros
    }

    public class Documento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NomeArquivo { get; set; }
        [Required]
public string Url { get; set; }
        public TipoPasta? Pasta { get; set; }
        
        public int ObraId { get; set; }
        [ForeignKey("ObraId")]
        public virtual Obra Obra { get; set; }
    }
}
