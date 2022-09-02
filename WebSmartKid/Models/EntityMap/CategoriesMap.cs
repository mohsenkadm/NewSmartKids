
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class CategoriesMap : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.ToTable("Categories", "dbo");
            builder.HasKey(x => x.CategoriesId);
            builder.Property(x => x.Image).IsRequired();
            builder.Property(x => x.CategoriesName).IsRequired(); 
        }
    }
}
