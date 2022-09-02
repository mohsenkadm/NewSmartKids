 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;     
using Entity.Entity;

namespace WebSmartKid.Model.EntityMap
{
    public class PostsMap : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> builder)
        {
            builder.ToTable("Posts", "dbo");
            builder.HasKey(x => x.PostId);
            builder.Property(x => x.Title).IsRequired();       
            builder.Property(x => x.Url).IsRequired();                     
        }
    }
}
