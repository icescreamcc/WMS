using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ErrorResponseDto
    {
        public string message { get; set; }

        public string type { get; set; }

        public object param { get; set; }

        public object code { get; set; }
    }
}
