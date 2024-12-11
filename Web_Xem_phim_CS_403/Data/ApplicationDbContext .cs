using Microsoft.EntityFrameworkCore;
using Web_Xem_phim_CS_403.Models;

namespace Web_Xem_phim_CS_403.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập quan hệ Movie - Category
            modelBuilder.Entity<MovieCategory>()
        .HasKey(mc => new { mc.MovieID, mc.CategoryID });

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCategories)
                .HasForeignKey(mc => mc.MovieID);

            modelBuilder.Entity<MovieCategory>()
                .HasOne(mc => mc.Category)
                .WithMany(c => c.MovieCategories)
                .HasForeignKey(mc => mc.CategoryID);

            modelBuilder.Entity<Episode>()
              .HasOne(e => e.Movie)
              .WithMany(m => m.Episodes)
              .HasForeignKey(e => e.MovieID);

            // Thiết lập quan hệ Subtitle - Movie
            modelBuilder.Entity<Subtitle>()
                .HasOne(s => s.Movie)
                .WithMany()
                .HasForeignKey(s => s.MovieID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, RoleName = "Admin" },
               new Role { Id = 2, RoleName = "User" }
           );
        }
       
    
    }
}
