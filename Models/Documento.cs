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
    public int Id { get; set; }
    public int ObraId { get; set; }
    public Obra Obra { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataUpload { get; set; }
    public string CaminhoArquivo { get; set; }
    public byte[]? ConteudoArquivo { get; set; }
    public long TamanhoArquivo { get; set; }
    public DateTime DataAnexamento { get; set; }
}
}
