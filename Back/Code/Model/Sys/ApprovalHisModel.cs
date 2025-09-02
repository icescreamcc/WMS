using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class ApprovalHisModel
    {
        public int ApprovalId { get; set; }
         
        public string PrimaryId { get; set; }
         
        public string DataType { get; set; }
         
        public string ApprovalDate { get; set; }
         
        public string ApproverId { get; set; }
         
        public string ApproverName { get; set; }
         
        public string ApproverRole { get; set; }

        public string ApproverRoleName { get; set; }

        public string ApprovalModel { get; set; }

        public string ApprovalModelDesc { get; set; }

        public int ApprovalRank { get; set; }

        public string ApprovalStatus { get; set; }

        public string ApprovalStatusDesc { get; set; }

        public string Opinion { get; set; }
    }
}
