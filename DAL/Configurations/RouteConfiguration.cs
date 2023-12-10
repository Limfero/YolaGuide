using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Configurations
{
    internal class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("route");

            builder.HasKey(route => route.Id)
                .HasName("id_route");

            builder.Property(route => route.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(route => route.Description)
                .HasColumnName("description")
                .IsRequired();

            builder.Property(route => route.Cost)
                .HasColumnName("cost")
                .HasColumnType("money")
                .IsRequired();

            builder.Property(route => route.Telephone)
                .HasColumnName("telephone")
                .HasMaxLength(14)
                .IsRequired();

            builder.Property(route => route.Image)
                .HasColumnName("route_to_image");

            builder.HasMany(route => route.Users)
                .WithMany(user => user.Routes)
                .UsingEntity(j => j.ToTable("user_has_route"));

            builder.HasMany(route => route.Places)
                .WithMany(place => place.Routes)
                .UsingEntity(j => j.ToTable("route_has_place"));
        }
    }
}
