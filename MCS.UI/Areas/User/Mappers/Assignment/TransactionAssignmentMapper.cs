using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.DTO.Transaction.Vip;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.Admin.Mappers;

namespace MCS.UI.Areas.User.Mappers.Assignment
{
    public static class TransactionAssignmentMapper
    {
        public static List<TransactionAssignmentVM> Map(IList<TransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            if (transactionAssignmentDTOs == null || !transactionAssignmentDTOs.Any())
            {
                return new List<TransactionAssignmentVM>();
            }
            List<TransactionAssignmentVM> assignmentGroups = transactionAssignmentDTOs
                .Select(transactionAssignmentDTO => new TransactionAssignmentVM()
                {
                    ActionForAllId = transactionAssignmentDTO.ActionForAllId,
                    ActionId = transactionAssignmentDTO.ActionId,
                    ActionName = transactionAssignmentDTO.ActionName,
                    ActionNameForAll = transactionAssignmentDTO.ActionNameForAll,
                    ActionTypeForAllId = transactionAssignmentDTO.ActionTypeForAllId,
                    ActionTypeId = transactionAssignmentDTO.ActionTypeId,
                    Count = transactionAssignmentDTO.Count,
                    Date = transactionAssignmentDTO.Date,
                    DateH = transactionAssignmentDTO.DateH,
                    FromOrgUnitId = transactionAssignmentDTO.FromOrgUnitId,
                    FromOrgUnitName = transactionAssignmentDTO.FromOrgUnitName,
                    FromUserName = transactionAssignmentDTO.FromUserName,
                    GroupId = transactionAssignmentDTO.GroupId,
                    GroupName = transactionAssignmentDTO.GroupName,
                    GroupOrderNo = transactionAssignmentDTO.GroupOrderNo,
                    Id = transactionAssignmentDTO.Id,
                    IsAssigned = transactionAssignmentDTO.IsAssigned,
                    Remarks = transactionAssignmentDTO.Remarks,
                    RemarksForAll = transactionAssignmentDTO.RemarksForAll,
                    ToOrgUnitId = transactionAssignmentDTO.ToOrgUnitId,
                    ToOrgUnitName = transactionAssignmentDTO.ToOrgUnitName,
                    ToUserId = transactionAssignmentDTO.ToUserId,
                    ToUserName = transactionAssignmentDTO.ToUserName,
                    TrayName = transactionAssignmentDTO.TrayName,
                    DeliveryMethod = transactionAssignmentDTO.DeliveryMethod,
                    DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                    StringContent = transactionAssignmentDTO.StringContent,
                    SpecialExplanation = (SessionInfo.CurrentUser.Id == transactionAssignmentDTO.FromUserId || SessionInfo.CurrentUser.Id == transactionAssignmentDTO.ToUserId) ?
                    transactionAssignmentDTO.SpecialExplanation : "",
                    GeneralExplanation = transactionAssignmentDTO.GeneralExplanation,
                    ReceivedDate = transactionAssignmentDTO.ReceivedDate,
                    FromUserInternalNumber = transactionAssignmentDTO.FromUserInternalNumber,
                    ToUserInternalNumber = transactionAssignmentDTO.ToUserInternalNumber,

                }).ToList();

            return assignmentGroups;
        }
        public static List<TransactionAssignmentDTO> Map(IList<TransactionAssignmentVM> transactionAssignmentVMs)
        {
            if (transactionAssignmentVMs == null || !transactionAssignmentVMs.Any())
            {
                return new List<TransactionAssignmentDTO>();
            }
            List<TransactionAssignmentDTO> assignmentGroups = transactionAssignmentVMs
                .Select(transactionAssignmentVM => new TransactionAssignmentDTO()
                {
                    ActionForAllId = transactionAssignmentVM.ActionForAllId,
                    ActionId = transactionAssignmentVM.ActionId,
                    ActionName = transactionAssignmentVM.ActionName,
                    ActionNameForAll = transactionAssignmentVM.ActionNameForAll,
                    ActionTypeForAllId = transactionAssignmentVM.ActionTypeForAllId,
                    ActionTypeId = transactionAssignmentVM.ActionTypeId,
                    Count = transactionAssignmentVM.Count,
                    Date = transactionAssignmentVM.Date,
                    DateH = transactionAssignmentVM.DateH,
                    FromOrgUnitId = transactionAssignmentVM.FromOrgUnitId,
                    FromOrgUnitName = transactionAssignmentVM.FromOrgUnitName,
                    FromUserName = transactionAssignmentVM.FromUserName,
                    GroupId = transactionAssignmentVM.GroupId,
                    GroupName = transactionAssignmentVM.GroupName,
                    GroupOrderNo = transactionAssignmentVM.GroupOrderNo,
                    Id = transactionAssignmentVM.Id,
                    IsAssigned = transactionAssignmentVM.IsAssigned,
                    Remarks = transactionAssignmentVM.Remarks,
                    RemarksForAll = transactionAssignmentVM.RemarksForAll,
                    ToOrgUnitId = transactionAssignmentVM.ToOrgUnitId,
                    ToOrgUnitName = transactionAssignmentVM.ToOrgUnitName,
                    ToUserId = transactionAssignmentVM.ToUserId,
                    ToUserName = transactionAssignmentVM.ToUserName,
                    TrayName = transactionAssignmentVM.TrayName,
                    DeliveryMethod = transactionAssignmentVM.DeliveryMethod,
                    DeliveryMethodId = transactionAssignmentVM.DeliveryMethodId,
                    TrayId = transactionAssignmentVM.TrayId,
                    ReporterId = transactionAssignmentVM.ReporterId,
                    GeneralExplanation = transactionAssignmentVM.GeneralExplanation,
                    SpecialExplanation = transactionAssignmentVM.SpecialExplanation,
                }).ToList();

            return assignmentGroups;
        }
        public static TransactionAssignmentVM Map(TransactionAssignmentDTO transactionAssignmentDTO)
        {
            if (transactionAssignmentDTO != null)
            {
                return new TransactionAssignmentVM()
                {
                    ActionForAllId = transactionAssignmentDTO.ActionForAllId,
                    ActionId = transactionAssignmentDTO.ActionId,
                    ActionName = transactionAssignmentDTO.ActionName,
                    ActionNameForAll = transactionAssignmentDTO.ActionNameForAll,
                    ActionTypeForAllId = transactionAssignmentDTO.ActionTypeForAllId,
                    ActionTypeId = transactionAssignmentDTO.ActionTypeId,
                    Count = transactionAssignmentDTO.Count,
                    Date = transactionAssignmentDTO.Date,
                    DateH = transactionAssignmentDTO.DateH,
                    FromOrgUnitId = transactionAssignmentDTO.FromOrgUnitId,
                    FromOrgUnitName = transactionAssignmentDTO.FromOrgUnitName,
                    FromUserName = transactionAssignmentDTO.FromUserName,
                    GroupId = transactionAssignmentDTO.GroupId,
                    GroupName = transactionAssignmentDTO.GroupName,
                    GroupOrderNo = transactionAssignmentDTO.GroupOrderNo,
                    Id = transactionAssignmentDTO.Id,
                    IsAssigned = transactionAssignmentDTO.IsAssigned,
                    Remarks = transactionAssignmentDTO.Remarks,
                    RemarksForAll = transactionAssignmentDTO.RemarksForAll,
                    ToOrgUnitId = transactionAssignmentDTO.ToOrgUnitId,
                    ToOrgUnitName = transactionAssignmentDTO.ToOrgUnitName,
                    ToUserId = transactionAssignmentDTO.ToUserId,
                    ToUserName = transactionAssignmentDTO.ToUserName,
                    TrayName = transactionAssignmentDTO.TrayName,
                    DeliveryMethod = transactionAssignmentDTO.DeliveryMethod,
                    DeliveryMethodId = transactionAssignmentDTO.DeliveryMethodId,
                    PhysicalEntityId = transactionAssignmentDTO.PhysicalEntityId,
                    PhysicalUserId = transactionAssignmentDTO.PhysicalUserId,
                    PhysicalEntityName = transactionAssignmentDTO.PhysicalEntityName,
                    PhysicalUserName = transactionAssignmentDTO.PhysicalUserName,
                    PhysicalDate = transactionAssignmentDTO.PhysicalDate,
                    PhysicalDateH = transactionAssignmentDTO.PhysicalDateH,
                    FromUserInternalNumber = transactionAssignmentDTO.FromUserInternalNumber,
                    ToUserInternalNumber = transactionAssignmentDTO.ToUserInternalNumber,
                };
            }
            return new TransactionAssignmentVM();
        }
        public static TransactionAssignmentDTO Map(TransactionAssignmentVM transactionAssignmentVM)
        {
            if (transactionAssignmentVM != null)
            {
                return new TransactionAssignmentDTO()
                {
                    ActionForAllId = transactionAssignmentVM.ActionForAllId,
                    ActionId = transactionAssignmentVM.ActionId,
                    ActionName = transactionAssignmentVM.ActionName,
                    ActionNameForAll = transactionAssignmentVM.ActionNameForAll,
                    ActionTypeForAllId = transactionAssignmentVM.ActionTypeForAllId,
                    ActionTypeId = transactionAssignmentVM.ActionTypeId,
                    Count = transactionAssignmentVM.Count,
                    Date = transactionAssignmentVM.Date,
                    DateH = transactionAssignmentVM.DateH,
                    FromOrgUnitId = transactionAssignmentVM.FromOrgUnitId,
                    FromOrgUnitName = transactionAssignmentVM.FromOrgUnitName,
                    FromUserName = transactionAssignmentVM.FromUserName,
                    GroupId = transactionAssignmentVM.GroupId,
                    GroupName = transactionAssignmentVM.GroupName,
                    GroupOrderNo = transactionAssignmentVM.GroupOrderNo,
                    Id = transactionAssignmentVM.Id,
                    IsAssigned = transactionAssignmentVM.IsAssigned,
                    Remarks = transactionAssignmentVM.Remarks,
                    RemarksForAll = transactionAssignmentVM.RemarksForAll,
                    ToOrgUnitId = transactionAssignmentVM.ToOrgUnitId,
                    ToOrgUnitName = transactionAssignmentVM.ToOrgUnitName,
                    ToUserId = transactionAssignmentVM.ToUserId,
                    ToUserName = transactionAssignmentVM.ToUserName,
                    TrayName = transactionAssignmentVM.TrayName,
                    DeliveryMethod = transactionAssignmentVM.DeliveryMethod,
                    DeliveryMethodId = transactionAssignmentVM.DeliveryMethodId,
                    ReporterId = transactionAssignmentVM.ReporterId,

                };
            }
            return new TransactionAssignmentDTO();
        }



