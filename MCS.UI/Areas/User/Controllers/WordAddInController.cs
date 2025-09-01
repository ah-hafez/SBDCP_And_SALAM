using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Helpers;
using System;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;

namespace MCS.UI.Areas.User.Controllers
{

    public class WordAddInController : BaseController
    {
        public string TempStorgepath = string.Empty;
        public static string StartKey = "Transaction";
        public static string EndKey = "GAMI";
        public static char Sperator = '_';

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UpdateDocument(WordAddinDocumentDTO dataDoc)
        {

            try
            {

                PostResult postResult = null;
                if (dataDoc != null && dataDoc.content != null && dataDoc.content.Length > 0)
                {

                    //dataDoc.content = DocumentViewerHelper.ConvertDocToDocx(dataDoc.FileName, dataDoc.content);
                    //var pdf = ConvertWordToPDF(Convert.ToBase64String(dataDoc.content));
                    dataDoc.contentAsPDF = null;
                }
                PostResult postResultCall = HttpClientWrapper<PostResult>.PostRequest("api/WordAddIn/UpdateDocument", dataDoc).Result;
                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetTemplate(string id)
        {

            string fileName = id.Replace(".docx", "").Replace(".doc", "");
            GetResult<WordAddinDocumentDTO> filedataDto = HttpClientWrapper<GetResult<WordAddinDocumentDTO>>.GetItemRequest(String.Format("api/WordAddIn/GetTempDocument?fileName={0}&cultureName={1}", fileName, SessionInfo.CultureShortName)).Result;

            Response.Headers.Add("Authorization", SessionInfo.AccessToken);
            return File(filedataDto.Result.content, "System.Net.Mime.MediaTypeNames.Application.Octet", id);
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult MarkDocumentAsRead(string fileName)
        {
            try
            {

                var filedataDto = HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/WordAddIn/MarkDocumentAsRead?fileName={0}", fileName)).Result;

                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MessageText = ex.ToString(), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetTempWord(string transactionId)
        {
            try
            {

                MCS.UI.Areas.User.Models.Shared.UserVM user = (MCS.UI.Areas.User.Models.Shared.UserVM)SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey);
                string bodyContent = "";
                string pdfContent = "";
                byte[] bDocument = null;
                byte[] oldDocument = null;
                string fileName = StartKey + Sperator + user.UserName.ToLower() + Sperator + EndKey + ".doc";
                GetResult<WordAddinDocumentDTO> wordAddinDocumentDTO =
                HttpClientWrapper<GetResult<WordAddinDocumentDTO>>.GetItemRequest(string.Format("api/WordAddIn/GetTempWord?userName=" + fileName)).Result;

                if (wordAddinDocumentDTO.Result != null)
                {
                    WordAddinDocumentDTO dataDoc = new WordAddinDocumentDTO
                    {
                        FileName = fileName,
                        IsApproved = true,
                        userName = user.UserName,

                    };
                    if (wordAddinDocumentDTO.Result.contentAsPDF == null && wordAddinDocumentDTO.Result.content != null)
                    {

                        //dataDoc.content = DocumentViewerHelper.ConvertDocToDocx(dataDoc.FileName, wordAddinDocumentDTO.Result.content);
                        dataDoc.content = wordAddinDocumentDTO.Result.content;
                        var pdf = ConvertWordToPDF(Convert.ToBase64String(dataDoc.content));
                        dataDoc.contentAsPDF = pdf;
                        wordAddinDocumentDTO.Result.contentAsPDF = pdf;
                        PostResult postResultCall = HttpClientWrapper<PostResult>.PostRequest("api/WordAddIn/UpdateDocument", dataDoc).Result;
                    }
                    else
                    {
                        dataDoc.content = wordAddinDocumentDTO.Result.content;
                        dataDoc.contentAsPDF = wordAddinDocumentDTO.Result.contentAsPDF;
                    }
                    Session["DocoNutDocument"] = dataDoc.contentAsPDF;
                    pdfContent = Convert.ToBase64String(dataDoc.contentAsPDF);
                    bodyContent = Convert.ToBase64String(dataDoc.content);
                }
                else
                {
                    Session["DocoNutDocument"] = null;
                    bodyContent = null;
                }


                return Json(new
                {
                    MessageText = "",
                    MessageType = MessageType.Information,
                    WordAddinDocumentDTO = bodyContent,
                    PDFDocumentDTO = pdfContent,
                    FileName = fileName,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}