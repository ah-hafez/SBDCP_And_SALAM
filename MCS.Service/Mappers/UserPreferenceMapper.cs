using System.Collections.Generic;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserPreferenceMapper
    {
        public static UserPreference Map(UserPreferenceDTO userPreferenceDTO)
        {
            if (userPreferenceDTO == null)
            {
                return null;
            }

            UserPreference userPreference = new UserPreference()
            {
                Id = userPreferenceDTO.Id,
                CultureId = userPreferenceDTO.LanguageId,
                IsDelegationEnabled = userPreferenceDTO.ActivateDelegation,
                SignaturePassword = userPreferenceDTO.PasswordForSignature,
                MarkingDoc = userPreferenceDTO.MarkingDoc,
                Signature = userPreferenceDTO.SignatureDoc,
                SignatureBehalf = userPreferenceDTO.SignatureBehalfDoc,
                SignatureCommand = userPreferenceDTO.SignatureCommandDoc,
                MessageSignatureDoc = userPreferenceDTO.MessageSignatureDoc,
                SealSignatureDoc = userPreferenceDTO.SealSignatureDoc,
                UserProfileId = userPreferenceDTO.UserId,
                SignaturePasswordText = userPreferenceDTO.SignaturePasswordText,
                FollowUpOrgId=userPreferenceDTO.FollowUpOrgId,
                FollowUpUserId= userPreferenceDTO.FollowUpUserId,
                UserDelegations = UserDelegationMapper.Map(userPreferenceDTO.DelegationUsers),
                NotificationSubscriptions = NotificationSubscriptionMapper.Map(userPreferenceDTO.NotificationSubscriptions),
                ThemeId = userPreferenceDTO.Theme,
                SMSNotifications =userPreferenceDTO.SMSNotifications,
                DefaultDisplay = userPreferenceDTO.DefaultDisplay,
                DefaultAssignmentPaper = userPreferenceDTO.DefaultAssignmentPaper,
            };

            userPreference.UserTrayPreferences = new List<UserTrayPreference>();

            foreach (UserTrayPreferencesDTO userTrayPreferencesDTO in userPreferenceDTO.UserTrays)
            {
                if (userTrayPreferencesDTO.IsSelected)
                    userPreference.UserTrayPreferences.Add(new UserTrayPreference() { TrayId = userTrayPreferencesDTO.Id });
            }

            return userPreference;
        }
    }
}