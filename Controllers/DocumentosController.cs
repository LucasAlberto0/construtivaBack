using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/obras/{obraId}/documentos")]
[ApiController]
public class DocumentosController : ControllerBase
{
    private readonly IDocumentoService _documentoService;
    private readonly IObraService _obraService; // Para verificar se a obra existe

    public DocumentosController(IDocumentoService documentoService, IObraService obraService)
    {
        _documentoService = documentoService;
        _obraService = obraService;
    }

    // GET: api/obras/5/documentos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentoDto>>> GetDocumentos(int obraId)
    {
        if (!await _obraService.ObraExistsAsync(obraId)) return NotFound("Obra not found.");

        var documentos = await _documentoService.GetDocumentosByObraIdAsync(obraId);
        return Ok(documentos);
    }

    // POST: api/obras/5/documentos/upload
    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocumento(int obraId, [FromForm] UploadDocumentoDto model)
    {
        var filePath = await _documentoService.UploadDocumentoAsync(obraId, model);
        if (filePath == null) return NotFound("Obra not found.");
        if (filePath.StartsWith("Nenhum arquivo") || filePath.StartsWith("Tipo de arquivo") || filePath.StartsWith("Tamanho do arquivo"))
        {
            return BadRequest(filePath);
        }

        return Ok(new { message = "Documento enviado com sucesso!", filePath = filePath });
    }

    // DELETE: api/obras/5/documentos/1 (Soft Delete)
    [HttpDelete("{documentoId}")]
    [Authorize(Roles = "Administrador,Coordenador")] // RN006: Exclusão apenas via soft delete, com permissão
    public async Task<IActionResult> DeleteDocumento(int obraId, int documentoId)
    {
        var result = await _documentoService.DeleteDocumentoAsync(obraId, documentoId);
        if (!result) return NotFound();

        return NoContent();
    }

    // GET: api/obras/5/documentos/1/download (Simulado)
    [HttpGet("{documentoId}/download")]
    public async Task<IActionResult> DownloadDocumento(int obraId, int documentoId)
    {
        var documento = await _documentoService.GetDocumentoForDownloadAsync(obraId, documentoId);
        if (documento == null) return NotFound("Documento não encontrado ou foi excluído.");

        return Ok(new { message = "Simulação de download", filePath = documento.Path, fileName = documento.Nome });
    }
}