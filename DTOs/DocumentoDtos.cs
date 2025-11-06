
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class DocumentoDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Pasta { get; set; } = string.Empty;
    public int Versao { get; set; } = 1;
    public bool IsDeleted { get; set; } = false;
}

public class UploadDocumentoDto
{
    [Required]
    public IFormFile File { get; set; } = default!;

    public string Pasta { get; set; } = "Geral";
}
