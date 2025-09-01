using System;
using System.IO;
using System.Net;
using System.Text;
using MCS.Framework.Notifications;
using MCS.Common;

namespace MCS.Business
{
    public class SMSNotificationService : ISMSNotificationService
    {
        private delegate void SendDelegate(NotificationMessage notificationMessage);

        /// <summary>
        /// Send Messsage using HTTP protocol by creating  WebRequest and WebResponse objects and read the reponse stream.   
        /// </summary>
        /// <param name="notificationMessage"> Object of type Message.</param>
        public void Send(NotificationMessage notificationMessage)
        {
            if (!SystemConfigurations.IsSMSEnabled)
            {
                return;
            }

            if (notificationMessage == null)
            {
                throw new ArgumentException("Notification Message Cannot be Null");
            }

            WebRequest request = null;
            WebResponse response = null;
            string result = string.Empty;

            try
            {
                SMSMessage smsMessage = notificationMessage as SMSMessage;

                /*
                 * Maximum length of the English message is 160 char for one message, if it 
                 * is greater than 160 char then it will divide into two message each one with 134 char length.
                 * */

                /* Maximum length of Arabic message is 70 char for one message, if it is 
                 * greater than 70 char then it will be divided into two messages each one with 67 char length.
                 * */

                NotificationConfigurationSection configSection = NotificationConfiguration.GetSections.Notification;

                if (string.IsNullOrEmpty(configSection.SMSOriginatingAddress) ||
                    string.IsNullOrEmpty(configSection.SMSPassword) ||
                    string.IsNullOrEmpty(configSection.SMSUserName) ||
                    string.IsNullOrEmpty(configSection.URLToSendHTTPSMS.AbsoluteUri)
                    )
                {
                    throw new NotificationConfigurationException("MAKE SURE THAT ALL DATA IN SMS GATEWAY CONFIGURATION FILE ARE NOT NULL OR EMPTY FOR (SMSOriginatingAddress, SMSPassword, SMSUserName AND URLToSendHTTPXml)");
                }

                string from = configSection.SMSOriginatingAddress;

                if (from.Length > 11)
                {
                    throw new NotificationConfigurationException("(MESSAGE.FROM)  MAXIMUM SIZE ALLOWED IS 11 CHARS");
                }

                //// concreate the common helper on the provider, check how to handle the common helper 
                //// as special type of namespace logical nameing.
                if (!Helper.CheckEnglishChars(from))
                {
                    throw new NotificationConfigurationException("(MESSAGE.FROM) VALUE SHOULD CONTAIN ENGLISH CHARACTERS ONLY. SPACE AND SPECIAL CHARACTERS NOT ALLOWED.");
                }

                if (string.IsNullOrEmpty(smsMessage.Body) || string.IsNullOrEmpty(smsMessage.ToNumber))
                {
                    throw new NotificationConfigurationException("(MESSAGE RECIPIENT(S) OR MESSAGE BODY) IS NULL OR EMPTY, IT IS MANDATORY FIELD");
                }

                ////There is a constrains on using http you send the message to one recipient.
                ////The phone number should not start with the internationl extension number 00 or + .
                //if (message.To.Length > 14)
                //{
                //    throw new NotificationServiceUnexpectedException(string.Format(CultureInfo.CurrentCulture, "(MESSAGE.TO) VALUE SHOULD CONTAIN ONE AND ONLY ONE NUMBER IN INTERNATIONAL FORMAT."));
                //}

                ////Create StringBuilder object to contain create query like
                //// like the following format (http://www.resalty.net/api/sendSMS.php?userid=YourUser&password=YourPassword&to=MobileNumber&msg=Message&sender=SenderName) 

                StringBuilder queryString = new StringBuilder();

                queryString.Append(configSection.URLToSendHTTPSMS + "?");
                queryString.Append("mobile=").Append(configSection.SMSUserName).Append("&");
                queryString.Append("password=").Append(configSection.SMSPassword).Append("&");
                queryString.Append("numbers=").Append(smsMessage.ToNumber).Append("&");

                //// URL-Encoded  - encode the message content.
                //// Encoding the body using UniCode format to be passed over the HTTP stream.
                //// Convert the message body to unicode to be send to the SMS provider (HttpUtility.UrlEncode())..                

                string encodedBody = ConvertToUnicode(smsMessage.Body);

                queryString.Append("msg=").Append(encodedBody).Append("&");
                queryString.Append("sender=").Append(configSection.SMSOriginatingAddress).Append("&");
                queryString.Append("applicationType=24");

                //queryString.Append("&applicationType=24");

                // if the message has a valid date time it will be used to schedual the message based on the Date and Time.
                //if (message.Date != null)
                //{
                //    queryString.Append("&dateSend=" + message.Date.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                //    queryString.Append("&timeSend=" + message.Date.Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
                //}

                request = WebRequest.Create(new System.Uri(queryString.ToString()));

                request.Method = "POST";

                response = request.GetResponse();

                using (Stream streamData = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(streamData))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                ValidateResponse(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Send Messsage Async using HTTP protocol by creating  WebRequest and WebResponse objects and read the reponse stream.   
        /// </summary>
        /// <param name="message"> Object of type Message.</param>
        public void SendAsync(NotificationMessage notificationMessage)
        {
            SendDelegate sendDelegate = new SendDelegate(Send);

            sendDelegate.BeginInvoke(notificationMessage, FinishWebRequest, null);
        }

        private void FinishWebRequest(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

                using (Stream streamData = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(streamData))
                    {
                        string responseString = streamReader.ReadToEnd();
                        ValidateResponse(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This function is responsible for validating the response from the SMS Service Provider and through exception in case of 
        /// unexpected result.
        /// </summary>
        /// <param name="response">An object of type string that hold response data.</param>
        /// <param name="message">The message to be sent.</param>
        /// <remarks>
        /// * Error Messages that received form SMS Service Provider        
        /// </remarks>
        public virtual void ValidateResponse(string result)
        {

        }

        /// <summary>
        /// Convert the message to Unicode format.
        /// </summary>
        /// <param name="message">Message to be encoded.</param>
        /// <returns> Encoded message as string.</returns>
        //private static string ConvertToUnicode(string message)
        //{
        //    return System.Web.HttpUtility.UrlEncode(message, System.Text.Encoding.GetEncoding("windows-1256"));
        //}

        private string ConvertToUnicode(string val)
        {
            string msg2 = string.Empty;

            for (int i = 0; i < val.Length; i++)
            {
                msg2 += convertToUnicode(System.Convert.ToChar(val.Substring(i, 1)));
            }

            return msg2;
        }

        private string convertToUnicode(char ch)
        {
            System.Text.UnicodeEncoding class1 = new System.Text.UnicodeEncoding();
            byte[] msg = class1.GetBytes(System.Convert.ToString(ch));

            return FourDigits(msg[1] + msg[0].ToString("X"));
        }

        /// <summary>
        /// Convert the value to four digits number.
        /// </summary>
        /// <param name="value"> Value to be converted.</param>
        /// <returns>Converted Value in four digits format.</returns>
        private static string FourDigits(string value)
        {
            string result = string.Empty;
            switch (value.Length)
            {
                case 1:
                    result = "000" + value;
                    break;
                case 2:
                    result = "00" + value;
                    break;
                case 3:
                    result = "0" + value;
                    break;
                case 4:
                    result = value;
                    break;
            }

            return result;
        }
    }
}
