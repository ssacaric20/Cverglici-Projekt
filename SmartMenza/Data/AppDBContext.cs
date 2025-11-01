
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Models;

namespace SmartMenza.API.Data
{
    public class AppDBContext : DbContext
    {
        // konstruktor
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        // za svaku tablicu u bp
        public DbSet<Uloga> Uloge { get; set; } = null!;
        public DbSet<Korisnik> Korisnici { get; set; } = null!;
        public DbSet<Jelo> Jela { get; set; } = null!;
        public DbSet<Sastojak> Sastojci { get; set; } = null!;
        public DbSet<Cilj> Ciljevi { get; set; } = null!;
        public DbSet<DnevniUnos> DnevniUnosi { get; set; } = null!;
        public DbSet<DnevniMeni> DnevniMeniji { get; set; } = null!;

        // vise-vise veze
        public DbSet<JeloSastojak> JeloSastojci { get; set; } = null!;
        public DbSet<Favorit> Favorit { get; set; } = null!;
        public DbSet<OcjenaJela> OcjenaJela { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. komponentni kljuc za JeloSastojak 
            modelBuilder.Entity<JeloSastojak>()
                .HasKey(js => new { js.JeloId, js.SastojakId });

            // 2. komponentni kljuc za Favorit
            modelBuilder.Entity<Favorit>()
                .HasKey(fj => new { fj.KorisnikId, fj.JeloId });
        }
    }
}
