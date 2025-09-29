using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class DeviceDto
    {
        public int DeviceId { get; set; }
         
        public string DeviceNo { get; set; }
         
        public string DeviceName { get; set; }
         
        public string DeviceType { get; set; }

        public string DeviceTypeDesc { get; set; }

        public string ConnectAddress { get; set; }
         
        public string WarehouseId { get; set; }
         
        public bool IsActive { get; set; }

        public int ConnectedCount { get; set; }

        public List<DeviceBinDetailDto> BinDetails { get; set; }
    }
}
