using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace construtivaBack.Services
{
    public class DiarioService : IDiarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiarioService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<DiarioObraListagemDto>> ObterTodosDiariosAsync(int obraId)
        {
            return await _context.DiariosDeObra
                .Where(d => d.ObraId == obraId)
                .Include(d => d.Obra)
                .Select(d => new DiarioObraListagemDto
                {
                    Id = d.Id,
                    Data = d.Data,
                    Clima = d.Clima,
                    ObraId = d.ObraId,
                    NomeObra = d.Obra.Nome
                })
                .ToListAsync();
        }

        public async Task<DiarioObraDetalhesDto?> ObterDiarioPorIdAsync(int id)
        {
            var diario = await _context.DiariosDeObra
                .Include(d => d.Obra)
                .Include(d => d.Comentarios)
                    .ThenInclude(c => c.Autor)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (diario == null)
            {
                return null;
            }

            return MapearParaDiarioObraDetalhesDto(diario);
        }

        public async Task<DiarioObraDetalhesDto> CriarDiarioAsync(DiarioObraCriacaoDto diarioDto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == diarioDto.ObraId);
            if (!obraExiste)
            {
                throw new ArgumentException("Obra não encontrada.");
            }

            var diario = new DiarioObra
            {
                Data = DateTime.SpecifyKind(diarioDto.Data, DateTimeKind.Utc),
                Clima = diarioDto.Clima,
                QuantidadeColaboradores = diarioDto.QuantidadeColaboradores,
                DescricaoAtividades = diarioDto.DescricaoAtividades,
                Observacoes = diarioDto.Observacoes,
                ObraId = diarioDto.ObraId
            };

            if (diarioDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await diarioDto.Foto.CopyToAsync(memoryStream);
                    diario.Foto = memoryStream.ToArray();
                    diario.FotoMimeType = diarioDto.Foto.ContentType;
                }
            }

            _context.DiariosDeObra.Add(diario);
            await _context.SaveChangesAsync();

            // Removed the loop for adding comments during diario creation,
            // as comments should be added via the dedicated endpoint.

            await _context.Entry(diario).Reference(d => d.Obra).LoadAsync();
            await _context.Entry(diario).Collection(d => d.Comentarios).Query().Include(c => c.Autor).LoadAsync();

            return MapearParaDiarioObraDetalhesDto(diario);
        }

        public async Task<DiarioObraDetalhesDto?> AtualizarDiarioAsync(int id, DiarioObraAtualizacaoDto diarioDto)
        {
            var diario = await _context.DiariosDeObra.FindAsync(id);

            if (diario == null)
            {
                return null;
            }

            diario.Data = DateTime.SpecifyKind(diarioDto.Data, DateTimeKind.Utc);
            diario.Clima = diarioDto.Clima;
            diario.QuantidadeColaboradores = diarioDto.QuantidadeColaboradores;
            diario.DescricaoAtividades = diarioDto.DescricaoAtividades;
            diario.Observacoes = diarioDto.Observacoes;

            if (diarioDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await diarioDto.Foto.CopyToAsync(memoryStream);
                    diario.Foto = memoryStream.ToArray();
                    diario.FotoMimeType = diarioDto.Foto.ContentType;
                }
            }

            _context.DiariosDeObra.Update(diario);
            await _context.SaveChangesAsync();

            await _context.Entry(diario).Reference(d => d.Obra).LoadAsync();
            await _context.Entry(diario).Collection(d => d.Comentarios).Query().Include(c => c.Autor).LoadAsync();

            return MapearParaDiarioObraDetalhesDto(diario);
        }

        public async Task<bool> ExcluirDiarioAsync(int id)
        {
            var diario = await _context.DiariosDeObra.FindAsync(id);
            if (diario == null)
            {
                return false;
            }

            _context.DiariosDeObra.Remove(diario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(byte[]?, string?)> ObterFotoDiarioAsync(int id)
        {
            var diario = await _context.DiariosDeObra.FindAsync(id);
            return (diario?.Foto, diario?.FotoMimeType);
        }

        public async Task<ComentarioDto?> AdicionarComentarioDiarioAsync(int diarioId, ComentarioCriacaoDto comentarioDto, string autorId)
        {
            var diario = await _context.DiariosDeObra.Include(d => d.Comentarios).FirstOrDefaultAsync(d => d.Id == diarioId);
            if (diario == null)
            {
                return null; // Diario de Obra not found
            }

            var autor = await _userManager.FindByIdAsync(autorId);
            if (autor == null)
            {
                // This case should ideally not happen if the user is authenticated,
                // but as a safeguard, we can throw an exception or return null.
                // For now, let's throw an ArgumentException as it indicates an unexpected state.
                throw new ArgumentException($"Autor com ID {autorId} não encontrado. O usuário autenticado não foi encontrado no sistema.");
            }

            var comentario = new Comentario { Texto = comentarioDto.Texto, Data = DateTime.UtcNow, AutorId = autorId, DiarioObraId = diarioId };
            diario.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return new ComentarioDto { Id = comentario.Id, Texto = comentario.Texto, Data = comentario.Data, AutorNome = autor.NomeCompleto ?? autor.UserName };
        }

        public async Task<bool> RemoverComentarioDiarioAsync(int comentarioId)
        {
            var comentario = await _context.Comentarios.FindAsync(comentarioId);
            if (comentario == null)
            {
                return false;
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            return true;
        }

        private DiarioObraDetalhesDto MapearParaDiarioObraDetalhesDto(DiarioObra diario)
        {
            return new DiarioObraDetalhesDto
            {
                Id = diario.Id,
                Data = diario.Data,
                Clima = diario.Clima,
                QuantidadeColaboradores = diario.QuantidadeColaboradores,
                DescricaoAtividades = diario.DescricaoAtividades,
                Observacoes = diario.Observacoes,
                HasFoto = diario.Foto != null,
                ObraId = diario.ObraId,
                NomeObra = diario.Obra?.Nome,
                Comentarios = diario.Comentarios?.Select(c => new ComentarioDto
                {
                    Id = c.Id,
                    Texto = c.Texto,
                    Data = c.Data,
                    AutorNome = c.Autor?.NomeCompleto ?? c.Autor?.UserName
                }).ToList()
            };
        }
    }
}
