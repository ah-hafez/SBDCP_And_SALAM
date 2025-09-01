using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Actions;

namespace MCS.UI.Areas.User.Mappers.Action
{
    public static class ActionMapper
    {
        public static AddActionDTO Map(AddActionVM addActionVM)
        {
            if (addActionVM != null)
            {
                AddActionDTO addActionDTO = new AddActionDTO()
                {
                    Description = LocalizationMapper.Map(addActionVM.Description),
                    TypeId = addActionVM.TypeId,
                    IsAsCopy = addActionVM.UpdateActionName
                };
                return addActionDTO;
            }
            return new AddActionDTO();
        }
        public static EditActionVM Map(EditActionDTO editActionDTO)
        {
            if (editActionDTO != null)
            {
                EditActionVM editActionVM = new EditActionVM()
                {
                    Description = LocalizationMapper.Map(editActionDTO.Description),
                    UpdateActionName = editActionDTO.IsAsCopy,
                    TypeId = editActionDTO.TypeId,
                    Id = editActionDTO.Id
                };
                return editActionVM;
            }
            return new EditActionVM();
        }
        public static EditActionDTO Map(EditActionVM editActionVM)
        {
            if (editActionVM != null)
            {
                EditActionDTO editActionDTO = new EditActionDTO()
                {
                    Description = LocalizationMapper.Map(editActionVM.Description),
                    IsAsCopy = editActionVM.UpdateActionName,
                    TypeId = editActionVM.TypeId,
                    Id = editActionVM.Id
                };
                return editActionDTO;
            }
            return new EditActionDTO();
        }
        public static ActionVM Map(ActionDTO processDTO)
        {
            if (processDTO != null)
            {
                ActionVM processVM = new ActionVM()
                {
                    Id = processDTO.Id,
                    TypeId = processDTO.TypeId,
                    Description = LocalizationMapper.Map(processDTO.Description),
                    LocalName = processDTO.LocalName
                };
                return processVM;
            }
            return new ActionVM();
        }
        public static ActionDTO Map(ActionVM processVM)
        {
            if (processVM != null)
            {
                ActionDTO processDTO = new ActionDTO()
                {
                    Id = processVM.Id,
                    TypeId = processVM.TypeId,
                    Description = LocalizationMapper.Map(processVM.Description),
                    LocalName = processVM.LocalName
                };
                return processDTO;
            }
            return new ActionDTO();
        }
        public static List<ActionVM> Map(IList<ActionDTO> processDTOs)
        {
            if (processDTOs == null || !processDTOs.Any())
            {
                return new List<ActionVM>();
            }
            List<ActionVM> processVMs = processDTOs
                .Select(processDTO => new ActionVM()
                {
                    Id = processDTO.Id,
                    TypeId = processDTO.TypeId,
                    LocalName = processDTO.LocalName,
                    Description = LocalizationMapper.Map(processDTO.Description),
                    SortNo = processDTO.SortNo
                }
                ).ToList();
            return processVMs;
        }
        public static List<ActionDTO> Map(IList<ActionVM> processVMs)
        {
            if (processVMs == null || !processVMs.Any())
            {
                return new List<ActionDTO>();
            }
            List<ActionDTO> processDTOs = processVMs
                .Select(processDTO => new ActionDTO()
                {
                    Id = processDTO.Id,
                    TypeId = processDTO.TypeId,
                    LocalName = processDTO.LocalName,
                    Description = LocalizationMapper.Map(processDTO.Description)
                }
                ).ToList();
            return processDTOs;
        }
    }
}