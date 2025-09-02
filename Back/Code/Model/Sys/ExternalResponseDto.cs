using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
    public class ExternalResponseDto
    {
        public string status { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }
}
