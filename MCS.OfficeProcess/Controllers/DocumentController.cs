
using MCS.Framework.Exceptions;
using MCS.Framework.Logging;
using MCS.OfficeProcess.Helpers;
using MCS.OfficeProcess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MCS.OfficeProcess.Controllers
{
    public class DocumentController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult ConvertDocToDocx([FromUri] string fileName)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            try
            {
             
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    StatusCode = HttpStatusCode.OK,
                };

                DocumentViewerHelper.ConvertDocToDocx(fileName);
                response.ResponseCode = ResponseCodeConst.Success;

                return Json(response);



            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                return Json(response);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult CreatePDF([FromUri] string fileName, string exportDir)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            try
            {
           
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    StatusCode = HttpStatusCode.OK,
                };

                DocumentViewerHelper.CreatePDF_New(fileName, exportDir);

                response.ResponseCode = ResponseCodeConst.Success;

                return Json(response);

            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                return Json(response);
            }


        }
    }
}
