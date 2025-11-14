using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class ManutencaoService : IManutencaoService
    {
        private readonly ApplicationDbContext _context;

        public ManutencaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ManutencaoListagemDto>> ObterTodasManutencoesAsync(int obraId)
        {
            return await _context.Manutencoes
                .Where(m => m.ObraId == obraId)
                .Include(m => m.Obra)
                .Select(m => new ManutencaoListagemDto
                {
                    Id = m.Id,
                    DataInicio = m.DataInicio,
                    DataTermino = m.DataTermino,
                    ObraId = m.ObraId,
                    NomeObra = m.Obra.Nome
                })
                .ToListAsync();
        }

        public async Task<ManutencaoDetalhesDto?> ObterManutencaoPorIdAsync(int id)
        {
            var manutencao = await _context.Manutencoes
                .Include(m => m.Obra)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null)
            {
                return null;
            }

            return MapearParaManutencaoDetalhesDto(manutencao);
        }

        public async Task<ManutencaoDetalhesDto> CriarManutencaoAsync(ManutencaoCriacaoDto manutencaoDto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == manutencaoDto.ObraId);
            if (!obraExiste)
            {
                throw new ArgumentException("Obra não encontrada.");
            }

            var manutencao = new Manutencao
            {
                DataInicio = manutencaoDto.DataInicio,
                DataTermino = manutencaoDto.DataTermino,
                ImagemUrl = manutencaoDto.ImagemUrl,
                DatasManutencao = manutencaoDto.DatasManutencao,
                ObraId = manutencaoDto.ObraId
            };

            _context.Manutencoes.Add(manutencao);
            await _context.SaveChangesAsync();

            await _context.Entry(manutencao).Reference(m => m.Obra).LoadAsync();

            return MapearParaManutencaoDetalhesDto(manutencao);
        }

        public async Task<ManutencaoDetalhesDto?> AtualizarManutencaoAsync(int id, ManutencaoAtualizacaoDto manutencaoDto)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);

            if (manutencao == null)
            {
                return null;
            }

            manutencao.DataInicio = manutencaoDto.DataInicio;
            manutencao.DataTermino = manutencaoDto.DataTermino;
            manutencao.ImagemUrl = manutencaoDto.ImagemUrl;
            manutencao.DatasManutencao = manutencaoDto.DatasManutencao;

            _context.Manutencoes.Update(manutencao);
            await _context.SaveChangesAsync();

            await _context.Entry(manutencao).Reference(m => m.Obra).LoadAsync();

            return MapearParaManutencaoDetalhesDto(manutencao);
        }

        public async Task<bool> ExcluirManutencaoAsync(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
            {
                return false;
            }

            _context.Manutencoes.Remove(manutencao);
            await _context.SaveChangesAsync();
            return true;
        }

        private ManutencaoDetalhesDto MapearParaManutencaoDetalhesDto(Manutencao manutencao)
        {
            return new ManutencaoDetalhesDto
            {
                Id = manutencao.Id,
                DataInicio = manutencao.DataInicio,
                DataTermino = manutencao.DataTermino,
                ImagemUrl = manutencao.ImagemUrl,
                DatasManutencao = manutencao.DatasManutencao,
                ObraId = manutencao.ObraId,
                NomeObra = manutencao.Obra?.Nome
            };
        }
    }
}
