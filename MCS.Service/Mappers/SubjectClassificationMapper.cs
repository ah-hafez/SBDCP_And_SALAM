using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class SubjectClassificationMapper
    {
        

        public static List<SubjectClassification> Map(List<SubjectClassificationDTO> subjectClassificationDTOs)
        {
            if (subjectClassificationDTOs == null || !subjectClassificationDTOs.Any())
            {
                return null;
            }
            List<SubjectClassification> subjectClassifications = subjectClassificationDTOs
                .Select(subjectClassification => new SubjectClassification()
            {
                Id = subjectClassification.Id,
                ParentId = subjectClassification.ParentId,
                LocalizationIdentifier = subjectClassification.Description !=null ? LocalizationIdentifierMapper.Map(subjectClassification.Description):null,
                IsGroup = subjectClassification.IsGroup,
                IsNew = subjectClassification.IsNew,
                IsDeleted = subjectClassification.IsDeleted,
                SubjectOrgUnits = MapOrgUnits(subjectClassification.OrgUnits)
            }).ToList();

            

            return subjectClassifications;
        }

        public static List<SubjectClassificationDTO> Map(IList<SubjectClassification> subjectClassifications)
        {
            if (subjectClassifications == null || !subjectClassifications.Any())
            {
                return null;
            }
            List<SubjectClassificationDTO> subjectClassificationDTOs = subjectClassifications
                .Select(subjectClassificationDTO => new SubjectClassificationDTO()
                {
                    Id = subjectClassificationDTO.Id,
                    ParentId = subjectClassificationDTO.ParentId,
                    IsGroup = subjectClassificationDTO.IsGroup,
                    OrgUnits = MapOrgUnits(subjectClassificationDTO.SubjectOrgUnits),
                    LocalName = subjectClassificationDTO.Text,
                    Description =
                    LocalizationIdentifierMapper.Map(subjectClassificationDTO.LocalizationIdentifier?.Localizations)
                }).ToList();
          

            return subjectClassificationDTOs;
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
        public static SubjectClassificationDTO Map(SubjectClassification subjectClassification)
        {
            SubjectClassificationDTO subjectClassificationDTO = new SubjectClassificationDTO()
            {
                Id = subjectClassification.Id,
                ParentId = subjectClassification.ParentId,
                IsGroup = subjectClassification.IsGroup,
                // OrgUnits = MapOrgUnits(subjectClassification.SubjectOrgUnits),
                LocalName = subjectClassification.Text,
                Description = LocalizationIdentifierMapper.Map(subjectClassification.LocalizationIdentifier?.Localizations)
            };

            return subjectClassificationDTO;
        }

        public static SubjectClassification Map(SubjectClassificationDTO subjectClassificationDTO)
        {
            SubjectClassification subjectClassification = new SubjectClassification()
            {
                Id = subjectClassificationDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = subjectClassificationDTO.Description != null ? LocalizationIdentifierMapper.Map(subjectClassificationDTO.Description) : null,
            };

            return subjectClassification;
        }
    }
}