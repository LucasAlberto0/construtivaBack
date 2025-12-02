using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs
{
    // DTO para criação de um novo Checklist
    public class ChecklistCriacaoDto
    {
        [Required(ErrorMessage = "O tipo de checklist é obrigatório.")]
        public TipoChecklist Tipo { get; set; }
        [Required(ErrorMessage = "O ID da obra é obrigatório.")]
        public int ObraId { get; set; }
        public List<ChecklistItemCriacaoDto>? Itens { get; set; }
    }

    // DTO para atualização de um Checklist existente
    public class ChecklistAtualizacaoDto
    {
        [Required(ErrorMessage = "O tipo de checklist é obrigatório.")]
        public TipoChecklist Tipo { get; set; }
        public List<ChecklistItemAtualizacaoDto>? Itens { get; set; }
    }

    // DTO para exibição de detalhes de um Checklist
    public class ChecklistDetalhesDto
    {
        public int Id { get; set; }
        public TipoChecklist Tipo { get; set; }
        public int? ObraId { get; set; }
        public string? NomeObra { get; set; }
        public ICollection<ChecklistItemDto>? Itens { get; set; }
    }

    // DTO para listagem de Checklists
    public class ChecklistListagemDto
    {
        public int Id { get; set; }
        public TipoChecklist Tipo { get; set; }
        public int? ObraId { get; set; }
        public string? NomeObra { get; set; }
    }

    // DTO para criação de ChecklistItem
    public class ChecklistItemCriacaoDto
    {
        [Required(ErrorMessage = "O nome do item é obrigatório.")]
        public string Nome { get; set; }
        public bool Concluido { get; set; } = false;
        public string? Observacao { get; set; }
    }

    // DTO para atualização de ChecklistItem
    public class ChecklistItemAtualizacaoDto
    {
        [Required(ErrorMessage = "O ID do item é obrigatório para atualização.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome do item é obrigatório.")]
        public string Nome { get; set; }
        public bool Concluido { get; set; }
        public string? Observacao { get; set; }
    }

    // DTO para exibição de ChecklistItem
    public class ChecklistItemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Concluido { get; set; }
        public string? Observacao { get; set; }
    }
}
