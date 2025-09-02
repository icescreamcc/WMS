using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
   public class TreeModel
    {
        public object Id { get; set; }

        public string Label { get; set; }

        public string Type { get; set; }

        public string Remark { get; set; }

        public int Rank { get; set; }

        public object ParentId { get; set; }

        public object ParentName { get; set; }

        public List<TreeModel> Children { get; set; }
    }
}
