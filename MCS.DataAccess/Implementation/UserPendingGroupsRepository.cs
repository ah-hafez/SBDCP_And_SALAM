using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public class UserPendingGroupsRepository : BaseRepository<UserPendingGroup>, IUserPendingGroupsRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public UserPendingGroupsRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int RequestRole(UserPendingGroup userPendingGroup)
        {
            try
            {
                _oMCSDbContext.UserPendingGroups.Add(userPendingGroup);
                _oMCSDbContext.SaveChanges();
                return userPendingGroup.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<UserPendingGroup> GetuserPendingGroup(string cultureName)
        {

            try
            {
                List<UserPendingGroup> userPendingGroups = _oMCSDbContext.UserPendingGroups.Where(x => x.IsApproved).ToList();
                var userPendingGroupResult = userPendingGroups.ToList().Select(u => new UserPendingGroup
                {
                    Id = u.Id,
                    GroupId = u.GroupId,
                    UserId = u.UserId,
                    User = new UserProfile
                    {
                        Id = u.UserId,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    Group = new Group
                    {
                        Id = u.GroupId,
                        Name = u.Group.GroupName.Localizations.Where(l => l.Culture.Id == 1).LocalText()
                    },
                });
                return userPendingGroupResult.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<UserPendingGroup> GetuserPendingRequest(string cultureName, int userId)
        {

            try
            {
                var orgunit = _oMCSDbContext.OrgUnits.Where(x => x.ManagerId == userId).ToList().Select(x => x.Id);
                List<UserPendingGroup> userPendingGroups = _oMCSDbContext.UserPendingGroups.Where(x => !x.IsApproved && orgunit.Any(org => org == x.User.MainOrgUnitId)).ToList();
                var userPendingGroupResult = userPendingGroups.ToList().Select(u => new UserPendingGroup
                {
                    Id = u.Id,
                    GroupId = u.GroupId,
                    UserId = u.UserId,
                    User = new UserProfile
                    {
                        Id = u.UserId,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },
                    Group = new Group
                    {
                        Id = u.GroupId,
                        Name = u.Group.GroupName.Localizations.Where(l => l.Culture.Id == 1).LocalText()
                    },
                });
                return userPendingGroupResult.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public UserGroup ApproveRoleRequest(int Id, string CultureName)
        {
            try
            {
                UserPendingGroup userPendingGroup = FindBy(u => u.Id == Id);
                UserGroup userGroup = new UserGroup();
                userGroup.GroupId = userPendingGroup.GroupId;
                userGroup.UserId = userPendingGroup.UserId;
                _oMCSDbContext.UserGroups.Add(userGroup);
                _oMCSDbContext.SaveChanges();
                userGroup.User = new UserProfile
                {
                    Id = userPendingGroup.UserId,
                    LocalName = userPendingGroup.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).LocalText(),
                    Email = userPendingGroup.User.Email,
                };
                userGroup.Group = new Group
                {
                    Id = userPendingGroup.GroupId,
                    Name = userPendingGroup.Group.GroupName.Localizations.Where(l => l.Culture.Id == 1).LocalText()
                };
                if (userGroup.Id != 0)
                {
                    _oMCSDbContext.UserPendingGroups.Remove(userPendingGroup);
                }
                _oMCSDbContext.SaveChanges();

                return userGroup;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public bool RejectRoleRequest(int Id)
        {
            try
            {
                UserPendingGroup userPendingGroup = FindBy(u => u.Id == Id);
                _oMCSDbContext.UserPendingGroups.Remove(userPendingGroup);
                _oMCSDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool ApproveManagerRoleRequest(int Id)
        {
            try
            {
                UserPendingGroup userPendingGroup = _oMCSDbContext.UserPendingGroups.FirstOrDefault(u => u.Id == Id);
                userPendingGroup.IsApproved = true;
                _oMCSDbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public bool RejectManagerRoleRequest(int Id)
        {
            try
            {
                UserPendingGroup userPendingGroup = FindBy(u => u.Id == Id);
                _oMCSDbContext.UserPendingGroups.Remove(userPendingGroup);
                _oMCSDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }



        #endregion Methods
    }
}
