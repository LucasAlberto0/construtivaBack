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
}
