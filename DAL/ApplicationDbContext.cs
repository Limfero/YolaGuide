using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Configurations;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Place> Plases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Fact> Facts { get; set; }
        public DbSet<Route> Routes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
         //   Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FactConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PlaceConfiguration());
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
        }
    }
}
