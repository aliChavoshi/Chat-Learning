using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Entities.Configuration
{
    public class UserLikeConfiguration : IEntityTypeConfiguration<UserLike>
    {
        public void Configure(EntityTypeBuilder<UserLike> builder)
        {
            builder.HasKey(x => new { x.SourceUserId, x.TargetUserId });
            builder.HasOne(x => x.SourceUser).WithMany(x => x.SourceUserLikes).HasForeignKey(x => x.SourceUserId);
            builder.HasOne(x=>x.TargetUser).WithMany(x=>x.TargetUserLikes).HasForeignKey(x=>x.TargetUserId);
        }
    }
}