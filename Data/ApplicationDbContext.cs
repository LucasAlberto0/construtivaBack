
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Obra> Obras { get; set; }
    public DbSet<DiarioObra> DiariosDeObra { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<Manutencao> Manutencoes { get; set; }
    public DbSet<Aditivo> Aditivos { get; set; }
    public DbSet<Checklist> Checklists { get; set; }
    public DbSet<ChecklistItem> ChecklistItens { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Configurações adicionais do modelo, como chaves compostas ou relacionamentos complexos, podem ser adicionadas aqui.
    }
}
