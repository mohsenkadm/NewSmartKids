
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class ProductsMap : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.ToTable("Products", "dbo");
            builder.HasKey(x => x.ProductsId);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Count);
            builder.Property(x => x.Name).IsRequired(); 
            builder.Property(x => x.NoOfBuyers).IsRequired(); 
            builder.Property(x => x.Detail).IsRequired(); 
            builder.Property(x => x.LikeCount).IsRequired();    
            builder.Property(x => x.CategoriesId).IsRequired();  
            builder.Property(x => x.DiscountPercentage).IsRequired();   
            builder.Property(x => x.IsDiscount).IsRequired();   
            builder.Ignore(x => x.SourceLike);        
            builder.Ignore(x => x.CategoriesName);        
            builder.Ignore(x => x.Image);        
            builder.Ignore(x => x.UserId);        
            builder.Ignore(x => x.AgeFilter);        
        }
    }
}
