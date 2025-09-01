using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class SuggestedTopicMapper
    {

        public static List<SuggestedTopic> Map(List<SuggestedTopicDTO> suggestedTopicDTOs)
        {
            if (suggestedTopicDTOs == null || !suggestedTopicDTOs.Any())
            {
                return null;
            }


            List<SuggestedTopic> suggestedTopics = suggestedTopicDTOs.Select(suggestedTopic => new SuggestedTopic()
            {
                Id = suggestedTopic.Id,
                ParentId = suggestedTopic.ParentId,
                LocalizationIdentifier = suggestedTopic.Description != null ? LocalizationIdentifierMapper.Map(suggestedTopic.Description) : null,
                IsGroup = suggestedTopic.IsGroup,
                IsNew = suggestedTopic.IsNew,
                IsDeleted = suggestedTopic.IsDeleted,
                SubjectOrgUnits = MapOrgUnits(suggestedTopic.OrgUnits)
            }).ToList();
            return suggestedTopics;




        }

        public static List<SuggestedTopicDTO> Map(IList<SuggestedTopic> suggestedTopics)
        {
            if (suggestedTopics == null || !suggestedTopics.Any())
            {
                return null;
            }
            List<SuggestedTopicDTO> suggestedTopicDTOs = suggestedTopics.Select(suggestedTopicDTO => new SuggestedTopicDTO()
            {
                Id = suggestedTopicDTO.Id,
                ParentId = suggestedTopicDTO.ParentId,
                IsGroup = suggestedTopicDTO.IsGroup,
                OrgUnits = MapOrgUnits(suggestedTopicDTO.SubjectOrgUnits),
                LocalName = suggestedTopicDTO.Text,
                Description = suggestedTopicDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(suggestedTopicDTO.LocalizationIdentifier.Localizations) : null
            }).ToList();

            return suggestedTopicDTOs;
        }

        private static IList<SubjectOrgUnit> MapOrgUnits(List<int> organizationUnitIds)
        {
            if (organizationUnitIds == null || !organizationUnitIds.Any())
            {
                return null;
            }
            IList<SubjectOrgUnit> organizationUnits = new List<SubjectOrgUnit>();

            if (organizationUnitIds != null)
            {
                foreach (var id in organizationUnitIds)
                {
                    organizationUnits.Add(new SubjectOrgUnit() { OrgUnitId = id });
                }
            }

            return organizationUnits;
        }

        private static List<int> MapOrgUnits(IList<SubjectOrgUnit> subjectOrgUnits)
        {
            if (subjectOrgUnits == null || !subjectOrgUnits.Any())
            {
                return null;
            }
            List<int> organizationUnits = new List<int>();

            if (subjectOrgUnits != null)
            {
                foreach (SubjectOrgUnit subjectOrgUnit in subjectOrgUnits)
                {
                    organizationUnits.Add(subjectOrgUnit.OrgUnitId);
                }
            }

            return organizationUnits;
        }
    }
}