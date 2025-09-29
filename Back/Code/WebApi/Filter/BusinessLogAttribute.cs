using Models.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filter
{
    public class BusinessLogAttribute: Attribute
    {
        public string Titile { get; set; }

        public LogType LogType { get; set; }

        public string ModuleName { get; set; }

        public BusinessLogAttribute(string titile, LogType logType,string moduleName=null)
        {
            Titile = titile;
            LogType = logType;
            ModuleName = moduleName;
        }
    }
}
