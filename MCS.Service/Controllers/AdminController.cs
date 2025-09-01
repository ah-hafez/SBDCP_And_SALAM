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
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.DTO.Escalation;
using MCS.Service.Mappers;
using MCS.DataAccess;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class AdminController : ApiBaseController
    {
        #region Escalation
        [HttpGet]
        public HttpResponseMessage GetEscalations(int TransactionCategoryId, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<EscalationDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    List<Escalation> escalations = escalationBL.GetEscalations(TransactionCategoryId, cultureName).ToList();
                    List<EscalationDTO> escalationDTO = EscalationMapper.Map(escalations);
                    getResult = GetResult<List<EscalationDTO>>.Create(statusCode, escalationDTO, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<EscalationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<EscalationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEscalationCategoryId(int EscalationId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    int CategoryId = escalationBL.GetEscalationCategoryId(EscalationId);
                    getResult = GetResult<int>.Create(statusCode, CategoryId, rowsCount);
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
        [HttpGet]
        public HttpResponseMessage GetEscalationPriorityId(int EscalationId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    int PriorityId = escalationBL.GetEscalationPriorityId(EscalationId);
                    getResult = GetResult<int>.Create(statusCode, PriorityId, rowsCount);
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

        [HttpGet]
        public HttpResponseMessage GetEscalationsByPriorityId(int TransactionCategoryId, int priorityId, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<EscalationDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    List<Escalation> escalations = escalationBL.GetEscalationByPriority(TransactionCategoryId, priorityId, cultureName).ToList();
                    List<EscalationDTO> escalationDTO = EscalationMapper.Map(escalations);
                    getResult = GetResult<List<EscalationDTO>>.Create(statusCode, escalationDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<EscalationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<EscalationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEscalationById(int EscalationId, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EscalationDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    Escalation escalations = escalationBL.GetEscalationById(EscalationId);
                    EscalationDTO escalationDTO = EscalationMapper.Map(escalations, cultureName);
                    getResult = GetResult<EscalationDTO>.Create(statusCode, escalationDTO, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<EscalationDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<EscalationDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostEscalation(EscalationDTO escalationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    Escalation escalation = EscalationMapper.Map(escalationDTO);
                    int EscalationId = escalationBL.AddEscalation(escalation);
                    postResult = PostResult.Create(statusCode, EscalationId);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutEscalation(EscalationDTO escalationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                        Escalation escalation = EscalationMapper.Map(escalationDTO);
                        escalationBL.UpdateEscalation(escalation);
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

        [HttpDelete]
        public HttpResponseMessage DeleteEscalation(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IEscalationBL escalationBL = IoC.Resolve<IEscalationBL>();
                    escalationBL.DeleteEscalation(Id);
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
        #endregion Escalation
        #region User Management

        public HttpResponseMessage GetUsersByOrgId(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<UserProfile> usersProfiles = orgUnitBL.GetUsersByParentId(orgUnitId, cultureName);

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
        public HttpResponseMessage GetUserById(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EditUserProfileDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    UserProfile userProfile = userManagementBL.GetUserById(userId);

                    EditUserProfileDTO EditUserProfileDTO = UserProfileMapper.Map(userProfile, Language);
                    IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                    EditUserProfileDTO.SMSNotifications = userPreferenceRepository.GetSMSNotificationsConfirmByUserId(userId);
                    EditUserProfileDTO.IsFollowUpUser = userPreferenceRepository.GetfollowUpUserId(userId);

                    getResult = GetResult<EditUserProfileDTO>.Create(statusCode, EditUserProfileDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<EditUserProfileDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<EditUserProfileDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUsersProfiles([FromUri] SearchCriteria searchCriteria)
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
                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles, searchCriteria.CultureName);

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
        public HttpResponseMessage GetPendingRegestrationUsersProfiles([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetPendingRegestrationUsersProfiles(searchCriteria, out rowsCount);
                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles, searchCriteria.CultureName);

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
        public HttpResponseMessage GetUsersByOrgUnitId([FromUri] SearchCriteria searchCriteria, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersByOrgUnitId(orgUnitId, searchCriteria, out rowsCount);
                    List<UserProfileDTO> usersProfileDTOs = UserProfileMapper.Map(usersProfiles);

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
        public HttpResponseMessage GetAllUsers(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersProfiles(cultureName);
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
        public HttpResponseMessage GetUsersByPermissionId(string cultureName, int permissionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersByPermissionId(permissionId, cultureName);
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
        public HttpResponseMessage GetUsersByTrayId(string cultureName, int trayId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserProfile> usersProfiles = userManagementBL.GetUsersByTrayId(trayId, cultureName);
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

        [HttpPost]
        public HttpResponseMessage PostUsers(string cultureName, string resetPasswordUrl, List<AddUserProfileDTO> AddUserProfileDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    foreach (var AddUserProfileDTO in AddUserProfileDTOs)
                    {
                        UserProfile userProfile = UserProfileMapper.Map(AddUserProfileDTO);

                    }

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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

        [HttpPost]
        public HttpResponseMessage PostUser(string cultureName, string resetPasswordUrl, AddUserProfileDTO AddUserProfileDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    UserProfile userProfile = UserProfileMapper.Map(AddUserProfileDTO);

                    int userid = userManagementBL.AddUser(userProfile, resetPasswordUrl, cultureName);
                    IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                    userPreferenceRepository.UpdateSMSNotificationsConfirm(AddUserProfileDTO.Id, AddUserProfileDTO.SMSNotifications);
                    userPreferenceRepository.UpdatefollowUpUser(AddUserProfileDTO.Id, AddUserProfileDTO.IsFollowUpUser);
                    userPreferenceRepository.UpdateUserMobile(userid, AddUserProfileDTO.MainOrgUnitId, AddUserProfileDTO.AllowMobile);

                    postResult = PostResult.Create(statusCode, null);


                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutUser(EditUserProfileDTO EditUserProfileDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    UserProfile userProfile = UserProfileMapper.Map(EditUserProfileDTO);

                    userManagementBL.UpdateUser(userProfile, Language);
                    IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                    userPreferenceRepository.UpdateSMSNotificationsConfirm(EditUserProfileDTO.Id, EditUserProfileDTO.SMSNotifications);
                    userPreferenceRepository.UpdatefollowUpUser(EditUserProfileDTO.Id, EditUserProfileDTO.IsFollowUpUser);
                    userPreferenceRepository.UpdateUserMobile(EditUserProfileDTO.Id, EditUserProfileDTO.MainOrgUnitId, EditUserProfileDTO.AllowMobile);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

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

        [HttpPut]
        public HttpResponseMessage ActivateUser(int userId, bool isActive)
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

                        UserProfile userProfile = new UserProfile()
                        {
                            Id = userId,
                            IsActive = isActive
                        };

                        userManagementBL.ActivateUser(userProfile, Language);

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

        [HttpPost]
        public HttpResponseMessage DeleteUsers(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> usersCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> usersIds = ids.Split(',').Select(int.Parse).ToList();

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    userManagementBL.DeleteUsers(usersIds, out usersCannotBeDeleted, Language);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, usersCannotBeDeleted.ToList());
                    if (removeObjectResult.Result.Count > 0)
                    {
                        throw new BusinessException();
                    }

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = Common.StatusCode.UserUsedCanntDelete;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage CheckIfNotUsedUser(string id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        int usersId = Convert.ToInt32(id);
                        IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                        var result = userManagementBL.CheckIfNotUsedUser(usersId);

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
        public HttpResponseMessage GetUserPermissionGroups(int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    IList<UserPermission> userPermissions = userManagementBL.GetUserPermissions(userId, cultureName);
                    IList<Group> permissionsGroups = permissionBL.GetAllPermissionsGroups(cultureName, false);
                    List<PermissionGroupDTO> permissionsGroupsDTOs = PermissionMapper.Map(permissionsGroups);

                    foreach (PermissionGroupDTO permissionGroupDTO in permissionsGroupsDTOs)
                    {
                        foreach (PermissionDTO permissionDTO in permissionGroupDTO.Permissions)
                        {
                            if (userPermissions.Any(u => u.Permission.Id == permissionDTO.Id))
                            {
                                permissionDTO.IsSelected = true;
                            }
                        }
                    }

                    getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, permissionsGroupsDTOs, null);

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
        public HttpResponseMessage GetAllUserPermissions(int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<UserPermission> userPermissions = userManagementBL.GetUserPermissions(userId, cultureName);
                    List<PermissionDTO> PermissionsDTOs = PermissionMapper.MapUserPermissions(userPermissions);

                    getResult = GetResult<List<PermissionDTO>>.Create(statusCode, PermissionsDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ReSendNotificationEmail(int userId, string cultureName, string resetPasswordUrl)
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

                        UserProfile userprofile = userManagementBL.GetUserById(userId);

                        userManagementBL.SendUserCreationNotification(userprofile, cultureName, resetPasswordUrl);

                        postResult = PostResult.Create(statusCode, "");

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetAllUserTrays(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TrayDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    IList<Tray> userTrays = userManagementBL.GetUserTrays(userId);
                    List<TrayDTO> TraysDTOs = TrayMapper.Map(userTrays);

                    getResult = GetResult<List<TrayDTO>>.Create(statusCode, TraysDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  User Management

        #region Priority Managment

        [HttpPost]
        public HttpResponseMessage PostPriority(PriorityAddDTO priorityAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();
                        Priority priority = PriorityMapper.Map(priorityAddDTO);

                        int priorityId = priorityBL.AddPriority(priority);

                        postResult = PostResult.Create(statusCode, priorityId);

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
        public HttpResponseMessage PutPriority(PriorityEditDTO priorityEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();
                        Priority priority = PriorityMapper.Map(priorityEditDTO);

                        priorityBL.UpdatePriority(priority);

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

        [HttpPost]
        public HttpResponseMessage DeletePriorities(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> prioritiesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> prioritIds = ids.Split(',').Select(int.Parse).ToList();
                    IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();

                    priorityBL.DeletePriorities(prioritIds, out prioritiesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, prioritiesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPriorityById([FromUri] SearchCriteria searchCriteria, int priorityId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PriorityEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();
                    PriorityEditDTO priorityEditDTO = PriorityMapper.Map(priorityBL.GetPriorityById(searchCriteria, priorityId, out int PriorityExceptionsRowsCount), cultureName);

                    getResult = GetResult<PriorityEditDTO>.Create(statusCode, priorityEditDTO, PriorityExceptionsRowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PriorityEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PriorityEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPriorities([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PriorityDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();
                    List<Priority> priorities = priorityBL.GetPriorities(searchCriteria, out rowsCount).ToList();
                    List<PriorityDTO> prioritiesDTO = PriorityMapper.Map(priorities, searchCriteria.CultureName);

                    getResult = GetResult<List<PriorityDTO>>.Create(statusCode, prioritiesDTO, rowsCount);

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

        #endregion  Priority Managment 



        #region FollowUpPrioritytype  
        public HttpResponseMessage PostFollowUpPrioritytype(FollowUpLookUpAddDTO followUpPriorityTypeAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpPriorityTypeBL followUpPriorityTypeBL = IoC.Resolve<IFollowUpPriorityTypeBL>();
                        FollowUpPriorityType followUpLookUps = FollowUpLookUpsMapper.Map(followUpPriorityTypeAddDTO);
                        int FollowUpLookUpId = followUpPriorityTypeBL.AddFollowUpPrioritytype(followUpLookUps);

                        postResult = PostResult.Create(statusCode, FollowUpLookUpId);

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


        public HttpResponseMessage GetFollowUpPrioritytype([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpPriorityTypeBL followUpPriorityTypeBL = IoC.Resolve<IFollowUpPriorityTypeBL>();
                    List<FollowUpPriorityType> followUpPriorityType = followUpPriorityTypeBL.GetFollowUpPrioritytypes(searchCriteria, out rowsCount).ToList();
                    List<FollowUpLookUpDTO> followUpLookUpsDTOs = FollowUpLookUpsMapper.Map(followUpPriorityType, searchCriteria.CultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, followUpLookUpsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpPut]
        public HttpResponseMessage PutFollowUpPrioritytype(FollowUpLookUpEditDTO followUpPriorityTypeEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpPriorityTypeBL followUpPriorityTypeBL = IoC.Resolve<IFollowUpPriorityTypeBL>();
                        FollowUpPriorityType followUpPriorityType = FollowUpLookUpsMapper.Map(followUpPriorityTypeEditDTO);

                        followUpPriorityTypeBL.UpdateFollowUpPrioritytype(followUpPriorityType);

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
        public HttpResponseMessage GetFollowUpPrioritytypeById(int followUpPriorityTypeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<FollowUpLookUpEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpPriorityTypeBL followUpPriorityTypeBL = IoC.Resolve<IFollowUpPriorityTypeBL>();
                    FollowUpLookUpEditDTO followUpPriorityTypeEditDTO = FollowUpLookUpsMapper.Map(followUpPriorityTypeBL.GetFollowUpPrioritytypeId(followUpPriorityTypeId), cultureName);

                    getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, followUpPriorityTypeEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFollowUpPrioritytype(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> linkTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> followUpPriorityTypeIds = ids.Split(',').Select(int.Parse).ToList();
                    IFollowUpPriorityTypeBL followUpPriorityTypeBL = IoC.Resolve<IFollowUpPriorityTypeBL>();

                    followUpPriorityTypeBL.DeleteFollowUpPrioritytype(followUpPriorityTypeIds, out linkTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, linkTypesCannotBeDeleted.ToList());
                    if (linkTypesCannotBeDeleted.Count >= 1)
                    {
                        throw new BusinessException(Common.StatusCode.PermissionLinkDeleteLink);
                    }
                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        #endregion  FollowUpPrioritytype   
        #region FollowUpMethod

        public HttpResponseMessage PostFollowUpMethod(FollowUpLookUpAddDTO followUpMethodAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpMethodBL followUpMethodBL = IoC.Resolve<IFollowUpMethodBL>();
                        FollowUpMethod followUpLookUps = FollowUpMethodMapper.Map(followUpMethodAddDTO);
                        int FollowUpLookUpId = followUpMethodBL.AddFollowUpMethod(followUpLookUps);

                        postResult = PostResult.Create(statusCode, FollowUpLookUpId);

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


        public HttpResponseMessage GetFollowUpMethod([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpMethodBL followUpMethodBL = IoC.Resolve<IFollowUpMethodBL>();
                    List<FollowUpMethod> followUpMethod = followUpMethodBL.GetFollowUpMethods(searchCriteria, out rowsCount).ToList();
                    List<FollowUpLookUpDTO> followUpLookUpsDTOs = FollowUpMethodMapper.Map(followUpMethod, searchCriteria.CultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, followUpLookUpsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPut]
        public HttpResponseMessage PutFollowUpMethod(FollowUpLookUpEditDTO followUpMethodEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpMethodBL followUpMethodBL = IoC.Resolve<IFollowUpMethodBL>();
                        FollowUpMethod followUpMethod = FollowUpMethodMapper.Map(followUpMethodEditDTO);

                        followUpMethodBL.UpdateFollowUpMethod(followUpMethod);

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
        public HttpResponseMessage GetFollowUpMethodById(int followUpMethodId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<FollowUpLookUpEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpMethodBL followUpMethodBL = IoC.Resolve<IFollowUpMethodBL>();
                    FollowUpLookUpEditDTO followUpMethodEditDTO = FollowUpMethodMapper.Map(followUpMethodBL.GetFollowUpMethodId(followUpMethodId), cultureName);

                    getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, followUpMethodEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFollowUpMethod(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> linkTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> followUpMethodIds = ids.Split(',').Select(int.Parse).ToList();
                    IFollowUpMethodBL followUpMethodBL = IoC.Resolve<IFollowUpMethodBL>();

                    followUpMethodBL.DeleteFollowUpMethod(followUpMethodIds, out linkTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, linkTypesCannotBeDeleted.ToList());
                    if (linkTypesCannotBeDeleted.Count >= 1)
                    {
                        throw new BusinessException(Common.StatusCode.PermissionLinkDeleteLink);
                    }
                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }
        #endregion  FollowUpMethod 
        #region FollowUpSource
        public HttpResponseMessage PostFollowUpSource(FollowUpLookUpAddDTO followUpSourceAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpSourceBL followUpSourceBL = IoC.Resolve<IFollowUpSourceBL>();
                        FollowUpSource followUpLookUps = FollowUpSourceMapper.Map(followUpSourceAddDTO);
                        int FollowUpLookUpId = followUpSourceBL.AddFollowUpSource(followUpLookUps);

                        postResult = PostResult.Create(statusCode, FollowUpLookUpId);

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


        public HttpResponseMessage GetFollowUpSource([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpSourceBL followUpSourceBL = IoC.Resolve<IFollowUpSourceBL>();
                    List<FollowUpSource> followUpSource = followUpSourceBL.GetFollowUpSources(searchCriteria, out rowsCount).ToList();
                    List<FollowUpLookUpDTO> followUpLookUpsDTOs = FollowUpSourceMapper.Map(followUpSource, searchCriteria.CultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, followUpLookUpsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPut]
        public HttpResponseMessage PutFollowUpSource(FollowUpLookUpEditDTO followUpSourceEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpSourceBL followUpSourceBL = IoC.Resolve<IFollowUpSourceBL>();
                        FollowUpSource followUpSource = FollowUpSourceMapper.Map(followUpSourceEditDTO);

                        followUpSourceBL.UpdateFollowUpSource(followUpSource);

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
        public HttpResponseMessage GetFollowUpSourceById(int followUpSourceId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<FollowUpLookUpEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpSourceBL followUpSourceBL = IoC.Resolve<IFollowUpSourceBL>();
                    FollowUpLookUpEditDTO followUpSourceEditDTO = FollowUpSourceMapper.Map(followUpSourceBL.GetFollowUpSourceId(followUpSourceId), cultureName);

                    getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, followUpSourceEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFollowUpSource(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> linkTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> followUpSourceIds = ids.Split(',').Select(int.Parse).ToList();
                    IFollowUpSourceBL followUpSourceBL = IoC.Resolve<IFollowUpSourceBL>();

                    followUpSourceBL.DeleteFollowUpSource(followUpSourceIds, out linkTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, linkTypesCannotBeDeleted.ToList());
                    if (linkTypesCannotBeDeleted.Count >= 1)
                    {
                        throw new BusinessException(Common.StatusCode.PermissionLinkDeleteLink);
                    }
                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }
        #endregion  FollowUpSource  
        #region FollowUpProccess  
        public HttpResponseMessage PostFollowUpProccess(FollowUpLookUpAddDTO followUpProccessAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpProccessBL followUpProccessBL = IoC.Resolve<IFollowUpProccessBL>();
                        FollowUpProccess followUpLookUps = FollowUpProccessMapper.Map(followUpProccessAddDTO);
                        int FollowUpLookUpId = followUpProccessBL.AddFollowUpProccess(followUpLookUps);

                        postResult = PostResult.Create(statusCode, FollowUpLookUpId);

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


        public HttpResponseMessage GetFollowUpProccess([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpProccessBL followUpProccessBL = IoC.Resolve<IFollowUpProccessBL>();
                    List<FollowUpProccess> followUpProccess = followUpProccessBL.GetFollowUpProccess(searchCriteria, out rowsCount).ToList();
                    List<FollowUpLookUpDTO> followUpLookUpsDTOs = FollowUpProccessMapper.Map(followUpProccess, searchCriteria.CultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, followUpLookUpsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPut]
        public HttpResponseMessage PutFollowUpProccess(FollowUpLookUpEditDTO followUpProccessEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFollowUpProccessBL followUpProccessBL = IoC.Resolve<IFollowUpProccessBL>();
                        FollowUpProccess followUpProccess = FollowUpProccessMapper.Map(followUpProccessEditDTO);

                        followUpProccessBL.UpdateFollowUpProccess(followUpProccess);

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
        public HttpResponseMessage GetFollowUpProccessById(int followUpProccessId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<FollowUpLookUpEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpProccessBL followUpProccessBL = IoC.Resolve<IFollowUpProccessBL>();
                    FollowUpLookUpEditDTO followUpProccessEditDTO = FollowUpProccessMapper.Map(followUpProccessBL.GetFollowUpProccessId(followUpProccessId), cultureName);

                    getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, followUpProccessEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<FollowUpLookUpEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteFollowUpProccess(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> linkTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> followUpProccessIds = ids.Split(',').Select(int.Parse).ToList();
                    IFollowUpProccessBL followUpProccessBL = IoC.Resolve<IFollowUpProccessBL>();

                    followUpProccessBL.DeleteFollowUpProccess(followUpProccessIds, out linkTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, linkTypesCannotBeDeleted.ToList());
                    if (linkTypesCannotBeDeleted.Count >= 1)
                    {
                        throw new BusinessException(Common.StatusCode.PermissionLinkDeleteLink);
                    }
                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        #endregion  FollowUpProccess  



        #region Link Management

        [HttpPost]
        public HttpResponseMessage PostLink(LinkAddDTO linkAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILinkBL linkBL = IoC.Resolve<ILinkBL>();
                        Link link = LinkMapper.Map(linkAddDTO);
                        int linkId = linkBL.AddLink(link);

                        postResult = PostResult.Create(statusCode, linkId);

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
        public HttpResponseMessage PutLink(LinkEditDTO linkEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILinkBL linkBL = IoC.Resolve<ILinkBL>();
                        Link link = LinkMapper.Map(linkEditDTO);

                        linkBL.UpdateLink(link);

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

        [HttpPost]
        public HttpResponseMessage DeleteLinks(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> linkTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> linkIds = ids.Split(',').Select(int.Parse).ToList();
                    ILinkBL linkBL = IoC.Resolve<ILinkBL>();

                    linkBL.DeleteLinks(linkIds, out linkTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, linkTypesCannotBeDeleted.ToList());
                    if (linkTypesCannotBeDeleted.Count >= 1)
                    {
                        throw new BusinessException(Common.StatusCode.PermissionLinkDeleteLink);
                    }
                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLinkById(int linkId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<LinkEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILinkBL linkBL = IoC.Resolve<ILinkBL>();
                    LinkEditDTO linkEditDTO = LinkMapper.Map(linkBL.GetLinkById(linkId), cultureName);

                    getResult = GetResult<LinkEditDTO>.Create(statusCode, linkEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<LinkEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<LinkEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLinks([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LinkDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILinkBL linkBL = IoC.Resolve<ILinkBL>();
                    List<Link> links = linkBL.GetLinks(searchCriteria, out rowsCount).ToList();
                    List<LinkDTO> linkDTOs = LinkMapper.Map(links, searchCriteria.CultureName);

                    getResult = GetResult<List<LinkDTO>>.Create(statusCode, linkDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LinkDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LinkDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  Link Management

        #region Form Management

        [HttpPost]
        public HttpResponseMessage PostForm(FormAddDTO formAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int fromId = 0;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IFormBL formBL = IoC.Resolve<IFormBL>();
                    Form form = FormMapper.Map(formAddDTO);
                    formBL.AddForm(form);
                    postResult = PostResult.Create(statusCode, fromId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutForm(FormEditDTO formEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFormBL formBL = IoC.Resolve<IFormBL>();
                        Form form = FormMapper.Map(formEditDTO);
                        formBL.UpdateForm(form);
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

        [HttpDelete]
        public HttpResponseMessage DeleteForms(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> formIds = ids.Split(',').Select(int.Parse).ToList();
                    IFormBL formBL = IoC.Resolve<IFormBL>();

                    formBL.DeleteForms(formIds);

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

        [HttpDelete]
        public HttpResponseMessage DeleteCounterDetail(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICounterBL counterBL = IoC.Resolve<ICounterBL>();

                    counterBL.DeleteCounterDetailById(id);

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
        public HttpResponseMessage GetFormById(int formId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<FormEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFormBL formBL = IoC.Resolve<IFormBL>();
                    FormEditDTO formEditDTO = FormMapper.Map(formBL.GetFormById(formId), cultureName);

                    getResult = GetResult<FormEditDTO>.Create(statusCode, formEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<FormEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<FormEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetForms([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FormDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IFormBL formBL = IoC.Resolve<IFormBL>();
                    IList<Form> forms = formBL.GetForms(searchCriteria, out rowsCount);
                    List<FormDTO> formsDTO = FormMapper.Map(forms, searchCriteria.CultureName);

                    getResult = GetResult<List<FormDTO>>.Create(statusCode, formsDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FormDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FormDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        #endregion  Form Management

        #region LetterType Management

        [HttpPost]
        public HttpResponseMessage PostLetterType(LetterTypeAddDTO letterTypeAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                        LetterType letterType = LetterTypeMapper.Map(letterTypeAddDTO);

                        int letterTypeId = letterTypeBL.AddLetterType(letterType);

                        postResult = PostResult.Create(statusCode, letterTypeId);

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
        public HttpResponseMessage PutLetterType(LetterTypeEditDTO letterTypeEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                        LetterType letterType = LetterTypeMapper.Map(letterTypeEditDTO);

                        letterTypeBL.UpdateLetterType(letterType);

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

        [HttpPost]
        public HttpResponseMessage DeleteLetterTypes(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> letterTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> letterTypeIds = ids.Split(',').Select(int.Parse).ToList();
                    ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();

                    letterTypeBL.DeleteLetterTypes(letterTypeIds, out letterTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, letterTypesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLetterTypeById(int letterTypeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<LetterTypeEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                    LetterTypeEditDTO letterTypeEditDTO = LetterTypeMapper.Map(letterTypeBL.GetLetterTypeById(letterTypeId), cultureName);

                    getResult = GetResult<LetterTypeEditDTO>.Create(statusCode, letterTypeEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<LetterTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<LetterTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLetterTypes([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LetterTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                    IList<LetterType> letterTypes = letterTypeBL.GetLetterTypes(searchCriteria, out rowsCount).ToList();
                    List<LetterTypeDTO> letterTypesDTO = LetterTypeMapper.Map(letterTypes, searchCriteria.CultureName);

                    getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, letterTypesDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  LetterType Management

        #region AttachmentType Management

        [HttpPost]
        public HttpResponseMessage PostAttachmentType(AttachmentTypeAddDTO attachmentTypeAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                        AttachmentType attachmentType = AttachmentTypeMapper.Map(attachmentTypeAddDTO);

                        int attachmentTypeId = attachmentTypeBL.AddAttachmentType(attachmentType);

                        postResult = PostResult.Create(statusCode, attachmentTypeId);

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
        public HttpResponseMessage PutAttachmentType(AttachmentTypeEditDTO attachmentTypeEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                        AttachmentType attachmentType = AttachmentTypeMapper.Map(attachmentTypeEditDTO);

                        attachmentTypeBL.UpdateAttachmentType(attachmentType);

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

        [HttpPost]
        public HttpResponseMessage DeleteAttachmentTypes(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            //DeleteResult deleteResult = null;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> attachmentTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> attachmentTypeIds = ids.Split(',').Select(int.Parse).ToList();

                    IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();

                    attachmentTypeBL.DeleteAttachmentTypes(attachmentTypeIds, out attachmentTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, attachmentTypesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, attachmentTypesCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, attachmentTypesCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentTypeById(int attachmentTypeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<AttachmentTypeEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                    AttachmentTypeEditDTO attachmentTypeEditDTO = AttachmentTypeMapper.Map(attachmentTypeBL.GetAttachmentTypeById(attachmentTypeId), cultureName);

                    getResult = GetResult<AttachmentTypeEditDTO>.Create(statusCode, attachmentTypeEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<AttachmentTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<AttachmentTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentTypes([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AttachmentTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                    IList<AttachmentType> attachmentTypes = attachmentTypeBL.GetAttachmentTypes(searchCriteria, out rowsCount).ToList();
                    List<AttachmentTypeDTO> attachmentTypesDTOs = AttachmentTypeMapper.Map(attachmentTypes, searchCriteria.CultureName);

                    getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, attachmentTypesDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentExtentions([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AttachmentExtensionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                    IList<AttachmentExtension> attachmentExtentions = attachmentTypeBL.GetAttachmentExtentions(searchCriteria, out int rowsCount).ToList();
                    List<AttachmentExtensionDTO> attachmentExtensionDTOs = AttachmentExtensionMapper.Map(attachmentExtentions.ToList());

                    getResult = GetResult<List<AttachmentExtensionDTO>>.Create(statusCode, attachmentExtensionDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AttachmentExtensionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AttachmentExtensionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  AttachmentType Management
        #region Confidentiality Acknowledgments
        [HttpPost]
        public HttpResponseMessage PostConfidentialityAcknowledgments(ConfidentialityAcknowledgmentsAddDTO ConfidentialityAcknowledgmentsAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IConfidentialityAcknowledgmentsBL ConfidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();
                        ConfidentialityAcknowledgment ConfidentialityAcknowledgments = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsAddDTO);

                        int ConfidentialityAcknowledgmentsId = ConfidentialityAcknowledgmentsBL.AddConfidentialityAcknowledgments(ConfidentialityAcknowledgments);

                        postResult = PostResult.Create(statusCode, ConfidentialityAcknowledgmentsId);

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
        public HttpResponseMessage PutConfidentialityAcknowledgments(ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IConfidentialityAcknowledgmentsBL ConfidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();
                        ConfidentialityAcknowledgment ConfidentialityAcknowledgments = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsEditDTO);

                        ConfidentialityAcknowledgmentsBL.UpdateConfidentialityAcknowledgments(ConfidentialityAcknowledgments);

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

        [HttpPost]
        public HttpResponseMessage DeleteConfidentialityAcknowledgments(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            //DeleteResult deleteResult = null;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> ConfidentialityAcknowledgmentssCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> ConfidentialityAcknowledgmentsIds = ids.Split(',').Select(int.Parse).ToList();

                    IConfidentialityAcknowledgmentsBL ConfidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();

                    ConfidentialityAcknowledgmentsBL.DeleteConfidentialityAcknowledgments(ConfidentialityAcknowledgmentsIds, out ConfidentialityAcknowledgmentssCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, ConfidentialityAcknowledgmentssCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, ConfidentialityAcknowledgmentssCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, ConfidentialityAcknowledgmentssCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetConfidentialityAcknowledgmentsById(int ConfidentialityAcknowledgmentsId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ConfidentialityAcknowledgmentsEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IConfidentialityAcknowledgmentsBL ConfidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();
                    ConfidentialityAcknowledgmentsEditDTO ConfidentialityAcknowledgmentsEditDTO = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsBL.GetConfidentialityAcknowledgmentsById(ConfidentialityAcknowledgmentsId), cultureName);

                    getResult = GetResult<ConfidentialityAcknowledgmentsEditDTO>.Create(statusCode, ConfidentialityAcknowledgmentsEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ConfidentialityAcknowledgmentsEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ConfidentialityAcknowledgmentsEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetConfidentialityAcknowledgments([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConfidentialityAcknowledgmentsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IConfidentialityAcknowledgmentsBL ConfidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();
                    IList<ConfidentialityAcknowledgment> ConfidentialityAcknowledgments = ConfidentialityAcknowledgmentsBL.GetConfidentialityAcknowledgments(searchCriteria, out rowsCount).ToList();
                    List<ConfidentialityAcknowledgmentsDTO> ConfidentialityAcknowledgmentssDTOs = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgments, searchCriteria.CultureName);

                    getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, ConfidentialityAcknowledgmentssDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  Confidentiality Acknowledgments
        #region TransactionType Management

        [HttpPost]
        public HttpResponseMessage PostTransactionType(TransactionTypeAddDTO transactionTypeAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                        Domain.TransactionType transactionType = TransactionTypeMapper.Map(transactionTypeAddDTO);

                        int transactionTypeId = transactionTypeBL.AddTransactionSourceType(transactionType);

                        postResult = PostResult.Create(statusCode, transactionTypeId);

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
        public HttpResponseMessage PutTransactionType(TransactionTypeEditDTO transactionTypeEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                        Domain.TransactionType transactionType = TransactionTypeMapper.Map(transactionTypeEditDTO);

                        transactionTypeBL.UpdateTransactionSourceType(transactionType);

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

        [HttpPost]
        public HttpResponseMessage DeleteTransactionTypes(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> transactionTypesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> transactionTypeIds = ids.Split(',').Select(int.Parse).ToList();
                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();

                    transactionTypeBL.DeleteTransactionSourceTypes(transactionTypeIds, out transactionTypesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, transactionTypesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionTypeById(int transactionTypeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionTypeEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                    TransactionTypeEditDTO transactionTypeEditDTO =
                        TransactionTypeMapper.Map(transactionTypeBL.GetTransactionSourceTypeById(transactionTypeId), cultureName);

                    getResult = GetResult<TransactionTypeEditDTO>.Create(statusCode, transactionTypeEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionTypeEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionTypes([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    int rowsCount = 0;

                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                    IList<Domain.TransactionType> transactionTypes = transactionTypeBL.GetTransactionSourceTypes(searchCriteria, out rowsCount);
                    List<TransactionTypeDTO> transactionTypesDTOs = TransactionTypeMapper.Map(transactionTypes, searchCriteria.CultureName);

                    getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, transactionTypesDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  TransactionLink Management

        #region Permission Managment


        [HttpPost]
        public HttpResponseMessage PostPermission(PermissionDTO permissionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                        Permission permission = PermissionMapper.Map(permissionDTO);

                        permissionBL.AddPermission(permission);

                        postResult = PostResult.Create(statusCode, permission.Id);

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
        public HttpResponseMessage PutPermission(PermissionEditDTO permissionEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    Permission permission = PermissionMapper.Map(permissionEditDTO);

                    permissionBL.UpdatePermission(permission);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, putResult);

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
        public HttpResponseMessage DeletePermission(int permissionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    permissionBL.DeletePermission(permissionId);

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
        public HttpResponseMessage GetPermissionsByGroupId(int groupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PermissionEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    Permission permission = permissionBL.GetPermissionById(groupId);

                    PermissionEditDTO permissionDTO = PermissionMapper.MapEdit(permission);

                    getResult = GetResult<PermissionEditDTO>.Create(statusCode, permissionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PermissionEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PermissionEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPermissionByID(int permissionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PermissionEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    Permission permission = permissionBL.GetPermissionById(permissionId);

                    PermissionEditDTO permissionDTO = PermissionMapper.MapEdit(permission);

                    getResult = GetResult<PermissionEditDTO>.Create(statusCode, permissionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PermissionEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PermissionEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdatePermissionsName(List<PermissionGroupDTO> permissionGroupDTOs, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        var transactionContextScopeFactory = new TransactionContextScopeFactory();

                        IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                        IList<Permission> permissions = PermissionMapper.MapPermissions(permissionGroupDTOs, cultureName);

                        permissionBL.UpdatePermissions(permissions);

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
        public HttpResponseMessage GetAllPermissions(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Permission> permissions = permissionBL.GetPermissions(cultureName);
                    IList<PermissionDTO> permissionDTOs = PermissionMapper.Map(permissions);

                    getResult = GetResult<List<PermissionDTO>>.Create(statusCode, permissionDTOs.ToList(), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPermissionsGroups([FromUri] List<PermissionGroupName> permissionGroupNames, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> permissionGroups = permissionBL.GetPermissionsGroups(permissionGroupNames, cultureName);
                    IList<PermissionGroupDTO> permissionGroupDTOs = PermissionMapper.Map(permissionGroups);

                    getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, permissionGroupDTOs.ToList(), null);

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
        public HttpResponseMessage GetPermissionsGroups([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> permissionGroups = permissionBL.GetPermissionsGroups(searchCriteria, out rowsCount).ToList();
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
        public HttpResponseMessage GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups = true)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> permissionGroups = permissionBL.GetAllPermissionsGroups(cultureName, includeUserDefinedGroups).ToList();
                    List<PermissionGroupDTO> permissionsGroupDTOs = PermissionMapper.Map(permissionGroups);

                    getResult = GetResult<List<PermissionGroupDTO>>.Create(statusCode, permissionsGroupDTOs, null);

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
        public HttpResponseMessage GetAllUserDefinedGroups([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<GroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<Group> permissionGroups = permissionBL.GetAllUserDefinedGroups(searchCriteria, out rowsCount).ToList();
                    List<GroupDTO> permissionsGroupDTOs = PermissionMapper.MapGroups(permissionGroups);

                    getResult = GetResult<List<GroupDTO>>.Create(statusCode, permissionsGroupDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<GroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<GroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGroupPermissionsByGroupId(int groupId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PermissionGroupDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    Group group = permissionBL.GetPermissionsByGroupId(groupId, cultureName);
                    PermissionGroupDTO permissionGroupDTO = PermissionMapper.MapPermissionGroup(group);
                    getResult = GetResult<PermissionGroupDTO>.Create(statusCode, permissionGroupDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PermissionGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PermissionGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGroupByID(int groupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EditGroupDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    Group group = permissionBL.GetGroupById(groupId);
                    EditGroupDTO EditGroupDTO = PermissionMapper.Map(group);

                    getResult = GetResult<EditGroupDTO>.Create(statusCode, EditGroupDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<EditGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<EditGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostGroup(AddGroupDTO addGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    Group group = PermissionMapper.Map(addGroupDTO);

                    int actionId = permissionBL.AddGroup(group);

                    postResult = PostResult.Create(statusCode, actionId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutGroup(EditGroupDTO EditGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                        Group group = PermissionMapper.Map(EditGroupDTO);

                        permissionBL.UpdateGroup(group);

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

        [HttpDelete]
        public HttpResponseMessage DeleteGroups(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> groupsIds = ids.Split(',').Select(int.Parse).ToList();
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    permissionBL.DeleteGroups(groupsIds);

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

        #endregion Permission Managment

        #region UserCategories Managment

        [HttpPost]
        public HttpResponseMessage PostUserCategory(AddUserCategoryDTO userCategoryAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserCategory userCategory = UserCategoryMapper.Map(userCategoryAddDTO);

                    int priorityId = userManagementBL.AddUserCategory(userCategory);

                    postResult = PostResult.Create(statusCode, priorityId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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
        public HttpResponseMessage PutUserCategory(EditUserCategoryDTO editUserCategoryDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserCategory userCategory = UserCategoryMapper.Map(editUserCategoryDTO);

                    userManagementBL.UpdateUserCategory(userCategory);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

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
        public HttpResponseMessage DeleteUserCategories(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> userCategoriesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> userCategoryIds = ids.Split(',').Select(int.Parse).ToList();
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    userManagementBL.DeleteUserCategories(userCategoryIds, out userCategoriesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, userCategoriesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserCategoryById(int userCategoryId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EditUserCategoryDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    EditUserCategoryDTO userCategoryEditDTO = UserCategoryMapper.Map(userManagementBL.GetUserCategoryById(userCategoryId));

                    getResult = GetResult<EditUserCategoryDTO>.Create(statusCode, userCategoryEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<EditUserCategoryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<EditUserCategoryDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserCategories([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserCategoryDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserCategory> userCategories = userManagementBL.GetUserCategories(searchCriteria, out rowsCount).ToList();
                    List<UserCategoryDTO> userCategoryDTOs = UserCategoryMapper.Map(userCategories, searchCriteria.CultureName);

                    getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, userCategoryDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllUsersCategoriesTrays(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserCategoryTrayDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserCategory> userCategories = userManagementBL.GetUserCategories(cultureName).ToList();
                    List<UserCategoryTrayDTO> userCategoryTrayDTOs = UserCategoryTrayMapper.Map(userCategories, cultureName);

                    getResult = GetResult<List<UserCategoryTrayDTO>>.Create(statusCode, userCategoryTrayDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserCategoryTrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserCategoryTrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllUsersCategories(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserCategoryDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<UserCategory> userCategories = userManagementBL.GetUserCategories(cultureName).ToList();
                    List<UserCategoryDTO> userCategoryTrayDTOs = UserCategoryTrayMapper.MapUserCategories(userCategories);

                    getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, userCategoryTrayDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserCategoryDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserCategoryTrays(int userCategoryId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TrayDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    IList<Tray> userCategories = userManagementBL.GetUserCategoryTrays(userCategoryId);
                    List<TrayDTO> trayDTOs = TrayMapper.Map(userCategories);

                    getResult = GetResult<List<TrayDTO>>.Create(statusCode, trayDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage PutUsersCategoriesTrays(List<UserCategoryTrayDTO> usersCategoriesTraysDTO)
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
                        IList<UserCategoryTray> userCategoriesTrays = UserCategoryTrayMapper.Map(usersCategoriesTraysDTO);

                        userManagementBL.UpdateUsersCategoriesTrays(userCategoriesTrays);

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

        #endregion  UserCategories Managment

        #region Actions Managment

        [HttpPost]
        public HttpResponseMessage PostAction(AddActionDTO addActionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IActionBL actionBL = IoC.Resolve<IActionBL>();
                    Domain.Action process = ActionMapper.Map(addActionDTO);

                    int actionId = actionBL.AddAction(process);

                    postResult = PostResult.Create(statusCode, actionId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);

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
        public HttpResponseMessage PutAction(EditActionDTO actionEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IActionBL processBL = IoC.Resolve<IActionBL>();
                    Domain.Action process = ActionMapper.Map(actionEditDTO);

                    processBL.UpdateAction(process);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

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
        public HttpResponseMessage DeleteActions(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> actionsCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> actionsIds = ids.Split(',').Select(int.Parse).ToList();
                    IActionBL processBL = IoC.Resolve<IActionBL>();

                    processBL.DeleteAction(actionsIds, out actionsCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, actionsCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, actionsCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, actionsCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetActions([FromUri] SearchCriteria searchCriteria, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ActionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IActionBL processBL = IoC.Resolve<IActionBL>();
                    IList<Domain.Action> actions = processBL.GetAction(searchCriteria, out rowsCount, cultureName).ToList();
                    List<ActionDTO> actionsDTO = ActionMapper.Map(actions);

                    getResult = GetResult<List<ActionDTO>>.Create(statusCode, actionsDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetActionById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EditActionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IActionBL processBL = IoC.Resolve<IActionBL>();
                    EditActionDTO actionEditDTO = ActionMapper.Map(processBL.GetActionById(id));

                    getResult = GetResult<EditActionDTO>.Create(statusCode, actionEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<EditActionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<EditActionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ChangeEntitiesNameBeforeMove(ChangeEntityNameDTO changeEntityNameDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ChangeEntityName changeEntityName = new ChangeEntityName
                    {
                        EntityFromId = changeEntityNameDTO.EntityFromId,
                        EntityToId = changeEntityNameDTO.EntityToId,
                        EntityFromLocalizations = LocalizationIdentifierMapper.Maps(changeEntityNameDTO.EntityFromLocalizations),
                        EntityToLocalizations = LocalizationIdentifierMapper.Maps(changeEntityNameDTO.EntityToLocalizations)
                    };

                    IActionBL actionBL = IoC.Resolve<IActionBL>();
                    actionBL.ChangeEntitiesNameBeforeMove(changeEntityName);
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
        public HttpResponseMessage PostMergeDepartments(MergeDepartmentsDTO mergeDepartmentsDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    MergeDepartment mergeDepartment = new MergeDepartment()
                    {
                        Id = mergeDepartmentsDTO.Id,
                        BaseEntityId = mergeDepartmentsDTO.BaseEntityId,
                        MergedEntityId = mergeDepartmentsDTO.MergedEntityId,
                        ManagerId = mergeDepartmentsDTO.ManagerId,
                        NewEntityNames = LocalizationIdentifierMapper.Maps(mergeDepartmentsDTO.NewEntityNames)
                    };

                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    int conflictedEntityId = orgUnitBL.MergeDepartments(mergeDepartment);
                    if (conflictedEntityId != -1)
                    {
                        statusCode = Common.StatusCode.OrgUnitsToBeMergedHaveSameName;
                    }
                    postResult = PostResult.Create(statusCode, conflictedEntityId);
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

        #endregion

        #region OrgUnit Structure
        [HttpPost]
        public HttpResponseMessage PostOrgUnitStructure(string cultureName, OrgUnitStructureDesignDTO orgUnitStructureDesignDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<List<int>> postResult = null;
            //DatabaseNulls
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> orgUnitUsedInTransactions = new List<int>();

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();

                    IList<OrgUnit> orgUnits = OrgUnitStructureMapper.Map(orgUnitStructureDesignDTO.OrgUnits);

                    orgUnitStructureBL.BuildOrgUnitStructure(orgUnits, orgUnitStructureDesignDTO.Settings, out orgUnitUsedInTransactions);

                    postResult = PostObjectResult<List<int>>.Create(statusCode, orgUnitUsedInTransactions.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostObjectResult<List<int>>.Create(statusCode, null);

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
        [HttpGet]
        public HttpResponseMessage GetOrgUnitStructure(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitStructureDesignDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();
                    ISettingBL settingBL = new SettingBL();
                    IList<OrgUnit> orgUnits = orgUnitStructureBL.GetOrgUnitStructure();

                    List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.OrgUnitStructureKey);
                    Setting setting = settings.Find(a => a.Key == SettingsKeys.OrgUnitStructureKey);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    OrgUnitStructureDesignDTO orgUnitStructureDesignDTO = new OrgUnitStructureDesignDTO()
                    {
                        OrgUnits = OrgUnitDTO,
                        Settings = (setting != null) ? setting.Value : string.Empty
                    };

                    getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, orgUnitStructureDesignDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnits(string cultureName, int? orgUnitId = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(cultureName, orgUnitId);
                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

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
        [HttpGet]
        public HttpResponseMessage GetOrgUnitsGeneralCounter()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgStructureInfoDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();
                    OrgUnit orgUnits = orgUnitStructureBL.GetOrgUnitsGeneralCounter(Language);

                    OrgStructureInfoDTO OrgUnitDTO = OrgUnitStructureMapper.MapOrgUnit(orgUnits, Language);

                    getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitsUsedInTransaction(string orgUnitIds)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<int>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<int> orgUnitIdList = new List<int>();

                    orgUnitIds.Split(',').ToList().ForEach(id =>
                    {

                        orgUnitIdList.Add(Convert.ToInt32(id));
                    });

                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<int> orgUnitsHasTransactions = orgUnitBL.GetOrgUnitsTransactions(orgUnitIdList);

                    getResult = GetResult<List<int>>.Create(statusCode, orgUnitsHasTransactions.ToList(), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<int>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<int>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage CheckOrgUnitUsedInTransaction(int orgUnitId, List<int> transactionCategoryIds)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    List<int> transactionCategories = new List<int>();
                    foreach (var item in transactionCategoryIds)
                    {
                        switch ((TransactionCategories)item)
                        {
                            case TransactionCategories.Outbound:
                                transactionCategories.Add(TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                                break;
                            case TransactionCategories.Inbound:
                                transactionCategories.Add(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                                break;
                            case TransactionCategories.InternalOutbound:
                                transactionCategories.Add(TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                                break;
                            case TransactionCategories.DraftOutbound:
                                transactionCategories.Add(TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                                break;
                        }
                    }

                    bool result = orgUnitBL.CheckOrgUnitUsedInTransaction(orgUnitId, transactionCategories);

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
        public HttpResponseMessage GetOrgUnitById(string cultureName, int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgStructureInfoDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();
                    OrgUnit orgUnits = orgUnitStructureBL.GetOrgUnitById(id);

                    OrgStructureInfoDTO OrgUnitDTO = OrgUnitStructureMapper.MapOrgUnit(orgUnits, cultureName);

                    getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgStructureInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetAllOrgUnits(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitsLight(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsLight(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitsNew(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsNew(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetAllUnitByLineage(string lineage, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetAllUnitByLineage(lineage, cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetOrgUnitsWithCounter(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsWithCounter(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitsWithUser(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsWithUser(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitsWithLinks(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgStructureInfoDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsWithLinks(cultureName);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgStructureInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitWithCounter(OrgUnitDTO orgUnitDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitMapper.Map(orgUnitDTO, cultureName);

                    processBL.UpdateOrgUnitWithCounter(process, cultureName);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitToJoinGeneralCounter(int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();

                    processBL.UpdateOrgUnitToJoinGeneralCounter(orgUnitId);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitInfo(OrgStructureInfoDTO orgStructureInfoDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitStructureMapper.MapOrgUnit(orgStructureInfoDTO);
                    int Id;
                    Id = orgUnitBL.UpdateOrgUnitInfo(process);

                    putResult = PutResult.Create(statusCode, Id);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitWithUsers(OrgUnitDTO orgUnitDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitMapper.MapWithUsers(orgUnitDTO);

                    processBL.UpdateOrgUnitWithUsers(process, cultureName);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage MoveAllUserTransactions(int UserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<TransactionAssignmentBL>();

                    transactionAssignmentBL.MoveAllUserTransactions(UserId);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitWithLink(OrgUnitDTO orgUnitDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitMapper.MapWithLinks(orgUnitDTO);

                    processBL.UpdateOrgUnitWithLink(process, cultureName);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage UpdateOrgUnitWithBarcodeDesign(OrgUnitDTO orgUnitDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitMapper.MapWithBarcode(orgUnitDTO);

                    processBL.UpdateOrgUnitWithBarcodeDesign(process, cultureName);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage DeleteOrgUnit(int orgUnitKey)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            HttpStatusCode httpStatusCode = HttpStatusCode.ExpectationFailed;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL processBL = IoC.Resolve<IOrgUnitBL>();
                    var success = processBL.DeleteOrgUnit(orgUnitKey);
                    putResult = PutResult.Create(statusCode);
                    transactionContextScope.Commit();
                    if (success)
                    {
                        httpStatusCode = HttpStatusCode.OK;
                    }
                    return Request.CreateResponse(httpStatusCode, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                putResult = PutResult.Create(statusCode);
                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitStructureRoot(string cultureName, int? parentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitStructureDesignDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();
                    ISettingBL settingBL = new SettingBL();
                    IList<OrgUnit> orgUnits = orgUnitStructureBL.GetOrgUnitStructureRoot(parentId);

                    List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.OrgUnitStructureKey);
                    Setting setting = settings.Find(a => a.Key == SettingsKeys.OrgUnitStructureKey);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    OrgUnitStructureDesignDTO orgUnitStructureDesignDTO = new OrgUnitStructureDesignDTO()
                    {
                        OrgUnits = OrgUnitDTO,
                        Settings = (setting != null) ? setting.Value : string.Empty
                    };

                    getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, orgUnitStructureDesignDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitChildsStructure(string cultureName, int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitStructureDesignDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IOrgUnitBL orgUnitStructureBL = IoC.Resolve<IOrgUnitBL>();
                    ISettingBL settingBL = new SettingBL();
                    IList<OrgUnit> orgUnits = orgUnitStructureBL.GetOrgUnitStructureRoot(id);

                    List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.OrgUnitStructureKey);
                    Setting setting = settings.Find(a => a.Key == SettingsKeys.OrgUnitStructureKey);
                    List<OrgStructureInfoDTO> OrgUnitDTO = OrgUnitStructureMapper.Map(orgUnits, cultureName);

                    OrgUnitStructureDesignDTO orgUnitStructureDesignDTO = new OrgUnitStructureDesignDTO()
                    {
                        OrgUnits = OrgUnitDTO,
                        Settings = (setting != null) ? setting.Value : string.Empty
                    };

                    getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, orgUnitStructureDesignDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitStructureDesignDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }




        #endregion  OrgUnitStructure

        #region External Parties Managment

        [HttpPost]
        public HttpResponseMessage DeleteExternalPartyManagers(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;

            IList<int> managersCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> managersIds = ids.Split(',').Select(int.Parse).ToList();
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

                    externalPartyBL.DeleteExternalPartyManagers(managersIds, out managersCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, managersCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeletePartites(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;

            IList<int> partiesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> partiesIds = ids.Split(',').Select(int.Parse).ToList();
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

                    externalPartyBL.DeleteParties(partiesIds, out partiesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, partiesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, partiesCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }
        #endregion

        #region Trays Managment

        [HttpPut]
        public HttpResponseMessage PutTray(EditTrayDTO editTrayDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITrayBL trayBL = TrayBaseBL.Create((TrayType)editTrayDTO.Id);
                    Tray tray = TrayMapper.Map(editTrayDTO);

                    trayBL.UpdateTray(tray);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

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

        [HttpPut]
        public HttpResponseMessage PutTrays(List<TrayDTO> traysDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IList<Tray> trays = TrayMapper.Map(traysDTO);

                        TrayBaseBL.UpdateTrays(trays);

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
        public HttpResponseMessage GetAllTrays(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TrayDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IList<Tray> trays = TrayBaseBL.GetAllTrays(cultureName);
                    List<TrayDTO> trayDTOs = TrayMapper.Map(trays);

                    getResult = GetResult<List<TrayDTO>>.Create(statusCode, trayDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTrayById(int trayId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<EditTrayDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Tray tray = TrayBaseBL.GetTrayById(trayId);
                    EditTrayDTO trayDTO = TrayMapper.Map(tray);

                    getResult = GetResult<EditTrayDTO>.Create(statusCode, trayDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<EditTrayDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<EditTrayDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTrays([FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TrayDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IList<Tray> trays = TrayBaseBL.GetTrays(searchCriteria, out rowsCount);
                    List<TrayDTO> traysDTO = TrayMapper.Map(trays);

                    getResult = GetResult<List<TrayDTO>>.Create(statusCode, traysDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TrayDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion Trays Managment

        #region Barcode Designer

        [HttpPost]
        public HttpResponseMessage PostDesign(BarcodeDesignerDTO designDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    int barcodeDesignId = -1;

                    if (ModelState.IsValid)
                    {
                        IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();

                        BarcodeDesign barcodeDesign = BarcodeMapper.Map(designDTO);

                        barcodeDesignId = barcodeBL.AddOrUpdateBarcodeDesign(barcodeDesign);

                        postResult = PostResult.Create(statusCode, barcodeDesignId);

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
        public HttpResponseMessage GetBarcodeDesign(bool isGeneral, int typeId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<BarcodeDesignerDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();
                    BarcodeDesign barcodeDesign = barcodeBL.GetBarcodeDesign(isGeneral, typeId);

                    if (barcodeDesign == null)
                    {
                        getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }

                    BarcodeDesignerDTO barcodeDesignerDTO = BarcodeMapper.Map(barcodeDesign);

                    getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, barcodeDesignerDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetBarcodeDesignByOrgUnitId(int orgUnitId, int typeId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<BarcodeDesignerDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    BarcodeDesign barcodeDesign = orgUnitBL.GetBarcodeDesignByOrgUnitId(orgUnitId, typeId);

                    if (barcodeDesign == null)
                    {
                        getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }

                    BarcodeDesignerDTO barcodeDesignerDTO = BarcodeMapper.Map(barcodeDesign);

                    getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, barcodeDesignerDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<BarcodeDesignerDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        #endregion Barcode Designer

        #region Suggested Topics

        [HttpPost]
        public HttpResponseMessage PostSuggestedTopics(List<SuggestedTopicDTO> suggestedTopicDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<List<int>> postResult = null;
            IList<int> suggestedTopicsUsed = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    ISuggestedTopicBL suggestedTopicBL = IoC.Resolve<ISuggestedTopicBL>();

                    List<SuggestedTopic> suggestedTopics = SuggestedTopicMapper.Map(suggestedTopicDTOs);

                    suggestedTopicBL.SaveSuggestedTopics(suggestedTopics, out suggestedTopicsUsed);

                    postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostObjectResult<List<int>>.Create(statusCode, suggestedTopicsUsed.ToList());

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

        [HttpGet]
        public HttpResponseMessage GetSuggestedTopics()
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SuggestedTopicDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISuggestedTopicBL suggestedTopicBL = IoC.Resolve<ISuggestedTopicBL>();

                    IList<SuggestedTopic> suggestedTopics = suggestedTopicBL.GetAllSuggestedTopics();

                    List<SuggestedTopicDTO> suggestedTopicDTOs = SuggestedTopicMapper.Map(suggestedTopics);

                    getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, suggestedTopicDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion Suggested Topics

        #region Subject Classification

        [HttpPost]
        public HttpResponseMessage PostSubjectClassifications(List<SubjectClassificationDTO> subjectClassificationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<List<int>> postResult = null;
            IList<int> subjectClassificationsUsed = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ISubjectClassificationBL subjectClassificationBL = IoC.Resolve<ISubjectClassificationBL>();

                    List<SubjectClassification> subjectClassifications = SubjectClassificationMapper.Map(subjectClassificationDTO);

                    subjectClassificationBL.SaveSubjectClassifications(subjectClassifications, out subjectClassificationsUsed);

                    postResult = PostObjectResult<List<int>>.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostObjectResult<List<int>>.Create(statusCode, subjectClassificationsUsed.ToList());

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

        [HttpGet]
        public HttpResponseMessage GetSubjectClassifications()
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SubjectClassificationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISubjectClassificationBL subjectClassificationBL = IoC.Resolve<ISubjectClassificationBL>();

                    IList<SubjectClassification> classifications = subjectClassificationBL.GetAllSubjectClassifications();

                    List<SubjectClassificationDTO> subjectClassificationDTOs = SubjectClassificationMapper.Map(classifications);

                    getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, subjectClassificationDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion 

        #region PriorityException
        [HttpPost]
        public HttpResponseMessage PostPriorityException(PriorityExceptionDTO priorityExceptionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();
                        PriorityException priorityException = PriorityExceptionMapper.Map(priorityExceptionDTO);

                        int priorityExceptionId = priorityExceptionBL.AddPriorityException(priorityException);

                        postResult = PostResult.Create(statusCode, priorityExceptionId);

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
        public HttpResponseMessage PutPriorityException(PriorityExceptionDTO priorityExceptionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();
                        PriorityException priorityException = PriorityExceptionMapper.Map(priorityExceptionDTO);

                        priorityExceptionBL.UpdatePriorityException(priorityException);

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

        [HttpPost]
        public HttpResponseMessage DeletePriorityException(int priorityExceptionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();

                    priorityExceptionBL.DeletePriorityException(priorityExceptionId);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

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
        public HttpResponseMessage GetPriorityExceptionById(int PriorityExceptionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PriorityExceptionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();
                    PriorityExceptionDTO priorityExceptionDTO = PriorityExceptionMapper.Map(priorityExceptionBL.GetPriorityExceptionById(PriorityExceptionId), cultureName);

                    getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, priorityExceptionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPriorityExceptionByPriorityId(int priorityId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<PriorityExceptionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();
                    PriorityExceptionDTO priorityExceptionDTO = PriorityExceptionMapper.Map(priorityExceptionBL.GetPriorityExceptionByPriorityId(priorityId), cultureName);

                    getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, priorityExceptionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<PriorityExceptionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPriorityExceptions([FromUri] SearchCriteria searchCriteria, int priorityId, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PriorityExceptionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityExceptionBL priorityExceptionBL = IoC.Resolve<IPriorityExceptionBL>();
                    List<PriorityException> priorityExceptions = priorityExceptionBL.GetPriorityExceptions(searchCriteria, priorityId, out rowsCount).ToList();
                    List<PriorityExceptionDTO> priorityExceptionDTOs = PriorityExceptionMapper.Map(priorityExceptions, cultureName);

                    getResult = GetResult<List<PriorityExceptionDTO>>.Create(statusCode, priorityExceptionDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PriorityExceptionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PriorityExceptionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion

        #region Reporters
        [HttpPost]
        public HttpResponseMessage PostReporter(ReporterDTO ReporterDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ICorrespondentBL correspondentBL = IoC.Resolve<ICorrespondentBL>();
                        Reporter reporter = ReportersMapper.Map(ReporterDTO);

                        int correspondentId = correspondentBL.AddReporter(reporter);

                        postResult = PostResult.Create(statusCode, correspondentId);

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
        public HttpResponseMessage PutReporter(ReporterDTO reporterDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ICorrespondentBL correspondentBL = IoC.Resolve<ICorrespondentBL>();
                        Reporter reporter = ReportersMapper.Map(reporterDTO);

                        correspondentBL.UpdateReporter(reporter);

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

        [HttpPost]
        public HttpResponseMessage DeleteReporter(int reporterId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICorrespondentBL correspondentBL = IoC.Resolve<ICorrespondentBL>();

                    correspondentBL.DeleteReporter(reporterId);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

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
        public HttpResponseMessage GetReporterById(int reporterId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ReporterDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICorrespondentBL correspondentBL = IoC.Resolve<ICorrespondentBL>();
                    ReporterDTO reporterDTO = ReportersMapper.Map(correspondentBL.GetReporterById(reporterId), cultureName);

                    getResult = GetResult<ReporterDTO>.Create(statusCode, reporterDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ReporterDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ReporterDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetReporters([FromUri] SearchCriteria searchCriteria, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReporterDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICorrespondentBL correspondentBL = IoC.Resolve<ICorrespondentBL>();
                    List<ReporterDTO> reporterDTOs = ReportersMapper.Map(correspondentBL.GetReporters(searchCriteria, out rowsCount).ToList(), cultureName);

                    getResult = GetResult<List<ReporterDTO>>.Create(statusCode, reporterDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReporterDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReporterDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion

        #region Roles

        [HttpGet]
        public HttpResponseMessage ActivateDeactivateRole(int RoleId, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<GroupDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    GroupDTO groupDTO = PermissionMapper.MapGroup(permissionBL.ActivateDeactivateRole(RoleId, CultureName));
                    getResult = GetResult<GroupDTO>.Create(statusCode, groupDTO, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<GroupDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<GroupDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion

        #region SpecificLevel Management

        [HttpPost]
        public HttpResponseMessage PostSpecificLevel(SpecificLevelAddDTO specificLevelAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();
                        SpecificLevel specificLevel = SpecificLevelMapper.Map(specificLevelAddDTO);

                        int specificLevelId = specificLevelBL.AddSpecificLevel(specificLevel);

                        postResult = PostResult.Create(statusCode, specificLevelId);

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
        public HttpResponseMessage PutSpecificLevel(SpecificLevelEditDTO specificLevelEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();
                        SpecificLevel specificLevel = SpecificLevelMapper.Map(specificLevelEditDTO);

                        specificLevelBL.UpdateSpecificLevel(specificLevel);

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

        [HttpPost]
        public HttpResponseMessage DeleteSpecificLevels(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> specificLevelsCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> specificLevelIds = ids.Split(',').Select(int.Parse).ToList();
                    ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();

                    specificLevelBL.DeleteSpecificLevels(specificLevelIds, out specificLevelsCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, specificLevelsCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSpecificLevelById(int specificLevelId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SpecificLevelEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();
                    SpecificLevelEditDTO specificLevelEditDTO = SpecificLevelMapper.Map(specificLevelBL.GetSpecificLevelById(specificLevelId), cultureName);

                    getResult = GetResult<SpecificLevelEditDTO>.Create(statusCode, specificLevelEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SpecificLevelEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SpecificLevelEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSpecificLevels([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SpecificLevelDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();
                    IList<SpecificLevel> specificLevels = specificLevelBL.GetSpecificLevels(searchCriteria, out rowsCount).ToList();
                    List<SpecificLevelDTO> specificLevelDTOs = SpecificLevelMapper.Map(specificLevels, searchCriteria.CultureName);

                    getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, specificLevelDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  SpecificLevel Management

        #region ReleaseNotes Managment

        [HttpPost]
        public HttpResponseMessage ReleaseNotesAdd(ReleaseNotesDTO releaseNotesDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IReleaseNotesBL bl = IoC.Resolve<IReleaseNotesBL>();
                    Domain.ReleaseNote note = ReleaseNotesMapper.Map(releaseNotesDTO);
                    note.Id = 0;
                    int actionId = bl.ReleaseNotesAdd(note);

                    postResult = PostResult.Create(statusCode, actionId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);

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
        public HttpResponseMessage ReleaseNotesUpdate(ReleaseNotesDTO dtoObj)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IReleaseNotesBL bl = IoC.Resolve<IReleaseNotesBL>();
                    Domain.ReleaseNote note = ReleaseNotesMapper.Map(dtoObj);

                    bl.ReleaseNotesUpdate(note);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

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
        public HttpResponseMessage ReleaseNotesDelete(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            RemoveObjectResult<List<int>> removeObjectResult = null;
            IList<int> notesCannotBeDeleted = new List<int>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> notesIds = ids.Split(',').Select(int.Parse).ToList();
                    IReleaseNotesBL bl = IoC.Resolve<IReleaseNotesBL>();

                    bl.ReleaseNotesDelete(notesIds, out notesCannotBeDeleted);

                    removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, notesCannotBeDeleted.ToList());

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, notesCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                removeObjectResult = RemoveObjectResult<List<int>>.Create(statusCode, notesCannotBeDeleted.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, removeObjectResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage ReleaseNotesSelect([FromUri] SearchCriteria searchCriteria, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReleaseNotesDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IReleaseNotesBL bl = IoC.Resolve<IReleaseNotesBL>();
                    IList<Domain.ReleaseNote> notes = bl.ReleaseNotesSelect(searchCriteria, out rowsCount, cultureName).ToList();
                    List<ReleaseNotesDTO> dtoList = ReleaseNotesMapper.Map(notes);

                    getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, dtoList, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage ReleaseNotesSelectById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ReleaseNotesDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IReleaseNotesBL bl = IoC.Resolve<IReleaseNotesBL>();
                    ReleaseNotesDTO dtoObj = ReleaseNotesMapper.Map(bl.ReleaseNotesSelectById(id));

                    getResult = GetResult<ReleaseNotesDTO>.Create(statusCode, dtoObj, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ReleaseNotesDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ReleaseNotesDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion


        #region SAP Integration
        [HttpPost]
        public HttpResponseMessage UpdateOrgunitSAP(List<OrgunitSapDto> orgunitSapDtos)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    var process = OrgUnitStructureMapper.Map(orgunitSapDtos);

                    orgUnitBL.UpdateOrgFromService(process);

                    putResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        #endregion


        [HttpGet]
        public HttpResponseMessage ActivateDeactivateUser(int UserId, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserProfileDTO userProfileDTO = UserProfileMapper.MapUserProfile(userManagementBL.ActivateDeactivateUser(UserId, CultureName));
                    getResult = GetResult<UserProfileDTO>.Create(statusCode, userProfileDTO, null);
                    transactionContextScope.Commit();
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
        public HttpResponseMessage ApproveRequestedUser(int UserId, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserProfileDTO userProfileDTO = UserProfileMapper.MapUserProfile(userManagementBL.ApproveRequestedUser(UserId, CultureName));
                    getResult = GetResult<UserProfileDTO>.Create(statusCode, userProfileDTO, null);
                    transactionContextScope.Commit();
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
        public HttpResponseMessage RejectRequestedUser(int UserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    bool userProfileRejectResult = userManagementBL.RejectRequestedUser(UserId);
                    getResult = GetResult<bool>.Create(statusCode, userProfileRejectResult, null);
                    transactionContextScope.Commit();
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
        public HttpResponseMessage ActivateDeleteUser(int UserId, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    UserProfileDTO userProfileDTO = UserProfileMapper.MapUserProfile(userManagementBL.ActivateDeleteUser(UserId, CultureName));
                    getResult = GetResult<UserProfileDTO>.Create(statusCode, userProfileDTO, null);
                    transactionContextScope.Commit();
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


        [HttpPut]
        public HttpResponseMessage MoveUser(string usersIDs, int orgunitID, int newOrgunitID, int loggedinUserID)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    orgUnitBL.AdminMoveUser(usersIDs, orgunitID, newOrgunitID, loggedinUserID);
                    putResult = PutResult.Create(statusCode);
                    transactionContextScope.Commit();
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
        public HttpResponseMessage MoveEntity(int entityFrom, int entityTo, int loginUser)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    int conflictedEntityId = orgUnitBL.MoveEntity(entityFrom, entityTo, loginUser, true);
                    if (conflictedEntityId != -1)
                    {
                        statusCode = Common.StatusCode.OrgUnitsHaveSameName;
                    }
                    getResult = GetResult<int>.Create(statusCode, conflictedEntityId, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage AdminMoveTransactions(int entityFromId, int entityToId, int userFromId, int userToId, int logInUser)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {

                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    orgUnitBL.AdminMoveTransactions(entityFromId, entityToId, userFromId, userToId, logInUser);
                    putResult = PutResult.Create(statusCode);
                    transactionContextScope.Commit();
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

        [HttpPut]
        public HttpResponseMessage MoveTransactionById(int transId, int toUserId, int toEntityId, int loggedInUser)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    orgUnitBL.AdminMoveTransactionById(transId, toUserId, toEntityId, loggedInUser);
                    putResult = PutResult.Create(statusCode);
                    transactionContextScope.Commit();
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
        [HttpPut]
        public HttpResponseMessage LockUnlockLookup(int lookupType, int lookUpId, int UserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILookupBL lookupBL = IoC.Resolve<LookupBL>();

                        lookupBL.LockUnlockLookup(lookupType, lookUpId, UserId);

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
        [HttpPut]
        public HttpResponseMessage ActiveDeactiveLookup(int lookupType, int lookUpId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILookupBL lookupBL = IoC.Resolve<LookupBL>();

                        lookupBL.ActiveDeactiveLookup(lookupType, lookUpId);

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
        [HttpGet]
        public HttpResponseMessage GetCities([FromUri] SearchCriteria searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CityDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICityBL cityBL = IoC.Resolve<ICityBL>();
                    List<City> cities = cityBL.GetCities(searchCriteria, out rowsCount).ToList();
                    List<CityDTO> cityDTOs = CityMapper.Map(cities);

                    getResult = GetResult<List<CityDTO>>.Create(statusCode, cityDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetConfidentialities([FromUri] SearchCriteria searchCriteria, int groupId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConfidentialityLevelDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IConfidentialityBL confidentialityBL = IoC.Resolve<IConfidentialityBL>();
                    List<ConfidentialityLevel> confidentialityLevels = confidentialityBL.GetConfidentialities(searchCriteria, groupId, out rowsCount).ToList();
                    List<ConfidentialityLevelDTO> confidentialityLevelDTOs = ConfidentialityLevelMapper.Map(confidentialityLevels);
                    getResult = GetResult<List<ConfidentialityLevelDTO>>.Create(statusCode, confidentialityLevelDTOs, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConfidentialityLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConfidentialityLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckUserClearance(string usersIds, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UsersClearanceDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<int> Ids = usersIds.Split(',').ToList().Select(u => Convert.ToInt32(u)).ToList();

                    IActionBL actionBL = IoC.Resolve<IActionBL>();
                    List<UsersClearance> usersClearances = actionBL.CheckUserClearance(Ids, cultureName);
                    getResult = GetResult<List<UsersClearanceDTO>>.Create(statusCode, UserClearanceMapper.Map(usersClearances), null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UsersClearanceDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UsersClearanceDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserByUserName(string userName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserProfileDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();

                    UserProfile userProfile = userManagementBL.GetUserByUserName(userName);

                    UserProfileDTO UserProfileDTO = UserProfileMapper.MapUserProfile(userProfile);

                    getResult = GetResult<UserProfileDTO>.Create(statusCode, UserProfileDTO, null);

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
        public HttpResponseMessage CheckUserNameExists(string userName, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int?> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    bool isExists = userManagementBL.CheckUserNameExists(userName, CultureName, out int? userId);
                    getResult = GetResult<int?>.Create(statusCode, userId, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<int?>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int?>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckOrgUnitNumber(string Number, int OrgUnitKey)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    bool IsAvailable = orgUnitBL.CheckOrgUnitNumber(Number, OrgUnitKey);

                    getResult = GetResult<bool>.Create(statusCode, IsAvailable, null);

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
        public HttpResponseMessage GetUsersWithGroups()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    var usersWithGroups = userManagementBL.GetUsersWithGroups(Language);

                    List<UserGroupDTO> usersWithGroupDTOs = UserGroupMapper.Map(usersWithGroups: usersWithGroups, Language);

                    getResult = GetResult<List<UserGroupDTO>>.Create(statusCode, usersWithGroupDTOs, usersWithGroups.Count);
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
        public HttpResponseMessage GetUsersWithGroups(string GroupId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserGroupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    var usersWithGroups = userManagementBL
                        .GetUsersWithGroups(Language, GroupId);

                    List<UserGroupDTO> usersWithGroupDTOs = UserGroupMapper
                        .Map(usersWithGroups: usersWithGroups, Language);

                    getResult = GetResult<List<UserGroupDTO>>
                        .Create(statusCode, usersWithGroupDTOs, usersWithGroups.Count);
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
        public HttpResponseMessage GetSubjectClassificationById(int subjectClassificationId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SubjectClassificationDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISubjectClassificationBL subjectClassificationBL = IoC.Resolve<ISubjectClassificationBL>();

                    SubjectClassification subjectClassification = subjectClassificationBL.GetSubjectClassificationById(subjectClassificationId);

                    SubjectClassificationDTO subjectClassificationDTO = SubjectClassificationMapper.Map(subjectClassification);

                    getResult = GetResult<SubjectClassificationDTO>.Create(statusCode, subjectClassificationDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SubjectClassificationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SubjectClassificationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage PutSubjectClassification(SubjectClassificationDTO subjectClassificationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ISubjectClassificationBL subjectClassificationBL = IoC.Resolve<ISubjectClassificationBL>();
                        SubjectClassification subjectClassification = SubjectClassificationMapper.Map(subjectClassificationDTO);

                        subjectClassificationBL.UpdateSubjectClassification(subjectClassification);

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

        [HttpPut]
        public HttpResponseMessage UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILookupBL lookupBL = IoC.Resolve<LookupBL>();

                        lookupBL.UpdateLetterTypeNotifyOption(letterTypeId, operationType);

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

        [HttpPut]
        public HttpResponseMessage UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILookupBL lookupBL = IoC.Resolve<LookupBL>();

                        lookupBL.UpdateLetterTypeWithExtraFieldOption(letterTypeId, operationType);

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

        [HttpGet]
        public HttpResponseMessage getGeneralIoDepartment(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<string> getResult = null;
            string OrgUnitName = string.Empty;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    int? GeneralIoDepartmentId = orgUnitBL.getGeneralIoDepartment();

                    if (GeneralIoDepartmentId.HasValue)
                    {

                        OrgUnitName = IoC.Resolve<IOrgUnitBL>().GetOrgUnitName(o => o.Id == GeneralIoDepartmentId.Value, cultureName);


                    }

                    getResult = GetResult<string>.Create(statusCode, OrgUnitName, null);

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
        public HttpResponseMessage GetUsers()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                    var users = userManagementBL.GetUsers(Language);

                    List<UserProfileDTO> userDTOs = UserProfileMapper.Map(userProfiles: users, Language);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, userDTOs, users.Count);
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


    }
}
