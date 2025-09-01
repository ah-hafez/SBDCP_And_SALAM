using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class SubjectClassificationMapper
    {
        public static List<SubjectClassificationDTO> Map(IList<SubjectClassificationVM> subjectClassificationVMs)
        {
            if (subjectClassificationVMs == null || !subjectClassificationVMs.Any())
            { return null; }
            List<SubjectClassificationDTO> subjectClassificationDTOs = subjectClassificationVMs
                .Select(b => new SubjectClassificationDTO
                {
                    Childs = SubjectClassificationMapper.Map(b.Childs),
                    Description = b.Description != null ? LocalizationMapper.Map(b.Description): null,
                    Id = b.Id,
                    IsDeleted = b.IsDeleted,
                    IsGroup = b.IsGroup,
                    IsNew = b.IsNew,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    ParentId = b.ParentId,
                    OrgUnits = b.OrgUnits



                }).ToList();
            return subjectClassificationDTOs;
        }
        public static List<SubjectClassificationVM> Map(IList<SubjectClassificationDTO> subjectClassificationDTOs)
        {
            if (subjectClassificationDTOs == null || !subjectClassificationDTOs.Any())
            { return null; }
            List<SubjectClassificationVM> subjectClassificationVMs = subjectClassificationDTOs
                .Select(b => new SubjectClassificationVM
                {
                    Childs = SubjectClassificationMapper.Map(b.Childs),
                    Description = b.Description !=null ? LocalizationMapper.Map(b.Description):null,
                    Id = b.Id,
                    IsDeleted = b.IsDeleted,
                    IsGroup = b.IsGroup,
                    IsNew = b.IsNew,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    ParentId = b.ParentId,
                    OrgUnits = b.OrgUnits

                }).ToList();
            return subjectClassificationVMs;
        }

        public static SubjectClassificationEditVM Map(SubjectClassificationDTO subjectClassificationDTO)
        {
            SubjectClassificationEditVM subjectClassificationEditVM = new SubjectClassificationEditVM
            {
                Description = subjectClassificationDTO.Description != null ? LocalizationMapper.Map(subjectClassificationDTO.Description) : null,
                Id = subjectClassificationDTO.Id
            };
            return subjectClassificationEditVM;
        }

        public static SubjectClassificationDTO Map(SubjectClassificationVM subjectClassificationVM)
        {
            SubjectClassificationDTO subjectClassificationDTO = new SubjectClassificationDTO
            {
                Description = LocalizationMapper.Map(subjectClassificationVM.Description),
                // TransactionCategories = TransactionCategoryMapper.Map(subjectClassificationEditVM.TransactionCategories),
                Id = subjectClassificationVM.Id
            };
            return subjectClassificationDTO;
        }
    }
    //public static SubjectClassificationEditVM Map(SubjectClassificationDTO subjectClassificationDTO)
    //{
    //    SubjectClassificationEditVM subjectClassificationEditVM = new SubjectClassificationEditVM
    //    {
    //        Description = subjectClassificationDTO.Description != null ? LocalizationMapper.Map(subjectClassificationDTO.Description) : null,
    //        Id = subjectClassificationDTO.Id
    //    };
    //    return subjectClassificationEditVM;
    //}

    //public static SubjectClassificationDTO Map(SubjectClassificationVM subjectClassificationVM)
    //{
    //    SubjectClassificationDTO subjectClassificationDTO = new SubjectClassificationDTO
    //    {
    //        Description = LocalizationMapper.Map(subjectClassificationVM.Description),
    //        // TransactionCategories = TransactionCategoryMapper.Map(subjectClassificationEditVM.TransactionCategories),
    //        Id = subjectClassificationVM.Id
    //    };
    //    return subjectClassificationDTO;
    //}
    
}