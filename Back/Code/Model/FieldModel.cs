using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class FieldModel
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public object Remark { get; set; }

        public Type Type { get; set; }

        public bool IsRequired { get; set; }

        public bool IsIgnoreImport { get; set; }

        public bool IsIgnoreExport { get; set; }

        public object DeftVal { get; set; }
    }
}
