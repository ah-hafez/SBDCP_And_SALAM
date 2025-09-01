using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Common;

namespace MCS.DataAccess
{
    public class UserManagementRepository : BaseRepository<UserProfile>, IUserManagementRepository
    {
        #region Attributes
        #endregion Attributes
        #region Constructors
        public UserManagementRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }
        #endregion Constructors
        #region Methods
        public UserProfile ActivateDeactivateUser(int UserId, string CultureName)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.FirstOrDefault(g => g.Id == UserId);

                if (userProfile != null)
                {

                    userProfile.IsActive = !userProfile.IsActive;
                    _oMCSDbContext.SaveChanges();

                    userProfile.LocalName = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text;
                    return userProfile;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message);
            }
        }

        public UserProfile ApproveRequestedUser(int UserId, string CultureName)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.FirstOrDefault(g => g.Id == UserId);

                if (userProfile != null)
                {

                    userProfile.PendingRegestration = false;
                    _oMCSDbContext.SaveChanges();

                    userProfile.LocalName = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text;
                    return userProfile;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message);
            }
        }


        public bool RejectRequestedUser(int UserId)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.FirstOrDefault(g => g.Id == UserId);
                userProfile.IsDeleted = true;
                _oMCSDbContext.SaveChanges();

                return true;

            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message);
            }
        }

        public UserProfile ActivateDeleteUser(int UserId, string CultureName)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.FirstOrDefault(g => g.Id == UserId);

                if (userProfile != null)
                {

                    userProfile.IsDeleted = false;
                    _oMCSDbContext.SaveChanges();

                    userProfile.LocalName = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text;
                    return userProfile;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message);
            }
        }

        public int AddUser(UserProfile userProfile)
        {
            try
            {
                _oMCSDbContext.UserProfiles.Add(userProfile);

                _oMCSDbContext.SaveChanges();

                //userProfile.OrgUnits.Where(o => o.Id == userProfile.MainOrgUnitId).FirstOrDefault().ManagerId = userProfile.Id;

                //_oMCSDbContext.SaveChanges();

                return userProfile.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateUser(UserProfile userProfile)
        {
            try
            {
                UserProfile user = GetUserById(userProfile.Id);
                if (user != null)
                {
                    userProfile.IdentityId = user.IdentityId;

                    _oMCSDbContext.Entry(user).CurrentValues.SetValues(userProfile);
                    _oMCSDbContext.Entry(user).State = EntityState.Modified;
                    foreach (Localization localization in userProfile.LocalizationIdentifier.Localizations)
                    {
                        Localization existingLocalization = user.LocalizationIdentifier.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

                        if (existingLocalization != null)
                        {
                            _oMCSDbContext.Entry(existingLocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    user.TitleId = userProfile.TitleId;
                    //user.GroupId = userProfile.GroupId;
                    //user.Group = userProfile.Group;
                    user.CategoryId = userProfile.CategoryId;
                    user.Gender = userProfile.Gender;

                    if (userProfile.Permissions != null)
                    {
                        var listOfPermissions = userProfile.Permissions.ToList();

                        user.Permissions.ToList().ForEach(p =>
                            user.Permissions.Remove(p));

                        listOfPermissions.ForEach(p =>
                        {
                            user.Permissions.Add(p);
                        });
                    }

                    if (userProfile.OrgUnits != null)
                    {
                        var listOfOrgUnits = userProfile.OrgUnits.ToList();

                        user.OrgUnits.ToList().ForEach(p =>
                          user.OrgUnits.Remove(p));

                        listOfOrgUnits.ForEach(o =>
                        {
                            user.OrgUnits.Add(o);
                        });
                    }

                    user.UserNationalId = userProfile.UserNationalId;
                    user.MainOrgUnitId = userProfile.MainOrgUnitId;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void IAMUpdateUser(UserProfile userProfile)
        {
            try
            {
                UserProfile user = GetUserById(userProfile.Id);
                if (user != null)
                {
                    userProfile.IdentityId = user.IdentityId;

                    _oMCSDbContext.Entry(user).CurrentValues.SetValues(userProfile);
                    _oMCSDbContext.Entry(user).State = EntityState.Modified;
                    foreach (Localization localization in userProfile.LocalizationIdentifier.Localizations)
                    {
                        Localization existingLocalization = user.LocalizationIdentifier.Localizations
                         .Where(l => l.CultureId == localization.CultureId).FirstOrDefault();
                        localization.Id = existingLocalization.Id;
                        if (existingLocalization != null)
                        {
                            _oMCSDbContext.Entry(existingLocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    user.TitleId = userProfile.TitleId;
                    //user.GroupId = userProfile.GroupId;
                    //user.Group = userProfile.Group;
                    user.CategoryId = userProfile.CategoryId;
                    user.Gender = userProfile.Gender;

                    if (userProfile.Permissions != null)
                    {
                        var listOfPermissions = userProfile.Permissions.ToList();

                        user.Permissions.ToList().ForEach(p =>
                            user.Permissions.Remove(p));

                        listOfPermissions.ForEach(p =>
                        {
                            user.Permissions.Add(p);
                        });
                    }

                    if (userProfile.OrgUnits != null)
                    {
                        var listOfOrgUnits = userProfile.OrgUnits.ToList();

                        user.OrgUnits.ToList().ForEach(p =>
                          user.OrgUnits.Remove(p));

                        listOfOrgUnits.ForEach(o =>
                        {
                            user.OrgUnits.Add(o);
                        });
                    }

                    user.UserNationalId = userProfile.UserNationalId;
                    user.MainOrgUnitId = userProfile.MainOrgUnitId;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateUserRoles(UserProfile userProfile)
        {
            try
            {
                var listOfUserGroups = userProfile.UserGroups.ToList();
                if (_oMCSDbContext.UserGroups.Where(g => g.UserId == userProfile.Id).Any())
                {
                    _oMCSDbContext.UserGroups.RemoveRange(_oMCSDbContext.UserGroups.Where(c => c.UserId == userProfile.Id));
                }

                listOfUserGroups.ForEach(ug =>
                {
                    var item = new UserGroup
                    {
                        UserId = ug.UserId,
                        GroupId = ug.GroupId
                    };
                    _oMCSDbContext.UserGroups.Add(item);
                });

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserProfile GetUserById(int userProfileId)
        {
            try
            {
                return FindBy(u => u.Id == userProfileId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetUserIdentityByUserId(int userProfileId)
        {
            try
            {
                UserProfile userProfile = FindBy(u => u.Id == userProfileId);
                if (userProfile != null)
                {
                    return userProfile.IdentityId;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersProfiles(Expression<Func<UserProfile, bool>> @where)
        {
            try
            {
                return (from userProfile in _oMCSDbContext.UserProfiles.Where(@where)
                        select new
                        {
                            userProfile.Id,
                        }).ToList().Select(u => new UserProfile
                        {
                            Id = u.Id
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUserByIdentity(string userProfileIdentity)
        {
            try
            {
                IList<UserProfile> userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                                   where userProfile.IdentityId == userProfileIdentity &&
                                                   userProfile.IsActive == true &&
                                                   userProfile.IsDeleted == false
                                                   select new
                                                   {
                                                       userProfile.Id,
                                                       OrgUnits = userProfile.OrgUnits.Where(o => o.IsDeleted == false),
                                                       userProfile.Permissions,
                                                       userProfile.Category,
                                                       userProfile.UserGroups,
                                                       userProfile.MainOrgUnitId
                                                   }).ToList().Select(a => new UserProfile
                                                   {
                                                       Id = a.Id,
                                                       Permissions = a.Permissions,
                                                       OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                                       {
                                                           Id = o.Id,
                                                       }).ToList(),
                                                       Category = new UserCategory
                                                       {
                                                           Id = a.Category.Id,
                                                       },
                                                       UserGroups = a.UserGroups,
                                                       MainOrgUnitId = a.MainOrgUnitId
                                                   }).ToList();
                return userProfiles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUserChatByIdentity(string userProfileIdentity)
        {
            try
            {
                var data = _oMCSDbContext.UserProfiles
                    .Include(a => a.Rooms)
                    .Include(a => a.OwnedRooms)
                    .Include(a => a.AllowedRooms)
                    .Include(a => a.Rooms.Select(r => r.ChatRoom))
                    .Include(a => a.LocalizationIdentifier)
                    .Include(a => a.LocalizationIdentifier.Localizations)
                    .Include(a => a.ConnectedClients)
                    .FirstOrDefault(userProfile => userProfile.IdentityId == userProfileIdentity
                    && userProfile.IsActive == true
                    && userProfile.IsDeleted == false);
                return data;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUserByIdentity(string userProfileIdentity, string cultureName)
        {
            try
            {
                //_oMCSDbContext.UserProfiles.Where(x => x.IdentityId == userProfileIdentity);
                UserProfile resutlUserProfile = (from userProfile in _oMCSDbContext.UserProfiles
                                                 where userProfile.IdentityId == userProfileIdentity &&
                                                 userProfile.IsActive == true &&
                                                 userProfile.IsDeleted == false
                                                 select new
                                                 {
                                                     userProfile.IsActive,
                                                     userProfile.Id,
                                                     //userProfile.GroupId,
                                                     //userProfile.Group,
                                                     userProfile.UserGroups,
                                                     userProfile.LocalizationIdentifier,
                                                     userProfileText = userProfile.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                     OrgUnits = userProfile.OrgUnits.Where(o => o.IsDeleted == false).Select(org => new
                                                     {
                                                         org.Id,
                                                         org.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                         org.LocalizationIdentifier,
                                                         org.ManagerId
                                                     }),
                                                     userProfile.Permissions,
                                                     userProfile.Category,
                                                     userProfile.Email,
                                                     userProfile.PhoneNumber,
                                                     userProfile.UserName,
                                                     userProfile.TitleId,
                                                     userProfile.PendingRegestration,
                                                     category = (userProfile.Category == null) ? null : new
                                                     {
                                                         CategoryId = userProfile.Category.Id,
                                                         CategoryLocalName = userProfile.Category.CategoryName.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                         userProfile.Category.CategoryName
                                                     },
                                                     userProfile.MainOrgUnitId,
                                                     userProfile.InternalNumber
                                                 }).ToList().Select(a => new UserProfile
                                                 {
                                                     IsActive = a.IsActive,
                                                     Id = a.Id,
                                                     //GroupId = a.GroupId,
                                                     //Group = a.Group,
                                                     UserGroups = a.UserGroups,
                                                     UserName = a.UserName,
                                                     Permissions = a.Permissions,
                                                     LocalName = a.userProfileText,
                                                     LocalizationIdentifier = a.LocalizationIdentifier,
                                                     TitleId = a.TitleId,
                                                     OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                                     {
                                                         Id = o.Id,
                                                         LocalName = o.Text,
                                                         LocalizationIdentifier = o.LocalizationIdentifier,
                                                         ManagerId = o.ManagerId
                                                     }).ToList(),
                                                     Category = (a.category == null) ? null : new UserCategory
                                                     {
                                                         Id = a.category.CategoryId,
                                                         LocalName = a.category.CategoryLocalName,
                                                         CategoryName = a.category.CategoryName
                                                     },
                                                     Email = a.Email,
                                                     PhoneNumber = a.PhoneNumber,
                                                     MainOrgUnitId = a.MainOrgUnitId,
                                                     PendingRegestration = a.PendingRegestration,
                                                     InternalNumber = a.InternalNumber,


                                                 }).FirstOrDefault();

                return resutlUserProfile;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUserByEmail(string sUserEmail, string cultureName)
        {
            try
            {
                IList<UserProfile> userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                                   where userProfile.Email == sUserEmail &&
                                                   userProfile.IsActive == true &&
                                                   userProfile.IsDeleted == false
                                                   select new
                                                   {
                                                       userProfile.Id,
                                                       userProfile.LocalizationIdentifier,
                                                       LocalizationIdentifierText = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                       OrgUnits = userProfile.OrgUnits.Where(o => o.IsDeleted == false),
                                                       userProfile.Permissions,
                                                       userProfile.Category,
                                                       userProfile.Email,
                                                       userProfile.UserGroups,
                                                       userProfile.UserName
                                                   }).ToList().Select(a => new UserProfile
                                                   {
                                                       Id = a.Id,
                                                       Permissions = a.Permissions,
                                                       LocalName = a.LocalizationIdentifierText,
                                                       LocalizationIdentifier = a.LocalizationIdentifier,
                                                       OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                                       {
                                                           Id = o.Id,
                                                           LocalName = o.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                           LocalizationIdentifier = o.LocalizationIdentifier
                                                       }).ToList(),
                                                       Category = (a.Category == null) ? null : new UserCategory
                                                       {
                                                           Id = a.Category.Id,
                                                           LocalName = a.Category.CategoryName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                           CategoryName = a.Category.CategoryName
                                                       },
                                                       Email = a.Email,
                                                       UserGroups = a.UserGroups,
                                                       UserName = a.UserName
                                                   }).ToList();
                return userProfiles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUserByUserNationalId(string sUserNationalId, string cultureName)
        {
            try
            {
                UserProfile resutlUserProfile = (from userProfile in _oMCSDbContext.UserProfiles
                                                 where userProfile.UserNationalId == sUserNationalId &&
                                                 userProfile.IsActive == true &&
                                                 userProfile.IsDeleted == false
                                                 select new
                                                 {
                                                     userProfile.Id,
                                                     userProfile.LocalizationIdentifier,
                                                     userProfileText = userProfile.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                     OrgUnits = userProfile.OrgUnits.Where(o => o.IsDeleted == false).Select(org => new
                                                     {
                                                         org.Id,
                                                         org.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                         org.LocalizationIdentifier
                                                     }),
                                                     userProfile.Permissions,
                                                     userProfile.Category,
                                                     userProfile.Email,
                                                     userProfile.UserName,
                                                     category = (userProfile.Category == null) ? null : new
                                                     {
                                                         CategoryId = userProfile.Category.Id,
                                                         CategoryLocalName = userProfile.Category.CategoryName.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text,
                                                         userProfile.Category.CategoryName
                                                     }
                                                 }).ToList().Select(a => new UserProfile
                                                 {
                                                     Id = a.Id,
                                                     UserName = a.UserName,
                                                     Permissions = a.Permissions,
                                                     LocalName = a.userProfileText,
                                                     LocalizationIdentifier = a.LocalizationIdentifier,
                                                     OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                                     {
                                                         Id = o.Id,
                                                         LocalName = o.Text,
                                                         LocalizationIdentifier = o.LocalizationIdentifier
                                                     }).ToList(),
                                                     Category = (a.category == null) ? null : new UserCategory
                                                     {
                                                         Id = a.category.CategoryId,
                                                         LocalName = a.category.CategoryLocalName,
                                                         CategoryName = a.category.CategoryName
                                                     },
                                                     Email = a.Email
                                                 }).FirstOrDefault();

                return resutlUserProfile;
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex)
            {
                throw new DataAccessException("Ora-031350");
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetUserIdByIdentity(string userProfileIdentity)
        {
            try
            {
                IQueryable<UserProfile> userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                                        where userProfile.IdentityId == userProfileIdentity &&
                                                        userProfile.IsActive == true &&
                                                        userProfile.IsDeleted == false
                                                        select new
                                                        {
                                                            Id = userProfile.Id,
                                                        }).ToList().Select(a => new UserProfile
                                                        {
                                                            Id = a.Id,
                                                        }).AsQueryable();
                return userProfiles.FirstOrDefault().Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersProfiles(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserProfile> userProfiles;
                if (searchCriteria.isDeleted == false)
                {
                    userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                    where userProfile.IsDeleted == searchCriteria.isDeleted && userProfile.IsInternal == false && userProfile.PendingRegestration != true
                                    select userProfile);
                }
                else
                {
                    userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                    where userProfile.IsInternal == false && userProfile.PendingRegestration != true
                                    select userProfile);
                }

                rowsCount = 0;
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (Filter filter in searchCriteria.Filters)
                        {
                            if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(filter.ColumnName).PropertyType))
                            {
                                userProfiles = SortByText(userProfiles, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            else if (typeof(UserProfile).GetProperty(filter.ColumnName).PropertyType == typeof(UserCategory))
                            {
                                userProfiles = SortUserCategoryByText(userProfiles, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            else
                            {
                                if (filter.ColumnName.ToLower() == "username")
                                {
                                    userProfiles = UserProfileWhereQuery(userProfiles, filter.ColumnName, filter.Value.ToLower(), filter.Type);
                                }
                                else
                                {
                                    userProfiles = WhereQuery(userProfiles, filter.ColumnName, filter.Value, filter.Type);
                                }
                            }
                        }
                    }
                    if (searchCriteria.OrderBy != null)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(searchCriteria.OrderBy).PropertyType))
                        {
                            userProfiles = OrderByText(userProfiles, searchCriteria.CultureName, searchCriteria.Ascending);
                        }
                        else
                        {
                            userProfiles = OrderQuery(userProfiles, searchCriteria.OrderBy, searchCriteria.Ascending);
                        }
                    }
                    if (searchCriteria.UserId.HasValue && searchCriteria.UserId.Value > 0)
                    {
                        userProfiles = userProfiles.Where(x => x.Id == searchCriteria.UserId.Value);

                    }
                    rowsCount = userProfiles.Count();
                    userProfiles = userProfiles.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                  .Take(searchCriteria.PageSize);
                    return userProfiles.ToList().Select(u => new UserProfile
                    {
                        Id = u.Id,
                        LocalName = u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        Email = u.Email,
                        Category = u.Category,
                        IdentityId = u.IdentityId,
                        IsActive = u.IsActive,
                        OrgUnits = u.OrgUnits,
                        UserName = u.UserName,
                        Permissions = u.Permissions,
                        UserNationalId = u.UserNationalId,
                        PhoneNumber = u.PhoneNumber,
                        Gender = u.Gender,
                        Rooms = u.Rooms,
                        MainOrgUnitId = u.MainOrgUnitId,
                        TransactionProcessingPeriod = u.TransactionProcessingPeriod,
                        Title = u.Title,
                        TitleId = u.TitleId,
                        IsDeleted = u.IsDeleted,
                        //Group = u.Group,
                        IsManager = u.IsManager,
                        //GroupId = u.GroupId,
                        ExternalId = u.ExternalId,
                        PendingRegestration = u.PendingRegestration,
                        UserGroups = u.UserGroups,
                        CategoryId = u.CategoryId,
                        AllowMobile = u.AllowMobile,
                        InternalNumber = u.InternalNumber,
                        LocalizationIdentifier = u.LocalizationIdentifier,


                    }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public IList<UserProfile> GetPendingRegestrationUsersProfiles(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserProfile> userProfiles;
                if (searchCriteria.isDeleted == false)
                {
                    userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                    where userProfile.IsDeleted == searchCriteria.isDeleted && userProfile.IsInternal == false && userProfile.PendingRegestration == true
                                    select userProfile);
                }
                else
                {
                    userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                    where userProfile.IsInternal == false && userProfile.PendingRegestration == true
                                    select userProfile);
                }

                rowsCount = 0;
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (Filter filter in searchCriteria.Filters)
                        {
                            if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(filter.ColumnName).PropertyType))
                            {
                                userProfiles = SortByText(userProfiles, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            else if (typeof(UserProfile).GetProperty(filter.ColumnName).PropertyType == typeof(UserCategory))
                            {
                                userProfiles = SortUserCategoryByText(userProfiles, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            else
                            {
                                if (filter.ColumnName.ToLower() == "username")
                                {
                                    userProfiles = UserProfileWhereQuery(userProfiles, filter.ColumnName, filter.Value.ToLower(), filter.Type);
                                }
                                else
                                {
                                    userProfiles = WhereQuery(userProfiles, filter.ColumnName, filter.Value, filter.Type);
                                }
                            }
                        }
                    }
                    if (searchCriteria.OrderBy != null)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(searchCriteria.OrderBy).PropertyType))
                        {
                            userProfiles = OrderByText(userProfiles, searchCriteria.CultureName, searchCriteria.Ascending);
                        }
                        else
                        {
                            userProfiles = OrderQuery(userProfiles, searchCriteria.OrderBy, searchCriteria.Ascending);
                        }
                    }
                    rowsCount = userProfiles.Count();
                    userProfiles = userProfiles.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                  .Take(searchCriteria.PageSize);
                    return userProfiles.ToList().Select(u => new UserProfile
                    {
                        Id = u.Id,
                        LocalName = u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        Email = u.Email,
                        Category = u.Category,
                        IdentityId = u.IdentityId,
                        IsActive = u.IsActive,
                        OrgUnits = u.OrgUnits,
                        UserName = u.UserName,
                        Permissions = u.Permissions,
                        UserNationalId = u.UserNationalId,
                        PhoneNumber = u.PhoneNumber,
                        Gender = u.Gender,
                        Rooms = u.Rooms,
                        MainOrgUnitId = u.MainOrgUnitId,
                        TransactionProcessingPeriod = u.TransactionProcessingPeriod,
                        Title = u.Title,
                        TitleId = u.TitleId,
                        IsDeleted = u.IsDeleted,
                        //Group = u.Group,
                        IsManager = u.IsManager,
                        //GroupId = u.GroupId,
                        ExternalId = u.ExternalId,
                        PendingRegestration = u.PendingRegestration,

                    }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IQueryable<UserProfile> UserProfileWhereQuery(IQueryable<UserProfile> source, string columnName, string propertyValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(s => s.UserName.ToString().ToLower().Contains(propertyValue)).AsQueryable();
                case FilterType.EndsWidth:
                    return source.Where(s => s.UserName.ToString().ToLower().EndsWith(propertyValue)).AsQueryable();
                case FilterType.StartsWith:
                    return source.Where(s => s.UserName.ToString().ToLower().StartsWith(propertyValue)).AsQueryable();
            }

            return source.Where(s => s.UserName.ToString().ToLower().Contains(propertyValue.ToLower()) || s.LocalizationIdentifier.Localizations.Where(l => l.CultureId == (int)CultureType.Arabic).FirstOrDefault().Text.Contains(propertyValue)).AsQueryable();
        }
        public List<UserProfile> GetUsersByOrgUnitId(int orgUnitId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserProfile> userProfiles = (from orgUnit in _oMCSDbContext.OrgUnits
                                                        where orgUnit.Id == orgUnitId && !orgUnit.IsDeleted && orgUnit.IsActive
                                                        select orgUnit).FirstOrDefault().Users.AsQueryable();
                rowsCount = 0;
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {
                        foreach (Filter filter in searchCriteria.Filters)
                        {
                            if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(filter.ColumnName).PropertyType))
                            {
                                userProfiles = SortByText(userProfiles, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            else
                            {
                                userProfiles = WhereQuery(userProfiles, filter.ColumnName, filter.Value, filter.Type);
                            }
                        }
                    }
                    if (searchCriteria.OrderBy != null)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(UserProfile).GetProperty(searchCriteria.OrderBy).PropertyType))
                        {
                            userProfiles = OrderByText(userProfiles, searchCriteria.CultureName, searchCriteria.Ascending);
                        }
                        else
                        {
                            userProfiles = OrderQuery(userProfiles, searchCriteria.OrderBy, searchCriteria.Ascending);
                        }
                    }
                    rowsCount = userProfiles.Count();
                    userProfiles = userProfiles.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                  .Take(searchCriteria.PageSize);
                    return userProfiles.ToList().Select(u => new UserProfile
                    {
                        Id = u.Id,
                        UserName = u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                        Email = u.Email,
                        Category = u.Category,
                        IdentityId = u.IdentityId,
                        IsActive = u.IsActive,
                        OrgUnits = u.OrgUnits,
                        Permissions = u.Permissions,
                        UserNationalId = u.UserNationalId,
                        PhoneNumber = u.PhoneNumber,
                        Gender = u.Gender,
                        Rooms = u.Rooms,
                        MainOrgUnitId = u.MainOrgUnitId,
                        TransactionProcessingPeriod = u.TransactionProcessingPeriod,
                        Title = u.Title,
                        TitleId = u.TitleId,
                        IsDeleted = u.IsDeleted,
                        //Group = u.Group,
                        IsManager = u.IsManager,
                        //GroupId = u.GroupId,
                        ExternalId = u.ExternalId
                    }).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetAllUsers(string cultureName, string searchQuery = null, int? entityId = null)
        {
            try
            {
                IList<UserProfile> userProfiles = new List<UserProfile>();
                bool isNumeric = false;
                if (searchQuery != null)
                {
                    isNumeric = int.TryParse(searchQuery, out int n);
                }
                if (isNumeric)
                {
                    int numberToSearch = Convert.ToInt32(searchQuery);
                    userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                    where userProfile.IsDeleted == false & userProfile.Id == numberToSearch
                                    select new
                                    {
                                        Id = userProfile.Id,
                                        LocalizationIdentifier = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                        Org = userProfile.OrgUnits
                                    }).ToList().Select(a => new UserProfile
                                    {
                                        Id = a.Id,
                                        LocalName = a.LocalizationIdentifier,
                                        OrgUnits = a.Org
                                    }).ToList();
                }
                else
                {
                    if (searchQuery != null && entityId != null)
                    {
                        userProfiles = _oMCSDbContext.UserProfiles.Where(up => up.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery) && up.OrgUnits.Any(o => o.Id == entityId))
                            .Select(user => new
                            {
                                user.Id,
                                Name = user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                            }).Take(10).ToList()
                            .Select(p => new UserProfile
                            {
                                Id = p.Id,
                                LocalName = p.Name,
                            }).ToList();
                    }
                    else if (searchQuery == null && entityId != null)
                    {
                        userProfiles = _oMCSDbContext.UserProfiles.Where(up => up.OrgUnits.Any(o => o.Id == entityId))
                          .Select(user => new
                          {
                              user.Id,
                              Name = user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                          }).Take(10).ToList()
                          .Select(p => new UserProfile
                          {
                              Id = p.Id,
                              LocalName = p.Name,
                          }).ToList();
                    }
                    else if (searchQuery != null && entityId == null)
                    {
                        userProfiles = _oMCSDbContext.UserProfiles.Where(up => up.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(searchQuery))
                           .Select(user => new
                           {
                               user.Id,
                               Name = user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                           }).Take(10).ToList()
                          .Select(p => new UserProfile
                          {
                              Id = p.Id,
                              LocalName = p.Name,
                          }).ToList();
                    }
                    else if (searchQuery == null && entityId == null)
                    {
                        userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                        where userProfile.IsDeleted == false
                                        select new
                                        {
                                            Id = userProfile.Id,
                                            LocalizationIdentifier = userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                            Org = userProfile.OrgUnits
                                        }).ToList().Select(a => new UserProfile
                                        {
                                            Id = a.Id,
                                            LocalName = a.LocalizationIdentifier,
                                            OrgUnits = a.Org
                                        }).ToList();
                    }
                }
                return userProfiles;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserPermission> GetUserPermissions(int userId, string cultureName)
        {
            try
            {

                IQueryable<UserPermission> userPermissions = (from userPermission in _oMCSDbContext.UserPermissions
                                                              where userPermission.UserProfileId == userId
                                                              select userPermission);

                return userPermissions.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tray> GetUserTrays(int userId)
        {
            try
            {
                return _oMCSDbContext.UserProfiles.Where(u => u.Id == userId).FirstOrDefault().Category.CategoryTrays.Select(c => c.Tary).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserGroup> GetUserGroup(int userId)
        {
            try
            {
                return _oMCSDbContext.UserGroups.Where(u => u.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserCategory GetUserCategoryByUserId(int userId)
        {
            try
            {
                IQueryable<UserCategory> userCategory = (from userProfile in _oMCSDbContext.UserProfiles
                                                         where userProfile.Id == userId
                                                         select userProfile.Category);
                return userCategory.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByPermissionId(int permissionId, string cultureName)
        {
            try
            {
                IQueryable<UserProfile> users = from userPermissions in _oMCSDbContext.UserPermissions
                                                where userPermissions.PermissionId == permissionId
                                                select userPermissions.UserProfile;
                IList<UserProfile> userProfiles = users.Where(u => u.IsDeleted == false).ToList();
                return userProfiles.ToList().Select(u => new UserProfile
                {
                    Id = u.Id,
                    LocalName = u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByTrayId(int trayId, string cultureName)
        {
            try
            {
                IQueryable<UserProfile> users = from user in _oMCSDbContext.UserProfiles
                                                where user.IsDeleted == false
                                                from userCategory in _oMCSDbContext.UserCategoryTrays
                                                where userCategory.Tary.Id == trayId
                                                where user.Category.Id == userCategory.UserCategory.Id
                                                select user;
                return users.ToList().Select(u => new UserProfile
                {
                    Id = u.Id,
                    LocalName = u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetUsersByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                return (from user in _oMCSDbContext.UserProfiles
                        where
                        user.OrgUnits.Any(o => o.Id == orgUnitId && o.IsActive && !o.IsVirtualUnit && !o.IsDeleted)
                        && user.IsActive && !user.IsDeleted
                        select new
                        {
                            user.Id,
                            user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                            user.UserImage
                        }).ToList().Select(a => new UserProfile
                        {
                            Id = a.Id,
                            LocalName = a.Text,
                            UserImage = a.UserImage
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> SearchUsersByOrgUnitId(int? orgUnitId, string cultureName, string term)
        {
            try
            {
                return (from user in _oMCSDbContext.UserProfiles
                        where
                        ((user.OrgUnits.Any(o => o.Id == orgUnitId && o.IsActive && !o.IsVirtualUnit && !o.IsDeleted))
                        &&
                        (term == null || term.Trim() == "" || user.LocalizationIdentifier.Localizations.Any(l => l.Text.ToLower().Contains(term.ToLower()))))
                        && user.IsActive && !user.IsDeleted
                        select new
                        {
                            user.Id,
                            user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                            user.UserImage
                        })
                        .OrderBy(x => x.Id)
                        .ToList().Select(a => new UserProfile
                        {
                            Id = a.Id,
                            LocalName = a.Text,
                            UserImage = a.UserImage
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetOrgUnitUsers(SearchCriteria searchCriteria, int orgUnitId, string cultureName, out int ItemsCount, bool noExternal = false)
        {
            try
            {
                IQueryable<UserProfile> UserProfiles = (from user in _oMCSDbContext.UserProfiles
                                                        where user.OrgUnits.Any(o => o.Id == orgUnitId &&
                                                                                     o.IsActive &&
                                                                                     !o.IsVirtualUnit &&
                                                                                     !o.IsDeleted &&
                                                                                     (!noExternal || (noExternal && !user.ExternalId.HasValue)))
                                                        select user);

                ItemsCount = UserProfiles.Count();

                if (searchCriteria.Ascending)
                {
                    UserProfiles = UserProfiles.OrderBy(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }
                else
                {
                    UserProfiles = UserProfiles.OrderByDescending(p => p.Id)
                        .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                        .Take(searchCriteria.PageSize);
                }

                return UserProfiles.ToList().Select(a => new UserProfile
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    Category = a.Category,
                    //Group = a.Group,
                    LocalName = a.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                }).ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserProfile> GetChildEntityUsersByOrgUnitId(int orgUnitId, string cultureName)
        {
            try
            {
                var lineage = _oMCSDbContext.OrgUnits.FirstOrDefault(ou => ou.Id == orgUnitId && ou.IsActive && !ou.IsVirtualUnit && !ou.IsDeleted).Lineage;
                return _oMCSDbContext.UserProfiles
                                                         .Where(user => user.OrgUnits.Any(o => o.IsActive
                                                                && !o.IsVirtualUnit
                                                                && !o.IsDeleted
                                                                && o.Lineage.StartsWith(lineage)))
                                                         .Select(u => new
                                                         {
                                                             u.Id,
                                                             u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                         }).ToList().Select(user => new UserProfile
                                                         {
                                                             Id = user.Id,
                                                             LocalName = user.Text
                                                         }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetUserName(int userId, string cultureName)
        {
            try
            {
                return FindBy(u => u.Id == userId).LocalizationIdentifier
                    .Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int AddAssignmentGroup(AssignmentGroup assignmentGroup)
        {
            try
            {
                _oMCSDbContext.AssignmentGroups.Add(assignmentGroup);
                _oMCSDbContext.SaveChanges();
                return assignmentGroup.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<AssignmentGroup> GetUserAssignmentGroups(int userId, string cultureName)
        {
            try
            {
                return (from assignmentGroup in _oMCSDbContext.AssignmentGroups
                        where assignmentGroup.Owner.Id == userId
                        select new
                        {
                            assignmentGroup.Id,
                            assignmentGroup.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                            assignmentGroup.AssignmentGroupDetails
                        }).ToList().Select(a => new AssignmentGroup
                        {
                            Id = a.Id,
                            LocalName = a.Text
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public AssignmentGroup GetAssignmentGroupById(int groupId)
        {
            try
            {
                AssignmentGroup assignmentGroup = (from assGroup in _oMCSDbContext.AssignmentGroups
                                                   where assGroup.Id == groupId
                                                   select assGroup
                                                 ).FirstOrDefault();
                return assignmentGroup;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public AssignmentGroup GetAssignmentGroupById(int groupId, string cultureName)
        {
            try
            {
                IList<AssignmentGroup> assignmentGroups = (from assignmentGroup in _oMCSDbContext.AssignmentGroups
                                                           where assignmentGroup.Id == groupId
                                                           select new
                                                           {
                                                               assignmentGroup.Id,
                                                               assignmentGroup.LocalizationIdentifier,
                                                               assignmentGroup.AssignmentGroupDetails
                                                           }).ToList().Select(a => new AssignmentGroup
                                                           {
                                                               Id = a.Id,
                                                               LocalName = a.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                                               AssignmentGroupDetails = a.AssignmentGroupDetails.Select(g => new AssignmentGroupDetail()
                                                               {
                                                                   Id = g.Id,
                                                                   UserProfile = (g.UserProfile != null) ? new UserProfile()
                                                                   {
                                                                       Id = (g.UserProfile != null) ? g.UserProfile.Id : -1,
                                                                       LocalName = g.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                   } : null,
                                                                   OrgUnit = new OrgUnit()
                                                                   {
                                                                       Id = g.OrgUnit.Id,
                                                                       LocalName = g.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                   }
                                                               }).ToList()
                                                           }).ToList();
                return assignmentGroups.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public UserProfile GetUser(string userName, string password)
        {
            try
            {
                return FindBy(u => u.UserName == userName & u.UserName == password);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserProfile GetUserByUserName(string userName)
        {
            try
            {
                UserProfile userProfile = (from user in _oMCSDbContext.UserProfiles
                                           where user.UserName == userName
                                           select new
                                           {
                                               user.Id,
                                               user.UserName,
                                               user.IsDeleted,
                                               user.ApiKey
                                           }).ToList().Select(a => new UserProfile
                                           {
                                               Id = a.Id,
                                               UserName = a.UserName,
                                               IsDeleted = a.IsDeleted,
                                               ApiKey = a.ApiKey
                                           }).SingleOrDefault();
                return userProfile;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public static UserProfile GetUserByUserNameForWordAddIn(string userName)
        {
            try
            {
                MCSDbContext _oMCSDbContextWordAddIn = new MCSDbContext(string.Empty);


                UserProfile userProfile = (from user in _oMCSDbContextWordAddIn.UserProfiles
                                           where user.UserName == userName
                                           select new
                                           {
                                               user.Id,
                                               user.UserName,
                                               user.IsDeleted
                                           }).ToList().Select(a => new UserProfile
                                           {
                                               Id = a.Id,
                                               UserName = a.UserName,
                                               IsDeleted = a.IsDeleted
                                           }).SingleOrDefault();
                return userProfile;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public UserProfile CheckIfValidUserInfo(string userName, string email)
        {
            try
            {
                return FindBy(u => u.UserName == userName && u.Email == email);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void ActivateUser(UserProfile userProfile)
        {
            try
            {
                UserProfile user = GetUserById(userProfile.Id);
                user.IsActive = userProfile.IsActive;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetAllUsersCount()
        {
            try
            {
                return _oMCSDbContext.UserProfiles.Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetUserLocalNameById(int userId, string cultureName)
        {
            return _oMCSDbContext.UserProfiles
                          .Where(u => u.Id == userId)
                          .Select(u => u.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text).FirstOrDefault();
        }
        #region MobileApi
        public UserProfile GetUserInfo(string userName, string cultureName)
        {
            try
            {
                UserProfile userProfile = (from user in _oMCSDbContext.UserProfiles
                                           where user.UserName == userName &&
                                           user.IsDeleted == false
                                           select new
                                           {
                                               user.Id,
                                               user.UserName,
                                               LocalName = user.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                               OrgUnits = user.OrgUnits.Where(o => o.IsDeleted == false && o.IsActive == true && o.IsVirtualUnit == false),
                                               user.AllowMobile
                                           }).ToList().Select(a => new UserProfile
                                           {
                                               Id = a.Id,
                                               UserName = a.UserName,
                                               LocalName = a.LocalName,
                                               AllowMobile = a.AllowMobile,
                                               OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                               {
                                                   Id = o.Id,
                                                   LocalName = o.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                                   LocalizationIdentifier = o.LocalizationIdentifier
                                               }).ToList()
                                           }).SingleOrDefault();
                return userProfile;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool CheckIfUserHasPermission(int userId, int permissionId)
        {
            return _oMCSDbContext.UserPermissions
                                .Where(up => up.UserProfileId == userId && up.PermissionId == permissionId).Any();
        }
        #endregion
        private IQueryable<UserProfile> SortByText(IQueryable<UserProfile> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from userProfile in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select userProfile);
                case FilterType.EndsWidth:
                    return (from userProfile in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select userProfile);
                case FilterType.StartsWith:
                    return (from userProfile in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select userProfile);
                case FilterType.Equals:
                    return (from userProfile in source.Where(p => p.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select userProfile);
            }
            return source;
        }
        private IQueryable<UserProfile> SortUserCategoryByText(IQueryable<UserProfile> userProfiles, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from userProfile in userProfiles.Where(p => p.Category.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue))
                            select userProfile);
                case FilterType.EndsWidth:
                    return (from userProfile in userProfiles.Where(p => p.Category.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select userProfile);
                case FilterType.StartsWith:
                    return (from userProfile in userProfiles.Where(p => p.Category.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select userProfile);
                case FilterType.Equals:
                    return (from userProfile in userProfiles.Where(p => p.Category.CategoryName.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select userProfile);
            }
            return userProfiles;
        }
        private IQueryable<UserProfile> OrderByText(IQueryable<UserProfile> source, string culureName, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(userProfile => userProfile.LocalizationIdentifier.Localizations
                             .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
            }
            return source.OrderByDescending(userProfile => userProfile.LocalizationIdentifier.Localizations
                         .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
        }
        public void UpdateUserProfile(int userId, string email)
        {
            try
            {
                var userProfile = _oMCSDbContext.UserProfiles.FirstOrDefault(a => a.Id == userId);
                if (userProfile != null)
                {
                    userProfile.Email = email;
                    _oMCSDbContext.Entry(userProfile).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateUserInternalNumber(int userId, string phoneNumber, string internalNumber)
        {
            try
            {
                var userProfile = _oMCSDbContext.UserProfiles.Where(a => a.Id == userId).FirstOrDefault();
                if (userProfile != null)
                {
                    userProfile.PhoneNumber = phoneNumber;
                    userProfile.InternalNumber = internalNumber;
                    _oMCSDbContext.Entry(userProfile).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void UpdateManger(UserProfile userProfile)
        {
            try
            {
                UserProfile user = GetUserById(userProfile.Id);
                user.IsManager = userProfile.IsManager;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckUserNameExists(string userName, string identity, out int? userId)
        {
            try
            {
                UserProfile user = _oMCSDbContext.UserProfiles.Where(u => u.UserName.ToLower() == userName.ToLower() && u.IdentityId == identity && u.IsDeleted).FirstOrDefault();
                if (user != null)
                {
                    userId = user.Id;
                    return true;
                }
                userId = (int?)null;
                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<UserGroup> GetUsersWithGroups()
        {
            try
            {
                return _oMCSDbContext.UserGroups.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<UserProfile> GetUsers()
        {
            try
            {
                return _oMCSDbContext.UserProfiles.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<UserGroup> GetUsersWithGroups(string GroupId)
        {
            try
            {
                if (!string.IsNullOrEmpty(GroupId))
                {
                    var _GroupId = int.Parse(GroupId);
                    return _oMCSDbContext.UserGroups
                        .Join(
                            _oMCSDbContext.UserProfiles,
                            userGroup => userGroup.CreatedBy,
                            adminUser => adminUser.Id,
                            (userGroup, adminUser) => new
                            {
                                AdminUserName = adminUser.UserName,
                                Group = userGroup.Group,
                                Id = userGroup.Id,
                                GroupId = userGroup.Id,
                                User = userGroup.User,
                                CreatedBy = userGroup.CreatedBy,
                                CreatedOn = userGroup.CreatedOn,
                                ModefiedBy = userGroup.ModefiedBy,
                                ModefiedOn = userGroup.ModefiedOn,
                                UserId = userGroup.UserId
                            }
                        )
                        .AsEnumerable()

                        .Where(joinedData => joinedData.User.MainOrgUnitId == _GroupId)
                        .Select(joinedData => new UserGroup
                        {
                            AdminUserName = joinedData.AdminUserName,
                            Group = joinedData.Group,
                            Id = joinedData.Id,
                            GroupId = joinedData.GroupId,
                            User = joinedData.User,
                            CreatedBy = joinedData.CreatedBy,
                            CreatedOn = joinedData.CreatedOn,
                            ModefiedBy = joinedData.ModefiedBy,
                            ModefiedOn = joinedData.ModefiedOn,
                            UserId = joinedData.UserId
                        }).ToList();
                }
                else
                {
                    return _oMCSDbContext.UserGroups
                        .Join(
                            _oMCSDbContext.UserProfiles,
                            userGroup => userGroup.CreatedBy,
                            adminUser => adminUser.Id,
                            (userGroup, adminUser) => new
                            {
                                AdminUserName = adminUser.UserName,
                                Group = userGroup.Group,
                                Id = userGroup.Id,
                                GroupId = userGroup.Id,
                                User = userGroup.User,
                                CreatedBy = userGroup.CreatedBy,
                                CreatedOn = userGroup.CreatedOn,
                                ModefiedBy = userGroup.ModefiedBy,
                                ModefiedOn = userGroup.ModefiedOn,
                                UserId = userGroup.UserId
                            }
                        )
                        .AsEnumerable()
                        .Select(joinedData => new UserGroup
                        {
                            AdminUserName = joinedData.AdminUserName,
                            Group = joinedData.Group,
                            Id = joinedData.Id,
                            GroupId = joinedData.GroupId,
                            User = joinedData.User,
                            CreatedBy = joinedData.CreatedBy,
                            CreatedOn = joinedData.CreatedOn,
                            ModefiedBy = joinedData.ModefiedBy,
                            ModefiedOn = joinedData.ModefiedOn,
                            UserId = joinedData.UserId
                        })
                        .ToList();

                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Permission> GetUserPrivileges(int userId, string currentUserIdentity, string cultureName)
        {
            try
            {
                // here
                IList<UserProfile> userProfiles = (from userProfile in _oMCSDbContext.UserProfiles
                                                   where userProfile.IdentityId == currentUserIdentity &&
                                                   userProfile.IsActive == true &&
                                                   userProfile.IsDeleted == false
                                                   select new
                                                   {
                                                       // userProfile.Id,
                                                       // OrgUnits = userProfile.OrgUnits.Where(o => o.IsDeleted == false),
                                                       userProfile.UserGroups,
                                                       // userProfile.Category,
                                                       // userProfile.UserGroups,
                                                       // userProfile.MainOrgUnitId
                                                   }).ToList().Select(a => new UserProfile
                                                   {
                                                       // Id = a.Id,
                                                       UserGroups = a.UserGroups,
                                                       // OrgUnits = a.OrgUnits.Select(o => new OrgUnit
                                                       // {
                                                       //    Id = o.Id,
                                                       // }).ToList(),
                                                       // Category = new UserCategory
                                                       // {
                                                       //    Id = a.Category.Id,
                                                       // },
                                                       // UserGroups = a.UserGroups,
                                                       // MainOrgUnitId = a.MainOrgUnitId
                                                   }).ToList();
                List<IList<UserGroup>> userGroupLists = userProfiles.Select(a => a.UserGroups).ToList();
                List<Permission> permissionList = new List<Permission>();

                foreach (List<UserGroup> userGroupList in userGroupLists)
                {
                    foreach (UserGroup userGroup in userGroupList)
                    {
                        foreach (Permission permission in userGroup.Group.Permissions)
                        {
                            permissionList.Add(permission);
                        }
                    }
                }

                return permissionList;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void AddUserGroup(int userid, int groupId)
        {
            try
            {
                var oldUserGroup = _oMCSDbContext.UserGroups.Where(ug => ug.UserId == userid && ug.GroupId == groupId).FirstOrDefault();
                if (oldUserGroup == null)
                {
                    UserGroup userGroup = new UserGroup
                    {
                        GroupId = groupId,
                        UserId = userid,

                    };
                    _oMCSDbContext.UserGroups.Add(userGroup);
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void RemoveUserGroup(int userid, int groupId)
        {
            try
            {
                var oldUserGroup = _oMCSDbContext.UserGroups.Where(ug => ug.UserId == userid && ug.GroupId == groupId).FirstOrDefault();
                if (oldUserGroup != null)
                {

                    _oMCSDbContext.UserGroups.Remove(oldUserGroup);
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UserLoginAction(int userID)
        {
            try
            {
                var userProfile = _oMCSDbContext.UserProfiles.Where(user => user.Id == userID).FirstOrDefault();
                if (userProfile != null)
                {
                    userProfile.LoginTime = DateTime.Now;
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UserLogoutAction(string userID)
        {
            try
            {
                var userProfile = _oMCSDbContext.UserProfiles.Where(user => user.IdentityId == userID).FirstOrDefault();
                if (userProfile != null)
                {
                    userProfile.LastLogout = DateTime.Now;
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods       
    }
}
