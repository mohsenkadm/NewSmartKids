using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class PromoCodeMap : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.ToTable("PromoCode", "dbo");
            builder.HasKey(x => x.PromoCodeId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValueSql("GETDATE()");
        }
    }
}
