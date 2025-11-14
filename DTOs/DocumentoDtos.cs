using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs
{
    // DTO para criação de um novo Documento
    public class DocumentoCriacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "A URL do arquivo é obrigatória.")]
        public string Url { get; set; }
        public TipoPasta? Pasta { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
    }

    // DTO para atualização de um Documento existente
    public class DocumentoAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string NomeArquivo { get; set; }
        [Required(ErrorMessage = "A URL do arquivo é obrigatória.")]
        public string Url { get; set; }
        public TipoPasta? Pasta { get; set; }
    }

    // DTO para exibição de detalhes de um Documento
    public class DocumentoDetalhesDto
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
        public TipoPasta? Pasta { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    // DTO para listagem de Documentos
    public class DocumentoListagemDto
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
        public TipoPasta? Pasta { get; set; }
        public int ObraId { get; set; }
    }
}
