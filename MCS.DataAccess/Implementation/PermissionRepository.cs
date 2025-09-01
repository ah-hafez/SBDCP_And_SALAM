using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
namespace MCS.DataAccess
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        #region Attributes
        #endregion Attributes
        #region Constructors
        public PermissionRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
        }
        #endregion Constructors
        #region Methods
        public void DeletePermission(int permissionId)
        {
            try
            {
                {
                    //TODO:To Review Code to Delete Cascasde For Lookup Using DataAccess Mapping .
                    Permission permission = _oMCSDbContext.Permissions.Where(p => p.Id == permissionId).FirstOrDefault();
                    if (permission != null)
                    {
                        List<LookupLocalization> lookupLocalizations = new List<LookupLocalization>();
                        lookupLocalizations.AddRange(permission.Name.Localizations);
                        foreach (LookupLocalization lookupLocalization in lookupLocalizations)
                        {
                            _oMCSDbContext.LookupLocalizations.Remove(lookupLocalization);
                        }
                        _oMCSDbContext.Lookups.Remove(permission.Name);
                        _oMCSDbContext.Permissions.Remove(permission);
                        _oMCSDbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void AddPermission(Permission permission)
        {
            try
            {
                _oMCSDbContext.Permissions.Add(permission);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdatePermission(Permission permission)
        {
            try
            {
                Permission permissionOld = GetPermissionById(permission.Id);
                if (permissionOld != null)
                {
                    foreach (LookupLocalization lookupLocalization in permission.Name.Localizations)
                    {
                        LookupLocalization currentlocalization = permissionOld.Name.Localizations
                                                                             .Where(l => l.Id == lookupLocalization.Id)
                                                                             .FirstOrDefault();
                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(lookupLocalization);
                        }
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdatePermissions(IList<Permission> permissions)
        {
            try
            {
                foreach (Permission permission in permissions)
                {
                    UpdatePermission(permission);
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Permission> GetPermissions(string cultureName)
        {
            try
            {
                IList<Permission> permissions = (from permission in _oMCSDbContext.Permissions
                                                 select new
                                                 {
                                                     permission.Id,
                                                     PName = permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                     permission.Code,
                                                     PermissionGroups = permission.PermissionGroups.Select(p =>
                                                     new
                                                     {
                                                         p.Id,
                                                         p.IsUserDefined,
                                                         p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     })
                                                 }).ToList().Select(p => new Permission
                                                 {
                                                     Id = p.Id,
                                                     LocalName = p.PName,
                                                     Code = p.Code,
                                                     PermissionGroups = p.PermissionGroups.Select(g => new Group()
                                                     {
                                                         Id = g.Id,
                                                         IsUserDefined = g.IsUserDefined,
                                                         Name = g.Text
                                                     }).ToList()
                                                 }).ToList();
                return permissions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Permission> GetPermissions(Expression<Func<Permission, bool>> @where, string cultureName)
        {
            try
            {
                IList<Permission> permissions = (from permission in _oMCSDbContext.Permissions.Where(@where)
                                                 select new
                                                 {
                                                     permission.Id,
                                                     PName = permission.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                     permission.Code,
                                                     PermissionGroups = permission.PermissionGroups.Select(p =>
                                                     new
                                                     {
                                                         p.Id,
                                                         p.IsUserDefined,
                                                         p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                     })
                                                 }).ToList().Select(p => new Permission
                                                 {
                                                     Id = p.Id,
                                                     LocalName = p.PName,
                                                     Code = p.Code,
                                                     PermissionGroups = p.PermissionGroups.Select(g => new Group()
                                                     {
                                                         Id = g.Id,
                                                         IsUserDefined = g.IsUserDefined,
                                                         Name = g.Text
                                                     }).ToList()
                                                 }).ToList();
                return permissions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Permission> GetPermissions(Expression<Func<Permission, bool>> @where)
        {
            try
            {
                return _oMCSDbContext.Permissions.Where(@where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Permission> GetUserPermissionsByGroupId(int groupId, int userId, string cultureName)
        {
            try
            {
                UserProfile userProfile = _oMCSDbContext.UserProfiles.Where(b => b.Id == userId).FirstOrDefault();
                IList<Permission> groupPermission = new List<Permission>();
                List<Permission> groupUserPermission = new List<Permission>();
                IList<Permission> permissionss = _oMCSDbContext.Permissions.ToList();
                foreach (var item in permissionss)
                {
                    IList<Group> PermissionGroups = item.PermissionGroups;
                    foreach (var group in PermissionGroups)
                    {
                        if (group.Id == groupId)
                        {
                            groupPermission = group.Permissions;
                        }
                        //else if (group.Id == userProfile.GroupId)
                        //{
                        //    groupUserPermission = group.Permissions;
                        //}



                        else
                        {
                            foreach (UserGroup userGroup in userProfile.UserGroups)
                            {
                                if (userGroup.GroupId == group.Id)
                                {
                                    groupUserPermission.AddRange(userGroup.Group.Permissions);
                                }
                            }
                        }



                    }
                    continue;
                }




                groupUserPermission = groupUserPermission.Distinct().ToList();



                IList<Permission> Group1 = groupPermission;
                IList<Permission> Group2 = groupUserPermission;



                IList<Permission> permissions = (from permissionGroup1 in Group1
                                                 join permissionGroup2 in Group2
                                                 on permissionGroup1.Id equals permissionGroup2.Id
                                                 select new
                                                 {
                                                     permissionGroup1.Id,
                                                     PName = permissionGroup1.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                     permissionGroup1.Code,
                                                     PermissionGroups = permissionGroup1.PermissionGroups.Select(p =>
                                                     new
                                                     {
                                                         p.Id,
                                                         p.IsUserDefined,
                                                         p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                                                     })
                                                 }).ToList().Select(p => new Permission
                                                 {
                                                     Id = p.Id,
                                                     LocalName = p.PName,
                                                     Code = p.Code,
                                                     PermissionGroups = p.PermissionGroups.Select(g => new Group()
                                                     {
                                                         Id = g.Id,
                                                         IsUserDefined = g.IsUserDefined,
                                                         Name = g.Text
                                                     }).ToList()
                                                 }).ToList();
                return permissions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Group> GetPermissionsGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Group> permissionGroups = (from permissionGroup in _oMCSDbContext.Groups
                                                      where permissionGroup.IsUserDefined == false
                                                      select permissionGroup);
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Group).GetProperty(filter.ColumnName).PropertyType))
                        {
                            permissionGroups = SortByText(permissionGroups, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }
                rowsCount = permissionGroups.Count();
                if (searchCriteria.Ascending)
                {
                    permissionGroups = permissionGroups.OrderBy(p => p.GroupName.Localizations
                                      .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                      .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                      .Take(searchCriteria.PageSize);
                }
                else
                {
                    permissionGroups = permissionGroups.OrderByDescending(p => p.GroupName.Localizations
                                      .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                      .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                      .Take(searchCriteria.PageSize);
                }
                return permissionGroups.ToList().Select(g => new Group
                {
                    Id = g.Id,
                    Name = g.GroupName.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    Permissions = g.Permissions.Select(p => new Permission { Id = p.Id, IsUserDefined = p.IsUserDefined, Name = p.Name }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Group> GetPermissionsGroups_IAM(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Group> permissionGroups = (from permissionGroup in _oMCSDbContext.Groups
                                                      select permissionGroup);
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Group).GetProperty(filter.ColumnName).PropertyType))
                        {
                            permissionGroups = SortByText(permissionGroups, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }
                rowsCount = permissionGroups.Count();
                if (searchCriteria.Ascending)
                {
                    permissionGroups = permissionGroups.OrderBy(p => p.GroupName.Localizations
                                      .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                      .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                      .Take(searchCriteria.PageSize);
                }
                else
                {
                    permissionGroups = permissionGroups.OrderByDescending(p => p.GroupName.Localizations
                                      .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text)
                                      .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                      .Take(searchCriteria.PageSize);
                }
                return permissionGroups.ToList().Select(g => new Group
                {
                    Id = g.Id,
                    Name = g.GroupName.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                    Permissions = g.Permissions.Select(p => new Permission { Id = p.Id, IsUserDefined = p.IsUserDefined, Name = p.Name }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public Group GetPermissionsGroupById(int permissionGroupId)
        {
            try
            {
                return _oMCSDbContext.Groups.Where(p => p.Id == permissionGroupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Group GetPermissionsGroupById(int permissionGroupId, string cultureName)
        {
            try
            {
                IList<Group> group = (from permissionGroup in _oMCSDbContext.Groups
                                      where permissionGroup.Id == permissionGroupId
                                      select new
                                      {
                                          permissionGroup.Id,
                                          permissionGroup.IsUserDefined,
                                          permissionGroup.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                          Permissions = permissionGroup.Permissions
                                                                       .Select(m =>
                                                                        new
                                                                        {
                                                                            m.Id,
                                                                            m.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                            m.IsUserDefined,
                                                                        })
                                      }).ToList().Select(g => new Group
                                      {
                                          Id = g.Id,
                                          IsUserDefined = g.IsUserDefined,
                                          Name = g.Text,
                                          Permissions = g.Permissions.Select(p => new Permission
                                          {
                                              Id = p.Id,
                                              LocalName = p.Text,
                                              IsUserDefined = p.IsUserDefined
                                          }).ToList()
                                      }).ToList();
                return group.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public IList<Group> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups)
        {
            try
            {
                IQueryable<Group> permissionGroups = (from permissionGroup in _oMCSDbContext.Groups
                                                      where permissionGroup.IsActive
                                                      select permissionGroup);
                if (!includeUserDefinedGroups)
                {
                    permissionGroups = permissionGroups.Where(g => g.IsUserDefined == false);
                }
                return permissionGroups
                                          .Select(a => new
                                          {
                                              GroupId = a.Id,
                                              GroupName = a.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                              a.IsUserDefined,
                                              Permissions = a.Permissions.Select(m =>
                                                            new
                                                            {
                                                                m.Id,
                                                                m.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                                                m.IsUserDefined,
                                                                m.Code,
                                                                m.PermissionGroups
                                                            }
                                                            )
                                          }).ToList().Select(g => new Group
                                          {
                                              Id = g.GroupId,
                                              Name = g.GroupName,
                                              IsUserDefined = g.IsUserDefined,
                                              Permissions = g.Permissions.Select(p => new Permission()
                                              {
                                                  Id = p.Id,
                                                  PermissionGroups = p.PermissionGroups,
                                                  IsUserDefined = p.IsUserDefined,
                                                  LocalName = p.Text,
                                                  Code = p.Code
                                              }).ToList()
                                          }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Group> GetAllUserDefinedGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Group> permissionGroups = (from permissionGroup in _oMCSDbContext.Groups
                                                      where permissionGroup.IsUserDefined == true
                                                      select permissionGroup
                                                      )
                                                      .AsQueryable();
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (filter.ColumnName == "RoleName")
                        {
                            permissionGroups = FilterByRoleName(permissionGroups, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Group).GetProperty(filter.ColumnName).PropertyType))
                        {
                            permissionGroups = SortByText(permissionGroups, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }
                rowsCount = permissionGroups.Count();
                if (searchCriteria.OrderBy != null)
                {
                    if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Group).GetProperty(searchCriteria.OrderBy).PropertyType))
                    {
                        permissionGroups = OrderByGroupText(permissionGroups, searchCriteria.CultureName, searchCriteria.Ascending);
                    }
                    else
                    {
                        permissionGroups = OrderByGroupId(permissionGroups, searchCriteria.Ascending);
                    }
                }
                permissionGroups = permissionGroups.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                              .Take(searchCriteria.PageSize);
                return permissionGroups.ToList().Select(p => new Group
                {
                    Id = p.Id,
                    Name = p.GroupName.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text,
                    IsActive = p.IsActive
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Permission GetPermissionById(int permissionId)
        {
            try
            {
                return FindBy(p => p.Id == permissionId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Group GetPermissionsByGroupId(int groupId, string CultureName)
        {
            try
            {
                var group = _oMCSDbContext.Groups.FirstOrDefault(p => p.Id == groupId);

                return new Group
                {
                    Id = group.Id,
                    Name = group?.GroupName?.Localizations?.Where(l => l.Culture.ShortName == CultureName)?.FirstOrDefault()?.Text,
                    IsActive = group.IsActive,
                    Permissions = group.Permissions.Select(p =>
                    {
                        return new Permission
                        {
                            LocalName = p?.Name?.Localizations?.Where(l => l.Culture.ShortName == CultureName)?.FirstOrDefault()?.Text,
                            Id = p.Id,
                            IsUserDefined = p.IsUserDefined
                        };
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Permission GetPermissionByCode(string permissionCode)
        {
            try
            {
                return FindBy(p => p.Code == permissionCode);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int AddGroup(Group group)
        {
            try
            {
                if (CheckIfGroupExist(group))
                {
                    throw new DataAccessException("ThisRoleIsAlreadyExist");
                }
                _oMCSDbContext.Groups.Add(group);
                _oMCSDbContext.SaveChanges();
                return group.Id;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message);
            }
        }
        public void UpdateGroup(Group group)
        {
            try
            {
                Group groupOld = GetPermissionsGroupById(group.Id);
                group.IsActive = groupOld.IsActive;

                if (groupOld != null)
                {
                    _oMCSDbContext.Entry(groupOld).CurrentValues.SetValues(group);

                    groupOld.Permissions.ToList().ForEach(p =>
                         groupOld.Permissions.Remove(p));

                    groupOld.Permissions = group.Permissions;

                    foreach (LookupLocalization name in group.GroupName.Localizations)
                    {
                        if (CheckIfGroupExist(group))
                        {
                            throw new DataAccessException("ThisRoleIsAlreadyExist");
                        }
                        LookupLocalization currentlocalization = groupOld.GroupName.Localizations
                                                                        .Where(l => l.Id == name.Id)
                                                                        .FirstOrDefault();
                        if (currentlocalization != null)
                        {
                            _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(name);
                        }
                    }

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex.Message);
            }
        }
        public void DeleteGroup(int id)
        {
            try
            {
                Group group = _oMCSDbContext.Groups.Where(p => p.Id == id).FirstOrDefault();
                List<int> DeletedGroupPermissionsIds = group.Permissions.Select(p => p.Id).ToList();
                //List<UserPermission> userPermissions = _oMCSDbContext.UserPermissions.Where(up => DeletedGroupPermissionsIds.Contains(up.PermissionId)).ToList();



                if (group != null)
                {
                    // TODO: check if no one use this role 
                    if (!CheckIfNoOneUseThisRole(id))
                    {
                        throw new DataAccessException("CanNotRemoveRoleThereIsUsersUseThisRole");
                    }
                    _oMCSDbContext.Groups.Remove(group);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Group ActivateDeactivateRole(int RoleId, string CultureName)
        {
            try
            {
                Group group = _oMCSDbContext.Groups.FirstOrDefault(g => g.IsUserDefined == true && g.Id == RoleId);

                if (group != null)
                {
                    // TODO: check if no one use this role 
                    if (!CheckIfNoOneUseThisRole(RoleId))
                    {
                        throw new DataAccessException("CanNotDeactivateRoleThereAreUsersUseThisRole");
                    }
                    group.IsActive = !group.IsActive;
                    _oMCSDbContext.SaveChanges();

                    group.Name = group.GroupName.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text;
                    return group;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new DataAccessException(ex.Message);
            }
        }

        private bool CheckIfNoOneUseThisRole(int RoleId)
        {
            return true;
        }
        private IQueryable<Group> SortByText(IQueryable<Group> source, string textValue, FilterType filterType, string cultureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from permissionGroup
                            in _oMCSDbContext.Groups.Where(p => p.GroupName.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select permissionGroup);
                case FilterType.EndsWidth:
                    return (from permissionGroup
                            in _oMCSDbContext.Groups.Where(p => p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.EndsWith(textValue))
                            select permissionGroup);
                case FilterType.StartsWith:
                    return (from permissionGroup
                            in _oMCSDbContext.Groups.Where(p => p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.StartsWith(textValue))
                            select permissionGroup);
                case FilterType.Equals:
                    return (from permissionGroup
                            in _oMCSDbContext.Groups.Where(p => p.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Equals(textValue))
                            select permissionGroup);
            }
            return source;
        }
        private IQueryable<Group> FilterByRoleName(IQueryable<Group> source, string textValue, FilterType filterType, string cultureName)
        {
            if (textValue == null)
            {
                return source;
            }
            return source.Where(s => s.GroupName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text.Contains(textValue.Trim()));
        }

        private IQueryable<Group> OrderByGroupText(IQueryable<Group> source, string culureName, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(group => group.GroupName.Localizations
                             .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
            }
            return source.OrderByDescending(group => group.GroupName.Localizations
                         .Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text);
        }
        private IQueryable<Group> OrderByGroupId(IQueryable<Group> source, bool isAscending)
        {
            if (isAscending)
            {
                return source.OrderBy(group => group.Id);
            }
            return source.OrderByDescending(group => group.Id);
        }
        public IList<TransactionPathDetails> GetTransactionPathUsersPermissions(int transactionPathId, int permissionId)
        {
            try
            {

                //var userPermissions = (from permission in _oMCSDbContext.Permissions
                //                       join userPermission in _oMCSDbContext.UserProfiles on permission.Id equals userPermission.Group.Id
                //                       join pathDetails in _oMCSDbContext.TransactionPathDetails on userPermission.Id equals pathDetails.UserId
                //                       where (pathDetails.TransactionPathId == transactionPathId
                //                              && permission.Id == permissionId)
                //                       select new
                //                       {
                //                           permission.Id,
                //                           pathDetails.UserId
                //                       }).ToList().Select(r => new TransactionPathDetails
                //                       {
                //                           Id = r.Id,
                //                           UserId = r.UserId
                //                       }).ToList();
                //return userPermissions;
                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods
        private bool CheckIfGroupExist(Group group)
        {
            try
            {
                var groups = _oMCSDbContext.Groups.Where(g => g.Id != group.Id).ToList();
                foreach (var item in groups)
                {
                    var NameAr = item.GroupName.Localizations.Where(l => l.Culture.Id == (int)CultureType.Arabic).FirstOrDefault().Text;
                    var AddGroupNameAr = group.GroupName.Localizations.Where(l => l.Culture.Id == (int)CultureType.Arabic).FirstOrDefault().Text;
                    var NameEn = item.GroupName.Localizations.Where(l => l.Culture.Id == (int)CultureType.English).FirstOrDefault().Text;
                    var AddGroupNameEn = group.GroupName.Localizations.Where(l => l.Culture.Id == (int)CultureType.English).FirstOrDefault().Text;

                    if (NameAr.CompareTo(AddGroupNameAr) == 0 || NameEn.CompareTo(AddGroupNameEn) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IList<UserGroup> GetUsersGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<UserGroup> userGroups = (from userGroup in _oMCSDbContext.UserGroups

                                                    select userGroup);

                rowsCount = userGroups.Count();

                userGroups = userGroups.OrderBy(p => p.UserId)
                                  .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                  .Take(searchCriteria.PageSize);


                return userGroups.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Group> GetAllGroups(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Group> groups = (from groupRole in _oMCSDbContext.Groups

                                                    select groupRole);

                rowsCount = groups.Count();

                groups = groups.OrderBy(p => p.Id)
                                  .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                  .Take(searchCriteria.PageSize);


                return groups.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


    }
}
