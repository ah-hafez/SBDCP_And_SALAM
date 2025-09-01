using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.UserManagement;

namespace MCS.UI.Areas.User.Mappers.UserManagement
{
    public static class UserProfileMapper
    {
        public static UserProfileVM Map(UserProfileDTO userProfileDTO)
        {
            if (userProfileDTO != null)
            {
                UserProfileVM userProfileVM = new UserProfileVM()
                {
                    Category = userProfileDTO.Category,
                    Email = userProfileDTO.Email,
                    Id = userProfileDTO.Id,
                    IsActive = userProfileDTO.IsActive,
                    IsEmailConfirmed = userProfileDTO.IsEmailConfirmed,
                    IsSelected = userProfileDTO.IsSelected,
                    LocalName = userProfileDTO.LocalName,
                    Names = LocalizationMapper.Map(userProfileDTO.Names),
                    UserName = userProfileDTO.UserName,
                    RoleName=userProfileDTO.RoleName
                };

                return userProfileVM;
            }
            return new UserProfileVM();
        }
        public static List<UserProfileVM> Map(IList<UserProfileDTO> userProfileDTOs)
        {
            if (userProfileDTOs == null || !userProfileDTOs.Any())
            {
                return new List<UserProfileVM>();
            }
            List<UserProfileVM> userProfileVMs = userProfileDTOs
                .Select(userProfileDTO => new UserProfileVM()
                {
                    Category = userProfileDTO.Category,
                    Email = userProfileDTO.Email,
                    Id = userProfileDTO.Id,
                    IsActive = userProfileDTO.IsActive,
                    IsEmailConfirmed = userProfileDTO.IsEmailConfirmed,
                    IsSelected = userProfileDTO.IsSelected,
                    LocalName = userProfileDTO.LocalName,
                    Names = LocalizationMapper.Map(userProfileDTO.Names),
                    UserName = userProfileDTO.UserName,
                    RoleName=userProfileDTO.RoleName
                }).ToList();

            return userProfileVMs;
        }
        public static UserProfileDTO Map(UserProfileVM userProfileVM)
        {
            if (userProfileVM != null)
            {
                UserProfileDTO userProfileDTO = new UserProfileDTO
                {
                    Category = userProfileVM.Category,
                    Email = userProfileVM.Email,
                    Id = userProfileVM.Id,
                    IsActive = userProfileVM.IsActive,
                    IsEmailConfirmed = userProfileVM.IsEmailConfirmed,
                    IsSelected = userProfileVM.IsSelected,
                    LocalName = userProfileVM.LocalName,
                    Names = LocalizationMapper.Map(userProfileVM.Names),
                    UserName = userProfileVM.UserName,
                    RoleName=userProfileVM.RoleName
                };

                return userProfileDTO;
            }
            return new UserProfileDTO();
        }
        public static List<UserProfileDTO> Map(IList<UserProfileVM> userProfileVMs)
        {
            if (userProfileVMs == null || !userProfileVMs.Any())
            {
                return new List<UserProfileDTO>();
            }
            List<UserProfileDTO> userProfileDTOs = userProfileVMs
                .Select(userProfileVM => new UserProfileDTO()
                {

                    Category = userProfileVM.Category,
                    Email = userProfileVM.Email,
                    Id = userProfileVM.Id,
                    IsActive = userProfileVM.IsActive,
                    IsEmailConfirmed = userProfileVM.IsEmailConfirmed,
                    IsSelected = userProfileVM.IsSelected,
                    LocalName = userProfileVM.LocalName,
                    Names = LocalizationMapper.Map(userProfileVM.Names),
                    UserName = userProfileVM.UserName
                }).ToList();

            return userProfileDTOs;
        }
        public static List<AddUserProfileDTO> Map(IList<AddUserProfileVM> addUserProfileVMs)
        {
            if (addUserProfileVMs == null || !addUserProfileVMs.Any())
            {
                return new List<AddUserProfileDTO>();
            }
            List<AddUserProfileDTO> addUserProfileDTOs = addUserProfileVMs
                .Select(addUserProfileDTO => new AddUserProfileDTO()
                {
                    CategoryId = addUserProfileDTO.CategoryId,
                    Email = addUserProfileDTO.Email,
                    Id = addUserProfileDTO.Id,
                    IsActive = addUserProfileDTO.IsActive,
                    Names = LocalizationMapper.Map(addUserProfileDTO.Names),
                    UserName = addUserProfileDTO.UserName
                }).ToList();

            return addUserProfileDTOs;
        }
        public static List<EditUserProfileDTO> Map(IList<EditUserProfileVM> editUserProfileVMs)
        {
            if (editUserProfileVMs == null || !editUserProfileVMs.Any())
            {
                return new List<EditUserProfileDTO>();
            }
            List<EditUserProfileDTO> editUserProfileDTOs = editUserProfileVMs
                .Select(userProfileVM => new EditUserProfileDTO()
                {
                    IdentifierId = userProfileVM.IdentifierId,
                    OrgUnits = userProfileVM.OrgUnits,
                    Permissions = userProfileVM.Permissions,
                    PhoneNumber = userProfileVM.PhoneNumber,
                    TitleId = userProfileVM.TitleId,
                    TransactionProcessingPeriod = userProfileVM.TransactionProcessingPeriod,
                    CategoryId = userProfileVM.CategoryId,
                    Email = userProfileVM.Email,
                    Id = userProfileVM.Id,
                    IsActive = userProfileVM.IsActive,
                    Names = LocalizationMapper.Map(userProfileVM.Names),
                    UserName = userProfileVM.UserName
                }).ToList();

            return editUserProfileDTOs;
        }
        public static List<EditUserProfileVM> Map(IList<EditUserProfileDTO> edituserProfileDTOs)
        {
            if (edituserProfileDTOs == null || !edituserProfileDTOs.Any())
            {
                return new List<EditUserProfileVM>();
            }
            List<EditUserProfileVM> edituserProfileVMs = edituserProfileDTOs
                .Select(userProfileDTO => new EditUserProfileVM()
                {
                    IdentifierId = userProfileDTO.IdentifierId,
                    OrgUnits = userProfileDTO.OrgUnits,
                    Permissions = userProfileDTO.Permissions,
                    PhoneNumber = userProfileDTO.PhoneNumber,
                    TitleId = userProfileDTO.TitleId,
                    TransactionProcessingPeriod = userProfileDTO.TransactionProcessingPeriod,
                    CategoryId = userProfileDTO.CategoryId,
                    Email = userProfileDTO.Email,
                    Id = userProfileDTO.Id,
                    IsActive = userProfileDTO.IsActive,
                    Names = LocalizationMapper.Map(userProfileDTO.Names),
                    UserName = userProfileDTO.UserName
                }).ToList();
            return edituserProfileVMs;
        }
    }
}