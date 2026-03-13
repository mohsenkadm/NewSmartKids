
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
            builder.Ignore(x => x.ProdName); 
            builder.Ignore(x => x.Image); 
            builder.Ignore(x => x.Name); 
            builder.Ignore(x => x.CountryName); 
            builder.Ignore(x => x.Details); 
            builder.Ignore(x => x.Detail); 
            builder.Ignore(x => x.Phone); 
            builder.Ignore(x => x.CountryId); 
            builder.Ignore(x => x.Address); 
            builder.Ignore(x => x.OrderNo); 
            builder.Ignore(x => x.OrderDate); 
            builder.Ignore(x => x.UserId); 
            builder.Ignore(x => x.Total); 
            builder.Ignore(x => x.NetAmount); 
            builder.Ignore(x => x.TotalDiscount); 
            builder.Ignore(x => x.FinalAmount); 
            builder.Ignore(x => x.UsedAccountBalance); 
            builder.Ignore(x => x.PromoCode); 
            builder.Ignore(x => x.IsCancel); 
            builder.Ignore(x => x.IsApporve); 
            builder.Ignore(x => x.IsDiscount); 
            builder.Ignore(x => x.IsDone);     
        }
    }
}
