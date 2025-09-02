using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class SocketMessageModel<T>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public T Data { get; set; }
    }
}
