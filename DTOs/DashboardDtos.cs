using System.Collections.Generic;
using construtivaBack.Models;

namespace construtivaBack.DTOs
{
    public class DashboardSummaryDto
    {
        public int TotalObras { get; set; }
        public int ObrasEmAndamento { get; set; }
        public int ObrasEmManutencao { get; set; }
        public int ObrasSuspensas { get; set; }
        public int ObrasFinalizadas { get; set; }
        public List<ObraListagemDto> ObrasRecentes { get; set; } = new List<ObraListagemDto>();
    }

    public class OverallProjectStatsDto
    {
        public int TotalProjects { get; set; }
        public Dictionary<string, int> ProjectsByStatus { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ProjectsByLocation { get; set; } = new Dictionary<string, int>();
        public double AverageProjectDurationDays { get; set; } // For completed projects
        public Dictionary<string, int> ProjectsByCoordinator { get; set; } = new Dictionary<string, int>();
        public double AverageDocumentsPerProject { get; set; }
    }
}