        public static List<VIPTransactionAssignmentDto> VipMap(List<VIPTransactionAssignmentVM> transactionAssignmentVMs, string generalExplanation, string notes)
        {
            List<VIPTransactionAssignmentDto> vIPTransactionAssignmentDtos = new List<VIPTransactionAssignmentDto>();
            if (transactionAssignmentVMs != null && transactionAssignmentVMs.Count > 0)
            {
                foreach (var transactionAssignmentVM in transactionAssignmentVMs)
                {
                    vIPTransactionAssignmentDtos.Add(VipMap(transactionAssignmentVM, generalExplanation, notes));
                }
            }
            return vIPTransactionAssignmentDtos;
        }

        public static VIPTransactionAssignmentDto VipMap(VIPTransactionAssignmentVM transactionAssignmentVM, string generalExplanation, string notes)
        {
            if (transactionAssignmentVM == null)
            {
                 return null;
            }
            VIPTransactionAssignmentDto vIPTransactionAssignmentDtos = new VIPTransactionAssignmentDto
            {
                ActionId = transactionAssignmentVM.ActionId,
                ChkConstant = transactionAssignmentVM.ChkConstant,
                DeliveryMethodId = transactionAssignmentVM.DeliveryMethodId,
                GroupId = transactionAssignmentVM.GroupId,
                Id = transactionAssignmentVM.Id,
                IsAssigned = transactionAssignmentVM.IsAssigned ?? false,
                IsCopy = transactionAssignmentVM.IsCopy,
                ToOrgUnitId = transactionAssignmentVM.ToOrgUnitId,
                Remarks = notes,
                ToUserId = transactionAssignmentVM.ToUserId,
                SpecialExplanation = transactionAssignmentVM.SpecialExplanation,
                FromEntityId = SessionInfo.OrgUnitId,
                FromUserId = SessionInfo.CurrentUser.Id,
                GeneralExplanation = generalExplanation,
                TrayId = transactionAssignmentVM.ToUserId.HasValue ? (int)TrayType.MyTransactions : (int)TrayType.OrgUnit,
                IsBcc = transactionAssignmentVM.IsBcc,



            };

            return vIPTransactionAssignmentDtos;
        }

    }
}