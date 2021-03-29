using Bari.Test.Job.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bari.Test.Job.Infra.Data.Mappings
{
    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");
            builder.Property(x => x.Id);
            builder.Property(x => x.Body).HasColumnType("varchar(255)");
            builder.Property(x => x.ServiceId).IsRequired();
            builder.Property(x => x.Timestamp).IsRequired();
            builder.Property(x => x.CreateDate).IsRequired().HasDefaultValueSql("GetDate()");
            builder.Property(x => x.LastUpdateDate).IsRequired();

        }
        
    }
}