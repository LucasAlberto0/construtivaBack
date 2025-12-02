using construtivaBack.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace construtivaBack.Services
{
    public interface IDiarioService
    {
        Task<IEnumerable<DiarioObraListagemDto>> ObterTodosDiariosAsync(int obraId);
        Task<DiarioObraDetalhesDto?> ObterDiarioPorIdAsync(int id);
        Task<DiarioObraDetalhesDto> CriarDiarioAsync(DiarioObraCriacaoDto diarioDto);
        Task<DiarioObraDetalhesDto?> AtualizarDiarioAsync(int id, DiarioObraAtualizacaoDto diarioDto);
        Task<bool> ExcluirDiarioAsync(int id);
        Task<(byte[]?, string?)> ObterFotoDiarioAsync(int id);
        Task<ComentarioDto?> AdicionarComentarioDiarioAsync(int diarioId, ComentarioCriacaoDto comentarioDto, string autorId);
        Task<bool> RemoverComentarioDiarioAsync(int comentarioId);
    }
}
