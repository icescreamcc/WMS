using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LogicBase
{
   public class BusinessException: Exception
    {
        public override string Message { get; }

        public BusinessException(string message)
        {
            Message = message;
        }
    }
}
