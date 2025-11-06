
using construtivaBack.DTOs;

namespace construtivaBack.Services;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
