using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;
using Microsoft.AspNetCore.Http; // Required for IFormFile

namespace construtivaBack.DTOs
{
    // DTO para criação de um novo Documento
    public class DocumentoCriacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O tipo do documento é obrigatório.")]
        public string Tipo { get; set; } // New
        public string? CaminhoArquivo { get; set; } // Made nullable as it might be set by file upload
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
        public string? Descricao { get; set; }
    }

    // DTO para atualização de um Documento existente
    public class DocumentoAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O tipo do documento é obrigatório.")]
        public string Tipo { get; set; } // New
        public string? CaminhoArquivo { get; set; } // Made nullable
        public string? Descricao { get; set; }
    }

    // DTO para anexo de arquivo a um Documento existente
    public class DocumentoAnexoRequestDto
    {
        [Required(ErrorMessage = "O arquivo é obrigatório.")]
        public IFormFile Arquivo { get; set; }
        public string? Descricao { get; set; }
        public string? Tipo { get; set; } // Allow updating type on attachment
    }

    // DTO para exibição de detalhes de um Documento
    public class DocumentoDetalhesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; } // New
        public string CaminhoArquivo { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
        public string? Descricao { get; set; }
        public long TamanhoArquivo { get; set; }
        public DateTime DataAnexamento { get; set; }
        public DateTime DataUpload { get; set; }
    }

    // DTO para listagem de Documentos
    public class DocumentoListagemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; } // New
        public string CaminhoArquivo { get; set; }
        public int ObraId { get; set; }
        public string? Descricao { get; set; }
        public long TamanhoArquivo { get; set; }
        public DateTime DataAnexamento { get; set; }
        public DateTime DataUpload { get; set; }
    }
}
