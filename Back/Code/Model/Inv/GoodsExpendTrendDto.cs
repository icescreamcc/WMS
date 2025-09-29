using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Inv
{
    public class GoodsExpendTrendDto
    {
        public string GoodsClassify { get; set; }

        public int DateMonth { get; set; }

        public float Count { get; set; }

        public float MaxCount { get; set; }

        public string UnitName { get; set; }
    }
}
