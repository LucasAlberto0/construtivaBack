
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class ComentarioService : IComentarioService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ComentarioService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<ComentarioDto>> GetComentariosByObraIdAsync(int obraId)
    {
        return await _context.Comentarios
            .Where(c => c.ObraId == obraId)
            .Include(c => c.User)
            .OrderByDescending(c => c.Timestamp)
            .Select(c => new ComentarioDto
            {
                Id = c.Id,
                ObraId = c.ObraId,
                UserId = c.UserId,
                UserName = c.User != null ? c.User.NomeCompleto ?? c.User.Email : "Desconhecido",
                Texto = c.Texto,
                Timestamp = c.Timestamp
            })
            .ToListAsync();
    }

    public async Task<ComentarioDto?> CreateComentarioAsync(int obraId, CreateComentarioDto createDto, string userId)
    {
        var obraExists = await _context.Obras.AnyAsync(o => o.Id == obraId);
        if (!obraExists) return null;

        var comentario = new Comentario
        {
            ObraId = obraId,
            UserId = userId,
            Texto = createDto.Texto,
            Timestamp = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(userId);
        return new ComentarioDto
        {
            Id = comentario.Id,
            ObraId = comentario.ObraId,
            UserId = comentario.UserId,
            UserName = user != null ? user.NomeCompleto ?? user.Email : "Desconhecido",
            Texto = comentario.Texto,
            Timestamp = comentario.Timestamp
        };
    }

    public async Task<bool> DeleteComentarioAsync(int obraId, int comentarioId, string userId, bool isAdmin)
    {
        var comentario = await _context.Comentarios.FirstOrDefaultAsync(c => c.Id == comentarioId && c.ObraId == obraId);
        if (comentario == null) return false;

        if (comentario.UserId != userId && !isAdmin)
        {
            return false; // Não autorizado
        }

        _context.Comentarios.Remove(comentario);
        await _context.SaveChangesAsync();
        return true;
    }
}
