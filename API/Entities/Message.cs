using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverUserName { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; } = false;
        public bool ReceiverDeleted { get; set; } = false;
        public bool IsRead { get; set; } = false;

        public Users Sender { get; set; }
        public Users Receiver { get; set; }
    }
}