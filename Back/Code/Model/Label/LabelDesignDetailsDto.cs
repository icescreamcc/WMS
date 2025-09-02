using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Label
{
    public class LabelDesignDetailsDto
    {
        public string LabelId { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemType { get; set; }

        public bool IsUsed { get; set; }

        public string DeftValue { get; set; }

        public bool ShowName { get; set; }

        public string ItemValueField { get; set; }

        public string ItemValueFieldInfo { get; set; }

        public string ValuePerfix { get; set; }

        public string ValueSuffix { get; set; }

        public string FieldSeparator { get; set; }

        public string ItemValueFormat { get; set; }

        public string ItemValueType { get; set; }

        public string Style { get; set; }

        public string Args { get; set; }

        public string Remark { get; set; }
    }
}
