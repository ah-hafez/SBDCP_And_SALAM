using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DTO;
using MCS.IntegrationServices.Common;
using MCS.IntegrationServices.Mappers;
using MCS.IntegrationServices.Models.IAM.User;
using MCS.IntegrationServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using MCS.Framework.Logging;
using MCS.Framework.Exceptions;
using Swashbuckle.Swagger;
using System.Web.Services.Description;
using MCS.IntegrationServices.Models.IAM.Role;
using MobileApi.Domain;
using System.IdentityModel.Metadata;
using MCS.IntegrationServices.Models.IAM.Common;

namespace MCS.IntegrationServices.Controllers
{
    [BasicAuthentication]
    public class UserController : BaseApiController
    {
        private string sToken = string.Empty;
        public string Token
        {
            get
            {
                return sToken != string.Empty ? sToken : Request.Headers.Authorization.ToString();
            }
            set
            {
                sToken = value;
            }
        }



        [HttpGet]
        public IHttpActionResult GetAllUsers([FromUri] GetAllUserRequest request)
        {
            GetAllUserResponse response = new GetAllUserResponse();
            try
            {
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.PageIndex.ToString() + request.PageSize.ToString() + request.RequestDate + (request.UserId.HasValue ? request.UserId.Value.ToString() : "");

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {

                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "Invalid signature ! ";
                    return Json(response);
                }


                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.
                                       GetItemRequest(string.Format("api/IAM/GetAllUsers?PageIndex={0}&PageSize={1}&CultureName={2}&UserId={3}",
                request.PageIndex, request.PageSize, SessionInfo.CultureShortName, request.UserId)).Result;
                response.Users = UserMapper.Map(userProfileDTOs.Result);
                response.TotalRecord = userProfileDTOs.RowsCount ?? 0;
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
                response.ResponseCode = ResponseCodeConst.Success;
                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }

        }
        [HttpGet]
        public IHttpActionResult GetAllRoles([FromUri] GetAllBaseRequest request)
        {
            GetAllRoleResponse response = new GetAllRoleResponse();
            try
            {
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.PageIndex.ToString() + request.PageSize.ToString() + request.RequestDate;

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "Invalid signature ! ";
                    return Json(response);
                }


                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.
                    GetItemRequest(string.Format("api/IAM/GetAllRoles?PageIndex={0}&PageSize={1}&CultureName={2}",
                request.PageIndex, request.PageSize, SessionInfo.CultureShortName)).Result;
                response.Roles = RoleMapper.Map(permissionGroupDTOs.Result);
                response.TotalRecord = permissionGroupDTOs.RowsCount ?? 0;
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
                response.ResponseCode = ResponseCodeConst.Success;
                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }

        }

        [HttpGet]
        public IHttpActionResult GetAllUsersInAllRoles([FromUri] GetAllBaseRequest request)
        {
            GetAllUsersInAllRolesResponse response = new GetAllUsersInAllRolesResponse();
            try
            {
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");
                var requestBody = request.PageIndex.ToString() + request.PageSize.ToString() + request.RequestDate;

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "signature Mismatch ! ";
                    return Json(response);
                }


                GetResult<List<RoleDTO>> usersGroupDTOs = HttpClientWrapper<GetResult<List<RoleDTO>>>.
                    GetItemRequest(string.Format("api/IAM/GetAllGroups?PageIndex={0}&PageSize={1}&CultureName={2}",
                request.PageIndex, request.PageSize, SessionInfo.CultureShortName)).Result;
                response.Roles = RoleMapper.Map(usersGroupDTOs.Result);
                response.TotalRecord = usersGroupDTOs.RowsCount ?? 0;
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
                response.ResponseCode = ResponseCodeConst.Success;
                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }

        }
        [HttpPost]
        public IHttpActionResult CreateUser(CreateUserRequest request)
        {

            CreateUserResponse response = new CreateUserResponse();
            response.ResponseCode = ResponseCodeConst.Success;

            try
            {

                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest("ar", out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");


                var requestBody = request.AllowMobile.ToString() + request.CategoryId.ToString() + request.Email + request.GenderId.ToString() +
request.InternalNumber.ToString() + request.IsActive.ToString() + request.IsManager.ToString() + request.MainOrgUnitId.ToString() +
 JsonConvert.SerializeObject(request.Names) + JsonConvert.SerializeObject(request.OrgUnits) + request.PhoneNumber + request.RequestDate + request.TitleId.ToString() +
request.TransactionProcessingPeriod.ToString() + request.Username + request.UserNationalId;

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "signature Mismatch ! ";
                    return Json(response);
                }


                var requestResult = HttpClientWrapper<PostResult>
                                          .PostRequest($"api/IAM/PostUser", UserMapper.Map(request), "ar", Token);


                PostResult postResult = requestResult.Result;

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    response.ResponseCode = postResult.StatusCode.ToString();
                    response.ResponseMessage = postResult.StatusCode.ToString();
                    return Json(response);
                }
                response.Id = requestResult.Result.Id ?? 0;

                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateUser(UpdateUserRequest request)
        {

            ApiBaseResponse response = new ApiBaseResponse();
            response.ResponseCode = ResponseCodeConst.Success;
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest("ar", out iHttpActionResult))
                {
                    return iHttpActionResult;
                }
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }

                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.AllowMobile.ToString() + request.CategoryId.ToString() + (request?.Email?.ToString() ?? "") + request.GenderId.ToString() +
