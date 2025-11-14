using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IAditivoService
    {
        Task<IEnumerable<AditivoListagemDto>> ObterTodosAditivosAsync(int obraId);
        Task<AditivoDetalhesDto?> ObterAditivoPorIdAsync(int id);
        Task<AditivoDetalhesDto> CriarAditivoAsync(AditivoCriacaoDto aditivoDto);
        Task<AditivoDetalhesDto?> AtualizarAditivoAsync(int id, AditivoAtualizacaoDto aditivoDto);
        Task<bool> ExcluirAditivoAsync(int id);
    }
}
