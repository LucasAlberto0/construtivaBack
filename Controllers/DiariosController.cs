using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;

namespace construtivaBack.Controllers
{
    [Route("api/obras/{obraId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DiariosController : ControllerBase
    {
        private readonly IDiarioService _diarioService;

        public DiariosController(IDiarioService diarioService)
        {
            _diarioService = diarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiarioObraListagemDto>>> GetDiarios(int obraId)
        {
            var diarios = await _diarioService.ObterTodosDiariosAsync(obraId);
            return Ok(diarios);
        }

        [HttpGet("{id}", Name = "GetDiarioById")]
        public async Task<ActionResult<DiarioObraDetalhesDto>> GetDiario(int id)
        {
            var diario = await _diarioService.ObterDiarioPorIdAsync(id);
            if (diario == null)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return Ok(diario);
        }

        [HttpGet("{id}/foto")]
        public async Task<IActionResult> GetDiarioFoto(int id)
        {
            var (foto, mimeType) = await _diarioService.ObterFotoDiarioAsync(id);
            if (foto == null || mimeType == null)
            {
                return NotFound();
            }
            return File(foto, mimeType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Fiscal")]
        public async Task<ActionResult<DiarioObraDetalhesDto>> PostDiario(int obraId, [FromForm] DiarioObraCriacaoDto diarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (obraId != diarioDto.ObraId)
            {
                return BadRequest(new { Message = "O ID da obra na rota não corresponde ao ID da obra no corpo da requisição." });
            }

            try
            {
                var diarioCriado = await _diarioService.CriarDiarioAsync(diarioDto);
                return CreatedAtAction(nameof(GetDiario), new { obraId = diarioCriado.ObraId, id = diarioCriado.Id }, diarioCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Fiscal")]
        public async Task<IActionResult> PutDiario(int id, [FromForm] DiarioObraAtualizacaoDto diarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var diarioAtualizado = await _diarioService.AtualizarDiarioAsync(id, diarioDto);
            if (diarioAtualizado == null)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return Ok(diarioAtualizado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Fiscal")]
        public async Task<IActionResult> DeleteDiario(int id)
        {
            var result = await _diarioService.ExcluirDiarioAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return NoContent();
        }

        [HttpPost("{diarioId}/comentarios")]
        [Authorize(Roles = "Admin,Fiscal")]
        public async Task<ActionResult<ComentarioDto>> AdicionarComentario(int obraId, int diarioId, [FromBody] ComentarioCriacaoDto comentarioDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Usuário não autenticado." });
            }

            try
            {
                var comentario = await _diarioService.AdicionarComentarioDiarioAsync(diarioId, comentarioDto, userId);
                if (comentario == null)
                {
                    return NotFound(new { Message = "Diário de Obra não encontrado." });
                }
                return CreatedAtRoute("GetDiarioById", new { obraId = obraId, id = diarioId }, comentario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{diarioId}/comentarios/{comentarioId}")]
        [Authorize(Roles = "Admin,Fiscal")]
        public async Task<IActionResult> RemoverComentario(int comentarioId)
        {
            var result = await _diarioService.RemoverComentarioDiarioAsync(comentarioId);
            if (!result)
            {
                return NotFound(new { Message = "Comentário não encontrado." });
            }
            return NoContent();
        }
    }
}
