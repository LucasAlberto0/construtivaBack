
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IObraService
{
    Task<IEnumerable<ObraDto>> GetAllObrasAsync();
    Task<ObraDto?> GetObraByIdAsync(int id);
    Task<ObraDto> CreateObraAsync(CreateObraDto createObraDto);
    Task<bool> UpdateObraAsync(int id, CreateObraDto updateObraDto);
    Task<bool> DeleteObraAsync(int id);
    Task<bool> ObraExistsAsync(int id);
}
