using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.OrgUnit;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class AssignmentPaperMapper
    {
        public static List<AssignmentPaperDTO> Map(IList<AssignmentPaperVM> assignmentPaperVMs)
        {
            if (assignmentPaperVMs == null || !assignmentPaperVMs.Any())
            {
                return null;
            }
            List<AssignmentPaperDTO> assignmentPaperDTOs = assignmentPaperVMs
                .Select(b => new AssignmentPaperDTO
                {  
                    Actions = AssignmentPaperMapper.Map(b.Actions),
                    Beneficiaries = AssignmentPaperMapper.Map(b.Beneficiaries),
                    Id = b.Id,
                    IsCreateGroupAllowed = b.IsCreateGroupAllowed
                }).ToList();
            return assignmentPaperDTOs;
        }
        public static List<AssignmentPaperVM> Map(IList<AssignmentPaperDTO> assignmentPaperDTOs)
        {
            if (assignmentPaperDTOs == null || !assignmentPaperDTOs.Any())
            {
                return null;
            }
            List<AssignmentPaperVM> assignmentPaperVMs = assignmentPaperDTOs
                .Select(b => new AssignmentPaperVM
                {
                    Actions = AssignmentPaperMapper.Map(b.Actions),
                    Beneficiaries = AssignmentPaperMapper.Map(b.Beneficiaries),
                    Id = b.Id,
                    IsCreateGroupAllowed = b.IsCreateGroupAllowed

                }).ToList();
            return assignmentPaperVMs;
        }
        public static AssignmentPaperDTO Map(AssignmentPaperVM assignmentPaperVM)
        {
            if (assignmentPaperVM != null)
            {
                return new AssignmentPaperDTO
                {
                    Actions = AssignmentPaperMapper.Map(assignmentPaperVM.Actions),
                    Beneficiaries = AssignmentPaperMapper.Map(assignmentPaperVM.Beneficiaries),
                    Id = assignmentPaperVM.Id,
                    IsCreateGroupAllowed = assignmentPaperVM.IsCreateGroupAllowed
                };
            }
            return null;
        }
        public static AssignmentPaperVM Map(AssignmentPaperDTO assignmentPaperDTO)
        {
            if (assignmentPaperDTO != null)
            {
                return new AssignmentPaperVM
                {
                    Actions = AssignmentPaperMapper.Map(assignmentPaperDTO.Actions),
                    Beneficiaries = AssignmentPaperMapper.Map(assignmentPaperDTO.Beneficiaries),
                    Id = assignmentPaperDTO.Id,
                    IsCreateGroupAllowed = assignmentPaperDTO.IsCreateGroupAllowed
                };
            }
            return null;
        }
        public static List<AssignmentPaperBeneficiaryDTO> Map(IList<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs)
        {
            if (assignmentPaperBeneficiaryVMs == null || !assignmentPaperBeneficiaryVMs.Any())
            {
                return null;
            }

                List<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs = assignmentPaperBeneficiaryVMs
                .Select(b => new AssignmentPaperBeneficiaryDTO
                { 
                    Id = b.Id,
                    BeneficiaryOrgUnitId = b.BeneficiaryOrgUnitId,
                    Key = b.Key,
                    OrgUnitName = b.OrgUnitName,
                    UserId = b.UserId,
                    UserName = b.UserName

                }).ToList();
            return assignmentPaperBeneficiaryDTOs;
        }

        public static List<AssignmentPaperBeneficiaryVM> Map(IList<AssignmentPaperBeneficiaryDTO> assignmentPaperBeneficiaryDTOs)
        {
            if (assignmentPaperBeneficiaryDTOs == null || !assignmentPaperBeneficiaryDTOs.Any())
            {
                return null;
            }

            List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = assignmentPaperBeneficiaryDTOs
                .Select(b => new AssignmentPaperBeneficiaryVM
                {
                    Id = b.Id,
                    BeneficiaryOrgUnitId = b.BeneficiaryOrgUnitId,
                    Key = b.Key,
                    OrgUnitName = b.OrgUnitName,
                    UserId = b.UserId,
                    UserName = b.UserName

                }).ToList();
            return assignmentPaperBeneficiaryVMs;
        }

        public static List<AssignmentPaperActionDTO> Map(IList<AssignmentPaperActionVM> assignmentPaperActionVMs)
        {
            if (assignmentPaperActionVMs == null || !assignmentPaperActionVMs.Any())
            {
                return null;
            }
            List<AssignmentPaperActionDTO> assignmentPaperActionDTOs = assignmentPaperActionVMs
                .Select(b => new AssignmentPaperActionDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    ActionId = b.ActionId


                }).ToList();
            return assignmentPaperActionDTOs;
        }

        public static List<AssignmentPaperActionVM> Map(IList<AssignmentPaperActionDTO> assignmentPaperActionDTOs)
        {
            if (assignmentPaperActionDTOs == null || !assignmentPaperActionDTOs.Any())
            {
                return null;
            }
            List<AssignmentPaperActionVM> assignmentPaperActionVMs = assignmentPaperActionDTOs
                .Select(b => new AssignmentPaperActionVM
                { 
                    Id = b.Id, 
                    Name = b.Name,
                    ActionId = b.ActionId
                     

                }).ToList();
            return assignmentPaperActionVMs;
        }


    }
}