using MCS.Domain;
using MCS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.Service.Mappers
{
    public static class AssignmentPaperGroupMapper
    {
        internal static List<AssignmentPaperGroupDTO> Map(List<AssignmentPaperGroup> assignmentPaperGroupList, string cultureName)
        {
            List<AssignmentPaperGroupDTO> assignmentPaperGroupDTOList = new List<AssignmentPaperGroupDTO>();

            foreach (AssignmentPaperGroup assignmentPaperGroup in assignmentPaperGroupList)
            {
                AssignmentPaperGroupDTO assignmentPaperGroupDTO = new AssignmentPaperGroupDTO
                {
                    Id = assignmentPaperGroup.Id,
                    UserId = assignmentPaperGroup.UserId,
                    Name = assignmentPaperGroup.Name,
                    DefaultActionId = assignmentPaperGroup.DefaultActionId,
                    OrderNo = assignmentPaperGroup.OrderNo,
                    DefaultActionName = assignmentPaperGroup.DefaultAction?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
               


                    // Names = assignmentPaperGroup.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(assignmentPaperGroup.LocalizationIdentifier.Localizations) : null
                };

                assignmentPaperGroupDTOList.Add(assignmentPaperGroupDTO);
            }

            return assignmentPaperGroupDTOList;
        }

        internal static AssignmentPaperGroup Map(AssignmentPaperGroupDTO assignmentPaperGroupVM)
        {
            AssignmentPaperGroup assignmentPaperGroup = new AssignmentPaperGroup
            {
                Id = assignmentPaperGroupVM.Id,
                Name = assignmentPaperGroupVM.Name,
                UserId = assignmentPaperGroupVM.UserId,
                OrderNo = assignmentPaperGroupVM.OrderNo,
                DefaultActionId = assignmentPaperGroupVM.DefaultActionId,

            };
            return assignmentPaperGroup;
        }

        internal static AssignmentPaperGroupDTO Map(AssignmentPaperGroup assignmentPaperGroup)
        {
            AssignmentPaperGroupDTO assignmentPaperGroupDTO = new AssignmentPaperGroupDTO
            {
                Name = assignmentPaperGroup.Name,
                UserId = assignmentPaperGroup.UserId,
                OrderNo = assignmentPaperGroup.OrderNo,
                DefaultActionId = assignmentPaperGroup.DefaultActionId,
            };
            return assignmentPaperGroupDTO;
        }
    }
}