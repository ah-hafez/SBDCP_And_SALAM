using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.Linq;
using MCS.Framework.Notifications;

namespace MCS.Business
{
    public class UserPendingGroupBL : BaseBL, IUserPendingGroupBL
    {

        public int RequestRoleItem(UserPendingGroup userPendingGroup, string cultureName)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                return userPendingGroupsRepository.RequestRole(userPendingGroup);
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

        public List<UserPendingGroup> GetuserPendingGroup(string cultureName)
        {
            try
            {

                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                return userPendingGroupsRepository.GetuserPendingGroup(cultureName);
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
        public List<UserPendingGroup> GetuserPendingRequest(string cultureName)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                return userPendingGroupsRepository.GetuserPendingRequest(cultureName, User.Id);
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



        public UserGroup ApproveRoleRequest(int Id, string CultureName)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                var userGroup = userPendingGroupsRepository.ApproveRoleRequest(Id, CultureName);

                SendRoleRequestApprovalNotification(userGroup.User, userGroup);
                return userGroup;
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

        public bool RejectRoleRequest(int Id)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                return userPendingGroupsRepository.RejectRoleRequest(Id);
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

        public bool ApproveManagerRoleRequest(int Id)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                var isSuccess = userPendingGroupsRepository.ApproveManagerRoleRequest(Id);
                return isSuccess;
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

        public bool RejectManagerRoleRequest(int Id)
        {
            try
            {
                IUserPendingGroupsRepository userPendingGroupsRepository = IoC.Resolve<UserPendingGroupsRepository>();
                return userPendingGroupsRepository.RejectManagerRoleRequest(Id);
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

        private void SendRoleRequestApprovalNotification(UserProfile user, UserGroup userGroup)
        {
            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(user.Id) };
            SendTransactionNotification(user.Id, NotificationSource.RoleRequest, NotificationTemplateType.RoleRequest,
                NotificationTemplateType.RoleRequest, NotificationEmailSubject.RoleRequest, NotificationWebSubject.RoleRequest,
                notificationUsers, "ar", userGroup);
        }


        private void SendTransactionNotification(int userId, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
        NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
                    IList<NotificationUser> notificationUsers, string cultureName, UserGroup userGroup)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{GroupName}"] = userGroup.Group.Name.ToString();
                keyValues["{UserName}"] = userGroup.User.LocalName.ToString();


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
                    notificationEmailSubject, notificationUsersEmail, cultureName, new List<NotificationAttachment>(), keyValues);
                //}
            }
        }






    }
}
