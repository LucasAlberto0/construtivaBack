
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class DiarioService : IDiarioService
{
    private readonly ApplicationDbContext _context;

    public DiarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiarioObraDto>> GetDiariosByObraIdAsync(int obraId)
    {
        return await _context.DiariosDeObra
            .Where(d => d.ObraId == obraId)
            .Select(d => new DiarioObraDto
            {
                Id = d.Id,
                ObraId = d.ObraId,
                Data = d.Data,
                Clima = d.Clima,
                EquipePresente = d.EquipePresente,
                AtividadesRealizadas = d.AtividadesRealizadas,
                ComentariosTecnicos = d.ComentariosTecnicos,
                Fotos = d.Fotos.ToList()
            })
            .ToListAsync();
    }

    public async Task<DiarioObraDto?> GetDiarioByIdAsync(int obraId, int diarioId)
    {
        var diario = await _context.DiariosDeObra.FirstOrDefaultAsync(d => d.Id == diarioId && d.ObraId == obraId);
        if (diario == null) return null;

        return new DiarioObraDto
        {
            Id = diario.Id,
            ObraId = diario.ObraId,
            Data = diario.Data,
            Clima = diario.Clima,
            EquipePresente = diario.EquipePresente,
            AtividadesRealizadas = diario.AtividadesRealizadas,
            ComentariosTecnicos = diario.ComentariosTecnicos,
            Fotos = diario.Fotos.ToList()
        };
    }

    public async Task<DiarioObraDto?> CreateDiarioAsync(int obraId, CreateDiarioObraDto createDto)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null;

        var diario = new DiarioObra
        {
            ObraId = obraId,
            Data = createDto.Data,
            Clima = createDto.Clima,
            EquipePresente = createDto.EquipePresente,
            AtividadesRealizadas = createDto.AtividadesRealizadas,
            ComentariosTecnicos = createDto.ComentariosTecnicos
        };

        _context.DiariosDeObra.Add(diario);
        await _context.SaveChangesAsync();

        return new DiarioObraDto { Id = diario.Id, ObraId = diario.ObraId, Data = diario.Data, Clima = diario.Clima };
    }

    public async Task<bool> UpdateDiarioAsync(int obraId, int diarioId, CreateDiarioObraDto updateDto)
    {
        var diario = await _context.DiariosDeObra.FirstOrDefaultAsync(d => d.Id == diarioId && d.ObraId == obraId);
        if (diario == null) return false;

        diario.Data = updateDto.Data;
        diario.Clima = updateDto.Clima;
        diario.EquipePresente = updateDto.EquipePresente;
        diario.AtividadesRealizadas = updateDto.AtividadesRealizadas;
        diario.ComentariosTecnicos = updateDto.ComentariosTecnicos;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteDiarioAsync(int obraId, int diarioId)
    {
        var diario = await _context.DiariosDeObra.FirstOrDefaultAsync(d => d.Id == diarioId && d.ObraId == obraId);
        if (diario == null) return false;

        _context.DiariosDeObra.Remove(diario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> UploadFotoAsync(int obraId, int diarioId, UploadFileDto model)
    {
        var diario = await _context.DiariosDeObra.FirstOrDefaultAsync(d => d.Id == diarioId && d.ObraId == obraId);
        if (diario == null) return null;

        if (model.File == null || model.File.Length == 0)
        {
            return "Nenhum arquivo enviado."; // Retorna mensagem de erro
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(model.File.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return "Tipo de arquivo não permitido. Apenas JPG e PNG são aceitos.";
        }

        var maxFileSize = 25 * 1024 * 1024; // 25 MB
        if (model.File.Length > maxFileSize)
        {
            return $"Tamanho do arquivo excede o limite de {maxFileSize / (1024 * 1024)}MB.";
        }

        var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        diario.Fotos.Add($"/uploads/diarios/{uniqueFileName}");
        await _context.SaveChangesAsync();

        return $"/uploads/diarios/{uniqueFileName}"; // Retorna o caminho do arquivo
    }
}
