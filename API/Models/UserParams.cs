using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        //1 , 2 3 4 5 6 
        public int PageNumber { get; set; } = 1;
        // 5 , 10 , 15 , 50 
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}