using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace construtivaBack.Controllers
{
    [Route("api/obras/{obraId}/[controller]")]
    [ApiController]
    [Authorize]
    public class AditivosController : ControllerBase
    {
        private readonly IAditivoService _aditivoService;

        public AditivosController(IAditivoService aditivoService)
        {
            _aditivoService = aditivoService;
        }

        // GET: api/obras/{obraId}/Aditivos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AditivoListagemDto>>> GetAditivos(int obraId)
        {
            var aditivos = await _aditivoService.ObterTodosAditivosAsync(obraId);
            return Ok(aditivos);
        }

        // GET: api/obras/{obraId}/Aditivos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AditivoDetalhesDto>> GetAditivo(int id)
        {
            var aditivo = await _aditivoService.ObterAditivoPorIdAsync(id);
            if (aditivo == null)
            {
                return NotFound(new { Message = "Aditivo não encontrado." });
            }
            return Ok(aditivo);
        }

        // POST: api/obras/{obraId}/Aditivos
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AditivoDetalhesDto>> PostAditivo(int obraId, [FromBody] AditivoCriacaoDto aditivoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (obraId != aditivoDto.ObraId)
            {
                return BadRequest(new { Message = "O ID da obra na rota não corresponde ao ID da obra no corpo da requisição." });
            }

            try
            {
                var aditivoCriado = await _aditivoService.CriarAditivoAsync(aditivoDto);
                return CreatedAtAction(nameof(GetAditivo), new { obraId = aditivoCriado.ObraId, id = aditivoCriado.Id }, aditivoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/obras/{obraId}/Aditivos/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAditivo(int id, [FromBody] AditivoAtualizacaoDto aditivoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aditivoAtualizado = await _aditivoService.AtualizarAditivoAsync(id, aditivoDto);
            if (aditivoAtualizado == null)
            {
                return NotFound(new { Message = "Aditivo não encontrado." });
            }
            return Ok(aditivoAtualizado);
        }

        // DELETE: api/obras/{obraId}/Aditivos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAditivo(int id)
        {
            var result = await _aditivoService.ExcluirAditivoAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Aditivo não encontrado." });
            }
            return NoContent();
        }
    }
}
