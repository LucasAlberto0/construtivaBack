using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class ObraService : IObraService
    {
        private readonly ApplicationDbContext _context;

        public ObraService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ObraListagemDto>> ObterTodasObrasAsync()
        {
            return await _context.Obras
                .Select(o => new ObraListagemDto
                {
                    Id = o.Id,
                    Nome = o.Nome,
                    Localizacao = o.Localizacao,
                    Status = o.Status,
                    DataInicio = o.DataInicio,
                    DataTermino = o.DataTermino
                })
                .ToListAsync();
        }

        public async Task<ObraDetalhesDto?> ObterObraPorIdAsync(int id)
        {
            var obra = await _context.Obras
                .Include(o => o.Aditivos)
                .Include(o => o.Manutencoes)
                .Include(o => o.DiariosObra)
                    .ThenInclude(diario => diario.Comentarios)
                        .ThenInclude(c => c.Autor)
                .Include(o => o.DiariosObra)
                    .ThenInclude(diario => diario.Fotos)
                .Include(o => o.Documentos)
                .Include(o => o.Checklists)
                    .ThenInclude(c => c.Itens)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obra == null)
            {
                return null;
            }

            return MapearParaObraDetalhesDto(obra);
        }

        public async Task<ObraDetalhesDto> CriarObraAsync(ObraCriacaoDto obraDto, string userId)
        {
            var obra = new Obra
            {
                Nome = obraDto.Nome,
                Localizacao = obraDto.Localizacao,
                Contratante = obraDto.Contratante,
                Contrato = obraDto.Contrato,
                OrdemInicioServico = obraDto.OrdemInicioServico,
                CoordenadorNome = obraDto.CoordenadorNome,
                AdministradorNome = obraDto.AdministradorNome,
                ResponsavelTecnicoNome = obraDto.ResponsavelTecnicoNome,
                Equipe = obraDto.Equipe,
                DataInicio = obraDto.DataInicio,
                DataTermino = obraDto.DataTermino,
                Status = obraDto.Status,
                Observacoes = obraDto.Observacoes
            };

            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();

            return MapearParaObraDetalhesDto(obra);
        }

        public async Task<ObraDetalhesDto?> AtualizarObraAsync(int id, ObraAtualizacaoDto obraDto)
        {
            var obra = await _context.Obras.FindAsync(id);

            if (obra == null)
            {
                return null;
            }

            obra.Nome = obraDto.Nome;
            obra.Localizacao = obraDto.Localizacao;
            obra.Contratante = obraDto.Contratante;
            obra.Contrato = obraDto.Contrato;
            obra.OrdemInicioServico = obraDto.OrdemInicioServico;
            obra.CoordenadorNome = obraDto.CoordenadorNome;
            obra.AdministradorNome = obraDto.AdministradorNome;
            obra.ResponsavelTecnicoNome = obraDto.ResponsavelTecnicoNome;
            obra.Equipe = obraDto.Equipe;
            obra.DataInicio = obraDto.DataInicio;
            obra.DataTermino = obraDto.DataTermino;
            obra.Status = obraDto.Status;
            obra.Observacoes = obraDto.Observacoes;

            _context.Obras.Update(obra);
            await _context.SaveChangesAsync();

            return MapearParaObraDetalhesDto(obra);
        }

        public async Task<bool> ExcluirObraAsync(int id)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
            {
                return false;
            }

            _context.Obras.Remove(obra);
            await _context.SaveChangesAsync();
            return true;
        }

        private ObraDetalhesDto MapearParaObraDetalhesDto(Obra obra)
        {
            return new ObraDetalhesDto
            {
                Id = obra.Id,
                Nome = obra.Nome,
                Localizacao = obra.Localizacao,
                Contratante = obra.Contratante,
                Contrato = obra.Contrato,
                OrdemInicioServico = obra.OrdemInicioServico,
                CoordenadorNome = obra.CoordenadorNome,
                AdministradorNome = obra.AdministradorNome,
                ResponsavelTecnicoNome = obra.ResponsavelTecnicoNome,
                Equipe = obra.Equipe,
                DataInicio = obra.DataInicio,
                DataTermino = obra.DataTermino,
                Status = obra.Status,
                Observacoes = obra.Observacoes,
                Aditivos = obra.Aditivos?.Select(a => new AditivoDto
                {
                    Id = a.Id,
                    Descricao = a.Descricao,
                    Data = a.Data
                }).ToList(),
                Manutencoes = obra.Manutencoes?.Select(m => new ManutencaoDto
                {
                    Id = m.Id,
                    DataManutencao = m.DataManutencao,
                    Descricao = m.Descricao,
                    HasFoto = m.Foto != null
                }).ToList(),
                DiariosObra = obra.DiariosObra?.Select(d => new DiarioObraDto
                {
                    Id = d.Id,
                    Data = d.Data,
                    Clima = d.Clima,
                    Colaboradores = d.Colaboradores,
                    Atividades = d.Atividades
                }).ToList(),
                Documentos = obra.Documentos?.Select(doc => new DocumentoDto
                {
                    Id = doc.Id,
                    Nome = doc.Nome,
                    Tipo = doc.Tipo,
                    CaminhoArquivo = doc.CaminhoArquivo,
                    Descricao = doc.Descricao,
                    TamanhoArquivo = doc.TamanhoArquivo,
                    DataAnexamento = doc.DataAnexamento,
                    DataUpload = doc.DataUpload
                }).ToList(),
                Checklists = obra.Checklists?.Select(c => new ChecklistDto
                {
                    Id = c.Id,
                    Tipo = c.Tipo,
                    Itens = c.Itens?.Select(ci => new ChecklistItemDto
                    {
                        Id = ci.Id,
                        Nome = ci.Nome,
                        Concluido = ci.Concluido,
                        Observacao = ci.Observacao
                    }).ToList()
                }).ToList()
            };
        }
    }
}
