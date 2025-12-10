using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace construtivaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetDashboardSummary()
        {
            var summary = await _dashboardService.ObterResumoDashboardAsync();
            return Ok(summary);
        }

        [HttpGet("overall-stats")]
        public async Task<ActionResult<OverallProjectStatsDto>> GetOverallProjectStats()
        {
            var stats = await _dashboardService.GetOverallProjectStatsAsync();
            return Ok(stats);
        }
    }
}
