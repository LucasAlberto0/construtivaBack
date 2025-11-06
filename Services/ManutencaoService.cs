
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class ManutencaoService : IManutencaoService
{
    private readonly ApplicationDbContext _context;

    public ManutencaoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ManutencaoDto>> GetManutencoesByObraIdAsync(int obraId)
    {
        return await _context.Manutencoes
            .Where(m => m.ObraId == obraId)
            .Select(m => new ManutencaoDto
            {
                Id = m.Id,
                ObraId = m.ObraId,
                DataRegistro = m.DataRegistro,
                Descricao = m.Descricao,
                Imagens = m.Imagens.ToList()
            })
            .ToListAsync();
    }

    public async Task<ManutencaoDto?> CreateManutencaoAsync(int obraId, CreateManutencaoDto createDto)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null;

        var manutencao = new Manutencao
        {
            ObraId = obraId,
            DataRegistro = createDto.DataRegistro,
            Descricao = createDto.Descricao
        };

        _context.Manutencoes.Add(manutencao);
        await _context.SaveChangesAsync();

        return new ManutencaoDto { Id = manutencao.Id, ObraId = manutencao.ObraId, DataRegistro = manutencao.DataRegistro, Descricao = manutencao.Descricao };
    }

    public async Task<bool> UpdateManutencaoAsync(int obraId, int manutencaoId, CreateManutencaoDto updateDto)
    {
        var manutencao = await _context.Manutencoes.FirstOrDefaultAsync(m => m.Id == manutencaoId && m.ObraId == obraId);
        if (manutencao == null) return false;

        manutencao.DataRegistro = updateDto.DataRegistro;
        manutencao.Descricao = updateDto.Descricao;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteManutencaoAsync(int obraId, int manutencaoId)
    {
        var manutencao = await _context.Manutencoes.FirstOrDefaultAsync(m => m.Id == manutencaoId && m.ObraId == obraId);
        if (manutencao == null) return false;

        _context.Manutencoes.Remove(manutencao);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> UploadImagemAsync(int obraId, int manutencaoId, UploadFileDto model)
    {
        var manutencao = await _context.Manutencoes.FirstOrDefaultAsync(m => m.Id == manutencaoId && m.ObraId == obraId);
        if (manutencao == null) return null;

        if (model.File == null || model.File.Length == 0)
        {
            return "Nenhum arquivo enviado.";
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
        manutencao.Imagens.Add($"/uploads/manutencoes/{uniqueFileName}");
        await _context.SaveChangesAsync();

        return $"/uploads/manutencoes/{uniqueFileName}";
    }
}
