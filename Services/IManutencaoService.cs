using construtivaBack.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace construtivaBack.Services
{
    public interface IManutencaoService
    {
        Task<IEnumerable<ManutencaoListagemDto>> ObterTodasManutencoesAsync(int obraId);
        Task<ManutencaoDetalhesDto?> ObterManutencaoPorIdAsync(int id);
        Task<ManutencaoDetalhesDto> CriarManutencaoAsync(ManutencaoCriacaoDto manutencaoDto);
        Task<ManutencaoDetalhesDto?> AtualizarManutencaoAsync(int id, ManutencaoAtualizacaoDto manutencaoDto);
        Task<bool> ExcluirManutencaoAsync(int id);
        Task<(byte[]?, string?)> ObterFotoManutencaoAsync(int id);
    }
}
