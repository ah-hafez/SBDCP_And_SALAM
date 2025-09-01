using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services;
using MCS.Common;
// using System.Web.Script.Serialization;
// using Framework;
// using Framework.Security;
// using Framework.Web;
//using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
// using MCS.Domain;

namespace MCS.IntegrationServices.Controllers
{
    public class OutlookIntegrationController : ApiController
    {
        #region Tranaction
        [HttpPost]
        public HttpResponseMessage CreateInbound(string cultureName, int orgUnitId, string access, int userId, MCS.DTO.AddInboundDTO inboundObj)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);
            var serviceCall = HttpClientWrapper<PostObjectResult<MCS.DTO.TransactionDetailsDTO>>.
                PostRequest("api/Transaction/PostTransaction?cultureName=" + cultureName + "&orgUnitId=" + orgUnitId, inboundObj).Result;
            PostObjectResult<MCS.DTO.TransactionDetailsDTO> postObjectResult = null;
            if (serviceCall.StatusCode != MCS.Common.StatusCode.Ok)
            {
                postObjectResult = PostObjectResult<MCS.DTO.TransactionDetailsDTO>.Create(serviceCall.StatusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }

            postObjectResult = PostObjectResult<MCS.DTO.TransactionDetailsDTO>.Create(MCS.Common.StatusCode.Ok, serviceCall.Result);
            return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
        }
        [WebMethod(EnableSession = true)]
        public HttpResponseMessage CreateOutboundInternal(string cultureName, int orgUnitId, string access, int userId, AddOutboundInternalDTO tranObj)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);

            tranObj.OutboundInternalBasicInfoAdd.DeliveryMethodId = 236; //Electronic

            // PostTransactionWithPdf
            var serviceCall = HttpClientWrapper<PostObjectResult<MCS.DTO.TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + cultureName + "&orgUnitId=" + orgUnitId, tranObj).Result;
            PostObjectResult<MCS.DTO.TransactionDetailsDTO> postObjectResult = null;
            if (serviceCall.StatusCode != MCS.Common.StatusCode.Ok)
            {
                postObjectResult = PostObjectResult<MCS.DTO.TransactionDetailsDTO>.Create(serviceCall.StatusCode, null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, postObjectResult);
            }

            postObjectResult = PostObjectResult<MCS.DTO.TransactionDetailsDTO>.Create(MCS.Common.StatusCode.Ok, serviceCall.Result);
            return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
        }

        [HttpPost]
        public HttpResponseMessage AssignmentCreate(string cultureName, string transactionIds, string access, int userId, List<MCS.DTO.TransactionAssignmentDTO> assigList)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);

            var servicePath = string.Format("api/Transaction/PostAssignTransactions?sTransactionsIds={0}&cultureName={1}", transactionIds, cultureName);
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(servicePath, assigList).Result;

            PutResult putResult = PutResult.Create(MCS.Common.StatusCode.Ok);
            if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
            {
                putResult = PutResult.Create(postResult.StatusCode);
                return Request.CreateResponse(HttpStatusCode.BadRequest, putResult);
            }

            return Request.CreateResponse(HttpStatusCode.OK, putResult);
        }
        #endregion

        #region OrgUnits
        [HttpPost]
        public HttpResponseMessage InternalUnits(string cultureName, int? parentId)
        {
            parentId = parentId == -1 ? null : parentId;
            GetResult<List<OrgUnitDTO>> serviceCall =
                HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(
                    string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}", cultureName, parentId)).Result;

            return Request.CreateResponse(HttpStatusCode.OK, serviceCall);
        }
        [HttpPost]
        public HttpResponseMessage InternalUnitsAutoComplete(string cultureName, string searchQuery, int pageSize)
        {
            GetResult<List<MCS.DTO.OrgUnitDTO>> serviceCall =
                HttpClientWrapper<GetResult<List<MCS.DTO.OrgUnitDTO>>>.GetItemRequest
                (string.Format("api/Common/GetOrgUnitsAutoComplete?cultureName={0}&searchQuery={1}&resultSize={2}", cultureName, searchQuery, pageSize)).Result;

            return Request.CreateResponse(HttpStatusCode.OK, serviceCall);
        }
        [HttpPost]
        public HttpResponseMessage ExternalUnits(string cultureName, int? parentId)
        {
            string servicePath = string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", cultureName, parentId);
            PostObjectResult<List<MCS.DTO.ExternalPartyDTO>> postResult = null;
            var serviceCall = HttpClientWrapper<GetResult<List<MCS.DTO.ExternalPartyDTO>>>.GetItemRequest(servicePath).Result;

            if (serviceCall.Result == null)
            {
                postResult = PostObjectResult<List<MCS.DTO.ExternalPartyDTO>>.
                    Create(MCS.Common.StatusCode.ActionNotFound, new List<MCS.DTO.ExternalPartyDTO>());
            }
            else
            {
                postResult = PostObjectResult<List<MCS.DTO.ExternalPartyDTO>>.
                    Create(MCS.Common.StatusCode.Ok, serviceCall.Result);
            }

            return Request.CreateResponse(HttpStatusCode.OK, postResult);

        }

        [HttpPost]
        public HttpResponseMessage ExternalUnitsAutoComplete(string cultureName, string searchQuery, string pageSize)
        {
            PostObjectResult<List<MCS.DTO.ExternalPartyDTO>> postResult = null;
            string servicePath = string.Format("api/Common/GetExternalPartiesAutoComplete?cultureName={0}&searchQuery={1}&resultSize={2}",
                    cultureName, searchQuery, pageSize);
            var serviceCall = HttpClientWrapper<GetResult<List<MCS.DTO.ExternalPartyDTO>>>.GetItemRequest(servicePath).Result;

            if (serviceCall.StatusCode != MCS.Common.StatusCode.Ok)
            {
                postResult = PostObjectResult<List<MCS.DTO.ExternalPartyDTO>>.
                    Create(MCS.Common.StatusCode.ActionNotFound, new List<MCS.DTO.ExternalPartyDTO>());
            }
            else
            {
                postResult = PostObjectResult<List<MCS.DTO.ExternalPartyDTO>>.
                    Create(MCS.Common.StatusCode.Ok, serviceCall.Result);
            }
            return Request.CreateResponse(postResult);
        }
        #endregion

        #region User Profile
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage Login(string emailAddress, string cultureName)
        {
            string servicePath = string.Format("api/Login/LoginByEmail?emailAddress={0}&cultureName={1}", emailAddress, cultureName);
            var service = HttpClientWrapper<PostObjectResult<MCS.DTO.UserDTO>>.PostRequest(servicePath, null).Result;

            if (service.StatusCode != MCS.Common.StatusCode.Ok)
                return Request.CreateResponse(HttpStatusCode.OK, service.Result);

            // to vm
            UserVM userVM = Mappers.UserMapper.Map(service.Result);
            SessionInfo.SetObjectInSession(userVM, Constants.LoggedInUserKey);

            service.Result.SessionId = System.Web.HttpContext.Current.Session.SessionID;

            GetResult<MCS.DTO.UserDTO> getResult = GetResult<MCS.DTO.UserDTO>.Create(MCS.Common.StatusCode.Ok, service.Result, 1);
            return Request.CreateResponse(HttpStatusCode.OK, getResult);
        }

        public HttpResponseMessage GetPriorities(string cultureName, string access, int userId)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);
            GetResult<List<MCS.DTO.PriorityDTO>> service =
           HttpClientWrapper<GetResult<List<MCS.DTO.PriorityDTO>>>.
           GetItemRequest(string.Format("api/UserProfile/GetPriorities?cultureName={0}", cultureName)).Result;
            GetResult<List<MCS.DTO.PriorityDTO>> getResult = GetResult<List<MCS.DTO.PriorityDTO>>.Create(MCS.Common.StatusCode.Ok, service.Result, service.Result.Count);
            return Request.CreateResponse(HttpStatusCode.OK, getResult);

        }

        public HttpResponseMessage GetConfidentialityLevel(string cultureName, string access, int userId)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);
            var urlPermission = string.Format("api/Common/GetOutlookPermissionsByGroupId?permissionGroupName={0}&cultureName={1}&userId={2}", PermissionGroupName.TransactiosConfidentiality, cultureName, userId);
            GetResult<List<MCS.DTO.PermissionDTO>> service = HttpClientWrapper<GetResult<List<MCS.DTO.PermissionDTO>>>.GetItemRequest(urlPermission).Result;
            GetResult<List<MCS.DTO.PermissionDTO>> getResult = GetResult<List<MCS.DTO.PermissionDTO>>.Create(MCS.Common.StatusCode.Ok, service.Result, service.Result.Count);
            return Request.CreateResponse(HttpStatusCode.OK, getResult);
        }

        public HttpResponseMessage GetSourceTypes(string cultureName, TransactionCategory transactionCategory, string access, int userId)
        {
            //UserVM user = new UserVM { Id = userId, AccessToken = access };
            //SessionInfo.SetObjectInSession(user, Constants.LoggedInUserKey);
            GetResult<List<MCS.DTO.TransactionTypeDTO>> service =
           HttpClientWrapper<GetResult<List<MCS.DTO.TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionSourceTypes?cultureName=" + cultureName + "&transactionCategory={0}", transactionCategory)).Result;
            GetResult<List<MCS.DTO.TransactionTypeDTO>> getResult = GetResult<List<MCS.DTO.TransactionTypeDTO>>.Create(MCS.Common.StatusCode.Ok, service.Result, service.Result.Count);
            return Request.CreateResponse(HttpStatusCode.OK, getResult);
        }

        [HttpPost]
        public HttpResponseMessage GetUsersByOrgUnitId(string cultureName, int orgUnitId)
        {
            PostObjectResult<List<MCS.DTO.UserProfileDTO>> postResult = null;

            var service = HttpClientWrapper<GetResult<List<MCS.DTO.UserProfileDTO>>>.GetItemRequest(
                string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", cultureName, orgUnitId)).Result;

            if (service.StatusCode != MCS.Common.StatusCode.Ok)
            {
                postResult = PostObjectResult<List<MCS.DTO.UserProfileDTO>>.
                    Create(MCS.Common.StatusCode.ActionNotFound, new List<MCS.DTO.UserProfileDTO>());
            }
            else
            {
                postResult = PostObjectResult<List<MCS.DTO.UserProfileDTO>>.
                    Create(MCS.Common.StatusCode.Ok, service.Result);
            }
            return Request.CreateResponse(postResult);
        }
        #endregion
    }
}
