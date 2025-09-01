using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO.Escalation;

namespace MCS.Service.Mappers
{
    public static class EscalationMapper
    {
        public static List<EscalationDTO> Map(IList<Escalation> escalation)
        {
            if (escalation == null || !escalation.Any())
            {
                return new List<EscalationDTO>();
            }
            List<EscalationDTO> escalationDTOs = escalation
                .Select(e => new EscalationDTO()
                {
                    EscalationAction = e.EscalationAction.Text,
                    EscalationActionId = e.EscalationAction.Id,
                    EscalationAfterDays = e.EscalationAfterDays,
                    EscalationTo = e.EscalationTo.Text,
                    EscalationToId = e.EscalationTo.Id,
                    Id = e.Id,
                    Priority = e.Priority.Text,
                    PriorityId = e.Priority.Id,
                    TransactionCategory = e.TransactionCategory.Id,
                    TransactionCategoryName = e.TransactionCategory.Text
                }).ToList();

            return escalationDTOs;
        }

        public static Escalation Map(EscalationDTO escalationDTO)
        {
            if (escalationDTO == null)
            {
                return new Escalation();
            }

            Escalation escalation = new Escalation()
            {
                EscalationToId = escalationDTO.EscalationToId,
                TransactionCategoryId = escalationDTO.TransactionCategory,
                PriorityId = escalationDTO.PriorityId,
                Id = escalationDTO.Id,
                EscalationAfterDays = escalationDTO.EscalationAfterDays,
                EscalationActionId = escalationDTO.EscalationActionId
            };

            return escalation;
        }


        public static EscalationDTO Map(Escalation escalation, string cultureName)
        {
            if (escalation != null)
            {
                EscalationDTO escalationDTO = new EscalationDTO()
                {
                    EscalationToId = escalation.EscalationToId,
                    EscalationTo = escalation.EscalationTo.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                    EscalationAction = escalation.EscalationAction.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                    EscalationActionId = escalation.EscalationActionId,
                    TransactionCategory = escalation.TransactionCategoryId,
                    TransactionCategoryName=escalation.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                    PriorityId = escalation.PriorityId,
                    Priority = escalation.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                    Id = escalation.Id,
                    EscalationAfterDays = escalation.EscalationAfterDays,

                };

                return escalationDTO;
            }
            return new EscalationDTO();
        }
    }
}