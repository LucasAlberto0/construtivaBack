
using construtivaBack.Models;

namespace construtivaBack.DTOs;

public class RelatorioResumoObraDto
{
    public int ObraId { get; set; }
    public string NomeObra { get; set; } = string.Empty;
    public string StatusObra { get; set; } = string.Empty;
    public int TotalDiarios { get; set; }
    public int TotalDocumentos { get; set; }
    public int TotalManutencoes { get; set; }
    public int TotalAditivosAprovados { get; set; }
    public int TotalAditivosPendentes { get; set; }
    public string ProgressoChecklistInicio { get; set; } = string.Empty;
    public string ProgressoChecklistEntrega { get; set; } = string.Empty;
}

public class RelatorioDiarioPorPeriodoDto
{
    public int DiarioId { get; set; }
    public DateTime Data { get; set; }
    public string? Clima { get; set; }
    public string? AtividadesRealizadas { get; set; }
    public List<string> Fotos { get; set; } = new();
}

public class RelatorioAditivoDto
{
    public int AditivoId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Aprovado { get; set; }
    public DateTime? DataAprovacao { get; set; }
}

public class RelatorioChecklistDto
{
    public ChecklistTipo Tipo { get; set; }
    public string DescricaoItem { get; set; } = string.Empty;
    public bool Concluido { get; set; }
}
