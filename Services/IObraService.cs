using construtivaBack.DTOs;
using construtivaBack.Models;

namespace construtivaBack.Services
{
    public interface IObraService
    {
        Task<IEnumerable<ObraListagemDto>> ObterTodasObrasAsync();
        Task<ObraDetalhesDto?> ObterObraPorIdAsync(int id);
        Task<ObraDetalhesDto> CriarObraAsync(ObraCriacaoDto obraDto, string userId);
        Task<ObraDetalhesDto?> AtualizarObraAsync(int id, ObraAtualizacaoDto obraDto);
        Task<bool> ExcluirObraAsync(int id);
    }
}
