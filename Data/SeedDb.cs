
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; 

namespace construtivaBack.Data;

public static class SeedDb
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        string[] roleNames = { "Admin", "Coordenador", "Fiscal" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        string adminEmail = "admin@system.com";
        string adminPassword = "Admin@123";

        ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminEmail,
                NomeCompleto = "Administrador do Sistema",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        await SeedChecklists(context);
    }

    private static async Task SeedChecklists(ApplicationDbContext context)
    {
        if (!await context.Checklists.AnyAsync())
        {
            var inicioObra = new Checklist
            {
                Tipo = TipoChecklist.InicioObra,
                Itens = new List<ChecklistItem>
                {
                    new ChecklistItem { Nome = "ART / RRT", Concluido = false },
                    new ChecklistItem { Nome = "Placa de obra instalada", Concluido = false },
                    new ChecklistItem { Nome = "EPIs disponíveis", Concluido = false },
                    new ChecklistItem { Nome = "CIPA constituída", Concluido = false },
                    new ChecklistItem { Nome = "Alvará de construção", Concluido = false },
                    new ChecklistItem { Nome = "Procuração e documentos", Concluido = false },
                    new ChecklistItem { Nome = "Diário de obra iniciado", Concluido = false },
                    new ChecklistItem { Nome = "Canteiro de obras montado", Concluido = false }
                }
            };

            var entregaObra = new Checklist
            {
                Tipo = TipoChecklist.EntregaObra,
                Itens = new List<ChecklistItem>
                {
                    new ChecklistItem { Nome = "Limpeza final realizada", Concluido = false },
                    new ChecklistItem { Nome = "Termos de garantia assinados", Concluido = false },
                    new ChecklistItem { Nome = "Manuais de equipamentos entregues", Concluido = false },
                    new ChecklistItem { Nome = "As Built finalizado", Concluido = false },
                    new ChecklistItem { Nome = "Habite-se obtido", Concluido = false },
                    new ChecklistItem { Nome = "Jogo de chaves entregue", Concluido = false },
                    new ChecklistItem { Nome = "Vistoria final aprovada", Concluido = false },
                    new ChecklistItem { Nome = "Documentação completa arquivada", Concluido = false }
                }
            };

            context.Checklists.AddRange(inicioObra, entregaObra);
            await context.SaveChangesAsync();
        }
    }
}
