using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entity.Entity;

namespace WebSmartKid.Models.EntityMap
{
    public class TypeDiscountMap : IEntityTypeConfiguration<TypeDiscount>
    {
        public void Configure(EntityTypeBuilder<TypeDiscount> builder)
        {
            builder.ToTable("TypeDiscount", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TypeDis).IsRequired();
            builder.Property(x => x.NameDis).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
