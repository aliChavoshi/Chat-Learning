using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge(this DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            //birthday = 1998/10/10
            //today = 2021/08/02
            // 23
            if (birthday.Date > today.AddYears(-age).Date)
            {
                age--;
            }
            return age;
        }
    }
}