using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.OSS
{
   public class ConfigModel
    {
        public string KeyID { get; set; }

        public string KeySecret { get; set; }

        public string Encrypt { get; set; }

        public string EndPoint { get; set; }

        public string BucketName { get; set; }

        public string BucketUrl { get; set; }

        public string RootFolder { get; set; }
    }
}
