using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class PriorityExceptionMapper
    {
        public static PriorityException Map(PriorityExceptionDTO priorityExceptionDTO)
        {
            if (priorityExceptionDTO == null)
            {
                return null;
            }

            PriorityException priorityException = new PriorityException
            {
                Id = priorityExceptionDTO.Id,
                OrgUnitId = priorityExceptionDTO.OrgUnitId,
                UserProfileId = priorityExceptionDTO.UserId,
                LateOnUsersAfter = priorityExceptionDTO.LateOnUsersAfter,
                PriorityId = priorityExceptionDTO.PriorityId
            };

            return priorityException;
        }
        public static PriorityExceptionDTO Map(PriorityException priorityException, string cultureName)
        {
            if (priorityException == null)
            {
                return null;
            }

            PriorityExceptionDTO priorityExceptionDTO = new PriorityExceptionDTO
            {
                Id = priorityException.Id,
                OrgUnitId = priorityException.OrgUnit.Id,
                OrgUnitName = priorityException.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                UserName = priorityException.UserProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                UserId = priorityException.UserProfile.Id,
                LateOnUsersAfter = priorityException.LateOnUsersAfter,
                PriorityId = priorityException.Priority.Id
            };

            return priorityExceptionDTO;
        }

        public static List<PriorityExceptionDTO> Map(List<PriorityException> priorityExceptions, string cultureName)
        {
            if (!priorityExceptions.Any())
            {
                return null;
            }

            return priorityExceptions.Select(pe => Map(pe, cultureName)).ToList();
        }
        public static List<PriorityException> Map(List<PriorityExceptionDTO> priorityExceptionDTOs)
        {
            if (!priorityExceptionDTOs.Any())
            {
                return null;
            }

            return priorityExceptionDTOs.Select(pe => Map(pe)).ToList();
        }
    }
}