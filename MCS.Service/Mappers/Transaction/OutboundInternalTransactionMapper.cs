using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class OutboundInternalTransactionMapper
    {
        public static Transaction Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null && transactionDTO.Id > 0)
            {
                return MapEditOutboundInternalTransaction(transactionDTO);
            }

            return MapNewOutboundInternalTransaction(transactionDTO);
        }

        private static Transaction MapNewOutboundInternalTransaction(TransactionDTO transactionDTO)
        {
            AddOutboundInternalDTO outboundInternalAddDTO = (AddOutboundInternalDTO)transactionDTO;

            Transaction transaction = new Transaction()
            {
                Attachments = TransactionAttachmentMapper.Map(outboundInternalAddDTO.Attachments),
                Links = TransactionLinkMapper.Map(outboundInternalAddDTO.Links, transactionDTO.TransactionCategory),
                Copies = TransactionCopyMapper.Map(outboundInternalAddDTO.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(outboundInternalAddDTO.ExternalCopies),
                Subject = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Subject,
                Remarks = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Remarks,
                PriorityId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.PriorityLevelId,
                LetterTypeId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.LetterTypeId,
                ConfidentialityId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.ConfidentialityLevelId,
                TransactionTypeId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.TransactionTypeId,
                TransactionCategoryId = transactionDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                MainDocument = DocumentMapper.Map(outboundInternalAddDTO.DocumentDTO),
                OrgUnitId = outboundInternalAddDTO.OrgUnitId,
                SuggestedTopicId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.SuggestedTopicId,
                GroupId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.GroupId,
                DeliveryMethodId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.DeliveryMethodId,
                ToUserId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.DirectedToId,
                EntityId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId,
                ReporterId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.ReporterId,
                SubjectClassificationsId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.SubjectClassificationsId,
                RecordNumber = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.RecordNumber,
                PrivecyId = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.privacyLevelId,
                LetterNumber = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.LetterNumber,
                IsElcOutBound = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.IsElcOutBound,
                NeedAcknowled = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.NeedAcknowled,
                OutBoundDraftNumber = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.OutBoundDraftNumber,
                Summary = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Summary,
                Encrypted = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Encrypted,
            };



            if (outboundInternalAddDTO.OutboundInternalBasicInfoAdd.SubjectClassifications != null
                 && outboundInternalAddDTO.OutboundInternalBasicInfoAdd.SubjectClassifications.Count > 0)
            {
                transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                outboundInternalAddDTO.OutboundInternalBasicInfoAdd.SubjectClassifications.ForEach(s =>
                    transaction.SubjectClassifications
                    .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
            }

            if (outboundInternalAddDTO.OutboundInternalBasicInfoAdd.RemindDate.HasValue)
            {
                TimeSpan ts = new TimeSpan(outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Hour.HasValue ? outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Hour.Value : 00, outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Minute.HasValue ? outboundInternalAddDTO.OutboundInternalBasicInfoAdd.Minute.Value : 00, 00);
                transaction.RemindDate = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.RemindDate.Value + ts;
                transaction.RemindDateH = outboundInternalAddDTO.OutboundInternalBasicInfoAdd.RemindDateH;
            }

            if (outboundInternalAddDTO.Names != null && outboundInternalAddDTO.Names.Count > 0)
            {
                transaction.Names = new List<TransactionName>();

                foreach (TransactionNameDTO transactionNameDTO in outboundInternalAddDTO.Names)
                {
                    TransactionName transactionName = new TransactionName();

                    transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                    transaction.Names.Add(transactionName);
                }
            }

            return transaction;
        }

        private static Transaction MapEditOutboundInternalTransaction(TransactionDTO transactionDTO)
        {

            if (transactionDTO != null)
            {
                EditOutboundInternalDTO outboundInternalEditDTO = (EditOutboundInternalDTO)transactionDTO;

                Transaction transaction = new Transaction()
                {
                    Id = outboundInternalEditDTO.Id,
                    Date = outboundInternalEditDTO.RecordDate,
                    DateH = outboundInternalEditDTO.HijriRecordDate,
                    Links = TransactionLinkMapper.Map(outboundInternalEditDTO.Links, transactionDTO.TransactionCategory),
                    Number = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Number,
                    Subject = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Subject,
                    Remarks = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Remarks,
                    PriorityId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.PriorityLevelId,
                    LetterTypeId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.LetterTypeId,
                    ConfidentialityId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.ConfidentialityLevelId,
                    TransactionTypeId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.TransactionTypeId,
                    TransactionCategoryId = transactionDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                    Attachments = TransactionAttachmentMapper.Map(outboundInternalEditDTO.Attachments),
                    MainDocument = DocumentMapper.Map(outboundInternalEditDTO.DocumentDTO),
                    OrgUnitId = outboundInternalEditDTO.OrgUnitId,
                    StatusId = outboundInternalEditDTO.StatusId,
                    SuggestedTopicId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SuggestedTopicId,
                    GroupId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.GroupId,
                    Copies = TransactionCopyMapper.Map(outboundInternalEditDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(outboundInternalEditDTO.ExternalCopies),
                    DeliveryMethodId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.DeliveryMethodId,
                    ToUserId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.DirectedToId,
                    EntityId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId,
                    ReporterId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.ReporterId,
                    ProcessPeriodTransaction = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.ProcessPeriodTransaction,
                    SubjectClassificationsId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassificationsId,
                    RecordNumber = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RecordNumber,
                    PrivecyId = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.privacyLevelId,
                    LetterNumber = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.LetterNumber,
                    IsElcOutBound = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.IsElcOutBound,
                    NeedAcknowled = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.NeedAcknowled,
                    OutBoundDraftNumber = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.OutBoundDraftNumber,
                    Summary = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Summary,
                    Encrypted = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Encrypted,
                    
                };

                if (outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassifications != null
                     && outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassifications.ForEach(s =>
                        transaction.SubjectClassifications
                        .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
                }

                if (outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RemindDate.HasValue)
                {
                    TimeSpan ts = new TimeSpan(outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Hour.HasValue ? outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Hour.Value : 00, outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Minute.HasValue ? outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Minute.Value : 00, 00);

                    transaction.RemindDate = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RemindDate.Value + ts;
                    transaction.RemindDateH = outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RemindDateH;
                }

                if (outboundInternalEditDTO.Names != null && outboundInternalEditDTO.Names.Count > 0)
                {
                    transaction.Names = new List<TransactionName>();

                    foreach (TransactionNameDTO transactionNameDTO in outboundInternalEditDTO.Names)
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
        public static TransactionDTO Map_VIP(Transaction transaction)
        {
            if (transaction != null)
            {
                EditOutboundInternalDTO inboundEditDTO = new EditOutboundInternalDTO()
                {
                    OutboundInternalBasicInfoEdit = new EditOutboundInternalBasicInfoDTO
                    {
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        Number = transaction.Number,
                        TransactionTypeId = transaction.TransactionTypeId.Value,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        PriorityLeveText = transaction.Priority != null ? transaction.Priority.Text : "",
                        LetterTypeId = transaction.LetterTypeId.Value,
                        PriorityLevelId = transaction.PriorityId,
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        IsElcOutBound = transaction.IsElcOutBound,
                        ProcessPeriodTransaction = transaction.ProcessPeriodTransaction ?? 0,
                        CreatedDateH = transaction.DateH,
                        RemindDateH = transaction.RemindDateH,

                    },
                };

                if (transaction.ToUser != null)
                {
                    inboundEditDTO.OutboundInternalBasicInfoEdit.DirectedToId = transaction.ToUser.Id;
                }

                inboundEditDTO.SavedTransactionAssignment = transaction?.SavedTransactionAssignments?.FirstOrDefault()?.AssignmentList;
                inboundEditDTO.Id = transaction.Id;
                inboundEditDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
                inboundEditDTO.HijriRecordDate = transaction.DateH;
                inboundEditDTO.RecordDate = transaction.Date;
                inboundEditDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                inboundEditDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                inboundEditDTO.StatusId = transaction.StatusId;
                inboundEditDTO.Names = new List<TransactionNameDTO>();
                inboundEditDTO.FollowUps = TransactionFollowUpMapper.Map(transaction.FollowUp);
                inboundEditDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                inboundEditDTO.FromOrgunitName = transaction.Assignments[0].FromEntity.LocalName;
                inboundEditDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                inboundEditDTO.UserId = transaction.UserId;
                inboundEditDTO.OutboundInternalBasicInfoEdit.SubjectClassificationsId = transaction.SubjectClassificationsId;
                inboundEditDTO.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;
                inboundEditDTO.OutboundInternalBasicInfoEdit.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;

                return inboundEditDTO;
            }
            return null;
        }

        public static TransactionDTO Map(Transaction transaction)
        {
            if (transaction != null)
            {
                EditOutboundInternalDTO outboundInternalEditDTO = new EditOutboundInternalDTO()
                {
                    OutboundInternalBasicInfoEdit = new EditOutboundInternalBasicInfoDTO()
                    {
                        LetterTypeId = transaction.LetterType != null ? transaction.LetterType.Id : 0,
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        PriorityLevelId = transaction.Priority.Id,
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        Number = transaction.Number,
                        TransactionTypeId = transaction.TransactionType.Id,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        DirectedToId = transaction.ToUserId,
                        DirectedToOrgUnitId = transaction.EntityId.HasValue ? transaction.EntityId.Value : 0,
                        ReporterId = transaction.ReporterId,
                        SubjectClassificationsId = transaction.SubjectClassificationsId,
                        RecordNumber = transaction.RecordNumber,
                        CreatedDateH = transaction.DateH,
                        EntityName = transaction?.Entity?.LocalName,
                        LetterNumber = transaction.LetterNumber,
                        IsElcOutBound = transaction.IsElcOutBound,
                        NeedAcknowled = transaction.NeedAcknowled,
                        OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                        Summary = transaction.Summary,
                        Encrypted = transaction.Encrypted,

                    },
                };
                if (transaction.SavedTransactionAssignments != null && transaction.SavedTransactionAssignments.Count > 0 && !string.IsNullOrWhiteSpace(transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList))
                {
                    outboundInternalEditDTO.SavedTransactionAssignment = transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList;
                }
                outboundInternalEditDTO.Id = transaction.Id;
                outboundInternalEditDTO.HijriRecordDate = transaction.DateH;
                outboundInternalEditDTO.RecordDate = transaction.Date;
                outboundInternalEditDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                outboundInternalEditDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                outboundInternalEditDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
                outboundInternalEditDTO.StatusId = transaction.Status.Id;
                outboundInternalEditDTO.Names = new List<TransactionNameDTO>();
                outboundInternalEditDTO.Copies = TransactionCopyMapper.Map(transaction.Copies);
                outboundInternalEditDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transaction.ExternalCopies);
                outboundInternalEditDTO.FollowUps = TransactionFollowUpMapper.Map(transaction.FollowUp);
                outboundInternalEditDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                outboundInternalEditDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                outboundInternalEditDTO.FromOrgunitName = transaction.Assignments[0].FromEntity.LocalName;
                outboundInternalEditDTO.UserId = transaction.UserId;
                if (transaction.SuggestedTopic != null)
                {
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s =>
                        outboundInternalEditDTO.OutboundInternalBasicInfoEdit.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                if (transaction.Names != null && transaction.Names.Count > 0)
                {
                    foreach (TransactionName transactionName in transaction.Names)
                    {
                        outboundInternalEditDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                    }
                }

                if (transaction.RemindDate.HasValue)
                {
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RemindDateH = transaction.RemindDateH;
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.RemindDate = transaction.RemindDate.Value;
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Hour = transaction.RemindDate.Value.Hour;
                    outboundInternalEditDTO.OutboundInternalBasicInfoEdit.Minute = transaction.RemindDate.Value.Minute;

                }
                outboundInternalEditDTO.OutboundInternalBasicInfoEdit.ProcessPeriodTransaction = (int)transaction.ProcessPeriodTransaction;
                return outboundInternalEditDTO;
            }
            return null;
        }

        public static TransactionDTO MapGetPrevious(Transaction transaction)
        {
            if (transaction != null)
            {
                AddOutboundInternalDTO outboundExternalAddDTO = new AddOutboundInternalDTO()
                {
                    OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoDTO()
                    {
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        PriorityLevelId = transaction.Priority.Id,
                        LetterTypeId = transaction.LetterType.Id,
                        TransactionTypeId = transaction.TransactionType.Id,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        ReporterId = transaction.ReporterId
                    },
                };

                if (transaction.ToUser != null)
                {
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.DirectedToId = transaction.ToUser.Id;
                }

                if (transaction.Entity != null)
                {
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = transaction.Entity.Id;
                }


                if (transaction.RemindDate.HasValue)
                {
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.RemindDateH = transaction.RemindDateH;
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.RemindDate = transaction.RemindDate.Value;
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.Hour = transaction.RemindDate.Value.Hour;
                    outboundExternalAddDTO.OutboundInternalBasicInfoAdd.Minute = transaction.RemindDate.Value.Minute;
                }

                return outboundExternalAddDTO;
            }
            return null;
        }
    }
}