using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.EchartsModel
{
    public class TreeMapModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public object Value { get; set; }

        public string Path { get; set; }

        public string Props { get; set; }

        public string Remark { get; set; }

        public List<TreeMapModel> Children { get; set; }
    }
}
