using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Sender).WithMany(x => x.MessagesSent)
                .HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.Receiver).WithMany(x => x.MessagesReceived)
                .HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}