using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound
{
    public class OutboundDraftMapper
    {
        public static AddOutboundDraftDTO Map(AddOutboundDraftVM addOutboundDraftVM)
        {
            if (addOutboundDraftVM != null)
            {
                AddOutboundDraftDTO addOutboundDraftDTO = new AddOutboundDraftDTO()
                {
                    DocumentDTO = DocumentMapper.Map(addOutboundDraftVM.DocumentVM),
                    Attachments = TransactionAttachmentMapper.Map(addOutboundDraftVM.Attachments),
                    Copies = TransactionCopyMapper.Map(addOutboundDraftVM.Copies),
                    EditorType = addOutboundDraftVM.EditorType,
                    HijriRecordDate = addOutboundDraftVM.HijriRecordDate,
                    Id = addOutboundDraftVM.Id,
                    IsSigned = addOutboundDraftVM.IsSigned,
                    Links = TransactionLinkMapper.Map(addOutboundDraftVM.Links),
                    Names = TransactionNameMapper.Map(addOutboundDraftVM.Names),
                    OrgUnitId = addOutboundDraftVM.OrgUnitId,
                    OutboundDraftBasicInfo = OutboundDraftBasicInfoMapper.Map(addOutboundDraftVM.OutboundDraftBasicInfo),
                    RecordDate = addOutboundDraftVM.RecordDate,
                    StatusId = addOutboundDraftVM.StatusId,
                    UserId = addOutboundDraftVM.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundDraftVM.ExternalCopies)
                };
                return addOutboundDraftDTO;
            }
            return new AddOutboundDraftDTO();
        }
        public static AddOutboundDraftVM Map(AddOutboundDraftDTO addOutboundDraftDTO)
        {
            if (addOutboundDraftDTO != null)
            {
                AddOutboundDraftVM addOutboundDraftVM = new AddOutboundDraftVM()
                {
                    DocumentVM = DocumentMapper.Map(addOutboundDraftDTO.DocumentDTO),
                    Attachments = TransactionAttachmentMapper.Map(addOutboundDraftDTO.Attachments),
                    Copies = TransactionCopyMapper.Map(addOutboundDraftDTO.Copies),
                    EditorType = addOutboundDraftDTO.EditorType,
                    HijriRecordDate = addOutboundDraftDTO.HijriRecordDate,
                    Id = addOutboundDraftDTO.Id,
                    IsSigned = addOutboundDraftDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(addOutboundDraftDTO.Links),
                    Names = TransactionNameMapper.Map(addOutboundDraftDTO.Names),
                    OrgUnitId = addOutboundDraftDTO.OrgUnitId,
                    OutboundDraftBasicInfo = OutboundDraftBasicInfoMapper.Map(addOutboundDraftDTO.OutboundDraftBasicInfo),
                    RecordDate = addOutboundDraftDTO.RecordDate,
                    StatusId = addOutboundDraftDTO.StatusId,
                    UserId = addOutboundDraftDTO.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundDraftDTO.ExternalCopies)
                };
                return addOutboundDraftVM;
            }
            return new AddOutboundDraftVM();
        }
        public static VIPEditOutboundDraftVM VIPMap(EditOutboundDraftDTO editOutboundInternalDTO)
        {
            if (editOutboundInternalDTO != null)
            {
                VIPEditOutboundDraftVM editOutboundInternalVM = new VIPEditOutboundDraftVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(editOutboundInternalDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(editOutboundInternalDTO.DocumentDTO),
                    OldDocumentVM = DocumentMapper.Map(editOutboundInternalDTO.OldDocumentDTO),
                    HijriRecordDate = editOutboundInternalDTO.HijriRecordDate,
                    Id = editOutboundInternalDTO.Id,
                    IsSigned = editOutboundInternalDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundInternalDTO.Links),
                    Names = TransactionNameMapper.Map(editOutboundInternalDTO.Names),
                    OrgUnitId = editOutboundInternalDTO.OrgUnitId,
                    OutboundDraftBasicInfo = OutboundDraftBasicInfoMapper.VIPMap(editOutboundInternalDTO.OutboundDraftBasicInfo),
                    RecordDate = editOutboundInternalDTO.RecordDate,
                    UserId = editOutboundInternalDTO.UserId,
                    Copies = TransactionCopyMapper.Map(editOutboundInternalDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundInternalDTO.ExternalCopies),


                };

                return editOutboundInternalVM;
            }
            return new VIPEditOutboundDraftVM();
        }
        public static EditOutboundDraftVM Map(EditOutboundDraftDTO editOutboundDraftDTO)
        {
            if (editOutboundDraftDTO != null)
            {
                EditOutboundDraftVM editOutboundDraftVM = new EditOutboundDraftVM()
                {
                    DocumentVM = DocumentMapper.Map(editOutboundDraftDTO.DocumentDTO),
                    Attachments = TransactionAttachmentMapper.Map(editOutboundDraftDTO.Attachments),
                    Copies = TransactionCopyMapper.Map(editOutboundDraftDTO.Copies),
                    EditorType = editOutboundDraftDTO.EditorType.HasValue ? editOutboundDraftDTO.EditorType.Value : MCS.Common.EditorType.TextEditor,
                    HijriRecordDate = editOutboundDraftDTO.HijriRecordDate,
                    Id = editOutboundDraftDTO.Id,
                    IsSigned = editOutboundDraftDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundDraftDTO.Links),
                    Names = TransactionNameMapper.Map(editOutboundDraftDTO.Names),
                    OrgUnitId = editOutboundDraftDTO.OrgUnitId,
                    OutboundDraftBasicInfo = OutboundDraftBasicInfoMapper.Map(editOutboundDraftDTO.OutboundDraftBasicInfo),
                    RecordDate = editOutboundDraftDTO.RecordDate,
                    StatusId = editOutboundDraftDTO.StatusId,
                    UserId = editOutboundDraftDTO.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundDraftDTO.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(editOutboundDraftDTO.FollowUps)
                };
                // editOutboundDraftVM.EntityName = editOutboundDraftDTO.FromOrgunitName;
                return editOutboundDraftVM;
            }
            return new EditOutboundDraftVM();
        }
        public static EditOutboundDraftDTO Map(EditOutboundDraftVM editOutboundDraftVM)
        {
            if (editOutboundDraftVM != null)
            {
                EditOutboundDraftDTO editOutboundDraftDTO = new EditOutboundDraftDTO()
                {
                    DocumentDTO = DocumentMapper.Map(editOutboundDraftVM.DocumentVM),
                    Attachments = TransactionAttachmentMapper.Map(editOutboundDraftVM.Attachments),
                    Copies = TransactionCopyMapper.Map(editOutboundDraftVM.Copies),
                    EditorType = editOutboundDraftVM.EditorType,
                    HijriRecordDate = editOutboundDraftVM.HijriRecordDate,
                    Id = editOutboundDraftVM.Id,
                    IsSigned = editOutboundDraftVM.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundDraftVM.Links),
                    Names = TransactionNameMapper.Map(editOutboundDraftVM.Names),
                    OrgUnitId = editOutboundDraftVM.OrgUnitId,
                    OutboundDraftBasicInfo = OutboundDraftBasicInfoMapper.Map(editOutboundDraftVM.OutboundDraftBasicInfo),
                    RecordDate = editOutboundDraftVM.RecordDate,
                    StatusId = editOutboundDraftVM.StatusId,
                    UserId = editOutboundDraftVM.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundDraftVM.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(editOutboundDraftVM.FollowUps)

                };
                return editOutboundDraftDTO;
            }
            return new EditOutboundDraftDTO();
        }
        public static VipOutboundDraftDto VIPEditMap(VipOutboundDraftUpdateVM editOutboundDraftVM)
        {

            VipOutboundDraftDto vipInboundUpdateDto = new VipOutboundDraftDto
            {

                Id = editOutboundDraftVM.OutboundDraftId,
                Assignments = TransactionAssignmentMapper.VipMap(editOutboundDraftVM.AssignmentVMs, editOutboundDraftVM.ExplanationForAssignmentPaper, editOutboundDraftVM.Notes),
                ExplanationConfedentialityForAssignmentPaperId = editOutboundDraftVM.ExplanationConfedentialityForAssignmentPaperId,
                ExplanationForAssignmentPaper = editOutboundDraftVM.ExplanationForAssignmentPaper,
                Notes = editOutboundDraftVM.Notes,
                PrivateFollowUps = TransactionFollowUpMapper.MapPrivate(editOutboundDraftVM.PrivateFollowUps),
                PublicFollowUps = TransactionFollowUpMapper.MapPublic(editOutboundDraftVM.PublicFollowUps),
                MainDocumentData = editOutboundDraftVM.DocumentBase64String,
                IsSigned = editOutboundDraftVM.IsSigned,
                OldMainDocumentData= editOutboundDraftVM.OldDocumentBase64String

            };

            vipInboundUpdateDto.ProccessDescriptions = new Dictionary<int, string>();
            vipInboundUpdateDto.ProccessDescriptions.Add((int)FollowupType.Privet, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPrivetFollowUp"));
            vipInboundUpdateDto.ProccessDescriptions.Add((int)FollowupType.Public, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp"));
            return vipInboundUpdateDto;
        }
    }
}