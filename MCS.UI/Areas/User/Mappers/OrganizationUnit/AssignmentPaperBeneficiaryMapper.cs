using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.OrgUnit;

namespace MCS.UI.Areas.User.Mappers.OrgUnit
{
    public class AssignmentPaperBeneficiaryMapper
    {
        public static List<AssignmentPaperBeneficiaryVM> Map(IList<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs)
        {
            if (assignmentPaperBeneficiaryDTOs == null || !assignmentPaperBeneficiaryDTOs.Any())
            {
                return new List<AssignmentPaperBeneficiaryVM>();
            }
            List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = assignmentPaperBeneficiaryDTOs
                .Select(assignmentPaperBeneficiaryDTO => new AssignmentPaperBeneficiaryVM()
                {
                    BeneficiaryOrgUnitId = assignmentPaperBeneficiaryDTO.BeneficiaryOrgUnitId,
                    Id = assignmentPaperBeneficiaryDTO.Id,
                    Key = assignmentPaperBeneficiaryDTO.Key,
                    OrgUnitName = assignmentPaperBeneficiaryDTO.OrgUnitName,
                    GroupId = assignmentPaperBeneficiaryDTO.GroupId,
                    GroupName = assignmentPaperBeneficiaryDTO.GroupName,
                    UserId = assignmentPaperBeneficiaryDTO.UserId,
                    UserName = assignmentPaperBeneficiaryDTO.UserName == null ? "استقبال الادارة" : assignmentPaperBeneficiaryDTO.UserName,
                    UserImageId = assignmentPaperBeneficiaryDTO.UserImageId,
                    ChkConstant = assignmentPaperBeneficiaryDTO.ChkConstant,
                    OrderNo = assignmentPaperBeneficiaryDTO.OrderNo,
                    DefaultActionId = assignmentPaperBeneficiaryDTO.DefaultActionId,
                    AssignmentPaperId = assignmentPaperBeneficiaryDTO.AssignmentPaperId,

                }).ToList();

            return assignmentPaperBeneficiaryVMs;
        }
        public static List<AssignmentPaperBeneficiaryDTO> Map(IList<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs)
        {
            if (assignmentPaperBeneficiaryVMs == null || !assignmentPaperBeneficiaryVMs.Any())
            {
                return new List<AssignmentPaperBeneficiaryDTO>();
            }
            List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs = assignmentPaperBeneficiaryVMs
                .Select(assignmentPaperBeneficiaryVM => new AssignmentPaperBeneficiaryDTO()
                {
                    BeneficiaryOrgUnitId = assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId.Value,
                    Id = assignmentPaperBeneficiaryVM.Id,
                    Key = assignmentPaperBeneficiaryVM.Key,
                    OrgUnitName = assignmentPaperBeneficiaryVM.OrgUnitName,
                    UserId = assignmentPaperBeneficiaryVM.UserId,
                    UserName = assignmentPaperBeneficiaryVM.UserName,
                    UserImageId = assignmentPaperBeneficiaryVM.UserImageId,
                    GroupId = assignmentPaperBeneficiaryVM.GroupId,
                    ChkConstant = assignmentPaperBeneficiaryVM.ChkConstant,
                    OrderNo = assignmentPaperBeneficiaryVM.OrderNo,
                    DefaultActionId = assignmentPaperBeneficiaryVM.DefaultActionId,
                    AssignmentPaperId = assignmentPaperBeneficiaryVM.AssignmentPaperId,
                    GroupName = assignmentPaperBeneficiaryVM.GroupName,

                }).ToList();

            return assignmentPaperBeneficiaryDTOs;
        }
    }
}