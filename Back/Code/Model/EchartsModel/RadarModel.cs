using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.EchartsModel
{
    public class RadarModel
    {
        public List<string> YearMonthArray { get; set; }

        public List<RadarIndicator> IndicatorArray { get; set; }

        public List<RadarData> DataArray { get; set; }

        public float MaxValue { get; set; }
    }
}
