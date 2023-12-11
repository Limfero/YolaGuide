using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Configurations
{
    public class PlaceConfiguration : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            builder.ToTable("place");

            builder.HasKey(place => place.Id)
                .HasName("id_place");

            builder.Property(place => place.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(place => place.Description)
                .HasColumnType("text")
                .HasColumnName("description")
                .IsRequired();

            builder.Property(place => place.Adress)
                .HasColumnName("adress")
                .IsRequired();

            builder.Property(place => place.Image)
                 .HasColumnName("route_to_image");

            builder.Property(place => place.YIdOrganization)
                 .HasColumnName("yid_org")
                 .IsRequired();

            builder.Property(place => place.Coordinates)
                 .HasColumnName("coordinates")
                 .IsRequired();

            builder.HasMany(place => place.Categories)
                .WithMany(category => category.Places)
                .UsingEntity(j => j.ToTable("place_has_category"));
        }
    }
}
