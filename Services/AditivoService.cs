
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class AditivoService : IAditivoService
{
    private readonly ApplicationDbContext _context;

    public AditivoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AditivoDto>> GetAditivosByObraIdAsync(int obraId)
    {
        return await _context.Aditivos
            .Where(a => a.ObraId == obraId)
            .Select(a => new AditivoDto
            {
                Id = a.Id,
                ObraId = a.ObraId,
                Descricao = a.Descricao,
                AnexoPath = a.AnexoPath,
                Aprovado = a.Aprovado,
                DataAprovacao = a.DataAprovacao,
                AprovadoPorUserId = a.AprovadoPorUserId
            })
            .ToListAsync();
    }

    public async Task<AditivoDto?> CreateAditivoAsync(int obraId, CreateAditivoDto createDto)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null;

        var aditivo = new Aditivo
        {
            ObraId = obraId,
            Descricao = createDto.Descricao,
            Aprovado = false
        };

        _context.Aditivos.Add(aditivo);
        await _context.SaveChangesAsync();

        return new AditivoDto { Id = aditivo.Id, ObraId = aditivo.ObraId, Descricao = aditivo.Descricao };
    }

    public async Task<bool> UpdateAditivoAsync(int obraId, int aditivoId, CreateAditivoDto updateDto)
    {
        var aditivo = await _context.Aditivos.FirstOrDefaultAsync(a => a.Id == aditivoId && a.ObraId == obraId);
        if (aditivo == null) return false;

        aditivo.Descricao = updateDto.Descricao;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAditivoAsync(int obraId, int aditivoId)
    {
        var aditivo = await _context.Aditivos.FirstOrDefaultAsync(a => a.Id == aditivoId && a.ObraId == obraId);
        if (aditivo == null) return false;

        _context.Aditivos.Remove(aditivo);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> UploadAnexoAsync(int obraId, int aditivoId, UploadFileDto model)
    {
        var aditivo = await _context.Aditivos.FirstOrDefaultAsync(a => a.Id == aditivoId && a.ObraId == obraId);
        if (aditivo == null) return null;

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

        var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        var filePath = $"/uploads/aditivos/{obraId}/{aditivoId}/{uniqueFileName}";

        aditivo.AnexoPath = filePath;
        await _context.SaveChangesAsync();

        return filePath;
    }

    public async Task<string?> AprovarAditivoAsync(int obraId, int aditivoId, AprovarAditivoDto model, string userId)
    {
        var aditivo = await _context.Aditivos.FirstOrDefaultAsync(a => a.Id == aditivoId && a.ObraId == obraId);
        if (aditivo == null) return null;

        if (aditivo.AnexoPath == null)
        {
            return "Aditivo deve ter um anexo antes de ser aprovado.";
        }

        aditivo.Aprovado = model.Aprovar;
        aditivo.DataAprovacao = DateTime.UtcNow;
        aditivo.AprovadoPorUserId = userId;

        await _context.SaveChangesAsync();

        return "OK";
    }
}
