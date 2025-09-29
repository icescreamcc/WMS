using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ImageResponseDto
    {
        public long created { get; set; }

        public List<ResponseImageDataDto> data { get; set; }
    }
}
