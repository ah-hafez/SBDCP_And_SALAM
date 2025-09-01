using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.DTO.ExternalParties;
using MCS.DTO.Transaction;
using MCS.Service.Mappers;
using System.Text;
using System.Web;
using YESSER.NCS.MCS.Service.Helpers;
using HashMechanism;
using static MCS.Service.Controllers.TransactionController.CertificationClient;
using RestSharp;
using System.IO;

namespace MCS.Service.Controllers
{


    [CustomAuthenticationAttribute]
    public class WordAddInController : ApiBaseController
    {
        public string TempStorgepath = string.Empty;
        public static string StartKey = "Transaction";
        public static string EndKey = "GAMI";

        public static char Sperator = '_';


        public WordAddInController()
        {
            TempStorgepath = SystemConfigurations.WordAddInStoragePath;

        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage UpdateDocument(WordAddinDocumentDTO dataDoc)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {

                string FileName = StartKey + Sperator + dataDoc.userName.ToLower() + Sperator + EndKey;
                using (var transactionContextScope = context.Create())
                {

                    IWordAddInBL wordAddInBL = IoC.Resolve<IWordAddInBL>();
                    wordAddInBL.UpdateTempDocument(dataDoc, dataDoc.FileName);
                }

                return Request.CreateResponse(HttpStatusCode.Created, postResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTempWord(string userName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<WordAddinDocumentDTO> getResult = null;

            WordAddinDocumentDTO wordAddinDocumentDTO = null;

            try
            {

                wordAddinDocumentDTO = new WordAddinDocumentDTO();

                using (var transactionContextScope = context.Create())
                {

                    IWordAddInBL wordAddInBL = IoC.Resolve<IWordAddInBL>();

                    var wordTemp = wordAddInBL.GetTempDocument(userName);

                    wordAddinDocumentDTO.userName = userName;

                    wordAddinDocumentDTO.content = wordTemp.content;
                    wordAddinDocumentDTO.contentAsPDF = wordTemp.contentAsPDF;

                }


                getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, wordAddinDocumentDTO, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetFormById(int formId, int transactionId, string userName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<byte[]> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFormBL formBL = IoC.Resolve<IFormBL>();

                    Form form = formBL.GetFormById(formId);

                    byte[] bte = form.FormContent.Document.Content;

                    getResult = GetResult<byte[]>.Create(statusCode, bte, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<byte[]>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<byte[]>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetTempDocument(string fileName)
        {

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<WordAddinDocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IWordAddInBL wordAddInBL = IoC.Resolve<IWordAddInBL>();

                    var result = wordAddInBL.GetTempDocument(fileName);

                    getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, result, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<WordAddinDocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage MarkDocumentAsRead(string fileName)
        {

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IWordAddInBL wordAddInBL = IoC.Resolve<IWordAddInBL>();

                    wordAddInBL.MarkDocumentAsRead(fileName);

                    getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }


        }
        [HttpPost]
        public HttpResponseMessage SaveTempDocument(WordAddinDocumentDTO file)
        {


            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IWordAddInBL wordAddInBL = IoC.Resolve<IWordAddInBL>();

                    wordAddInBL.SaveTempDocument(file.content, file.FileName);

                    getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }


        }

    }
}