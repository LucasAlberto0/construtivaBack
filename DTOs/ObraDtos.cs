using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using construtivaBack.Models;

namespace construtivaBack.DTOs
{
    // DTO para criação de uma nova Obra
    public class ObraCriacaoDto
    {
        [Required(ErrorMessage = "O nome da obra é obrigatório.")]
        public string Nome { get; set; }
        public string? Localizacao { get; set; }
        public string? Contratante { get; set; }
        public string? Contrato { get; set; }
        public string? OrdemInicioServico { get; set; }
        public string? CoordenadorNome { get; set; }
        public string? AdministradorNome { get; set; }
        public string? ResponsavelTecnicoNome { get; set; }
        public string? Equipe { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public ObraStatus Status { get; set; } = ObraStatus.EmAndamento;
        public string? Observacoes { get; set; }
    }

    // DTO para atualização de uma Obra existente
    public class ObraAtualizacaoDto
    {
        [Required(ErrorMessage = "O nome da obra é obrigatório.")]
        public string Nome { get; set; }
        public string? Localizacao { get; set; }
        public string? Contratante { get; set; }
        public string? Contrato { get; set; }
        public string? OrdemInicioServico { get; set; }
        public string? CoordenadorNome { get; set; }
        public string? AdministradorNome { get; set; }
        public string? ResponsavelTecnicoNome { get; set; }
        public string? Equipe { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public ObraStatus Status { get; set; }
        public string? Observacoes { get; set; }
    }

    // DTO para exibição de detalhes de uma Obra
    public class ObraDetalhesDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Localizacao { get; set; }
        public string? Contratante { get; set; }
        public string? Contrato { get; set; }
        public string? OrdemInicioServico { get; set; }
        public string? CoordenadorNome { get; set; }
        public string? AdministradorNome { get; set; }
        public string? ResponsavelTecnicoNome { get; set; }
        public string? Equipe { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public ObraStatus Status { get; set; }
        public string? Observacoes { get; set; }
        public ICollection<AditivoDto>? Aditivos { get; set; }
        public ICollection<ManutencaoDto>? Manutencoes { get; set; }
        public ICollection<DiarioObraDto>? DiariosObra { get; set; }
        public ICollection<DocumentoDto>? Documentos { get; set; }
        public ICollection<ChecklistDto>? Checklists { get; set; }
    }

    // DTO para listagem de Obras (visão mais resumida)
    public class ObraListagemDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Localizacao { get; set; }
        public ObraStatus Status { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
    }

    // DTOs para entidades relacionadas (simplificados para ObraDetalhesDto)
    public class AditivoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
    }

    public class ManutencaoDto
    {
        public int Id { get; set; }
        public DateTime DataManutencao { get; set; }
        public string Descricao { get; set; }
        public bool HasFoto { get; set; }
    }

    public class DiarioObraDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Clima { get; set; }
        public string? Colaboradores { get; set; }
        public string? Atividades { get; set; }
    }

    public class DocumentoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } // Renamed from NomeArquivo
        public string Tipo { get; set; } // New
        public string CaminhoArquivo { get; set; } // Renamed from Url
        public string? Descricao { get; set; } // New
        public long TamanhoArquivo { get; set; } // New
        public DateTime DataAnexamento { get; set; } // New
        public DateTime DataUpload { get; set; } // Existing
    }

    public class ChecklistDto
    {
        public int Id { get; set; }
        public TipoChecklist Tipo { get; set; }
        public ICollection<ChecklistItemDto>? Itens { get; set; }
    }
}
