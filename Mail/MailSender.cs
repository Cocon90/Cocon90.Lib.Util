using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Cocon90.Lib.Util.Mail
{
    public class MailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者的源邮箱 如：source@163.com</param>
        /// <param name="senderEmailPass">发送者的源邮箱对应的密码 如：123456</param>
        /// <param name="targetEmail">目标邮箱，发送给谁如abcdefg@163.com</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否是Html内容</param>
        /// <param name="enableSsl">是否使用SSL安全传输</param>
        /// <returns>是否发送成功</returns>
        public Exception sendMail(string senderEmail, string senderEmailPass, string[] targetEmail, string subject, string body, bool isBodyHtml, bool enableSsl = false)
        {
            var mailHost = "SMTP." + senderEmail.Substring(senderEmail.LastIndexOf('@') + 1);
            return sendMail(senderEmail, senderEmailPass, mailHost, 25, targetEmail, subject, body, isBodyHtml, enableSsl);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="senderEmail">发送者的源邮箱 如：source@163.com</param>
        /// <param name="senderEmailPass">发送者的源邮箱对应的密码 如：123456</param>
        /// <param name="targetEmails">目标邮箱，发送给谁如abcdefg@163.com</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否是Html内容</param>
        /// <param name="enableSsl">是否使用SSL安全传输</param>
        /// <returns>是否发送成功</returns>
        public Exception sendMail(string senderEmail, string senderEmailPass, string senderEmailHost, int targetEmailPort, string[] targetEmails, string subject, string body, bool isBodyHtml, bool enableSsl = false)
        {
            try
            {
                MailMessage objMailMessage = new MailMessage();

                objMailMessage.From = new MailAddress(senderEmail);
                if (targetEmails == null || targetEmails.Length == 0) throw new Exception("TargetEmails 至少要有一个！");
                foreach (var mail in targetEmails)
                {
                    objMailMessage.To.Add(new MailAddress(mail ?? ""));
                }
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objMailMessage.Subject = subject;
                objMailMessage.Body = body;
                objMailMessage.IsBodyHtml = isBodyHtml;

                SmtpClient objSmtpClient = new SmtpClient();
                objSmtpClient.Port = targetEmailPort;
                objSmtpClient.Host = senderEmailHost;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new System.Net.NetworkCredential(senderEmail, senderEmailPass);
                objSmtpClient.EnableSsl = enableSsl;//SMTP 服务器要求安全连接需要设置此属性
                objSmtpClient.Send(objMailMessage);
                return null;
            }
            catch (Exception ex) { return ex; }
        }

    }
}
