using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class Role : IdentityRole<int>
    { 
        //member,admin
        public ICollection<UserRole> UserRole { get; set; }
    }
}