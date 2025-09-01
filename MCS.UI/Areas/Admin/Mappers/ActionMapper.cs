using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Actions;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class ActionMapper
    {
        public static List<ActionDTO> Map(IList<ActionVM> ActionVMs)
        {
            if (ActionVMs == null || !ActionVMs.Any())
            { return null; }
            List<ActionDTO> ActionDTOs = ActionVMs
                .Select(b => new ActionDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    Id = b.Id,
                    LocalName = b.LocalName,
                    TypeId = b.TypeId,
                    IsAsCopy = b.IsAsCopy
                }).ToList();
            return ActionDTOs;
        }
        public static List<ActionVM> Map(IList<ActionDTO> ActionDTOs)
        {
            if (ActionDTOs == null || !ActionDTOs.Any())
            {
                return new List<ActionVM>();
            }
            List<ActionVM> ActionVMs = ActionDTOs.Select(b => new ActionVM
            {
                Description = LocalizationMapper.Map(b.Description),
                Id = b.Id,
                LocalName = b.LocalName,
                TypeId = b.TypeId,
                IsActive = b.IsActive,
                IsLocked = b.IsLocked,
                LockedBy = b.LockedBy,
                IsAsCopy = b.IsAsCopy
            }).ToList();
            return ActionVMs;
        }
        public static List<AddActionDTO> Map(IList<AddActionVM> ActionAddVMs)
        {
            if (ActionAddVMs == null || !ActionAddVMs.Any())
            { return null; }
            List<AddActionDTO> ActionAddDTOs = ActionAddVMs
                .Select(b => new AddActionDTO
                {
                    Description = LocalizationMapper.Map(b.Description),
                    IsAsCopy = b.IsAsCopy,
                    TypeId = b.TypeId
                }).ToList();
            return ActionAddDTOs;
        }
        public static List<EditActionVM> Map(IList<EditActionDTO> ActionEditDTOs)
        {
            if (ActionEditDTOs == null || !ActionEditDTOs.Any())
            { return null; }
            List<EditActionVM> ActionEditVMs = ActionEditDTOs
                .Select(b => new EditActionVM
                {
                    Description = LocalizationMapper.Map(b.Description),
                    IsAsCopy = b.IsAsCopy,
                    TypeId = b.TypeId,
                    Id = b.Id,
                    IsActive = b.IsActive,
                    IsLocked = b.IsLocked,
                    LockedBy = b.LockedBy
                }).ToList();
            return ActionEditVMs;
        }

        public static AddActionDTO Map(AddActionVM ActionAddVMs)
        {
            if (ActionAddVMs != null)
            {
                return new AddActionDTO
                {
                    Description = LocalizationMapper.Map(ActionAddVMs.Description),
                    IsAsCopy = ActionAddVMs.IsAsCopy,
                    TypeId = ActionAddVMs.TypeId
                };
            }
            return null;
        }

        public static EditActionDTO Map(EditActionVM editActionVM)
        {
            if (editActionVM != null)
            {
                return new EditActionDTO
                {
                    Description = LocalizationMapper.Map(editActionVM.Description),
                    IsAsCopy = editActionVM.IsAsCopy,
                    TypeId = editActionVM.TypeId,
                    Id = editActionVM.Id
                };
            }
            return null;
        }

        public static EditActionVM Map(EditActionDTO editActionDTO)
        {
            if (editActionDTO != null)
            {
                return new EditActionVM
                {
                    Description = LocalizationMapper.Map(editActionDTO.Description),
                    IsAsCopy = editActionDTO.IsAsCopy,
                    TypeId = editActionDTO.TypeId,
                    Id = editActionDTO.Id,
                    IsActive = editActionDTO.IsActive,
                    IsLocked = editActionDTO.IsLocked,
                    LockedBy = editActionDTO.LockedBy
                };
            }
            return null;
        }

    }
}