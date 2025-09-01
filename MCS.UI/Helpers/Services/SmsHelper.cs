using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MCS.UI.Helpers
{
    public class SmsHelper
    {
        // GET: Sms
        public static async Task<int> SendOTP(string phoneNumber)
        {
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;

            int otp = GenerateRandomNo();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync("https://10.8.250.103/NC/api/send/v1/?xml=" + "<?xml version=\"1.0\" encoding=\"utf-8\"?><Message ID=\"6060\"><MessageDetails><OTP>" + otp + "</OTP><Mobile> " + phoneNumber + " </Mobile></MessageDetails><ProviderDetail><ID>CPPA-OTP</ID><SecretCode>A27%23RN@Q94</SecretCode></ProviderDetail></Message>", null).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> t) =>
                {
                    string res = t.Result;
                });
            }

            return otp;
        }
        public static void SendSmsTransactionNumber(long transactionNumber , string phoneNumber)
        {

        }
        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}