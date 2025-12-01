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
    public string Tipo { get; set; } // e.g., "Planta", "Relatório", "Foto"
    public string Descricao { get; set; }
    public DateTime DataUpload { get; set; }
    public string CaminhoArquivo { get; set; } // Original filename
    public byte[]? ConteudoArquivo { get; set; } // File content as byte array
    public long TamanhoArquivo { get; set; } // File size in bytes
    public DateTime DataAnexamento { get; set; } // Date when the file was attached
}
}
