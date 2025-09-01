using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;

using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using MCS.Domain;
using System.Configuration;

namespace MCS.UI.Areas.User.Controllers
{
    public class ReadWritWordController : Controller
    {

        // GET: User/ReadWritWord


        [HttpGet]

        public ActionResult Index()
        {
            //var port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            //var host = Request.Url.Host;
            //var url = "http://localhost:8080/wopi/files/test.docx/";

            //var contents = url + "contents";
            //using (HttpClient client = new HttpClient())
            //{
            //    var check = await client.GetAsync(url);
            //    var stream = await client.GetStreamAsync(contents);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        int count = 0;
            //        do
            //        {
            //            byte[] buf = new byte[1024];
            //            count = stream.Read(buf, 0, 1024);
            //            ms.Write(buf, 0, count);
            //        } while (stream.CanRead && count > 0);
            //        //b = ms.ToArray();

            //        WordprocessingDocument doc = WordprocessingDocument.Open(ms, false);
            //        var body = doc.MainDocumentPart.Document.Body;

            //        ViewBag.Text = body.InnerText;
            //        //StreamReader reader = new StreamReader(response,Encoding.UTF8);
            //        //var text = reader.ReadToEnd();
            //        doc.Close();
            //    }
            //    // var text = Encoding.UTF8.GetString(response);

            //}

            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Index(string text)
        {

            // var port = int.Parse(ConfigurationManager.AppSettings["Port"]);

            // var host = ConfigurationManager.AppSettings["Host"];

            var url = "http://localhost:8080/wopi/files/test.docx/contents";


            using (HttpClient client = new HttpClient())
            {
                //var stream = await client.GetStreamAsync(url);
                using (var mem = new MemoryStream())
                {
                    //  stream.CopyTo(mem);
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                    {
                        // Add a main document part. 
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                        new DocumentFormat.OpenXml.Wordprocessing.Document(new Body()).Save(mainPart);

                        Body docbody = mainPart.Document.Body;
                        docbody.Append(new Paragraph(new Run(new Text(text))));

                        mainPart.Document.Save();
                        wordDocument.Close();
                        var body = new ByteArrayContent(mem.ToArray());
                        //body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(wordDocument.MainDocumentPart.ContentType);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("X-WOPI-Override", "PUT");
                        var result = await client.PostAsync(url, body);
                    }
                }
            }
            //ViewBag.Text = text;
            return RedirectToAction("index", new { Area = "User" });
        }

    }


}

