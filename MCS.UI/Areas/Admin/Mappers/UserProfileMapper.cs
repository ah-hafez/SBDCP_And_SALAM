using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.User.Models;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class UserProfileMapper
    {


        public static List<UserProfileDTO> Map(IList<UserProfileVM> userProfileVMs)
        {
            if (userProfileVMs == null || !userProfileVMs.Any())
            { return null; }
            List<UserProfileDTO> userProfileDTOs = userProfileVMs
                .Select(b => new UserProfileDTO
                {
                    Category = b.Category,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    IsEmailConfirmed = b.IsEmailConfirmed,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    GroupId = b.RoleId,
                    IsManager = b.IsManager,
                    ExternalId = b.ExternalId,
                    AllowMobile = b.AllowMobile,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                    LoginTime = b.LoginTime,
                    LastLogout = b.LastLogout,

                }).ToList();
            return userProfileDTOs;
        }
        public static List<UserProfileVM> Map(IList<UserProfileDTO> userProfileDTOs)
        {
            if (userProfileDTOs == null || !userProfileDTOs.Any())
            { return new List<UserProfileVM>(); }
            List<UserProfileVM> userProfileVMs = userProfileDTOs
                .Select(b => new UserProfileVM
                {
                    Category = b.Category,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    IsEmailConfirmed = b.IsEmailConfirmed,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    MainOrgUnitName = b.MainOrgUnitName,
                    PhoneNumber = b.PhoneNumber,
                    OrgUnitsNames = b.OrgUnitsNames,
                    IsManager = b.IsManager,
                    RoleId = b.GroupId,
                    RoleName = b.RoleName,
                    IsDeleted = b.IsDeleted,
                    ExternalId = b.ExternalId,
                    AllowMobile = b.AllowMobile,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                    LoginTime = b.LoginTime,
                    LastLogout = b.LastLogout,
                }).ToList();
            return userProfileVMs;
        }
        public static List<AddUserProfileDTO> Map(IList<AddUserProfileVM> addUserProfileVMs)
        {
            if (addUserProfileVMs == null || !addUserProfileVMs.Any())
            { return null; }
            List<AddUserProfileDTO> addUserProfileDTOs = addUserProfileVMs
                .Select(b => new AddUserProfileDTO
                {
                    CategoryId = b.CategoryId,
                    OrgUnits = b.OrgUnits,
                    Permissions = b.Permissions,
                    PhoneNumber = b.PhoneNumber,
                    TitleId = b.TitleId,
                    TransactionProcessingPeriod = b.TransactionProcessingPeriod,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    Gender = b.Gender,
                    UserNationalId = b.UserNationalId,
                    RoleId = b.RoleId,
                    IsManager = b.IsManager,
                    SMSNotifications = b.SMSNotifications,
                    IsFollowUpUser = b.IsFollowUpUser,
                    AllowMobile = b.AllowMobile,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                    UserMobileClassId = b.UserMobileClassId,
                }).ToList();
            return addUserProfileDTOs;
        }
        public static List<AddUserProfileVM> Map(IList<AddUserProfileDTO> addUserProfileDTOs)
        {
            if (addUserProfileDTOs == null || !addUserProfileDTOs.Any())
            { return null; }
            List<AddUserProfileVM> addUserProfileVMs = addUserProfileDTOs
                .Select(b => new AddUserProfileVM
                {
                    CategoryId = b.CategoryId,
                    OrgUnits = b.OrgUnits,
                    Permissions = b.Permissions,
                    PhoneNumber = b.PhoneNumber,
                    TitleId = b.TitleId,
                    TransactionProcessingPeriod = b.TransactionProcessingPeriod,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    UserNationalId = b.UserNationalId,
                    Gender = b.Gender,
                    IsManager = b.IsManager,
                    RoleId = b.RoleId,
                    SMSNotifications = b.SMSNotifications,
                    IsFollowUpUser = b.IsFollowUpUser,
                    AllowMobile = b.AllowMobile,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                    UserMobileClassId = b.UserMobileClassId,
                }).ToList();
            return addUserProfileVMs;
        }
        public static AddUserProfileDTO Map(AddUserProfileVM addUserProfileVM)
        {
            if (addUserProfileVM != null)
            {
                AddUserProfileDTO addUserProfileDTO = new AddUserProfileDTO
                {
                    CategoryId = addUserProfileVM.CategoryId,
                    OrgUnits = addUserProfileVM.OrgUnits,
                    Permissions = addUserProfileVM.Permissions,
                    PhoneNumber = addUserProfileVM.PhoneNumber,
                    TitleId = addUserProfileVM.TitleId,
                    TransactionProcessingPeriod = addUserProfileVM.TransactionProcessingPeriod,
                    Email = addUserProfileVM.Email,
                    Id = addUserProfileVM.Id,
                    IsActive = addUserProfileVM.IsActive,
                    Names = LocalizationMapper.Map(addUserProfileVM.Names),
                    UserName = addUserProfileVM.UserName,
                    UserNationalId = addUserProfileVM.UserNationalId,
                    IsManager = addUserProfileVM.IsManager,
                    MainOrgUnitId = addUserProfileVM.MainOrgUnitId.Value,
                    RoleId = addUserProfileVM.RoleId,
                    Gender = addUserProfileVM.Gender,
                    Password = addUserProfileVM.Password,
                    UserGroups = addUserProfileVM.UserGroupsList,
                    SMSNotifications = addUserProfileVM.SMSNotifications,
                    IsFollowUpUser = addUserProfileVM.IsFollowUpUser,
                    PendingRegestration = addUserProfileVM.PendingRegestration,
                    AllowMobile = addUserProfileVM.AllowMobile,
                    InternalNumber = addUserProfileVM.InternalNumber,
                    ApiKey = addUserProfileVM.ApiKey,
                    UserMobileClassId = addUserProfileVM.UserMobileClassId,


                };
                return addUserProfileDTO;
            }
            return null;
        }
        public static AddUserProfileVM Map(AddUserProfileDTO addUserProfileDTO)
        {
            if (addUserProfileDTO != null)
            {
                AddUserProfileVM addUserProfileVM = new AddUserProfileVM
                {
                    CategoryId = addUserProfileDTO.CategoryId,
                    OrgUnits = addUserProfileDTO.OrgUnits,
                    Permissions = addUserProfileDTO.Permissions,
                    PhoneNumber = addUserProfileDTO.PhoneNumber,
                    TitleId = addUserProfileDTO.TitleId,
                    TransactionProcessingPeriod = addUserProfileDTO.TransactionProcessingPeriod,
                    Email = addUserProfileDTO.Email,
                    Id = addUserProfileDTO.Id,
                    IsActive = addUserProfileDTO.IsActive,
                    Names = LocalizationMapper.Map(addUserProfileDTO.Names),
                    UserName = addUserProfileDTO.UserName,
                    Gender = addUserProfileDTO.Gender,
                    UserNationalId = addUserProfileDTO.UserNationalId,
                    RoleId = addUserProfileDTO.RoleId,
                    IsManager = addUserProfileDTO.IsManager,
                    SMSNotifications = addUserProfileDTO.SMSNotifications,
                    IsFollowUpUser = addUserProfileDTO.IsFollowUpUser,
                    AllowMobile = addUserProfileDTO.AllowMobile,
                    InternalNumber = addUserProfileDTO.InternalNumber,
                    ApiKey = addUserProfileDTO.ApiKey,
                    UserMobileClassId = addUserProfileDTO.UserMobileClassId,

                };
                return addUserProfileVM;
            }
            return null;
        }
        public static EditUserProfileVM Map(EditUserProfileDTO editUserProfileDTO)
        {
            if (editUserProfileDTO != null)
            {
                EditUserProfileVM editUserProfileVM = new EditUserProfileVM
                {
                    IdentifierId = editUserProfileDTO.IdentifierId,
                    CategoryId = editUserProfileDTO.CategoryId,
                    OrgUnits = null,
                    UserGroups = editUserProfileDTO.UserGroups,
                    Permissions = editUserProfileDTO.Permissions,
                    PhoneNumber = editUserProfileDTO.PhoneNumber,
                    TitleId = editUserProfileDTO.TitleId,
                    TitleName = editUserProfileDTO.TitleName,
                    TransactionProcessingPeriod = editUserProfileDTO.TransactionProcessingPeriod,
                    Email = editUserProfileDTO.Email,
                    Id = editUserProfileDTO.Id,
                    IsActive = editUserProfileDTO.IsActive,
                    Names = LocalizationMapper.Map(editUserProfileDTO.Names),
                    UserName = editUserProfileDTO.UserName,
                    UserNationalId = editUserProfileDTO.UserNationalId,
                    RoleId = editUserProfileDTO.RoleId,
                    Gender = editUserProfileDTO.Gender,
                    IsManager = editUserProfileDTO.IsManager,
                    MainOrgUnitId = editUserProfileDTO.MainOrgUnitId,
                    MainOrgUnitName = editUserProfileDTO.MainOrgUnitName,
                    OrgUnitList = editUserProfileDTO.OrgUnitList,
                    ExternalId = editUserProfileDTO.ExternalId,
                    SMSNotifications = editUserProfileDTO.SMSNotifications,
                    IsFollowUpUser = editUserProfileDTO.IsFollowUpUser,
                    AllowMobile = editUserProfileDTO.AllowMobile,
                    InternalNumber = editUserProfileDTO.InternalNumber,
                    ApiKey = editUserProfileDTO.ApiKey,
                    UserMobileClassId = editUserProfileDTO.UserMobileClassId,
                    UserMobileClassName = editUserProfileDTO.UserMobileClassName,


                };
                return editUserProfileVM;
            }
            return null;
        }
        public static UserProfileVM Map(UserProfileDTO b)
        {
            if (b != null)
            {
                UserProfileVM editUserProfileVM = new UserProfileVM
                {
                    Category = b.Category,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    IsEmailConfirmed = b.IsEmailConfirmed,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    PhoneNumber = b.PhoneNumber,
                    OrgUnitsNames = b.OrgUnitsNames,
                    OrgUnits = b.OrgUnits,
                    IsManager = b.IsManager,
                    RoleId = b.GroupId,
                    ExternalId = b.ExternalId,
                    AllowMobile = b.AllowMobile,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                };
                return editUserProfileVM;
            }
            return null;
        }
        public static EditUserProfileDTO Map(EditUserProfileVM editUserProfileVM)
        {
            if (editUserProfileVM != null)
            {
                EditUserProfileDTO editUserProfileDTO = new EditUserProfileDTO
                {
                    IdentifierId = editUserProfileVM.IdentifierId,
                    CategoryId = editUserProfileVM.CategoryId,
                    OrgUnits = editUserProfileVM.OrgUnits,
                    //Permissions = editUserProfileVM.Permissions,
                    PhoneNumber = editUserProfileVM.PhoneNumber,
                    TitleId = editUserProfileVM.TitleId,
                    TransactionProcessingPeriod = editUserProfileVM.TransactionProcessingPeriod,
                    Email = editUserProfileVM.Email,
                    Id = editUserProfileVM.Id,
                    IsActive = editUserProfileVM.IsActive,
                    Names = LocalizationMapper.Map(editUserProfileVM.Names),
                    UserName = editUserProfileVM.UserName,
                    UserNationalId = editUserProfileVM.UserNationalId,
                    RoleId = editUserProfileVM.RoleId,
                    MainOrgUnitId = editUserProfileVM.MainOrgUnitId.Value,
                    Gender = editUserProfileVM.Gender,
                    IsManager = editUserProfileVM.IsManager,
                    ExternalId = editUserProfileVM.ExternalId,
                    UserGroupsData = editUserProfileVM.UserGroupsData,
                    SMSNotifications = editUserProfileVM.SMSNotifications,
                    IsFollowUpUser = editUserProfileVM.IsFollowUpUser,
                    AllowMobile = editUserProfileVM.AllowMobile,
                    UserMobileClassId = editUserProfileVM.UserMobileClassId,
                    UserMobileClassName = editUserProfileVM.UserMobileClassName,
                    InternalNumber = editUserProfileVM.InternalNumber,
                    ApiKey = editUserProfileVM.ApiKey,
                };
                return editUserProfileDTO;
            }
            return null;
        }

        public static List<EditUserProfileDTO> Map(IList<EditUserProfileVM> editUserProfileVMs)
        {
            if (editUserProfileVMs == null || !editUserProfileVMs.Any())
            { return null; }
            List<EditUserProfileDTO> editUserProfileDTOs = editUserProfileVMs
                .Select(b => new EditUserProfileDTO
                {
                    IdentifierId = b.IdentifierId,
                    CategoryId = b.CategoryId,
                    OrgUnits = b.OrgUnits,
                    Permissions = b.Permissions,
                    PhoneNumber = b.PhoneNumber,
                    TitleId = b.TitleId,
                    TitleName = b.TitleName,
                    TransactionProcessingPeriod = b.TransactionProcessingPeriod,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    UserNationalId = b.UserNationalId,
                    IsManager = b.IsManager,
                    Gender = b.Gender,
                    MainOrgUnitId = b.MainOrgUnitId.Value,
                    RoleId = b.RoleId,
                    ExternalId = b.ExternalId,
                    SMSNotifications = b.SMSNotifications,
                    IsFollowUpUser = b.IsFollowUpUser,
                    AllowMobile = b.AllowMobile,
                    UserMobileClassId = b.UserMobileClassId,
                    UserMobileClassName = b.UserMobileClassName,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                }).ToList();
            return editUserProfileDTOs;
        }
        public static List<EditUserProfileVM> Map(IList<EditUserProfileDTO> editUserProfileDTOs)
        {
            if (editUserProfileDTOs == null || !editUserProfileDTOs.Any())
            { return null; }
            List<EditUserProfileVM> editUserProfileVMs = editUserProfileDTOs
                .Select(b => new EditUserProfileVM
                {
                    CategoryId = b.CategoryId,
                    OrgUnits = b.OrgUnits,
                    Permissions = b.Permissions,
                    PhoneNumber = b.PhoneNumber,
                    TitleId = b.TitleId,
                    TitleName = b.TitleName,
                    TransactionProcessingPeriod = b.TransactionProcessingPeriod,
                    Email = b.Email,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    Names = LocalizationMapper.Map(b.Names),
                    UserName = b.UserName,
                    IdentifierId = b.IdentifierId,
                    UserNationalId = b.UserNationalId,
                    IsManager = b.IsManager,
                    Gender = b.Gender,
                    MainOrgUnitId = b.MainOrgUnitId,
                    RoleId = b.RoleId,
                    ExternalId = b.ExternalId,
                    SMSNotifications = b.SMSNotifications,
                    IsFollowUpUser = b.IsFollowUpUser,
                    AllowMobile = b.AllowMobile,
                    UserMobileClassId = b.UserMobileClassId,
                    UserMobileClassName = b.UserMobileClassName,
                    InternalNumber = b.InternalNumber,
                    ApiKey = b.ApiKey,
                }).ToList();
            return editUserProfileVMs;
        }

        public static UpdateUserProfileDto Map(UpdateUserInformationVM updateUserInformationVM)
        {
            return new UpdateUserProfileDto
            {

                PhoneNumber = updateUserInformationVM.PhoneNumber,
                InternalNumber = updateUserInformationVM.TransferNumber,

            };
        }
    }
}