using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Configurations
{
    public class FactConfiguration : IEntityTypeConfiguration<Fact>
    {
        public void Configure(EntityTypeBuilder<Fact> builder)
        {
            builder.ToTable("fact");

            builder.HasKey(fact => fact.Id)
                .HasName("id_fact");

            builder.Property(fact => fact.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(fact => fact.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
