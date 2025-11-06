
using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/relatorios")]
[ApiController]
public class RelatoriosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RelatoriosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/obras/5/relatorios?tipo=resumo&formato=json (simulado)
    [HttpGet]
    public async Task<IActionResult> GerarRelatorio(int obraId, [FromQuery] string tipo, [FromQuery] string formato = "json", [FromQuery] DateTime? dataInicio = null, [FromQuery] DateTime? dataFim = null)
    {
        var obra = await _context.Obras.FirstOrDefaultAsync(o => o.Id == obraId);
        if (obra == null) return NotFound("Obra não encontrada.");

        object relatorioData;

        switch (tipo.ToLower())
        {
            case "resumo":
                relatorioData = await GetRelatorioResumoObra(obraId);
                break;
            case "aditivos":
                relatorioData = await GetRelatorioAditivos(obraId);
                break;
            case "checklists":
                relatorioData = await GetRelatorioChecklists(obraId);
                break;
            case "diario":
                relatorioData = await GetRelatorioDiarioPorPeriodo(obraId, dataInicio, dataFim);
                break;
            default:
                return BadRequest("Tipo de relatório inválido. Tipos aceitos: resumo, aditivos, checklists, diario.");
        }

        // Em um cenário real, aqui você usaria uma biblioteca para gerar PDF/CSV
        // e retornaria File(bytes, contentType, fileName).
        // Por enquanto, retornamos o JSON com os dados.
        if (formato.ToLower() == "json")
        {
            return Ok(relatorioData);
        }
        else if (formato.ToLower() == "pdf" || formato.ToLower() == "csv")
        {
            return Ok(new { message = $"Simulação de geração de relatório {tipo} em formato {formato}. Dados abaixo.", data = relatorioData });
        }
        else
        {
            return BadRequest("Formato de relatório inválido. Formatos aceitos: json, pdf, csv.");
        }
    }

    private async Task<RelatorioResumoObraDto> GetRelatorioResumoObra(int obraId)
    {
        var obra = await _context.Obras.FirstOrDefaultAsync(o => o.Id == obraId);
        if (obra == null) return new RelatorioResumoObraDto();

        var totalDiarios = await _context.DiariosDeObra.CountAsync(d => d.ObraId == obraId);
        var totalDocumentos = await _context.Documentos.CountAsync(d => d.ObraId == obraId && !d.IsDeleted);
        var totalManutencoes = await _context.Manutencoes.CountAsync(m => m.ObraId == obraId);
        var totalAditivosAprovados = await _context.Aditivos.CountAsync(a => a.ObraId == obraId && a.Aprovado);
        var totalAditivosPendentes = await _context.Aditivos.CountAsync(a => a.ObraId == obraId && !a.Aprovado);

        var checklistInicio = await _context.Checklists.Include(c => c.Itens).FirstOrDefaultAsync(c => c.ObraId == obraId && c.Tipo == ChecklistTipo.InicioObra);
        var checklistEntrega = await _context.Checklists.Include(c => c.Itens).FirstOrDefaultAsync(c => c.ObraId == obraId && c.Tipo == ChecklistTipo.EntregaObra);

        string progressoInicio = "N/A";
        if (checklistInicio != null && checklistInicio.Itens.Any())
        {
            var concluidos = checklistInicio.Itens.Count(i => i.Concluido);
            progressoInicio = $"{concluidos}/{checklistInicio.Itens.Count}";
        }

        string progressoEntrega = "N/A";
        if (checklistEntrega != null && checklistEntrega.Itens.Any())
        {
            var concluidos = checklistEntrega.Itens.Count(i => i.Concluido);
            progressoEntrega = $"{concluidos}/{checklistEntrega.Itens.Count}";
        }

        return new RelatorioResumoObraDto
        {
            ObraId = obra.Id,
            NomeObra = obra.Nome,
            StatusObra = obra.Status.ToString(),
            TotalDiarios = totalDiarios,
            TotalDocumentos = totalDocumentos,
            TotalManutencoes = totalManutencoes,
            TotalAditivosAprovados = totalAditivosAprovados,
            TotalAditivosPendentes = totalAditivosPendentes,
            ProgressoChecklistInicio = progressoInicio,
            ProgressoChecklistEntrega = progressoEntrega
        };
    }

    private async Task<List<RelatorioAditivoDto>> GetRelatorioAditivos(int obraId)
    {
        return await _context.Aditivos
            .Where(a => a.ObraId == obraId)
            .Select(a => new RelatorioAditivoDto
            {
                AditivoId = a.Id,
                Descricao = a.Descricao,
                Aprovado = a.Aprovado,
                DataAprovacao = a.DataAprovacao
            })
            .ToListAsync();
    }

    private async Task<List<RelatorioChecklistDto>> GetRelatorioChecklists(int obraId)
    {
        return await _context.Checklists
            .Where(c => c.ObraId == obraId)
            .Include(c => c.Itens)
            .SelectMany(c => c.Itens.Select(ci => new RelatorioChecklistDto
            {
                Tipo = c.Tipo,
                DescricaoItem = ci.Descricao,
                Concluido = ci.Concluido
            }))
            .ToListAsync();
    }

    private async Task<List<RelatorioDiarioPorPeriodoDto>> GetRelatorioDiarioPorPeriodo(int obraId, DateTime? dataInicio, DateTime? dataFim)
    {
        var query = _context.DiariosDeObra
            .Where(d => d.ObraId == obraId);

        if (dataInicio.HasValue)
        {
            query = query.Where(d => d.Data >= dataInicio.Value);
        }
        if (dataFim.HasValue)
        {
            query = query.Where(d => d.Data <= dataFim.Value);
        }

        return await query
            .Select(d => new RelatorioDiarioPorPeriodoDto
            {
                DiarioId = d.Id,
                Data = d.Data,
                Clima = d.Clima,
                AtividadesRealizadas = d.AtividadesRealizadas,
                Fotos = d.Fotos.ToList()
            })
            .ToListAsync();
    }
}
