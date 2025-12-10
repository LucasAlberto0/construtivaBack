using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace construtivaBack.Controllers
{
    [Route("api/obras/{obraId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistService _checklistService;

        public ChecklistsController(IChecklistService checklistService)
        {
            _checklistService = checklistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistListagemDto>>> GetChecklists(int obraId)
        {
            var checklists = await _checklistService.ObterTodosChecklistsAsync(obraId);
            return Ok(checklists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChecklistDetalhesDto>> GetChecklist(int id)
        {
            var checklist = await _checklistService.ObterChecklistPorIdAsync(id);
            if (checklist == null)
            {
                return NotFound(new { Message = "Checklist não encontrado." });
            }
            return Ok(checklist);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<ActionResult<ChecklistDetalhesDto>> PostChecklist(int obraId, [FromBody] ChecklistCriacaoDto checklistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (obraId != checklistDto.ObraId)
            {
                return BadRequest(new { Message = "O ID da obra na rota não corresponde ao ID da obra no corpo da requisição." });
            }

            try
            {
                var checklistCriado = await _checklistService.CriarChecklistAsync(checklistDto);
                return CreatedAtAction(nameof(GetChecklist), new { obraId = checklistCriado.ObraId, id = checklistCriado.Id }, checklistCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<IActionResult> PutChecklist(int id, [FromBody] ChecklistAtualizacaoDto checklistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checklistAtualizado = await _checklistService.AtualizarChecklistAsync(id, checklistDto);
            if (checklistAtualizado == null)
            {
                return NotFound(new { Message = "Checklist não encontrado." });
            }
            return Ok(checklistAtualizado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<IActionResult> DeleteChecklist(int id)
        {
            var result = await _checklistService.ExcluirChecklistAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Checklist não encontrado." });
            }
            return NoContent();
        }

        [HttpPost("{checklistId}/itens")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<ActionResult<ChecklistItemDto>> PostChecklistItem(int checklistId, [FromBody] ChecklistItemCriacaoDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemCriado = await _checklistService.AdicionarItemChecklistAsync(checklistId, itemDto);
            if (itemCriado == null)
            {
                return NotFound(new { Message = "Checklist não encontrado." });
            }
            return CreatedAtAction(nameof(GetChecklist), new { id = checklistId }, itemCriado);
        }

        [HttpPut("{checklistId}/itens/{itemId}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<IActionResult> PutChecklistItem(int itemId, [FromBody] ChecklistItemAtualizacaoDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (itemId != itemDto.Id)
            {
                return BadRequest(new { Message = "O ID do item na rota não corresponde ao ID do item no corpo da requisição." });
            }

            var itemAtualizado = await _checklistService.AtualizarItemChecklistAsync(itemId, itemDto);
            if (itemAtualizado == null)
            {
                return NotFound(new { Message = "Item do Checklist não encontrado." });
            }
            return Ok(itemAtualizado);
        }

        [HttpDelete("{checklistId}/itens/{itemId}")]
        [Authorize(Roles = "Admin,Coordenador")]
        public async Task<IActionResult> DeleteChecklistItem(int itemId)
        {
            var result = await _checklistService.ExcluirItemChecklistAsync(itemId);
            if (!result)
            {
                return NotFound(new { Message = "Item do Checklist não encontrado." });
            }
            return NoContent();
        }
    }
}
