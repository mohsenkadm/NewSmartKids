
using Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebSmartKid.Model.EntityMap
{
    public class MessagesMap : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.ToTable("Messages", "dbo");
            builder.HasKey(x => x.MessageId);
            builder.Property(x => x.MessageText).IsRequired();
            builder.Property(x => x.Date).IsRequired(); 
            builder.Property(x => x.UserSenderId).IsRequired(); 
            builder.Property(x => x.UserReciverId).IsRequired(); 
            builder.Ignore(x => x.IsOwner); 
            builder.Ignore(x => x.Name); 
            builder.Ignore(x => x.DateShow); 
        }
    }
}
