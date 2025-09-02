using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model.Baseinfo
{
   public class FileInfoDto
    {
        public int FileId { get; set; }

        public string FileName { get; set; }

        public string FileInfoType { get; set; }
         
        public string PrimaryId { get; set; } 

        public string Url { get; set; }
         
        public string Path { get; set; }
         
        public string Remark { get; set; }
    }
}
