using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace construtivaBack.DTOs
{
    public class ManutencaoCriacaoDto
    {
        [Required(ErrorMessage = "A data de manutenção é obrigatória.")]
        public DateTime DataManutencao { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
        public IFormFile? Foto { get; set; }
    }

    public class ManutencaoAtualizacaoDto
    {
        [Required(ErrorMessage = "A data de manutenção é obrigatória.")]
        public DateTime DataManutencao { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; }
        public IFormFile? Foto { get; set; }
    }

    public class ManutencaoDetalhesDto
    {
        public int Id { get; set; }
        public DateTime DataManutencao { get; set; }
        public string Descricao { get; set; }
        public bool HasFoto { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    public class ManutencaoListagemDto
    {
        public int Id { get; set; }
        public DateTime DataManutencao { get; set; }
        public string Descricao { get; set; }
        public bool HasFoto { get; set; }
        public int ObraId { get; set; }
        public string? NomeObra { get; set; }
    }
}
