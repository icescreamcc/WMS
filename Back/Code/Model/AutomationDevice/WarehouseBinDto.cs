using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class WarehouseBinDto
    {
        public int BinId { get; set; }

        public string BinNo { get; set; }
         
        public int Rank { get; set; }

        public string AGVNo { get; set; }
    }
}
