
using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Models;

namespace SmartMenza.Data.Data
{
    public class AppDBContext : DbContext
    {
        // konstruktor
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        // za svaku tablicu u bp
        public DbSet<RoleDto> Uloge { get; set; } = null!;
        public DbSet<UserDto> Korisnici { get; set; } = null!;
        public DbSet<DishDto> Jela { get; set; } = null!;
        public DbSet<IngredientDto> Sastojci { get; set; } = null!;
        public DbSet<NutricionGoalDto> Ciljevi { get; set; } = null!;
        public DbSet<DailyFoodIntakeDto> DnevniUnosi { get; set; } = null!;
        public DbSet<DailyMenuDto> DnevniMeniji { get; set; } = null!;

        // vise-vise veze
        public DbSet<DishIngredientDto> JeloSastojci { get; set; } = null!;
        public DbSet<FavoriteDishDto> Favorit { get; set; } = null!;
        public DbSet<DishRatingDto> OcjenaJela { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. komponentni kljuc za JeloSastojak 
            modelBuilder.Entity<DishIngredientDto>()
                .HasKey(di => new { di.SastojakId, di.JeloId});

            // 2. komponentni kljuc za Favorit
            modelBuilder.Entity<FavoriteDishDto>()
                .HasKey(fd => new { fd.KorisnikId, fd.JeloId});

            modelBuilder.Entity<DailyFoodIntakeDto>()
                .HasKey(dfi => new { dfi.Id, dfi.KorisnikId, dfi.JeloId});

            modelBuilder.Entity<DailyMenuDto>()
                .HasKey(dm => new { dm.Id, dm.JeloId});

            modelBuilder.Entity<DishRatingDto>()
                .HasKey(dr => new { dr.Id, dr.JeloId});

            modelBuilder.Entity<DishDto>()
                .HasKey(d => new { d.Id, d.NutritivneVrijednostiId});

            modelBuilder.Entity<IngredientDto>()
                .HasKey(i => new { i.Id});

            modelBuilder.Entity<NutricionGoalDto>()
                .HasKey(ng => new { ng.Id, ng.KorisnikId});

            modelBuilder.Entity<RoleDto>()
                .HasKey(r => new { r.Id});

            modelBuilder.Entity<UserDto>()
                .HasKey(u => new { u.Id});
        }
    }
}
