using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloppyDashbourd.Model.EntityMap
{
    public class LikeMap : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Like", "dbo");
            builder.HasKey(x => x.LikeId);
            builder.Property(x => x.ProductsId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();  
        }
    }
}
