using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public int SourceUserId { get; set; }
        public int TargetUserId { get; set; }
        public Users SourceUser { get; set; }
        public Users TargetUser { get; set; }
    }
}