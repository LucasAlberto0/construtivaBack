using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly ApplicationDbContext _context;

        public DocumentoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentoListagemDto>> ObterTodosDocumentosAsync(int obraId)
        {
            return await _context.Documentos
                .Where(d => d.ObraId == obraId)
                .Select(d => new DocumentoListagemDto
                {
                    Id = d.Id,
                    NomeArquivo = d.NomeArquivo,
                    Url = d.Url,
                    Pasta = d.Pasta,
                    ObraId = d.ObraId
                })
                .ToListAsync();
        }

        public async Task<DocumentoDetalhesDto?> ObterDocumentoPorIdAsync(int id)
        {
            var documento = await _context.Documentos
                .Include(d => d.Obra)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (documento == null)
            {
                return null;
            }

            return MapearParaDocumentoDetalhesDto(documento);
        }

        public async Task<DocumentoDetalhesDto> CriarDocumentoAsync(DocumentoCriacaoDto documentoDto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == documentoDto.ObraId);
            if (!obraExiste)
            {
                throw new ArgumentException("Obra não encontrada.");
            }

            var documento = new Documento
            {
                NomeArquivo = documentoDto.NomeArquivo,
                Url = documentoDto.Url,
                Pasta = documentoDto.Pasta,
                ObraId = documentoDto.ObraId
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            await _context.Entry(documento).Reference(d => d.Obra).LoadAsync();

            return MapearParaDocumentoDetalhesDto(documento);
        }

        public async Task<DocumentoDetalhesDto?> AtualizarDocumentoAsync(int id, DocumentoAtualizacaoDto documentoDto)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return null;
            }

            documento.NomeArquivo = documentoDto.NomeArquivo;
            documento.Url = documentoDto.Url;
            documento.Pasta = documentoDto.Pasta;

            _context.Documentos.Update(documento);
            await _context.SaveChangesAsync();

            await _context.Entry(documento).Reference(d => d.Obra).LoadAsync();

            return MapearParaDocumentoDetalhesDto(documento);
        }

        public async Task<bool> ExcluirDocumentoAsync(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return false;
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

        private DocumentoDetalhesDto MapearParaDocumentoDetalhesDto(Documento documento)
        {
            return new DocumentoDetalhesDto
            {
                Id = documento.Id,
                NomeArquivo = documento.NomeArquivo,
                Url = documento.Url,
                Pasta = documento.Pasta ?? TipoPasta.Outros,
                ObraId = documento.ObraId,
                NomeObra = documento.Obra?.Nome
            };
        }
    }
}
