using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class ApprovalProcessModel
    {
        public int ProcessId { get; set; }
         
        public string DataType { get; set; }
         
        public string ApproverRole { get; set; }

        public string ApproverRoleName { get; set; }

        public string ApproverId { get; set; }

        public string ApproverName { get; set; }

        public string ApproverEmail { get; set; }

        public int Rank { get; set; }
         
        public bool IsLastApproval { get; set; }

        public bool IsJump { get; set; }

        public string ApprovalModel { get; set; }
    }
}
