using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class WarehouseDto
    {
        public string WarehouseId { get; set; }

        public string WarehouseName { get; set; }

        public List<WarehouseAreaDto> Areas { get; set; }

        public List<DeviceDto> TransportDevice { get; set; }
    }
}
