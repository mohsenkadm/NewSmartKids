
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class ProductAndAgeMap : IEntityTypeConfiguration<ProductAndAge>
    {
        public void Configure(EntityTypeBuilder<ProductAndAge> builder)
        {
            builder.ToTable("ProductAndAge", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ProductsId).IsRequired();                       
            builder.Property(x => x.AgeId).IsRequired();                       
            builder.Property(x => x.State).IsRequired();                       
            builder.Ignore(x => x.AgeName);                       
        }
    }
}
