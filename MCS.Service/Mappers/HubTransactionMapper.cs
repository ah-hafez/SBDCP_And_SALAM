using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Localization.SupportClasses;
using MCS.Business;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.HubTransaction;

namespace MCS.Service.Hubs
{
    public static class HubTransactionMapper
    {
        public static List<HubTransactionDTO> Map(IList<HubTransaction> hubTransactionDTOList, string culture)
        {
            IPriorityRepository priorityRepository = IoC.Resolve<IPriorityRepository>();
            IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
            IExternalPartyRepository externalPartyRepository = IoC.Resolve<IExternalPartyRepository>();
            ILookupRepository lookupRepository = IoC.Resolve<ILookupRepository>();
            ITransactionTypeBL transactionSourceTypeBL = new TransactionTypeBL();

            List<HubTransactionDTO> transactions = hubTransactionDTOList
                 .Select(t => new HubTransactionDTO
                 {
                     Id = t.Id,
                     ConfidentialityLevelId = t.ConfidentialityLevelId,
                     ConfidentialityName = permissionRepository.Get(t.ConfidentialityLevelId)
                     .Name.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                     DestinationId = t.DestinationId,
                     HijriRecordDate = t.HijriRecordDate,
                     PriorityLevelId = t.PriorityLevelId,
                     PriorityText = priorityRepository.Get(t.PriorityLevelId)
                     .LocalizationIdentifier.Localizations
                     .Where(s => s.Culture.ShortName == culture).LocalText(),
                     OrgUnitId = t.OrgUnitId,
                     RecordDate = t.RecordDate,
                     Subject = t.Subject,
                     Remarks = t.Remarks,
                     RQUID = t.RQUID,
                     TransactionNumber = t.TransactionNumber,
                     ExternalPartyName = externalPartyRepository.Get(t.OrgUnitId).Name.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                     TransactionCategory = lookupRepository.GetLookupItem(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)).Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                     TransactionTypeName = transactionSourceTypeBL.GetTransactionSourceTypeById((int)HubConstants.TransactionTypeId).LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                     Status = t.Status
                 }).ToList();
            return transactions;
        }
        public static HubTransactionDTO Map(HubTransaction hubTransactionDTO, string culture)
        {
            IPriorityRepository priorityRepository = IoC.Resolve<IPriorityRepository>();
            IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
            IExternalPartyRepository externalPartyRepository = IoC.Resolve<IExternalPartyRepository>();
            ILookupRepository lookupRepository = IoC.Resolve<ILookupRepository>();
            ITransactionTypeBL transactionSourceTypeBL = new TransactionTypeBL();

            HubTransactionDTO transaction = new HubTransactionDTO
            {
                Id = hubTransactionDTO.Id,
                ConfidentialityLevelId = hubTransactionDTO.ConfidentialityLevelId,
                ConfidentialityName = permissionRepository.Get(hubTransactionDTO.ConfidentialityLevelId)
                     .Name.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                DestinationId = hubTransactionDTO.DestinationId,
                HijriRecordDate = hubTransactionDTO.HijriRecordDate,
                PriorityLevelId = hubTransactionDTO.PriorityLevelId,
                PriorityText = priorityRepository.Get(hubTransactionDTO.PriorityLevelId)
                     .LocalizationIdentifier.Localizations
                     .Where(s => s.Culture.ShortName == culture).LocalText(),
                OrgUnitId = hubTransactionDTO.OrgUnitId,
                RecordDate = hubTransactionDTO.RecordDate,
                Subject = hubTransactionDTO.Subject,
                Remarks = hubTransactionDTO.Remarks,
                RQUID = hubTransactionDTO.RQUID,
                TransactionNumber = hubTransactionDTO.TransactionNumber,
                ExternalPartyName = externalPartyRepository.Get(hubTransactionDTO.OrgUnitId).Name.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                TransactionCategory = lookupRepository.GetLookupItem(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, culture)).Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                TransactionTypeName = transactionSourceTypeBL.GetTransactionSourceTypeById((int)HubConstants.TransactionTypeId).LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == culture).LocalText(),
                Status = hubTransactionDTO.Status,
                ReminderGDate = hubTransactionDTO.ReminderGDate,
                ReminderHDate = hubTransactionDTO.ReminderHDate,
                HubRelatedPersons = hubTransactionDTO.HubRelatedPersons.Select(hrp =>
                {
                    HubRelatedPersonDTO hubRelatedPersonDTO = new HubRelatedPersonDTO
                    {
                        Id = hrp.Id,
                        Address = hrp.Address,
                        Email = hrp.Email,
                        Name = hrp.Name,
                        NationalId = hrp.NationalId
                    };
                    return hubRelatedPersonDTO;
                }).ToList(),
                MainDocument = hubTransactionDTO.MainDocument != null ? new DocumentInfoDTO
                {
                    Name = hubTransactionDTO.MainDocument.Name,
                    ECMId = hubTransactionDTO.MainDocument.ECMId,
                    IsDeleted = hubTransactionDTO.MainDocument.IsDeleted,
                    Size = hubTransactionDTO.MainDocument.Size,
                    MimeType = hubTransactionDTO.MainDocument.MimeType,
                    Document = new DocumentDTO
                    {
                        Id = hubTransactionDTO.MainDocument.Document.Id,
                        Content = hubTransactionDTO.MainDocument.Document.Content
                    }
                } : new DocumentInfoDTO() { Document = new DocumentDTO() }
            };

            transaction.HubAttachments = hubTransactionDTO.HubAttachments.Select(ta =>
            {
                HubAttachmentDTO hubAttachmentDTO = new HubAttachmentDTO
                {
                    Id = ta.Id,
                    Count = ta.Count,
                    Description = ta.Description,
                    ExternalAttachementId = ta.ExternalAttachementId,
                    TypeId = ta.TypeId
                };

                hubAttachmentDTO.Type = new AttachmentTypeDTO
                {
                    LocalName = ta.Type.LocalizationIdentifier.Localizations.Where(li => li.Culture.ShortName == culture).FirstOrDefault().Text
                };

                if (ta.DocumentInfo != null)
                {
                    hubAttachmentDTO.DocumentInfo = new DocumentInfoDTO
                    {
                        Id = ta.DocumentInfo.Id,
                        ECMId = ta.DocumentInfo.ECMId,
                        IsDeleted = ta.DocumentInfo.IsDeleted,
                        MimeType = ta.DocumentInfo.MimeType,
                        Name = ta.DocumentInfo.Name,
                        Size = ta.DocumentInfo.Size
                    };

                    if (ta.DocumentInfo.Document != null)
                    {
                        hubAttachmentDTO.DocumentInfo.Document = new DocumentDTO
                        {
                            Id = ta.DocumentInfo.Document.Id,
                            Content = ta.DocumentInfo.Document.Content
                        };
                    }
                }

                return hubAttachmentDTO;
            }).ToList();

            return transaction;
        }
    }
}

