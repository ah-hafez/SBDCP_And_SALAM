using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class AssignmentPaperMapper
    {
        public static AssignmentPaper Map(AssignmentPaperDTO assignmentPaperDTO)
        {
            if (assignmentPaperDTO != null)
            {
                AssignmentPaper assignmentPaper = new AssignmentPaper()
                {
                    Id = assignmentPaperDTO.Id,
                    AssignmentPaperActions = Map(assignmentPaperDTO.Actions),
                    AssignmentPaperBeneficiaries = Map(assignmentPaperDTO.Beneficiaries),
                    IsCreateGroupAllowed = assignmentPaperDTO.IsCreateGroupAllowed,

                };

                return assignmentPaper;
            }

            return null;
        }
        public static AssignmentPaperDTO Map(AssignmentPaper assignmentPaper)
        {
            if (assignmentPaper != null)
            {
                AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO()
                {
                    Id = assignmentPaper.Id,
                    Actions = Map(assignmentPaper.AssignmentPaperActions),
                    Beneficiaries = Map(assignmentPaper.AssignmentPaperBeneficiaries),
                    IsCreateGroupAllowed = assignmentPaper.IsCreateGroupAllowed,
                    

                };

                return assignmentPaperDTO;
            }

            return null;
        }
        private static List<AssignmentPaperAction> Map(IList<AssignmentPaperActionDTO> assignmentPaperActionDTOs)
        {
            if (assignmentPaperActionDTOs == null || !assignmentPaperActionDTOs.Any())
            {
                return null;
            }
            List<AssignmentPaperAction> assignmentPaperActions = assignmentPaperActionDTOs
                    .Select(assignmentPaperAction => new AssignmentPaperAction()
                    {
                        ActionId = assignmentPaperAction.ActionId,
                    }).ToList();



            return assignmentPaperActions;

        }
        private static List<AssignmentPaperActionDTO> Map(IList<AssignmentPaperAction> assignmentPaperActions)
        {
            if (assignmentPaperActions == null || !assignmentPaperActions.Any())
            {
                return null;
            }
            List<AssignmentPaperActionDTO> assignmentPaperActionDTOs;


            assignmentPaperActionDTOs = assignmentPaperActions
                .Select(assignmentPaperActionDTO => new AssignmentPaperActionDTO()
                {
                    ActionId = assignmentPaperActionDTO.Action.Id,
                    Name = assignmentPaperActionDTO.Action.LocalName
                }).ToList();


            return assignmentPaperActionDTOs;
        }
        private static List<AssignmentPaperBeneficiary> Map(IList<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs)
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
                        Id = assignmentPaperAction.Id,
                        AssignmentPaperId = assignmentPaperAction.Id,
                    }).ToList();



            return assignmentPaperActions;

        }
        private static List<AssignmentPaperBeneficiaryDTO> Map(IList<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            if (assignmentPaperBeneficiaries == null || !assignmentPaperBeneficiaries.Any())
            {
                return null;
            }
            List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs;

            assignmentPaperBeneficiaryDTOs = assignmentPaperBeneficiaries
                .Select(assignmentPaperActionDTO => new AssignmentPaperBeneficiaryDTO()
                {
                    BeneficiaryOrgUnitId = assignmentPaperActionDTO.OrgUnit.Id,
                    OrgUnitName = assignmentPaperActionDTO.OrgUnit.LocalName,
                    UserId = assignmentPaperActionDTO.User?.Id,
                    UserName = assignmentPaperActionDTO.User?.LocalName,
                    UserImageId = assignmentPaperActionDTO.User?.UserImage?.Id,
                    GroupId = assignmentPaperActionDTO.AssignmentPaperGroupId,
                    GroupName = assignmentPaperActionDTO.AssignmentPaperGroup.Name,
                    ChkConstant = assignmentPaperActionDTO.ChkConstant,
                    OrderNo = assignmentPaperActionDTO.OrderNo,
                    DefaultActionId = assignmentPaperActionDTO.DefaultActionId,
                    AssignmentPaperId = assignmentPaperActionDTO.AssignmentPaperId,
                    GroupOrderNo = assignmentPaperActionDTO.AssignmentPaperGroup.OrderNo,
                    Id = assignmentPaperActionDTO.AssignmentPaperGroupId,


                }).ToList();
            return assignmentPaperBeneficiaryDTOs;


        }

    }
}