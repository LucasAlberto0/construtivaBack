using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class AditivoService : IAditivoService
    {
        private readonly ApplicationDbContext _context;

        public AditivoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AditivoListagemDto>> ObterTodosAditivosAsync(int obraId)
        {
            return await _context.Aditivos
                .Where(a => a.ObraId == obraId)
                .Include(a => a.Obra)
                .Select(a => new AditivoListagemDto
                {
                    Id = a.Id,
                    Descricao = a.Descricao,
                    Data = a.Data,
                    ObraId = a.ObraId
                })
                .ToListAsync();
        }

        public async Task<AditivoDetalhesDto?> ObterAditivoPorIdAsync(int id)
        {
            var aditivo = await _context.Aditivos
                .Include(a => a.Obra)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aditivo == null)
            {
                return null;
            }

            return MapearParaAditivoDetalhesDto(aditivo);
        }

        public async Task<AditivoDetalhesDto> CriarAditivoAsync(AditivoCriacaoDto aditivoDto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == aditivoDto.ObraId);
            if (!obraExiste)
            {
                throw new ArgumentException("Obra não encontrada.");
            }

            var aditivo = new Aditivo
            {
                Descricao = aditivoDto.Descricao,
                Data = aditivoDto.Data,
                ObraId = aditivoDto.ObraId
            };

            _context.Aditivos.Add(aditivo);
            await _context.SaveChangesAsync();

            await _context.Entry(aditivo).Reference(a => a.Obra).LoadAsync();

            return MapearParaAditivoDetalhesDto(aditivo);
        }

        public async Task<AditivoDetalhesDto?> AtualizarAditivoAsync(int id, AditivoAtualizacaoDto aditivoDto)
        {
            var aditivo = await _context.Aditivos.FindAsync(id);

            if (aditivo == null)
            {
                return null;
            }

            aditivo.Descricao = aditivoDto.Descricao;
            aditivo.Data = aditivoDto.Data;

            _context.Aditivos.Update(aditivo);
            await _context.SaveChangesAsync();

            await _context.Entry(aditivo).Reference(a => a.Obra).LoadAsync();

            return MapearParaAditivoDetalhesDto(aditivo);
        }

        public async Task<bool> ExcluirAditivoAsync(int id)
        {
            var aditivo = await _context.Aditivos.FindAsync(id);
            if (aditivo == null)
            {
                return false;
            }

            _context.Aditivos.Remove(aditivo);
            await _context.SaveChangesAsync();
            return true;
        }

        private AditivoDetalhesDto MapearParaAditivoDetalhesDto(Aditivo aditivo)
        {
            return new AditivoDetalhesDto
            {
                Id = aditivo.Id,
                Descricao = aditivo.Descricao,
                Data = aditivo.Data,
                ObraId = aditivo.ObraId,
                NomeObra = aditivo.Obra?.Nome
            };
        }
    }
}
