using construtivaBack.Data;
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services
{
    public class ChecklistService : IChecklistService
    {
        private readonly ApplicationDbContext _context;

        public ChecklistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChecklistListagemDto>> ObterTodosChecklistsAsync(int obraId)
        {
            return await _context.Checklists
                .Where(c => c.ObraId == obraId)
                .Include(c => c.Obra)
                .Select(c => new ChecklistListagemDto
                {
                    Id = c.Id,
                    Tipo = c.Tipo,
                    ObraId = c.ObraId,
                    NomeObra = c.Obra.Nome
                })
                .ToListAsync();
        }

        public async Task<ChecklistDetalhesDto?> ObterChecklistPorIdAsync(int id)
        {
            var checklist = await _context.Checklists
                .Include(c => c.Obra)
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (checklist == null)
            {
                return null;
            }

            return MapearParaChecklistDetalhesDto(checklist);
        }

        public async Task<ChecklistDetalhesDto> CriarChecklistAsync(ChecklistCriacaoDto checklistDto)
        {
            var obraExiste = await _context.Obras.AnyAsync(o => o.Id == checklistDto.ObraId);
            if (!obraExiste)
            {
                throw new ArgumentException("Obra não encontrada.");
            }

            var checklist = new Checklist
            {
                Tipo = checklistDto.Tipo,
                ObraId = checklistDto.ObraId
            };

            if (checklistDto.Itens != null && checklistDto.Itens.Any())
            {
                foreach (var itemDto in checklistDto.Itens)
                {
                    checklist.Itens.Add(new ChecklistItem
                    {
                        Nome = itemDto.Nome,
                        Concluido = itemDto.Concluido,
                        Observacao = itemDto.Observacao
                    });
                }
            }

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            await _context.Entry(checklist).Reference(c => c.Obra).LoadAsync();
            await _context.Entry(checklist).Collection(c => c.Itens).LoadAsync();

            return MapearParaChecklistDetalhesDto(checklist);
        }

        public async Task<ChecklistDetalhesDto?> AtualizarChecklistAsync(int id, ChecklistAtualizacaoDto checklistDto)
        {
            var checklist = await _context.Checklists.Include(c => c.Itens).FirstOrDefaultAsync(c => c.Id == id);

            if (checklist == null)
            {
                return null;
            }

            checklist.Tipo = checklistDto.Tipo;

            // Atualizar itens existentes e adicionar novos
            if (checklistDto.Itens != null)
            {
                foreach (var itemDto in checklistDto.Itens)
                {
                    var existingItem = checklist.Itens.FirstOrDefault(i => i.Id == itemDto.Id);
                    if (existingItem != null)
                    {
                        existingItem.Nome = itemDto.Nome;
                        existingItem.Concluido = itemDto.Concluido;
                        existingItem.Observacao = itemDto.Observacao;
                    }
                    else
                    {
                        // Adicionar novo item se não existir (apenas se o ID for 0 ou não for fornecido)
                        if (itemDto.Id == 0)
                        {
                            checklist.Itens.Add(new ChecklistItem
                            {
                                Nome = itemDto.Nome,
                                Concluido = itemDto.Concluido,
                                Observacao = itemDto.Observacao
                            });
                        }
                    }
                }

                // Remover itens que não estão mais no DTO
                var itensParaRemover = checklist.Itens
                    .Where(item => !checklistDto.Itens.Any(dtoItem => dtoItem.Id == item.Id))
                    .ToList();

                foreach (var item in itensParaRemover)
                {
                    _context.ChecklistItens.Remove(item);
                }
            }

            _context.Checklists.Update(checklist);
            await _context.SaveChangesAsync();

            await _context.Entry(checklist).Reference(c => c.Obra).LoadAsync();
            await _context.Entry(checklist).Collection(c => c.Itens).LoadAsync();

            return MapearParaChecklistDetalhesDto(checklist);
        }

        public async Task<bool> ExcluirChecklistAsync(int id)
        {
            var checklist = await _context.Checklists.FindAsync(id);
            if (checklist == null)
            {
                return false;
            }

            _context.Checklists.Remove(checklist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ChecklistItemDto?> AdicionarItemChecklistAsync(int checklistId, ChecklistItemCriacaoDto itemDto)
        {
            var checklist = await _context.Checklists.Include(c => c.Itens).FirstOrDefaultAsync(c => c.Id == checklistId);
            if (checklist == null)
            {
                return null;
            }

            var item = new ChecklistItem
            {
                Nome = itemDto.Nome,
                Concluido = itemDto.Concluido,
                Observacao = itemDto.Observacao,
                ChecklistId = checklistId
            };

            checklist.Itens.Add(item);
            await _context.SaveChangesAsync();

            return new ChecklistItemDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Concluido = item.Concluido,
                Observacao = item.Observacao
            };
        }

        public async Task<ChecklistItemDto?> AtualizarItemChecklistAsync(int itemId, ChecklistItemAtualizacaoDto itemDto)
        {
            var item = await _context.ChecklistItens.FindAsync(itemId);
            if (item == null)
            {
                return null;
            }

            item.Nome = itemDto.Nome;
            item.Concluido = itemDto.Concluido;
            item.Observacao = itemDto.Observacao;

            _context.ChecklistItens.Update(item);
            await _context.SaveChangesAsync();

            return new ChecklistItemDto
            {
                Id = item.Id,
                Nome = item.Nome,
                Concluido = item.Concluido,
                Observacao = item.Observacao
            };
        }

        public async Task<bool> ExcluirItemChecklistAsync(int itemId)
        {
            var item = await _context.ChecklistItens.FindAsync(itemId);
            if (item == null)
            {
                return false;
            }

            _context.ChecklistItens.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        private ChecklistDetalhesDto MapearParaChecklistDetalhesDto(Checklist checklist)
        {
            return new ChecklistDetalhesDto
            {
                Id = checklist.Id,
                Tipo = checklist.Tipo,
                ObraId = checklist.ObraId,
                NomeObra = checklist.Obra?.Nome,
                Itens = checklist.Itens?.Select(i => new ChecklistItemDto
                {
                    Id = i.Id,
                    Nome = i.Nome,
                    Concluido = i.Concluido,
                    Observacao = i.Observacao
                }).ToList()
            };
        }
    }
}
