using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class PriorityExceptionMapper
    {
        public static PriorityExceptionVM Map(PriorityExceptionDTO priorityExceptionDTO)
        {
            if (priorityExceptionDTO == null)
            {
                return null;
            }

            PriorityExceptionVM priorityExceptionVM = new PriorityExceptionVM
            {
                Id = priorityExceptionDTO.Id,
                OrgUnitId = priorityExceptionDTO.OrgUnitId,
                OrgUnitName = priorityExceptionDTO.OrgUnitName,
                UserName = priorityExceptionDTO.UserName,
                UserId = priorityExceptionDTO.UserId,
                LateOnUsersAfter = priorityExceptionDTO.LateOnUsersAfter,
                PriorityId = priorityExceptionDTO.PriorityId
            };

            return priorityExceptionVM;
        }
        public static PriorityExceptionDTO Map(PriorityExceptionVM priorityExceptionVM)
        {
            if (priorityExceptionVM == null)
            {
                return null;
            }

            PriorityExceptionDTO priorityExceptionDTO = new PriorityExceptionDTO
            {
                Id = priorityExceptionVM.Id,
                OrgUnitId = priorityExceptionVM.OrgUnitId,
                UserId = priorityExceptionVM.UserId.Value,
                LateOnUsersAfter = priorityExceptionVM.LateOnUsersAfter,
                PriorityId = priorityExceptionVM.PriorityId
            };

            return priorityExceptionDTO;
        }

        public static List<PriorityExceptionDTO> Map(List<PriorityExceptionVM> priorityExceptionVMs)
        {
            if (priorityExceptionVMs == null)
            {
                return new List<PriorityExceptionDTO>();
            }

            return priorityExceptionVMs.Select(pe => Map(pe)).ToList();
        }
        public static List<PriorityExceptionVM> Map(List<PriorityExceptionDTO> priorityExceptionDTOs)
        {
            if (priorityExceptionDTOs == null)
            {
                return new List<PriorityExceptionVM>();
            }

            return priorityExceptionDTOs.Select(pe => Map(pe)).ToList();
        }
    }
}