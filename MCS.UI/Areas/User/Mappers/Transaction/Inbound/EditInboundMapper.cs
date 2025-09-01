using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Mappers.Transaction.Inbound
{
    public static class EditInboundMapper
    {
        public static EditInboundVM Map(EditInboundDTO EditInboundDTO)
        {
            if (EditInboundDTO != null)
            {
                EditInboundVM editInboundVM = new EditInboundVM()
                {
                    Id = EditInboundDTO.Id,
                    Attachments = TransactionAttachmentMapper.Map(EditInboundDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(EditInboundDTO.DocumentDTO),
                    HijriRecordDate = EditInboundDTO.HijriRecordDate,
                    InboundBasicInfoEdit = EditInboundBasicInfoMapper.Map(EditInboundDTO.InboundBasicInfoEdit),
                    IsSigned = EditInboundDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(EditInboundDTO.Links),
                    ModifiedByUserId = EditInboundDTO.ModifiedByUserId,
                    Names = TransactionNameMapper.Map(EditInboundDTO.Names),
                    OrgUnitId = EditInboundDTO.OrgUnitId,
                    RecordDate = EditInboundDTO.RecordDate,
                    StatusId = EditInboundDTO.StatusId,
                    UserId = EditInboundDTO.UserId,
                    Copies = TransactionCopyMapper.Map(EditInboundDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(EditInboundDTO.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(EditInboundDTO.FollowUps),
                    AssignedFromUser = UserProfileMapper.Map(EditInboundDTO.FromUser),
                    AssignedToUser = UserProfileMapper.Map(EditInboundDTO.ToUser),
                    ProcessPeriodTransaction = EditInboundDTO.ProcessPeriodTransaction,
                    SavedTransactionAssignment = EditInboundDTO.SavedTransactionAssignment,
                    // SideContactExternalEntityID = EditInboundDTO.SideContactExternalEntityID,
                    // NumberContact = EditInboundDTO.NumberContact

                };

                return editInboundVM;
            }
            return new EditInboundVM();
        }
        public static VIPEditInboundVM VIPMap(EditInboundDTO EditInboundDTO)
        {
            if (EditInboundDTO != null)
            {
                VIPEditInboundVM editInboundVM = new VIPEditInboundVM()
                {
                    Id = EditInboundDTO.Id,
                    Attachments = TransactionAttachmentMapper.Map(EditInboundDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(EditInboundDTO.DocumentDTO),
                    HijriRecordDate = EditInboundDTO.HijriRecordDate,
                    InboundBasicInfoEdit = EditInboundBasicInfoMapper.VIPMap(EditInboundDTO.InboundBasicInfoEdit),
                    IsSigned = EditInboundDTO.IsSigned,
                    Links = TransactionLinkMapper.VipMap(EditInboundDTO.Links),
                    ModifiedByUserId = EditInboundDTO.ModifiedByUserId,
                    Names = TransactionNameMapper.Map(EditInboundDTO.Names),
                    OrgUnitId = EditInboundDTO.OrgUnitId,
                    RecordDate = EditInboundDTO.RecordDate,
                    StatusId = EditInboundDTO.StatusId,
                    UserId = EditInboundDTO.UserId,
                    Copies = TransactionCopyMapper.Map(EditInboundDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(EditInboundDTO.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(EditInboundDTO.FollowUps),
                    AssignedFromUser = UserProfileMapper.Map(EditInboundDTO.FromUser),
                    AssignedToUser = UserProfileMapper.Map(EditInboundDTO.ToUser),
                    ProcessPeriodTransaction = EditInboundDTO.ProcessPeriodTransaction,
                    RemindDate = EditInboundDTO?.InboundBasicInfoEdit?.RemindDate,
                    RemindDateH = EditInboundDTO?.InboundBasicInfoEdit?.RemindDateH,
                    IsEnableAssignBack = EditInboundDTO.FromUser.Id != EditInboundDTO.ToUser.Id,
                    SavedTransactionAssignment = EditInboundDTO.SavedTransactionAssignment

                    // SideContactExternalEntityID = EditInboundDTO.SideContactExternalEntityID,
                    // NumberContact = EditInboundDTO.NumberContact

                };
                editInboundVM.InboundBasicInfoEdit.EntityName = EditInboundDTO.FromOrgunitName;
                return editInboundVM;
            }
            return new VIPEditInboundVM();
        }


        public static EditInboundDTO Map(EditInboundVM editInboundVM)
        {
            if (editInboundVM == null)
            {
                return new EditInboundDTO();
            }
            EditInboundDTO editInboundDTO = new EditInboundDTO()
            {
                Id = editInboundVM.Id,
                Attachments = TransactionAttachmentMapper.Map(editInboundVM.Attachments),
                DocumentDTO = DocumentMapper.Map(editInboundVM.DocumentVM),
                HijriRecordDate = editInboundVM.HijriRecordDate,
                InboundBasicInfoEdit = EditInboundBasicInfoMapper.Map(editInboundVM.InboundBasicInfoEdit),
                IsSigned = editInboundVM.IsSigned,
                Links = TransactionLinkMapper.Map(editInboundVM.Links),
                ModifiedByUserId = editInboundVM.ModifiedByUserId,
                Names = TransactionNameMapper.Map(editInboundVM.Names),
                OrgUnitId = editInboundVM.OrgUnitId,
                RecordDate = editInboundVM.RecordDate,
                StatusId = editInboundVM.StatusId,
                UserId = editInboundVM.UserId,
                Copies = TransactionCopyMapper.Map(editInboundVM.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(editInboundVM.ExternalCopies),
                FollowUps = TransactionFollowUpMapper.Map(editInboundVM.FollowUps),
                ProcessPeriodTransaction = (int)editInboundVM.InboundBasicInfoEdit.ProcessPeriodTransaction,
                NumberContact = editInboundVM.InboundBasicInfoEdit.NumberContact,
            };

            editInboundDTO.InboundBasicInfoEdit.SideContactExternalEntityID = editInboundVM.InboundBasicInfoEdit.SideContactExternalEntityID;
            return editInboundDTO;
        }
        public static EditInboundDTO Map(VIPEditInboundVM editInboundVM)
        {
            if (editInboundVM == null)
            {
                return new EditInboundDTO();
            }
            EditInboundDTO editInboundDTO = new EditInboundDTO()
            {
                Id = editInboundVM.Id,
                Attachments = TransactionAttachmentMapper.Map(editInboundVM.Attachments),
                DocumentDTO = DocumentMapper.Map(editInboundVM.DocumentVM),
                HijriRecordDate = editInboundVM.HijriRecordDate,
                InboundBasicInfoEdit = EditInboundBasicInfoMapper.Map(editInboundVM.InboundBasicInfoEdit),
                IsSigned = editInboundVM.IsSigned,
                Links = TransactionLinkMapper.Map(editInboundVM.Links),
                ModifiedByUserId = editInboundVM.ModifiedByUserId,
                Names = TransactionNameMapper.Map(editInboundVM.Names),
                OrgUnitId = editInboundVM.OrgUnitId,
                RecordDate = editInboundVM.RecordDate,
                StatusId = editInboundVM.StatusId,
                UserId = editInboundVM.UserId,
                Copies = TransactionCopyMapper.Map(editInboundVM.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(editInboundVM.ExternalCopies),
                FollowUps = TransactionFollowUpMapper.Map(editInboundVM.FollowUps),
                ProcessPeriodTransaction = (int)editInboundVM.InboundBasicInfoEdit.ProcessPeriodTransaction,
                NumberContact = editInboundVM.InboundBasicInfoEdit.NumberContact,
                RemindDate = editInboundVM.RemindDate,
                RemindDateH = editInboundVM.RemindDateH,
            };

            editInboundDTO.InboundBasicInfoEdit.RemindDate = editInboundVM.RemindDate;
            editInboundDTO.InboundBasicInfoEdit.RemindDateH = editInboundVM.RemindDateH;
            editInboundDTO.InboundBasicInfoEdit.SideContactExternalEntityID = editInboundVM.InboundBasicInfoEdit.SideContactExternalEntityID;

            return editInboundDTO;
        }



        public static VipInboundUpdateDto VIPEditMap(VipInboundUpdateVM editInboundVM)
        {

            VipInboundUpdateDto vipInboundUpdateDto = new VipInboundUpdateDto
            {

                Id = editInboundVM.InboundId,
                Assignments = TransactionAssignmentMapper.VipMap(editInboundVM.AssignmentVMs, editInboundVM.ExplanationForAssignmentPaper, editInboundVM.Notes),
                ExplanationConfedentialityForAssignmentPaperId = editInboundVM.ExplanationConfedentialityForAssignmentPaperId,
                ExplanationForAssignmentPaper = editInboundVM.ExplanationForAssignmentPaper,
                Notes = editInboundVM.Notes,
                PrivateFollowUps = TransactionFollowUpMapper.MapPrivate(editInboundVM.PrivateFollowUps),
                PublicFollowUps = TransactionFollowUpMapper.MapPublic(editInboundVM.PublicFollowUps),
                DocumentDTO = DocumentMapper.Map(editInboundVM.DocumentVM),
                Summary = editInboundVM.Summary,


            };

            vipInboundUpdateDto.ProccessDescriptions = new Dictionary<int, string>();
            vipInboundUpdateDto.ProccessDescriptions.Add((int)FollowupType.Privet, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPrivetFollowUp"));
            vipInboundUpdateDto.ProccessDescriptions.Add((int)FollowupType.Public, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp"));
            return vipInboundUpdateDto;
        }
    }

}