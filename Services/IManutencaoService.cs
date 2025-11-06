
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IManutencaoService
{
    Task<IEnumerable<ManutencaoDto>> GetManutencoesByObraIdAsync(int obraId);
    Task<ManutencaoDto?> CreateManutencaoAsync(int obraId, CreateManutencaoDto createDto);
    Task<bool> UpdateManutencaoAsync(int obraId, int manutencaoId, CreateManutencaoDto updateDto);
    Task<bool> DeleteManutencaoAsync(int obraId, int manutencaoId);
    Task<string?> UploadImagemAsync(int obraId, int manutencaoId, UploadFileDto model);
}
