
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class OrdersMap : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.ToTable("Orders", "dbo");
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.OrderNo);
            builder.Property(x => x.OrderDate).IsRequired(); 
            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.Total).IsRequired(); 
            builder.Property(x => x.TotalDiscount).IsRequired(); 
            builder.Property(x => x.NetAmount).IsRequired(); 
            builder.Property(x => x.IsCancel).IsRequired(); 
            builder.Property(x => x.IsApporve).IsRequired(); 
            builder.Property(x => x.IsDone).IsRequired();       
        }
    }
}
