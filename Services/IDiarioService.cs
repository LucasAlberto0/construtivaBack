
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IDiarioService
{
    Task<IEnumerable<DiarioObraDto>> GetDiariosByObraIdAsync(int obraId);
    Task<DiarioObraDto?> GetDiarioByIdAsync(int obraId, int diarioId);
    Task<DiarioObraDto?> CreateDiarioAsync(int obraId, CreateDiarioObraDto createDto);
    Task<bool> UpdateDiarioAsync(int obraId, int diarioId, CreateDiarioObraDto updateDto);
    Task<bool> DeleteDiarioAsync(int obraId, int diarioId);
    Task<string?> UploadFotoAsync(int obraId, int diarioId, UploadFileDto model);
}
