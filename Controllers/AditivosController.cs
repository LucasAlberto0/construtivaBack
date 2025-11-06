using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/aditivos")]
[ApiController]
public class AditivosController : ControllerBase
{
    private readonly IAditivoService _aditivoService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public AditivosController(IAditivoService aditivoService, IObraService obraService)
    {
        _aditivoService = aditivoService;
        _obraService = obraService;
    }

    // GET: api/obras/5/aditivos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AditivoDto>>> GetAditivos(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var aditivos = await _aditivoService.GetAditivosByObraIdAsync(obraId);
        return Ok(aditivos);
    }

    // POST: api/obras/5/aditivos
    [HttpPost]
    public async Task<ActionResult<AditivoDto>> PostAditivo(int obraId, CreateAditivoDto createDto)
    {
        var aditivoDto = await _aditivoService.CreateAditivoAsync(obraId, createDto);
        if (aditivoDto == null) return NotFound("Obra not found.");

        return CreatedAtAction(nameof(GetAditivos), new { obraId = obraId, id = aditivoDto.Id }, aditivoDto);
    }

    // PUT: api/obras/5/aditivos/1
    [HttpPut("{aditivoId}")]
    public async Task<IActionResult> PutAditivo(int obraId, int aditivoId, CreateAditivoDto updateDto)
    {
        var result = await _aditivoService.UpdateAditivoAsync(obraId, aditivoId, updateDto);
        if (!result) return NotFound();

        return NoContent();
    }

    // DELETE: api/obras/5/aditivos/1
    [HttpDelete("{aditivoId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteAditivo(int obraId, int aditivoId)
    {
        var result = await _aditivoService.DeleteAditivoAsync(obraId, aditivoId);
        if (!result) return NotFound();

        return NoContent();
    }

    // POST: api/obras/5/aditivos/1/upload-anexo
    [HttpPost("{aditivoId}/upload-anexo")]
    public async Task<IActionResult> UploadAnexo(int obraId, int aditivoId, [FromForm] UploadFileDto model)
    {
        var filePath = await _aditivoService.UploadAnexoAsync(obraId, aditivoId, model);
        if (filePath == null) return NotFound("Aditivo não encontrado.");
        if (filePath.StartsWith("Nenhum arquivo") || filePath.StartsWith("Tipo de arquivo") || filePath.StartsWith("Tamanho do arquivo"))
        {
            return BadRequest(filePath);
        }

        return Ok(new { message = "Anexo enviado com sucesso!", filePath = filePath });
    }

    // POST: api/obras/5/aditivos/1/aprovar
    [HttpPost("{aditivoId}/aprovar")]
    [Authorize(Roles = "Administrador,Coordenador")] // RN001: Apenas Admin ou Coordenador pode aprovar
    public async Task<IActionResult> AprovarAditivo(int obraId, int aditivoId, [FromBody] AprovarAditivoDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _aditivoService.AprovarAditivoAsync(obraId, aditivoId, model, userId);
        if (result == null) return NotFound("Aditivo não encontrado.");
        if (result != "OK") return BadRequest(result);

        return Ok(new { message = $"Aditivo {(model.Aprovar ? "aprovado" : "reprovado")} com sucesso!" });
    }
}