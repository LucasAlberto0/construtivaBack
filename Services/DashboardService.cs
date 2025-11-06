
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var obras = await _context.Obras.ToListAsync();

        var stats = new DashboardStatsDto
        {
            EmAndamento = obras.Count(o => o.Status == ObraStatus.EmAndamento),
            Manutencao = obras.Count(o => o.Status == ObraStatus.Manutencao),
            Suspenso = obras.Count(o => o.Status == ObraStatus.Suspenso),
            Finalizado = obras.Count(o => o.Status == ObraStatus.Finalizado),
            TotalObras = obras.Count()
        };

        return stats;
    }
}
