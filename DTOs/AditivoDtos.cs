using System;
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs
{
    // DTO para criação de um novo Aditivo
    public class AditivoCriacaoDto
    {
        [Required(ErrorMessage = "A descrição do aditivo é obrigatória.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A data do aditivo é obrigatória.")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
    }

    // DTO para atualização de um Aditivo existente
    public class AditivoAtualizacaoDto
    {
        [Required(ErrorMessage = "A descrição do aditivo é obrigatória.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "A data do aditivo é obrigatória.")]
        public DateTime Data { get; set; }
    }

    // DTO para exibição de detalhes de um Aditivo
    public class AditivoDetalhesDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    // DTO para listagem de Aditivos
    public class AditivoListagemDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public int ObraId { get; set; }
    }
}
