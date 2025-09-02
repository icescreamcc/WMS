using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ModelList
    {
        public object @object { get; set; }

        public List<ModelData> data { get; set; }
    }
}
