using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;
using Microsoft.AspNetCore.Http;

namespace construtivaBack.DTOs
{
    public class DocumentoCriacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O tipo do documento é obrigatório.")]
        public string Tipo { get; set; }
        public string? CaminhoArquivo { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
        public string? Descricao { get; set; }
    }

    public class DocumentoAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome do arquivo é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O tipo do documento é obrigatório.")]
        public string Tipo { get; set; }
        public string? CaminhoArquivo { get; set; }
        public string? Descricao { get; set; }
    }

    public class DocumentoAnexoRequestDto
    {
        [Required(ErrorMessage = "O arquivo é obrigatório.")]
        public IFormFile Arquivo { get; set; }
        public string? Descricao { get; set; }
        public string? Tipo { get; set; }
    }

    public class DocumentoDetalhesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string CaminhoArquivo { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
        public string? Descricao { get; set; }
        public long TamanhoArquivo { get; set; }
        public DateTime DataAnexamento { get; set; }
        public DateTime DataUpload { get; set; }
    }

    public class DocumentoListagemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string CaminhoArquivo { get; set; }
        public int ObraId { get; set; }
        public string? Descricao { get; set; }
        public long TamanhoArquivo { get; set; }
        public DateTime DataAnexamento { get; set; }
        public DateTime DataUpload { get; set; }
    }
}
