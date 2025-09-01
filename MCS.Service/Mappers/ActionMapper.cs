using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ActionMapper
    {
        public static Action Map(AddActionDTO addActionDTO)
        {
            if (addActionDTO == null)
                return null;

            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

            Action action = new Action()
            {
                Type = lookupBL.GetLookupItem(addActionDTO.TypeId),
                IsActive = true,
                IsAsCopy = addActionDTO.IsAsCopy,
                LocalizationIdentifier = addActionDTO.Description != null ? LocalizationIdentifierMapper.Map(addActionDTO.Description) : null
            };

            return action;
        }

        public static Action Map(EditActionDTO actionEditDTO)
        {
            if (actionEditDTO == null)
                return null;

            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

            Action action = new Action()
            {
                Id = actionEditDTO.Id,
                Type = lookupBL.GetLookupItem(actionEditDTO.TypeId),
                IsActive = true,
                IsAsCopy = actionEditDTO.IsAsCopy,
                LocalizationIdentifier = actionEditDTO.Description != null ? LocalizationIdentifierMapper.Map(actionEditDTO.Description) : null

            };

            return action;
        }

        public static List<ActionDTO> Map(IList<Action> actions)
        {
            if (actions == null || !actions.Any())
            {
                return null;
            }
            List<ActionDTO> actionDTOs = actions
                .Select(action => new ActionDTO()
                {
                    Id = action.Id,
                    TypeId = action.Type != null ? action.Type.Id : -1,
                    LocalName = action.LocalName,
                    Description = action.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(action.LocalizationIdentifier.Localizations) : null,
                    IsActive = action.IsActive,
                    IsLocked = action.IsLocked,
                    LockedBy = action.LockedBy,
                    IsAsCopy = action.IsAsCopy,
                    SortNo = action.SortNo
                }).ToList();


            return actionDTOs;
        }

        public static EditActionDTO Map(Action action)
        {
            if (action == null)
                return null;

            EditActionDTO actionDTO = new EditActionDTO()
            {
                Id = action.Id,
                TypeId = action.Type != null ? action.Type.Id : -1,
                IsActive = action.IsActive,
                IsLocked = action.IsLocked,
                LockedBy = action.LockedBy,
                IsAsCopy = action.IsAsCopy,
            };

            if (action.LocalizationIdentifier != null)
            {
                actionDTO.Description = action.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(action.LocalizationIdentifier.Localizations) : null;
            }

            return actionDTO;
        }
    }
}