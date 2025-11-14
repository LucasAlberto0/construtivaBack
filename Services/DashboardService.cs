using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> ObterResumoDashboardAsync()
        {
            var obras = await _context.Obras.ToListAsync();

            var summary = new DashboardSummaryDto
            {
                TotalObras = obras.Count,
                ObrasEmAndamento = obras.Count(o => o.Status == ObraStatus.EmAndamento),
                ObrasEmManutencao = obras.Count(o => o.Status == ObraStatus.EmManutencao),
                ObrasSuspensas = obras.Count(o => o.Status == ObraStatus.Suspenso),
                ObrasFinalizadas = obras.Count(o => o.Status == ObraStatus.Finalizado),
                ObrasRecentes = obras
                    .OrderByDescending(o => o.DataInicio ?? DateTime.MinValue) // Order by start date, or min value if null
                    .Take(5) // Get top 5 recent obras
                    .Select(o => new ObraListagemDto
                    {
                        Id = o.Id,
                        Nome = o.Nome,
                        Localizacao = o.Localizacao,
                        Status = o.Status,
                        DataInicio = o.DataInicio,
                        DataTermino = o.DataTermino
                    })
                    .ToList()
            };

            return summary;
        }
    }
}
