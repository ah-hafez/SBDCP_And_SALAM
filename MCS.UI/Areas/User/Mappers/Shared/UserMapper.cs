using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public static class UserMapper
    {
        public static List<UserDTO> Map(IList<UserVM> userVMs)
        {
            if (userVMs == null || !userVMs.Any())
            {
                return new List<UserDTO>();
            }
            List<UserDTO> userDTOs = userVMs
                .Select(b => new UserDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = b.LoclizationName != null ? LocalizationMapper.Map(b.LoclizationName) : null,
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    SignatureCommand = b.SignatureCommand,
                    SignatureBehalf = b.SignatureBehalf,
                    MessageSignature = b.MessageSignature,
                    SealSignatureDoc = b.SealSignatureDoc,
                    TrayDetails = TrayDetailsMapper.Map(b.TrayDetails),
                    UserName = b.UserName,
                    LoclizationUserCategory = LocalizationMapper.Map(b.LoclizationUserCategory),
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                }).ToList();
            return userDTOs;
        }
        public static List<UserVM> Map(IList<UserDTO> userDTOs)
        {
            if (userDTOs == null || !userDTOs.Any())
            {
                return new List<UserVM>();
            }
            List<UserVM> userVMs = userDTOs
                .Select(b => new UserVM
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    SignatureCommand = b.SignatureCommand,
                    SignatureBehalf = b.SignatureBehalf,
                    MessageSignature = b.MessageSignature,
                    SealSignatureDoc = b.SealSignatureDoc,
                    TrayDetails = TrayDetailsMapper.Map(b.TrayDetails),
                    UserName = b.UserName,
                    LoclizationUserCategory = LocalizationMapper.Map(b.LoclizationUserCategory),
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                }).ToList();
            return userVMs;

        }
        public static UserVM Map(UserDTO b)
        {
            if (b != null)
            {
                UserVM userVM = new UserVM()
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = b.LoclizationName != null ? LocalizationMapper.Map(b.LoclizationName) : null,
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    SignatureCommand = b.SignatureCommand,
                    SignatureBehalf = b.SignatureBehalf,
                    MessageSignature = b.MessageSignature,
                    SealSignatureDoc = b.SealSignatureDoc,
                    Signature = b.Signature,
                    TrayDetails = TrayDetailsMapper.Map(b.TrayDetails),
                    UserName = b.UserName,
                    LoclizationUserCategory = LocalizationMapper.Map(b.LoclizationUserCategory),
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits),
                    Email = b.Email,
                    PhoneNumber = b.PhoneNumber,
                    TenantLogo = b.TenantLogo,
                    TenantName = b.LocalName,
                    CultureId = b.CultureId,
                    ThemeId = b.ThemeId,
                    ThemePath = b.ThemePath,
                    SMSNotifications = b.SMSNotifications,
                    HasSignaturePasswordText = b.HasSignaturePasswordText,
                    IsVIPUser = b.IsVIPUser,
                    DefaultDisplay = b.DefaultDisplay,
                    IsManager = b.IsManager,
                    InternalNumber = b.InternalNumber,
                    DefaultAssignmentPaper = b.DefaultAssignmentPaper,

                };
                return userVM;
            }
            return new UserVM();

        }
        public static UserDTO Map(UserVM b)
        {
            if (b != null)
            {
                UserDTO userDTO = new UserDTO()
                {
                    Id = b.Id,
                    Name = b.Name,
                    AccessToken = b.AccessToken,
                    BaseOrgUnitName = b.BaseOrgUnitName,
                    Claims = b.Claims,
                    LoclizationName = LocalizationMapper.Map(b.LoclizationName),
                    Marking = b.Marking,
                    SessionId = b.SessionId,
                    Signature = b.Signature,
                    SignatureCommand = b.SignatureCommand,
                    SignatureBehalf = b.SignatureBehalf,
                    SealSignatureDoc = b.SealSignatureDoc,
                    MessageSignature = b.MessageSignature,
                    TrayDetails = TrayDetailsMapper.Map(b.TrayDetails),
                    UserName = b.UserName,
                    LoclizationUserCategory = LocalizationMapper.Map(b.LoclizationUserCategory),
                    UserCategoryName = b.UserCategoryName,
                    UserOrgUnits = UserOrgUnitMapper.Map(b.UserOrgUnits)
                };
                return userDTO;
            }
            return new UserDTO();

        }
    }
}