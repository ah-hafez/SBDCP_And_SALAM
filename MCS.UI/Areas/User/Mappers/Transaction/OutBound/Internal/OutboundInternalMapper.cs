using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;

namespace MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal
{
    public class OutboundInternalMapper
    {
        public static AddOutboundInternalVM Map(AddOutboundInternalDTO addOutboundInternalDTO)
        {
            if (addOutboundInternalDTO != null)
            {
                AddOutboundInternalVM addOutboundInternalVM = new AddOutboundInternalVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(addOutboundInternalDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(addOutboundInternalDTO.DocumentDTO),
                    HijriRecordDate = addOutboundInternalDTO.HijriRecordDate,
                    Id = addOutboundInternalDTO.Id,
                    IsSigned = addOutboundInternalDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(addOutboundInternalDTO.Links),
                    Names = TransactionNameMapper.Map(addOutboundInternalDTO.Names),
                    OrgUnitId = addOutboundInternalDTO.OrgUnitId,
                    OutboundInternalBasicInfoAdd = OutboundInternalBasicInfoMapper.Map(addOutboundInternalDTO.OutboundInternalBasicInfoAdd),
                    RecordDate = addOutboundInternalDTO.RecordDate,
                    StatusId = addOutboundInternalDTO.StatusId,
                    UserId = addOutboundInternalDTO.UserId,
                    Copies = TransactionCopyMapper.Map(addOutboundInternalDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundInternalDTO.ExternalCopies)
                };
                return addOutboundInternalVM;
            }
            return new AddOutboundInternalVM();
        }
        public static AddOutboundInternalDTO Map(AddOutboundInternalVM addOutboundInternalVM)
        {
            if (addOutboundInternalVM != null)
            {
                AddOutboundInternalDTO addOutboundInternalDTO = new AddOutboundInternalDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(addOutboundInternalVM.Attachments),
                    DocumentDTO = DocumentMapper.Map(addOutboundInternalVM.DocumentVM),
                    HijriRecordDate = addOutboundInternalVM.HijriRecordDate,
                    Id = addOutboundInternalVM.Id,
                    IsSigned = addOutboundInternalVM.IsSigned,
                    Links = TransactionLinkMapper.Map(addOutboundInternalVM.Links),
                    Names = TransactionNameMapper.Map(addOutboundInternalVM.Names),
                    OrgUnitId = addOutboundInternalVM.OrgUnitId,
                    OutboundInternalBasicInfoAdd = OutboundInternalBasicInfoMapper.Map(addOutboundInternalVM.OutboundInternalBasicInfoAdd),
                    RecordDate = addOutboundInternalVM.RecordDate,
                    StatusId = addOutboundInternalVM.StatusId,
                    UserId = addOutboundInternalVM.UserId,
                    Copies = TransactionCopyMapper.Map(addOutboundInternalVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundInternalVM.ExternalCopies),
                };
                return addOutboundInternalDTO;
            }
            return new AddOutboundInternalDTO();
        }
        public static EditOutboundInternalVM Map(EditOutboundInternalDTO editOutboundInternalDTO)
        {
            if (editOutboundInternalDTO != null)
            {
                EditOutboundInternalVM editOutboundInternalVM = new EditOutboundInternalVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(editOutboundInternalDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(editOutboundInternalDTO.DocumentDTO),
                    HijriRecordDate = editOutboundInternalDTO.HijriRecordDate,
                    Id = editOutboundInternalDTO.Id,
                    IsSigned = editOutboundInternalDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundInternalDTO.Links),
                    Names = TransactionNameMapper.Map(editOutboundInternalDTO.Names),
                    OrgUnitId = editOutboundInternalDTO.OrgUnitId,
                    OutboundInternalBasicInfoEdit = OutboundInternalBasicInfoMapper.Map(editOutboundInternalDTO.OutboundInternalBasicInfoEdit),
                    RecordDate = editOutboundInternalDTO.RecordDate,
                    StatusId = editOutboundInternalDTO.StatusId,
                    UserId = editOutboundInternalDTO.UserId,
                    Copies = TransactionCopyMapper.Map(editOutboundInternalDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundInternalDTO.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(editOutboundInternalDTO.FollowUps),
                    AssignedFromUser = UserProfileMapper.Map(editOutboundInternalDTO.FromUser),
                    AssignedToUser = UserProfileMapper.Map(editOutboundInternalDTO.ToUser),
                    IsEnableAssignBack = editOutboundInternalDTO.FromUser.Id != editOutboundInternalDTO.ToUser.Id,
                    SavedTransactionAssignment = editOutboundInternalDTO.SavedTransactionAssignment



                };
                return editOutboundInternalVM;
            }
            return new EditOutboundInternalVM();
        }

