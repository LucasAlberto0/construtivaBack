using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IChecklistService
    {
        Task<IEnumerable<ChecklistListagemDto>> ObterTodosChecklistsAsync(int? obraId);
        Task<ChecklistDetalhesDto?> ObterChecklistPorIdAsync(int id);
        Task<ChecklistDetalhesDto> CriarChecklistAsync(ChecklistCriacaoDto checklistDto);
        Task<ChecklistDetalhesDto?> AtualizarChecklistAsync(int id, ChecklistAtualizacaoDto checklistDto);
        Task<bool> ExcluirChecklistAsync(int id);
        Task<ChecklistItemDto?> AdicionarItemChecklistAsync(int checklistId, ChecklistItemCriacaoDto itemDto);
        Task<ChecklistItemDto?> AtualizarItemChecklistAsync(int itemId, ChecklistItemAtualizacaoDto itemDto);
        Task<bool> ExcluirItemChecklistAsync(int itemId);
    }
}
