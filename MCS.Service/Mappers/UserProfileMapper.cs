using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Localization.SupportClasses;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;
using MCS.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace MCS.Service.Mappers
{
    public static class UserProfileMapper
    {
        public static UserProfile Map(AddUserProfileDTO oAddUserProfileDTO)
        {
            if (oAddUserProfileDTO == null)
            {
                return null;
            }

            UserProfile userProfile = new UserProfile()
            {
                IsActive = true,
                Email = string.IsNullOrEmpty(oAddUserProfileDTO.Email) ? "" : oAddUserProfileDTO.Email,
                PhoneNumber = oAddUserProfileDTO.PhoneNumber,
                TransactionProcessingPeriod = oAddUserProfileDTO.TransactionProcessingPeriod,
                UserName = string.IsNullOrEmpty(oAddUserProfileDTO.UserName) ? "" : oAddUserProfileDTO.UserName,
                TitleId = oAddUserProfileDTO.TitleId,
                Permissions = UserPermissionMapper.Map(oAddUserProfileDTO.Permissions, oAddUserProfileDTO.RoleId),
                CategoryId = oAddUserProfileDTO.CategoryId,
                LocalizationIdentifier = oAddUserProfileDTO.Names != null ? LocalizationIdentifierMapper.Map(oAddUserProfileDTO.Names) : null,
                OrgUnits = MapOrgUnits(oAddUserProfileDTO.OrgUnits),
                UserNationalId = oAddUserProfileDTO.UserNationalId,
                MainOrgUnitId = oAddUserProfileDTO.MainOrgUnitId,
                Gender = oAddUserProfileDTO.Gender,
                //GroupId = oAddUserProfileDTO.RoleId,
                IsManager = oAddUserProfileDTO.IsManager,
                Password = oAddUserProfileDTO.Password,
                UserGroups = MapGroupList(oAddUserProfileDTO.UserGroups, oAddUserProfileDTO.Id),
                PendingRegestration = oAddUserProfileDTO.PendingRegestration,
                AllowMobile = oAddUserProfileDTO.AllowMobile,
                InternalNumber = oAddUserProfileDTO.InternalNumber,
                ApiKey = oAddUserProfileDTO.ApiKey,
                UserMobileClassId = oAddUserProfileDTO.UserMobileClassId,
            };

            return userProfile;
        }

        public static UserProfile Map(EditUserProfileDTO userProfileEditDTO)
        {
            if (userProfileEditDTO == null)
            {
                return null;
            }

            UserProfile userProfile = new UserProfile()
            {
                IsActive = true,
                Email = userProfileEditDTO.Email,
                PhoneNumber = userProfileEditDTO.PhoneNumber,
                TransactionProcessingPeriod = userProfileEditDTO.TransactionProcessingPeriod,
                UserName = userProfileEditDTO.UserName,
                TitleId = userProfileEditDTO.TitleId,
                CategoryId = userProfileEditDTO.CategoryId,
                LocalizationIdentifier = userProfileEditDTO.Names != null ? LocalizationIdentifierMapper.Map(userProfileEditDTO.Names) : null,
                Id = userProfileEditDTO.Id,
                Permissions = UserPermissionMapper.Map(userProfileEditDTO.Permissions, userProfileEditDTO.RoleId),
                OrgUnits = MapOrgUnits(userProfileEditDTO.OrgUnits),
                UserNationalId = userProfileEditDTO.UserNationalId,
                Gender = userProfileEditDTO.Gender,
                IsManager = userProfileEditDTO.IsManager,
                //GroupId = userProfileEditDTO.RoleId,
                MainOrgUnitId = userProfileEditDTO.MainOrgUnitId,
                Password = userProfileEditDTO.Password,
                ExternalId = userProfileEditDTO.ExternalId,
                UserGroups = MapGroupList(userProfileEditDTO.UserGroupsData, userProfileEditDTO.Id),
                AllowMobile = userProfileEditDTO.AllowMobile,
                InternalNumber = userProfileEditDTO.InternalNumber,
                ApiKey = userProfileEditDTO.ApiKey,
                UserMobileClassId = userProfileEditDTO.UserMobileClassId,
            };

            return userProfile;
        }
        public static UserProfile IAMMap(EditUserProfileDTO userProfileEditDTO)
        {
            if (userProfileEditDTO == null)
            {
                return null;
            }

            UserProfile userProfile = new UserProfile()
            {
                IsActive = userProfileEditDTO.IsActive,
                Email = userProfileEditDTO.Email,
                PhoneNumber = userProfileEditDTO.PhoneNumber,
                TransactionProcessingPeriod = userProfileEditDTO.TransactionProcessingPeriod,
                UserName = userProfileEditDTO.UserName,
                TitleId = userProfileEditDTO.TitleId,
                CategoryId = userProfileEditDTO.CategoryId,
                LocalizationIdentifier = userProfileEditDTO.Names != null ? LocalizationIdentifierMapper.Map(userProfileEditDTO.Names) : null,
                Id = userProfileEditDTO.Id,
                Permissions = UserPermissionMapper.Map(userProfileEditDTO.Permissions, userProfileEditDTO.RoleId),
                OrgUnits = MapOrgUnits(userProfileEditDTO.OrgUnits),
                UserNationalId = userProfileEditDTO.UserNationalId,
                Gender = userProfileEditDTO.Gender,
                IsManager = userProfileEditDTO.IsManager,
                //GroupId = userProfileEditDTO.RoleId,
                MainOrgUnitId = userProfileEditDTO.MainOrgUnitId,
                Password = userProfileEditDTO.Password,
                ExternalId = userProfileEditDTO.ExternalId,
                UserGroups = MapGroupList(userProfileEditDTO.UserGroupsData, userProfileEditDTO.Id),
                AllowMobile = userProfileEditDTO.AllowMobile,
                InternalNumber = userProfileEditDTO.InternalNumber,
                ApiKey = userProfileEditDTO.ApiKey,
                UserMobileClassId = userProfileEditDTO.UserMobileClassId,
            };

            return userProfile;
        }

        public static EditUserProfileDTO Map(UserProfile userProfile, string culture = "ar")
        {
            if (userProfile == null)
            {
                return null;
            }

            EditUserProfileDTO userProfileEditDTO = new EditUserProfileDTO()
            {
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
                TransactionProcessingPeriod = userProfile.TransactionProcessingPeriod,
                UserName = userProfile.UserName,
                Id = userProfile.Id,
                CategoryId = userProfile.Category == null ? 0 : userProfile.Category.Id,
                IdentifierId = userProfile.LocalizationIdentifier.Id,
                TitleId = userProfile.Title == null ? 0 : userProfile.Title.Id,
                TitleName = userProfile.Title == null ? string.Empty : userProfile.Title.Localizations.Where(l => l.Culture.Id == 1).LocalText(),
                Names = LocalizationIdentifierMapper.Map(userProfile.LocalizationIdentifier.Localizations),
                OrgUnits = userProfile.OrgUnits.Select(o => o.Id).ToList(),
                UserGroups = MapGroup(userProfile.UserGroups),
                OrgUnitList = OrgUnitMapper.Map(userProfile.OrgUnits, culture),
                UserNationalId = userProfile.UserNationalId,
                Gender = userProfile.Gender,
                //RoleId = userProfile.GroupId,
                //Permissions = userProfile.Group == null ? new List<int>() : userProfile.Group.Permissions.Select(l => l.Id).ToList(),
                //RoleName = userProfile.Group == null ? string.Empty : userProfile.Group.GroupName.Localizations.Where(l=>l.Culture.Id ==1).LocalText(),
                IsManager = userProfile.IsManager,
                MainOrgUnitId = userProfile.MainOrgUnitId,
                MainOrgUnitName = userProfile.OrgUnits.Where(a => a.Id == userProfile.MainOrgUnitId).Select(o => o.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == "ar").FirstOrDefault()).LocalText(),
                UserImageId = userProfile.UserImage == null ? 0 : userProfile.UserImage.Id,
                ExternalId = userProfile.ExternalId,
                AllowMobile = userProfile.AllowMobile,
                InternalNumber = userProfile.InternalNumber,
                ApiKey = userProfile.ApiKey,
                UserMobileClassId = userProfile.UserMobileClassId,
                UserMobileClassName = userProfile.UserMobileClassId.HasValue ? userProfile.UserMobileClass.Localizations.Where(l => l.Culture.Id == 1).LocalText() : string.Empty,

            };


            return userProfileEditDTO;
        }


        public static List<UserGroupDTO> MapGroup(IList<UserGroup> userGroups)
        {

            if (userGroups == null || !userGroups.Any())
            {
                return null;
            }
            List<UserGroupDTO> userGroupDTOs = userGroups
                .Select(userGroupItem => new UserGroupDTO()
                {
                    GroupId = userGroupItem.GroupId,
                    UserId = userGroupItem.GroupId,
                    UserName = userGroupItem.Group.GroupName.Localizations.Where(l => l.Culture.Id == 1).LocalText()
                }).ToList();

            return userGroupDTOs;
        }
        public static List<UserProfileDTO> Map(IList<UserProfile> userProfiles, string CultureName = null)
        {
            if (userProfiles == null || !userProfiles.Any())
            {
                return null;
            }
            IOrgUnitBL orgUnitBL = new OrgUnitBL();

            List<UserProfileDTO> usersProfileDTOs = userProfiles
                .Select(userProfileItem => new UserProfileDTO()
                {
                    Email = userProfileItem.Email != null ? userProfileItem.Email : string.Empty,
                    Id = userProfileItem.Id,
                    LocalName = userProfileItem.LocalName,
                    IsActive = userProfileItem.IsActive,
                    IsEmailConfirmed = userProfileItem.IdentityId == null ? false : UserManager.UserManagerProvider.CheckUserEmailConfirmed(userProfileItem.IdentityId),
                    Names = userProfileItem.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(userProfileItem.LocalizationIdentifier.Localizations) : null,
                    OrgUnits = userProfileItem.OrgUnits != null ? userProfileItem.OrgUnits.Select(o => (int?)o.Id).ToList() : null,
                    Category = userProfileItem.Category != null ? userProfileItem.Category.CategoryName.Localizations.Where(l => l.Culture.Id == 1).LocalText() : null,
                    UserName = userProfileItem.UserName != null ? userProfileItem.UserName : string.Empty,
                    PhoneNumber = userProfileItem.PhoneNumber != null ? userProfileItem.PhoneNumber : string.Empty,
                    OrgUnitsNames = CultureName != null ? userProfileItem.OrgUnits.Select(o => o.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text).ToList() : null,
                    IsManager = userProfileItem.IsManager,
                    //GroupId= userProfileItem.Group != null ? userProfileItem.Group.Id: 0,
                    //RoleName = userProfileItem.Group != null ? userProfileItem.Group.GroupName.Localizations.Where(l=>l.Culture.Id == 1).LocalText(): string.Empty,
                    MainOrgUnitName = userProfileItem.MainOrgUnitId != 0 ? orgUnitBL.GetOrgUnitName(o => o.Id == userProfileItem.MainOrgUnitId, "ar") : string.Empty,
                    UserImageId = userProfileItem.UserImage?.Id,
                    IsDeleted = userProfileItem.IsDeleted,
                    ExternalId = userProfileItem.ExternalId,
                    AllowMobile = userProfileItem.AllowMobile,
                    InternalNumber = userProfileItem.InternalNumber,
                    ApiKey = userProfileItem.ApiKey,
                    TitileId = userProfileItem.TitleId,
                    UserNationalId = userProfileItem.UserNationalId,
                    UserGroups = userProfileItem?.UserGroups != null ? userProfileItem.UserGroups.Select(g => g.GroupId).ToList() :
                    new List<int>(),
                    TransactionProcessingPeriod = userProfileItem.TransactionProcessingPeriod,
                    GenderId = userProfileItem.Gender,
                    LoginTime = userProfileItem.LoginTime,
                    LastLogout = userProfileItem.LastLogout,
                    OrgUnitDTOs = userProfileItem.OrgUnits != null ? userProfileItem.OrgUnits.Select(org => new OrgUnitDTO
                    {
                        Id = org.Id,
                        Name = org.LocalName
                    }).ToList() : new List<OrgUnitDTO>()


                }).ToList();

            return usersProfileDTOs;
        }
        public static List<UserProfileDTO> Map(IList<UserProfile> userProfiles, IList<Lookup> genders, string CultureName = null)
        {
            if (userProfiles == null || !userProfiles.Any())
            {
                return null;
            }
            IOrgUnitBL orgUnitBL = new OrgUnitBL();

            List<UserProfileDTO> usersProfileDTOs = userProfiles
                .Select(userProfileItem => new UserProfileDTO()
                {
                    Email = userProfileItem.Email != null ? userProfileItem.Email : string.Empty,
                    Id = userProfileItem.Id,
                    LocalName = userProfileItem.LocalName,
                    IsActive = userProfileItem.IsActive,
                    IsEmailConfirmed = userProfileItem.IdentityId == null ? false : UserManager.UserManagerProvider.CheckUserEmailConfirmed(userProfileItem.IdentityId),
                    Names = userProfileItem.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(userProfileItem.LocalizationIdentifier.Localizations) : null,
                    OrgUnits = userProfileItem.OrgUnits != null ? userProfileItem.OrgUnits.Select(o => (int?)o.Id).ToList() : null,
                    Category = userProfileItem.Category != null ? userProfileItem.Category.CategoryName.Localizations.Where(l => l.Culture.Id == 1).LocalText() : null,
                    CategoryId = userProfileItem.CategoryId,
                    UserName = userProfileItem.UserName != null ? userProfileItem.UserName : string.Empty,
                    PhoneNumber = userProfileItem.PhoneNumber != null ? userProfileItem.PhoneNumber : string.Empty,
                    OrgUnitsNames = CultureName != null ? userProfileItem.OrgUnits.Select(o => o.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text).ToList() : null,
                    IsManager = userProfileItem.IsManager,
                    //GroupId= userProfileItem.Group != null ? userProfileItem.Group.Id: 0,
                    //RoleName = userProfileItem.Group != null ? userProfileItem.Group.GroupName.Localizations.Where(l=>l.Culture.Id == 1).LocalText(): string.Empty,
                    MainOrgUnitName = userProfileItem.MainOrgUnitId != 0 ? orgUnitBL.GetOrgUnitName(o => o.Id == userProfileItem.MainOrgUnitId, "ar") : string.Empty,
                    UserImageId = userProfileItem.UserImage?.Id,
                    MainOrgUnitId = userProfileItem.MainOrgUnitId,
                    IsDeleted = userProfileItem.IsDeleted,
                    ExternalId = userProfileItem.ExternalId,
                    AllowMobile = userProfileItem.AllowMobile,
                    InternalNumber = userProfileItem.InternalNumber,
                    ApiKey = userProfileItem.ApiKey,
                    TitileId = userProfileItem.TitleId,
                    UserNationalId = userProfileItem.UserNationalId,
                    UserGroups = userProfileItem?.UserGroups != null ? userProfileItem.UserGroups.Select(g => g.GroupId).ToList() :
                    new List<int>(),
                    TransactionProcessingPeriod = userProfileItem.TransactionProcessingPeriod,
                    GenderId = userProfileItem.Gender,
                    Gender = genders.Where(g => g.Id == userProfileItem.Gender)?.FirstOrDefault()?.Text,
                    Title = userProfileItem?.Title?.Localizations.Where(x => x.Culture.ShortName == CultureName).FirstOrDefault()?.Text,
                    UserGroupDTOs = userProfileItem.UserGroups.Select(x => new UserGroupDTO
                    {
                        GroupId = x.GroupId,
                        GroupName = x.Group.GroupName.Localizations.Where(g => g.Culture.ShortName == CultureName).FirstOrDefault()?.Text,

                    }).ToList(),
                    OrgUnitDTOs = userProfileItem.OrgUnits != null ? userProfileItem.OrgUnits.Select(org => new OrgUnitDTO
                    {
                        Id = org.Id,
                        Name = org.LocalizationIdentifier.Localizations.FirstOrDefault(o => o.Culture.ShortName == CultureName).Text

                    }).ToList() : new List<OrgUnitDTO>()


                }).ToList();

            return usersProfileDTOs;
        }
        public static UserProfileDTO MapUserProfile(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return null;
            }
            var vipUserId = Title.VIPUser.LookupIdentity(LookupCategory.Title, "ar");

            UserProfileDTO userProfileDTO = new UserProfileDTO()
            {
                Email = userProfile.Email,
                Id = userProfile.Id,
                LocalName = userProfile.LocalName,
                IsActive = userProfile.IsActive,
                IsEmailConfirmed = userProfile.IdentityId == null ? false : UserManager.UserManagerProvider.CheckUserEmailConfirmed(userProfile.IdentityId),
                IsDeleted = userProfile.IsDeleted,
                AllowMobile = userProfile.AllowMobile,
                InternalNumber = userProfile.InternalNumber,
                ApiKey = userProfile.ApiKey,
                TitileId = userProfile.TitleId,
                IsVipUser = userProfile.TitleId == vipUserId

            };

            if (userProfile.LocalizationIdentifier != null)
            {
                userProfileDTO.Names = userProfile.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(userProfile.LocalizationIdentifier.Localizations) : null;
            }

            if (userProfile.Category != null)
            {
                userProfileDTO.Category = userProfile.Category.CategoryName.Localizations.Where(l => l.Culture.Id == 1).LocalText();
            }

            return userProfileDTO;
        }

        public static UserProfileDTO MapUserProfileChat(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return null;
            }

            UserProfileDTO userProfileDTO = new UserProfileDTO()
            {
                Email = userProfile.Email,
                Id = userProfile.Id,
                LocalName = userProfile.LocalName,
                IsActive = userProfile.IsActive,
                UserName = userProfile.UserName,
                Status = userProfile.Status,
            };

            //if (userProfile.LocalizationIdentifier != null)
            //{
            //    userProfileDTO.Names = userProfile.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(userProfile.LocalizationIdentifier.Localizations) : null;
            //}

            //if (userProfile.Category != null)
            //{
            //    userProfileDTO.Category = userProfile.Category.CategoryName.Localizations.Where(l => l.Culture.Id == 1).LocalText();
            //}

            return userProfileDTO;
        }

        public static UserProfile Map(UserProfileDTO oAddUserProfileDTO)
        {
            if (oAddUserProfileDTO == null)
            {
                return null;
            }

            UserProfile userProfile = new UserProfile()
            {
                Id = oAddUserProfileDTO.Id,
                IsActive = true,
                Email = string.IsNullOrEmpty(oAddUserProfileDTO.Email) ? "" : oAddUserProfileDTO.Email,
                UserName = string.IsNullOrEmpty(oAddUserProfileDTO.UserName) ? "" : oAddUserProfileDTO.UserName,
                LocalName = oAddUserProfileDTO.LocalName,
            };

            return userProfile;
        }

        private static List<OrgUnit> MapOrgUnits(List<int> departmentsIds)
        {
            if (departmentsIds == null || !departmentsIds.Any())
            {
                return null;
            }

            List<OrgUnit> organizationUnit = new List<OrgUnit>();
            IOrgUnitBL OrgUnitBL = IoC.Resolve<IOrgUnitBL>();

            foreach (var id in departmentsIds)
            {
                organizationUnit.Add(OrgUnitBL.GetOrgUnitById(id));
            }

            return organizationUnit;
        }

        private static List<UserGroup> MapGroupList(List<int> GroupIds, int UserId)
        {
            if (GroupIds == null || !GroupIds.Any())
            {
                return null;
            }

            List<UserGroup> userGroups = new List<UserGroup>();

            for (int i = 0; i < GroupIds.Count; i++)
            {
                UserGroup userGroup = new UserGroup();
                userGroup.GroupId = GroupIds[i];
                userGroup.UserId = UserId;
                userGroups.Add(userGroup);
            }


            return userGroups;
        }


        public static UserProfile MapIAM(AddUserProfileDTO oAddUserProfileDTO)
        {
            if (oAddUserProfileDTO == null)
            {
                return null;
            }

            UserProfile userProfile = new UserProfile()
            {
                IsActive = oAddUserProfileDTO.IsActive,
                Email = string.IsNullOrEmpty(oAddUserProfileDTO.Email) ? "" : oAddUserProfileDTO.Email,
                PhoneNumber = oAddUserProfileDTO.PhoneNumber,
                TransactionProcessingPeriod = oAddUserProfileDTO.TransactionProcessingPeriod,
                UserName = oAddUserProfileDTO.UserName,
                TitleId = oAddUserProfileDTO.TitleId,
                CategoryId = oAddUserProfileDTO.CategoryId,
                LocalizationIdentifier = oAddUserProfileDTO.Names != null ? LocalizationIdentifierMapper.Map(oAddUserProfileDTO.Names) : null,
                OrgUnits = MapOrgUnits(oAddUserProfileDTO.OrgUnits),
                UserNationalId = oAddUserProfileDTO.UserNationalId,
                MainOrgUnitId = oAddUserProfileDTO.MainOrgUnitId,
                Gender = oAddUserProfileDTO.Gender,
                IsManager = oAddUserProfileDTO.IsManager,
                Password = oAddUserProfileDTO.Password,
                UserGroups = MapGroupList(oAddUserProfileDTO.UserGroups, oAddUserProfileDTO.Id),
                PendingRegestration = oAddUserProfileDTO.PendingRegestration,
                AllowMobile = oAddUserProfileDTO.AllowMobile,
                InternalNumber = oAddUserProfileDTO.InternalNumber,
                ApiKey = oAddUserProfileDTO.ApiKey,

            };

            return userProfile;
        }

    }
}
