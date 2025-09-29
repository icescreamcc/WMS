using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class ResponseDataCenter<T> where T : class
    {
        public int code { get; set; }

        public string message { get; set; }

        public ResponseData<T>[] data { get; set; }
    }

    public class ResponseData<T> where T : class
    {
        public T result { get; set; }
    }
}
