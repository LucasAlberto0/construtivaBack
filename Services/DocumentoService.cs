
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class DocumentoService : IDocumentoService
{
    private readonly ApplicationDbContext _context;

    public DocumentoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DocumentoDto>> GetDocumentosByObraIdAsync(int obraId)
    {
        return await _context.Documentos
            .Where(d => d.ObraId == obraId && !d.IsDeleted)
            .Select(d => new DocumentoDto
            {
                Id = d.Id,
                ObraId = d.ObraId,
                Nome = d.Nome,
                Path = d.Path,
                Pasta = d.Pasta,
                Versao = d.Versao,
                IsDeleted = d.IsDeleted
            })
            .ToListAsync();
    }

    public async Task<string?> UploadDocumentoAsync(int obraId, UploadDocumentoDto model)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null; // Indica que a obra não foi encontrada

        if (model.File == null || model.File.Length == 0)
        {
            return "Nenhum arquivo enviado.";
        }

        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return "Tipo de arquivo não permitido. Apenas PDF, DOC, DOCX, JPG e PNG são aceitos.";
        }

        var maxFileSize = 50 * 1024 * 1024; // 50 MB
        if (model.File.Length > maxFileSize)
        {
            return $"Tamanho do arquivo excede o limite de {maxFileSize / (1024 * 1024)}MB.";
        }

        var existingDoc = await _context.Documentos
            .Where(d => d.ObraId == obraId && d.Nome == model.File.FileName && !d.IsDeleted)
            .OrderByDescending(d => d.Versao)
            .FirstOrDefaultAsync();

        int newVersion = 1;
        if (existingDoc != null)
        {
            newVersion = existingDoc.Versao + 1;
        }

        var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        var filePath = $"/uploads/documentos/{obraId}/{uniqueFileName}";

        var documento = new Documento
        {
            ObraId = obraId,
            Nome = model.File.FileName,
            Path = filePath,
            Pasta = model.Pasta,
            Versao = newVersion,
            IsDeleted = false
        };

        _context.Documentos.Add(documento);
        await _context.SaveChangesAsync();

        return filePath; // Retorna o caminho do arquivo
    }

    public async Task<bool> DeleteDocumentoAsync(int obraId, int documentoId)
    {
        var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == documentoId && d.ObraId == obraId);
        if (documento == null) return false;

        documento.IsDeleted = true;
        _context.Entry(documento).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<DocumentoDto?> GetDocumentoForDownloadAsync(int obraId, int documentoId)
    {
        var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == documentoId && d.ObraId == obraId && !d.IsDeleted);
        if (documento == null) return null;

        return new DocumentoDto
        {
            Id = documento.Id,
            ObraId = documento.ObraId,
            Nome = documento.Nome,
            Path = documento.Path,
            Pasta = documento.Pasta,
            Versao = documento.Versao,
            IsDeleted = documento.IsDeleted
        };
    }
}
