using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Encryption;
using MCS.DTO.Escalation;
using MCS.UI.Areas.Admin.Models.Escalation;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class EscalationMapper
    {

        public static List<EscalationDTO> Map(IList<EscalationVM> escalationVMs)
        {
            if (escalationVMs == null || !escalationVMs.Any())
            { return new List<EscalationDTO>(); }
            List<EscalationDTO> escalationDTOs = escalationVMs
                .Select(b => new EscalationDTO
                {
                    Id = b.Id,
                    EscalationAfterDays = b.EscalationAfterDays,
                    TransactionCategory = b.TransactionCategory,
                    TransactionCategoryName = b.TransactionCategoryName,
                    EscalationActionId = b.EscalationActionId,
                    EscalationAction = b.EscalationAction,
                    EscalationTo = b.EscalationTo,
                    EscalationToId = b.EscalationToId,
                    Priority = b.Priority,
                    PriorityId = b.PriorityId

                }).ToList();
            return escalationDTOs;
        }
        public static List<EscalationVM> Map(IList<EscalationDTO> escalationDTOs)
        {
            if (escalationDTOs == null || !escalationDTOs.Any())
            { return new List<EscalationVM>(); }
            List<EscalationVM> escalationVMs = escalationDTOs
                .Select(b => new EscalationVM
                {
                    Id = b.Id,
                    EscalationAfterDays = b.EscalationAfterDays,
                    TransactionCategory = b.TransactionCategory,
                    TransCategoryIdEncrypt = AESEncrytDecry.Base64Encode(b.TransactionCategory.ToString()),
                    TransactionCategoryName = b.TransactionCategoryName,
                    EscalationActionId = b.EscalationActionId,
                    EscalationAction = b.EscalationAction,
                    EscalationTo = b.EscalationTo,
                    EscalationToId = b.EscalationToId,
                    Priority = b.Priority,
                    PriorityId = b.PriorityId

                }).ToList();
            return escalationVMs;
        }
        public static List<EscalationGridVM> MapGrid(IList<EscalationDTO> escalationDTOs)
        {
            if (escalationDTOs == null || !escalationDTOs.Any())
            { return new List<EscalationGridVM>(); }
            List<EscalationGridVM> escalationVMs = escalationDTOs
                .Select(b => new EscalationGridVM
                {
                    EscalationAfterDays = b.EscalationAfterDays,
                    EscalationActionId = b.EscalationActionId,
                    EscalationAction = b.EscalationAction,
                    EscalationTo = b.EscalationTo,
                    EscalationToId = b.EscalationToId,
                    Priority = b.Priority,
                    PriorityId = b.PriorityId

                }).ToList();
            return escalationVMs;
        }

        public static EscalationDTO Map(EscalationVM escalationVM)
        {
            if (escalationVM != null)
            {
                return new EscalationDTO
                {
                    Id = escalationVM.Id,
                    EscalationAfterDays = escalationVM.EscalationAfterDays,
                    TransactionCategory = escalationVM.TransactionCategory,
                    TransactionCategoryName = escalationVM.TransactionCategoryName,
                    EscalationActionId = escalationVM.EscalationActionId,
                    EscalationAction = escalationVM.EscalationAction,
                    EscalationTo = escalationVM.EscalationTo,
                    EscalationToId = escalationVM.EscalationToId,
                    Priority = escalationVM.Priority,
                    PriorityId = escalationVM.PriorityId
                };
            }
            return null;
        }

        public static EscalationVM Map(EscalationDTO escalation)
        {
            if (escalation != null)
            {
                return new EscalationVM
                {
                    Id = escalation.Id,
                    EscalationAfterDays = escalation.EscalationAfterDays,
                    TransactionCategory = escalation.TransactionCategory,
                    TransactionCategoryName = escalation.TransactionCategoryName,
                    EscalationActionId = escalation.EscalationActionId,
                    EscalationAction = escalation.EscalationAction,
                    EscalationTo = escalation.EscalationTo,
                    EscalationToId = escalation.EscalationToId,
                    Priority = escalation.Priority,
                    PriorityId = escalation.PriorityId
                };
            }
            return null;
        }
    }
}