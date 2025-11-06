using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/diarios")]
[ApiController]
public class DiariosController : ControllerBase
{
    private readonly IDiarioService _diarioService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public DiariosController(IDiarioService diarioService, IObraService obraService)
    {
        _diarioService = diarioService;
        _obraService = obraService;
    }

    // GET: api/obras/5/diarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiarioObraDto>>> GetDiarios(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var diarios = await _diarioService.GetDiariosByObraIdAsync(obraId);
        return Ok(diarios);
    }

    // POST: api/obras/5/diarios
    [HttpPost]
    public async Task<ActionResult<DiarioObraDto>> PostDiario(int obraId, CreateDiarioObraDto createDto)
    {
        var diarioDto = await _diarioService.CreateDiarioAsync(obraId, createDto);
        if (diarioDto == null) return NotFound("Obra not found.");

        return CreatedAtAction(nameof(GetDiarios), new { obraId = obraId, id = diarioDto.Id }, diarioDto);
    }

    // PUT: api/obras/5/diarios/1
    [HttpPut("{diarioId}")]
    [Authorize(Roles = "Administrador")] // RN005 - Apenas Admin pode editar
    public async Task<IActionResult> PutDiario(int obraId, int diarioId, CreateDiarioObraDto updateDto)
    {
        var result = await _diarioService.UpdateDiarioAsync(obraId, diarioId, updateDto);
        if (!result) return NotFound();

        return NoContent();
    }

    // DELETE: api/obras/5/diarios/1
    [HttpDelete("{diarioId}")]
    [Authorize(Roles = "Administrador")] // Apenas Admin pode deletar
    public async Task<IActionResult> DeleteDiario(int obraId, int diarioId)
    {
        var result = await _diarioService.DeleteDiarioAsync(obraId, diarioId);
        if (!result) return NotFound();

        return NoContent();
    }

    // POST: api/obras/5/diarios/1/upload-foto
    [HttpPost("{diarioId}/upload-foto")]
    public async Task<IActionResult> UploadFoto(int obraId, int diarioId, [FromForm] UploadFileDto model)
    {
        var filePath = await _diarioService.UploadFotoAsync(obraId, diarioId, model);
        if (filePath == null) return NotFound("Diário de Obra não encontrado.");
        if (filePath.StartsWith("Nenhum arquivo") || filePath.StartsWith("Tipo de arquivo") || filePath.StartsWith("Tamanho do arquivo"))
        {
            return BadRequest(filePath);
        }

        return Ok(new { message = "Foto enviada com sucesso!", filePath = filePath });
    }
}