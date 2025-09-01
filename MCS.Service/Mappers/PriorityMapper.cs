using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class PriorityMapper
    {
        public static Priority Map(PriorityAddDTO priorityAddDTO)
        {
            if (priorityAddDTO == null)
                return null;

            TransactionCategories transactionCategories =
                TransactionCategoryMapper.Map(priorityAddDTO.TransactionCategories);

            Priority priority = new Priority()
            {
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = priorityAddDTO.Description != null ? LocalizationIdentifierMapper.Map(priorityAddDTO.Description) : null,
                HasDate = priorityAddDTO.HasDate
            };

            return priority;
        }

        public static Priority Map(PriorityEditDTO priorityEditDTO)
        {
            if (priorityEditDTO == null)
                return null;

            TransactionCategories transactionCategories =
                TransactionCategoryMapper.Map(priorityEditDTO.TransactionCategories);

            Priority priority = new Priority()
            {
                Id = priorityEditDTO.Id,
                TransactionCategories = transactionCategories,
                LocalizationIdentifier = priorityEditDTO.Description != null ? LocalizationIdentifierMapper.Map(priorityEditDTO.Description) : null,
                HasDate = priorityEditDTO.HasDate,
                HasPriorityExceptions = priorityEditDTO.HasPriorityExceptions,
                LateForEntity = priorityEditDTO.LateForEntity,
                LateForUser = priorityEditDTO.LateForUser,
                Sort = priorityEditDTO.Sort,
                ProcessPeriod = priorityEditDTO.ProcessPeriod
            };

            return priority;
        }

        public static PriorityEditDTO Map(Priority priority, string cultureName)
        {
            if (priority == null)
                return null;

            PriorityEditDTO priorityEditDTO = new PriorityEditDTO()
            {
                Id = priority.Id,
                Description = priority.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(priority.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(priority.TransactionCategories, cultureName),
                HasDate = priority.HasDate,
                HasPriorityExceptions = priority.HasPriorityExceptions,
                LateForEntity = priority.LateForEntity,
                LateForUser = priority.LateForUser,
                PriorityExceptions = PriorityExceptionMapper.Map(priority.PriorityExceptions, cultureName),
                Sort = priority.Sort,
                ProcessPeriod = priority.ProcessPeriod
            };

            return priorityEditDTO;
        }

        public static List<PriorityDTO> Map(IList<Priority> priorities, string cultureName)
        {
            if (priorities == null || !priorities.Any())
            {
                return null;
            }
            List<PriorityDTO> priorityDTOs = priorities
                .Select(priorityDTO => new PriorityDTO()
                {
                    Id = priorityDTO.Id,
                    LocalName = priorityDTO.Text,
                    HasDate = priorityDTO.HasDate,
                    TransactionCategories = TransactionCategoryMapper.Map(priorityDTO.TransactionCategories, cultureName),
                    Description = priorityDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(priorityDTO.LocalizationIdentifier.Localizations) : null,
                    HasPriorityExceptions = priorityDTO.HasPriorityExceptions,
                    LateForEntity = priorityDTO.LateForEntity,
                    LateForUser = priorityDTO.LateForUser,
                    Sort = priorityDTO.Sort,
                    ProcessPeriod = priorityDTO.ProcessPeriod
                }).ToList();


            return priorityDTOs;
        }

        public static PriorityDTO MapPriority(Priority priority, string cultureName)
        {
            if (priority == null)
            {
                return null;
            }

            PriorityDTO priorityDTO = new PriorityDTO()
            {
                Id = priority.Id,
                LocalName = priority.Text,
                HasDate = priority.HasDate,
                TransactionCategories = TransactionCategoryMapper.Map(priority.TransactionCategories, cultureName),
                HasPriorityExceptions = priority.HasPriorityExceptions,
                LateForEntity = priority.LateForEntity,
                LateForUser = priority.LateForUser,
                Sort = priority.Sort,
                ProcessPeriod = priority.ProcessPeriod
            };

            if (priority.LocalizationIdentifier != null)
            {
                priorityDTO.Description = priority.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(priority.LocalizationIdentifier.Localizations) : null;
            }

            return priorityDTO;
        }
    }
}
