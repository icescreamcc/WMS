using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.AutomationDevice
{
    public class AutoTransTaskInput
    {
        public string OrderNo { get; set; }

        public string TaskType { get; set; }

        public string BusinessType { get; set; }

        public string TaskModel { get; set; }

        public bool IsRequirementForRemaining { get; set; }

        public string GoodsClassifyGroup { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; } 
    }
}
