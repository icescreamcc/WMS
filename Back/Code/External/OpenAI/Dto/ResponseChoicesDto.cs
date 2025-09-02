using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ResponseChoicesDto
    {
        public string text { get; set; }

        public int index { get; set; }

        public object logprobs { get; set; }

        public string finish_reason { get; set; }
    }
}
