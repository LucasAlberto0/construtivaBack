using construtivaBack.DTOs;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace construtivaBack.Controllers
{
    [Route("api/obras/{obraId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentosController : ControllerBase
    {
        private readonly IDocumentoService _documentoService;

        public DocumentosController(IDocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        // GET: api/obras/{obraId}/Documentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoListagemDto>>> GetDocumentos(int obraId)
        {
            var documentos = await _documentoService.ObterTodosDocumentosAsync(obraId);
            return Ok(documentos);
        }

        // GET: api/obras/{obraId}/Documentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentoDetalhesDto>> GetDocumento(int id)
        {
            var documento = await _documentoService.ObterDocumentoPorIdAsync(id);
            if (documento == null)
            {
                return NotFound(new { Message = "Documento não encontrado." });
            }
            return Ok(documento);
        }

        // POST: api/obras/{obraId}/Documentos
        [HttpPost]
        public async Task<ActionResult<DocumentoDetalhesDto>> PostDocumento(int obraId, [FromBody] DocumentoCriacaoDto documentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (obraId != documentoDto.ObraId)
            {
                return BadRequest(new { Message = "O ID da obra na rota não corresponde ao ID da obra no corpo da requisição." });
            }

            try
            {
                var documentoCriado = await _documentoService.CriarDocumentoAsync(documentoDto);
                return CreatedAtAction(nameof(GetDocumento), new { obraId = documentoCriado.ObraId, id = documentoCriado.Id }, documentoCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/obras/{obraId}/Documentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, [FromBody] DocumentoAtualizacaoDto documentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var documentoAtualizado = await _documentoService.AtualizarDocumentoAsync(id, documentoDto);
            if (documentoAtualizado == null)
            {
                return NotFound(new { Message = "Documento não encontrado." });
            }
            return Ok(documentoAtualizado);
        }

        // DELETE: api/obras/{obraId}/Documentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var result = await _documentoService.ExcluirDocumentoAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Documento não encontrado." });
            }
            return NoContent();
        }
    }
}
