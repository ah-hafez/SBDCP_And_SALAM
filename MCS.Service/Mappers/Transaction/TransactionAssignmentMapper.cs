using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class TransactionAssignmentMapper
    {
        public static List<TransactionAssignment> Map(IList<TransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            if (transactionAssignmentDTOs == null || !transactionAssignmentDTOs.Any())
            {
                return null;
            }

            List<TransactionAssignment> transactionAssignments = transactionAssignmentDTOs
                .Select(transactionAssignmentDTO => new TransactionAssignment()
                {
                    FromEntityId = transactionAssignmentDTO.FromOrgUnitId,
                    ToEntityId = transactionAssignmentDTO.ToOrgUnitId,
                    ActionId = transactionAssignmentDTO.ActionId,
                    Description = transactionAssignmentDTO.Remarks,
                    ToUserId = transactionAssignmentDTO.ToUserId,
                    DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                    TrayId = transactionAssignmentDTO.TrayId,
                    ReporterId = transactionAssignmentDTO.ReporterId,
                    GeneralExplanation = transactionAssignmentDTO.GeneralExplanation,
                    SpecialExplanation = transactionAssignmentDTO.SpecialExplanation,
                }).ToList();
            return transactionAssignments;
        }
        public static List<TransactionAssignment> Map(IList<VIPTransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            if (transactionAssignmentDTOs == null || !transactionAssignmentDTOs.Any())
            {
                return null;
            }

            List<TransactionAssignment> transactionAssignments = transactionAssignmentDTOs
                .Select(transactionAssignmentDTO => new TransactionAssignment()
                {
                    FromEntityId = transactionAssignmentDTO.FromOrgUnitId,
                    ToEntityId = transactionAssignmentDTO.ToOrgUnitId,
                    ActionId = transactionAssignmentDTO.ActionId,
                    Description = transactionAssignmentDTO.Remarks,
                    ToUserId = transactionAssignmentDTO.ToUserId,
                    DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                    TrayId = transactionAssignmentDTO.TrayId,
                    ReporterId = transactionAssignmentDTO.ReporterId,

                }).ToList();
            return transactionAssignments;
        }

        public static TransactionAssignment Map(TransactionAssignmentDTO transactionAssignmentDTO)
        {
            if (transactionAssignmentDTO == null)
            {
                return null;
            }

            TransactionAssignment transactionAssignment = new TransactionAssignment()
            {
                FromEntityId = transactionAssignmentDTO.FromOrgUnitId,
                ToEntityId = transactionAssignmentDTO.ToOrgUnitId,
                ActionId = transactionAssignmentDTO.ActionId,
                Description = transactionAssignmentDTO.Remarks,
                ToUserId = transactionAssignmentDTO.ToUserId,
                DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                TrayId = transactionAssignmentDTO.TrayId,
                ReporterId = transactionAssignmentDTO.ReporterId
            };
            return transactionAssignment;
        }
        public static TransactionAssignmentDTO Map(TransactionAssignment transactionAssignment)
        {
            if (transactionAssignment == null)
            {
                return null;
            }

            TransactionAssignmentDTO transactionAssignmentDTO = new TransactionAssignmentDTO();

            transactionAssignmentDTO.FromOrgUnitId = transactionAssignment.FromEntityId;
            transactionAssignmentDTO.ToOrgUnitId = transactionAssignment.ToEntityId;
            transactionAssignmentDTO.ActionId = transactionAssignment.ActionId.HasValue ? transactionAssignment.ActionId.Value : 42;
            transactionAssignmentDTO.Remarks = transactionAssignment.Description;
            transactionAssignmentDTO.ToUserId = transactionAssignment?.ToUserId;
            transactionAssignmentDTO.DeliveryMethodId = transactionAssignment.DeliveryMethodId;
            transactionAssignmentDTO.FromUserId = transactionAssignment.FromUserId;
            transactionAssignmentDTO.TrayId = transactionAssignment.TrayId;
            transactionAssignmentDTO.PhysicalEntityId = transactionAssignment.PhysicalEntityId;
            transactionAssignmentDTO.PhysicalUserId = transactionAssignment.PhysicalUserId;
            transactionAssignmentDTO.PhysicalEntityName = transactionAssignment.PhysicalEntity?.LocalName;
            transactionAssignmentDTO.PhysicalUserName = transactionAssignment.PhysicalUser?.LocalName;
            transactionAssignmentDTO.ToUserName = transactionAssignment.ToUser == null ? string.Empty : transactionAssignment.ToUser.LocalName;
            transactionAssignmentDTO.ToOrgUnitName = transactionAssignment.ToEntity.LocalName;
            transactionAssignmentDTO.Date = transactionAssignment.Date;
            transactionAssignmentDTO.DateH = transactionAssignment.DateH;
            transactionAssignmentDTO.PhysicalDate = transactionAssignment.PhysicalDate;
            transactionAssignmentDTO.PhysicalDateH = transactionAssignment.PhysicalDateH;
            transactionAssignmentDTO.FromUserInternalNumber = transactionAssignment?.FromUser?.InternalNumber;
            transactionAssignmentDTO.ToUserInternalNumber = transactionAssignment?.ToUser?.InternalNumber;

            return transactionAssignmentDTO;
        }

        public static List<TransactionAssignmentDTO> Map(IList<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries)
        {
            if (assignmentPaperBeneficiaries == null || !assignmentPaperBeneficiaries.Any())
            {
                return null;
            }
            List<TransactionAssignmentDTO> transactionAssignmentDTOs = assignmentPaperBeneficiaries
                .Select(assignmentPaperBeneficiary => new TransactionAssignmentDTO()
                {
                    ToOrgUnitId = assignmentPaperBeneficiary.OrgUnit.Id,
                    ToOrgUnitName = assignmentPaperBeneficiary.OrgUnit.LocalName,
                    ToUserId = assignmentPaperBeneficiary.User != null ? assignmentPaperBeneficiary.User.Id : -1,
                    ToUserName = assignmentPaperBeneficiary.User != null ? assignmentPaperBeneficiary.User.LocalName : null
                }).ToList();

            return transactionAssignmentDTOs;
        }

        public static List<TransactionAssignment> Map(List<MCS.DTO.Transaction.Vip.VIPTransactionAssignmentDto> transactionAssignmentDTOs)
        {
            if (transactionAssignmentDTOs == null || !transactionAssignmentDTOs.Any())
            {
                return null;
            }

            List<TransactionAssignment> transactionAssignments = transactionAssignmentDTOs
                .Select(transactionAssignmentDTO => new TransactionAssignment()
                {
                    FromEntityId = transactionAssignmentDTO.FromEntityId,
                    ToEntityId = transactionAssignmentDTO.ToOrgUnitId,
                    ActionId = transactionAssignmentDTO.ActionId,
                    Description = transactionAssignmentDTO.Remarks,
                    ToUserId = transactionAssignmentDTO.ToUserId,
                    DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                    TrayId = transactionAssignmentDTO.TrayId,
                    SpecialExplanation = transactionAssignmentDTO.SpecialExplanation,
                    GeneralExplanation = transactionAssignmentDTO.GeneralExplanation,



                }).ToList();
            return transactionAssignments;
        }

    }
}