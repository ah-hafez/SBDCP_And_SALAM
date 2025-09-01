using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;

namespace MCS.Framework.Notifications
{
    public class EmailNotificationService : IEmailNotificationService
    {
        //private delegate void LongTimeTask(EmailMessage mailMessage, IList<Attachment> attachmnts);

        /// <summary>
        /// Gets Instance of type Sections.
        /// </summary>
        private NotificationSections notificationConfigSections;

        /// <summary>
        /// Gets Instance of type Sections.
        /// </summary>
        private NotificationSections NotificationConfigSections
        {
            get
            {
                if (this.notificationConfigSections == null)
                {
                    this.notificationConfigSections = NotificationConfiguration.GetSections;
                }

                return this.notificationConfigSections;
            }
        }

        /// <summary>
        /// Occurs when email asynchronance send completed.
        /// </summary>
        private EventHandler<AsyncCompletedEventArgs> emailSendCompleted;

        /// <summary>
        /// Occurs when email asynchronance send completed.
        /// </summary>
        public event EventHandler<AsyncCompletedEventArgs> EmailSendCompleted
        {
            add
            {
                lock (this)
                {
                    this.emailSendCompleted += value;
                }
            }

            remove
            {
                lock (this)
                {
                    this.emailSendCompleted -= value;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:EmailSendCompleted"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnEmailSendCompleted(AsyncCompletedEventArgs e)
        {
            if (this.emailSendCompleted != null)
            {
                this.emailSendCompleted(this, e);
            }
        }

        /// <summary>
        /// Handles the SendCompleted event of the smtpClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/>Instance containing the event data.</param>
        private void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.OnEmailSendCompleted(e);
        }

        public void Send(NotificationMessage notificationMessage)
        {
            Logging.Logger.WriteInformation("tart Send Email_________/n Request Parameters : " + JsonConvert.SerializeObject(notificationMessage));

            if (notificationMessage == null)
            {
                Logging.Logger.WriteInformation("Notification Message Cannot be Null : ");
                throw new ArgumentException("Notification Message Cannot be Null");
            }

            string defaultProvider = this.NotificationConfigSections.EmailProvidersElement.DefaultProvider;

            if (string.IsNullOrEmpty(defaultProvider))
            {
                Logging.Logger.WriteInformation("AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, DEFAULT PROVIDER CANNOT BE NULL ");
                throw new NotificationConfigurationException("AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, DEFAULT PROVIDER CANNOT BE NULL");
            }

            EmailProviderCollection emailProviders =
                 this.NotificationConfigSections.EmailProvidersElement.EmailProviders;

            EmailProvider emailProvider = emailProviders[defaultProvider];

            if (emailProvider == null)
            {
                Logging.Logger.WriteInformation("AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, DEFAULT PROVIDER CANNOT BE NULL");
                throw new NotificationConfigurationException("AN ERROR OCCURED IN THE NOTIFICATION CONFIGURATION FILE, DEFAULT PROVIDER CANNOT BE NULL");
            }

            string host = emailProvider.Host;
            string userName = emailProvider.UserName;
            string password = emailProvider.Password;
            string portValue = emailProvider.Port;
            bool enableSSl = emailProvider.EnableSSl;
            int portNumber = 25;

            if (!string.IsNullOrEmpty(portValue))
            {
                portNumber = System.Convert.ToInt32(emailProvider.Port);
            }

            EmailMessage emailMessage = notificationMessage as EmailMessage;
            MailMessage mailMessage = new MailMessage();

            if (emailMessage.Attachments != null)
            {
                foreach (Attachment attachment in emailMessage.Attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }
            Logging.Logger.WriteInformation("After Attachments");
            mailMessage.Body = emailMessage.Body;
            mailMessage.Subject = emailMessage.Subject;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(emailProvider.From);

            mailMessage.To.Add(new MailAddress(emailMessage.To));

            if (emailMessage.CC != null)
            {
                mailMessage.CC.Add(new MailAddress(emailMessage.CC));
            }
            Logging.Logger.WriteInformation("After CC");
            //SmtpClient smtpClient = new SmtpClient(host, portNumber);

            //NetworkCredential networkCredential = new NetworkCredential(userName, password);
            SmtpClient smtpClient;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                smtpClient = new SmtpClient(host, portNumber)
                {
                    Credentials = new NetworkCredential(userName, password),
                    EnableSsl = true
                };
            }
            else
            {
                smtpClient = new SmtpClient(host, portNumber);
            }
            Logging.Logger.WriteInformation("After Fill smtpClient");
            //smtpClient.UseDefaultCredentials = false;

            //smtpClient.Credentials = networkCredential;

            smtpClient.EnableSsl = enableSSl;

            //mailMessage.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", true);

            //TODO: To Handle smtp server when is offline ---> 
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            try
            {
                Logging.Logger.WriteInformation("Try Send Email");
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Logging.Logger.WriteInformation("Exception" + ex.Message.ToString());
                //log email exception here
                Logging.Logger.WriteException(ex);
                throw ex;
            }
        }
    }
}
