using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
                    DataManutencao = m.DataManutencao,
                    Descricao = m.Descricao,
                    HasFoto = m.Foto != null,
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
                DataManutencao = DateTime.SpecifyKind(manutencaoDto.DataManutencao, DateTimeKind.Utc),
                Descricao = manutencaoDto.Descricao,
                ObraId = manutencaoDto.ObraId
            };

            if (manutencaoDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await manutencaoDto.Foto.CopyToAsync(memoryStream);
                    manutencao.Foto = memoryStream.ToArray();
                    manutencao.FotoMimeType = manutencaoDto.Foto.ContentType;
                }
            }

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

            manutencao.DataManutencao = DateTime.SpecifyKind(manutencaoDto.DataManutencao, DateTimeKind.Utc);
            manutencao.Descricao = manutencaoDto.Descricao;

            if (manutencaoDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await manutencaoDto.Foto.CopyToAsync(memoryStream);
                    manutencao.Foto = memoryStream.ToArray();
                    manutencao.FotoMimeType = manutencaoDto.Foto.ContentType;
                }
            }

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

        public async Task<(byte[]?, string?)> ObterFotoManutencaoAsync(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            return (manutencao?.Foto, manutencao?.FotoMimeType);
        }

        private ManutencaoDetalhesDto MapearParaManutencaoDetalhesDto(Manutencao manutencao)
        {
            return new ManutencaoDetalhesDto
            {
                Id = manutencao.Id,
                DataManutencao = manutencao.DataManutencao,
                Descricao = manutencao.Descricao,
                HasFoto = manutencao.Foto != null,
                ObraId = manutencao.ObraId,
                NomeObra = manutencao.Obra?.Nome
            };
        }
    }
}
