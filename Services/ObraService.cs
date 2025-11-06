
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class ObraService : IObraService
{
    private readonly ApplicationDbContext _context;

    public ObraService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ObraDto>> GetAllObrasAsync()
    {
        return await _context.Obras
            .Select(o => new ObraDto
            {
                Id = o.Id,
                Nome = o.Nome,
                Localizacao = o.Localizacao,
                Contratante = o.Contratante,
                Contrato = o.Contrato,
                OrdemDeServico = o.OrdemDeServico,
                Status = o.Status.ToString()
            })
            .ToListAsync();
    }

    public async Task<ObraDto?> GetObraByIdAsync(int id)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra == null) return null;

        return new ObraDto
        {
            Id = obra.Id,
            Nome = obra.Nome,
            Localizacao = obra.Localizacao,
            Contratante = obra.Contratante,
            Contrato = obra.Contrato,
            OrdemDeServico = obra.OrdemDeServico,
            Status = obra.Status.ToString()
        };
    }

    public async Task<ObraDto> CreateObraAsync(CreateObraDto createObraDto)
    {
        var obra = new Obra
        {
            Nome = createObraDto.Nome,
            Localizacao = createObraDto.Localizacao,
            Contratante = createObraDto.Contratante,
            Contrato = createObraDto.Contrato,
            OrdemDeServico = createObraDto.OrdemDeServico,
            Status = createObraDto.Status
        };

        _context.Obras.Add(obra);
        await _context.SaveChangesAsync();

        return new ObraDto
        {
            Id = obra.Id,
            Nome = obra.Nome,
            Localizacao = obra.Localizacao,
            Contratante = obra.Contratante,
            Contrato = obra.Contrato,
            OrdemDeServico = obra.OrdemDeServico,
            Status = obra.Status.ToString()
        };
    }

    public async Task<bool> UpdateObraAsync(int id, CreateObraDto updateObraDto)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra == null) return false;

        obra.Nome = updateObraDto.Nome;
        obra.Localizacao = updateObraDto.Localizacao;
        obra.Contratante = updateObraDto.Contratante;
        obra.Contrato = updateObraDto.Contrato;
        obra.OrdemDeServico = updateObraDto.OrdemDeServico;
        obra.Status = updateObraDto.Status;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ObraExistsAsync(id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
        return true;
    }

    public async Task<bool> DeleteObraAsync(int id)
    {
        var obra = await _context.Obras.FindAsync(id);
        if (obra == null) return false;

        _context.Obras.Remove(obra);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ObraExistsAsync(int id)
    {
        return await _context.Obras.AnyAsync(e => e.Id == id);
    }
}
