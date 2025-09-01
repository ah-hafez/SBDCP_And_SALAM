using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.UserPreferences.UserDelegation;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers.UserPreferences
{
    public static class UserPreferenceMapper
    {
        public static List<UserPreferenceVM> Map(IList<UserPreferenceDTO> userPreferenceDTOs)
        {
            if (userPreferenceDTOs == null || !userPreferenceDTOs.Any())
            {
                return new List<UserPreferenceVM>();
            }
            List<UserPreferenceVM> userPreferenceVMs = userPreferenceDTOs
                .Select(userPreferenceDTO => new UserPreferenceVM()
                {
                    Id = userPreferenceDTO.Id,
                    ActivateDelegation = userPreferenceDTO.ActivateDelegation,
                    CurrentDelegationUsers = UserDelegationMapper.Map(userPreferenceDTO.CurrentDelegationUsers),
                    DelegationUsers = UserDelegationMapper.Map(userPreferenceDTO.DelegationUsers),
                    Email = userPreferenceDTO.Email,
                    LanguageId = userPreferenceDTO.LanguageId,
                    Marking = userPreferenceDTO.Marking,
                    MarkingDoc = userPreferenceDTO.MarkingDoc,
                    NotificationSubscriptions = NotificationSubscriptionMapper.Map(userPreferenceDTO.NotificationSubscriptions),
                    PasswordForSignature = userPreferenceDTO.PasswordForSignature,                   
                    Signature = userPreferenceDTO.Signature,
                    SignatureBehalf = userPreferenceDTO.SignatureBehalf,
                    SignatureCommand = userPreferenceDTO.SignatureCommand,
                    SignatureDoc = userPreferenceDTO.SignatureDoc,
                    SignatureBehalfDoc = userPreferenceDTO.SignatureBehalfDoc,
                    SignatureCommandDoc = userPreferenceDTO.SignatureCommandDoc,

                    UserId = userPreferenceDTO.UserId,
                    UserTrays = UserTrayPreferencesMapper.Map(userPreferenceDTO.UserTrays),
                    Theme = userPreferenceDTO.Theme ,
                    SMSNotifications = userPreferenceDTO.SMSNotifications,
                    PhoneNumber = userPreferenceDTO.PhoneNumber
                }).ToList();

            return userPreferenceVMs;
        }
        public static List<UserPreferenceDTO> Map(IList<UserPreferenceVM> userPreferenceVMs)
        {
            if (userPreferenceVMs == null || !userPreferenceVMs.Any())
            {
                return new List<UserPreferenceDTO>();
            }
            List<UserPreferenceDTO> userPreferenceDTOs = userPreferenceVMs
                .Select(userPreferenceVM => new UserPreferenceDTO()
                {
                    Id = userPreferenceVM.Id,
                    ActivateDelegation = userPreferenceVM.ActivateDelegation,
                    CurrentDelegationUsers = UserDelegationMapper.Map(userPreferenceVM.CurrentDelegationUsers),
                    DelegationUsers = UserDelegationMapper.Map(userPreferenceVM.DelegationUsers),
                    Email = userPreferenceVM.Email,
                    LanguageId = userPreferenceVM.LanguageId,
                    Marking = userPreferenceVM.Marking,
                    MarkingDoc = userPreferenceVM.MarkingDoc,
                    NotificationSubscriptions = NotificationSubscriptionMapper.Map(userPreferenceVM.NotificationSubscriptions),
                    PasswordForSignature = userPreferenceVM.PasswordForSignature,
                    Signature = userPreferenceVM.Signature,
                    SignatureDoc = userPreferenceVM.SignatureDoc,
                    SignatureBehalf = userPreferenceVM.SignatureBehalf,
                    SignatureBehalfDoc = userPreferenceVM.SignatureBehalfDoc,
                    SignatureCommand = userPreferenceVM.SignatureCommand,
                    SignatureCommandDoc = userPreferenceVM.SignatureCommandDoc,
                    UserId = userPreferenceVM.UserId,
                    UserTrays = UserTrayPreferencesMapper.Map(userPreferenceVM.UserTrays),
                    Theme = userPreferenceVM.Theme,
                    SMSNotifications = userPreferenceVM.SMSNotifications,
                    PhoneNumber = userPreferenceVM.PhoneNumber
                }).ToList();

            return userPreferenceDTOs;
        }
        public static UserPreferenceVM Map(UserPreferenceDTO userPreferenceDTO)
        {
            if (userPreferenceDTO != null)
            {
                return new UserPreferenceVM()
                {
                    Id = userPreferenceDTO.Id,
                    ActivateDelegation = userPreferenceDTO.ActivateDelegation,
                    CurrentDelegationUsers = UserDelegationMapper.Map(userPreferenceDTO.CurrentDelegationUsers),
                    DelegationUsers = UserDelegationMapper.Map(userPreferenceDTO.DelegationUsers),
                    Email = userPreferenceDTO.Email,
                    LanguageId = userPreferenceDTO.LanguageId,
                    Marking = userPreferenceDTO.Marking,
                    MarkingDoc = userPreferenceDTO.MarkingDoc,
                    NotificationSubscriptions = NotificationSubscriptionMapper.Map(userPreferenceDTO.NotificationSubscriptions),
                    PasswordForSignature = userPreferenceDTO.PasswordForSignature,
                    Signature = userPreferenceDTO.Signature,
                    SignatureDoc = userPreferenceDTO.SignatureDoc,
                    SignatureBehalf = userPreferenceDTO.SignatureBehalf,
                    SignatureBehalfDoc = userPreferenceDTO.SignatureBehalfDoc,
                    SignatureCommand = userPreferenceDTO.SignatureCommand,
                    SignatureCommandDoc = userPreferenceDTO.SignatureCommandDoc,
                    SealSignature = userPreferenceDTO.SealSignature,
                    SealSignatureDoc= userPreferenceDTO.SealSignatureDoc,
                    MessageSignature = userPreferenceDTO.MessageSignature,
                    MessageSignatureDoc = userPreferenceDTO.MessageSignatureDoc,
                    UserId = userPreferenceDTO.UserId,
                    HasSignaturePasswordText = userPreferenceDTO.HasSignaturePasswordText,
                    FollowUpOrgId=userPreferenceDTO.FollowUpOrgId,
                    FollowUpUserId=userPreferenceDTO.FollowUpUserId,
                    UserTrays = UserTrayPreferencesMapper.Map(userPreferenceDTO.UserTrays),
                    Theme = userPreferenceDTO.Theme,
                    SMSNotifications = userPreferenceDTO.SMSNotifications,
                    PhoneNumber = userPreferenceDTO.PhoneNumber,
                    MyDelegations = UserDelegationMapper.Map(userPreferenceDTO.MyDelegationUsers),
                    DefaultDisplay = userPreferenceDTO.DefaultDisplay,
                    DefaultAssignmentPaper = userPreferenceDTO.DefaultAssignmentPaper,
                    
                };
            }
            return new UserPreferenceVM();
        }

        public static List<UserPendingRequest> Map(List<UserPendingGroupDTO> UserPendingGroupDTOs)
        {
            if (UserPendingGroupDTOs == null || !UserPendingGroupDTOs.Any())
            { return new List<UserPendingRequest>(); }
            List<UserPendingRequest> userPendingGroupVMs = UserPendingGroupDTOs
                .Select(b => new UserPendingRequest
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GroupId = b.GroupId,
                    UserName = b.UserName,
                    GroupName = b.GroupName,

                }).ToList();
            return userPendingGroupVMs;
        }

    }
}