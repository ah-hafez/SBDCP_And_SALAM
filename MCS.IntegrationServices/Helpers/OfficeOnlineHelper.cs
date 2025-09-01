using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

//using NPOI.XWPF.UserModel;

namespace MCS.IntegrationServices.Helpers
{
    public class OfficeOnlineHelper
    {
     
        public async static Task<byte[]> ConvertDocToPDF(byte[] docx)
        {
            HttpContent bytesContent = new ByteArrayContent(docx);
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(bytesContent, "file2", "file2");
                client.Timeout = TimeSpan.FromMinutes(2);
                var response = await client.PostAsync("http://localhost/MCS.WordToPDF/api/ConverterDocToPdf/", formData).ConfigureAwait(false); 

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception();
                }

                Stream x = await response.Content.ReadAsStreamAsync();

                using (MemoryStream ms = new MemoryStream())
                {
                    x.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        [HttpGet]
        public string Test ()

        {
            return "Test ";
        }

    }
}