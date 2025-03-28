using EpicEvents.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EpicEvents.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            string[] roles = new[] { "Utente", "Amministratore" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@epicevents.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Amministratore");
                }
            }

           
            if (!context.Artisti.Any())
            {
                var artista = new Artista
                {
                    Nome = "DJ Snake",
                    Genere = "EDM",
                    Biografia = "Famoso DJ francese, noto per i suoi live set in festival internazionali."
                };

                context.Artisti.Add(artista);
                await context.SaveChangesAsync();

         
                var evento = new Evento
                {
                    Titolo = "Summer Festival 2025",
                    Data = DateTime.Today.AddMonths(2),
                    Luogo = "Roma, Ippodromo Capannelle",
                    ArtistaId = artista.ArtistaId
                };

                context.Eventi.Add(evento);
                await context.SaveChangesAsync();
            }
        }
    }
}
