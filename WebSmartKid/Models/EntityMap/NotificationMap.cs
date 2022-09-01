
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class NotificationMap : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification", "dbo");
            builder.HasKey(x => x.NotificationId);
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Details).IsRequired();
            builder.Property(x => x.DateInsert).IsRequired();     
        }
    }
}
