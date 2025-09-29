using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class WarehouseAreaDto
    {
        public string AreaNo { get; set; }

        public List<WarehouseShelfDto> Shelfs { get; set; }
    }
}
