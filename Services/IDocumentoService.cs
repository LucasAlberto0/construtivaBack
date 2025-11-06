
using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services;

public interface IDocumentoService
{
    Task<IEnumerable<DocumentoDto>> GetDocumentosByObraIdAsync(int obraId);
    Task<string?> UploadDocumentoAsync(int obraId, UploadDocumentoDto model);
    Task<bool> DeleteDocumentoAsync(int obraId, int documentoId);
    Task<DocumentoDto?> GetDocumentoForDownloadAsync(int obraId, int documentoId);
}
