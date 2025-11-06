using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/manutencoes")]
[ApiController]
public class ManutencoesController : ControllerBase
{
    private readonly IManutencaoService _manutencaoService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public ManutencoesController(IManutencaoService manutencaoService, IObraService obraService)
    {
        _manutencaoService = manutencaoService;
        _obraService = obraService;
    }

    // GET: api/obras/5/manutencoes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManutencaoDto>>> GetManutencoes(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var manutencoes = await _manutencaoService.GetManutencoesByObraIdAsync(obraId);
        return Ok(manutencoes);
    }

    // POST: api/obras/5/manutencoes
    [HttpPost]
    public async Task<ActionResult<ManutencaoDto>> PostManutencao(int obraId, CreateManutencaoDto createDto)
    {
        var manutencaoDto = await _manutencaoService.CreateManutencaoAsync(obraId, createDto);
        if (manutencaoDto == null) return NotFound("Obra not found.");

        return CreatedAtAction(nameof(GetManutencoes), new { obraId = obraId, id = manutencaoDto.Id }, manutencaoDto);
    }

    // PUT: api/obras/5/manutencoes/1
    [HttpPut("{manutencaoId}")]
    public async Task<IActionResult> PutManutencao(int obraId, int manutencaoId, CreateManutencaoDto updateDto)
    {
        var result = await _manutencaoService.UpdateManutencaoAsync(obraId, manutencaoId, updateDto);
        if (!result) return NotFound();

        return NoContent();
    }

    // DELETE: api/obras/5/manutencoes/1
    [HttpDelete("{manutencaoId}")]
    [Authorize(Roles = "Administrador")] // Exemplo de proteção
    public async Task<IActionResult> DeleteManutencao(int obraId, int manutencaoId)
    {
        var result = await _manutencaoService.DeleteManutencaoAsync(obraId, manutencaoId);
        if (!result) return NotFound();

        return NoContent();
    }

    // POST: api/obras/5/manutencoes/1/upload-imagem
    [HttpPost("{manutencaoId}/upload-imagem")]
    public async Task<IActionResult> UploadImagem(int obraId, int manutencaoId, [FromForm] UploadFileDto model)
    {
        var filePath = await _manutencaoService.UploadImagemAsync(obraId, manutencaoId, model);
        if (filePath == null) return NotFound("Manutenção não encontrada.");
        if (filePath.StartsWith("Nenhum arquivo") || filePath.StartsWith("Tipo de arquivo") || filePath.StartsWith("Tamanho do arquivo"))
        {
            return BadRequest(filePath);
        }

        return Ok(new { message = "Imagem enviada com sucesso!", filePath = filePath });
    }
}