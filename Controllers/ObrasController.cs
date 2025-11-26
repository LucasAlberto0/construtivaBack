using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;

namespace construtivaBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ObrasController : ControllerBase
    {
        private readonly IObraService _obraService;

        public ObrasController(IObraService obraService)
        {
            _obraService = obraService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ObraListagemDto>>>> GetObras()
        {
            var obras = await _obraService.ObterTodasObrasAsync();
            return Ok(ApiResponse<IEnumerable<ObraListagemDto>>.CreateSuccess(obras, "Obras listadas com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ObraDetalhesDto>>> GetObra(int id)
        {
            var obra = await _obraService.ObterObraPorIdAsync(id);
            if (obra == null)
            {
                return NotFound(ApiResponse<ObraDetalhesDto>.CreateError("Obra não encontrada."));
            }
            return Ok(ApiResponse<ObraDetalhesDto>.CreateSuccess(obra, "Obra encontrada com sucesso."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<ActionResult<ApiResponse<ObraDetalhesDto>>> PostObra([FromBody] ObraCriacaoDto obraDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<ObraDetalhesDto>.CreateError("Erro de validação.", errors));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<ObraDetalhesDto>.CreateError("Usuário não autenticado."));
            }

            var obraCriada = await _obraService.CriarObraAsync(obraDto, userId);
            var response = ApiResponse<ObraDetalhesDto>.CreateSuccess(obraCriada, "Obra criada com sucesso.");
            return CreatedAtAction(nameof(GetObra), new { id = obraCriada.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<ActionResult<ApiResponse<ObraDetalhesDto>>> PutObra(int id, [FromBody] ObraAtualizacaoDto obraDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<ObraDetalhesDto>.CreateError("Erro de validação.", errors));
            }

            var obraAtualizada = await _obraService.AtualizarObraAsync(id, obraDto);
            if (obraAtualizada == null)
            {
                return NotFound(ApiResponse<ObraDetalhesDto>.CreateError("Obra não encontrada para atualização."));
            }
            return Ok(ApiResponse<ObraDetalhesDto>.CreateSuccess(obraAtualizada, "Obra atualizada com sucesso."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteObra(int id)
        {
            var result = await _obraService.ExcluirObraAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<object>.CreateError("Obra não encontrada para exclusão."));
            }
            return Ok(ApiResponse<object>.CreateSuccess(null, "Obra excluída com sucesso."));
        }
    }
}
