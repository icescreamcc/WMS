using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ChatResponseDto
    {
        public string id { get; set; }

        public object @object { get; set; }

        public long created { get; set; } 

        public List<ResponseChatChoicesDto> choices { get; set; }

        public ResponseUsageDto usage { get; set; }
    }
}
