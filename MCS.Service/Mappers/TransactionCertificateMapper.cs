using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using System.Text;

namespace MCS.Service.Mappers
{
    public class TransactionCertificateMapper
    {
        public static InboundCertificateDTO MapInbound(TransactionCertificateInfo transactionCertificate, string cultureName)
        {
            if (transactionCertificate == null)
            {
                return null;
            }

            InboundCertificateDTO inboundCertificateDTO = new InboundCertificateDTO();
            inboundCertificateDTO.Id = transactionCertificate.Id;
            inboundCertificateDTO.InboundNumber = transactionCertificate.Number;
            inboundCertificateDTO.HijriDate = transactionCertificate.DateH;
            inboundCertificateDTO.Date = transactionCertificate.Date;
            inboundCertificateDTO.InboundDocumentNumber = transactionCertificate.DocumentNumber;
            inboundCertificateDTO.Destination = transactionCertificate.ExternalParty;
            inboundCertificateDTO.ConfidentialityLevel = transactionCertificate.Confidentiality;
            inboundCertificateDTO.CreatedByOrgUnit = transactionCertificate.OrgUnitCreatedBy;
            inboundCertificateDTO.TransactionType = transactionCertificate.TransactionType;
            inboundCertificateDTO.LetterType = transactionCertificate.LetterType;
            inboundCertificateDTO.DirectedTo = transactionCertificate.ToUser;
            inboundCertificateDTO.SignedBy = transactionCertificate.SignedBy;
            inboundCertificateDTO.PriorityLevel = transactionCertificate.Priority;
            inboundCertificateDTO.RemindDateH = transactionCertificate.RemindDateH;
            inboundCertificateDTO.RemindTime = transactionCertificate.RemindTime;
            inboundCertificateDTO.CreatedByUser = transactionCertificate.UserCreatedBy;
            inboundCertificateDTO.Status = transactionCertificate.Status;
            inboundCertificateDTO.Subject = transactionCertificate.Subject;
            inboundCertificateDTO.IsAssignToMoreThanOne = transactionCertificate.IsMultiOwnership;
            inboundCertificateDTO.Links = Map(transactionCertificate.Links, cultureName);
            inboundCertificateDTO.Attachments = TransactionAttachmentMapper.Map(transactionCertificate.Attachments);
            inboundCertificateDTO.Assignments = Map(transactionCertificate.AssignmentsHistory);
            inboundCertificateDTO.Copies = TransactionCopyMapper.Map(transactionCertificate.Copies);
            inboundCertificateDTO.ExternalCopies = TransactionExternalCopyMapper.Map(transactionCertificate.ExternalCopies);
            inboundCertificateDTO.DocumentDTO = DocumentMapper.Map(transactionCertificate.MainDocument);
            inboundCertificateDTO.TransactionCertificateHistory = Map(transactionCertificate.TransactionLog);
            inboundCertificateDTO.InboundIntendedPerson = transactionCertificate.InboundIntendedPerson;
            inboundCertificateDTO.IsForIndividual = transactionCertificate.IsForIndividual;
            inboundCertificateDTO.DeliveryMethod = transactionCertificate.DeliveryMethod;
            inboundCertificateDTO.HasDate = transactionCertificate.HasDate;
            inboundCertificateDTO.Remarks = transactionCertificate.Remarks;
            inboundCertificateDTO.OrgUnit = transactionCertificate.OrgUnitCreatedBy;
            inboundCertificateDTO.ToEntity = transactionCertificate.ToEntity;
            inboundCertificateDTO.ProcessPeriodTransaction = transactionCertificate.ProcessPeriodTransaction;
            inboundCertificateDTO.SideContactExternalEntityName = transactionCertificate.SideContactExternalEntityName;
            inboundCertificateDTO.NumberContact = transactionCertificate.NumberContact;
            inboundCertificateDTO.RecordNumber = transactionCertificate.RecordNumber;
            inboundCertificateDTO.ConfidentialityId = transactionCertificate.ConfidentialityId;
            inboundCertificateDTO.LetterNumber = transactionCertificate.LetterNumber;
            inboundCertificateDTO.Encrypted = transactionCertificate.Encrypted;
            inboundCertificateDTO.ClassificationName = transactionCertificate.ClassificationName;
            inboundCertificateDTO.FileDescription = transactionCertificate.FileDescription;
            inboundCertificateDTO.FileNumber = transactionCertificate.FileNumber;


            if (transactionCertificate.CurrentAssignment != null)
            {
                inboundCertificateDTO.LatestAssignment = Map(transactionCertificate.CurrentAssignment);
            }

            inboundCertificateDTO.Names = new List<TransactionNameDTO>();

            if (transactionCertificate.Names != null && transactionCertificate.Names.Count > 0)
            {
                foreach (TransactionName transactionName in transactionCertificate.Names)
                {
                    inboundCertificateDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                }
            }

            return inboundCertificateDTO;
        }

