using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class ModelData
    {
        public string id { get; set; }

        public object @object { get; set; }

        public long created { get; set; }

        public object owned_by { get; set; }

        public object root { get; set; }

        public object parent { get; set; }

        public List<DataPermission> permission { get; set; }
    }
}
