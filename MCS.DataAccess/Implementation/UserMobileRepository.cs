using System;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class UserMobileRepository : BaseRepository<UserMobile>, IUserMobileRepository
    {
        #region Attributes

        #endregion Attributes

        #region Constructors

        public UserMobileRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {


        }

        #endregion Constructors

        #region Methods
        public UserMobile GetUserMobile(int? userId, string userName, string cultureName)
        {
            try
            {
                if (userId != null && userId != -1)
                {
                    return _oMCSDbContext.UserMobiles
                                           .Where(um => um.UserId == userId)
                                           .Select(um => new
                                           {
                                               um.UserId,
                                               um.Token,
                                               um.DeviceToken,
                                               um.ActivationRequestCode,
                                               um.ActivataionCode,
                                               um.DeactivationRequestCode,
                                               um.SignedCert,
                                               um.CA,
                                               um.CACRL,
                                               um.IsUpdated,
                                               um.UpdateFlags,
                                               um.LastLoginDate,
                                               LoginName = um.UserProfile.UserName,
                                               EntityId = um.DefaultEntityId ?? um.UserProfile.OrgUnits.FirstOrDefault().Id,
                                               um.UserProfile.AllowMobile,
                                               um.Settings
                                           }).ToList().Select(userMobile => new UserMobile
                                           {
                                               UserId = userMobile.UserId,
                                               Token = userMobile.Token,
                                               DeviceToken = userMobile.DeviceToken,
                                               ActivationRequestCode = userMobile.ActivationRequestCode,
                                               ActivataionCode = userMobile.ActivataionCode,
                                               DeactivationRequestCode = userMobile.DeactivationRequestCode,
                                               SignedCert = userMobile.SignedCert,
                                               CA = userMobile.CA,
                                               CACRL = userMobile.CACRL,
                                               IsUpdated = userMobile.IsUpdated,
                                               UpdateFlags = userMobile.UpdateFlags,
                                               LastLoginDate = userMobile.LastLoginDate,
                                               LoginName = userMobile.LoginName,
                                               EntityId = userMobile.EntityId,
                                               AllowMobile = userMobile.AllowMobile,
                                               Settings = userMobile.Settings
                                           }).FirstOrDefault();
                }
                else
                {
                    int nUserId = _oMCSDbContext.UserProfiles.Where(u => u.UserName == userName).Select(u => u.Id).FirstOrDefault();
                    return _oMCSDbContext.UserMobiles
                                            .Where(um => um.UserId == nUserId)
                                            .Select(um => new
                                            {
                                                um.UserId,
                                                um.Token,
                                                um.DeviceToken,
                                                um.ActivationRequestCode,
                                                um.ActivataionCode,
                                                um.DeactivationRequestCode,
                                                um.SignedCert,
                                                um.CA,
                                                um.CACRL,
                                                um.IsUpdated,
                                                um.UpdateFlags,
                                                um.LastLoginDate,
                                                LoginName = um.UserProfile.UserName,
                                                EntityId = um.DefaultEntityId ?? um.UserProfile.OrgUnits.FirstOrDefault().Id,
                                                um.UserProfile.AllowMobile,
                                                um.Settings,
                                                um.UserProfile.UserMobileClassId
                                            }).ToList().Select(userMobile => new UserMobile
                                            {
                                                UserId = userMobile.UserId,
                                                Token = userMobile.Token,
                                                DeviceToken = userMobile.DeviceToken,
                                                ActivationRequestCode = userMobile.ActivationRequestCode,
                                                ActivataionCode = userMobile.ActivataionCode,
                                                DeactivationRequestCode = userMobile.DeactivationRequestCode,
                                                SignedCert = userMobile.SignedCert,
                                                CA = userMobile.CA,
                                                CACRL = userMobile.CACRL,
                                                IsUpdated = userMobile.IsUpdated,
                                                UpdateFlags = userMobile.UpdateFlags,
                                                LastLoginDate = userMobile.LastLoginDate,
                                                LoginName = userMobile.LoginName,
                                                EntityId = userMobile.EntityId,
                                                AllowMobile = userMobile.AllowMobile,
                                                Settings = userMobile.Settings,
                                                UserMobileClassId = userMobile.UserMobileClassId
                                            }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserMobile GetUserMobileForUpdate(int? userId, string userName, string cultureName)
        {
            try
            {
                return FindBy(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateUserMobile(UserMobile userMobile, string cultureName)
        {
            try
            {
                UserMobile oldUserMobile = GetUserMobileForUpdate(userMobile.UserId, string.Empty, cultureName);
                if (oldUserMobile != null)
                {
                    oldUserMobile.Token = userMobile.Token;
                    //oldUserMobile.DeviceToken = oldUserMobile.DeviceToken;
                    //oldUserMobile.ActivationRequestCode = oldUserMobile.ActivationRequestCode;
                    //oldUserMobile.ActivataionCode = oldUserMobile.ActivataionCode;
                    //oldUserMobile.DeactivationRequestCode = oldUserMobile.DeactivationRequestCode;
                    //oldUserMobile.SignedCert = oldUserMobile.SignedCert;
                    //oldUserMobile.CA = oldUserMobile.CA;
                    //oldUserMobile.CACRL = oldUserMobile.CACRL;
                    //oldUserMobile.IsUpdated = oldUserMobile.IsUpdated;
                    //oldUserMobile.UpdateFlags = oldUserMobile.UpdateFlags;
                    oldUserMobile.LastLoginDate = userMobile.LastLoginDate;
                    //oldUserMobile.Logs = oldUserMobile.Logs;
                    //oldUserMobile.Settings = oldUserMobile.Settings;
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void SetDefaultEntity(int userId, int defaultEntityId)
        {
            try
            {
                var user = _oMCSDbContext.UserMobiles.Where(x => x.UserId == userId && x.UserProfile.OrgUnits.Any(o => o.Id == defaultEntityId)).FirstOrDefault();
                if (user != null)
                {
                    user.DefaultEntityId = defaultEntityId;
                    _oMCSDbContext.SaveChanges();
                }
                else
                {
                    throw new DataAccessException("InvalidEntityId");
                }

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion
    }
}