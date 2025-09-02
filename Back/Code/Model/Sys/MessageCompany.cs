using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class MessageCompany
    {
        public int MessageId { get; set; }
         
        public string Content { get; set; }
         
        public bool IsPublishCurrent { get; set; }
         
        public string Remark { get; set; }
         
        public string DateTime { get; set; }
    }
}
