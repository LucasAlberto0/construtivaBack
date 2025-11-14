using System;
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs
{
    // DTO para criação de uma nova Manutenção
    public class ManutencaoCriacaoDto
    {
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }
        [Required(ErrorMessage = "A data de término é obrigatória.")]
        public DateTime DataTermino { get; set; }
        public string? ImagemUrl { get; set; }
        public string? DatasManutencao { get; set; } // JSON string of dates
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
    }

    // DTO para atualização de uma Manutenção existente
    public class ManutencaoAtualizacaoDto
    {
        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }
        [Required(ErrorMessage = "A data de término é obrigatória.")]
        public DateTime DataTermino { get; set; }
        public string? ImagemUrl { get; set; }
        public string? DatasManutencao { get; set; } // JSON string of dates
    }

    // DTO para exibição de detalhes de uma Manutenção
    public class ManutencaoDetalhesDto
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string? ImagemUrl { get; set; }
        public string? DatasManutencao { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    // DTO para listagem de Manutenções
    public class ManutencaoListagemDto
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }
}
