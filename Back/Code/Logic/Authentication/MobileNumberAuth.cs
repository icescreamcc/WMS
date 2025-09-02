using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Authentication
{
   public class MobileNumberAuth
    {
        /// <summary>
        /// 手机号验证码登录验证
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> Validate(string phoneNumber, string code)
        {
            return await Task.Run(() =>
            {
                return true;
            });
        }
    }
}