        public static VIPEditOutboundInternalVM VIPMap(EditOutboundInternalDTO editOutboundInternalDTO)
        {
            if (editOutboundInternalDTO != null)
            {
                VIPEditOutboundInternalVM editOutboundInternalVM = new VIPEditOutboundInternalVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(editOutboundInternalDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(editOutboundInternalDTO.DocumentDTO),
                    HijriRecordDate = editOutboundInternalDTO.HijriRecordDate,
                    Id = editOutboundInternalDTO.Id,
                    IsSigned = editOutboundInternalDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(editOutboundInternalDTO.Links),
                    Names = TransactionNameMapper.Map(editOutboundInternalDTO.Names),
                    OrgUnitId = editOutboundInternalDTO.OrgUnitId,
                    OutboundInternalBasicInfoEdit = OutboundInternalBasicInfoMapper.VIPMap(editOutboundInternalDTO.OutboundInternalBasicInfoEdit),
                    RecordDate = editOutboundInternalDTO.RecordDate,
                    UserId = editOutboundInternalDTO.UserId,
                    Copies = TransactionCopyMapper.Map(editOutboundInternalDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(editOutboundInternalDTO.ExternalCopies),
                    IsEnableAssignBack = editOutboundInternalDTO.FromUser.Id != editOutboundInternalDTO.ToUser.Id,
                    RemindDate = editOutboundInternalDTO.OutboundInternalBasicInfoEdit.RemindDate,
                    RemindDateH = editOutboundInternalDTO.OutboundInternalBasicInfoEdit.RemindDateH,
                    SavedTransactionAssignment = editOutboundInternalDTO.SavedTransactionAssignment


                };
                editOutboundInternalVM.OutboundInternalBasicInfoEdit.EntityName = editOutboundInternalDTO.FromOrgunitName;
                return editOutboundInternalVM;
            }
            return new VIPEditOutboundInternalVM();
        }
        public static EditOutboundInternalDTO Map(EditOutboundInternalVM eddOutboundInternalVM)
        {
            if (eddOutboundInternalVM != null)
            {
                EditOutboundInternalDTO EditOutboundInternalDTO = new EditOutboundInternalDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(eddOutboundInternalVM.Attachments),
                    DocumentDTO = DocumentMapper.Map(eddOutboundInternalVM.DocumentVM),
                    HijriRecordDate = eddOutboundInternalVM.HijriRecordDate,
                    Id = eddOutboundInternalVM.Id,
                    IsSigned = eddOutboundInternalVM.IsSigned,
                    Links = TransactionLinkMapper.Map(eddOutboundInternalVM.Links),
                    Names = TransactionNameMapper.Map(eddOutboundInternalVM.Names),
                    OrgUnitId = eddOutboundInternalVM.OrgUnitId,
                    OutboundInternalBasicInfoEdit = OutboundInternalBasicInfoMapper.Map(eddOutboundInternalVM.OutboundInternalBasicInfoEdit),
                    RecordDate = eddOutboundInternalVM.RecordDate,
                    StatusId = eddOutboundInternalVM.StatusId,
                    UserId = eddOutboundInternalVM.UserId,
                    Copies = TransactionCopyMapper.Map(eddOutboundInternalVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(eddOutboundInternalVM.ExternalCopies),
                    FollowUps = TransactionFollowUpMapper.Map(eddOutboundInternalVM.FollowUps)
                };
                return EditOutboundInternalDTO;
            }
            return new EditOutboundInternalDTO();
        }

        public static EditOutboundInternalDTO Map(VIPEditOutboundInternalVM eddOutboundInternalVM)
        {
            if (eddOutboundInternalVM != null)
            {
                EditOutboundInternalDTO EditOutboundInternalDTO = new EditOutboundInternalDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(eddOutboundInternalVM.Attachments),
                    DocumentDTO = DocumentMapper.Map(eddOutboundInternalVM.DocumentVM),
                    HijriRecordDate = eddOutboundInternalVM.HijriRecordDate,
                    Id = eddOutboundInternalVM.Id,
                    IsSigned = eddOutboundInternalVM.IsSigned,
                    Links = TransactionLinkMapper.Map(eddOutboundInternalVM.Links),
                    Names = TransactionNameMapper.Map(eddOutboundInternalVM.Names),
                    OrgUnitId = eddOutboundInternalVM.OrgUnitId,
                    OutboundInternalBasicInfoEdit = new EditOutboundInternalBasicInfoDTO
                    {
                        ConfidentialityLevelId = eddOutboundInternalVM.OutboundInternalBasicInfoEdit.ConfidentialityLevelId,
                        PriorityLevelId = eddOutboundInternalVM.OutboundInternalBasicInfoEdit.PriorityLevelId,
                        RemindDate = eddOutboundInternalVM.RemindDate,
                        RemindDateH = eddOutboundInternalVM.RemindDateH,
                        Hour = eddOutboundInternalVM.OutboundInternalBasicInfoEdit.Hour,
                        Minute = eddOutboundInternalVM.OutboundInternalBasicInfoEdit.Minute
                    },
                    RecordDate = eddOutboundInternalVM.RecordDate,
                    UserId = eddOutboundInternalVM.UserId,
                    ExternalCopies = TransactionExternalCopyMapper.Map(eddOutboundInternalVM.ExternalCopies),


                };
                return EditOutboundInternalDTO;
            }
            return new EditOutboundInternalDTO();
        }
        public static VipOutboundInternalDto VIPEditMap(VipOutboundInternalUpdateVM editInboundVM)
        {

            VipOutboundInternalDto vipInboundUpdateDto = new VipOutboundInternalDto
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