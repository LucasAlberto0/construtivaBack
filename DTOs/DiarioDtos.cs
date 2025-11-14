using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs
{
    // DTO para criação de um novo Diário de Obra
    public class DiarioObraCriacaoDto
    {
        [Required(ErrorMessage = "A data do diário é obrigatória.")]
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public string? Colaboradores { get; set; }
        public string? Atividades { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
        public List<string>? FotosUrls { get; set; } // URLs das fotos a serem adicionadas
        public List<ComentarioCriacaoDto>? Comentarios { get; set; } // Comentários a serem adicionados
    }

    // DTO para atualização de um Diário de Obra existente
    public class DiarioObraAtualizacaoDto
    {
        [Required(ErrorMessage = "A data do diário é obrigatória.")]
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public string? Colaboradores { get; set; }
        public string? Atividades { get; set; }
    }

    // DTO para exibição de detalhes de um Diário de Obra
    public class DiarioObraDetalhesDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public string? Colaboradores { get; set; }
        public string? Atividades { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
        public ICollection<FotoDiarioDto>? Fotos { get; set; }
        public ICollection<ComentarioDto>? Comentarios { get; set; }
    }

    // DTO para listagem de Diários de Obra
    public class DiarioObraListagemDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    // DTO para FotoDiario
    public class FotoDiarioDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }

    // DTO para criação de Comentário
    public class ComentarioCriacaoDto
    {
        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        public string Texto { get; set; }
        [Required(ErrorMessage = "O ID do autor é obrigatório.")]
        public string AutorId { get; set; }
    }

    // DTO para exibição de Comentário
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public string AutorNome { get; set; }
    }
}
