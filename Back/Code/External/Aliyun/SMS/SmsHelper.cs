using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using External.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Aliyun.SMS
{
   public class SmsHelper
    {
        private readonly ConfigEnvironment _configEnvironment;

        private readonly LogHelper _logHelper;

        public SmsHelper(ConfigEnvironment configEnvironment, LogHelper logHelper)
        {
            _configEnvironment = configEnvironment;
            _logHelper = logHelper;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumbers">必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式【13567939495】</param>
        /// <param name="templateCode">必填:短信模板-可在短信控制台中找到【SMS_71135039】</param>
        /// <param name="templateParam">可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为【{\"customer\":\"123\"}】</param>
        /// <returns>请求成功或失败的状态码</returns>
        public int Send(string phoneNumbers, string templateCode, string templateParam) => Send(phoneNumbers, templateCode, templateParam, "", "");

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumbers">必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式【13567939495】</param>
        /// <param name="templateCode">必填:短信模板-可在短信控制台中找到【SMS_71135039】</param>
        /// <param name="templateParam">可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为【{\"customer\":\"123\"}】</param>
        /// <param name="outId">可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者【21212121211】</param>
        /// <returns>请求成功或失败的状态码</returns>
        public int Send(string phoneNumbers, string templateCode, string templateParam, string outId) => Send(phoneNumbers, templateCode, templateParam, outId, "");

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumbers">必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式【13567939495】</param>
        /// <param name="templateCode">必填:短信模板-可在短信控制台中找到【SMS_71135039】</param>
        /// <param name="templateParam">可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为【{\"customer\":\"123\"}】</param>
        /// <param name="outId">可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者【21212121211】</param>
        /// <param name="smsSignName">短信签名，如果是空字符串，那么取配置默认签名</param>
        /// <returns>请求成功或失败的状态码</returns>
        public int Send(string phoneNumbers, string templateCode, string templateParam, string outId, string smsSignName)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", _configEnvironment.Config.KeyID, _configEnvironment.Config.KeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            request.Protocol = ProtocolType.HTTPS;
            request.AddQueryParameters("PhoneNumbers", phoneNumbers);
            if (smsSignName == "")
            {
                smsSignName = _configEnvironment.Config.SignName;
            }
            request.AddQueryParameters("SignName", smsSignName);
            request.AddQueryParameters("TemplateCode", templateCode); //   string templateParam = "{\"code\":\"" + authCode + "\"}";
            request.AddQueryParameters("TemplateParam", templateParam);
            if (outId != "")
            {
                request.AddQueryParameters("OutId", outId);
            }
            int res=0;
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                res= response.HttpStatus;
            }
            catch (Exception ex)
            {
                _logHelper.LogError("阿里云短信发送失败", ex.Message, phoneNumbers,ex,ex.StackTrace);
            }
            return res;
        }
    }
}