request.Id.ToString() + (request?.InternalNumber?.ToString() ?? "") + request.IsActive.ToString() + request.IsManager.ToString() + request.MainOrgUnitId.ToString() +
JsonConvert.SerializeObject(request.Names) + JsonConvert.SerializeObject(request.OrgUnits) + (request?.PhoneNumber?.ToString() ?? "") + request.RequestDate + request.TitleId.ToString() +
request.TransactionProcessingPeriod.ToString() + JsonConvert.SerializeObject(request.UserRoles) + request.Username.ToString() + (request?.UserNationalId?.ToString() ?? "");

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "signature Mismatch ! ";
                    return Json(response);
                }

                PutResult putResult = HttpClientWrapper<PutResult>
                                                      .PutRequest("api/IAM/PutUser", "ar", UserMapper.Update_Map(request), Token)
                                                      .Result;


                if (putResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    response.ResponseCode = putResult.StatusCode.ToString();
                    response.ResponseMessage = putResult.StatusCode.ToString();
                }


                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }
        }
        [HttpPost]
        public IHttpActionResult AssignUserRole(AssignRoleRequest request)
        {

            ApiBaseResponse response = new ApiBaseResponse();
            response.ResponseCode = ResponseCodeConst.Success;
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest("ar", out iHttpActionResult))
                {
                    return iHttpActionResult;
                }
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }


                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.RequestDate + request.RoleId.ToString() + request.UserId.ToString();
                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "signature Mismatch ! ";
                    return Json(response);
                }


                PostResult postResult = HttpClientWrapper<PostResult>
                                     .PostRequest("api/IAM/AddUserGroup", UserMapper.Map(request), "ar", Token)
                                     .Result;


                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    response.ResponseCode = postResult.StatusCode.ToString();
                    response.ResponseMessage = postResult.StatusCode.ToString();
                }


                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }
        }
        [HttpPost]
        public IHttpActionResult RevokeUserRole(AssignRoleRequest request)
        {

            ApiBaseResponse response = new ApiBaseResponse();
            response.ResponseCode = ResponseCodeConst.Success;
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest("ar", out iHttpActionResult))
                {
                    return iHttpActionResult;
                }
                string signature = "";
                var validateResult = ValidateReuqest(request, out signature);
                if (validateResult.ResponseCode != ResponseCodeConst.Success)
                {
                    return Json(validateResult);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.RequestDate + request.RoleId.ToString() + request.UserId.ToString();
                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature, userprofile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "signature Mismatch ! ";
                    return Json(response);
                }

                PostResult postResult = HttpClientWrapper<PostResult>
                                                    .PostRequest("api/IAM/RemoveUserGroup", UserMapper.Map(request), "ar", Token)
                                                    .Result;

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    response.ResponseCode = postResult.StatusCode.ToString();
                    response.ResponseMessage = postResult.StatusCode.ToString();
                }


                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }
        }

        private bool PreRequest(string languageAbbreviation, out IHttpActionResult iHttpActionResult)
        {
            AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

            if (authenticationIdentity == null || string.IsNullOrEmpty(authenticationIdentity.UserName))
            {
                iHttpActionResult = BadRequest();

                return false;
            }

            DataResult result = new DataResult();
            DateTime lastLoginDate = DateTime.MinValue;

            LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
            {
                UserName = authenticationIdentity.UserName,
                Password = string.Empty
            };

            PostObjectResult<string> postResultUserDTO =
               HttpClientWrapper<PostObjectResult<string>>.PostRequest("api/Login/LoginByMobile?cultureName=" + languageAbbreviation, loginInfoDTO, languageAbbreviation, string.Empty, -1).Result;

            Token = postResultUserDTO.Result;
            iHttpActionResult = Ok();


            GetResult<UserData> getResultUserData = HttpClientWrapper<GetResult<UserData>>
                                                    .GetItemRequest(string.Format("api/MobileApi/GetUserInfo?userName={0}", authenticationIdentity.UserName), languageAbbreviation, Token)
                                                    .Result;

            if (getResultUserData.Result == null && getResultUserData.RowsCount == 0)
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageAbbreviation);

                iHttpActionResult = Content(HttpStatusCode.Unauthorized, result);

                return false;
            }



            return true;
        }
        [HttpGet]
        public IHttpActionResult GetAllOrgunits([FromUri] BaseRequest request)
        {
            GetAllOrgUnitResponse response = new GetAllOrgUnitResponse();
            try
            {
                var signature = Request.Headers.Where(x => x.Key == "Signature").FirstOrDefault();
                if (signature.Value == null)
                {
                    response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                    response.ResponseMessage = "UnAuthorized Request";
                    return Json(response);
                }


                if (!ModelState.IsValid)
                {
                    response.ResponseCode = ResponseCodeConst.ValidationError;
                    response.ResponseMessage = string.Join("<br/> ", ModelState.Values
                           .SelectMany(x => x.Errors)
                           .Select(x => x.ErrorMessage));

                    return Json(response);
                }
                if (!IsValidDate(request.RequestDate))
                {

                    response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                    response.ResponseMessage = "Expired Request or Invalid Datetime";
                    return Json(response);
                }
                if (IsDuplicateSignature(signature.Value.FirstOrDefault()))
                {

                    response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                    response.ResponseMessage = "Duplicate Signature";
                    return Json(response);
                }
                GetResult<UserProfileDTO> userprofile = GetUserProfile("");

                var requestBody = request.RequestDate;

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature.Value.FirstOrDefault(), userprofile.Result.ApiKey);
                if (!isValidKey)
                {

                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "Invalid signature ! ";
                    return Json(response);
                }


                GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.
                    GetItemRequest(string.Format("api/IAM/GetAllOrgunits?CultureName={0}", SessionInfo.CultureShortName)).Result;
                response.Orgunits = OrgUnitMapper.Map(orgUnitDTOs.Result);
                response.ResponseCode = ResponseCodeConst.Success;
                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }

        }
        private ApiBaseResponse ValidateReuqest(BaseRequest request, out string signatureValue)
        {
            ApiBaseResponse response = new ApiBaseResponse();
            response.ResponseCode = ResponseCodeConst.Success;
            signatureValue = "";
            var signature = Request.Headers.Where(x => x.Key == "Signature").FirstOrDefault();
            if (signature.Value == null)
            {
                response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                response.ResponseMessage = "UnAuthorized Request";
                return response;
            }


            if (!ModelState.IsValid)
            {
                response.ResponseCode = ResponseCodeConst.ValidationError;
                response.ResponseMessage = string.Join("<br/> ", ModelState.Values
                       .SelectMany(x => x.Errors)
                       .Select(x => x.ErrorMessage));

                return response;
            }
            if (!IsValidDate(request.RequestDate))
            {

                response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                response.ResponseMessage = "Expired Request or Invalid Datetime";
                return response;
            }

            if (IsDuplicateSignature(signature.Value.FirstOrDefault()))
            {

                response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                response.ResponseMessage = "Duplicate Signature";
                return response;
            }
            signatureValue = signature.Value.FirstOrDefault();
            return response;
        }
    }


}
