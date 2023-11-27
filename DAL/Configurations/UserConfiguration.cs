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

            builder.HasKey(user => user.Id)
                .HasName("id_user");

            builder.Property(user => user.Username)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(user => user.Places)
                .WithMany(place => place.Users)
                .UsingEntity(j => j.ToTable("user_has_place"));
        }
    }
}
