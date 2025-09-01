using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class SuggestedTopicMapper
    {
        public static List<SuggestedTopicDTO> Map(IList<SuggestedTopicVM> suggestedTopicVMs)
        {
            if (suggestedTopicVMs == null || !suggestedTopicVMs.Any())
            { return null; }
            List<SuggestedTopicDTO> suggestedTopicDTOs = suggestedTopicVMs
                .Select(b => new SuggestedTopicDTO
                {
                    Childs = SuggestedTopicMapper.Map(b.Childs),
                    Parent = SuggestedTopicMapper.Map(b.Parent),
                    Id = b.Id, 
                    LocalName = b.LocalName,
                    Description = b.Description !=null ? LocalizationMapper.Map(b.Description):null,
                    IsDeleted = b.IsDeleted,
                    IsGroup = b.IsGroup,
                    IsNew = b.IsNew,
                    IsSelected = b.IsSelected,
                    OrgUnits = b.OrgUnits,
                    ParentId = b.ParentId,
                    //must add recursive function  for childs and parent

                }).ToList();
            return suggestedTopicDTOs;
        }
        public static List<SuggestedTopicVM> Map(IList<SuggestedTopicDTO> suggestedTopicDTOs)
        {
            if (suggestedTopicDTOs == null || !suggestedTopicDTOs.Any())
            { return null; }
            List<SuggestedTopicVM> suggestedTopicVMs = suggestedTopicDTOs
                .Select(b => new SuggestedTopicVM
                {
                    Childs = SuggestedTopicMapper.Map(b.Childs),
                    Parent = SuggestedTopicMapper.Map(b.Parent),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    Description = b.Description != null ? LocalizationMapper.Map(b.Description) : null,
                    IsDeleted = b.IsDeleted,
                    IsGroup = b.IsGroup,
                    IsNew = b.IsNew,
                    IsSelected = b.IsSelected,
                    OrgUnits = b.OrgUnits,
                    ParentId = b.ParentId,
                    //must add recursive function  for childs and parent

                }).ToList();
            return suggestedTopicVMs;
        }
        public static SuggestedTopicDTO Map(SuggestedTopicVM suggestedTopicVM)
        {
            if (suggestedTopicVM != null)
            {
                SuggestedTopicDTO suggestedTopicDTO = new SuggestedTopicDTO()
                {
                    Childs = SuggestedTopicMapper.Map(suggestedTopicVM.Childs),
                    Parent = SuggestedTopicMapper.Map(suggestedTopicVM.Parent),
                    Id = suggestedTopicVM.Id,
                    LocalName = suggestedTopicVM.LocalName,
                    Description = suggestedTopicVM.Description!=null? LocalizationMapper.Map(suggestedTopicVM.Description):null,
                    IsDeleted = suggestedTopicVM.IsDeleted,
                    IsGroup = suggestedTopicVM.IsGroup,
                    IsNew = suggestedTopicVM.IsNew,
                    IsSelected = suggestedTopicVM.IsSelected,
                    OrgUnits = suggestedTopicVM.OrgUnits,
                    ParentId = suggestedTopicVM.ParentId,


                };
                return suggestedTopicDTO;
            }
            return null;
        }
        public static SuggestedTopicVM Map(SuggestedTopicDTO suggestedTopicDTO)
        {
            if(suggestedTopicDTO!=null)
                {
                SuggestedTopicVM suggestedTopicVM = new SuggestedTopicVM()
                {
                    Childs = SuggestedTopicMapper.Map(suggestedTopicDTO.Childs),
                    Parent = SuggestedTopicMapper.Map(suggestedTopicDTO.Parent),
                    Id = suggestedTopicDTO.Id,
                    LocalName = suggestedTopicDTO.LocalName,
                    Description = suggestedTopicDTO.Description !=null? LocalizationMapper.Map(suggestedTopicDTO.Description):null,
                    IsDeleted = suggestedTopicDTO.IsDeleted,
                    IsGroup = suggestedTopicDTO.IsGroup,
                    IsNew = suggestedTopicDTO.IsNew,
                    IsSelected = suggestedTopicDTO.IsSelected,
                    OrgUnits = suggestedTopicDTO.OrgUnits,
                    ParentId = suggestedTopicDTO.ParentId,


                };
                return suggestedTopicVM;
            }
            return null;
        }
    }
}