using MCS.Business;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class UserPreferenceInfoMapper
    {
        public static UserPreferenceDTO Map(UserPreferenceInfo userPreferenceInfo , string CultureName)
        {
            if (userPreferenceInfo == null)
            {
                return null;
            }
            UserPreferenceDTO userPreferenceDTO = new UserPreferenceDTO()
            {
                Id = userPreferenceInfo.Id,
                LanguageId = userPreferenceInfo.Culture.Id,
                MarkingDoc = userPreferenceInfo.Marking,
                PasswordForSignature = userPreferenceInfo.PasswordComfiration,
                SignatureDoc = userPreferenceInfo.Signature,
                SignatureCommandDoc = userPreferenceInfo.SignatureCommand,
                SignatureBehalfDoc = userPreferenceInfo.SignatureBehalf,
                Email = userPreferenceInfo.Email,
                UserId=userPreferenceInfo.UserProfile.Id,
                ActivateDelegation=userPreferenceInfo.IsDelegated,
                HasSignaturePasswordText = userPreferenceInfo.HasSignaturePasswordText,
                FollowUpOrgId= userPreferenceInfo.FollowUpOrgId,
                FollowUpUserId= userPreferenceInfo.FollowUpUserId,
                MessageSignatureDoc= userPreferenceInfo.MessageSignature,
                SealSignatureDoc = userPreferenceInfo.SealSignatureDoc,
                NotificationSubscriptions = NotificationSubscriptionMapper.Map(userPreferenceInfo.NotificationSubscriptions, CultureName),
                CurrentDelegationUsers = UserDelegationMapper.Map(userPreferenceInfo.UserDelegations),
                Theme = userPreferenceInfo.ThemeId,
                SMSNotifications = userPreferenceInfo.SMSNotifications,
                PhoneNumber = userPreferenceInfo.UserProfile.PhoneNumber,
                MyDelegationUsers = UserDelegationMapper.Map(userPreferenceInfo.MyDelegations, CultureName),
                DefaultDisplay = userPreferenceInfo.DefaultDisplay,
                DefaultAssignmentPaper = userPreferenceInfo.DefaultAssignmentPaper,
            };
            if (userPreferenceInfo.UserTrayPreferencesInfo != null)
            {
                userPreferenceDTO.UserTrays = UserTrayPreferenceInfoMapper.Map(userPreferenceInfo.UserTrayPreferencesInfo);
            }
            return userPreferenceDTO;
        }

    }
}