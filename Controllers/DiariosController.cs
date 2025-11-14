using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/obras/{obraId}/Diarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiarioObraListagemDto>>> GetDiarios(int obraId)
        {
            var diarios = await _diarioService.ObterTodosDiariosAsync(obraId);
            return Ok(diarios);
        }

        // GET: api/obras/{obraId}/Diarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiarioObraDetalhesDto>> GetDiario(int id)
        {
            var diario = await _diarioService.ObterDiarioPorIdAsync(id);
            if (diario == null)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return Ok(diario);
        }

        // POST: api/obras/{obraId}/Diarios
        [HttpPost]
        public async Task<ActionResult<DiarioObraDetalhesDto>> PostDiario(int obraId, [FromBody] DiarioObraCriacaoDto diarioDto)
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

        // PUT: api/obras/{obraId}/Diarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiario(int id, [FromBody] DiarioObraAtualizacaoDto diarioDto)
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

        // DELETE: api/obras/{obraId}/Diarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiario(int id)
        {
            var result = await _diarioService.ExcluirDiarioAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return NoContent();
        }

        // POST: api/obras/{obraId}/Diarios/{diarioId}/fotos
        [HttpPost("{diarioId}/fotos")]
        public async Task<ActionResult<FotoDiarioDto>> AdicionarFoto(int diarioId, [FromBody] string urlFoto)
        {
            var foto = await _diarioService.AdicionarFotoDiarioAsync(diarioId, urlFoto);
            if (foto == null)
            {
                return NotFound(new { Message = "Diário de Obra não encontrado." });
            }
            return CreatedAtAction(nameof(GetDiario), new { id = diarioId }, foto); // Retorna 201 com a foto adicionada
        }

        // DELETE: api/obras/{obraId}/Diarios/{diarioId}/fotos/{fotoId}
        [HttpDelete("{diarioId}/fotos/{fotoId}")]
        public async Task<IActionResult> RemoverFoto(int fotoId)
        {
            var result = await _diarioService.RemoverFotoDiarioAsync(fotoId);
            if (!result)
            {
                return NotFound(new { Message = "Foto não encontrada." });
            }
            return NoContent();
        }

        // POST: api/obras/{obraId}/Diarios/{diarioId}/comentarios
        [HttpPost("{diarioId}/comentarios")]
        public async Task<ActionResult<ComentarioDto>> AdicionarComentario(int diarioId, [FromBody] ComentarioCriacaoDto comentarioDto)
        {
            try
            {
                var comentario = await _diarioService.AdicionarComentarioDiarioAsync(diarioId, comentarioDto);
                if (comentario == null)
                {
                    return NotFound(new { Message = "Diário de Obra não encontrado." });
                }
                return CreatedAtAction(nameof(GetDiario), new { id = diarioId }, comentario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/obras/{obraId}/Diarios/{diarioId}/comentarios/{comentarioId}
        [HttpDelete("{diarioId}/comentarios/{comentarioId}")]
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
