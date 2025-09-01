using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.File;

namespace MCS.UI.Areas.User.Mappers.File
{
    public static class TransactionAssignmentInfoMapper
    {
        public static List<TransactionAssignmentInfoVM> Map(IList<TransactionAssignmentInfoDTO> transactionAssignmentInfoDTOs)
        {
            if (transactionAssignmentInfoDTOs == null || !transactionAssignmentInfoDTOs.Any())
            {
                return null;
            }
            List<TransactionAssignmentInfoVM> transactionAssignmentInfoVMs = transactionAssignmentInfoDTOs
                .Select(transactionAssignmentInfoDTO => new TransactionAssignmentInfoVM()
                {
                    Id = transactionAssignmentInfoDTO.Id,
                    Action = transactionAssignmentInfoDTO.Action,
                    ActionId = transactionAssignmentInfoDTO.ActionId,
                    Date = transactionAssignmentInfoDTO.Date,
                    DateH = transactionAssignmentInfoDTO.DateH,
                    FromEntity = transactionAssignmentInfoDTO.FromEntity,
                    FromEntityId = transactionAssignmentInfoDTO.FromEntityId,
                    FromUser = transactionAssignmentInfoDTO.FromUser,
                    FromUserId = transactionAssignmentInfoDTO.FromUserId,
                    HasCollaboration = transactionAssignmentInfoDTO.HasCollaboration,
                    IsLate = transactionAssignmentInfoDTO.IsLate,
                    ToEntity = transactionAssignmentInfoDTO.ToEntity,
                    ToEntityId = transactionAssignmentInfoDTO.ToEntityId,
                    ToUser = transactionAssignmentInfoDTO.ToUser,
                    ToUserId = transactionAssignmentInfoDTO.ToUserId,
                    Viewed = transactionAssignmentInfoDTO.Viewed,
                    Description = transactionAssignmentInfoDTO.Description,
                }).ToList();

            return transactionAssignmentInfoVMs;
        }
        public static List<TransactionAssignmentInfoDTO> Map(IList<TransactionAssignmentInfoVM> transactionAssignmentInfoVMs)
        {
            if (transactionAssignmentInfoVMs == null || !transactionAssignmentInfoVMs.Any())
            {
                return new List<TransactionAssignmentInfoDTO>();
            }
            List<TransactionAssignmentInfoDTO> transactionAssignmentInfoDTOs = transactionAssignmentInfoVMs
                .Select(transactionAssignmentInfoVM => new TransactionAssignmentInfoDTO()
                {
                    Id = transactionAssignmentInfoVM.Id,
                    Action = transactionAssignmentInfoVM.Action,
                    ActionId = transactionAssignmentInfoVM.ActionId,
                    Date = transactionAssignmentInfoVM.Date,
                    DateH = transactionAssignmentInfoVM.DateH,
                    FromEntity = transactionAssignmentInfoVM.FromEntity,
                    FromEntityId = transactionAssignmentInfoVM.FromEntityId,
                    FromUser = transactionAssignmentInfoVM.FromUser,
                    FromUserId = transactionAssignmentInfoVM.FromUserId,
                    HasCollaboration = transactionAssignmentInfoVM.HasCollaboration,
                    IsLate = transactionAssignmentInfoVM.IsLate,
                    ToEntity = transactionAssignmentInfoVM.ToEntity,
                    ToEntityId = transactionAssignmentInfoVM.ToEntityId,
                    ToUser = transactionAssignmentInfoVM.ToUser,
                    ToUserId = transactionAssignmentInfoVM.ToUserId,
                    Description = transactionAssignmentInfoVM.Description,
                }).ToList();

            return transactionAssignmentInfoDTOs;
        }
    }
}