using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class SubjectClassificationMapper
    {
        public static List<SubjectClassificationVM> Map(IList<SubjectClassificationDTO> subjectClassificationDTOs)
        {
            if (subjectClassificationDTOs == null || !subjectClassificationDTOs.Any())
            {
                return new List<SubjectClassificationVM>();
            }
            List<SubjectClassificationVM> subjectClassificationVMs = subjectClassificationDTOs
                .Select(subjectClassificationDTO => new SubjectClassificationVM()
                { 
                    Id = subjectClassificationDTO.Id,
                    Description = LocalizationMapper.Map(subjectClassificationDTO.Description),
                    ParentId = subjectClassificationDTO.ParentId,
                    Childs = Map(subjectClassificationDTO.Childs),
                    OrgUnits = subjectClassificationDTO.OrgUnits,
                    LocalName = subjectClassificationDTO.LocalName,
                    IsSelected = subjectClassificationDTO.IsSelected,
                    IsNew = subjectClassificationDTO.IsNew,
                    IsDeleted = subjectClassificationDTO.IsDeleted,
                    IsGroup = subjectClassificationDTO.IsGroup
                }).ToList();
            return subjectClassificationVMs;
        }
        public static List<SubjectClassificationDTO> Map(IList<SubjectClassificationVM> subjectClassificationVMs)
        {
            if (subjectClassificationVMs == null || !subjectClassificationVMs.Any())
            {
                return new List<SubjectClassificationDTO>();
            }
            List<SubjectClassificationDTO> subjectClassificationDTOs = subjectClassificationVMs
                .Select(subjectClassificationVM => new SubjectClassificationDTO()
                { 
                    Id = subjectClassificationVM.Id,
                    Description = LocalizationMapper.Map(subjectClassificationVM.Description),
                    ParentId = subjectClassificationVM.ParentId,
                    Childs = Map(subjectClassificationVM.Childs),
                    OrgUnits = subjectClassificationVM.OrgUnits,
                    LocalName = subjectClassificationVM.LocalName,
                    IsSelected = subjectClassificationVM.IsSelected,
                    IsNew = subjectClassificationVM.IsNew,
                    IsDeleted = subjectClassificationVM.IsDeleted,
                    IsGroup = subjectClassificationVM.IsGroup
                }).ToList();
            return subjectClassificationDTOs;
        }
    }
}