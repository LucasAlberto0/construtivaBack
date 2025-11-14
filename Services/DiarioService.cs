using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                .Include(d => d.Fotos)
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
                Data = diarioDto.Data,
                Clima = diarioDto.Clima,
                Colaboradores = diarioDto.Colaboradores,
                Atividades = diarioDto.Atividades,
                ObraId = diarioDto.ObraId
            };

            _context.DiariosDeObra.Add(diario);
            await _context.SaveChangesAsync();

            if (diarioDto.FotosUrls != null && diarioDto.FotosUrls.Any())
            {
                foreach (var url in diarioDto.FotosUrls)
                {
                    diario.Fotos.Add(new FotoDiario { Url = url, DiarioObraId = diario.Id });
                }
            }

            if (diarioDto.Comentarios != null && diarioDto.Comentarios.Any())
            {
                foreach (var comentarioDto in diarioDto.Comentarios)
                {
                    var autor = await _userManager.FindByIdAsync(comentarioDto.AutorId);
                    if (autor == null)
                    {
                        throw new ArgumentException($"Autor com ID {comentarioDto.AutorId} não encontrado.");
                    }
                    diario.Comentarios.Add(new Comentario { Texto = comentarioDto.Texto, Data = DateTime.Now, AutorId = comentarioDto.AutorId, DiarioObraId = diario.Id });
                }
            }

            await _context.SaveChangesAsync();

            // Recarregar para incluir as entidades relacionadas recém-adicionadas
            await _context.Entry(diario).Reference(d => d.Obra).LoadAsync();
            await _context.Entry(diario).Collection(d => d.Fotos).LoadAsync();
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

            diario.Data = diarioDto.Data;
            diario.Clima = diarioDto.Clima;
            diario.Colaboradores = diarioDto.Colaboradores;
            diario.Atividades = diarioDto.Atividades;

            _context.DiariosDeObra.Update(diario);
            await _context.SaveChangesAsync();

            await _context.Entry(diario).Reference(d => d.Obra).LoadAsync();
            await _context.Entry(diario).Collection(d => d.Fotos).LoadAsync();
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

        public async Task<FotoDiarioDto?> AdicionarFotoDiarioAsync(int diarioId, string urlFoto)
        {
            var diario = await _context.DiariosDeObra.Include(d => d.Fotos).FirstOrDefaultAsync(d => d.Id == diarioId);
            if (diario == null)
            {
                return null;
            }

            var foto = new FotoDiario { Url = urlFoto, DiarioObraId = diarioId };
            diario.Fotos.Add(foto);
            await _context.SaveChangesAsync();

            return new FotoDiarioDto { Id = foto.Id, Url = foto.Url };
        }

        public async Task<bool> RemoverFotoDiarioAsync(int fotoId)
        {
            var foto = await _context.FotosDiario.FindAsync(fotoId);
            if (foto == null)
            {
                return false;
            }

            _context.FotosDiario.Remove(foto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ComentarioDto?> AdicionarComentarioDiarioAsync(int diarioId, ComentarioCriacaoDto comentarioDto)
        {
            var diario = await _context.DiariosDeObra.Include(d => d.Comentarios).FirstOrDefaultAsync(d => d.Id == diarioId);
            if (diario == null)
            {
                return null;
            }

            var autor = await _userManager.FindByIdAsync(comentarioDto.AutorId);
            if (autor == null)
            {
                throw new ArgumentException($"Autor com ID {comentarioDto.AutorId} não encontrado.");
            }

            var comentario = new Comentario { Texto = comentarioDto.Texto, Data = DateTime.Now, AutorId = comentarioDto.AutorId, DiarioObraId = diarioId };
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
                Colaboradores = diario.Colaboradores,
                Atividades = diario.Atividades,
                ObraId = diario.ObraId,
                NomeObra = diario.Obra?.Nome,
                Fotos = diario.Fotos?.Select(f => new FotoDiarioDto { Id = f.Id, Url = f.Url }).ToList(),
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
