using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IManutencaoService
    {
        Task<IEnumerable<ManutencaoListagemDto>> ObterTodasManutencoesAsync(int obraId);
        Task<ManutencaoDetalhesDto?> ObterManutencaoPorIdAsync(int id);
        Task<ManutencaoDetalhesDto> CriarManutencaoAsync(ManutencaoCriacaoDto manutencaoDto);
        Task<ManutencaoDetalhesDto?> AtualizarManutencaoAsync(int id, ManutencaoAtualizacaoDto manutencaoDto);
        Task<bool> ExcluirManutencaoAsync(int id);
    }
}
