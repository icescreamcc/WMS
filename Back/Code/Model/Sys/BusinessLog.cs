using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class BusinessLog
    { 
        public int LogId { get; set; }
         
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }
         
        public string Message { get; set; }
         
        public string Remark { get; set; }
         
        public string LogType { get; set; }
         
        public string DateTime { get; set; }
    }
}
