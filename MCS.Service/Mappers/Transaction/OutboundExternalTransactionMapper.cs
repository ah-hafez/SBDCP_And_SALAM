using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class OutboundExternalTransactionMapper
    {
        public static Transaction Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null && transactionDTO.Id > 0)
            {
                return MapEditOutboundExternalTransaction(transactionDTO);
            }

            return MapNewOutboundExternalTransaction(transactionDTO);
        }

        public static TransactionDTO Map(Transaction transaction)
        {
            if (transaction != null && transaction.Id > 0)
            {
                return MapEditOutboundExternalTransaction(transaction);
            }

            return MapNewOutboundExternalTransaction(transaction);
        }

    

        public static TransactionDTO MapNewOutboundExternalTransaction(Transaction transaction)
        {
            AddOutboundExternalDTO outboundExternalAddDTO = new AddOutboundExternalDTO()
            {
                OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO
                {
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    OutboundNumber = transaction.Number,
                    ConfidentialityLevelId = transaction.Confidentiality.Id,
                    ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                    ExternalPartyId = transaction.ExternalParty.Id,
                    PriorityLevelId = transaction.Priority.Id,
                    SignedById = transaction.SignedByUserId,
                    TransactionTypeId = transaction.TransactionType.Id,
                    PreparationEntityId = transaction.EntityId.HasValue ? transaction.EntityId.Value : 0,
                    DeliveryMethod = transaction.DeliveryMethod.ToString(),
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    POBox = transaction.POBox,
                    PostCode = transaction.PostCode,
                    IsDraft = transaction.IsDraft,
                    ReporterId = transaction.ReporterId,
                    CreatedDateH = transaction.DateH,
                    EntityName = transaction.Entity.LocalName,
                    LetterNumber = transaction.LetterNumber,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    Summary = transaction.Summary,
                    Encrypted = transaction.Encrypted,
                    RemindDateH = transaction.RemindDateH,
                },
            };

            if (transaction.SuggestedTopic != null)
            {
                outboundExternalAddDTO.OutboundExternalBasicInfo.SuggestedTopicId = transaction.SuggestedTopic.Id;
            }

            if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
            {
                outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications = new List<int>();

                transaction.SubjectClassifications.ToList().ForEach(s => outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications.Add(s.SubjectClassification.Id));
            }

            if (transaction.LetterType != null)
            {
                outboundExternalAddDTO.OutboundExternalBasicInfo.LetterTypeId = transaction.LetterType.Id;
            }

            if (transaction.ExternalPartyManager != null)
            {
                outboundExternalAddDTO.OutboundExternalBasicInfo.DirectedToId = transaction.ExternalPartyManager.Id;
            }

            if (transaction.RemindDate.HasValue)
            {
                outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDateH = transaction.RemindDateH;
                outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDate = transaction.RemindDate.Value;
                outboundExternalAddDTO.OutboundExternalBasicInfo.Hour = transaction.RemindDate.Value.Hour;
                outboundExternalAddDTO.OutboundExternalBasicInfo.Minute = transaction.RemindDate.Value.Minute;
            }

            outboundExternalAddDTO.Id = transaction.Id;
            outboundExternalAddDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
            outboundExternalAddDTO.HijriRecordDate = transaction.DateH;
            outboundExternalAddDTO.RecordDate = transaction.Date;
            outboundExternalAddDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
            outboundExternalAddDTO.Links = TransactionLinkMapper.Map(transaction.Links);
            outboundExternalAddDTO.Copies = TransactionCopyMapper.Map(transaction.Copies);
            outboundExternalAddDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transaction.ExternalCopies);
            outboundExternalAddDTO.StatusId = transaction.Status.Id;
            outboundExternalAddDTO.Names = new List<TransactionNameDTO>();
            outboundExternalAddDTO.EditorTypeId = transaction.OutboundDraftEditorType;


            if (transaction.Names != null && transaction.Names.Count > 0)
            {
                foreach (TransactionName transactionName in transaction.Names)
                {
                    outboundExternalAddDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                }
            }

            return outboundExternalAddDTO;
        }

        public static TransactionDTO MapEditOutboundExternalTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                EditOutboundExternalDTO outboundExternalEditDTO = new EditOutboundExternalDTO()
                {
                    OutboundExternalBasicInfo = new EditOutboundExternalBasicInfoDTO
                    {
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        OutboundNumber = transaction.Number,
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        ExternalPartyId = transaction.ExternalParty != null ? transaction.ExternalParty.Id : -1,
                        PriorityLevelId = transaction.Priority.Id,
                        SignedById = transaction.SignedByUserId,
                        TransactionTypeId = transaction.TransactionType.Id,
                        PreparationEntityId = transaction.Entity.Id,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        DeliveryMethod = transaction.DeliveryMethod.Text,
                        IsDraft = transaction.IsDraft,
                        POBox = transaction.POBox,
                        PostCode = transaction.PostCode,
                        LetterTypeId = transaction.LetterTypeId ?? -1,
                        ReporterId = transaction.ReporterId,
                        ProcessPeriodTransaction = (int)transaction.ProcessPeriodTransaction,
                        SubjectClassificationsId = transaction.SubjectClassificationsId,
                        ComplaintNumber = transaction.ComplaintNumber,
                        LetterNumber = transaction.LetterNumber,
                        IsPresentationDraft = transaction.IsPresentationDraft,
                        PresentationDraftNumber = transaction.PresentationDraftNumber,
                        IsElcOutBound = transaction.IsElcOutBound,
                        NeedAcknowled = transaction.NeedAcknowled,
                        OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                        Summary = transaction.Summary,
                        Encrypted = transaction.Encrypted,
                    },
                };

                if (transaction.SavedTransactionAssignments != null && transaction.SavedTransactionAssignments.Count > 0 && !string.IsNullOrWhiteSpace(transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList))
                {
                    outboundExternalEditDTO.SavedTransactionAssignment = transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList;
                }
                if (transaction.SuggestedTopic != null)
                {
                    outboundExternalEditDTO.OutboundExternalBasicInfo.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    outboundExternalEditDTO.OutboundExternalBasicInfo.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => outboundExternalEditDTO.OutboundExternalBasicInfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                if (transaction.LetterType != null)
                {
                    outboundExternalEditDTO.OutboundExternalBasicInfo.LetterTypeId = transaction.LetterType.Id;
                }

                if (transaction.ExternalPartyManager != null)
                {
                    outboundExternalEditDTO.OutboundExternalBasicInfo.DirectedToId = transaction.ExternalPartyManager.Id;
                }

                if (transaction.RemindDate.HasValue)
                {
                    outboundExternalEditDTO.OutboundExternalBasicInfo.RemindDateH = transaction.RemindDateH;
                    outboundExternalEditDTO.OutboundExternalBasicInfo.RemindDate = transaction.RemindDate.Value;
                    outboundExternalEditDTO.OutboundExternalBasicInfo.Hour = transaction.RemindDate.Value.Hour;
                    outboundExternalEditDTO.OutboundExternalBasicInfo.Minute = transaction.RemindDate.Value.Minute;
                }

                outboundExternalEditDTO.Id = transaction.Id;
                outboundExternalEditDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
                outboundExternalEditDTO.HijriRecordDate = transaction.DateH;
                outboundExternalEditDTO.RecordDate = transaction.Date;
                outboundExternalEditDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                outboundExternalEditDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                outboundExternalEditDTO.Copies = TransactionCopyMapper.Map(transaction.Copies);
                outboundExternalEditDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transaction.ExternalCopies);
                outboundExternalEditDTO.StatusId = transaction.Status.Id;
                outboundExternalEditDTO.Names = new List<TransactionNameDTO>();
                outboundExternalEditDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                outboundExternalEditDTO.FromOrgunitName =transaction.Assignments[0].FromEntity.LocalName;
                outboundExternalEditDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                outboundExternalEditDTO.UserId = transaction.UserId;
                if (transaction.Names != null && transaction.Names.Count > 0)
                {
                    foreach (TransactionName transactionName in transaction.Names)
                    {
                        outboundExternalEditDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                    }
                }

                return outboundExternalEditDTO;
            }
            return null;
        }

        public static TransactionDTO MapGetPrevious(Transaction transaction)
        {
            if (transaction != null)
            {
                AddOutboundExternalDTO outboundExternalAddDTO = new AddOutboundExternalDTO()
                {
                    OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO
                    {
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        PreparationEntityId = transaction.Entity != null ? transaction.Entity.Id : -1,
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        PriorityLevelId = transaction.Priority.Id,
                        SignedById = transaction.SignedByUser != null ? transaction.SignedByUser.Id : -1,
                        TransactionTypeId = transaction.TransactionType.Id,
                        DeliveryMethod = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.ToString() : string.Empty,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        POBox = transaction.POBox,
                        PostCode = transaction.PostCode,
                        IsDraft = transaction.IsDraft,
                        IsPresentationDraft = transaction.IsPresentationDraft,
                         LetterTypeId = transaction.LetterTypeId ?? -1,
                        ReporterId = transaction.ReporterId,
                        LetterNumber = transaction.LetterNumber
                    },
                };

                if (transaction.LetterType != null)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.LetterTypeId = transaction.LetterType.Id;
                }

                if (transaction.SuggestedTopic != null)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }
                if (transaction.ExternalParty != null)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.ExternalPartyId = transaction.ExternalParty.Id;
                }
                if (transaction.ExternalPartyManager != null)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.DirectedToId = transaction.ExternalPartyManager.Id;
                }

                if (transaction.RemindDate.HasValue)
                {
                    outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDateH = transaction.RemindDateH;
                    outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDate = transaction.RemindDate.Value;
                    outboundExternalAddDTO.OutboundExternalBasicInfo.Hour = transaction.RemindDate.Value.Hour;
                    outboundExternalAddDTO.OutboundExternalBasicInfo.Minute = transaction.RemindDate.Value.Minute;
                }
                outboundExternalAddDTO.OutboundExternalBasicInfo.ExternalPartyId = transaction.ExternalPartyId.Value;
                return outboundExternalAddDTO;
            }
            return null;
        }

        private static Transaction MapNewOutboundExternalTransaction(TransactionDTO transactionDTO)
        {
            AddOutboundExternalDTO outboundExternalAddDTO = (AddOutboundExternalDTO)transactionDTO;

            Transaction transaction = new Transaction()
            {
                Attachments = TransactionAttachmentMapper.Map(outboundExternalAddDTO.Attachments),
                Links = TransactionLinkMapper.Map(outboundExternalAddDTO.Links, transactionDTO.TransactionCategory),
                OrgUnitId = outboundExternalAddDTO.OrgUnitId,
                Copies = TransactionCopyMapper.Map(outboundExternalAddDTO.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(outboundExternalAddDTO.ExternalCopies),
                Subject = outboundExternalAddDTO.OutboundExternalBasicInfo.Subject,
                Remarks = outboundExternalAddDTO.OutboundExternalBasicInfo.Remarks,
                PriorityId = outboundExternalAddDTO.OutboundExternalBasicInfo.PriorityLevelId,
                ConfidentialityId = outboundExternalAddDTO.OutboundExternalBasicInfo.ConfidentialityLevelId,
                ExternalPartyId = outboundExternalAddDTO.OutboundExternalBasicInfo.ExternalPartyId,
                TransactionCategoryId = outboundExternalAddDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                SignedByUserId = outboundExternalAddDTO.OutboundExternalBasicInfo.SignedById,
                EntityId = outboundExternalAddDTO.OutboundExternalBasicInfo.PreparationEntityId,
                TransactionTypeId = outboundExternalAddDTO.OutboundExternalBasicInfo.TransactionTypeId,
                SuggestedTopicId = outboundExternalAddDTO.OutboundExternalBasicInfo.SuggestedTopicId,
                MainDocument = DocumentMapper.Map(outboundExternalAddDTO.DocumentDTO),
                DateH = outboundExternalAddDTO.HijriRecordDate,
                DeliveryMethodId = outboundExternalAddDTO.OutboundExternalBasicInfo.DeliveryMethodId,
                POBox = outboundExternalAddDTO.OutboundExternalBasicInfo.POBox,
                PostCode = outboundExternalAddDTO.OutboundExternalBasicInfo.PostCode,
                IsDraft = outboundExternalAddDTO.OutboundExternalBasicInfo.IsDraft,
                ReporterId = outboundExternalAddDTO.OutboundExternalBasicInfo.ReporterId,
                StatusId = outboundExternalAddDTO.StatusId,
                SubjectClassificationsId = outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassificationsId,
                ComplaintNumber = outboundExternalAddDTO.OutboundExternalBasicInfo.ComplaintNumber,
                PrivecyId = outboundExternalAddDTO.OutboundExternalBasicInfo.privacyLevelId,
                IsPresentationDraft = outboundExternalAddDTO.OutboundExternalBasicInfo.IsPresentationDraft,
                PresentationDraftNumber = outboundExternalAddDTO.OutboundExternalBasicInfo.PresentationDraftNumber,
                IsElcOutBound = outboundExternalAddDTO.OutboundExternalBasicInfo.IsElcOutBound,
                NeedAcknowled = outboundExternalAddDTO.OutboundExternalBasicInfo.NeedAcknowled,
                OutBoundDraftNumber = outboundExternalAddDTO.OutboundExternalBasicInfo.OutBoundDraftNumber,
                IsMultiExternal = outboundExternalAddDTO.OutboundExternalBasicInfo.IsMultiExternal,
                Summary = outboundExternalAddDTO.OutboundExternalBasicInfo.Summary,
                Encrypted = outboundExternalAddDTO.OutboundExternalBasicInfo.Encrypted,
                ToUserId = outboundExternalAddDTO.OutboundExternalBasicInfo.DirectedToId,


            };

            if (outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications != null
             && outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications.Count > 0)
            {
                transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                outboundExternalAddDTO.OutboundExternalBasicInfo.SubjectClassifications.ForEach(s =>
                    transaction.SubjectClassifications
                    .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
            }

            if (outboundExternalAddDTO.OutboundExternalBasicInfo.DirectedToId.HasValue)
            {
                transaction.ExternalPartyManagerId = outboundExternalAddDTO.OutboundExternalBasicInfo.DirectedToId.Value;
            }

            transaction.LetterTypeId = outboundExternalAddDTO.OutboundExternalBasicInfo.LetterTypeId;

            if (outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDate.HasValue)
            {
                TimeSpan ts = new TimeSpan(outboundExternalAddDTO.OutboundExternalBasicInfo.Hour.HasValue ? outboundExternalAddDTO.OutboundExternalBasicInfo.Hour.Value : 00, outboundExternalAddDTO.OutboundExternalBasicInfo.Minute.HasValue ? outboundExternalAddDTO.OutboundExternalBasicInfo.Minute.Value : 00, 00);
                transaction.RemindDate = outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDate.Value + ts;
                transaction.RemindDateH = outboundExternalAddDTO.OutboundExternalBasicInfo.RemindDateH;
            }

            if (outboundExternalAddDTO.Names != null && outboundExternalAddDTO.Names.Count > 0)
            {
                transaction.Names = new List<TransactionName>();

                foreach (TransactionNameDTO transactionNameDTO in outboundExternalAddDTO.Names)
                {
                    TransactionName transactionName = new TransactionName();

                    transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                    transaction.Names.Add(transactionName);
                }
            }
            transaction.LetterNumber = outboundExternalAddDTO.OutboundExternalBasicInfo.LetterNumber;
            return transaction;
        }

        private static Transaction MapEditOutboundExternalTransaction(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null)
            {
                EditOutboundExternalDTO outboundExternalEditDTO = (EditOutboundExternalDTO)transactionDTO;

                Transaction transaction = new Transaction()
                {
                    Id = outboundExternalEditDTO.Id,
                    Date = outboundExternalEditDTO.RecordDate,
                    DateH = outboundExternalEditDTO.HijriRecordDate,
                    Attachments = TransactionAttachmentMapper.Map(outboundExternalEditDTO.Attachments),
                    Links = TransactionLinkMapper.Map(outboundExternalEditDTO.Links, transactionDTO.TransactionCategory),
                    Copies = TransactionCopyMapper.Map(outboundExternalEditDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundExternalEditDTO.ExternalCopies),
                    OrgUnitId = outboundExternalEditDTO.OrgUnitId,
                    Subject = outboundExternalEditDTO.OutboundExternalBasicInfo.Subject,
                    Remarks = outboundExternalEditDTO.OutboundExternalBasicInfo.Remarks,
                    PriorityId = outboundExternalEditDTO.OutboundExternalBasicInfo.PriorityLevelId,
                    ConfidentialityId = outboundExternalEditDTO.OutboundExternalBasicInfo.ConfidentialityLevelId,
                    ExternalPartyId = outboundExternalEditDTO.OutboundExternalBasicInfo.ExternalPartyId,
                    TransactionCategoryId = outboundExternalEditDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                    SignedByUserId = outboundExternalEditDTO.OutboundExternalBasicInfo.SignedById,
                    EntityId = outboundExternalEditDTO.OutboundExternalBasicInfo.PreparationEntityId,
                    TransactionTypeId = outboundExternalEditDTO.OutboundExternalBasicInfo.TransactionTypeId,
                    SuggestedTopicId = outboundExternalEditDTO.OutboundExternalBasicInfo.SuggestedTopicId,
                    MainDocument = DocumentMapper.Map(outboundExternalEditDTO.DocumentDTO),
                    StatusId = outboundExternalEditDTO.StatusId,
                    DeliveryMethodId = outboundExternalEditDTO.OutboundExternalBasicInfo.DeliveryMethodId,
                    IsDraft = outboundExternalEditDTO.OutboundExternalBasicInfo.IsDraft,
                    PostCode = outboundExternalEditDTO.OutboundExternalBasicInfo.PostCode,
                    POBox = outboundExternalEditDTO.OutboundExternalBasicInfo.POBox,
                    ReporterId = outboundExternalEditDTO.OutboundExternalBasicInfo.ReporterId,
                    ProcessPeriodTransaction = outboundExternalEditDTO.OutboundExternalBasicInfo.ProcessPeriodTransaction,
                    ComplaintNumber = outboundExternalEditDTO.OutboundExternalBasicInfo.ComplaintNumber,
                    PrivecyId = outboundExternalEditDTO.OutboundExternalBasicInfo.privacyLevelId,
                    IsPresentationDraft = outboundExternalEditDTO.OutboundExternalBasicInfo.IsPresentationDraft,
                    PresentationDraftNumber = outboundExternalEditDTO.OutboundExternalBasicInfo.PresentationDraftNumber,
                    IsElcOutBound = outboundExternalEditDTO.OutboundExternalBasicInfo.IsElcOutBound,
                    NeedAcknowled = outboundExternalEditDTO.OutboundExternalBasicInfo.NeedAcknowled,
                    OutBoundDraftNumber = outboundExternalEditDTO.OutboundExternalBasicInfo.OutBoundDraftNumber,
                    Summary = outboundExternalEditDTO.OutboundExternalBasicInfo.Summary,
                    Encrypted = outboundExternalEditDTO.OutboundExternalBasicInfo.Encrypted,
                    ToUserId = outboundExternalEditDTO.OutboundExternalBasicInfo.DirectedToId, 
                };

                if (outboundExternalEditDTO.OutboundExternalBasicInfo.SubjectClassifications != null
                    && outboundExternalEditDTO.OutboundExternalBasicInfo.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                    outboundExternalEditDTO.OutboundExternalBasicInfo.SubjectClassifications.ForEach(s =>
                        transaction.SubjectClassifications
                        .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
                }

                if (outboundExternalEditDTO.OutboundExternalBasicInfo.DirectedToId.HasValue)
                {
                    transaction.ExternalPartyManagerId = outboundExternalEditDTO.OutboundExternalBasicInfo.DirectedToId.Value;
                }

                transaction.LetterTypeId = outboundExternalEditDTO.OutboundExternalBasicInfo.LetterTypeId;

                if (outboundExternalEditDTO.OutboundExternalBasicInfo.RemindDate.HasValue)
                {
                    TimeSpan ts = new TimeSpan(outboundExternalEditDTO.OutboundExternalBasicInfo.Hour.HasValue ? outboundExternalEditDTO.OutboundExternalBasicInfo.Hour.Value : 00, outboundExternalEditDTO.OutboundExternalBasicInfo.Minute.HasValue ? outboundExternalEditDTO.OutboundExternalBasicInfo.Minute.Value : 00, 00);

                    transaction.RemindDate = outboundExternalEditDTO.OutboundExternalBasicInfo.RemindDate.Value + ts;
                    transaction.RemindDateH = outboundExternalEditDTO.OutboundExternalBasicInfo.RemindDateH;
                }

                if (outboundExternalEditDTO.Names != null && outboundExternalEditDTO.Names.Count > 0)
                {
                    transaction.Names = new List<TransactionName>();

                    foreach (TransactionNameDTO transactionNameDTO in outboundExternalEditDTO.Names)
                    {
                        TransactionName transactionName = new TransactionName();

                        transactionName.TransactionId = transaction.Id;
                        transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                        transactionName.NameId = transactionName.Name.Id;
                        transaction.Names.Add(transactionName);
                    }
                }
                transaction.LetterNumber = outboundExternalEditDTO.OutboundExternalBasicInfo.LetterNumber;
                return transaction;
            }
            return null;
        }
    }
}