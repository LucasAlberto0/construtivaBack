
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IChecklistService
{
    Task<IEnumerable<ChecklistDto>> GetChecklistsByObraIdAsync(int obraId);
    Task<ChecklistDto?> CreateChecklistAsync(int obraId, CreateChecklistDto createDto);
    Task<bool> UpdateChecklistItemAsync(int obraId, int checklistId, int itemId, UpdateChecklistItemDto updateDto);
    Task<bool> DeleteChecklistAsync(int obraId, int checklistId);
}
