using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.OpenAI.Dto
{
   public class DataPermission
    {
        public string id { get; set; }

        public object @object { get; set; }

        public long created { get; set; }

        public bool allow_create_engine { get; set; }

        public bool allow_sampling { get; set; }

        public bool allow_logprobs { get; set; }

        public bool allow_search_indices { get; set; }

        public bool allow_view { get; set; }

        public bool allow_fine_tuning { get; set; }

        public object organization { get; set; }

        public object group { get; set; }

        public bool is_blocking { get; set; }
    }
}
