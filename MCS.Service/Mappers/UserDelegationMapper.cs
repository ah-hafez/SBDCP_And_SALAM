using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserDelegationMapper
    {
        public static UserDelegation Map(UserDelegationDTO delegationUserDTO)
        {
            if (delegationUserDTO == null)
            {
                return null;
            }
            UserDelegation userDelegation = new UserDelegation()
            {
                Id = delegationUserDTO.Id,
                FromDate = delegationUserDTO.FromDate,
                ToDate = delegationUserDTO.ToDate,
                FromDateH = delegationUserDTO.FromDateH,
                ToDateH = delegationUserDTO.ToDateH,
                UserProfileId = delegationUserDTO.DirectedToId,
                OrgUnitId = delegationUserDTO.OrgUnitId,
                ConfidentialityId = delegationUserDTO.ConfidentialityId,
                PriorityId = delegationUserDTO.PriorityId,
                TransactionTypeId = delegationUserDTO.SourceTypesId,
                UserPreferenceId = delegationUserDTO.UserPreferenceId,
                StatusId = delegationUserDTO.StatusId,
                RejectionReason = delegationUserDTO.RejectionReason,
            };

            return userDelegation;
        }

        public static UserDelegationDTO Map(UserDelegation userDelegation)
        {
            if (userDelegation == null)
            {
                return null;
            }
            UserDelegationDTO userDelegationEditDTO = new UserDelegationDTO()
            {
                Id = userDelegation.Id,
                FromDateH = userDelegation.FromDateH,
                ToDateH = userDelegation.ToDateH,
                DirectedTo = userDelegation.UserProfile.LocalName,
                OrgUnit = userDelegation.OrgUnit != null ? userDelegation.OrgUnit.LocalName : string.Empty,
                Confidentiality = userDelegation.Confidentiality != null ? userDelegation.Confidentiality.LocalName : string.Empty,
                Priority = userDelegation.Priority != null ? userDelegation.Priority.Text : string.Empty,
                SourceType = userDelegation.TransactionType != null ? userDelegation.TransactionType.Text : string.Empty,
                FromDate = userDelegation.FromDate,
                ToDate = userDelegation.ToDate,
                PriorityId = userDelegation.Priority != null ? userDelegation.Priority.Id : (int?)null,
                ConfidentialityId = userDelegation.Confidentiality != null ? userDelegation.Confidentiality.Id : (int?)null,
                SourceTypesId = userDelegation.TransactionType != null ? userDelegation.TransactionType.Id : (int?)null,
                OrgUnitId = userDelegation.OrgUnitId,
                StatusId = userDelegation.StatusId,
                Status = userDelegation.Status.Text,
                RejectionReason = userDelegation.RejectionReason,
                DirectedToId = userDelegation.UserProfileId,
                UserPreferenceId = userDelegation.UserPreferenceId
            };

            return userDelegationEditDTO;
        }

        public static List<UserDelegation> Map(IList<AddUserDelegationDTO> delegationUserDTOs)
        {
            if (delegationUserDTOs == null || !delegationUserDTOs.Any())
            {
                return null;
            }
            List<UserDelegation> userDelegations = delegationUserDTOs
                .Select(delegationUserDTO => new UserDelegation()
                {
                    FromDate = delegationUserDTO.FromDate,
                    ToDate = delegationUserDTO.ToDate,
                    FromDateH = delegationUserDTO.FromDateH,
                    ToDateH = delegationUserDTO.ToDateH,
                    UserProfileId = delegationUserDTO.DirectedToId,
                    OrgUnitId = delegationUserDTO.OrgUnitId,
                    ConfidentialityId = delegationUserDTO.ConfidentialityId,
                    PriorityId = delegationUserDTO.PriorityId,
                    TransactionTypeId = delegationUserDTO.SourceTypesId,
                    StatusId = delegationUserDTO.StatusId,
                    RejectionReason = delegationUserDTO.RejectionReason
                }).ToList();

            return userDelegations;
        }

        public static List<UserDelegationDTO> Map(IList<UserDelegation> delegationUserDTOs, string culture = "ar")
        {
            if (delegationUserDTOs == null || !delegationUserDTOs.Any())
            {
                return new List<UserDelegationDTO>();
            }
            List<UserDelegationDTO> userDelegationDTOs = delegationUserDTOs
                .Select(userDelegation => new UserDelegationDTO()
                {
                    Id = userDelegation.Id,
                    FromDateH = userDelegation.FromDateH,
                    ToDateH = userDelegation.ToDateH,
                    DirectedTo = userDelegation.UserProfile.LocalName,
                    OrgUnit = userDelegation.OrgUnit != null ? userDelegation.OrgUnit.LocalName : string.Empty,
                    Confidentiality = userDelegation.Confidentiality != null ? userDelegation.Confidentiality.LocalName : string.Empty,
                    Priority = userDelegation.Priority != null ? userDelegation.Priority.Text : string.Empty,
                    SourceType = userDelegation.TransactionType != null ? userDelegation.TransactionType.Text : string.Empty,
                    FromDate = userDelegation.FromDate,
                    ToDate = userDelegation.ToDate,
                    PriorityId = userDelegation.Priority != null ? userDelegation.Priority.Id : (int?)null,
                    ConfidentialityId = userDelegation.Confidentiality != null ? userDelegation.Confidentiality.Id : (int?)null,
                    SourceTypesId = userDelegation.TransactionType != null ? userDelegation.TransactionType.Id : (int?)null,
                    OrgUnitId = userDelegation.OrgUnitId,
                    StatusId = userDelegation.StatusId,
                    Status = !string.IsNullOrWhiteSpace(userDelegation.Status.Text)? userDelegation.Status.Text : userDelegation.Status.Localizations.Where(l => l.Culture.ShortName == culture).FirstOrDefault().Text,
                    RejectionReason = userDelegation.RejectionReason,
                    DirectedToId = userDelegation.UserProfileId,
                    ReceiveCopy = userDelegation.ReceiveCopy,
                    ShowTransaction = userDelegation.ShowTransaction,
                    UserPreferenceName = userDelegation.UserPreference != null ? userDelegation.UserPreference.LocalName : string.Empty,
                    TransacionCategoryIds = userDelegation.TransacionCategoryIds,
                    TransacionConfidentialityIds = userDelegation.TransacionConfidentialityIds
                }).ToList();

            return userDelegationDTOs;
        }

        public static List<UserDelegation> Map(IList<UserDelegationDTO> delegationUserDTOs)
        {
            if (delegationUserDTOs == null || !delegationUserDTOs.Any())
            {
                return new List<UserDelegation>();
            }
            List<UserDelegation> userDelegations = delegationUserDTOs
                .Select(delegationUserDTO => new UserDelegation()
                {
                    Id = delegationUserDTO.Id,
                    FromDate = delegationUserDTO.FromDate,
                    ToDate = delegationUserDTO.ToDate,
                    FromDateH = delegationUserDTO.FromDateH,
                    ToDateH = delegationUserDTO.ToDateH,
                    UserProfileId = delegationUserDTO.DirectedToId,
                    OrgUnitId = delegationUserDTO.OrgUnitId,
                    ConfidentialityId = delegationUserDTO.ConfidentialityId,
                    PriorityId = delegationUserDTO.PriorityId,
                    TransactionTypeId = delegationUserDTO.SourceTypesId,
                    StatusId = delegationUserDTO.StatusId,
                    RejectionReason = delegationUserDTO.RejectionReason,
                    ReceiveCopy = delegationUserDTO.ReceiveCopy,
                    ShowTransaction = delegationUserDTO.ShowTransaction,
                    TransacionCategoryIds = delegationUserDTO.SelectedTransactionCategoriesIdList,
                    TransacionConfidentialityIds = delegationUserDTO.SelectedConfidentialityLevelsIdList
                }).ToList();

            return userDelegations;
        }

    }
}
