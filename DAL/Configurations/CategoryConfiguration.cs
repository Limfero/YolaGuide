using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("category");

            builder.HasKey(category => category.Id)
                .HasName("id_category");

            builder.Property(category => category.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(category => category.IdSubcategory)
                .HasColumnName("id_subcategory");

            builder.HasOne(category => category.Subcategory)
                .WithMany(category => category.Subcategories)
                .HasConstraintName("subcategory")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
