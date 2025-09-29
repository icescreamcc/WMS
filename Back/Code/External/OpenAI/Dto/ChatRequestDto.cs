using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ChatRequestDto: DialogRequestParamsDto
    {
        public string model { get; set; }

        public List<MessagesDto> messages { get; set; }
    }
}
