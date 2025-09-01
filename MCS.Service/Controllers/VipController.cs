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
using MCS.DocRepository.DataDef;
using MCS.Domain;
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
using static MCS.Common.UserClaims;
using MCS.DTO.Transaction.Vip;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class VipController : ApiBaseController
    {

        [HttpGet]
        public HttpResponseMessage GetTransaction(int transactionId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = IoC.Resolve<IEditorBL>();
                        Transaction transaction = editorBL.GetTransaction_VIP(transactionId, orgUnitId, cultureName);

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            if (transaction.MainDocument.Document.Content == null)
                            {
                                DocData docData = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation());
                                transaction.MainDocument.Document.Content = docData.Data;
                            }
                        }
                        if (transaction?.OldWordDocumnt?.Document != null && transaction.OldWordDocumnt.Document.Content == null)
                        {

                            DocData docData = DocRepository.DocRepository.Load(transaction.OldWordDocumnt.Id.ToString(), new DocumentLocation());
                            transaction.OldWordDocumnt.Document.Content = docData.Data;

                        }

                        TransactionDTO transactionDTO = TransactionMapper.Map_VIP(transaction);

                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage SaveInbound(VipInboundUpdateDto inboundUpdateDto)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                List<TransactionFollowUp> transactionFollowUps = new List<TransactionFollowUp>();
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        //add followup
                        byte[] mainDocumentContent = null;
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                        Transaction transaction = TransactionBL.GetTransactionById(inboundUpdateDto.Id);

                        if (inboundUpdateDto.PublicFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPublicMap(inboundUpdateDto.PublicFollowUps));
                        }
                        if (inboundUpdateDto.PrivateFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPrivateMap(inboundUpdateDto.PrivateFollowUps));
                        }

                        //add copy
                        var copies = inboundUpdateDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList();
                        List<TransactionCopy> transactionCopies = new List<TransactionCopy>();
                        if (copies != null && copies.Count > 0)
                        {
                            transactionCopies = TransactionCopyMapper.Map(inboundUpdateDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList());

                        }
                        transactionBL.UpdateVipInbound(transactionFollowUps, transactionCopies, inboundUpdateDto.Id, inboundUpdateDto.ExplanationConfedentialityForAssignmentPaperId, inboundUpdateDto.DocumentDTO.Content, inboundUpdateDto.Summary);


                    }



                    //add entity details
                    IEditorBL editorBL = new EditorBL();

                    //add audit trail followup
                    if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                    {
                        var followUpAuditTrails = FollowUpAuditTrailMapper.Map(transactionFollowUps, inboundUpdateDto.ProccessDescriptions).ToList();
                        foreach (var transactionFollowUp in followUpAuditTrails)
                        {
                            TransactionBL.AddFollowupUditTrial(transactionFollowUp);

                        }
                    }


                    //add assignment
                    List<VIPTransactionAssignmentDto> assignmentDto = inboundUpdateDto.Assignments.Where(x => x.IsAssigned).ToList();
                    IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(assignmentDto.ToList());

                    if (transactionAssignments != null && transactionAssignments.Count > 0)
                    {
                        transactionAssignments.ToList().ForEach(a => a.Description = inboundUpdateDto.Notes);
                        transactionAssignments.ToList().ForEach(a => a.TransactionId = inboundUpdateDto.Id);
                        editorBL.AssignTransaction(inboundUpdateDto.Id, transactionAssignments, Language);
                    }

                    transactionContextScope.Commit();
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetNextTransactionId(TrayType trayType, int orgUnitId, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<VipBasicTransactionInfoDto> getResult = null;
            int rowsCount = 0;
            if (searchCriteria.OrderBy == null || searchCriteria.OrderBy == "")
            {
                searchCriteria.OrderBy = "Id";
            }

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    Transaction transaction = fileBL.GetNextTransactionId(trayType, orgUnitId, searchCriteria);

                    VipBasicTransactionInfoDto transactionDTO = TransactionMapper.MapBasic_Vip(transaction);

                    getResult = GetResult<VipBasicTransactionInfoDto>.Create(statusCode, transactionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<VipBasicTransactionInfoDto>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<VipBasicTransactionInfoDto>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveOutboundInternal(VipOutboundInternalDto outboundInternalUpdateDto)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                List<TransactionFollowUp> transactionFollowUps = new List<TransactionFollowUp>();
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        //add followup

                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.InternalOutbound);

                        Transaction transaction = TransactionBL.GetTransactionById(outboundInternalUpdateDto.Id);

                        if (outboundInternalUpdateDto.PublicFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPublicMap(outboundInternalUpdateDto.PublicFollowUps));
                        }
                        if (outboundInternalUpdateDto.PrivateFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPrivateMap(outboundInternalUpdateDto.PrivateFollowUps));
                        }

                        //add copy
                        var copies = outboundInternalUpdateDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList();
                        List<TransactionCopy> transactionCopies = new List<TransactionCopy>();
                        if (copies != null && copies.Count > 0)
                        {
                            transactionCopies = TransactionCopyMapper.Map(outboundInternalUpdateDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList());

                        }
                        transactionBL.UpdateVipOutboundInternal(transactionFollowUps, transactionCopies, outboundInternalUpdateDto.Id, outboundInternalUpdateDto.ExplanationConfedentialityForAssignmentPaperId, outboundInternalUpdateDto.DocumentDTO.Content, outboundInternalUpdateDto.Summary);


                    }

                    //add entity details
                    IEditorBL editorBL = new EditorBL();

                    //add audit trail followup
                    if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                    {
                        var followUpAuditTrails = FollowUpAuditTrailMapper.Map(transactionFollowUps, outboundInternalUpdateDto.ProccessDescriptions).ToList();
                        foreach (var transactionFollowUp in followUpAuditTrails)
                        {
                            TransactionBL.AddFollowupUditTrial(transactionFollowUp);

                        }
                    }


                    //add assignment
                    List<VIPTransactionAssignmentDto> assignmentDto = outboundInternalUpdateDto.Assignments.Where(x => x.IsAssigned).ToList(); 
                    IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(assignmentDto.ToList());
                    if (transactionAssignments != null && transactionAssignments.Count > 0)
                    {
                        transactionAssignments.ToList().ForEach(a => a.TransactionId = outboundInternalUpdateDto.Id);
                        transactionAssignments.ToList().ForEach(a => a.Description = outboundInternalUpdateDto.Notes);
                        editorBL.AssignTransaction(outboundInternalUpdateDto.Id, transactionAssignments, Language);
                    }


                    transactionContextScope.Commit();
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveOutboundDraft(VipOutboundDraftDto outboundDraftDto)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                List<TransactionFollowUp> transactionFollowUps = new List<TransactionFollowUp>();
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        //add followup

                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.InternalOutbound);

                        Transaction transaction = TransactionBL.GetTransactionById(outboundDraftDto.Id);

                        if (outboundDraftDto.PublicFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPublicMap(outboundDraftDto.PublicFollowUps));
                        }
                        if (outboundDraftDto.PrivateFollowUps != null)
                        {
                            transactionFollowUps.Add(TransactionFollowUpMapper.VipPrivateMap(outboundDraftDto.PrivateFollowUps));
                        }

                        //add copy
                        var copies = outboundDraftDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList();
                        List<TransactionCopy> transactionCopies = new List<TransactionCopy>();
                        if (copies != null && copies.Count > 0)
                        {
                            transactionCopies = TransactionCopyMapper.Map(outboundDraftDto.Assignments.Where(x => x.IsCopy.HasValue && x.IsCopy.Value).ToList());

                        }
                        transactionBL.UpdateVipOutboundDraft(transactionFollowUps, transactionCopies, outboundDraftDto.Id,
                            outboundDraftDto.ExplanationConfedentialityForAssignmentPaperId, outboundDraftDto.MainDocumentData, outboundDraftDto.OldMainDocumentData, outboundDraftDto.IsSigned);


                    }

                    //add entity details
                    IEditorBL editorBL = new EditorBL();

                    //add audit trail followup
                    if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                    {
                        var followUpAuditTrails = FollowUpAuditTrailMapper.Map(transactionFollowUps, outboundDraftDto.ProccessDescriptions).ToList();
                        foreach (var transactionFollowUp in followUpAuditTrails)
                        {
                            TransactionBL.AddFollowupUditTrial(transactionFollowUp);

                        }
                    }


                    //add assignment
                    if (!outboundDraftDto.IsSigned)
                    {
                        List<VIPTransactionAssignmentDto> assignmentDto = outboundDraftDto.Assignments.Where(x => x.IsAssigned).ToList();
                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(assignmentDto.ToList());
                        if (transactionAssignments != null && transactionAssignments.Count > 0)
                        {
                            transactionAssignments.ToList().ForEach(a => a.Description = outboundDraftDto.Notes);
                            transactionAssignments.ToList().ForEach(a => a.TransactionId = outboundDraftDto.Id);
                            editorBL.AssignTransaction(outboundDraftDto.Id, transactionAssignments, Language);
                        }
                    }



                    transactionContextScope.Commit();
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }

}

