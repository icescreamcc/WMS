using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.HIK_RCS.Dto
{
    public class RCSArgsOutput<T>
    {
        public string code { get; set; }

        public string message { get; set; }

        public string reqCode { get; set; }

        public T data { get; set; } 
    }
}
