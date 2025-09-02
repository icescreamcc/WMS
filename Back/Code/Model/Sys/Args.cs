using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
   public class Args
    {
        public int ArgsId { get; set; }

        public string ArgsKey { get; set; }

        public string ArgsKeyName { get; set; }

        public string ArgsKeyNameEn { get; set; }

        public string ArgsValue { get; set; }

        public string ArgsGroup { get; set; }

        public string Remark { get; set; }

        public string ArgsType { get; set; }

        public int Rank { get; set; }

        public bool IsVisible { get; set; }

        public List<ArgsOptions> ArgsOptions { get; set; }
    }
}
