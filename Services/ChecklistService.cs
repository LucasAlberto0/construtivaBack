
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class ChecklistService : IChecklistService
{
    private readonly ApplicationDbContext _context;

    public ChecklistService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ChecklistDto>> GetChecklistsByObraIdAsync(int obraId)
    {
        return await _context.Checklists
            .Where(c => c.ObraId == obraId)
            .Include(c => c.Itens)
            .Select(c => new ChecklistDto
            {
                Id = c.Id,
                ObraId = c.ObraId,
                Tipo = c.Tipo,
                Itens = c.Itens.Select(ci => new ChecklistItemDto
                {
                    Id = ci.Id,
                    ChecklistId = ci.ChecklistId,
                    Descricao = ci.Descricao,
                    Concluido = ci.Concluido
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<ChecklistDto?> CreateChecklistAsync(int obraId, CreateChecklistDto createDto)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null;

        var checklist = new Checklist
        {
            ObraId = obraId,
            Tipo = createDto.Tipo
        };

        foreach (var itemDesc in createDto.ItensDescricao)
        {
            checklist.Itens.Add(new ChecklistItem { Descricao = itemDesc });
        }

        _context.Checklists.Add(checklist);
        await _context.SaveChangesAsync();

        return new ChecklistDto
        {
            Id = checklist.Id,
            ObraId = checklist.ObraId,
            Tipo = checklist.Tipo,
            Itens = checklist.Itens.Select(ci => new ChecklistItemDto
            {
                Id = ci.Id,
                ChecklistId = ci.ChecklistId,
                Descricao = ci.Descricao,
                Concluido = ci.Concluido
            }).ToList()
        };
    }

    public async Task<bool> UpdateChecklistItemAsync(int obraId, int checklistId, int itemId, UpdateChecklistItemDto updateDto)
    {
        var item = await _context.ChecklistItens
            .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.ChecklistId == checklistId && ci.Checklist!.ObraId == obraId);

        if (item == null) return false;

        item.Concluido = updateDto.Concluido;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteChecklistAsync(int obraId, int checklistId)
    {
        var checklist = await _context.Checklists.FirstOrDefaultAsync(c => c.Id == checklistId && c.ObraId == obraId);
        if (checklist == null) return false;

        _context.Checklists.Remove(checklist);
        await _context.SaveChangesAsync();

        return true;
    }
}
