using Models.Model.Baseinfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Sys
{
    public class MailDto
    {
        public int MailId { get; set; }
         
        public string PrimaryId { get; set; }

        public string AttachmentId { get; set; }

        public string BusinessType { get; set; }
         
        public string Sender { get; set; }
         
        public string[] ToReceiver { get; set; }
         
        public string[] ToCC { get; set; }
         
        public string Subject { get; set; }
         
        public string Body { get; set; }
         
        public string CreateUserName { get; set; }
         
        public DateTime CreateDate { get; set; }
          
        public List<FileInfoDto> Attachments { get; set; }
    }
}
