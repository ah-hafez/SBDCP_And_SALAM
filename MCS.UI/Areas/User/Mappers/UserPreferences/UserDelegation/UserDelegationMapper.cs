using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.UserPreferences.UserDelegation;

namespace MCS.UI.Areas.User.Mappers.UserPreferences.UserDelegation
{
    public static class UserDelegationMapper
    {
        public static List<UserDelegationVM> Map(IList<UserDelegationDTO> userDelegationDTOs)
        {
            if (userDelegationDTOs == null || !userDelegationDTOs.Any())
            {
                return new List<UserDelegationVM>();
            }
            List<UserDelegationVM> userDelegationVMs = userDelegationDTOs
                .Select(userDelegationDTO => new UserDelegationVM()
                {
                    Id = userDelegationDTO.Id,
                    Confidentiality = userDelegationDTO.Confidentiality,
                    ConfidentialityId = userDelegationDTO.ConfidentialityId,
                    DirectedTo = userDelegationDTO.DirectedTo,
                    DirectedToId = userDelegationDTO.DirectedToId,
                    FromDate = userDelegationDTO.FromDate,
                    FromDateH = userDelegationDTO.FromDateH,
                    FromDateG = userDelegationDTO.FromDate.ToShortDateString(),
                    ToDateG = userDelegationDTO.ToDate.ToShortDateString(),
                    OrgUnit = userDelegationDTO.OrgUnit,
                    OrgUnitId = userDelegationDTO.OrgUnitId,
                    Priority = userDelegationDTO.Priority,
                    PriorityId = userDelegationDTO.PriorityId,
                    SourceType = userDelegationDTO.SourceType,
                    SourceTypesId = userDelegationDTO.SourceTypesId,
                    ToDate = userDelegationDTO.ToDate,
                    ToDateH = userDelegationDTO.ToDateH,
                    Status = userDelegationDTO.Status,
                    StatusId = userDelegationDTO.StatusId,
                    RejectionReason = userDelegationDTO.RejectionReason,
                    ReceiveCopy=userDelegationDTO.ReceiveCopy,
                    ShowTransaction= userDelegationDTO.ShowTransaction,
                    DirectedFrom = userDelegationDTO.UserPreferenceName,
                    TransacionCategoryIds = userDelegationDTO.TransacionCategoryIds,
                    TransacionConfidentialityIds = userDelegationDTO.TransacionConfidentialityIds
                }).ToList();

            return userDelegationVMs;
        }
        public static List<UserDelegationDTO> Map(IList<UserDelegationVM> userDelegationVMs)
        {
            if (userDelegationVMs == null || !userDelegationVMs.Any())
            {
                return new List<UserDelegationDTO>();
            }
            List<UserDelegationDTO> userDelegationDTOs = userDelegationVMs
                .Select(userDelegationVM => new UserDelegationDTO()
                {
                    Id = userDelegationVM.Id,
                    Confidentiality = userDelegationVM.Confidentiality,
                    ConfidentialityId = userDelegationVM.ConfidentialityId,
                    DirectedTo = userDelegationVM.DirectedTo,
                    FromDate = userDelegationVM.FromDate,
                    FromDateH = userDelegationVM.FromDateH,
                    OrgUnit = userDelegationVM.OrgUnit,
                    Priority = userDelegationVM.Priority,
                    PriorityId = userDelegationVM.PriorityId,
                    SourceType = userDelegationVM.SourceType,
                    SourceTypesId = userDelegationVM.SourceTypesId,
                    ToDate = userDelegationVM.ToDate,
                    ToDateH = userDelegationVM.ToDateH,
                    DirectedToId = userDelegationVM.DirectedToId,
                    OrgUnitId = userDelegationVM.OrgUnitId,
                    RejectionReason = userDelegationVM.RejectionReason,
                    StatusId = userDelegationVM.StatusId,
                    Status = userDelegationVM.Status,
                    ReceiveCopy= userDelegationVM.ReceiveCopy,
                    ShowTransaction= userDelegationVM.ShowTransaction   ,
                    SelectedTransactionCategoriesIdList = userDelegationVM.SelectedTransactionCategoriesIdList,
                    SelectedConfidentialityLevelsIdList = userDelegationVM.SelectedConfidentialityLevelsIdList
                }).ToList();

            return userDelegationDTOs;
        }
        public static List<AddUserDelegationDTO> Map(IList<AddUserDelegationVM> addUserDelegationVMs)
        {
            if (addUserDelegationVMs == null || !addUserDelegationVMs.Any())
            {
                return new List<AddUserDelegationDTO>();
            }
            List<AddUserDelegationDTO> addUserDelegationDTOs = addUserDelegationVMs
                .Select(addUserDelegationVM => new AddUserDelegationDTO()
                {
                    Id = addUserDelegationVM.Id,
                    ToDateH = addUserDelegationVM.ToDateH,
                    ToDate = addUserDelegationVM.ToDate,
                    SourceTypesId = addUserDelegationVM.SourceTypesId,
                    PriorityId = addUserDelegationVM.PriorityId,
                    ConfidentialityId = addUserDelegationVM.ConfidentialityId,
                    DirectedToId = addUserDelegationVM.DirectedToId,
                    FromDate = addUserDelegationVM.FromDate,
                    FromDateH = addUserDelegationVM.FromDateH,
                    OrgUnitId = addUserDelegationVM.OrgUnitId,
                    UserId = addUserDelegationVM.UserId
                }).ToList();

            return addUserDelegationDTOs;
        }
        public static List<AddUserDelegationVM> Map(IList<AddUserDelegationDTO> addUserDelegationDTOs)
        {
            if (addUserDelegationDTOs == null || !addUserDelegationDTOs.Any())
            {
                return new List<AddUserDelegationVM>();
            }
            List<AddUserDelegationVM> addUserDelegationVMs = addUserDelegationDTOs
                .Select(addUserDelegationDTO => new AddUserDelegationVM()
                {
                    Id = addUserDelegationDTO.Id,
                    ToDateH = addUserDelegationDTO.ToDateH,
                    ToDate = addUserDelegationDTO.ToDate,
                    SourceTypesId = addUserDelegationDTO.SourceTypesId,
                    PriorityId = addUserDelegationDTO.PriorityId,
                    ConfidentialityId = addUserDelegationDTO.ConfidentialityId,
                    DirectedToId = addUserDelegationDTO.DirectedToId,
                    FromDate = addUserDelegationDTO.FromDate,
                    FromDateH = addUserDelegationDTO.FromDateH,
                    OrgUnitId = addUserDelegationDTO.OrgUnitId,
                    UserId = addUserDelegationDTO.UserId
                }).ToList();

            return addUserDelegationVMs;
        }
        public static List<EditUserDelegationVM> Map(IList<EditUserDelegationDTO> editUserDelegationDTOs)
        {
            if (editUserDelegationDTOs == null || !editUserDelegationDTOs.Any())
            {
                return new List<EditUserDelegationVM>();
            }
            List<EditUserDelegationVM> editUserDelegationVMs = editUserDelegationDTOs
                .Select(editUserDelegationDTO => new EditUserDelegationVM()
                {
                    Id = editUserDelegationDTO.Id,
                    ToDateH = editUserDelegationDTO.ToDateH,
                    ToDate = editUserDelegationDTO.ToDate,
                    SourceTypesId = editUserDelegationDTO.SourceTypesId,
                    PriorityId = editUserDelegationDTO.PriorityId,
                    ConfidentialityId = editUserDelegationDTO.ConfidentialityId,
                    DirectedToId = editUserDelegationDTO.DirectedToId,
                    FromDate = editUserDelegationDTO.FromDate,
                    FromDateH = editUserDelegationDTO.FromDateH,
                    OrgUnitId = editUserDelegationDTO.OrgUnitId,
                    PreferenceId = editUserDelegationDTO.PreferenceId
                }).ToList();

            return editUserDelegationVMs;
        }
        public static EditUserDelegationVM Map(EditUserDelegationDTO editUserDelegationDTO)
        {
            if (editUserDelegationDTO == null)
            {
                return new EditUserDelegationVM();
            }

            var editUserDelegationVMs = new EditUserDelegationVM()
            {
                Id = editUserDelegationDTO.Id,
                ToDateH = editUserDelegationDTO.ToDateH,
                ToDate = editUserDelegationDTO.ToDate,
                SourceTypesId = editUserDelegationDTO.SourceTypesId,
                PriorityId = editUserDelegationDTO.PriorityId,
                ConfidentialityId = editUserDelegationDTO.ConfidentialityId,
                DirectedToId = editUserDelegationDTO.DirectedToId,
                FromDate = editUserDelegationDTO.FromDate,
                FromDateH = editUserDelegationDTO.FromDateH,
                OrgUnitId = editUserDelegationDTO.OrgUnitId,
                PreferenceId = editUserDelegationDTO.PreferenceId
            };
            return editUserDelegationVMs;
        }
        public static List<EditUserDelegationDTO> Map(IList<EditUserDelegationVM> editUserDelegationVMs)
        {
            if (editUserDelegationVMs == null || !editUserDelegationVMs.Any())
            {
                return new List<EditUserDelegationDTO>();
            }
            List<EditUserDelegationDTO> editUserDelegationDTOs = editUserDelegationVMs
                .Select(editUserDelegationVM => new EditUserDelegationDTO()
                {
                    Id = editUserDelegationVM.Id,
                    ToDateH = editUserDelegationVM.ToDateH,
                    ToDate = editUserDelegationVM.ToDate,
                    SourceTypesId = editUserDelegationVM.SourceTypesId,
                    PriorityId = editUserDelegationVM.PriorityId,
                    ConfidentialityId = editUserDelegationVM.ConfidentialityId,
                    DirectedToId = editUserDelegationVM.DirectedToId,
                    FromDate = editUserDelegationVM.FromDate,
                    FromDateH = editUserDelegationVM.FromDateH,
                    OrgUnitId = editUserDelegationVM.OrgUnitId,
                    PreferenceId = editUserDelegationVM.PreferenceId
                }).ToList();

            return editUserDelegationDTOs;
        }
    }
}