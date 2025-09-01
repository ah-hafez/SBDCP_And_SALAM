using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using System;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound.External
{
    public class OutboundExternalMapper
    {
        public static AddOutboundExternalVM Map(AddOutboundExternalDTO addOutboundExternalDTO)
        {
            if (addOutboundExternalDTO != null)
            {
                AddOutboundExternalVM addOutboundExternalVM = new AddOutboundExternalVM()
                {
                    EditorTypeId = addOutboundExternalDTO.EditorTypeId,
                    OutboundExternalBasicInfo = OutboundExternalBasicInfoMapper.Map(addOutboundExternalDTO.OutboundExternalBasicInfo),
                    Copies = TransactionCopyMapper.Map(addOutboundExternalDTO.Copies),
                    Attachments = TransactionAttachmentMapper.Map(addOutboundExternalDTO.Attachments),
                    Names = TransactionNameMapper.Map(addOutboundExternalDTO.Names),
                    Links = TransactionLinkMapper.Map(addOutboundExternalDTO.Links),
                    DocumentVM = DocumentMapper.Map(addOutboundExternalDTO.DocumentDTO),
                    HijriRecordDate = addOutboundExternalDTO.HijriRecordDate,
                    Id = addOutboundExternalDTO.Id,
                    IsSigned = addOutboundExternalDTO.IsSigned,
                    OrgUnitId = addOutboundExternalDTO.OrgUnitId,
                    RecordDate = addOutboundExternalDTO.RecordDate,
                    StatusId = addOutboundExternalDTO.StatusId,
                    UserId = addOutboundExternalDTO.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundExternalDTO.ExternalCopies),

                };
                return addOutboundExternalVM;
            }
            return null;
        }
        public static AddOutboundExternalDTO Map(AddOutboundExternalVM addOutboundExternalVM)
        {
            if (addOutboundExternalVM != null)
            {
                AddOutboundExternalDTO addOutboundExternalDTO = new AddOutboundExternalDTO()
                {
                    EditorTypeId = addOutboundExternalVM.EditorTypeId,
                    OutboundExternalBasicInfo = OutboundExternalBasicInfoMapper.Map(addOutboundExternalVM.OutboundExternalBasicInfo),
                    Copies = TransactionCopyMapper.Map(addOutboundExternalVM.Copies),
                    Attachments = TransactionAttachmentMapper.Map(addOutboundExternalVM.Attachments),
                    Names = TransactionNameMapper.Map(addOutboundExternalVM.Names),
                    Links = TransactionLinkMapper.Map(addOutboundExternalVM.Links),
                    DocumentDTO = DocumentMapper.Map(addOutboundExternalVM.DocumentVM),
                    OldDocumentDTO = DocumentMapper.Map(addOutboundExternalVM.OldDocumentVM),
                    HijriRecordDate = addOutboundExternalVM.HijriRecordDate,
                    Id = addOutboundExternalVM.Id,
                    IsSigned = addOutboundExternalVM.IsSigned,
                    OrgUnitId = addOutboundExternalVM.OrgUnitId,
                    RecordDate = addOutboundExternalVM.RecordDate,
                    StatusId = addOutboundExternalVM.StatusId,
                    UserId = addOutboundExternalVM.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundExternalVM.ExternalCopies)
                };
                return addOutboundExternalDTO;
            }
            return null;
        }
        public static EditOutboundExternalDTO Map(EditOutboundExternalVM editOutboundExternalVM)
        {
            if (editOutboundExternalVM != null)
            {
                EditOutboundExternalDTO addOutboundExternalDTO = new EditOutboundExternalDTO()
                {
                    EditorType = editOutboundExternalVM.EditorType,
                    OutboundExternalBasicInfo = OutboundExternalBasicInfoMapper.Map(editOutboundExternalVM.OutboundExternalBasicInfo),
                    Copies = TransactionCopyMapper.Map(editOutboundExternalVM.Copies),
                    Attachments = TransactionAttachmentMapper.Map(editOutboundExternalVM.Attachments),
                    Names = TransactionNameMapper.Map(editOutboundExternalVM.Names),
                    Links = TransactionLinkMapper.Map(editOutboundExternalVM.Links),
                    DocumentDTO = DocumentMapper.Map(editOutboundExternalVM.DocumentVM),
                    OldDocumentDTO = DocumentMapper.Map(editOutboundExternalVM.OldDocumentVM),
                    HijriRecordDate = editOutboundExternalVM.HijriRecordDate,
                    Id = editOutboundExternalVM.Id,
                    IsSigned = editOutboundExternalVM.IsSigned,
                    OrgUnitId = editOutboundExternalVM.OrgUnitId,
                    RecordDate = editOutboundExternalVM.RecordDate,
                    StatusId = editOutboundExternalVM.StatusId,
                    UserId = editOutboundExternalVM.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundExternalVM.ExternalCopies),

                };
                return addOutboundExternalDTO;
            }
            return null;
        }
        public static EditOutboundExternalVM Map(EditOutboundExternalDTO editOutboundExternalDTO)
        {
            if (editOutboundExternalDTO != null)
            {
                EditOutboundExternalVM addOutboundExternalVM = new EditOutboundExternalVM()
                {
                    EditorType = editOutboundExternalDTO.EditorType,
                    OutboundExternalBasicInfo = OutboundExternalBasicInfoMapper.Map(editOutboundExternalDTO.OutboundExternalBasicInfo),
                    Copies = TransactionCopyMapper.Map(editOutboundExternalDTO.Copies),
                    Attachments = TransactionAttachmentMapper.Map(editOutboundExternalDTO.Attachments),
                    Names = TransactionNameMapper.Map(editOutboundExternalDTO.Names),
                    Links = TransactionLinkMapper.Map(editOutboundExternalDTO.Links),
                    DocumentVM = DocumentMapper.Map(editOutboundExternalDTO.DocumentDTO),
                    OldDocumentVM = DocumentMapper.Map(editOutboundExternalDTO.OldDocumentDTO),
                    HijriRecordDate = editOutboundExternalDTO.HijriRecordDate,
                    Id = editOutboundExternalDTO.Id,
                    IsSigned = editOutboundExternalDTO.IsSigned,
                    OrgUnitId = editOutboundExternalDTO.OrgUnitId,
                    RecordDate = editOutboundExternalDTO.RecordDate,
                    StatusId = editOutboundExternalDTO.StatusId,
                    UserId = editOutboundExternalDTO.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundExternalDTO.ExternalCopies),
                    AssignedFromUser = UserProfileMapper.Map(editOutboundExternalDTO.FromUser),
                    AssignedToUser = UserProfileMapper.Map(editOutboundExternalDTO.ToUser),
                    SavedTransactionAssignment = editOutboundExternalDTO.SavedTransactionAssignment
                };
                return addOutboundExternalVM;
            }
            return null;
        }

        internal static VIPEditOutboundExternalVM VIPMap(EditOutboundExternalDTO outboundExternalDTO)
        {
            if (outboundExternalDTO != null)
            {
                VIPEditOutboundExternalVM editOutboundInternalVM = new VIPEditOutboundExternalVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(outboundExternalDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(outboundExternalDTO.DocumentDTO),
                    HijriRecordDate = outboundExternalDTO.HijriRecordDate,
                    Id = outboundExternalDTO.Id,
                    IsSigned = outboundExternalDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(outboundExternalDTO.Links),
                    Names = TransactionNameMapper.Map(outboundExternalDTO.Names),
                    OrgUnitId = outboundExternalDTO.OrgUnitId,
                    OutboundExternalBasicInfoEdit = OutboundExternalBasicInfoMapper.VIPMap(outboundExternalDTO.OutboundExternalBasicInfo),
                    RecordDate = outboundExternalDTO.RecordDate,
                    UserId = outboundExternalDTO.UserId,
                    Copies = TransactionCopyMapper.Map(outboundExternalDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundExternalDTO.ExternalCopies),
                    IsEnableAssignBack = outboundExternalDTO.FromUser.Id != outboundExternalDTO.ToUser.Id,
                    RemindDate = outboundExternalDTO.OutboundExternalBasicInfo.RemindDate,
                    RemindDateH = outboundExternalDTO.OutboundExternalBasicInfo.RemindDateH,



                };
                return editOutboundInternalVM;
            }
            return new VIPEditOutboundExternalVM();
        }

        internal static EditOutboundDraftDTO Map(VIPEditOutboundExternalVM vipEditOutboundExternalVM)
        {
            if (vipEditOutboundExternalVM != null)
            {
                EditOutboundDraftDTO EditOutboundInternalDTO = new EditOutboundDraftDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(vipEditOutboundExternalVM.Attachments),
                    DocumentDTO = DocumentMapper.Map(vipEditOutboundExternalVM.DocumentVM),
                    HijriRecordDate = vipEditOutboundExternalVM.HijriRecordDate,
                    Id = vipEditOutboundExternalVM.Id,
                    IsSigned = vipEditOutboundExternalVM.IsSigned,
                    Links = TransactionLinkMapper.Map(vipEditOutboundExternalVM.Links),
                    Names = TransactionNameMapper.Map(vipEditOutboundExternalVM.Names),
                    OrgUnitId = vipEditOutboundExternalVM.OrgUnitId,
                    OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoDTO
                    {
                        ConfidentialityLevelId = vipEditOutboundExternalVM.OutboundExternalBasicInfoEdit.ConfidentialityLevelId,
                        PriorityLevelId = vipEditOutboundExternalVM.OutboundExternalBasicInfoEdit.PriorityLevelId,
                        RemindDate = vipEditOutboundExternalVM.RemindDate,
                        RemindDateH = vipEditOutboundExternalVM.RemindDateH,
                        Hour = vipEditOutboundExternalVM.OutboundExternalBasicInfoEdit.Hour,
                        Minute = vipEditOutboundExternalVM.OutboundExternalBasicInfoEdit.Minute
                    },
                    RecordDate = vipEditOutboundExternalVM.RecordDate,
                    UserId = vipEditOutboundExternalVM.UserId,
                    Copies = TransactionCopyMapper.Map(vipEditOutboundExternalVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(vipEditOutboundExternalVM.ExternalCopies),


                };
                return EditOutboundInternalDTO;
            }
            return new EditOutboundDraftDTO();
        }


        internal static EditOutboundDraftVM Map(EditOutboundDraftDTO editOutboundDraftDTO)
        {
            if (editOutboundDraftDTO != null)
            {
                EditOutboundDraftVM editOutboundDraftVM = new EditOutboundDraftVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(editOutboundDraftDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(editOutboundDraftDTO.DocumentDTO),
                    HijriRecordDate = editOutboundDraftDTO.HijriRecordDate,
                    Id = editOutboundDraftDTO.Id,
                    IsSigned = editOutboundDraftDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundDraftDTO.Links),
                    Names = TransactionNameMapper.Map(editOutboundDraftDTO.Names),
                    OrgUnitId = editOutboundDraftDTO.OrgUnitId,
                    OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoVM
                    {
                        ConfidentialityLevelId = editOutboundDraftDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                        PriorityLevelId = editOutboundDraftDTO.OutboundDraftBasicInfo.PriorityLevelId,
                        RemindDate = editOutboundDraftDTO.OutboundDraftBasicInfo.RemindDate,
                        RemindDateH = editOutboundDraftDTO.OutboundDraftBasicInfo.RemindDateH,
                        Hour = editOutboundDraftDTO.OutboundDraftBasicInfo.Hour,
                        Minute = editOutboundDraftDTO.OutboundDraftBasicInfo.Minute,
                        ConfidentialityLevelText = editOutboundDraftDTO.OutboundDraftBasicInfo.ConfidentialityLevelText
                    },
                    RecordDate = editOutboundDraftDTO.RecordDate,
                    UserId = editOutboundDraftDTO.UserId,
                    Copies = TransactionCopyMapper.Map(editOutboundDraftDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundDraftDTO.ExternalCopies),


                };
                return editOutboundDraftVM;
            }
            return new EditOutboundDraftVM();
        }
    }
}