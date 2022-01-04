using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Enums;

namespace API.Models
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        //1 , 2 3 4 5 6 
        public int PageNumber { get; set; } = 1;
        public string currentUserName { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Male;
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        // 5 , 10 , 15 , 50 
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}