        public static OutboundCertificateDTO MapOutbound(TransactionCertificateInfo transactionCertificate, string cultureName)
        {
            if (transactionCertificate == null)
            {
                return null;
            }

            OutboundCertificateDTO inboundCertificateDTO = new OutboundCertificateDTO()
            {
                Id = transactionCertificate.Id,
                OutboundNumber = transactionCertificate.Number,
                HijriDate = transactionCertificate.DateH,
                Date = transactionCertificate.Date,
                Destination = transactionCertificate.ExternalParty,
                ConfidentialityLevel = transactionCertificate.Confidentiality,
                CreatedByOrgUnit = transactionCertificate.OrgUnitCreatedBy,
                DirectedTo = transactionCertificate.ToUser,
                SignedBy = transactionCertificate.ToUser,
                TransactionType = transactionCertificate.TransactionType,
                PriorityLevel = transactionCertificate.Priority,
                RemindDateH = transactionCertificate.RemindDateH,
                RemindTime = transactionCertificate.RemindTime,
                CreatedByUser = transactionCertificate.UserCreatedBy,
                Status = transactionCertificate.Status,
                Subject = transactionCertificate.Subject,
                IsAssignToMoreThanOne = transactionCertificate.IsMultiOwnership,
                Links = TransactionCertificateMapper.Map(transactionCertificate.Links, cultureName),
                Attachments = TransactionAttachmentMapper.Map(transactionCertificate.Attachments),
                Copies = TransactionCopyMapper.Map(transactionCertificate.Copies),
                ExternalCopies = TransactionExternalCopyMapper.Map(transactionCertificate.ExternalCopies),
                DocumentDTO = DocumentMapper.Map(transactionCertificate.MainDocument),
                TransactionCertificateHistory = TransactionCertificateMapper.Map(transactionCertificate.TransactionLog),
                Assignments = TransactionCertificateMapper.Map(transactionCertificate.AssignmentsHistory),
                HasDate = transactionCertificate.HasDate,
                Remarks = transactionCertificate.Remarks,
                ToEntity = transactionCertificate.ToEntity,
                ProcessPeriodTransaction = transactionCertificate.ProcessPeriodTransaction,
                SideContactExternalEntityName = transactionCertificate.SideContactExternalEntityName,
                NumberContact = transactionCertificate.NumberContact,
                ClassificationName = transactionCertificate.ClassificationName,
                FileDescription = transactionCertificate.FileDescription,
                FileNumber = transactionCertificate.FileNumber,
            };

            if (transactionCertificate.CurrentAssignment != null)
            {
                inboundCertificateDTO.LatestAssignment = TransactionCertificateMapper.Map(transactionCertificate.CurrentAssignment);
            }

            inboundCertificateDTO.Names = new List<TransactionNameDTO>();

            if (transactionCertificate.Names != null && transactionCertificate.Names.Count > 0)
            {
                foreach (TransactionName transactionName in transactionCertificate.Names)
                {
                    inboundCertificateDTO.Names.Add(TransactionNameMapper.Map(transactionName.Name));
                }
            }

            return inboundCertificateDTO;
        }

        public static TransactionCertificateDTO Map(Transaction transaction, string cultureName)
        {
            if (transaction == null)
            {
                return null;
            }

            TransactionCertificateDTO transactionLinkDTO = new TransactionCertificateDTO()
            {
                Id = transaction.Id,
                Number = transaction.Number,
                TransactionCategory = (TransactionCategory)transaction.TransactionCategory.Id,
                Source = (transaction.TransactionType != null) ? !string.IsNullOrWhiteSpace(transaction.TransactionType?.Text) ? transaction.TransactionType?.Text : transaction.TransactionType?.LocalizationIdentifier?.Localizations?.Where(l => l.Culture.ShortName == cultureName)?.LocalText() : null,
                HijriDate = transaction.DateH,
                Date = transaction.Date,
                Links = TransactionCertificateMapper.Map(transaction.Links, cultureName)
            };

            return transactionLinkDTO;
        }

