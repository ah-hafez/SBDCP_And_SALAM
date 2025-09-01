using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MCS.Framework;
using MCS.Framework.Encryption;
using MCS.Framework.Notifications;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Business.ASPNETIdentity;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;
using System.Text.RegularExpressions;

namespace MCS.Business
{
    public class UserManagementBL : BaseBL, IUserManagementBL
    {
        #region Attributes

        private ICustomSignInManager _signInManager = null;

        #endregion Attributes

        #region Constructors

        public UserManagementBL()
        {
            IMemeberShipProvider memeberShipProvider = new AspNetIdentityProvider();
            _signInManager = memeberShipProvider.GetMemeberShipInstance();
        }

        #endregion Constructors

        #region Methods
        //ToDo:enable it when implement asp .net identity 
        public UserProfile ActivateDeactivateUser(int UserId, string CultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.ActivateDeactivateUser(UserId, CultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public UserProfile ApproveRequestedUser(int UserId, string CultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                var user = userManagementRepository.ApproveRequestedUser(UserId, CultureName);
                SendApproveRequestedUserNotification(user);
                return user;

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public bool RejectRequestedUser(int UserId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                return userManagementRepository.RejectRequestedUser(UserId); ;

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public UserProfile ActivateDeleteUser(int UserId, string CultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.ActivateDeleteUser(UserId, CultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int AddUser(UserProfile userProfile, string url, string culture)
        {
            try
            {
                int result = -1;

                ISettingBL settingBL = new SettingBL();

                List<Setting> settings = settingBL.GetSettingByKey(SettingsKeys.TenantUsersCount);
                Setting setting = settings.Find(a => a.Key == SettingsKeys.TenantUsersCount);

                if (!string.IsNullOrEmpty(setting.Value))
                {
                    int usersCount = GetAllUsersCount();

                    if (usersCount > Convert.ToInt32(setting.Value))
                    {
                        throw new BusinessException(StatusCode.MaxUsersReached);
                    }
                }
                //validate user data
                if ((userProfile.LocalizationIdentifier != null && userProfile.LocalizationIdentifier.Localizations != null && userProfile.LocalizationIdentifier.Localizations.Count == 0) ||
                    string.IsNullOrEmpty(userProfile.UserName) || string.IsNullOrEmpty(userProfile.Email))
                {
                    throw new BusinessException(StatusCode.ModelNotValid);
                }

                IApplicationUser applicationUser = new AspNetIdentityProvider().GetMemeberShipApplicationUser();

                applicationUser.Email = userProfile.Email;
                applicationUser.UserName = userProfile.UserName;
                applicationUser.PhoneNumber = userProfile.PhoneNumber;

                //check if user name already exist
                IApplicationUser existingApplicationUser = _signInManager.FindByName(applicationUser.UserName);

                if (existingApplicationUser != null)
                {
                    throw new BusinessException(StatusCode.UserNameAlreadyExist);
                }

                //check if user email already exist
                existingApplicationUser = _signInManager.FindByEmail(applicationUser.Email);

                if (existingApplicationUser != null)
                {
                    throw new BusinessException(StatusCode.UserEmailAlreadyExist);
                }


                string identityId = string.Empty;
                // string defaultPassword = settingBL.GetSettingByKey(SettingsKeys.DefaultPassword).Value;

                //create the user with the default password
                userProfile.Password = "p@ssw0rd";
                bool success = _signInManager.GenerateUser(applicationUser, AESEncrytDecry.DecryptStringAES(userProfile.Password), out identityId);

                if (success)
                {
                    userProfile.IdentityId = identityId;

                    IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();


                    //Update Manager
                    //if (userProfile.IsManager == true)
                    //{
                    //    IList<UserProfile> userProfiles = userManagementRepository.GetUsersByOrgUnitId(userProfile.MainOrgUnitId, culture);

                    //    foreach (var UserProfileItem in userProfiles)
                    //    {
                    //        UserProfileItem.IsManager = false;
                    //        userManagementRepository.UpdateManger(UserProfileItem);
                    //    }
                    //}

                    result = userManagementRepository.AddUser(userProfile);




                    UserPreference userPreference = new UserPreference()
                    {
                        UserProfileId = userProfile.Id,
                        PhoneNumber = userProfile.PhoneNumber,
                        IsDelegationEnabled = false,
                        CultureId = 1,
                        NotificationSubscriptions = NotificationSubscriptions.Delegation ^ NotificationSubscriptions.ElectronicCopies ^ NotificationSubscriptions.Explanation
                                                    ^ NotificationSubscriptions.Followup ^ NotificationSubscriptions.MyTransactions ^ NotificationSubscriptions.OrgUnit
                                                    ^ NotificationSubscriptions.OutboundDraft ^ NotificationSubscriptions.ReceiveReport ^ NotificationSubscriptions.Tasks
                                                    ^ NotificationSubscriptions.VerificationCode,
                        DefaultDisplay = 1,
                        DefaultAssignmentPaper = false,
                    };
                    AddUserPreference(userPreference);
                    //send email 

                    //SendUserCreationNotification(userProfile, culture, url);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateUser(UserProfile userProfile, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                string userIdentityId = userManagementRepository.GetUserIdentityByUserId(userProfile.Id);

                IApplicationUser existingApplicationUser = _signInManager.FindByName(userProfile.UserName);

                if (existingApplicationUser != null && existingApplicationUser.Id != userIdentityId)
                {
                    throw new BusinessException(StatusCode.UserNameAlreadyExist);
                }

                //check if user email already exist
                existingApplicationUser = _signInManager.FindByEmail(userProfile.Email);

                if (existingApplicationUser != null && existingApplicationUser.Id != userIdentityId)
                {
                    throw new BusinessException(StatusCode.UserEmailAlreadyExist);
                }

                IApplicationUser applicationUser = _signInManager.GetUser(userIdentityId);

                applicationUser.Email = userProfile.Email;
                applicationUser.UserName = userProfile.UserName;
                applicationUser.PhoneNumber = userProfile.PhoneNumber;

                _signInManager.UpdateUser(applicationUser);

                if (userProfile.Password != null && userProfile.Password != string.Empty && !(AESEncrytDecry.DecryptStringAES(userProfile.Password).ToLower() == userProfile.UserName.ToLower()))
                {
                    string token = _signInManager.GenerateResetPasswordToken(userIdentityId);

                    _signInManager.ResetPassword(userIdentityId, token, AESEncrytDecry.DecryptStringAES(userProfile.Password));
                }

                //Update Manager
                //if (userProfile.IsManager == true)
                //{
                //    IList<UserProfile> userProfiles = userManagementRepository.GetUsersByOrgUnitId(userProfile.MainOrgUnitId, cultureName);

                //    foreach (var UserProfileItem in userProfiles)
                //    {
                //        UserProfileItem.IsManager = false;
                //        userManagementRepository.UpdateManger(UserProfileItem);
                //    }
                //}
                userManagementRepository.UpdateUser(userProfile);

                userManagementRepository.UpdateUserRoles(userProfile);




                //if (userProfile.IsDeleted)
                //{
                //    SendUserNotification(userProfile, NotificationSource.DeleteUser, NotificationTemplateType.DeleteUserEmail,
                //        NotificationEmailSubject.DeleteUser, cultureName);
                //}
                //else
                //{
                //    SendUserNotification(userProfile, NotificationSource.ModifiedUser, NotificationTemplateType.ModifiedUserEmail,
                //        NotificationEmailSubject.ModifiedUser, cultureName);
                //}
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void IAMUpdateUser(UserProfile userProfile, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                string userIdentityId = userManagementRepository.GetUserIdentityByUserId(userProfile.Id);

                IApplicationUser existingApplicationUser = _signInManager.FindByName(userProfile.UserName);

                if (existingApplicationUser != null && existingApplicationUser.Id != userIdentityId)
                {
                    throw new BusinessException(StatusCode.UserNameAlreadyExist);
                }

                //check if user email already exist
                existingApplicationUser = _signInManager.FindByEmail(userProfile.Email);

                if (existingApplicationUser != null && existingApplicationUser.Id != userIdentityId)
                {
                    throw new BusinessException(StatusCode.UserEmailAlreadyExist);
                }

                IApplicationUser applicationUser = _signInManager.GetUser(userIdentityId);

                applicationUser.Email = userProfile.Email;
                applicationUser.UserName = userProfile.UserName;
                applicationUser.PhoneNumber = userProfile.PhoneNumber;

                _signInManager.UpdateUser(applicationUser);

                if (userProfile.Password != null && userProfile.Password != string.Empty && !(AESEncrytDecry.DecryptStringAES(userProfile.Password).ToLower() == userProfile.UserName.ToLower()))
                {
                    string token = _signInManager.GenerateResetPasswordToken(userIdentityId);

                    _signInManager.ResetPassword(userIdentityId, token, AESEncrytDecry.DecryptStringAES(userProfile.Password));
                }

                //Update Manager
                //if (userProfile.IsManager == true)
                //{
                //    IList<UserProfile> userProfiles = userManagementRepository.GetUsersByOrgUnitId(userProfile.MainOrgUnitId, cultureName);

                //    foreach (var UserProfileItem in userProfiles)
                //    {
                //        UserProfileItem.IsManager = false;
                //        userManagementRepository.UpdateManger(UserProfileItem);
                //    }
                //}
                userManagementRepository.IAMUpdateUser(userProfile);

                userManagementRepository.UpdateUserRoles(userProfile);




                //if (userProfile.IsDeleted)
                //{
                //    SendUserNotification(userProfile, NotificationSource.DeleteUser, NotificationTemplateType.DeleteUserEmail,
                //        NotificationEmailSubject.DeleteUser, cultureName);
                //}
                //else
                //{
                //    SendUserNotification(userProfile, NotificationSource.ModifiedUser, NotificationTemplateType.ModifiedUserEmail,
                //        NotificationEmailSubject.ModifiedUser, cultureName);
                //}
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public bool CheckIfNotUsedUser(int id)
        {
            try
            {
                return CheckIfUserCanBeDeleted(id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteUsers(IList<int> ids, out IList<int> usersCannotBeDeleted, string cultureName)
        {
            try
            {
                usersCannotBeDeleted = new List<int>();
                foreach (var id in ids)
                {
                    if (CheckIfUserCanBeDeleted(id))
                    {
                        usersCannotBeDeleted.Add(id);

                        continue;
                    }

                    UserProfile userProfile = GetUserById(id);

                    userProfile.IsDeleted = true;

                    UpdateUser(userProfile, cultureName);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsers(Expression<Func<UserProfile, bool>> @where)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersProfiles(@where);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserProfile GetUserById(int userProfileId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.Get(userProfileId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public UserProfile GetUserByUserName(string userName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserByUserName(userName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public static UserProfile GetUserByUserNameForWordAddIn(string userName)
        {
            try
            {
                return UserManagementRepository.GetUserByUserNameForWordAddIn(userName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public UserProfile GetUserByIdentity(string userProfileIdentity)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserByIdentity(userProfileIdentity);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserProfile GetUserChatByIdentity(string userProfileIdentity)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>("transient_UserManagementRepository");
                return userManagementRepository.GetUserChatByIdentity(userProfileIdentity);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserProfile GetUserByIdentity(string userProfileIdentity, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserByIdentity(userProfileIdentity, cultureName);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int GetUserIdByIdentity(string userProfileIdentity)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserIdByIdentity(userProfileIdentity);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserProfile GetUserByEmail(string sUserEmail, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserByEmail(sUserEmail, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserProfile GetUserByUserNationalId(string sUserNationalId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserByUserNationalId(sUserNationalId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                if (ex.Message == "Ora-031350")
                {
                    throw new BusinessException(StatusCode.Ora031350);
                }
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersProfiles(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersProfiles(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<UserProfile> GetPendingRegestrationUsersProfiles(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetPendingRegestrationUsersProfiles(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersByOrgUnitId(orgUnitId, searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersProfiles(string cultureName, string searchQuery = null, int? entityId = null)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetAllUsers(cultureName, searchQuery, entityId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersProfiles(Expression<Func<UserProfile, bool>> @where)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersProfiles(@where);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserPermission> GetUserPermissions(int userId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserPermissions(userId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Tray> GetUserTrays(int userId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserTrays(userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByPermissionId(int permissionId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersByPermissionId(permissionId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByTrayId(int trayId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersByTrayId(trayId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> SearchUsersByOrgUnitId(int? OrgUnitId, string cultureName, string term)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.SearchUsersByOrgUnitId(OrgUnitId, cultureName, term);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUsersByOrgUnitId(OrgUnitId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetOrgUnitUsers(SearchCriteria searchCriteria, int orgUnitId, string cultureName, out int ItemsCount, bool noExternal = false)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                if (!SystemConfigurations.ERPIntegrationEnabled)
                    noExternal = false;

                return userManagementRepository.GetOrgUnitUsers(searchCriteria, orgUnitId, cultureName, out ItemsCount, noExternal);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetChildEntityUsersByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetChildEntityUsersByOrgUnitId(OrgUnitId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public string GetUserName(int userId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserName(userId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public bool CheckPassword(string identityId, string password)
        {
            try
            {
                return _signInManager.CheckPassword(identityId, password);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public bool ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                IUserManagementBL userManagementBL = new UserManagementBL();

                UserProfile userProfile = userManagementBL.GetUserById(User.Id);

                IEnumerable<string> errors;

                bool succeeded = _signInManager.ChangePassword(userProfile.IdentityId, oldPassword, newPassword, out errors);

                if (!succeeded)
                {
                    throw new BusinessException(StatusCode.InvalidOldPassword);
                }

                return succeeded;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void ResetPasswordStepOne(string username, string email, string cultureName, string resetPasswordUrl)
        {
            try
            {
                UserProfile user = CheckIfValidUserInfo(username, email);

                string token = HttpUtility.UrlEncode(_signInManager.GenerateResetPasswordToken(user.IdentityId));

                string varificationCode = _signInManager.GenerateVarificationCode(user.IdentityId, user.PhoneNumber);

                SendResetPasswordNotification(user, token, cultureName, resetPasswordUrl, varificationCode);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void ResetPasswordStepTwo(ResetPasswordDTO resetPasswordDTO)
        {
            try
            {
                _signInManager.ResetPassword(resetPasswordDTO.IdentityId, resetPasswordDTO.Token, resetPasswordDTO.NewPassword, resetPasswordDTO.Code, resetPasswordDTO.PhoneNumber);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void SendUserCreationNotification(UserProfile user, string culture, string url)
        {
            try
            {
                string token = HttpUtility.UrlEncode(_signInManager.GenerateResetPasswordToken(user.IdentityId));

                string varificationCode = _signInManager.GenerateVarificationCode(user.IdentityId, user.PhoneNumber);

                SendUserCreationNotification(user, token, culture, url, varificationCode);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int AddAssignmentGroup(AssignmentGroup assignmentGroup, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                assignmentGroup.OwnerId = User.Id;

                IList<AssignmentGroup> assignmentGroups = GetUserAssignmentGroups(User.Id, cultureName);

                if (assignmentGroups.Where(a => a.LocalName == assignmentGroup.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text).FirstOrDefault() != null)
                {
                    throw new BusinessException(StatusCode.UserGroupNameAlreadyExist);
                }

                return userManagementRepository.AddAssignmentGroup(assignmentGroup);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<AssignmentGroup> GetUserAssignmentGroups(int userId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserAssignmentGroups(userId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public AssignmentGroup GetAssignmentGroupById(int groupId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetAssignmentGroupById(groupId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public AssignmentGroup GetAssignmentGroupById(int groupId, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetAssignmentGroupById(groupId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void ActivateUser(UserProfile userProfile, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                userManagementRepository.ActivateUser(userProfile);
                userProfile = userManagementRepository.GetUserById(userProfile.Id);
                if (userProfile.IsActive)
                {
                    SendUserNotification(userProfile, NotificationSource.EnabledUser, NotificationTemplateType.EnabledUserEmail,
                        NotificationEmailSubject.EnabledUser, cultureName);
                }
                else
                {
                    SendUserNotification(userProfile, NotificationSource.DisabledUser, NotificationTemplateType.DisabledUserEmail,
                        NotificationEmailSubject.DisabledUser, cultureName);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void AddUserPreference(UserPreference userPreference, int? orgUnitId = null)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                if (userPreference.Id > 0 && orgUnitId.HasValue)
                {
                    userPreferenceRepository.UpdateUserPreferenceFollowup(userPreference.Id, orgUnitId.Value, userPreference.FollowUpOrgId, userPreference.FollowUpUserId);
                    userPreference.FollowUpUserId = null;
                    userPreference.FollowUpOrgId = null;
                }

                userPreferenceRepository.AddUserPreference(userPreference);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateUserPreference(UserPreference userPreference, int? orgUnitId = null)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                UserPreference oldUserPreference = userPreferenceRepository.GetUserPreferenceByUserId(userPreference.UserProfileId);
                List<UserDelegation> oldDelegations = oldUserPreference.UserDelegations.ToList();

                userPreference.AssignmentPaper = oldUserPreference.AssignmentPaper;

                if (userPreference.UserDelegations != null && userPreference.UserDelegations.Count > 0)
                {
                    foreach (var userDelegation in userPreference.UserDelegations)
                    {
                        UserDelegation existedDelegation = oldDelegations.Where(d =>
                                             d.ConfidentialityId == userDelegation.ConfidentialityId &&
                                             d.PriorityId == userDelegation.PriorityId &&
                                             d.TransactionTypeId == userDelegation.TransactionTypeId &&
                                             (d.FromDate < userDelegation.ToDate && userDelegation.FromDate < d.ToDate)
                                                ).FirstOrDefault();
                        if (existedDelegation != null)
                        {
                            throw new BusinessException(StatusCode.DuplicateUserDeligation);
                        }
                    }
                }

                if (userPreference.Id > 0 && orgUnitId.HasValue)
                {
                    userPreferenceRepository.UpdateUserPreferenceFollowup(userPreference.Id, orgUnitId.Value, userPreference.FollowUpOrgId, userPreference.FollowUpUserId);
                    userPreference.FollowUpUserId = null;
                    userPreference.FollowUpOrgId = null;
                }

                userPreferenceRepository.UpdateUserPreference(userPreference);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateUserDelegation(UserDelegation userDelegation, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                List<UserDelegation> oldDelegations = userPreferenceRepository.GetUserPreference(userDelegation.UserPreferenceId).UserDelegations.ToList();
                UserDelegation existedDelegation = oldDelegations.Where(d => d.FromDate < userDelegation.ToDate && userDelegation.FromDate < d.ToDate).FirstOrDefault();
                if (existedDelegation == null)
                {
                    throw new BusinessException(StatusCode.DuplicateUserDeligation);
                }

                userPreferenceRepository.UpdateUserDelegation(userDelegation);

                if ((DelegationStatus)userDelegation.StatusId.LookupInternalID(LookupCategory.DelegationStatus, cultureName) == DelegationStatus.Approved)
                {
                    var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(userDelegation.UserProfileId) };
                    SendDelegationNotification(userDelegation, NotificationSource.ApprovedDelegation, NotificationTemplateType.ApprovedDelegationWeb,
                        NotificationTemplateType.ApprovedDelegationEmail, NotificationEmailSubject.ApprovedDelegationEmail,
                        NotificationWebSubject.ApprovedDelegation, notificationUsers, cultureName);
                }
                else if ((DelegationStatus)userDelegation.StatusId.LookupInternalID(LookupCategory.DelegationStatus, cultureName) == DelegationStatus.Rejected)
                {
                    var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(userDelegation.UserProfileId) };
                    SendDelegationNotification(userDelegation, NotificationSource.RejectedDelegation, NotificationTemplateType.RejectedDelegationWeb,
                        NotificationTemplateType.RejectedDelegationEmail, NotificationEmailSubject.RejectedDelegationEmail,
                        NotificationWebSubject.RejectedDelegation, notificationUsers, cultureName);
                }
                else if ((DelegationStatus)userDelegation.StatusId.LookupInternalID(LookupCategory.DelegationStatus, cultureName) == DelegationStatus.Disabled)
                {
                    IOrgUnitBL orgUnitB = new OrgUnitBL();
                    var notificationAdmin = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(orgUnitB.GetOrgUnitById(userDelegation.OrgUnitId).ManagerId) };
                    SendDelegationNotification(userDelegation, NotificationSource.DisabledDelegation, NotificationTemplateType.DisabledDelegationWeb,
                        NotificationTemplateType.DisabledDelegationEmail, NotificationEmailSubject.DisabledDelegationEmail, NotificationWebSubject.DisabledDelegation,
                        notificationAdmin, cultureName);

                    var notificationUser = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(userDelegation.UserProfileId) };
                    SendDelegationNotification(userDelegation, NotificationSource.DisabledDelegation, NotificationTemplateType.DisabledDelegationWeb,
                        NotificationTemplateType.DisabledDelegationEmail, NotificationEmailSubject.DisabledDelegationEmail, NotificationWebSubject.DisabledDelegation,
                        notificationUser, cultureName);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateUserDelegations(int userId, IList<UserDelegation> userDelegations, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateUserDelegations(userId, userDelegations);
                foreach (var delegateItem in userDelegations)
                {
                    delegateItem.CreatedBy = userId;
                    IOrgUnitBL orgUnitB = new OrgUnitBL();
                    var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(delegateItem.UserProfileId) };
                    SendDelegationNotification(delegateItem, NotificationSource.AddDelegation, NotificationTemplateType.AddDelegationWeb,
                        NotificationTemplateType.AddDelegationEmail, NotificationEmailSubject.AddDelegationEmail, NotificationWebSubject.AddDelegation
                        , notificationUsers, cultureName);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private void SendDelegationNotification(UserDelegation userDelegation, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL orgUnitB = new OrgUnitBL();
                var userManagementBL = new UserManagementBL();
                IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();
                foreach (var item in notificationUsers)
                {
                    var userPreferenceInfo = userManagementBL.GetUserPreferenceByUserId(item.User.Id, cultureName);
                    if (userPreferenceInfo != null && userPreferenceInfo.NotificationSubscriptions.HasFlag(NotificationSubscriptions.Delegation))
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>
                        {
                            ["{CreatedBy}"] = userManagementBL.GetUserById(userDelegation.CreatedBy.Value).UserName,
                            ["{UserProfileId}"] = userManagementRepository.GetUserLocalNameById(userDelegation.UserProfileId, cultureName),
                            ["{FromDateH}"] = userDelegation.FromDateH,
                            ["{ToDateH}"] = userDelegation.ToDateH
                        };

                        //Notification Web
                        NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                        //Notification Email
                        if (SystemConfigurations.MultiTenantEnabled)
                        {
                            TenantBL tenantBL = new TenantBL();
                            tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType, notificationEmailSubject,
                            notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                        }
                        else
                        {
                            var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                            //System Notification  Email
                            NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                                notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                        }
                    }
                }
            }
        }


        public void UpdateUserDelegationStatus(int delegateId, int statusId, string rejectionReason, string cultureName)
        {
            try
            {
                UserDelegation oldDelegate = GetUserDelegationById(delegateId, cultureName);
                if (oldDelegate != null)
                {
                    oldDelegate.StatusId = statusId;
                    if (statusId == DelegationStatus.Rejected.LookupIdentity(LookupCategory.DelegationStatus, cultureName))
                    {
                        oldDelegate.RejectionReason = rejectionReason;
                    }

                    UpdateUserDelegation(oldDelegate, cultureName);
                }

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public static UserPreferenceInfo GetUserPreferenceByUserIdForWordAddIn(int userId)
        {
            try
            {
                UserPreference userPreference = UserPreferenceRepository.GetUserPreferenceByUserIdForWordAddIn(userId);

                if (userPreference != null)
                {
                    UserPreferenceInfo userPreferenceInfo = new UserPreferenceInfo()
                    {
                        Id = userPreference.Id,
                        IsDelegated = userPreference.IsDelegationEnabled,
                        PasswordComfiration = userPreference.SignaturePassword,
                        Signature = userPreference.Signature,
                        SignatureBehalf = userPreference.SignatureBehalf,
                        MessageSignature = userPreference.MessageSignatureDoc,
                        SealSignatureDoc = userPreference.SealSignatureDoc,
                        SignatureCommand = userPreference.SignatureCommand,
                        UserProfile = userPreference.UserProfile,
                        UserDelegations = userPreference.UserDelegations,
                        NotificationSubscriptions = userPreference.NotificationSubscriptions,
                        UserTrayPreferencesInfo = new List<UserTrayPreferenceInfo>(),
                        Culture = userPreference.Culture,
                        Marking = userPreference.MarkingDoc,
                        HasSignaturePasswordText = userPreference.HasSignaturePasswordText,
                        FollowUpOrgId = userPreference.FollowUpOrgId,
                        FollowUpUserId = userPreference.FollowUpUserId,
                        ThemeId = userPreference.ThemeId,
                        SMSNotifications = userPreference.SMSNotifications,
                        MyDelegations = userPreference.MyDelegations,
                        DefaultDisplay = userPreference.DefaultDisplay
                    };

                    return userPreferenceInfo;
                }

                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public UserPreferenceInfo GetUserPreferenceByUserId(int userId, string cultureName, int? orgUnitId = null)
        {
            try
            {
                IList<Tray> trays = TrayBaseBL.GetAllTrays(cultureName);

                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                UserPreference userPreference = userPreferenceRepository.GetUserPreferenceByUserId(userId, cultureName, orgUnitId);

                if (userPreference != null)
                {
                    return Map(userPreference, trays);
                }

                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public NotificationSubscriptions GetUserNotificationSubscriptions(int userId, string cultureName)
        {
            try
            {

                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                NotificationSubscriptions notificationSubscriptions = userPreferenceRepository.GetUserNotificationSubscriptions(userId, cultureName);



                return notificationSubscriptions;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public byte[] GetUserSignByType(int userId, int signType)
        {
            try
            {


                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();

                return userPreferenceRepository.GetUserSignByType(userId, signType);


            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<UserPreferenceInfo> GetUserPreferenceByUserIds(List<int> userIds)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                var userPreference = userPreferenceRepository.GetUserPreferenceByUserIds(userIds);
                var result = Map(userPreference);
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public AssignmentPaper GetAssignmentPaperByUserId(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                AssignmentPaper assignmentPaper = userPreferenceRepository.GetAssignmentPaperByUserId(userId, cultureName);

                if (assignmentPaper != null)
                {
                    return assignmentPaper;
                }

                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void AddAssignmentPaper(AssignmentPaper assignmentPaper, int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.AddAssignmentPaper(assignmentPaper, userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateAssignmentPaper(AssignmentPaper assignmentPaper, int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateAssignmentPaper(assignmentPaper, userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaper, int groupId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateGroupAssignmentPaper(assignmentPaper, groupId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiary> assignmentPaper)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateGroupAssignmentPaper(assignmentPaper);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int AddDistributionList(DistributionList distributionList)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.AddDistributionList(distributionList);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int SaveDistributionListDetails(List<DistributionListDetails> distributionListDetails, int DistributionListId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.SaveDistributionListDetails(distributionListDetails, DistributionListId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int UpdateDistributionList(DistributionList distributionList)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.UpdateDistributionList(distributionList);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int DeleteDistributionList(int distributionListId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.DeleteDistributionList(distributionListId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<DistributionList> GetDistributionList(int userId, int orgUnitId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetDistributionList(userId, orgUnitId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public DistributionList GetDistributionListById(int userId, int orgUnitId, int id)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetDistributionListById(userId, orgUnitId, id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public bool VerifySignaturePassword(string SignaturePasswordTxt, int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.VerifySignaturePassword(SignaturePasswordTxt, userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteDelegations(IList<int> ids)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                foreach (var id in ids)
                {
                    userPreferenceRepository.DeleteDelegation(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        private UserPreferenceInfo Map(UserPreference userPreference, IList<Tray> trays)
        {
            UserPreferenceInfo userPreferenceInfo = new UserPreferenceInfo()
            {
                Id = userPreference.Id,
                IsDelegated = userPreference.IsDelegationEnabled,
                PasswordComfiration = userPreference.SignaturePassword,
                Signature = userPreference.Signature,
                SignatureBehalf = userPreference.SignatureBehalf,
                MessageSignature = userPreference.MessageSignatureDoc,
                SignatureCommand = userPreference.SignatureCommand,
                SealSignatureDoc = userPreference.SealSignatureDoc,
                UserProfile = userPreference.UserProfile,
                UserDelegations = userPreference.UserDelegations,
                NotificationSubscriptions = userPreference.NotificationSubscriptions,
                UserTrayPreferencesInfo = new List<UserTrayPreferenceInfo>(),
                Culture = userPreference.Culture,
                Marking = userPreference.MarkingDoc,
                HasSignaturePasswordText = userPreference.HasSignaturePasswordText,
                FollowUpOrgId = userPreference.FollowUpOrgId,
                FollowUpUserId = userPreference.FollowUpUserId,
                ThemeId = userPreference.ThemeId,
                SMSNotifications = userPreference.SMSNotifications,
                MyDelegations = userPreference.MyDelegations,
                DefaultDisplay = userPreference.DefaultDisplay,
                DefaultAssignmentPaper = userPreference.DefaultAssignmentPaper,
            };

            foreach (Tray tray in trays)
            {
                UserTrayPreference userTrayPreference =
                    userPreference.UserTrayPreferences.Where(p => p.Tray.Id == tray.Id).FirstOrDefault();

                UserTrayPreferenceInfo userTrayPreferenceInfo = new UserTrayPreferenceInfo()
                {
                    TrayId = tray.Id,
                    TrayName = tray.LocalName
                };

                if (userTrayPreference != null)
                {
                    userTrayPreferenceInfo.IsSelected = true;
                }

                userPreferenceInfo.UserTrayPreferencesInfo.Add(userTrayPreferenceInfo);
            }

            return userPreferenceInfo;
        }

        private List<UserPreferenceInfo> Map(List<UserPreference> userPreferences)
        {
            if (userPreferences == null)
            {
                return new List<UserPreferenceInfo>();
            }

            List<UserPreferenceInfo> userPreferenceInfos = new List<UserPreferenceInfo>();

            foreach (var item in userPreferences)
            {
                UserPreferenceInfo userPreferenceInfo = new UserPreferenceInfo()
                {
                    Id = item.Id,
                    UserProfile = item.UserProfile,
                    NotificationSubscriptions = item.NotificationSubscriptions,
                    Culture = item.Culture,
                    ThemeId = item.ThemeId
                };
                userPreferenceInfos.Add(userPreferenceInfo);
            }
            return userPreferenceInfos;
        }
        private UserPreferenceInfo Map(UserPreference userPreference)
        {
            UserPreferenceInfo userPreferenceInfo = new UserPreferenceInfo()
            {
                Id = userPreference.Id,
                IsDelegated = userPreference.IsDelegationEnabled,
                PasswordComfiration = userPreference.SignaturePassword,
                Signature = userPreference.Signature,
                SignatureBehalf = userPreference.SignatureBehalf,
                SealSignatureDoc = userPreference.SealSignatureDoc,
                MessageSignature = userPreference.MessageSignatureDoc,
                SignatureCommand = userPreference.SignatureCommand,
                UserProfile = userPreference.UserProfile,
                UserDelegations = userPreference.UserDelegations,
                NotificationSubscriptions = userPreference.NotificationSubscriptions,
                UserTrayPreferencesInfo = new List<UserTrayPreferenceInfo>(),
                Culture = userPreference.Culture,
                Marking = userPreference.MarkingDoc,
                ThemeId = userPreference.ThemeId
            };
            return userPreferenceInfo;
        }
        private UserPreferenceInfo MapForLogin(UserPreference userPreference)
        {
            UserPreferenceInfo userPreferenceInfo = new UserPreferenceInfo()
            {
                Id = userPreference.Id,
                Signature = userPreference.Signature,
                SignatureBehalf = userPreference.SignatureBehalf,
                SealSignatureDoc = userPreference.SealSignatureDoc,
                SignatureCommand = userPreference.SignatureCommand,
                MessageSignature = userPreference.MessageSignatureDoc,
                Marking = userPreference.MarkingDoc,
                CultureId = userPreference.CultureId,
                ThemeId = userPreference.ThemeId,
                SMSNotifications = userPreference.SMSNotifications,
                HasSignaturePasswordText = userPreference.SignaturePasswordText == null ? false : true,
                DefaultDisplay = userPreference.DefaultDisplay,
                DefaultAssignmentPaper = userPreference.DefaultAssignmentPaper,

            };
            return userPreferenceInfo;
        }
        public UserDelegation GetUserDelegationById(int id, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserDelegationById(id, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<UserDelegation> GetUserDelegations(int preferenceId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserDelegations(preferenceId, searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<UserDelegation> GetUserDelegationsByUserId(int? userId, string cultureName, int? orgUnitId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserDelegationsByUserId(userId, cultureName, orgUnitId, searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserPreference GetUserPreferenceByUserId(int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserPreferenceByUserId(userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int AddUserCategory(UserCategory userCategory)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.AddUserCategory(userCategory);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateUserCategory(UserCategory userCategory)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                userCategoryRepository.UpdateUserCategory(userCategory);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserCategory GetUserCategoryById(int userCategoryId)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.Get(userCategoryId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public UserCategory GetUserCategoryByUserId(int userId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetUserCategoryByUserId(userId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteUserCategories(IList<int> ids, out IList<int> userCategoriesCannotBeDeleted)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                userCategoriesCannotBeDeleted = new List<int>();

                foreach (var id in ids)
                {
                    if (CheckIfUserCategoryUsed(id))
                    {
                        userCategoriesCannotBeDeleted.Add(id);

                        continue;
                    }
                    userCategoryRepository.DeleteUserCategory(id);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserCategory> GetUserCategories(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.GetUserCategories(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Tray> GetUserCategoryTrays(int userCategoryId)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.GetUserCategoryTrays(userCategoryId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Tray> GetUserCategoryTrays(int userCategoryId, string cultureName)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.GetUserCategoryTrays(userCategoryId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateUsersCategoriesTrays(IList<UserCategoryTray> usersCategoriesTrays)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                userCategoryRepository.UpdateUsersCategoriesTrays(usersCategoriesTrays);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<UserCategory> GetUserCategories(string cultureName)
        {
            try
            {
                IUserCategoryRepository userCategoryRepository = IoC.Resolve<IUserCategoryRepository>();
                return userCategoryRepository.GetAllUsersCategoriesTrays(cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public int GetAllUsersCount()
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                return userManagementRepository.GetAllUsersCount();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        private bool CheckIfUserCategoryUsed(int userCategoryId)
        {
            try
            {
                IUserManagementBL userManagementBL = new UserManagementBL();
                IList<UserProfile> userProfiles = userManagementBL.GetUsersProfiles(u => u.Category.Id == userCategoryId);
                return userProfiles.ToList().Count > 0;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        private bool CheckIfUserCanBeDeleted(int userId)
        {
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

            IList<Transaction> transactions = TransactionBL.GetTransactions(t => t.SignedByUser.Id == userId || t.User.Id == userId || t.ToUser.Id == userId);

            IList<TransactionAssignmentHistory> transactionAssignmentHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistories(t => t.FromUser.Id == userId || t.ToUser.Id == userId);

            IList<Task> tasks = transactionTaskBL.GetTasks(t => t.ToUser.Id == userId || t.FromUser.Id == userId);

            return transactions.ToList().Count > 0 || transactionAssignmentHistories.ToList().Count > 0 || tasks.ToList().Count > 0;
        }
        private UserProfile CheckIfValidUserInfo(string userName, string email)
        {
            IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
            UserProfile user = userManagementRepository.CheckIfValidUserInfo(userName, email);

            if (user == null)
            {
                throw new BusinessException(StatusCode.UsernameOrEmailNotCorrect);
            }

            return user;
        }
        private void SendUserCreationNotification(UserProfile user, string token, string cultureName, string url, string varificationCode)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                notificationUsers.Add(NotificationsManager.BuildNotificationUser(user.Id));

                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                int cultureId = cultureName == "ar" ? (int)CultureType.Arabic : (int)CultureType.English;
                keyValues.Add("{UserName}", user.LocalizationIdentifier.Localizations.Where(l => l.CultureId == cultureId).FirstOrDefault().Text);
                keyValues.Add("{Url}", string.Concat(url, string.Format("?identityId={0}&token={1}&username={2}&phoneNumber={3}", user.IdentityId, token, user.UserName, user.PhoneNumber)));
                keyValues.Add("{Code}", varificationCode);

                //Tenant Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(NotificationSource.NewUser, NotificationTemplateType.NewUserEmail,
                        NotificationEmailSubject.NewUserEmail, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                }
                else
                {
                    var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                    //System Notification  Email
                    NotificationsManager.EmailNotification(NotificationSource.NewUser, NotificationTemplateType.NewUserEmail,
                        NotificationEmailSubject.NewUserEmail, notificationUsersEmail, cultureName, null, keyValues);
                }
            }
        }
        private void SendResetPasswordNotification(UserProfile user, string token, string cultureName, string resetPasswordUrl, string varificationCode)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                notificationUsers.Add(NotificationsManager.BuildNotificationUser(user.Id));

                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                string userName = string.Empty;
                Localization localization = user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();
                if (localization != null)
                {
                    keyValues.Add("{UserName}", localization.Text);
                    keyValues.Add("{Url}", string.Concat(resetPasswordUrl, string.Format("?identityId={0}&token={1}&username={2}&phoneNumber={3}", user.IdentityId, token, user.UserName, user.PhoneNumber)));
                    keyValues.Add("{Code}", varificationCode);
                    //Notification Web
                    NotificationsManager.EmailNotification(NotificationSource.ResetPassword, NotificationTemplateType.ResetPasswordEmail,
                        NotificationEmailSubject.ResetPasswordEmail, notificationUsers, cultureName, null, keyValues);
                }
            }
        }
        private void SendUserNotification(UserProfile user, NotificationSource notificationSource,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                notificationUsers.Add(NotificationsManager.BuildNotificationUser(user.Id));

                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add("{UserName}", user.UserName);

                //Tenant Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                }
                else
                {
                    var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                    //System Notification  Email
                    NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                }
            }
        }

        public void UpdateTransactionPath(TransactionPath transactionPath, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateTransactionPath(transactionPath);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<TransactionPath> GetTransactionPath(int? userId, int? orgUnitId, int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetTransactionPath(userId, orgUnitId, pageIndex, pageSize, cultureName, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<TransactionPath> GetAllPaths(int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetAllPaths(pageIndex, pageSize, cultureName, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<TransactionPath> GetPathsName(int OrgUnitId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetPathsName(OrgUnitId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<TransactionPath> GetTransactionPathForTransaction(int? userId, int? orgUnitId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetTransactionPathForTransaction(userId, orgUnitId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public TransactionPath GetTransactionPathById(int pathId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetTransactionPathById(pathId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int DeleteTransactionPath(int pathId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.DeleteTransactionPath(pathId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateTransactionPathDetailsSort(int pathId, int sort, string order)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateTransactionPathDetailsSort(pathId, sort, order);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        /// <summary>
        /// get User PreferenceInfo By UserId without tray information
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserPreferenceInfo GetUserPreferenceInfoByUserId(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                UserPreference userPreference = userPreferenceRepository.GetUserPreferenceByUserId(userId, cultureName);
                if (userPreference != null)
                {
                    return Map(userPreference);
                }
                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public UserPreferenceInfo GetUserPreferenceForLogin(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                UserPreference userPreference = userPreferenceRepository.GetUserPreferenceForLogin(userId, cultureName);
                if (userPreference != null)
                {
                    return MapForLogin(userPreference);
                }
                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public string GetThemeByIdForLogin(int ThemeId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                string path = userPreferenceRepository.GetThemesById(ThemeId);
                if (path != null)
                {
                    return path;
                }
                return null;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UserLoginAction(int userId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();
                 userManagementRepository.UserLoginAction(userId);
                
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UserLogoutAction(string userId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();
                userManagementRepository.UserLogoutAction(userId);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public bool GenerateVerificationCode(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                Random rnd = new Random();
                string code = rnd.Next(100000, 999999).ToString();
                bool isUpdate = userPreferenceRepository.GenerateVerificationCode(userId, code);
                List<int> userIds = new List<int> { userId };
                var userPreferenceInfo = userManagementBL.GetUserPreferenceByUserIds(userIds).FirstOrDefault();
                if (isUpdate && userPreferenceInfo != null && userPreferenceInfo.NotificationSubscriptions.HasFlag(NotificationSubscriptions.VerificationCode))
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>() { { "{Code}", code }, { "{UserName}", User.UserName } };
                    //System Notification Email
                    NotificationDetail notificationDetail = NotificationsManager.BuildNotificationDetail(NotificationType.Email,
                                                            NotificationTemplateType.VerificationCodeEmail,
                                                            null,
                                                            NotificationEmailSubject.VerificationCodeEmail,
                                                            cultureName);
                    notificationDetail.Body = FormatEmailLabels(notificationDetail.Body, keyValues);
                    if (notificationDetail != null)
                    {
                        //Send Verification Code Email
                        var emailMessage = new EmailMessage();
                        emailMessage.Subject = notificationDetail.Subject;
                        emailMessage.Body = notificationDetail.Body;
                        emailMessage.To = User.Email;
                        IEmailNotificationService emailNotificationService = new EmailNotificationService();
                        emailNotificationService.Send(emailMessage);

                        //Send SMS
                        //if (SystemConfigurations.IsSMSEnabled)
                        //{
                        //}
                    }
                }
                return isUpdate;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private string FormatEmailLabels(string notificationBody, Dictionary<string, string> labels)
        {
            foreach (KeyValuePair<string, string> label in labels)
            {
                notificationBody = notificationBody.Replace(label.Key, label.Value);
            }
            return notificationBody;
        }

        public void UpdateUserProfile(int userId, string email)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                userManagementRepository.UpdateUserProfile(userId, email);
                User.Email = email;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateUserPreference(int userId, string code)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                var userPreference = userPreferenceRepository.GetUserPreferenceByUserId(userId);
                if (userPreference != null)
                {
                    if (userPreference.OTP != code)
                    {
                        throw new BusinessException(StatusCode.OTPInvalid);
                    }
                    var timeSub = DateTime.Now - userPreference.OTPCreatedOn.Value;
                    if (timeSub.TotalMinutes > SystemConfigurations.OTPExpirationMinutes)
                    {
                        throw new BusinessException(StatusCode.OTPVeryOld);
                    }
                }
                userPreferenceRepository.UpdateUserPreference(userId, code);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateSignaturePassword(string signaturePassword, PasswordType passwordType)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateSignaturePassword(User.Id, signaturePassword, passwordType);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public bool CheckUserNameExists(string userName, string CultureName, out int? userId)
        {
            try
            {

                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                //check if user name already exist
                IApplicationUser existingApplicationUser = _signInManager.FindByName(userName.ToLower());

                if (existingApplicationUser != null)
                {
                    bool isExists = userManagementRepository.CheckUserNameExists(userName, existingApplicationUser.Id, out int? newUserId);
                    userId = newUserId;
                    return isExists;
                }
                userId = (int?)null;
                return false;

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<UserGroup> GetUsersWithGroups(string language)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                List<UserGroup> usersWithGroupsList = userManagementRepository.GetUsersWithGroups();

                foreach (var usersWithGroups in usersWithGroupsList)
                {
                    usersWithGroups.Group.Name = usersWithGroups.Group.GroupName.Localizations.FirstOrDefault(a => a.Culture.ShortName == language).Text;
                }

                return usersWithGroupsList;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<UserProfile> GetUsers(string language)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                List<UserProfile> usersList = userManagementRepository.GetUsers();

                return usersList;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<UserGroup> GetUsersWithGroups(string language, string GroupId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();

                List<UserGroup> usersWithGroupsList = userManagementRepository.GetUsersWithGroups(GroupId);






                foreach (var usersWithGroups in usersWithGroupsList)
                {
                    usersWithGroups.Group.Name = usersWithGroups.Group.GroupName.Localizations.FirstOrDefault(a => a.Culture.ShortName == language).Text;
                }

                return usersWithGroupsList;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<AssignmentPaperGroup> GetAssignmentPaperGroupsByUserId(int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                List<AssignmentPaperGroup> assignmentPaperGroupList = userPreferenceRepository.GetAssignmentPaperGroupsByUserId(userId);

                return assignmentPaperGroupList;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public List<AssignmentPaperBeneficiary> GetBeneficiaryByAssignmentPaperGroupId(int groupId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                List<AssignmentPaperBeneficiary> assignmentPaperGroupList = userPreferenceRepository.GetBeneficiaryByAssignmentPaperGroupId(groupId);

                return assignmentPaperGroupList;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void SaveAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.SaveAssignmentPaperGroup(assignmentPaperGroup);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public AssignmentPaperGroup GetAssignmentPaperGroupById(int assignmentPaperGroupId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                AssignmentPaperGroup assignmentPaperGroup = userPreferenceRepository.GetAssignmentPaperGroupById(assignmentPaperGroupId);

                return assignmentPaperGroup;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateAssignmentPaperGroup(AssignmentPaperGroup assignmentPaperGroup)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateAssignmentPaperGroup(assignmentPaperGroup);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<UserDelegation> GetLoggedInUserDelegations(int UserId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetLoggedInUserDelegations(UserId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<UserDelegation> GetUserDelegationsById(int UserId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserDelegationsById(UserId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }




        private void SendApproveRequestedUserNotification(UserProfile user)
        {


            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(user.Id) };
            SendTransactionNotification(user.Id, NotificationSource.NewUser, NotificationTemplateType.NewUserRequest,
                NotificationTemplateType.NewUserRequest, NotificationEmailSubject.NewUserEmail, NotificationWebSubject.NewUser,
                notificationUsers, "ar", null);

        }


        private void SendTransactionNotification(int userId, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
        NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
                    IList<NotificationUser> notificationUsers, string cultureName, IList<NotificationAttachment> attachments)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                //keyValues["{Number}"] = transaction.Number.ToString();
                //keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                //keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                //keyValues["{sender}"] = User.UserName;
                //keyValues["{Date}"] = transaction.DateH;
                //keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                //keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                //keyValues["{TransactionId}"] = transaction.Id.ToString();
                //keyValues["{UserName}"] = User.UserName;
                //keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);

                //System Notification Web
                //NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

                //System Notification Email
                //if (SystemConfigurations.MultiTenantEnabled)
                //{
                //    TenantBL tenantBL = new TenantBL();
                //    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                //        notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                //}
                //else
                //{
                var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                //System Notification  Email
                NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                    notificationEmailSubject, notificationUsersEmail, cultureName, attachments, keyValues);
                //}
            }
        }



        public int AddAllowedAssignment(AllowedAssignment allowedAssignment)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<UserPreferenceRepository>();


                return userPreferenceRepository.AddAllowedAssignment(allowedAssignment);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<AllowedAssignment> GetAllowedAssignment(int UserId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<UserPreferenceRepository>();


                return userPreferenceRepository.GetAllowedAssignment(UserId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }



        public bool RemoveAllowedAssignment(int Id)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<UserPreferenceRepository>();


                return userPreferenceRepository.RemoveAllowedAssignment(Id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public AllowedAssignment GetAllowedUserAssignment(int ToUserId, int FromUserId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<UserPreferenceRepository>();


                return userPreferenceRepository.GetAllowedUserAssignment(ToUserId, FromUserId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public int DeleteAssignmentPaperGroupById(int assignmentPaperGroupId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.DeleteAssignmentPaperBeneficiary(assignmentPaperGroupId);
                userPreferenceRepository.DeleteAssignmentPaperGroup(assignmentPaperGroupId);

                return 0;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void RemoveSignaturePassword(int userId)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.UpdateSignaturePassword(userId, string.Empty, PasswordType.Delete);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateUserInternalNumber(int userId, string phoneNumber, string internalNumber)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                userManagementRepository.UpdateUserInternalNumber(userId, phoneNumber, internalNumber);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public void ChangeGroupOrder(int id, bool isMoveUp)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.ChangeGroupOrder(id, isMoveUp);


            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public void AddUserGroup(int userid, int groupId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                userManagementRepository.AddUserGroup(userid, groupId);


            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void RemoveUserGroup(int userid, int groupId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<UserManagementRepository>();
                userManagementRepository.RemoveUserGroup(userid, groupId);


            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}