using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/checklists")]
[ApiController]
public class ChecklistsController : ControllerBase
{
    private readonly IChecklistService _checklistService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public ChecklistsController(IChecklistService checklistService, IObraService obraService)
    {
        _checklistService = checklistService;
        _obraService = obraService;
    }

    // GET: api/obras/5/checklists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChecklistDto>>> GetChecklists(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var checklists = await _checklistService.GetChecklistsByObraIdAsync(obraId);
        return Ok(checklists);
    }

    // POST: api/obras/5/checklists
    [HttpPost]
    public async Task<ActionResult<ChecklistDto>> PostChecklist(int obraId, CreateChecklistDto createDto)
    {
        var checklistDto = await _checklistService.CreateChecklistAsync(obraId, createDto);
        if (checklistDto == null) return NotFound("Obra not found.");

        return CreatedAtAction(nameof(GetChecklists), new { obraId = obraId, id = checklistDto.Id }, checklistDto);
    }

    // PUT: api/obras/5/checklists/1/itens/1
    [HttpPut("{checklistId}/itens/{itemId}")]
    public async Task<IActionResult> UpdateChecklistItem(int obraId, int checklistId, int itemId, UpdateChecklistItemDto updateDto)
    {
        var result = await _checklistService.UpdateChecklistItemAsync(obraId, checklistId, itemId, updateDto);
        if (!result) return NotFound();

        return NoContent();
    }

    // DELETE: api/obras/5/checklists/1
    [HttpDelete("{checklistId}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteChecklist(int obraId, int checklistId)
    {
        var result = await _checklistService.DeleteChecklistAsync(obraId, checklistId);
        if (!result) return NotFound();

        return NoContent();
    }
}