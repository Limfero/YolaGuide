using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(user => user.Id)
                .HasColumnName("id_user")
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(user => user.Username)
                .HasColumnName("username")
                .IsRequired();

            builder.Property(user => user.State)
                .HasColumnName("state")
                .IsRequired();

            builder.Property(user => user.Substate)
                .HasColumnName("substate")
                .IsRequired();

            builder.Property(user => user.Language)
                .HasColumnName("language")
                .IsRequired();

            builder.HasMany(user => user.Places)
                .WithMany(place => place.Users)
                .UsingEntity(j => j.ToTable("user_has_place"));

            builder.HasMany(user => user.Categories)
                .WithMany(place => place.Users)
                .UsingEntity(j => j.ToTable("user_has_category"));
        }
    }
}
