using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enums;

namespace API.Models
{
    public class GetLikeParams : BasePagination
    {
        public PredicateLikeEnum PredicateUserLike { get; set; }
    }
}