        public static List<TransactionCertificateLinkDTO> Map(IList<TransactionLink> transactionLinks, string cultureName)
        {
            if (transactionLinks == null || !transactionLinks.Any())
            {
                List<TransactionCertificateLinkDTO> tt = new List<TransactionCertificateLinkDTO>();
                return tt;
            }
            List<TransactionCertificateLinkDTO> transactionLinkDTOs = transactionLinks
                .Select(transactionLink => new TransactionCertificateLinkDTO()
                {
                    Transaction = TransactionCertificateMapper.Map(transactionLink.ToTransaction, cultureName),
                    LinkTypeId = transactionLink.Type.Id,
                    LinkTypeName = transactionLink.Type.Text,
                    TransactionNumber = transactionLink.ToTransaction.Number,
                }).ToList();
            return transactionLinkDTOs;
        }
        public static List<TransactionCertificateLinkDTO> MapForCertificate(IList<TransactionLink> transactionLinks, int TransactionId, string cultureName)
        {
            if (transactionLinks == null || !transactionLinks.Any())
            {
                return null;
            }
            List<TransactionCertificateLinkDTO> transactionLinkDTOs = transactionLinks
                .Select(transactionLink =>
                {
                    TransactionCertificateLinkDTO transactionCertificateLinkDTO = new TransactionCertificateLinkDTO();
                    if (transactionLink.TransactionId == TransactionId)
                    {
                        transactionCertificateLinkDTO.Transaction = TransactionCertificateMapper.Map(transactionLink.ToTransaction, cultureName);
                        transactionCertificateLinkDTO.LinkTypeId = transactionLink.Type.Id;
                        transactionCertificateLinkDTO.LinkTypeName = transactionLink.Type.Text;
                        transactionCertificateLinkDTO.TransactionNumber = transactionLink.ToTransaction.Number;
                    }
                    else
                    {
                        transactionCertificateLinkDTO.Transaction = TransactionCertificateMapper.Map(transactionLink.Transaction, cultureName);
                        transactionCertificateLinkDTO.LinkTypeId = transactionLink.Type.Id;
                        transactionCertificateLinkDTO.LinkTypeName = transactionLink.Type.Text;
                        transactionCertificateLinkDTO.TransactionNumber = transactionLink.Transaction.Number;
                    }

                    return transactionCertificateLinkDTO;
                }).ToList();
            return transactionLinkDTOs;
        }
        public static TransactionAssignmentDTO Map(TransactionAssignment transactionAssignment)
        {
            if (transactionAssignment == null)
            {
                return null;
            }

            TransactionAssignmentDTO transactionAssignmentDTO = new TransactionAssignmentDTO()
            {
                ToUserName = transactionAssignment.ToUser != null ? transactionAssignment.ToUser.LocalName : null,
                ToOrgUnitName = transactionAssignment.ToEntity.LocalName,
                ActionId = transactionAssignment.Action != null ? transactionAssignment.Action.Id : 0,
                Remarks = transactionAssignment.Description,
                DateH = transactionAssignment.DateH,
                Date = transactionAssignment.Date
            };

            return transactionAssignmentDTO;
        }

        public static List<TransactionAssignmentDTO> Map(IList<TransactionAssignmentHistory> TransactionAssignmentHistories)
        {
            if (TransactionAssignmentHistories == null || !TransactionAssignmentHistories.Any())
            {
                return null;
            }
            List<TransactionAssignmentDTO> transactionAssignmentDTOs = new List<TransactionAssignmentDTO>();
            foreach (var transactionAssignment in TransactionAssignmentHistories)
            {
                transactionAssignmentDTOs.Add(new TransactionAssignmentDTO()
                {
                    FromUserName = transactionAssignment.FromUser != null ? transactionAssignment.FromUser.LocalName : null,
                    ToUserName = transactionAssignment.ToUser != null ? transactionAssignment.ToUser.LocalName : null,
                    FromOrgUnitName = transactionAssignment.FromEntity.LocalName,
                    ToOrgUnitName = transactionAssignment.ToEntity.LocalName,
                    FromUserId = transactionAssignment.FromUser != null ? transactionAssignment.FromUser.Id : 0,
                    ToUserId = transactionAssignment.ToUser != null ? transactionAssignment.ToUser.Id : 0,
                    ActionId = transactionAssignment.Action != null ? transactionAssignment.Action.Id : 0,
                    Remarks = transactionAssignment.Description,
                    DateH = transactionAssignment.DateH,
                    Date = transactionAssignment.Date,
                    TrayName = transactionAssignment.Tray != null ? transactionAssignment.Tray.LocalName : null,
                    ActionName = transactionAssignment.Action != null ? transactionAssignment.Action.LocalName : "",
                    StringContent = transactionAssignment.ExplanationId.HasValue & transactionAssignment.ExplanationId != -1
                    && transactionAssignment?.Explanation?.Document?.Document?.Content != null ? Encoding.Unicode.GetString(transactionAssignment.Explanation.Document.Document.Content) : string.Empty,
                    GeneralExplanation = transactionAssignment.GeneralExplanation,
                    SpecialExplanation = transactionAssignment.SpecialExplanation,
                    ReceivedDate = transactionAssignment?.ReceivedDate?.Replace("PM", "م").Replace("AM", "ص"),
                    FromUserInternalNumber = transactionAssignment?.FromUser?.InternalNumber,
                    ToUserInternalNumber = transactionAssignment?.ToUser?.InternalNumber,
                });
            }

            return transactionAssignmentDTOs;
        }

