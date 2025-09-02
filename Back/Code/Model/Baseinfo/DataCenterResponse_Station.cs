using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
    public class DataCenterResponse_Station
    {
        public int area_id { get; set; }

        public int line_id { get; set; }

        public string area { get; set; }

        public string line { get; set; }

        public string station_id { get; set; }

        public string station { get; set; }

        public string station_perform { get; set; }

        public string isfpy { get; set; }

        public string is_output { get; set; }
    }
}
