using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.UserManagement;

namespace MCS.UI.Areas.User.Mappers.UserProfile
{
    public class UserProfileMapper
    {
        public static List<UserProfileDTO> Map(IList<UserProfileVM> userProfileVMs)
        {
            if (userProfileVMs == null || !userProfileVMs.Any())
            {
                return new List<UserProfileDTO>();
            }
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
                    UserName = b.UserName

                }).ToList();
            return userProfileDTOs;
        }
        public static List<UserProfileVM> Map(IList<UserProfileDTO> userProfileDTOs)
        {
            if (userProfileDTOs == null || !userProfileDTOs.Any())
            {
                return new List<UserProfileVM>();
            }
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
                    UserImageId=b.UserImageId

                }).ToList();
            return userProfileVMs;
        }
        public static List<AddUserProfileDTO> Map(IList<AddUserProfileVM> addUserProfileVMs)
        {
            if (addUserProfileVMs == null || !addUserProfileVMs.Any())
            {
                return new List<AddUserProfileDTO>();
            }
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
                    UserName = b.UserName

                }).ToList();
            return addUserProfileDTOs;
        }
        public static List<AddUserProfileVM> Map(IList<AddUserProfileDTO> addUserProfileDTOs)
        {
            if (addUserProfileDTOs == null || !addUserProfileDTOs.Any())
            {
                return new List<AddUserProfileVM>();
            }
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
                    UserName = b.UserName

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
                    UserName = addUserProfileVM.UserName

                };
                return addUserProfileDTO;
            }
            return new AddUserProfileDTO();
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
                    UserName = addUserProfileDTO.UserName

                };
                return addUserProfileVM;
            }
            return new AddUserProfileVM();
        }
        public static EditUserProfileVM Map(EditUserProfileDTO editUserProfileDTO)
        {
            if (editUserProfileDTO != null)
            {
                EditUserProfileVM editUserProfileVM = new EditUserProfileVM
                {
                    IdentifierId = editUserProfileDTO.IdentifierId,
                    CategoryId = editUserProfileDTO.CategoryId,
                    OrgUnits = editUserProfileDTO.OrgUnits,
                    Permissions = editUserProfileDTO.Permissions,
                    PhoneNumber = editUserProfileDTO.PhoneNumber,
                    TitleId = editUserProfileDTO.TitleId,
                    TransactionProcessingPeriod = editUserProfileDTO.TransactionProcessingPeriod,
                    Email = editUserProfileDTO.Email,
                    Id = editUserProfileDTO.Id,
                    IsActive = editUserProfileDTO.IsActive,
                    Names = LocalizationMapper.Map(editUserProfileDTO.Names),
                    UserName = editUserProfileDTO.UserName

                };
                return editUserProfileVM;
            }
            return new EditUserProfileVM();
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
                    Permissions = editUserProfileVM.Permissions,
                    PhoneNumber = editUserProfileVM.PhoneNumber,
                    TitleId = editUserProfileVM.TitleId,
                    TransactionProcessingPeriod = editUserProfileVM.TransactionProcessingPeriod,
                    Email = editUserProfileVM.Email,
                    Id = editUserProfileVM.Id,
                    IsActive = editUserProfileVM.IsActive,
                    Names = LocalizationMapper.Map(editUserProfileVM.Names),
                    UserName = editUserProfileVM.UserName

                };
                return editUserProfileDTO;
            }
            return new EditUserProfileDTO();
        }
        public static List<EditUserProfileDTO> Map(IList<EditUserProfileVM> editUserProfileVMs)
        {
            if (editUserProfileVMs == null || !editUserProfileVMs.Any())
            {
                return new List<EditUserProfileDTO>();
            }
            List<EditUserProfileDTO> editUserProfileDTOs = editUserProfileVMs
                .Select(b => new EditUserProfileDTO
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
                    IdentifierId = b.IdentifierId

                }).ToList();
            return editUserProfileDTOs;
        }
        public static List<EditUserProfileVM> Map(IList<EditUserProfileDTO> editUserProfileDTOs)
        {
            if (editUserProfileDTOs == null || !editUserProfileDTOs.Any())
            {
                return new List<EditUserProfileVM>();
            }
            List<EditUserProfileVM> editUserProfileVMs = editUserProfileDTOs
                .Select(b => new EditUserProfileVM
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
                    IdentifierId = b.IdentifierId

                }).ToList();
            return editUserProfileVMs;
        }
    }
}