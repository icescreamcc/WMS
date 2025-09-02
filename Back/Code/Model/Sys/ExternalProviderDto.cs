using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
    public class ExternalProviderDto
    {
        public string ProviderName { get; set; }

        public string ProviderSecretKey { get; set; }

        public string ProviderSecret { get; set; }

        public string ProviderHost { get; set; }

        public string Remark { get; set; }

        public bool IsValid { get; set; }
    }
}
