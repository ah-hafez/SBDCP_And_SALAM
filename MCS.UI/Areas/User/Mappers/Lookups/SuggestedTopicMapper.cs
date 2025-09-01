using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Mappers.Lookups
{
    public static class SuggestedTopicMapper
    {
        public static List<SuggestedTopicVM> Map(IList<SuggestedTopicDTO> suggestedTopicDTOs)
        {
            if (suggestedTopicDTOs == null || !suggestedTopicDTOs.Any())
            {
                return new List<SuggestedTopicVM>();
            }
            List<SuggestedTopicVM> suggestedTopicVMs = suggestedTopicDTOs
                .Select(suggestedTopicDTO => new SuggestedTopicVM()
                {
                    Parent = SuggestedTopicMapper.Map(suggestedTopicDTO.Parent),
                    Id = suggestedTopicDTO.Id,
                    Childs = Map(suggestedTopicDTO.Childs),
                    Description = LocalizationMapper.Map(suggestedTopicDTO.Description),
                    IsDeleted = suggestedTopicDTO.IsDeleted,
                    IsGroup = suggestedTopicDTO.IsGroup,
                    IsNew = suggestedTopicDTO.IsNew,
                    IsSelected = suggestedTopicDTO.IsSelected,
                    LocalName = suggestedTopicDTO.LocalName,
                    OrgUnits = suggestedTopicDTO.OrgUnits,
                    ParentId = suggestedTopicDTO.ParentId
                }).ToList();
            return suggestedTopicVMs;
        }
        public static List<SuggestedTopicDTO> Map(IList<SuggestedTopicVM> suggestedTopicVMs)
        {
            if (suggestedTopicVMs == null || !suggestedTopicVMs.Any())
            {
                return new List<SuggestedTopicDTO>();
            }
            List<SuggestedTopicDTO> suggestedTopicDTOs = suggestedTopicVMs
                .Select(suggestedTopicDTO => new SuggestedTopicDTO()
                {
                    Parent = SuggestedTopicMapper.Map(suggestedTopicDTO.Parent),
                    Id = suggestedTopicDTO.Id,
                    Childs = Map(suggestedTopicDTO.Childs),
                    Description = LocalizationMapper.Map(suggestedTopicDTO.Description),
                    IsDeleted = suggestedTopicDTO.IsDeleted,
                    IsGroup = suggestedTopicDTO.IsGroup,
                    IsNew = suggestedTopicDTO.IsNew,
                    IsSelected = suggestedTopicDTO.IsSelected,
                    LocalName = suggestedTopicDTO.LocalName,
                    OrgUnits = suggestedTopicDTO.OrgUnits,
                    ParentId = suggestedTopicDTO.ParentId
                }).ToList();
            return suggestedTopicDTOs;
        }
        public static SuggestedTopicDTO Map(SuggestedTopicVM suggestedTopicVM)
        {
            if (suggestedTopicVM == null)
            {
                return new SuggestedTopicDTO();
            }
            SuggestedTopicDTO suggestedTopicDTOs = new SuggestedTopicDTO()
            {
                Parent = SuggestedTopicMapper.Map(suggestedTopicVM.Parent),
                Id = suggestedTopicVM.Id,
                Childs = Map(suggestedTopicVM.Childs),
                Description = LocalizationMapper.Map(suggestedTopicVM.Description),
                IsDeleted = suggestedTopicVM.IsDeleted,
                IsGroup = suggestedTopicVM.IsGroup,
                IsNew = suggestedTopicVM.IsNew,
                IsSelected = suggestedTopicVM.IsSelected,
                LocalName = suggestedTopicVM.LocalName,
                OrgUnits = suggestedTopicVM.OrgUnits,
                ParentId = suggestedTopicVM.ParentId
            };
            return suggestedTopicDTOs;
        }
        public static SuggestedTopicVM Map(SuggestedTopicDTO suggestedTopicDTO)
        {
            if (suggestedTopicDTO == null)
            {
                return new SuggestedTopicVM();
            }
            SuggestedTopicVM suggestedTopicVM = new SuggestedTopicVM()
            {
                Parent = SuggestedTopicMapper.Map(suggestedTopicDTO.Parent),
                Id = suggestedTopicDTO.Id,
                Childs = Map(suggestedTopicDTO.Childs),
                Description = LocalizationMapper.Map(suggestedTopicDTO.Description),
                IsDeleted = suggestedTopicDTO.IsDeleted,
                IsGroup = suggestedTopicDTO.IsGroup,
                IsNew = suggestedTopicDTO.IsNew,
                IsSelected = suggestedTopicDTO.IsSelected,
                LocalName = suggestedTopicDTO.LocalName,
                OrgUnits = suggestedTopicDTO.OrgUnits,
                ParentId = suggestedTopicDTO.ParentId
            };
            return suggestedTopicVM;
        }
    }
}