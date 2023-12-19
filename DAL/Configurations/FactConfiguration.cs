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

            builder.Property(fact => fact.Id)
                 .HasColumnName("id_fact")
                 .ValueGeneratedOnAdd()
                 .IsRequired();

            builder.Property(fact => fact.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(fact => fact.Description)
                .HasColumnName("description")
                .HasColumnType("text")
                .IsRequired();
        }
    }
}
