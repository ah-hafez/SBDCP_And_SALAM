using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.DTO;
using MCS.Service.Mappers;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class DocumentController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetHUBDocumentById(string cultureName, int documentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IHubAttachmentBL hubAttachmentBL = IoC.Resolve<IHubAttachmentBL>();

                    DocumentDTO documentDTO = DocumentMapper.MapWithContent(hubAttachmentBL.GetHubDocumentById(documentId));

                    //if (documentDTO != null)
                    //{
                    //    documentDTO.Content = DocRepository.DocRepository.Load(documentDTO.Id.ToString(), new DocumentLocation()).Data;
                    //}

                    getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, null);

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

        [HttpGet]
        public HttpResponseMessage GetDocumentById(string cultureName, int documentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();

                    DocumentDTO documentDTO = DocumentMapper.MapWithContent(documentBL.GetDocumentById(documentId));

                    if (documentDTO != null && documentDTO.Content == null)
                    {
                        documentDTO.Content = DocRepository.DocRepository.Load(documentDTO.Id.ToString(), new DocumentLocation()).Data;
                    }

                    getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, null);

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

        [HttpDelete]
        public HttpResponseMessage DeleteDocument(int documentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();

                    documentBL.DeleteDocument(documentId);

                    deleteResult = DeleteResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }
    }
}