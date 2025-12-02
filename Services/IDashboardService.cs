using construtivaBack.DTOs;

namespace construtivaBack.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> ObterResumoDashboardAsync();
        Task<OverallProjectStatsDto> GetOverallProjectStatsAsync();
    }
}
