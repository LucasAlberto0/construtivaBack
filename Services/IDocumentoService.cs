using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IDocumentoService
    {
        Task<IEnumerable<DocumentoListagemDto>> ObterTodosDocumentosAsync(int obraId);
        Task<DocumentoDetalhesDto?> ObterDocumentoPorIdAsync(int id);
        Task<DocumentoDetalhesDto> CriarDocumentoAsync(DocumentoCriacaoDto documentoDto);
        Task<DocumentoDetalhesDto?> AtualizarDocumentoAsync(int id, DocumentoAtualizacaoDto documentoDto);
        Task<bool> ExcluirDocumentoAsync(int id);
        Task<DocumentoDetalhesDto?> AnexarArquivoDocumentoAsync(int id, DocumentoAnexoRequestDto anexoDto);
        Task<(byte[]? fileContents, string? contentType, string? fileName)> DownloadDocumentoAsync(int id);
    }
}
