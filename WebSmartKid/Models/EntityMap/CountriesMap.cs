
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class CountriesMap : IEntityTypeConfiguration<Countries>
    {
        public void Configure(EntityTypeBuilder<Countries> builder)
        {
            builder.ToTable("Countries", "dbo");
            builder.HasKey(x => x.CountryId);
            builder.Property(x => x.CountryName).IsRequired();                    
        }
    }
}
