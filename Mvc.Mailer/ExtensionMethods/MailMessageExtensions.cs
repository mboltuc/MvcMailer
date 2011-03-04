﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace Mvc.Mailer
{
    /// <summary>
    /// Adds the much needed send method to MailMessage so that you can do the following
    /// MailMessage email = new MyMailer().WelcomeMessage();
    /// email.Send();
    /// 
    /// The underlying implementation utilizes the SMTPClient class to send the emails.
    /// </summary>
    public static class MailMessageExtensions
    {
        /// <summary>
        /// Sends a MailMessage using smtpClient
        /// </summary>
        /// <param name="message">The mailMessage Object</param>
        /// <param name="smtpClient">leave null to use default System.Net.Mail.SmtpClient</param>
        public static void Send(this MailMessage message, ISmtpClient smtpClient = null)
        {
            smtpClient = smtpClient ?? GetSmtpClient();
            using (smtpClient)
            {
                smtpClient.Send(message);
            }
        }

        /// <summary>
        /// Asynchronously Sends a MailMessage using smtpClient
        /// </summary>
        /// <param name="message">The mailMessage Object</param>
        /// <param name="smtpClient">leave null to use default System.Net.Mail.SmtpClient</param>
        public static void SendAsync(this MailMessage message, ISmtpClient smtpClient = null)
        {
            smtpClient = smtpClient ?? GetSmtpClient();
            var userState = "userState";
            
            using (smtpClient)
            {
                smtpClient.SendAsync(message, userState);
            }
        }

        public static ISmtpClient GetSmtpClient()
        {
            if (MailerBase.IsTestModeEnabled)
            {
                return new TestSmtpClient();
            }
            return new SmtpClientWrapper();
            
        }

    }

}