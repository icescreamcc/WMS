using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class AutoProdDeviceAGVDto
    {
        public int AGVId { get; set; }
         
        public string AGVNo { get; set; }
         
        public string AGVType { get; set; }

        public string AGVTypeDesc { get; set; }

        public string TaskType { get; set; }
         
        public string CtnrType { get; set; }
         
        public string PositionCodeType { get; set; }
         
        public bool IsDeft { get; set; }

        public string Status { get; set; }

        public string StatusDesc { get; set; }

        public string UpdateUserId { get; set; }
         
        public string UpdateUserName { get; set; }
         
        public DateTime UpdateDate { get; set; }
    }
}
