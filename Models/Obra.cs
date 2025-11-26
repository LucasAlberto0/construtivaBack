using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models
{
    public enum ObraStatus
    {
        EmAndamento,
        EmManutencao,
        Suspenso,
        Finalizado
    }

    public class Obra
    {
        [Key]
        public int Id { get; set; }

        // Dados básicos
        [Required]
        public string Nome { get; set; }
        public string? Localizacao { get; set; }
        public string? Contratante { get; set; }
        public string? Contrato { get; set; }
        public string? OrdemInicioServico { get; set; }

        // Responsáveis
        public string? CoordenadorNome { get; set; }
        public string? AdministradorNome { get; set; }
        public string? ResponsavelTecnicoNome { get; set; }

        public string? Equipe { get; set; }

        // Status dos serviços
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }
        public ObraStatus Status { get; set; } = ObraStatus.EmAndamento;
        public string? Observacoes { get; set; }

        // Navigation properties
        public virtual ICollection<Aditivo> Aditivos { get; set; } = new List<Aditivo>();
        public virtual ICollection<Manutencao> Manutencoes { get; set; } = new List<Manutencao>();
        public virtual ICollection<DiarioObra> DiariosObra { get; set; } = new List<DiarioObra>();
        public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
        public virtual ICollection<Checklist> Checklists { get; set; } = new List<Checklist>();
    }
}
