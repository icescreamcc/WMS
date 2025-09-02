using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Token
{
    public class TokenModel
    {
        public string TenantId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string AuthAccount { get; set; }

        public string Signature { get; set; }

        public DateTime Expiration { get; set; }

        public string Device { get; set; } 
    }
}
