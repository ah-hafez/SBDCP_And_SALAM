using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AssignmentPaperBeneficiaryMapper
    {



        public static List<AssignmentPaperBeneficiary> Map(IList<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs)
        {
            if (assignmentPaperBeneficiaryDTOs == null || !assignmentPaperBeneficiaryDTOs.Any())
            {
                return null;
            }
            List<AssignmentPaperBeneficiary> assignmentPaperActions = assignmentPaperBeneficiaryDTOs
                    .Select(assignmentPaperAction => new AssignmentPaperBeneficiary()
                    {
                        OrgUnitId = assignmentPaperAction.BeneficiaryOrgUnitId,
                        UserId = assignmentPaperAction.UserId,
                        AssignmentPaperGroupId = assignmentPaperAction.GroupId,
                        ChkConstant = assignmentPaperAction.ChkConstant,
                        OrderNo = assignmentPaperAction.OrderNo,
                        DefaultActionId = assignmentPaperAction.DefaultActionId,
                        AssignmentPaperId = assignmentPaperAction.AssignmentPaperId,
                        Id = assignmentPaperAction.Id,




                    }).ToList();



            return assignmentPaperActions;

        }
        public static List<AssignmentPaperBeneficiaryDTO> Map(IList<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries, string cultureName)
        {
            if (assignmentPaperBeneficiaries == null || !assignmentPaperBeneficiaries.Any())
            {
                return null;
            }
            List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs;

            assignmentPaperBeneficiaryDTOs = assignmentPaperBeneficiaries
                .Select(assignmentPaperAction => new AssignmentPaperBeneficiaryDTO()
                {
                    BeneficiaryOrgUnitId = assignmentPaperAction.OrgUnit.Id,
                    OrgUnitName = assignmentPaperAction.OrgUnit.LocalName,
                    UserId = assignmentPaperAction.User?.Id,
                    UserName = assignmentPaperAction?.User?.LocalizationIdentifier?.Localizations.Where(l => l.Culture.ShortName == cultureName)?.FirstOrDefault()?.Text,
                    UserImageId = assignmentPaperAction.User?.UserImage?.Id,
                    GroupId = assignmentPaperAction.AssignmentPaperGroupId,
                    GroupName = assignmentPaperAction.AssignmentPaperGroup.Name,
                    ChkConstant = assignmentPaperAction.ChkConstant,
                    OrderNo = assignmentPaperAction.OrderNo,
                    DefaultActionId = assignmentPaperAction.DefaultActionId,
                    AssignmentPaperId = assignmentPaperAction.AssignmentPaperId,
                    Id = assignmentPaperAction.Id,
                }).ToList();
            return assignmentPaperBeneficiaryDTOs;


        }

    }
}