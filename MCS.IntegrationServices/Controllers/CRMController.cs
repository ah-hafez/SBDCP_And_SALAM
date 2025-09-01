using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Exceptions;
using MCS.IntegrationServices.Mappers;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MCS.IntegrationServices.Controllers
{
    public class CRMController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> CreateDraft(string complaintNumber, string complaintDescription, int toExternalPartyId)
        {
            try
            {
                LoginInfoDTO loginInfoDTO = new LoginInfoDTO();

                loginInfoDTO.UserName = ConfigurationManager.AppSettings["CRMUserName"];
                loginInfoDTO.Password = ConfigurationManager.AppSettings["CRMUserPassword"];

                PostObjectResult<UserDTO> loginPostResult = await
                    HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest(SystemConfigurations.WebApiUrl + "api/Login/Login?cultureName=" + SessionInfo.CultureShortName, loginInfoDTO).ConfigureAwait(false);

                int ComplaintUnitId = SystemConfigurations.ComplaintUnitId;

                AddOutboundExternalVM outboundExternalAddVM = new AddOutboundExternalVM();
                outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft = true;
                outboundExternalAddVM.OutboundExternalBasicInfo.LetterTypeId = 55; // Complaint letter type
                outboundExternalAddVM.OutboundExternalBasicInfo.PreparationEntityId = ComplaintUnitId;
                outboundExternalAddVM.OrgUnitId = ComplaintUnitId;
                outboundExternalAddVM.OutboundExternalBasicInfo.ComplaintNumber = complaintNumber;

                outboundExternalAddVM.OutboundExternalBasicInfo.Subject = complaintDescription + " " + complaintNumber;

                outboundExternalAddVM.OutboundExternalBasicInfo.TransactionTypeId = 1;   //نوع الصادر//
                outboundExternalAddVM.OutboundExternalBasicInfo.DestinationId = 1; // or 1   //جهة الصادر//
                outboundExternalAddVM.OutboundExternalBasicInfo.DirectedToId = toExternalPartyId;   //صادر الى//
                outboundExternalAddVM.OutboundExternalBasicInfo.LetterTypeId = 1;    //نوع خطاب الصادر//
                outboundExternalAddVM.OutboundExternalBasicInfo.PriorityLevelId = 1; //درجة الأسبقية//
                outboundExternalAddVM.OutboundExternalBasicInfo.ConfidentialityLevelId = 27;   //درجة السريه//
                outboundExternalAddVM.OutboundExternalBasicInfo.DeliveryMethodId = 236;

                outboundExternalAddVM.DocumentVM = new DocumentVM();

                PostObjectResult<TransactionDetailsDTO> postResult = null;
                AddOutboundExternalDTO addOutbound = OutboundExternalMapper.Map(outboundExternalAddVM);

                if (outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft)
                {
                    AddOutboundDraftDTO addOutboundDraftDTO = new AddOutboundDraftDTO();
                    addOutboundDraftDTO.Attachments = addOutbound.Attachments;
                    addOutboundDraftDTO.Copies = addOutbound.Copies;
                    addOutboundDraftDTO.ExternalCopies = addOutbound.ExternalCopies;
                    addOutboundDraftDTO.Names = addOutbound.Names;
                    addOutboundDraftDTO.Links = addOutbound.Links;
                    addOutboundDraftDTO.OrgUnitId = ComplaintUnitId; //SessionInfo.OrgUnitId;
                    addOutboundDraftDTO.DocumentDTO = addOutbound.DocumentDTO;

                    addOutboundDraftDTO.StatusId = 396; // Not sent => TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    addOutboundDraftDTO.OutboundDraftBasicInfo.SubjectClassifications = addOutbound.OutboundExternalBasicInfo.SubjectClassifications;
                    
                    addOutboundDraftDTO.OutboundDraftBasicInfo = new AddOutboundDraftBasicInfoDTO()
                    {
                        TransactionTypeId = addOutbound.OutboundExternalBasicInfo.TransactionTypeId,
                        ConfidentialityLevelId = addOutbound.OutboundExternalBasicInfo.ConfidentialityLevelId,
                        DestinationId = addOutbound.OutboundExternalBasicInfo.DestinationId,
                        DirectedToId = addOutbound.OutboundExternalBasicInfo.DirectedToId,
                        Hour = addOutbound.OutboundExternalBasicInfo.Hour,
                        Minute = addOutbound.OutboundExternalBasicInfo.Minute,
                        PriorityLevelId = addOutbound.OutboundExternalBasicInfo.PriorityLevelId,
                        RemindDate = addOutbound.OutboundExternalBasicInfo.RemindDate,
                        RemindDateH = addOutbound.OutboundExternalBasicInfo.RemindDateH,
                        SignedById = addOutbound.OutboundExternalBasicInfo.SignedById,
                        Subject = addOutbound.OutboundExternalBasicInfo.Subject,
                        SubjectClassifications = addOutbound.OutboundExternalBasicInfo.SubjectClassifications,
                        SuggestedTopicId = addOutbound.OutboundExternalBasicInfo.SuggestedTopicId,
                        LetterTypeId = addOutbound.OutboundExternalBasicInfo.LetterTypeId,
                        DeliveryMethodId = addOutbound.OutboundExternalBasicInfo.DeliveryMethodId,
                        IsDraft = addOutbound.OutboundExternalBasicInfo.IsDraft,
                        POBox = addOutbound.OutboundExternalBasicInfo.POBox,
                        PostCode = addOutbound.OutboundExternalBasicInfo.PostCode,
                        ReporterId = addOutbound.OutboundExternalBasicInfo.ReporterId,
                        TransactionPathId = addOutbound.OutboundExternalBasicInfo.TransactionPathId,
                        SubjectClassificationsId = addOutbound.OutboundExternalBasicInfo.SubjectClassificationsId,
                        PreparationEntityId = addOutbound.OutboundExternalBasicInfo.PreparationEntityId,
                        isOutboundInternalDraft = addOutbound.OutboundExternalBasicInfo.isOutboundInternalDraft
                    };

                    SessionInfo.SessionId = loginPostResult.Result.SessionId;
                    SessionInfo.AccessToken = loginPostResult.Result.AccessToken;

                    postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest(SystemConfigurations.WebApiUrl + "api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundDraftDTO).Result;
                }

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    string message = postResult.StatusCode.ToString(); 
                    return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, message);
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, postResult.Result.Number);
            }
            catch (BusinessException ex)
            {
                StatusCode statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                PostObjectResult<TransactionDetailsDTO>
                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                StatusCode statusCode = MCS.Common.StatusCode.GeneralError;
                PostObjectResult<TransactionDetailsDTO> postObjectResult =
                    PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetExternalParties()
        {
            try
            {
                GetResult<List<ExternalPartyDTO>> getResult = 
                    HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(SystemConfigurations.WebApiUrl + "api/Common/GetExternalParties?parentId=null&cultureName=" + SessionInfo.CultureShortName).Result;

                if (getResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    string message = getResult.StatusCode.ToString();
                    return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, message);
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                StatusCode statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                PostObjectResult<TransactionDetailsDTO>
                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                StatusCode statusCode =MCS.Common.StatusCode.GeneralError;
                PostObjectResult<TransactionDetailsDTO> postObjectResult =
                    PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }
    }
}