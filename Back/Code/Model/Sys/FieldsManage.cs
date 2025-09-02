using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class FieldsManage
    {
        public string FieldsManageId { get; set; }

        public string TableName { get; set; }
         
        public string TableDesc { get; set; }
         
        public string FieldName { get; set; } 

        public string FieldDesc { get; set; }

        public string FieldType { get; set; }
         
        public int FieldLength { get; set; }
         
        public string FieldArgsKey { get; set; }

        public bool IsEnable { get; set; }

        public string Remark { get; set; }

        public int Rank { get; set; }
    }
}
