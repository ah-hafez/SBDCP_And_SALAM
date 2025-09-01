
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Persistence;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class UserProfileController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage SearchUsersByOrgUnitId(string cultureName, int? orgUnitId, string term)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<UserProfile> usersProfiles = userManagementBL.SearchUsersByOrgUnitId(orgUnitId, cultureName, term);

                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage GetUsersByOrgUnitId(string cultureName, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersByOrgUnitId(orgUnitId, cultureName);

                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitUsers([FromUri] SearchCriteria searchCriteria, int orgUnitId, string cultureName, bool noExternal = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<UserProfile> usersProfiles = userManagementBL.GetOrgUnitUsers(searchCriteria, orgUnitId, cultureName, out int ItemsCount, noExternal);

                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, ItemsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage GetChildEntityUsersByOrgUnitId(string cultureName, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<UserProfile> usersProfiles = userManagementBL.GetChildEntityUsersByOrgUnitId(orgUnitId, cultureName);

                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAssignmentGroupById(int groupId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<AssignmentGroupDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    AssignmentGroupDTO userProfileEditDTO = AssignmentGroupMapper.Map(userManagementBL.GetAssignmentGroupById(groupId, cultureName));

                    getResult = GetResult<AssignmentGroupDTO>.Create(statusCode, userProfileEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserAssignmentGroups(int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AssignmentGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<AssignmentGroup> assignmentGroups =
                        userManagementBL.GetUserAssignmentGroups(userId, cultureName);

                    List<AssignmentGroupDTO> assignmentGroupDTO = AssignmentGroupMapper.Map(assignmentGroups);

                    getResult = GetResult<List<AssignmentGroupDTO>>.Create(statusCode, assignmentGroupDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AssignmentGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AssignmentGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostAssignmentGroup(AssignmentGroupDTO assignmentGroupDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        AssignmentGroup assignmentGroup = AssignmentGroupMapper.Map(assignmentGroupDTO);

                        int groupId = userManagementBL.AddAssignmentGroup(assignmentGroup, cultureName);

                        postResult = PostResult.Create(statusCode, groupId);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, -1);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage ChangePassword(string oldPassword, string newPassword)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.ChangePassword(oldPassword, newPassword);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPriorities(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PriorityDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();

                    TransactionCategories transactionCategories =
                        TransactionCategories.Inbound |
                        TransactionCategories.InternalOutbound |
                        TransactionCategories.DraftOutbound;

                    IList<Priority> priorities = priorityBL.GetPriorities(transactionCategories, cultureName);

                    List<PriorityDTO> prioritiesDTO = PriorityMapper.Map(priorities, cultureName);

                    getResult = GetResult<List<PriorityDTO>>.Create(statusCode, prioritiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PriorityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PriorityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostUserPreference(UserPreferenceDTO userPreferenceDTO, int? orgUnitId = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserPreference UserPreference = UserPreferenceMapper.Map(userPreferenceDTO);

                        userManagementBL.AddUserPreference(UserPreference);

                        postResult = PostResult.Create(statusCode, UserPreference.Id);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, -1);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserPreference(int userId, string cultureName, int? orgUnitId = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<UserPreferenceDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserPreferenceInfo userPreferenceInfo = userManagementBL.GetUserPreferenceByUserId(userId, cultureName, orgUnitId);

                        UserPreferenceDTO userPreferenceDTO = null;

                        if (userPreferenceInfo != null)
                        {
                            userPreferenceDTO = UserPreferenceInfoMapper.Map(userPreferenceInfo, cultureName);

                            userPreferenceDTO.Email = userPreferenceInfo.UserProfile.Email;
                            userPreferenceDTO.PhoneNumber = userPreferenceInfo.UserProfile.PhoneNumber;
                        }

                        getResult = GetResult<UserPreferenceDTO>.Create(statusCode, userPreferenceDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserSignByType(int userId, int signType)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<string> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        byte[] content = userManagementBL.GetUserSignByType(userId, signType);

                        string contentString = "";
                        if (content != null && content.Length > 0)
                        {
                            contentString = Convert.ToBase64String(content);
                        }

                        getResult = GetResult<string>.Create(statusCode, contentString, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<string>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutUserPreference(UserPreferenceDTO userPreferenceDTO, int? orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserPreference UserPreference = UserPreferenceMapper.Map(userPreferenceDTO);

                        userManagementBL.UpdateUserPreference(UserPreference, orgUnitId);

                        UserProfile userProfile = userManagementBL.GetUserById(UserPreference.UserProfileId);

                        userProfile.Email = userPreferenceDTO.Email;
                        userProfile.PhoneNumber = userPreferenceDTO.PhoneNumber;

                        userManagementBL.UpdateUser(userProfile, "ar");

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostUserDelegations(int userId, List<UserDelegationDTO> userDelegationDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        List<UserDelegation> userDelegations = UserDelegationMapper.Map(userDelegationDTOs);

                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        userManagementBL.UpdateUserDelegations(userId, userDelegations, Language);

                        postResult = PostResult.Create(statusCode, null);



                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpPost]
        public HttpResponseMessage UpdateUserDelegationStatus(int delegateId, int statusId, string rejectionReason, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        userManagementBL.UpdateUserDelegationStatus(delegateId, statusId, rejectionReason, cultureName);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpPut]
        public HttpResponseMessage PutUserDelegation(UserDelegationDTO editUserDelegationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserDelegation userDelegation = UserDelegationMapper.Map(editUserDelegationDTO);

                        userManagementBL.UpdateUserDelegation(userDelegation, Language);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteDelegations(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> delegationIds = ids.Split(',').Select(int.Parse).ToList();
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    userManagementBL.DeleteDelegations(delegationIds);

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

        [HttpGet]
        public HttpResponseMessage GetUserDelegationById(int id, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<UserDelegationDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserDelegation userDelegation = userManagementBL.GetUserDelegationById(id, cultureName);

                        UserDelegationDTO userDelegationEditDTO = UserDelegationMapper.Map(userDelegation);

                        getResult = GetResult<UserDelegationDTO>.Create(statusCode, userDelegationEditDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<UserDelegationDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserDelegationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserDelegationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage GenerateVerificationCode(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    bool result = userManagementBL.GenerateVerificationCode(userId, Language);
                    getResult = GetResult<bool>.Create(statusCode, result, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetUserDelegations(int preferenceId, [FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<UserDelegationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        List<UserDelegation> userDelegation = userManagementBL.GetUserDelegations(preferenceId, searchCriteria, out rowsCount);

                        List<UserDelegationDTO> userDelegationDTOs = UserDelegationMapper.Map(userDelegation);

                        getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, userDelegationDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserDelegationsByUserId(int? userId, string cultureName, int? orgUnitId, [FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<UserDelegationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    //if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        List<UserDelegation> userDelegation = userManagementBL.GetUserDelegationsByUserId(userId, cultureName, orgUnitId, searchCriteria, out rowsCount);

                        List<UserDelegationDTO> userDelegationDTOs = UserDelegationMapper.Map(userDelegation);

                        getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, userDelegationDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAssignmentPaperByUserId(int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        AssignmentPaper assignmentPaper = userManagementBL.GetAssignmentPaperByUserId(userId, cultureName);

                        AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(assignmentPaper);

                        getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, assignmentPaperDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddAssignmentPaper(AssignmentPaperDTO assignmentPaper, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.AddAssignmentPaper(AssignmentPaperMapper.Map(assignmentPaper), userId);

                        getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateAssignmentPaper(AssignmentPaperDTO assignmentPaper, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateAssignmentPaper(AssignmentPaperMapper.Map(assignmentPaper), userId);

                        getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTO, int groupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperBeneficiaryDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateAssignmentPaperBeneficiary(AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryDTO), groupId);

                        getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateGroupAssignmentPaper(List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperBeneficiaryDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateAssignmentPaperBeneficiary(AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryDTO));

                        getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperBeneficiaryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDistributionListById(int userId, int orgUnitId, string cultureName, int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<DistributionListDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        DistributionList distributionLists = userManagementBL.GetDistributionListById(userId, orgUnitId, id);

                        getResult = GetResult<DistributionListDTO>.Create(statusCode, DistributionListMapper.Map(distributionLists, cultureName), null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<DistributionListDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DistributionListDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DistributionListDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetDistributionList(int userId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<DistributionListDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        List<DistributionList> distributionLists = userManagementBL.GetDistributionList(userId, orgUnitId);

                        getResult = GetResult<List<DistributionListDTO>>.Create(statusCode, DistributionListMapper.Map(distributionLists, cultureName), null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<DistributionListDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<DistributionListDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<DistributionListDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage SaveDistributionListDetails(List<DistributionListDetailsDTO> distributionListDetailsDTOs, int DistributionListId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        int result = userManagementBL.SaveDistributionListDetails(DistributionListMapper.Map(distributionListDetailsDTOs), DistributionListId);

                        getResult = GetResult<int>.Create(statusCode, result, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<int>.Create(statusCode, 0, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage AddDistributionList(DistributionListDTO distributionListDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        int result = userManagementBL.AddDistributionList(DistributionListMapper.Map(distributionListDTO));

                        getResult = GetResult<int>.Create(statusCode, result, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<int>.Create(statusCode, 0, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteDistributionList(DistributionListDTO distributionListDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        int result = userManagementBL.DeleteDistributionList(distributionListDTO.Id);

                        getResult = GetResult<int>.Create(statusCode, result, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<int>.Create(statusCode, 0, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateDistributionList(DistributionListDTO distributionListDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        int result = userManagementBL.UpdateDistributionList(DistributionListMapper.Map(distributionListDTO));

                        getResult = GetResult<int>.Create(statusCode, result, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<int>.Create(statusCode, 0, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage VerifySignaturePassword(CredentialDTO credentialDTO, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        var result = userManagementBL.VerifySignaturePassword(credentialDTO.SignatureCurrentPasswordTxt, userId);
                        getResult = GetResult<bool>.Create(statusCode, result, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }
                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<bool>.Create(statusCode, false, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitManager(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    UserProfile manager = orgUnitBL.GetOrgUnitManager(orgUnitId, cultureName);

                    UserProfileDTO userProfileDTO = UserProfileMapper.MapUserProfile(manager);

                    getResult = GetResult<UserProfileDTO>.Create(statusCode, userProfileDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserProfileDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserProfileDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTransactionPath(TransactionPathDTO transactionPathDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        TransactionPath transactionPath = TransactionPathMapper.Map(transactionPathDTO);

                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        userManagementBL.UpdateTransactionPath(transactionPath, Language);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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
        public HttpResponseMessage GetPathsName(int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionPathDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    List<TransactionPath> Names = userManagementBL.GetPathsName(OrgUnitId);
                    List<TransactionPathDTO> transactionPathDTOs = TransactionPathMapper.Map(Names);
                    getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, transactionPathDTOs, 0);
                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionPath(int? userId, int? orgUnitId, int pageIndex, int pageSize)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionPathDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    List<TransactionPath> transactionPaths = userManagementBL.GetTransactionPath(userId, orgUnitId, pageIndex, pageSize, Language, out int rowsCount);

                    List<TransactionPathDTO> transactionPathDTOs = TransactionPathMapper.Map(transactionPaths);

                    getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, transactionPathDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetAllPaths(int pageIndex, int pageSize)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionPathDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    List<TransactionPath> transactionPaths = userManagementBL.GetAllPaths(pageIndex, pageSize, Language, out int rowsCount);

                    List<TransactionPathDTO> transactionPathDTOs = TransactionPathMapper.Map(transactionPaths);

                    getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, transactionPathDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionPathForTransaction(int? userId, int? orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionPathDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    List<TransactionPath> transactionPaths = userManagementBL.GetTransactionPathForTransaction(userId, orgUnitId, Language);

                    List<TransactionPathDTO> transactionPathDTOs = TransactionPathMapper.Map(transactionPaths);

                    getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, transactionPathDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionPathDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionPathById(int pathId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<TransactionPathDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        TransactionPath transactionPath = userManagementBL.GetTransactionPathById(pathId, Language);

                        TransactionPathDTO transactionPathDTO = TransactionPathMapper.Map(transactionPath);

                        getResult = GetResult<TransactionPathDTO>.Create(statusCode, transactionPathDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<TransactionPathDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionPathDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionPathDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteTransactionPath(int pathId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int isDeleted = userManagementBL.DeleteTransactionPath(pathId);

                        postResult = PostResult.Create(statusCode, isDeleted);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpPost]
        public HttpResponseMessage UpdateTransactionPathDetailsSort(int pathId, int sort, string order)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        userManagementBL.UpdateTransactionPathDetailsSort(pathId, sort, order);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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
        public HttpResponseMessage GetAllUsers(string cultureName, string searchQuery, int? entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersProfiles(cultureName, searchQuery, entityId);
                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetUserPreferenceInfoByUserId(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<UserPreferenceDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        UserPreferenceInfo userPreferenceInfo = userManagementBL.GetUserPreferenceInfoByUserId(userId, Language);

                        UserPreferenceDTO userPreferenceDTO = null;

                        if (userPreferenceInfo != null)
                        {
                            userPreferenceDTO = UserPreferenceInfoMapper.Map(userPreferenceInfo, Language);
                        }

                        getResult = GetResult<UserPreferenceDTO>.Create(statusCode, userPreferenceDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserPreferenceDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateUserProfile(int userId, string email)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateUserProfile(userId, email);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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


        [HttpPost]
        public HttpResponseMessage ChangeGroupOrder(int id, bool isMoveUp)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.ChangeGroupOrder(id, isMoveUp);
                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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
        [HttpPost]
        public HttpResponseMessage UpdateUserPreference(int userId, string code)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateUserPreference(userId, code);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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
        [HttpPost]
        public HttpResponseMessage UpdateSignaturePassword(CredentialDTO credentialDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.UpdateSignaturePassword(credentialDTO.SignatureNewPasswordTxt, credentialDTO.PasswordType);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpPut]
        public HttpResponseMessage RemoveSignaturePassword(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        userManagementBL.RemoveSignaturePassword(userId);

                        postResult = PutResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PutResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PutResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PutResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }
        public HttpResponseMessage GetAssignmentPaperGroupsByUserId(int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<AssignmentPaperGroupDTO>> getResult = null;

            try
            {
                List<AssignmentPaperGroupDTO> AssignmentPaperGroupDTOList = new List<AssignmentPaperGroupDTO>();

                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    List<AssignmentPaperGroup> assignmentPaperGroupList = userManagementBL.GetAssignmentPaperGroupsByUserId(userId);

                    if (assignmentPaperGroupList != null)
                    {
                        AssignmentPaperGroupDTOList = AssignmentPaperGroupMapper.Map(assignmentPaperGroupList, cultureName);
                    }

                    getResult = GetResult<List<AssignmentPaperGroupDTO>>.Create(statusCode, AssignmentPaperGroupDTOList, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AssignmentPaperGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AssignmentPaperGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetBeneficiaryByAssignmentPaperGroupId(int groupId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<AssignmentPaperBeneficiaryDTO>> getResult = null;

            try
            {
                List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOList = new List<AssignmentPaperBeneficiaryDTO>();

                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    List<AssignmentPaperBeneficiary> assignmentPaperGroupList = userManagementBL.GetBeneficiaryByAssignmentPaperGroupId(groupId);

                    if (assignmentPaperGroupList != null)
                    {
                        assignmentPaperBeneficiaryDTOList = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperGroupList, cultureName);
                    }

                    getResult = GetResult<List<AssignmentPaperBeneficiaryDTO>>.Create(statusCode, assignmentPaperBeneficiaryDTOList, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AssignmentPaperBeneficiaryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AssignmentPaperBeneficiaryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostAssignmentPaperGroup(AssignmentPaperGroupDTO assignmentPaperGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<List<int>> postResult = null;
            IList<int> AssignmentPaperGroupsUsed = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    AssignmentPaperGroup assignmentPaperGroup = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTO);

                    userManagementBL.SaveAssignmentPaperGroup(assignmentPaperGroup);

                    postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostObjectResult<List<int>>.Create(statusCode, AssignmentPaperGroupsUsed.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        public HttpResponseMessage GetAssignmentPaperGroupById(int assignmentPaperGroupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AssignmentPaperGroupDTO> getResult = null;

            try
            {
                AssignmentPaperGroupDTO assignmentPaperGroupDTO = new AssignmentPaperGroupDTO();

                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    AssignmentPaperGroup assignmentPaperGroup = userManagementBL.GetAssignmentPaperGroupById(assignmentPaperGroupId);

                    if (assignmentPaperGroup != null)
                    {
                        assignmentPaperGroupDTO = AssignmentPaperGroupMapper.Map(assignmentPaperGroup);
                    }

                    getResult = GetResult<AssignmentPaperGroupDTO>.Create(statusCode, assignmentPaperGroupDTO, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AssignmentPaperGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AssignmentPaperGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage PutAssignmentPaperGroup(AssignmentPaperGroupDTO assignmentPaperGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        AssignmentPaperGroup assignmentPaperGroup = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTO);

                        userManagementBL.UpdateAssignmentPaperGroup(assignmentPaperGroup);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLoggedInUserDelegations(int UserId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<UserDelegationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        List<UserDelegation> userDelegation = userManagementBL.GetLoggedInUserDelegations(UserId, cultureName);

                        List<UserDelegationDTO> userDelegationDTOs = UserDelegationMapper.Map(userDelegation);

                        getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, userDelegationDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpGet]
        public HttpResponseMessage GetUserDelegationsById(int UserId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<UserDelegationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        List<UserDelegation> userDelegation = userManagementBL.GetUserDelegationsById(UserId, cultureName);

                        List<UserDelegationDTO> userDelegationDTOs = UserDelegationMapper.Map(userDelegation);

                        getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, userDelegationDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserDelegationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage AddAllowedAssignment(AllowedAssignmentDTO allowedAssignmentDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                        AllowedAssignment allowedAssignment = AllowedAssignmentMapper.Map(allowedAssignmentDTO);

                        int groupId = userManagementBL.AddAllowedAssignment(allowedAssignment);

                        postResult = PostResult.Create(statusCode, groupId);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, -1);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetAllowedAssignment(int UserId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<AllowedAssignmentDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        List<AllowedAssignment> allowedAssignment = userManagementBL.GetAllowedAssignment(UserId, cultureName);

                        List<AllowedAssignmentDTO> allowedAssignmentDTOs = AllowedAssignmentMapper.Map(allowedAssignment, cultureName);

                        getResult = GetResult<List<AllowedAssignmentDTO>>.Create(statusCode, allowedAssignmentDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<AllowedAssignmentDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AllowedAssignmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AllowedAssignmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage RemoveAllowedAssignment(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        bool removeAllowedResult = userManagementBL.RemoveAllowedAssignment(Id);


                        getResult = GetResult<bool>.Create(statusCode, removeAllowedResult, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<bool>.Create(statusCode, false, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllowedUserAssignment(int ToUserId, int FromUserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<AllowedAssignmentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        int rowsCount = 0;
                        AllowedAssignment Result = userManagementBL.GetAllowedUserAssignment(ToUserId, FromUserId);

                        AllowedAssignmentDTO allowedAssignmentDTOs = AllowedAssignmentMapper.Map(Result);

                        getResult = GetResult<AllowedAssignmentDTO>.Create(statusCode, allowedAssignmentDTOs, rowsCount);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<AllowedAssignmentDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AllowedAssignmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AllowedAssignmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage DeleteAssignmentPaperGroup(int assignmentPaperGroupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<int> deleteResult = null;

            try
            {
                AssignmentPaperGroupDTO assignmentPaperGroupDTO = new AssignmentPaperGroupDTO();

                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    userManagementBL.DeleteAssignmentPaperGroupById(assignmentPaperGroupId);

                    transactionContextScope.Commit();

                    deleteResult = GetResult<int>.Create(statusCode, 0, null);

                    return Request.CreateResponse(HttpStatusCode.Created, deleteResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateUserInternalNumber(UpdateUserProfileDto updateUserProfile)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<List<int>> postResult = null;
            IList<int> AssignmentPaperGroupsUsed = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    userManagementBL.UpdateUserInternalNumber(updateUserProfile.UserProfileId, updateUserProfile.PhoneNumber, updateUserProfile.InternalNumber);

                    postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostObjectResult<List<int>>.Create(statusCode, AssignmentPaperGroupsUsed.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

    }
}
