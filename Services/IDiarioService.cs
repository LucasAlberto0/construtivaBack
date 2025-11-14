using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IDiarioService
    {
        Task<IEnumerable<DiarioObraListagemDto>> ObterTodosDiariosAsync(int obraId);
        Task<DiarioObraDetalhesDto?> ObterDiarioPorIdAsync(int id);
        Task<DiarioObraDetalhesDto> CriarDiarioAsync(DiarioObraCriacaoDto diarioDto);
        Task<DiarioObraDetalhesDto?> AtualizarDiarioAsync(int id, DiarioObraAtualizacaoDto diarioDto);
        Task<bool> ExcluirDiarioAsync(int id);
        Task<FotoDiarioDto?> AdicionarFotoDiarioAsync(int diarioId, string urlFoto);
        Task<bool> RemoverFotoDiarioAsync(int fotoId);
        Task<ComentarioDto?> AdicionarComentarioDiarioAsync(int diarioId, ComentarioCriacaoDto comentarioDto);
        Task<bool> RemoverComentarioDiarioAsync(int comentarioId);
    }
}
