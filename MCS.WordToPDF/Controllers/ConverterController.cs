using MCS.WordToPDF.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;

namespace MCS.WordToPDF.Controllers
{
    public class ConverterController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.Contents.Count != 1)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                    "You must include exactly one file per request."));
            }

            var file = provider.Contents[0];

            byte[] buffer = await file.ReadAsByteArrayAsync();

            var fileName = System.AppContext.BaseDirectory + @"Tool\Temp\" + Guid.NewGuid().ToString() + ".docx";
            string newFileName = fileName.Replace(".docx", ".pdf");

            File.WriteAllBytes(fileName, buffer);

            try
            {
                ProcessAsUser.Launch(System.AppContext.BaseDirectory + @"Tool\wordToPDF.exe " + fileName);

                if (File.Exists(newFileName))
                {
                    byte[] pdfData = File.ReadAllBytes(newFileName);
                    File.Delete(newFileName);
                    File.Delete(fileName);

                    var result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(pdfData)
                    };
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = newFileName
                    };
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var response = ResponseMessage(result);

                    return response;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(fileName.Replace(".docx", ".txt"), ex.Message);
            }

            return BadRequest();
        }
    }
}
