using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public UserLike(int sourceId,int targetId)
        {
            SourceUserId = sourceId;
            TargetUserId = targetId;
        }
        public int SourceUserId { get; set; }
        public int TargetUserId { get; set; }
        public Users SourceUser { get; set; }
        public Users TargetUser { get; set; }
    }
}