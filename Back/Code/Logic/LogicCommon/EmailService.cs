using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Logic.LogicCommon
{
    public class EmailService
    {
        private readonly string _host;

        private readonly int _port;

        private readonly string _sender;

        private readonly string _senderUid;

        private readonly string _senderDisplay;

        private readonly string _senderPwd;

        private readonly bool _isAnonymity;
        public EmailService(IConfiguration configuration)
        {
            _host = configuration.GetSection("MailConfig:Host").Value;
            _port = int.Parse(configuration.GetSection("MailConfig:Port").Value);
            _sender = configuration.GetSection("MailConfig:Sender").Value;
            _senderUid = configuration.GetSection("MailConfig:SenderUid").Value;
            _senderDisplay = configuration.GetSection("MailConfig:SenderDisplay").Value;
            _senderPwd = configuration.GetSection("MailConfig:SenderPwd").Value;
            _isAnonymity = bool.Parse(configuration.GetSection("MailConfig:IsAnonymity").Value);
        }

        public async Task SendEmail(string subject, string body, string[] toReciver, string[] toCC,string attachmentPath="")
        {
           await Send(_host, _port, _isAnonymity, _sender, _senderUid,_senderDisplay, _senderPwd, subject, body, toReciver, toCC, attachmentPath);
        }
          
        public async Task Send(string host, int port, bool isAnonymity, string sender, string senderUid, string senderDisplay, string senderPwd, string subject, string body, string[] toReciver, string[] toCC,string attachmentPath=null)
        {
            var smtpClient = new SmtpClient()
            {
                Host = host,
                Port = port,
                // EnableSsl = true
            };
            smtpClient.UseDefaultCredentials = true;
            if (!isAnonymity)
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderUid, senderPwd);
            }
            var mailMsg = new MailMessage
            {
                From = new MailAddress(sender, senderDisplay),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                Priority = MailPriority.High,
            };
            foreach (var rec in toReciver)
            {
                mailMsg.To.Add(new MailAddress(rec));
            }
            if (toCC != null && toCC.Length > 0)
            {
                foreach (var cc in toCC)
                {
                    mailMsg.CC.Add(cc);
                }
            }
            if(!string.IsNullOrEmpty(attachmentPath))
            {
                if (attachmentPath.IndexOf(",")!=-1)
                {
                    foreach (var path in attachmentPath.Split(",")) 
                    {
                        var attachment = new Attachment(path);
                        mailMsg.Attachments.Add(attachment);
                    }
                }
                else
                {
                    var attachment = new Attachment(attachmentPath);
                    mailMsg.Attachments.Add(attachment);
                }
            }
            smtpClient.Send(mailMsg);
            await Task.CompletedTask;
        }
    }
}
