 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;     
using Entity.Entity;

namespace WebSmartKid.Model.EntityMap
{
    public class AdminMap : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin", "dbo");
            builder.HasKey(x => x.AdminId);
            builder.Property(x => x.AdminName).IsRequired();       
            builder.Property(x => x.AdminNo).IsRequired();       
            builder.Property(x => x.Password).IsRequired();          
            builder.Ignore(x => x.Token);                       
            builder.Ignore(x => x.CountItem);                       
            builder.Ignore(x => x.CountOrder);                       
            builder.Ignore(x => x.CountUser);                       
            builder.Ignore(x => x.PriceSale);                       
        }
    }
}
