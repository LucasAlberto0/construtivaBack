using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
                    Nome = d.Nome,
                    Tipo = d.Tipo,
                    CaminhoArquivo = d.CaminhoArquivo,
                    ObraId = d.ObraId,
                    Descricao = d.Descricao,
                    TamanhoArquivo = d.TamanhoArquivo,
                    DataAnexamento = d.DataAnexamento,
                    DataUpload = d.DataUpload
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
                Nome = documentoDto.Nome,
                Tipo = documentoDto.Tipo,
                CaminhoArquivo = documentoDto.CaminhoArquivo,
                ObraId = documentoDto.ObraId,
                Descricao = documentoDto.Descricao,
                DataUpload = DateTime.UtcNow
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

            documento.Nome = documentoDto.Nome;
            documento.Tipo = documentoDto.Tipo;
            documento.CaminhoArquivo = documentoDto.CaminhoArquivo;
            documento.Descricao = documentoDto.Descricao;

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

        public async Task<DocumentoDetalhesDto?> AnexarArquivoDocumentoAsync(int id, DocumentoAnexoRequestDto anexoDto)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return null;
            }

            if (anexoDto.Arquivo != null && anexoDto.Arquivo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await anexoDto.Arquivo.CopyToAsync(memoryStream);
                    documento.ConteudoArquivo = memoryStream.ToArray();
                }

                documento.TamanhoArquivo = anexoDto.Arquivo.Length;
                documento.DataAnexamento = DateTime.UtcNow;
                documento.CaminhoArquivo = anexoDto.Arquivo.FileName;
                documento.Descricao = anexoDto.Descricao ?? documento.Descricao;
                documento.Tipo = anexoDto.Tipo ?? documento.Tipo;
            }

            _context.Documentos.Update(documento);
            await _context.SaveChangesAsync();

            await _context.Entry(documento).Reference(d => d.Obra).LoadAsync();

            return MapearParaDocumentoDetalhesDto(documento);
        }

        public async Task<(byte[]? fileContents, string? contentType, string? fileName)> DownloadDocumentoAsync(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null || documento.ConteudoArquivo == null)
            {
                return (null, null, null);
            }

            string contentType = "application/octet-stream";
            if (!string.IsNullOrEmpty(documento.CaminhoArquivo))
            {
                var extension = Path.GetExtension(documento.CaminhoArquivo)?.ToLowerInvariant();
                switch (extension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    case ".doc":
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".xls":
                    case ".xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                }
            }

            return (documento.ConteudoArquivo, contentType, documento.CaminhoArquivo);
        }

        private DocumentoDetalhesDto MapearParaDocumentoDetalhesDto(Documento documento)
        {
            return new DocumentoDetalhesDto
            {
                Id = documento.Id,
                Nome = documento.Nome,
                Tipo = documento.Tipo,
                CaminhoArquivo = documento.CaminhoArquivo,
                ObraId = documento.ObraId,
                NomeObra = documento.Obra?.Nome,
                Descricao = documento.Descricao,
                TamanhoArquivo = documento.TamanhoArquivo,
                DataAnexamento = documento.DataAnexamento,
                DataUpload = documento.DataUpload
            };
        }
    }
}
