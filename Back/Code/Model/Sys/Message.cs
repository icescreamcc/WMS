using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
    public class Message
    {
        public int MessageId { get; set; }

        public string MessageType { get; set; }

        public string Sender { get; set; }
         
        public string Receiver { get; set; }
         
        public string Content { get; set; } 

        public string Link { get; set; }
         
        public string Remark { get; set; } 

        public DateTime CreateDate { get; set; }

        public bool IsRead { get; set; }
    }
}
