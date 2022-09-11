
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class TblAgesMap : IEntityTypeConfiguration<TblAges>
    {
        public void Configure(EntityTypeBuilder<TblAges> builder)
        {
            builder.ToTable("TblAges", "dbo");
            builder.HasKey(x => x.AgeId);
            builder.Property(x => x.AgeName).IsRequired();                       
            builder.Ignore(x => x.State) ;                       
        }
    }
}
