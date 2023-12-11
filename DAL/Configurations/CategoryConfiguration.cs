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

            builder.Property(category => category.Id)
                .HasColumnName("id_category")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(category => category.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(category => category.Subcategory)
                .WithMany(category => category.Subcategories)
                .IsRequired(false)
                .HasConstraintName("subcategory")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
