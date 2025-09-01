using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.UI.Areas.User.Mappers.Lookups;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public static class AssignmentPaperGroupMapper
    {
        public static List<AssignmentPaperGroupVM> Map(List<AssignmentPaperGroupDTO> assignmentPaperGroupDTOList)
        {
            List<AssignmentPaperGroupVM> assignmentPaperGroupVMList = new List<AssignmentPaperGroupVM>();

            foreach (AssignmentPaperGroupDTO assignmentPaperGroupDTO in assignmentPaperGroupDTOList)
            {
                AssignmentPaperGroupVM assignmentPaperGroupVM = new AssignmentPaperGroupVM
                {
                    Id = assignmentPaperGroupDTO.Id,
                    UserId = assignmentPaperGroupDTO.UserId,
                    Name = assignmentPaperGroupDTO.Name,
                    OrderNo = assignmentPaperGroupDTO.OrderNo,
                    DefaultActionId = assignmentPaperGroupDTO.DefaultActionId,
                    DefaultActionName = assignmentPaperGroupDTO.DefaultActionName,
      
                    // Names = LocalizationMapper.Map(assignmentPaperGroupDTO.Names)
                };
                assignmentPaperGroupVMList.Add(assignmentPaperGroupVM);
            }
            return assignmentPaperGroupVMList;
        }

        internal static AssignmentPaperGroupDTO Map(AssignmentPaperGroupVM assignmentPaperGroupVM, int userId)
        {
            AssignmentPaperGroupDTO assignmentPaperGroupDTO = new AssignmentPaperGroupDTO
            {
                Id = assignmentPaperGroupVM.Id,
                Name = assignmentPaperGroupVM.Name,
                UserId = userId,
                DefaultActionId = assignmentPaperGroupVM.DefaultActionId,
                OrderNo = assignmentPaperGroupVM.OrderNo,
         
            };
            return assignmentPaperGroupDTO;
        }

        internal static AssignmentPaperGroupVM Map(AssignmentPaperGroupDTO assignmentPaperGroupDTO)
        {
            AssignmentPaperGroupVM assignmentPaperGroupVM = new AssignmentPaperGroupVM
            {
                Name = assignmentPaperGroupDTO.Name,
                UserId = assignmentPaperGroupDTO.UserId
            };
            return assignmentPaperGroupVM;
        }

        internal static AssignmentPaperGroupEditVM Map(AssignmentPaperGroupDTO assignmentPaperGroupDTO, string culture)
        {
            AssignmentPaperGroupEditVM assignmentPaperGroupEditVM = new AssignmentPaperGroupEditVM
            {
                Name = assignmentPaperGroupDTO.Name,
                UserId = assignmentPaperGroupDTO.UserId,
                DefaultActionId = assignmentPaperGroupDTO.DefaultActionId > 0 ? assignmentPaperGroupDTO.DefaultActionId : -1,
                OrderNo = assignmentPaperGroupDTO.OrderNo,
            };
            return assignmentPaperGroupEditVM;
        }
    }
}