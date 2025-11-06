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
[Route("api/obras/{obraId}/comentarios")]
[ApiController]
public class ComentariosController : ControllerBase
{
    private readonly IComentarioService _comentarioService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public ComentariosController(IComentarioService comentarioService, IObraService obraService)
    {
        _comentarioService = comentarioService;
        _obraService = obraService;
    }

    // GET: api/obras/5/comentarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComentarioDto>>> GetComentarios(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var comentarios = await _comentarioService.GetComentariosByObraIdAsync(obraId);
        return Ok(comentarios);
    }

    // POST: api/obras/5/comentarios
    [HttpPost]
    public async Task<ActionResult<ComentarioDto>> PostComentario(int obraId, CreateComentarioDto createDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var comentarioDto = await _comentarioService.CreateComentarioAsync(obraId, createDto, userId);
        if (comentarioDto == null) return NotFound("Obra not found.");

        return CreatedAtAction(nameof(GetComentarios), new { obraId = obraId, id = comentarioDto.Id }, comentarioDto);
    }

    // DELETE: api/obras/5/comentarios/1
    [HttpDelete("{comentarioId}")]
    public async Task<IActionResult> DeleteComentario(int obraId, int comentarioId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var isAdmin = User.IsInRole("Administrador");

        var result = await _comentarioService.DeleteComentarioAsync(obraId, comentarioId, userId, isAdmin);
        if (!result) return Forbid(); // Retorna Forbid se não autorizado ou NotFound se não encontrado

        return NoContent();
    }
}