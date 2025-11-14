using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Adicionado para Claims

namespace construtivaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protege todos os endpoints deste controller
    public class ObrasController : ControllerBase
    {
        private readonly IObraService _obraService;

        public ObrasController(IObraService obraService)
        {
            _obraService = obraService;
        }

        // GET: api/Obras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObraListagemDto>>> GetObras()
        {
            var obras = await _obraService.ObterTodasObrasAsync();
            return Ok(obras);
        }

        // GET: api/Obras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObraDetalhesDto>> GetObra(int id)
        {
            var obra = await _obraService.ObterObraPorIdAsync(id);
            if (obra == null)
            {
                return NotFound(new { Message = "Obra não encontrada." });
            }
            return Ok(obra);
        }

        // POST: api/Obras
        [HttpPost]
        public async Task<ActionResult<ObraDetalhesDto>> PostObra([FromBody] ObraCriacaoDto obraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Usuário não autenticado." });
            }

            var obraCriada = await _obraService.CriarObraAsync(obraDto, userId);
            return CreatedAtAction(nameof(GetObra), new { id = obraCriada.Id }, obraCriada);
        }

        // PUT: api/Obras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObra(int id, [FromBody] ObraAtualizacaoDto obraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obraAtualizada = await _obraService.AtualizarObraAsync(id, obraDto);
            if (obraAtualizada == null)
            {
                return NotFound(new { Message = "Obra não encontrada." });
            }
            return Ok(obraAtualizada);
        }

        // DELETE: api/Obras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObra(int id)
        {
            var result = await _obraService.ExcluirObraAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Obra não encontrada." });
            }
            return NoContent();
        }
    }
}
