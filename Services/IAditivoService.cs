
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IAditivoService
{
    Task<IEnumerable<AditivoDto>> GetAditivosByObraIdAsync(int obraId);
    Task<AditivoDto?> CreateAditivoAsync(int obraId, CreateAditivoDto createDto);
    Task<bool> UpdateAditivoAsync(int obraId, int aditivoId, CreateAditivoDto updateDto);
    Task<bool> DeleteAditivoAsync(int obraId, int aditivoId);
    Task<string?> UploadAnexoAsync(int obraId, int aditivoId, UploadFileDto model);
    Task<string?> AprovarAditivoAsync(int obraId, int aditivoId, AprovarAditivoDto model, string userId);
}
