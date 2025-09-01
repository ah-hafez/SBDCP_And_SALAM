using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Persistence;
using MCS.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MCS.Service.Controllers
{
    [CustomAuthentication]
    public class IAMController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetAllUsers([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersProfiles(searchCriteria, out rowsCount);

                    IList<Lookup> lookups =
                        CacheHelper.Get(CachedObjectsKey.Lookups + LookupCategory.Gender.ToString(), searchCriteria.CultureName) as IList<Lookup>;

                    if (lookups == null)
                    {
                        ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                        lookups = lookupBL.GetLookupItems(LookupCategory.Gender, searchCriteria.CultureName);

                        CacheHelper.Insert(CachedObjectsKey.Lookups + LookupCategory.Gender.ToString(), lookups, searchCriteria.CultureName);
                    }

                    List<LookupDTO> lookupDTOs = LookupMapper.Map(lookups);
                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles, lookups, searchCriteria.CultureName);


                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, usersProfileDTOs, rowsCount);

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
        public HttpResponseMessage GetAllRoles([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> permissionGroups = permissionBL.GetPermissionsGroups_IAM(searchCriteria, out rowsCount).ToList();
                    List<PermissionGroupDTO> permissionsGroupDTOs = PermissionMapper.Map(permissionGroups);

                    getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, permissionsGroupDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserProfileByName([FromUri] string username)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserProfile usersProfiles = userManagementBL.GetUserByUserName(username);
                    UserProfileDTO userProfileDTO = UserProfileMapper.MapUserProfile(usersProfiles);
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



        [HttpGet]
        public HttpResponseMessage GetAllUsersInAllRoles([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<UserGroup> usersGroups = permissionBL.GetAllUserGroups(searchCriteria, out rowsCount).ToList();
                    var groupUserDto = UserGroupMapper.Map(usersGroups.ToList(), "ar");
                    getResult = GetResult<List<UserGroupDTO>>.Create(statusCode, groupUserDto, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllGroups([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<RoleDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> usersGroups = permissionBL.GetAllGroups(searchCriteria, out rowsCount).ToList();
                    var groupUserDto = UserGroupMapper.MapRole(usersGroups.ToList(), "ar");
                    getResult = GetResult<List<RoleDTO>>.Create(statusCode, groupUserDto, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<RoleDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<RoleDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostUser(AddUserProfileDTO AddUserProfileDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {

                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();



                int userid = userManagementBL.AddUser(UserProfileMapper.Map(AddUserProfileDTO), "", "");
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateSMSNotificationsConfirm(AddUserProfileDTO.Id, AddUserProfileDTO.SMSNotifications);
                userPreferenceRepository.UpdatefollowUpUser(AddUserProfileDTO.Id, AddUserProfileDTO.IsFollowUpUser);
                userPreferenceRepository.UpdateUserMobile(userid, AddUserProfileDTO.MainOrgUnitId, AddUserProfileDTO.AllowMobile);

                postResult = PostResult.Create(statusCode, userid);


                return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutUser(EditUserProfileDTO EditUserProfileDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {


                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                UserProfile userProfile = UserProfileMapper.IAMMap(EditUserProfileDTO);

                userManagementBL.IAMUpdateUser(userProfile, Language);
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateSMSNotificationsConfirm(EditUserProfileDTO.Id, EditUserProfileDTO.SMSNotifications);
                userPreferenceRepository.UpdatefollowUpUser(EditUserProfileDTO.Id, EditUserProfileDTO.IsFollowUpUser);
                userPreferenceRepository.UpdateUserMobile(EditUserProfileDTO.Id, EditUserProfileDTO.MainOrgUnitId, EditUserProfileDTO.AllowMobile);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);


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
        public HttpResponseMessage AddUserGroup(UserGroupDTO userGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                userManagementBL.AddUserGroup(userGroupDTO.UserId, userGroupDTO.GroupId);
                putResult = PutResult.Create(statusCode);
                return Request.CreateResponse(HttpStatusCode.OK, putResult);
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
        public HttpResponseMessage RemoveUserGroup(UserGroupDTO userGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {


                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();


                userManagementBL.RemoveUserGroup(userGroupDTO.UserId, userGroupDTO.GroupId);


                putResult = PutResult.Create(statusCode);


                return Request.CreateResponse(HttpStatusCode.OK, putResult);


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
        public HttpResponseMessage GetAllOrgunits([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.IAMGetOrgUnits(searchCriteria.CultureName);
                    List<OrgUnitDTO> orgUnitDTOs = OrgUnitMapper.Map(orgUnits, searchCriteria.CultureName);
                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, orgUnitDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

    }
}
