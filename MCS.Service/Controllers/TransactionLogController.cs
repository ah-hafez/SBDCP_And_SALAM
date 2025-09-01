using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;
using System.Text;
using MCS.Service.Helpers;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class TransactionLogController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetInboundBasicInfo(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<InboundCertificateDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();

                    TransactionCertificateInfo transactionCertificateInfo = transactionLoggingBL.GetTransactionBasicInfo(transactionId, cultureName);
                    InboundCertificateDTO inboundCertificateDTO = TransactionCertificateMapper.MapInbound(transactionCertificateInfo, cultureName);
                    getResult = GetResult<InboundCertificateDTO>.Create(statusCode, inboundCertificateDTO, null);
                    LogAction(AuditingActionCode.ViewBasicInformation, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<InboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<InboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetOutboundBasicInfo(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OutboundCertificateDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                     
                    TransactionCertificateInfo transactionCertificateInfo = transactionLoggingBL.GetTransactionBasicInfo(transactionId, cultureName);
                    OutboundCertificateDTO outboundCertificateDTO = TransactionCertificateMapper.MapOutbound(transactionCertificateInfo, cultureName);
                    getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, outboundCertificateDTO, null);
                    LogAction(AuditingActionCode.ViewBasicInformation, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAssignmentHistories(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionAssignmentDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<TransactionAssignmentHistory> transactionAssignmentHistories = transactionLoggingBL.GetTransactionAssignmentHistories(transactionId, cultureName).ToList();
                    List<TransactionAssignmentDTO> transactionAssignmentDTOs = TransactionCertificateMapper.Map(transactionAssignmentHistories);
                    getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, transactionAssignmentDTOs, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAssignmentHistoryWithContent(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionAssignmentDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<TransactionAssignmentHistory> transactionAssignmentHistories = transactionLoggingBL.GetTransactionAssignmentHistoryWithContent(transactionId, cultureName).ToList();

                    List<TransactionAssignmentDTO> transactionAssignmentDTOs = TransactionCertificateMapper.Map(transactionAssignmentHistories);

                    getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, transactionAssignmentDTOs, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetTransactionCopiesByTransactionId(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionCopyDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<TransactionCopy> transactionCopies = transactionLoggingBL.GetTransactionCopiesByTransactionId(transactionId, cultureName).ToList();
                    List<TransactionCopyDTO> transactionCopyDTOs = TransactionCopyMapper.Map(transactionCopies);
                    getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, transactionCopyDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionCopies, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionExternalCopiesByTransactionId(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionExternalCopyDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    IList<TransactionExternalCopy> transactionExternalCopies = transactionLoggingBL.GetTransactionExternalCopiesByTransactionId(transactionId, cultureName);
                    List<TransactionExternalCopyDTO> transactionExternalCopyDTOs = TransactionExternalCopyMapper.Map(transactionExternalCopies);
                    getResult = GetResult<List<TransactionExternalCopyDTO>>.Create(statusCode, transactionExternalCopyDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionCopies, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionExternalCopyDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionExternalCopyDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetExplanationsByTransactionId(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExplanationDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    IList<Explanation> explanations = transactionLoggingBL.GetExplanationsByTransactionId(transactionId, cultureName);
                    List<ExplanationDTO> explanationDTOs = ExplanationMapper.Map(explanations);
                    getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, explanationDTOs, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAssignment(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionAssignmentDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    TransactionAssignment transactionAssignment = transactionLoggingBL.GetTransactionAssignment(transactionId, cultureName);
                    TransactionAssignmentDTO transactionAssignmentDTO = TransactionAssignmentMapper.Map(transactionAssignment);
                    getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, transactionAssignmentDTO, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionNames(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionNameDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    IList<TransactionName> transactionNames = transactionLoggingBL.GetTransactionNames(transactionId, cultureName);
                    List<TransactionNameDTO> transactionNameDTOs = transactionNames.Select(tn => TransactionNameMapper.Map(tn.Name)).ToList();
                    getResult = GetResult<List<TransactionNameDTO>>.Create(statusCode, transactionNameDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionNames, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionNameDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionNameDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionLinks(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionCertificateLinkDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<TransactionLink> transactionLinks = transactionLoggingBL.GetTransactionLinks(transactionId, cultureName).ToList();
                    List<TransactionCertificateLinkDTO> transactionCertificateLinkDTOs = TransactionCertificateMapper.Map(transactionLinks, cultureName);
                    getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, transactionCertificateLinkDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionLinks, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionLinksForCertificate(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionCertificateLinkDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<TransactionLink> transactionLinks = transactionLoggingBL.GetTransactionLinksForCertificate(transactionId, cultureName).ToList();
                    List<TransactionCertificateLinkDTO> transactionCertificateLinkDTOs = TransactionCertificateMapper.MapForCertificate(transactionLinks, transactionId, cultureName);
                    getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, transactionCertificateLinkDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionLinks, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionCertificateLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAttachments(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionAttachmentDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<Attachment> attachments = transactionLoggingBL.GetTransactionAttachments(transactionId, cultureName).ToList();
                    List<TransactionAttachmentDTO> transactionAttachmentDTOs = TransactionAttachmentMapper.Map(attachments);
                    getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, transactionAttachmentDTOs, null);
                    LogAction(AuditingActionCode.ViewTransactionAttachmentsArchiving, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }



    }
}
