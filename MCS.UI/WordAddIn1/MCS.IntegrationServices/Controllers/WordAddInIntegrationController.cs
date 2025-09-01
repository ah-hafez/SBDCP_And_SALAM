using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Services;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Word_Add_in;
using MCS.IntegrationServices.Helpers;

namespace MCS.IntegrationServices.Controllers
{
    public class WordAddInIntegrationController : ApiController
    {


        private void LogData(string data)
        {
            string filePath = @"C:\CustomLog\";
            StringBuilder sb = new StringBuilder();

            sb.Append(data);


            // flush every 20 seconds as you do it
            File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }


        [HttpPost]
        public HttpResponseMessage PostDocumentStringObject(WordAddinDocumentDTO dataDoc)
        {
         


            PostResult postResult = null;
            StatusCode statusCode = Common.StatusCode.Ok;

            postResult = PostResult.Create(statusCode, null);
          

        

            var docToPdf = OfficeOnlineHelper.ConvertDocToPDF(dataDoc.content).Result;

     

            dataDoc.contentAsPDF = docToPdf;


            PostResult postResultCall = HttpClientWrapper<PostResult>.PostRequest("api/WordAddIn/PostDocumentStringObject", dataDoc).Result;


            if (postResultCall.StatusCode != Common.StatusCode.Ok)
            {
                statusCode = Common.StatusCode.NotFound;
            }


            return Request.CreateResponse(HttpStatusCode.OK, postResult);
        }

    }
}
