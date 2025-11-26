using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace construtivaBack.Controllers
{
    [Route("api/obras/{obraId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ManutencoesController : ControllerBase
    {
        private readonly IManutencaoService _manutencaoService;

        public ManutencoesController(IManutencaoService manutencaoService)
        {
            _manutencaoService = manutencaoService;
        }

        // GET: api/obras/{obraId}/Manutencoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManutencaoListagemDto>>> GetManutencoes(int obraId)
        {
            var manutencoes = await _manutencaoService.ObterTodasManutencoesAsync(obraId);
            return Ok(manutencoes);
        }

        // GET: api/obras/{obraId}/Manutencoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManutencaoDetalhesDto>> GetManutencao(int id)
        {
            var manutencao = await _manutencaoService.ObterManutencaoPorIdAsync(id);
            if (manutencao == null)
            {
                return NotFound(new { Message = "Manutenção não encontrada." });
            }
            return Ok(manutencao);
        }

        // POST: api/obras/{obraId}/Manutencoes
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ManutencaoDetalhesDto>> PostManutencao(int obraId, [FromBody] ManutencaoCriacaoDto manutencaoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (obraId != manutencaoDto.ObraId)
            {
                return BadRequest(new { Message = "O ID da obra na rota não corresponde ao ID da obra no corpo da requisição." });
            }

            try
            {
                var manutencaoCriada = await _manutencaoService.CriarManutencaoAsync(manutencaoDto);
                return CreatedAtAction(nameof(GetManutencao), new { obraId = manutencaoCriada.ObraId, id = manutencaoCriada.Id }, manutencaoCriada);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/obras/{obraId}/Manutencoes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutManutencao(int id, [FromBody] ManutencaoAtualizacaoDto manutencaoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var manutencaoAtualizada = await _manutencaoService.AtualizarManutencaoAsync(id, manutencaoDto);
            if (manutencaoAtualizada == null)
            {
                return NotFound(new { Message = "Manutenção não encontrada." });
            }
            return Ok(manutencaoAtualizada);
        }

        // DELETE: api/obras/{obraId}/Manutencoes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteManutencao(int id)
        {
            var result = await _manutencaoService.ExcluirManutencaoAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Manutenção não encontrada." });
            }
            return NoContent();
        }
    }
}
