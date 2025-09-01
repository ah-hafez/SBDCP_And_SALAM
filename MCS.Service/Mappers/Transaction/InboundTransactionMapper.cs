using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class InboundTransactionMapper
    {
        public static Transaction Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null && transactionDTO.Id > 0)
            {
                return MapEditInboundTransaction(transactionDTO);
            }

            return MapNewInboundTransaction(transactionDTO);
        }

        public static TransactionDTO Map(Transaction transaction)
        {
            if (transaction != null)
            {
                EditInboundDTO inboundEditDTO = new EditInboundDTO()
                {
                    InboundBasicInfoEdit = new EditInboundBasicInfoDTO
                    {
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        InboundNumber = transaction.Number,
                        TransactionTypeId = transaction.TransactionType.Id,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        InboundDocumentNumber = transaction.DocumentNumber,
                        LetterTypeId = transaction.LetterType.Id,
                        DestinationId = transaction.ExternalParty?.Id,
                        PriorityLevelId = transaction.Priority.Id,
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        OutboundDraftId = transaction.OutboundDraftId,
                        InboundDateH = transaction.InboundDateH,
                        IsForIndividual = transaction.IsForIndividual,
                        ReporterId = transaction.ReporterId,
                        InboundIntendedPerson = transaction.InboundIntendedPerson,
                        ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                        SubjectClassificationsId = transaction.SubjectClassificationsId,
                        RecordNumber = transaction.RecordNumber,
                        SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                        NumberContact = transaction.NumberContact,
                        CreatedDateH = transaction.DateH,
                        EntityName = transaction.Entity.LocalName,
                        ContactDateH = transaction.ContactDateH,
                        LetterNumber = transaction.LetterNumber,
                        CityId = transaction?.City?.Id,
                        CityName = transaction?.City?.Text,
                        Summary = transaction.Summary,
                        Encrypted = transaction.Encrypted,
                        ToUserId = transaction.ToUserId,

                    },
                };

                if (transaction.SavedTransactionAssignments != null && transaction.SavedTransactionAssignments.Count > 0 && !string.IsNullOrWhiteSpace(transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList))
                {
                    inboundEditDTO.SavedTransactionAssignment = transaction.SavedTransactionAssignments.FirstOrDefault().AssignmentList;
                }
                if (transaction.ToUser != null)
                {
                    inboundEditDTO.InboundBasicInfoEdit.DirectedToId = transaction.ToUser.Id;
                }

                if (transaction.Entity != null)
                {
                    inboundEditDTO.InboundBasicInfoEdit.DirectedToOrgUnitId = transaction.Entity.Id;
                }

                if (transaction.ExternalPartyManager != null)
                {
                    inboundEditDTO.InboundBasicInfoEdit.SignedById = transaction.ExternalPartyManager.Id;
                }

                if (transaction.SuggestedTopic != null)
                {
                    inboundEditDTO.InboundBasicInfoEdit.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    inboundEditDTO.InboundBasicInfoEdit.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => inboundEditDTO.InboundBasicInfoEdit.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                if (transaction.RemindDate.HasValue)
                {
                    inboundEditDTO.InboundBasicInfoEdit.RemindDateH = transaction.RemindDateH;
                    inboundEditDTO.InboundBasicInfoEdit.RemindDate = transaction.RemindDate.Value;
                    inboundEditDTO.InboundBasicInfoEdit.Hour = transaction.RemindDate.Value.Hour;
                    inboundEditDTO.InboundBasicInfoEdit.Minute = transaction.RemindDate.Value.Minute;
                }

                inboundEditDTO.Id = transaction.Id;
                inboundEditDTO.DocumentDTO = DocumentMapper.MapWithContent(transaction.MainDocument);
                inboundEditDTO.HijriRecordDate = transaction.DateH;
                inboundEditDTO.RecordDate = transaction.Date;
                inboundEditDTO.Links = TransactionLinkMapper.Map(transaction.Links);
                inboundEditDTO.Attachments = TransactionAttachmentMapper.Map(transaction.Attachments);
                inboundEditDTO.StatusId = transaction.Status.Id;
                inboundEditDTO.Names = new List<TransactionNameDTO>();
                inboundEditDTO.Copies = TransactionCopyMapper.Map(transaction.Copies);
                inboundEditDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transaction.ExternalCopies);
                inboundEditDTO.FollowUps = TransactionFollowUpMapper.Map(transaction.FollowUp);
                inboundEditDTO.FromUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].FromUser);
                inboundEditDTO.FromOrgunitName = transaction.Assignments[0].FromEntity.LocalName;
                inboundEditDTO.ToUser = UserProfileMapper.MapUserProfile(transaction.Assignments[0].ToUser);
                inboundEditDTO.UserId = transaction.UserId;
                inboundEditDTO.InboundBasicInfoEdit.SubjectClassificationsId = transaction.SubjectClassificationsId;
                if (transaction.Names != null && transaction.Names.Count > 0)
                {
                    foreach (TransactionName transactionName in transaction.Names)
                    {
                        inboundEditDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                    }
                }
                inboundEditDTO.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;
                inboundEditDTO.InboundBasicInfoEdit.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;

                return inboundEditDTO;
            }
            return null;
        }

        public static TransactionDTO Map_VIP(Transaction transaction)
        {
            if (transaction != null)
            {
                EditInboundDTO inboundEditDTO = new EditInboundDTO()
                {
                    InboundBasicInfoEdit = new EditInboundBasicInfoDTO
                    {
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        InboundNumber = transaction.Number,
                        TransactionTypeId = transaction.TransactionTypeId.Value,
                        ConfidentialityLevelText = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                        PriorityLevelText = transaction.Priority != null ? transaction.Priority.Text : "",
                        InboundDocumentNumber = transaction.DocumentNumber,
                        LetterTypeId = transaction.LetterTypeId.Value,
                        DestinationId = transaction.ExternalPartyId,
                        PriorityLevelId = transaction.PriorityId,
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        InboundDateH = transaction.InboundDateH,
                        IsForIndividual = transaction.IsForIndividual,
                        ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                        CreatedDateH = transaction.DateH,
                        RemindDateH = transaction.RemindDateH,
                        
                    },
                };

                if (transaction.ToUser != null)
                {
                    inboundEditDTO.InboundBasicInfoEdit.DirectedToId = transaction.ToUser.Id;
                }


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
                inboundEditDTO.InboundBasicInfoEdit.SubjectClassificationsId = transaction.SubjectClassificationsId;
                inboundEditDTO.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;
                inboundEditDTO.InboundBasicInfoEdit.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.HasValue ? (int)transaction.ProcessPeriodTransaction : 0;
                inboundEditDTO.SavedTransactionAssignment = transaction?.SavedTransactionAssignments?.FirstOrDefault()?.AssignmentList;
                return inboundEditDTO;
            }
            return null;
        }
        public static TransactionDTO MapLight(Transaction transaction)
        {
            if (transaction != null)
            {
                EditInboundDTO inboundEditDTO = new EditInboundDTO()
                {
                    InboundBasicInfoEdit = new EditInboundBasicInfoDTO
                    {
                        Viewed = transaction.Assignments.LastOrDefault().Viewed,
                        DeliveryMethodId = transaction.Assignments.FirstOrDefault().DeliveryMethodId,
                    },
                };
                return inboundEditDTO;
            }
            return null;
        }

        public static TransactionDTO MapGetPrevious(Transaction transaction)
        {
            if (transaction != null)
            {
                AddInboundDTO AddInboundDTO = new AddInboundDTO()
                {
                    InboundBasicInfo = new AddInboundBasicInfoDTO
                    {
                        ConfidentialityLevelId = transaction.Confidentiality.Id,
                        TransactionTypeId = transaction.TransactionType.Id,
                        LetterTypeId = transaction.LetterType.Id,
                        PriorityLevelId = transaction.Priority.Id,
                        Remarks = transaction.Remarks,
                        Subject = transaction.Subject,
                        DeliveryMethodId = transaction.DeliveryMethodId,
                        InboundDateH = transaction.InboundDateH,
                        IsForIndividual = transaction.IsForIndividual,
                        ReporterId = transaction.ReporterId,
                        InboundIntendedPerson = transaction.InboundIntendedPerson,
                        SideContactExternalEntityID = transaction.SideContactExternalEntityID
                    },
                };

                if (transaction.ToUser != null)
                {
                    AddInboundDTO.InboundBasicInfo.DirectedToId = transaction.ToUser.Id;
                }

                if (transaction.Entity != null)
                {
                    AddInboundDTO.InboundBasicInfo.DirectedToOrgUnitId = transaction.Entity.Id;
                }

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    AddInboundDTO.InboundBasicInfo.SubjectClassifications = new List<int>();
                    transaction.SubjectClassifications.ToList().ForEach(s => AddInboundDTO.InboundBasicInfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                if (transaction.SuggestedTopic != null)
                {
                    AddInboundDTO.InboundBasicInfo.SuggestedTopicId = transaction.SuggestedTopic.Id;
                }

                if (transaction.RemindDate.HasValue)
                {
                    AddInboundDTO.InboundBasicInfo.RemindDateH = transaction.RemindDateH;
                    AddInboundDTO.InboundBasicInfo.RemindDate = transaction.RemindDate.Value;
                    AddInboundDTO.InboundBasicInfo.Hour = transaction.RemindDate.Value.Hour;
                    AddInboundDTO.InboundBasicInfo.Minute = transaction.RemindDate.Value.Minute;
                }

                if (transaction.ExternalParty != null)
                {
                    AddInboundDTO.InboundBasicInfo.DestinationId = transaction.ExternalParty.Id;
                }
                if (transaction.SideContactExternalEntityID != null)
                {
                    AddInboundDTO.InboundBasicInfo.SideContactExternalEntityID = transaction.SideContactExternalEntityID;
                }
                if (transaction.ExternalPartyManager != null)
                {
                    AddInboundDTO.InboundBasicInfo.SignedById = transaction.ExternalPartyManager.Id;
                }
                AddInboundDTO.InboundBasicInfo.DestinationId = transaction.ExternalPartyId;
                return AddInboundDTO;
            }
            return null;

        }

        private static Transaction MapNewInboundTransaction(TransactionDTO transactionDTO)
        {

            AddInboundDTO inboundTransactionDTO = (AddInboundDTO)transactionDTO;

            Transaction transaction = new Transaction()
            {
                Attachments = TransactionAttachmentMapper.Map(inboundTransactionDTO.Attachments),
                Links = TransactionLinkMapper.Map(inboundTransactionDTO.Links, transactionDTO.TransactionCategory),
                Subject = inboundTransactionDTO.InboundBasicInfo.Subject,
                Remarks = inboundTransactionDTO.InboundBasicInfo.Remarks,
                PriorityId = inboundTransactionDTO.InboundBasicInfo.PriorityLevelId,
                LetterTypeId = inboundTransactionDTO.InboundBasicInfo.LetterTypeId,
                ConfidentialityId = inboundTransactionDTO.InboundBasicInfo.ConfidentialityLevelId,
                ToUserId = inboundTransactionDTO.InboundBasicInfo.DirectedToId,
                EntityId = inboundTransactionDTO.InboundBasicInfo.DirectedToOrgUnitId,
                TransactionTypeId = inboundTransactionDTO.InboundBasicInfo.TransactionTypeId,
                OrgUnitId = inboundTransactionDTO.OrgUnitId,
                TransactionCategoryId = transactionDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                DocumentNumber = inboundTransactionDTO.InboundBasicInfo.InboundDocumentNumber,
                ExternalPartyId = inboundTransactionDTO.InboundBasicInfo.DestinationId,
                SuggestedTopicId = inboundTransactionDTO.InboundBasicInfo.SuggestedTopicId,
                MainDocument = DocumentMapper.Map(inboundTransactionDTO.DocumentDTO),
                Copies = TransactionCopyMapper.Map(inboundTransactionDTO.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(inboundTransactionDTO.ExternalCopies),
                DeliveryMethodId = inboundTransactionDTO.InboundBasicInfo.DeliveryMethodId,
                InboundDateH = inboundTransactionDTO.InboundBasicInfo.InboundDateH,
                IsForIndividual = inboundTransactionDTO.InboundBasicInfo.IsForIndividual,
                ReporterId = inboundTransactionDTO.InboundBasicInfo.ReporterId,
                InboundIntendedPerson = inboundTransactionDTO.InboundBasicInfo.InboundIntendedPerson,
                ProcessPeriodTransaction = inboundTransactionDTO.ProcessPeriodTransaction,
                SubjectClassificationsId = inboundTransactionDTO.InboundBasicInfo.SubjectClassificationsId,
                RecordNumber = inboundTransactionDTO.InboundBasicInfo.RecordNumber,
                SideContactExternalEntityID = inboundTransactionDTO.InboundBasicInfo.SideContactExternalEntityID,
                NumberContact = inboundTransactionDTO.InboundBasicInfo.NumberContact,
                ContactDateH = inboundTransactionDTO.InboundBasicInfo.ContactDateH,
                PrivecyId = inboundTransactionDTO.InboundBasicInfo.privacyLevelId,
                LetterNumber = inboundTransactionDTO.InboundBasicInfo.LetterNumber,
                CityId = inboundTransactionDTO.InboundBasicInfo.CityId,
                Summary = inboundTransactionDTO.InboundBasicInfo.Summary,
                Encrypted = inboundTransactionDTO.InboundBasicInfo.Encrypted,

            };

            if (inboundTransactionDTO.InboundBasicInfo.SubjectClassifications != null
                && inboundTransactionDTO.InboundBasicInfo.SubjectClassifications.Count > 0)
            {
                transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                inboundTransactionDTO.InboundBasicInfo.SubjectClassifications.ForEach(s =>
                    transaction.SubjectClassifications
                    .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
            }

            if (inboundTransactionDTO.InboundBasicInfo.SignedById.HasValue)
            {
                transaction.ExternalPartyManagerId = inboundTransactionDTO.InboundBasicInfo.SignedById.Value;
            }

            if (inboundTransactionDTO.InboundBasicInfo.RemindDate.HasValue)
            {
                TimeSpan ts = new TimeSpan(inboundTransactionDTO.InboundBasicInfo.Hour.HasValue ? inboundTransactionDTO.InboundBasicInfo.Hour.Value : 00, inboundTransactionDTO.InboundBasicInfo.Minute.HasValue ? inboundTransactionDTO.InboundBasicInfo.Minute.Value : 00, 00);
                transaction.RemindDate = inboundTransactionDTO.InboundBasicInfo.RemindDate.Value + ts;
                transaction.RemindDateH = inboundTransactionDTO.InboundBasicInfo.RemindDateH;
            }

            if (inboundTransactionDTO.Names != null && inboundTransactionDTO.Names.Count > 0)
            {
                transaction.Names = new List<TransactionName>();

                foreach (TransactionNameDTO transactionNameDTO in inboundTransactionDTO.Names)
                {
                    TransactionName transactionName = new TransactionName();

                    transactionName.Name = TransactionNameMapper.Map(transactionNameDTO);
                    transaction.Names.Add(transactionName);
                }
            }

            return transaction;

        }

        private static Transaction MapEditInboundTransaction(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null)
            {
                EditInboundDTO inboundEditTransactionDTO = (EditInboundDTO)transactionDTO;

                Transaction transaction = new Transaction()
                {
                    Id = inboundEditTransactionDTO.Id,
                    Date = inboundEditTransactionDTO.RecordDate,
                    DateH = inboundEditTransactionDTO.HijriRecordDate,
                    Number = inboundEditTransactionDTO.InboundBasicInfoEdit.InboundNumber,
                    Links = TransactionLinkMapper.Map(inboundEditTransactionDTO.Links, transactionDTO.TransactionCategory),
                    Subject = inboundEditTransactionDTO.InboundBasicInfoEdit.Subject,
                    Remarks = inboundEditTransactionDTO.InboundBasicInfoEdit.Remarks,
                    PriorityId = inboundEditTransactionDTO.InboundBasicInfoEdit.PriorityLevelId,
                    LetterTypeId = inboundEditTransactionDTO.InboundBasicInfoEdit.LetterTypeId,
                    ConfidentialityId = inboundEditTransactionDTO.InboundBasicInfoEdit.ConfidentialityLevelId,
                    ToUserId = inboundEditTransactionDTO.InboundBasicInfoEdit.DirectedToId,
                    EntityId = inboundEditTransactionDTO.InboundBasicInfoEdit.DirectedToOrgUnitId,
                    TransactionTypeId = inboundEditTransactionDTO.InboundBasicInfoEdit.TransactionTypeId,
                    DocumentNumber = inboundEditTransactionDTO.InboundBasicInfoEdit.InboundDocumentNumber,
                    ModefiedBy = inboundEditTransactionDTO.ModifiedByUserId,
                    OrgUnitId = inboundEditTransactionDTO.OrgUnitId,
                    TransactionCategoryId = transactionDTO.TransactionCategory.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                    Attachments = TransactionAttachmentMapper.Map(inboundEditTransactionDTO.Attachments),
                    ExternalPartyId = inboundEditTransactionDTO.InboundBasicInfoEdit.DestinationId,
                    SuggestedTopicId = inboundEditTransactionDTO.InboundBasicInfoEdit.SuggestedTopicId,
                    MainDocument = DocumentMapper.Map(inboundEditTransactionDTO.DocumentDTO),
                    StatusId = inboundEditTransactionDTO.StatusId,
                    Copies = TransactionCopyMapper.Map(inboundEditTransactionDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(inboundEditTransactionDTO.ExternalCopies),
                    DeliveryMethodId = inboundEditTransactionDTO.InboundBasicInfoEdit.DeliveryMethodId,
                    InboundDateH = inboundEditTransactionDTO.InboundBasicInfoEdit.InboundDateH,
                    IsForIndividual = inboundEditTransactionDTO.InboundBasicInfoEdit.IsForIndividual,
                    ReporterId = inboundEditTransactionDTO.InboundBasicInfoEdit.ReporterId,
                    InboundIntendedPerson = inboundEditTransactionDTO.InboundBasicInfoEdit.InboundIntendedPerson,
                    FollowUp = TransactionFollowUpMapper.Map(inboundEditTransactionDTO.FollowUps),
                    ProcessPeriodTransaction = inboundEditTransactionDTO.ProcessPeriodTransaction,
                    SubjectClassificationsId = inboundEditTransactionDTO.InboundBasicInfoEdit.SubjectClassificationsId,
                    NumberContact = inboundEditTransactionDTO.InboundBasicInfoEdit.NumberContact,
                    SideContactExternalEntityID = inboundEditTransactionDTO.InboundBasicInfoEdit.SideContactExternalEntityID,
                    ContactDateH = inboundEditTransactionDTO.InboundBasicInfoEdit.ContactDateH,
                    PrivecyId = inboundEditTransactionDTO.InboundBasicInfoEdit.privacyLevelId,
                    LetterNumber = inboundEditTransactionDTO.InboundBasicInfoEdit.LetterNumber,
                    CityId = inboundEditTransactionDTO.InboundBasicInfoEdit.CityId,
                    Summary = inboundEditTransactionDTO.InboundBasicInfoEdit.Summary,
                    Encrypted = inboundEditTransactionDTO.InboundBasicInfoEdit.Encrypted,
                };

                if (inboundEditTransactionDTO.InboundBasicInfoEdit.SubjectClassifications != null
                 && inboundEditTransactionDTO.InboundBasicInfoEdit.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications = new List<TransactionSubjectClassification>();

                    inboundEditTransactionDTO.InboundBasicInfoEdit.SubjectClassifications.ForEach(s =>
                        transaction.SubjectClassifications
                        .Add(new TransactionSubjectClassification { SubjectClassificationId = s }));
                }

                if (inboundEditTransactionDTO.InboundBasicInfoEdit.SignedById.HasValue)
                {
                    transaction.ExternalPartyManagerId = inboundEditTransactionDTO.InboundBasicInfoEdit.SignedById.Value;
                }

                if (inboundEditTransactionDTO.InboundBasicInfoEdit.RemindDate.HasValue)
                {
                    TimeSpan ts = new TimeSpan(inboundEditTransactionDTO.InboundBasicInfoEdit.Hour.HasValue ? inboundEditTransactionDTO.InboundBasicInfoEdit.Hour.Value : 00, inboundEditTransactionDTO.InboundBasicInfoEdit.Minute.HasValue ? inboundEditTransactionDTO.InboundBasicInfoEdit.Minute.Value : 00, 00);

                    transaction.RemindDate = inboundEditTransactionDTO.InboundBasicInfoEdit.RemindDate.Value + ts;
                    transaction.RemindDateH = inboundEditTransactionDTO.InboundBasicInfoEdit.RemindDateH;
                }

                if (inboundEditTransactionDTO.Names != null && inboundEditTransactionDTO.Names.Count > 0)
                {
                    transaction.Names = new List<TransactionName>();

                    foreach (TransactionNameDTO transactionNameDTO in inboundEditTransactionDTO.Names)
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