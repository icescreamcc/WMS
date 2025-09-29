using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class MailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string[] ToReceiver { get; set; }

        public string[] ToCC { get; set; }

        public List<FileInfoDto> Attachments { get; set; }

    }
}
