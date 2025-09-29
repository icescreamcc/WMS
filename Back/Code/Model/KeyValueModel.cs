using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
   public class KeyValueModel
    {
        public KeyValueModel()
        {

        }

        public KeyValueModel(object key,object value)
        {
            Key = key;
            Value = value;
        }

        public object Key { get; set; }

        public object Value { get; set; }

        public object Remark { get; set; }

        public string Type { get; set; }

        public List<object> Options { get; set; }
    }
}
