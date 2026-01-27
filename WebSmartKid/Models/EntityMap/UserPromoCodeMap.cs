using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class UserPromoCodeMap : IEntityTypeConfiguration<UserPromoCode>
    {
        public void Configure(EntityTypeBuilder<UserPromoCode> builder)
        {
            builder.ToTable("UserPromoCode", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.PromoCodeId).IsRequired();
            builder.Property(x => x.UsedDate).IsRequired().HasDefaultValueSql("GETDATE()");
        }
    }
}
