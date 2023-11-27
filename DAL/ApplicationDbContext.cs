using Microsoft.EntityFrameworkCore;
using YolaGuide.DAL.Configurations;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL
{
    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = "Data Source=DESKTOP-B3FAFAI\\SQLEXPRESS01;Initial Catalog=YolaGuide;TrustServerCertificate=True;Integrated Security=SSPI";

        public DbSet<Place> Plases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Fact> Facts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new FactConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new PlaceConfiguration());
        }
    }
}
