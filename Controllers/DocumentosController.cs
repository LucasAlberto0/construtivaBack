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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentoListagemDto>>> GetDocumentos(int obraId)
        {
            var documentos = await _documentoService.ObterTodosDocumentosAsync(obraId);
            return Ok(documentos);
        }

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DocumentoDetalhesDto>> PostDocumento(int obraId, [FromBody] DocumentoCriacaoDto documentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
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

        [HttpPost("{id}/anexar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DocumentoDetalhesDto>> AnexarArquivo(int id, [FromForm] DocumentoAnexoRequestDto anexoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var documentoAtualizado = await _documentoService.AnexarArquivoDocumentoAsync(id, anexoDto);
                if (documentoAtualizado == null)
                {
                    return NotFound(new { Message = "Documento não encontrado." });
                }
                return Ok(documentoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadDocumento(int obraId, int id)
        {
            var (fileContents, contentType, fileName) = await _documentoService.DownloadDocumentoAsync(id);

            if (fileContents == null || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(contentType))
            {
                return NotFound(new { Message = "Documento ou conteúdo não encontrado." });
            }

            return File(fileContents, contentType, fileName);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
