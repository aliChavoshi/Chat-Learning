using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class MessageParams : BasePagination
    {
        public string UserName { get; set; }
        public string Container { get; set; } = "Unread"; //unread,outbox,inbox
    }
}