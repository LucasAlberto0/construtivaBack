
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IComentarioService
{
    Task<IEnumerable<ComentarioDto>> GetComentariosByObraIdAsync(int obraId);
    Task<ComentarioDto?> CreateComentarioAsync(int obraId, CreateComentarioDto createDto, string userId);
    Task<bool> DeleteComentarioAsync(int obraId, int comentarioId, string userId, bool isAdmin);
}
