
using EpicEvents.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EpicEvents.Controllers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.CookiePolicy;


namespace EpicEvents.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artista> Artisti { get; set; }
        public DbSet<Evento> Eventi { get; set; }
        public DbSet<Biglietto> Biglietti { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Artista>()
                .HasMany(a => a.Eventi)
                .WithOne(e => e.Artista)
                .HasForeignKey(e => e.ArtistaId);

            builder.Entity<Evento>()
                .HasMany(e => e.Biglietti)
                .WithOne(b => b.Evento)
                .HasForeignKey(b => b.EventoId);

            builder.Entity<Biglietto>()
                .HasOne(b => b.User)
                .WithMany(u => u.Biglietti)
                .HasForeignKey(b => b.UserId);
        }
    }
}
