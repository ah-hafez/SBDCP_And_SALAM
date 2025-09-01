using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class OutboundDraftTransactionMapper
    {
        public static Transaction Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null && transactionDTO.Id > 0)
            {
                return MapEditOutboundDraftTransaction(transactionDTO);
            }

            return MapNewOutboundDraftTransaction(transactionDTO);
        }
        public static TransactionDTO Map(Transaction transaction)
        {
            if (transaction != null)
            {
                EditOutboundDraftDTO outboundDraftEditDTO = new EditOutboundDraftDTO()
                {
                    OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoDTO
                    {
                        DestinationId = transaction.ExternalPartyId.HasValue ? 0 : transaction.EntityId.Value,
                        PriorityLevelId = transaction.PriorityId,
                        ConfidentialityLevelId = transaction.ConfidentialityId,
                        SignedById = transaction.SignedByUserId,
                        Subject = transaction.Subject,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        DraftNumber = transaction.Number,
                        TransactionTypeId = transaction.TransactionTypeId.HasValue ? transaction.TransactionTypeId.Value : 0,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        IsDraft = transaction.IsDraft,
                        CreatedOn = transaction.CreatedOn,
                        DirectedToId = transaction.ExternalPartyManagerId.HasValue ? transaction.ExternalPartyManagerId.Value : 0,
                        LetterTypeId = transaction.TransactionTypeId.HasValue ? transaction.TransactionTypeId.Value : 0,
                        SuggestedTopicId = transaction.SuggestedTopicId,
                        DeliveryMethod = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.Text : null,
                        POBox = transaction.POBox,
                        PostCode = transaction.PostCode,
                        ReporterId = transaction.ReporterId,
                        TransactionPathId = transaction.Assignments.FirstOrDefault().TransactionPathId,
                        PreparationEntityId = transaction.Entity.Id,
                        CreatedDateH = transaction.DateH,
                        EntityName = transaction.Entity.LocalName,
                        Remarks = transaction.Remarks,
                        LetterNumber = transaction.LetterNumber,
                        ExternalPartyId = transaction.ExternalPartyId,
                        IsPresentationDraft = transaction.IsPresentationDraft,
                        PresentationDraftNumber = transaction.PresentationDraftNumber,
                        IsElcOutBound = transaction.IsElcOutBound,
                        NeedAcknowled = transaction.NeedAcknowled,
                        OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                        IsDecisionDraft = transaction.IsDecisionDraft,
                        Summary = transaction.Summary,
                        Encrypted = transaction.Encrypted,
                    },
                };
                if (transaction.SavedTransactionAssignments != null && transaction.SavedTransactionAssignments.Count > 0 && !string.IsNullOrWhiteSpace(transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList))
                {
                    outboundDraftEditDTO.SavedTransactionAssignment = transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList;
                }
                if (transaction.SuggestedTopic != null)
                {
                    outboundDraftEditDTO.OutboundDraftBasicInfo.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                if (transaction.LetterType != null)
                {
                    outboundDraftEditDTO.OutboundDraftBasicInfo.LetterTypeId = transaction.LetterType.Id;
                }

                if (transaction.ExternalPartyManager != null)
                {
                    outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId = transaction.ExternalPartyManager.Id;
                }

                if (transaction.RemindDate.HasValue)
                {
                    outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDateH = transaction.RemindDateH;
                    outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate = transaction.RemindDate.Value;
                    outboundDraftEditDTO.OutboundDraftBasicInfo.Hour = transaction.RemindDate.Value.Hour;
                    outboundDraftEditDTO.OutboundDraftBasicInfo.Minute = transaction.RemindDate.Value.Minute;
                }

                if (transaction.FollowUp != null && transaction.FollowUp.Any())
                {
                    outboundDraftEditDTO.FollowUps = TransactionFollowUpMapper.Map(transaction.FollowUp);
                }

                outboundDraftEditDTO.Id = transaction.Id;
                outboundDraftEditDTO.UserId = transaction.UserId;
                outboundDraftEditDTO.OrgUnitId = transaction.OrgUnitId;
                outboundDraftEditDTO.HijriRecordDate = transaction.DateH;
                outboundDraftEditDTO.RecordDate = transaction.Date;
                outboundDraftEditDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);

                outboundDraftEditDTO.Copies = TransactionCopyMapper.Map(transaction.Copies);
                outboundDraftEditDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                outboundDraftEditDTO.StatusId = transaction.Status.Id;
                outboundDraftEditDTO.FromOrgunitName = transaction.Assignments[0].FromEntity.LocalName;

                if (transaction.OutboundDraftEditorType.HasValue)
                {
                    outboundDraftEditDTO.EditorType = (EditorType)transaction.OutboundDraftEditorType.Value;
                }
                else
                {
                    outboundDraftEditDTO.EditorType = EditorType.Scanning;
                }
                if (transaction.OldWordDocumnt != null)
                {
                    outboundDraftEditDTO.OldDocumentDTO = DocumentMapper.MapWithContent(transaction.OldWordDocumnt);
                }
                outboundDraftEditDTO.IsSigned = transaction.IsSigned;
                outboundDraftEditDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transaction.ExternalCopies);
                outboundDraftEditDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                outboundDraftEditDTO.Names = new List<TransactionNameDTO>();
                outboundDraftEditDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                outboundDraftEditDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                if (transaction.Names != null && transaction.Names.Count > 0)
                {
                    foreach (TransactionName transactionName in transaction.Names)
                    {
                        outboundDraftEditDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                    }
                }

                return outboundDraftEditDTO;
            }
            return null;
        }
        public static TransactionDTO Map_VIP(Transaction transaction)
        {
            if (transaction != null)
            {
                EditOutboundDraftDTO outboundDraftDTO = new EditOutboundDraftDTO()
                {
                    OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoDTO
                    {
                        ConfidentialityLevelId = transaction.ConfidentialityId,
                        DraftNumber = transaction.Number,
                        TransactionTypeId = transaction.TransactionTypeId.Value,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        PriorityLeveText = transaction.Priority != null ? transaction.Priority.Text : "",
                        LetterTypeId = transaction.LetterTypeId.Value,
                        PriorityLevelId = transaction.PriorityId,
                        Subject = transaction.Subject,
                        EntityName = transaction.Entity.LocalName,
                        CreatedDateH = transaction.DateH,
                        IsDecisionDraft = transaction.IsDecisionDraft,
                        RemindDateH = transaction.RemindDateH,
                    },
                };

                if (transaction.ToUser != null)
                {
                    outboundDraftDTO.OutboundDraftBasicInfo.DirectedToId = transaction.ToUser.Id;
                }
                outboundDraftDTO.SavedTransactionAssignment = transaction?.SavedTransactionAssignments?.FirstOrDefault()?.AssignmentList;
                outboundDraftDTO.Id = transaction.Id;
                outboundDraftDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
                outboundDraftDTO.OldDocumentDTO = DocumentMapper.MapWithContent(transaction.OldWordDocumnt);
                outboundDraftDTO.HijriRecordDate = transaction.DateH;
                outboundDraftDTO.RecordDate = transaction.Date;
                outboundDraftDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                outboundDraftDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                outboundDraftDTO.StatusId = transaction.StatusId;
                outboundDraftDTO.FollowUps = TransactionFollowUpMapper.Map(transaction.FollowUp);
                outboundDraftDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                outboundDraftDTO.FromOrgunitName = transaction.Assignments[0].FromEntity.LocalName;
                outboundDraftDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                outboundDraftDTO.UserId = transaction.UserId;
                outboundDraftDTO.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;

                return outboundDraftDTO;
            }


            return null;
        }
        private static Transaction MapNewOutboundDraftTransaction(TransactionDTO transactionDTO)
        {

            AddOutboundDraftDTO outboundDraftAddDTO = (AddOutboundDraftDTO)transactionDTO;

            Transaction transaction = new Transaction()
            {
                Id = outboundDraftAddDTO.Id,
                Attachments = TransactionAttachmentMapper.Map(outboundDraftAddDTO.Attachments),
                Links = TransactionLinkMapper.Map(outboundDraftAddDTO.Links, transactionDTO.TransactionCategory),
                Copies = TransactionCopyMapper.Map(outboundDraftAddDTO.Copies),
                Subject = outboundDraftAddDTO.OutboundDraftBasicInfo.Subject,
                PriorityId = outboundDraftAddDTO.OutboundDraftBasicInfo.PriorityLevelId,
                ConfidentialityId = outboundDraftAddDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                SignedByUserId = outboundDraftAddDTO.OutboundDraftBasicInfo.SignedById,
                TransactionTypeId = outboundDraftAddDTO.OutboundDraftBasicInfo.TransactionTypeId,
                MainDocument = DocumentMapper.Map(outboundDraftAddDTO.DocumentDTO),
                OldWordDocumnt = DocumentMapper.Map(outboundDraftAddDTO.OldDocumentDTO),
                OrgUnitId = outboundDraftAddDTO.OrgUnitId,
                TransactionCategoryId = outboundDraftAddDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                OutboundDraftEditorType = (int)outboundDraftAddDTO.EditorType,
                LetterTypeId = outboundDraftAddDTO.OutboundDraftBasicInfo.LetterTypeId,
                SuggestedTopicId = outboundDraftAddDTO.OutboundDraftBasicInfo.SuggestedTopicId,
                IsSigned = outboundDraftAddDTO.IsSigned,
                ExternalCopies = TransactionExternalCopyMapper.Map(outboundDraftAddDTO.ExternalCopies),
                DeliveryMethodId = outboundDraftAddDTO.OutboundDraftBasicInfo.DeliveryMethodId,
                IsDraft = outboundDraftAddDTO.OutboundDraftBasicInfo.IsDraft,
                POBox = outboundDraftAddDTO.OutboundDraftBasicInfo.POBox,
                PostCode = outboundDraftAddDTO.OutboundDraftBasicInfo.PostCode,
                ReporterId = outboundDraftAddDTO.OutboundDraftBasicInfo.ReporterId,
                TransactionPathId = outboundDraftAddDTO.OutboundDraftBasicInfo.TransactionPathId,
                PrivecyId = outboundDraftAddDTO.OutboundDraftBasicInfo.privacyLevelId,
                LetterNumber = outboundDraftAddDTO.OutboundDraftBasicInfo.LetterNumber,
                IsPresentationDraft = outboundDraftAddDTO.OutboundDraftBasicInfo.IsPresentationDraft,
                PresentationDraftNumber = outboundDraftAddDTO.OutboundDraftBasicInfo.PresentationDraftNumber,
                IsElcOutBound = outboundDraftAddDTO.OutboundDraftBasicInfo.IsElcOutBound,
                NeedAcknowled = outboundDraftAddDTO.OutboundDraftBasicInfo.NeedAcknowled,
                OutBoundDraftNumber = outboundDraftAddDTO.OutboundDraftBasicInfo.OutBoundDraftNumber,
                IsDecisionDraft = outboundDraftAddDTO.OutboundDraftBasicInfo.IsDecisionDraft,
                Summary = outboundDraftAddDTO.OutboundDraftBasicInfo.Summary,
                Encrypted = outboundDraftAddDTO.OutboundDraftBasicInfo.Encrypted,
                ToUserId = outboundDraftAddDTO.OutboundDraftBasicInfo.DirectedToId,

            };

            // handling internal outbound draft
            if (outboundDraftAddDTO.OutboundDraftBasicInfo.isOutboundInternalDraft)
            {
                transaction.ExternalPartyId = null;
                transaction.EntityId = outboundDraftAddDTO.OutboundDraftBasicInfo.DestinationId;
            }
            else
            {
                transaction.ExternalPartyId = outboundDraftAddDTO.OutboundDraftBasicInfo.ExternalPartyId;
                transaction.EntityId = outboundDraftAddDTO.OutboundDraftBasicInfo.PreparationEntityId;
            }

            if (outboundDraftAddDTO.OutboundDraftBasicInfo.SubjectClassifications != null
             && outboundDraftAddDTO.OutboundDraftBasicInfo.SubjectClassifications.Count > 0)
            {
                transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                outboundDraftAddDTO.OutboundDraftBasicInfo.SubjectClassifications.ForEach(s =>
                    transaction.SubjectClassifications
                    .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
            }

            if (outboundDraftAddDTO.OutboundDraftBasicInfo.DirectedToId.HasValue)
            {
                transaction.ExternalPartyManagerId = outboundDraftAddDTO.OutboundDraftBasicInfo.DirectedToId.Value;
            }

            if (outboundDraftAddDTO.OutboundDraftBasicInfo.RemindDate.HasValue)
            {
                TimeSpan ts = new TimeSpan(outboundDraftAddDTO.OutboundDraftBasicInfo.Hour.HasValue ? outboundDraftAddDTO.OutboundDraftBasicInfo.Hour.Value : 00, outboundDraftAddDTO.OutboundDraftBasicInfo.Minute.HasValue ? outboundDraftAddDTO.OutboundDraftBasicInfo.Minute.Value : 00, 00);

                transaction.RemindDate = outboundDraftAddDTO.OutboundDraftBasicInfo.RemindDate.Value + ts;
                transaction.RemindDateH = outboundDraftAddDTO.OutboundDraftBasicInfo.RemindDateH;
            }

            if (outboundDraftAddDTO.Names != null && outboundDraftAddDTO.Names.Count > 0)
            {
                transaction.Names = new List<TransactionName>();

                foreach (TransactionNameDTO transactionNameDTO in outboundDraftAddDTO.Names)
                {
                    TransactionName transactionName = new TransactionName();

                    transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                    transaction.Names.Add(transactionName);
                }
            }
            return transaction;

        }
        private static Transaction MapEditOutboundDraftTransaction(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null)
            {
                EditOutboundDraftDTO outboundDraftEditDTO = (EditOutboundDraftDTO)transactionDTO;

                Transaction transaction = new Transaction()
                {
                    Id = outboundDraftEditDTO.Id,
                    Date = outboundDraftEditDTO.RecordDate,
                    DateH = outboundDraftEditDTO.HijriRecordDate,
                    Attachments = TransactionAttachmentMapper.Map(outboundDraftEditDTO.Attachments),
                    Subject = outboundDraftEditDTO.OutboundDraftBasicInfo.Subject,
                    PriorityId = outboundDraftEditDTO.OutboundDraftBasicInfo.PriorityLevelId,
                    ConfidentialityId = outboundDraftEditDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                    ExternalPartyId = outboundDraftEditDTO.OutboundDraftBasicInfo.ExternalPartyId,
                    SignedByUserId = outboundDraftEditDTO.OutboundDraftBasicInfo.SignedById,
                    EntityId = outboundDraftEditDTO.OutboundDraftBasicInfo.DestinationId,
                    TransactionCategoryId = outboundDraftEditDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                    TransactionTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionTypeId,
                    MainDocument = DocumentMapper.Map(outboundDraftEditDTO.DocumentDTO),
                    OldWordDocumnt = DocumentMapper.Map(outboundDraftEditDTO.OldDocumentDTO),
                    OrgUnitId = outboundDraftEditDTO.OrgUnitId,
                    StatusId = outboundDraftEditDTO.StatusId,
                    OutboundDraftEditorType = (int)outboundDraftEditDTO.EditorType,
                    LetterTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterTypeId,
                    ReporterId = outboundDraftEditDTO.OutboundDraftBasicInfo.ReporterId,
                    SuggestedTopicId = outboundDraftEditDTO.OutboundDraftBasicInfo.SuggestedTopicId,
                    IsSigned = outboundDraftEditDTO.IsSigned,
                    Copies = TransactionCopyMapper.Map(outboundDraftEditDTO.Copies),
                    Links = TransactionLinkMapper.Map(outboundDraftEditDTO.Links, transactionDTO.TransactionCategory),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundDraftEditDTO.ExternalCopies),
                    DeliveryMethodId = outboundDraftEditDTO.OutboundDraftBasicInfo.DeliveryMethodId,
                    IsDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsDraft,
                    PostCode = outboundDraftEditDTO.OutboundDraftBasicInfo.PostCode,
                    POBox = outboundDraftEditDTO.OutboundDraftBasicInfo.POBox,
                    ExternalPartyManagerId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId,
                    OutboundDraftId = outboundDraftEditDTO.Id,
                    FollowUp = TransactionFollowUpMapper.Map(outboundDraftEditDTO.FollowUps),
                    TransactionPathId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionPathId,
                    Remarks = outboundDraftEditDTO.OutboundDraftBasicInfo.Remarks,
                    PrivecyId = outboundDraftEditDTO.OutboundDraftBasicInfo.privacyLevelId,
                    LetterNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterNumber,
                    IsPresentationDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsPresentationDraft,
                    PresentationDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.PresentationDraftNumber,
                    IsElcOutBound = outboundDraftEditDTO.OutboundDraftBasicInfo.IsElcOutBound,
                    NeedAcknowled = outboundDraftEditDTO.OutboundDraftBasicInfo.NeedAcknowled,
                    OutBoundDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.OutBoundDraftNumber,
                    IsDecisionDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsDecisionDraft,
                    Summary = outboundDraftEditDTO.OutboundDraftBasicInfo.Summary,
                    Encrypted = outboundDraftEditDTO.OutboundDraftBasicInfo.Encrypted,
                    ToUserId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId,
                };

                // handling internal outbound draft
                if (outboundDraftEditDTO.OutboundDraftBasicInfo.isOutboundInternalDraft)
                {
                    transaction.ExternalPartyId = null;
                    transaction.EntityId = outboundDraftEditDTO.OutboundDraftBasicInfo.DestinationId;
                }
                else
                {
                    transaction.ExternalPartyId = outboundDraftEditDTO.OutboundDraftBasicInfo.ExternalPartyId;
                    transaction.EntityId = outboundDraftEditDTO.OutboundDraftBasicInfo.PreparationEntityId;
                }

                if (outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications != null
                    && outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                    outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications.ForEach(s =>
                        transaction.SubjectClassifications
                        .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
                }

                if (outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId.HasValue)
                {
                    transaction.ExternalPartyManagerId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId.Value;
                }

                if (outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate.HasValue)
                {
                    TimeSpan ts = new TimeSpan(outboundDraftEditDTO.OutboundDraftBasicInfo.Hour.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.Hour.Value : 00, outboundDraftEditDTO.OutboundDraftBasicInfo.Minute.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.Minute.Value : 00, 00);

                    transaction.RemindDate = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate.Value + ts;
                    transaction.RemindDateH = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDateH;
                }

                if (outboundDraftEditDTO.Names != null && outboundDraftEditDTO.Names.Count > 0)
                {
                    transaction.Names = new List<TransactionName>();

                    foreach (TransactionNameDTO transactionNameDTO in outboundDraftEditDTO.Names)
                    {
                        TransactionName transactionName = new TransactionName();

                        transactionName.TransactionId = transaction.Id;
                        transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                        transactionName.NameId = transactionName.Name.Id;
                        transaction.Names.Add(transactionName);
                    }
                }

                return transaction;

            }
            return null;
        }
    }
}