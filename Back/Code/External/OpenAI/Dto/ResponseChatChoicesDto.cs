using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ResponseChatChoicesDto
    {
        public int index { get; set; }

        public MessagesDto message { get; set; }

        public string finish_reason { get; set; }
    }
}
