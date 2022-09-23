
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class OrderDetailMap : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail", "dbo");
            builder.HasKey(x => x.OrderDetailId);
            builder.Property(x => x.OrderId).IsRequired();
            builder.Property(x => x.ProductsId).IsRequired(); 
            builder.Property(x => x.Price).IsRequired(); 
            builder.Property(x => x.Count).IsRequired(); 
            builder.Property(x => x.DiscountPercentage).IsRequired(); 
            builder.Ignore(x => x.Name); 
            builder.Ignore(x => x.Detail); 
            builder.Ignore(x => x.Phone); 
            builder.Ignore(x => x.CountryId); 
            builder.Ignore(x => x.AgeName); 
            builder.Ignore(x => x.OrderNo); 
            builder.Ignore(x => x.OrderDate); 
            builder.Ignore(x => x.UserId); 
            builder.Ignore(x => x.Total); 
            builder.Ignore(x => x.NetAmount); 
            builder.Ignore(x => x.TotalDiscount); 
            builder.Ignore(x => x.IsCancel); 
            builder.Ignore(x => x.IsApporve); 
            builder.Ignore(x => x.IsDiscount); 
            builder.Ignore(x => x.IsDone);     
        }
    }
}