        public static TransactionAssignmentHistory Map(TransactionAssignmentDTO transactionAssignmentDTOs)
        {
            if (transactionAssignmentDTOs == null)
            {
                return null;
            }

            TransactionAssignmentHistory transactionAssignmentHistories = new TransactionAssignmentHistory()
            {
                DateH = transactionAssignmentDTOs.DateH,
                Date = transactionAssignmentDTOs.Date,
            };
            return transactionAssignmentHistories;
        }

        public static List<TransactionCertificateHistoryDTO> Map(IList<TransactionLogInfo> transactionLogInfos)
        {
            if (transactionLogInfos == null || !transactionLogInfos.Any())
            {
                return null;
            }
            List<TransactionCertificateHistoryDTO> transactionCertificateHistoryDTOs = transactionLogInfos
                .Select(transactionLogInfo => new TransactionCertificateHistoryDTO()
                {

                    UserId = transactionLogInfo.UserId,
                    UserName = transactionLogInfo.UserName,
                    CertificateHistoryDetails = TransactionCertificateMapper.Map(transactionLogInfo.TransactionLogDetails)
                }).ToList();
            return transactionCertificateHistoryDTOs;
        }

        public static List<TransactionLogInfo> Map(List<TransactionCertificateHistoryDTO> transactionCertificateHistoryDTOs)
        {
            if (transactionCertificateHistoryDTOs == null || !transactionCertificateHistoryDTOs.Any())
            {
                return null;
            }
            List<TransactionLogInfo> UserTrayPreferenceInfo = transactionCertificateHistoryDTOs
                .Select(transactionCertificateHistoryDTO => new TransactionLogInfo()
                {
                    UserId = transactionCertificateHistoryDTO.UserId,
                    UserName = transactionCertificateHistoryDTO.UserName
                }).ToList();
            return UserTrayPreferenceInfo;
        }

        public static List<TransactionCertificateHistoryDetailDTO> Map(IList<TransactionLogDetailInfo> transactionLogInfoDetails)
        {
            if (transactionLogInfoDetails == null || !transactionLogInfoDetails.Any())
            {
                return null;
            }
            List<TransactionCertificateHistoryDetailDTO> transactionCertificateHistoryDetailDTOs =
                transactionLogInfoDetails
                .Select(transactionLogInfoDetail => new TransactionCertificateHistoryDetailDTO()
                {
                    Date = transactionLogInfoDetail.Date,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionLogInfoDetail.Date),
                    Description = transactionLogInfoDetail.Description
                }).ToList();

            return transactionCertificateHistoryDetailDTOs;
        }
        public static List<TransactionLogDetailInfo> Map(IList<TransactionCertificateHistoryDetailDTO> transactionCertificateHistoryDetailDTOs)
        {
            if (transactionCertificateHistoryDetailDTOs == null || !transactionCertificateHistoryDetailDTOs.Any())
            {
                return null;
            }
            List<TransactionLogDetailInfo> UserTrayPreferenceInfo = transactionCertificateHistoryDetailDTOs
                .Select(transactionCertificateHistoryDetailDTO => new TransactionLogDetailInfo()
                {
                    Date = transactionCertificateHistoryDetailDTO.Date,
                    Description = transactionCertificateHistoryDetailDTO.Description
                }).ToList();
            return UserTrayPreferenceInfo;
        }

    }
}





