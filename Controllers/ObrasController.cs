using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize] // Protege todos os endpoints neste controller
[Route("api/[controller]")]
[ApiController]
public class ObrasController : ControllerBase
{
    private readonly IObraService _obraService;

    public ObrasController(IObraService obraService)
    {
        _obraService = obraService;
    }

    // GET: api/Obras
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObraDto>>> GetObras()
    {
        var obras = await _obraService.GetAllObrasAsync();
        return Ok(obras);
    }

    // GET: api/Obras/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ObraDto>> GetObra(int id)
    {
        var obra = await _obraService.GetObraByIdAsync(id);

        if (obra == null)
        {
            return NotFound();
        }

        return obra;
    }

    // POST: api/Obras
    [HttpPost]
    public async Task<ActionResult<ObraDto>> PostObra(CreateObraDto createObraDto)
    {
        var obraDto = await _obraService.CreateObraAsync(createObraDto);
        return CreatedAtAction(nameof(GetObra), new { id = obraDto.Id }, obraDto);
    }

    // PUT: api/Obras/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutObra(int id, CreateObraDto updateObraDto)
    {
        var result = await _obraService.UpdateObraAsync(id, updateObraDto);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    // DELETE: api/Obras/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> DeleteObra(int id)
    {
        var result = await _obraService.DeleteObraAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}