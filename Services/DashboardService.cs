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
                    .OrderByDescending(o => o.DataInicio ?? DateTime.MinValue)
                    .Take(5)
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

        public async Task<OverallProjectStatsDto> GetOverallProjectStatsAsync()
        {
            var allObras = await _context.Obras
                                        .Include(o => o.Documentos)
                                        .ToListAsync();

            var stats = new OverallProjectStatsDto
            {
                TotalProjects = allObras.Count,
                ProjectsByStatus = allObras
                    .GroupBy(o => o.Status.ToString())
                    .ToDictionary(g => g.Key, g => g.Count()),
                ProjectsByLocation = allObras
                    .Where(o => !string.IsNullOrEmpty(o.Localizacao))
                    .GroupBy(o => o.Localizacao)
                    .ToDictionary(g => g.Key, g => g.Count()),
                ProjectsByCoordinator = allObras
                    .Where(o => !string.IsNullOrEmpty(o.CoordenadorNome))
                    .GroupBy(o => o.CoordenadorNome)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            var completedProjectsWithDates = allObras
                .Where(o => o.Status == ObraStatus.Finalizado && o.DataInicio.HasValue && o.DataTermino.HasValue)
                .ToList();

            if (completedProjectsWithDates.Any())
            {
                stats.AverageProjectDurationDays = completedProjectsWithDates
                    .Average(o => (o.DataTermino.Value - o.DataInicio.Value).TotalDays);
            }
            else
            {
                stats.AverageProjectDurationDays = 0;
            }

            if (allObras.Any())
            {
                stats.AverageDocumentsPerProject = allObras.Average(o => o.Documentos.Count);
            }
            else
            {
                stats.AverageDocumentsPerProject = 0;
            }

            return stats;
        }
    }
}
