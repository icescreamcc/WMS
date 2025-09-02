using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class CompletionResponseDto
    {
        public string id { get; set; }

        public object @object { get; set; }

        public long created { get; set; }

        public string model { get; set; }

        public List<ResponseChoicesDto> choices { get; set; }

        public ResponseUsageDto usage { get; set; }
    }
}
