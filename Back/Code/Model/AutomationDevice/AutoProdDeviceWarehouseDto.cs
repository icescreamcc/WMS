using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class AutoProdDeviceWarehouseDto
    {
        public int DeviceId { get; set; }

        public string DeviceNo { get; set; }

        public string DeviceName { get; set; }

        public string DeviceType { get; set; }

        public string DeviceTypeDesc { get; set; }

        public int BinId { get; set; }
         
        public string BinNo { get; set; }

        public string AGVBinCode_Delivery { get; set; }
         
        public string AGVBinCode_Receive { get; set; }

        public string BinStatus { get; set; }

        public string BinStatusDesc { get; set; }

        public int Sort { get; set; }
         
        public int Tier { get; set; }
         
        public int Column { get; set; }
         
        public int Row { get; set; } 
    }
}
