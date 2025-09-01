using MobileApi.Domain;
using MobileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;
using YESSERCommon = MCS.Common;
using YESSERDomain = MCS.Domain;
using TransactionCategory = MCS.Common.TransactionCategory;
using MCS.Common;

namespace MCS.Service.Mappers
{
    public class UserMobileMapper
    {
        public static List<UserMobileOrgUnitDTO> Map(IList<OrgUnit> organizationUnits, string cultureName)
        {
            if (organizationUnits == null || !organizationUnits.Any())
            {
                return new List<UserMobileOrgUnitDTO>();
            }
            List<UserMobileOrgUnitDTO> userMobileOrgUnitDTOs = new List<UserMobileOrgUnitDTO>();

            foreach (OrgUnit organizationUnit in organizationUnits)
            {
                UserMobileOrgUnitDTO userMobileOrgUnitDTO = Map(organizationUnit, cultureName);

                userMobileOrgUnitDTOs.Add(userMobileOrgUnitDTO);
            }

            return userMobileOrgUnitDTOs;
        }

        public static UserMobileOrgUnitDTO Map(OrgUnit organizationUnit, string cultureName)
        {
            if (organizationUnit == null)
            {
                return new UserMobileOrgUnitDTO();
            }

            List<UserMobileOrgUnitUsersDTO> userMobileOrgUnitUsersDTOs = OrgUnitUserMap(organizationUnit.Users, organizationUnit.Id, cultureName);

            UserMobileOrgUnitDTO userMobileOrgUnitDTO = new UserMobileOrgUnitDTO()
            {
                Id = organizationUnit.Id,
                Name = organizationUnit.LocalName,
                IsVirtual = organizationUnit.IsVirtualUnit,
                ParentId = organizationUnit.ParentId,
                UserDefinedId = organizationUnit.Number.ToString(),
                HasChilds = organizationUnit.HasChilds,
                Active = organizationUnit.IsActive,
                Persons = userMobileOrgUnitUsersDTOs
            };

            if (organizationUnit.LocalizationIdentifier != null)
            {
                Localization name = organizationUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                if (name != null)
                {
                    userMobileOrgUnitDTO.Name = name.Text;
                }
            }

            return userMobileOrgUnitDTO;
        }

        public static List<UserMobileOrgUnitUsersDTO> OrgUnitUserMap(IList<UserProfile> usersProfile, int EntityId, string cultureName)
        {
            if (usersProfile == null || !usersProfile.Any())
            {
                return new List<UserMobileOrgUnitUsersDTO>();
            }
            List<UserMobileOrgUnitUsersDTO> userMobileOrgUnitUsersDTOs = new List<UserMobileOrgUnitUsersDTO>();

            foreach (UserProfile userProfile in usersProfile)
            {
                UserMobileOrgUnitUsersDTO userMobileOrgUnitDTO = OrgUnitUserMap(userProfile, EntityId, cultureName);

                userMobileOrgUnitUsersDTOs.Add(userMobileOrgUnitDTO);
            }

            return userMobileOrgUnitUsersDTOs;
        }
        public static List<UserMobileOrgUnitUsersDTO> OrgUnitUserMap(IList<ExternalPartyManager> externalPartyManagers, int EntityId, string cultureName)
        {
            if (externalPartyManagers == null || !externalPartyManagers.Any())
            {
                return new List<UserMobileOrgUnitUsersDTO>();
            }
            List<UserMobileOrgUnitUsersDTO> userMobileOrgUnitUsersDTOs = new List<UserMobileOrgUnitUsersDTO>();

            foreach (ExternalPartyManager externalPartyManager in externalPartyManagers)
            {
                UserMobileOrgUnitUsersDTO userMobileOrgUnitDTO = OrgUnitUserMap(externalPartyManager, EntityId, cultureName);

                userMobileOrgUnitUsersDTOs.Add(userMobileOrgUnitDTO);
            }

            return userMobileOrgUnitUsersDTOs;
        }
        public static UserMobileOrgUnitUsersDTO OrgUnitUserMap(ExternalPartyManager externalPartyManager, int EntityId, string cultureName)
        {
            if (externalPartyManager == null)
            {
                return new UserMobileOrgUnitUsersDTO();
            }

            UserMobileOrgUnitUsersDTO userMobileOrgUnitUsersDTO = new UserMobileOrgUnitUsersDTO()
            {
                Id = externalPartyManager.Id,

                Name = externalPartyManager.Name.Localizations.Where(n => n.Culture.ShortName == cultureName).FirstOrDefault().Text,
                EntityId = EntityId
            };

            return userMobileOrgUnitUsersDTO;
        }

        public static UserMobileOrgUnitUsersDTO OrgUnitUserMap(UserProfile usersProfile, int EntityId, string cultureName)
        {
            if (usersProfile == null)
            {
                return new UserMobileOrgUnitUsersDTO();
            }

            UserMobileOrgUnitUsersDTO userMobileOrgUnitUsersDTO = new UserMobileOrgUnitUsersDTO()
            {
                Id = usersProfile.Id,
                Name = usersProfile.LocalizationIdentifier.Localizations.Where(n => n.Culture.ShortName == cultureName).FirstOrDefault().Text,
                EntityId = EntityId
            };

            return userMobileOrgUnitUsersDTO;
        }
        public static List<Confidentiality> ConfidentialityMap(IList<YESSERDomain.Permission> permissions)
        {
            List<Confidentiality> confidentialities = permissions.Select(permission => new Confidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList();

            confidentialities.AddRange(permissions.Select(permission => new Confidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());
            confidentialities.AddRange(permissions.Select(permission => new Confidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());

            confidentialities.AddRange(permissions.Select(permission => new Confidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());
            return confidentialities;
        }
        public static List<AttachConfidentiality> AttachConfidentialityMap(IList<YESSERDomain.Permission> permissions)
        {
            List<AttachConfidentiality> attachConfidentialities = permissions.Select(permission => new AttachConfidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList();

            attachConfidentialities.AddRange(permissions.Select(permission => new AttachConfidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());
            attachConfidentialities.AddRange(permissions.Select(permission => new AttachConfidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),

                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());
            attachConfidentialities.AddRange(permissions.Select(permission => new AttachConfidentiality()
            {
                Id = permission.Id,
                Text = permission.LocalName != null ? permission.LocalName : "",
                CategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = permission.Code,
                PermisionName = permission.Code
            }).ToList());
            return attachConfidentialities;
        }

        public static List<TransactionSource> TransactionSourceMap(List<YESSERDomain.TransactionType> transactionTypes, YESSERCommon.TransactionCategories sourceTransactionType)
        {
            return transactionTypes.Select(sourceType => new TransactionSource()
            {
                Id = sourceType.Id,
                Text = sourceType.Text != null ? sourceType.Text : "",
                CategoryId = sourceTransactionType == YESSERCommon.TransactionCategories.Inbound ? TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) :
                 sourceTransactionType == YESSERCommon.TransactionCategories.Outbound ? TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) :
               sourceTransactionType == YESSERCommon.TransactionCategories.DraftOutbound ? TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
               : TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                PrivilegeName = sourceType.Permission.Code,
                PermisionName = sourceType.Permission.Code,
            }).ToList();
        }

        public static List<TransactionProcess> GetAllActions(List<YESSERDomain.Action> actions)
        {
            List<TransactionProcess> actionList = actions.Select(action => new TransactionProcess()
            {
                Id = action.Id,
                Text = action.LocalName != null ? action.LocalName : "",
                CategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList();
            actionList.AddRange(actions.Select(action => new TransactionProcess()
            {
                Id = action.Id,
                Text = action.LocalName != null ? action.LocalName : "",
                CategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());
            actionList.AddRange(actions.Select(action => new TransactionProcess()
            {
                Id = action.Id,
                Text = action.LocalName != null ? action.LocalName : "",
                CategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());
            actionList.AddRange(actions.Select(action => new TransactionProcess()
            {
                Id = action.Id,
                Text = action.LocalName != null ? action.LocalName : "",
                CategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());
            return actionList;
        }

        public static List<MobileApi.Domain.Priority> PriorityMap(List<YESSERDomain.Priority> priorities, YESSERCommon.TransactionCategories transactionCategories)
        {
            return priorities.Select(priority => new MobileApi.Domain.Priority()
            {
                Id = priority.Id,
                Text = priority.Text != null ? priority.Text : "",
                CategoryId = transactionCategories == YESSERCommon.TransactionCategories.Inbound ? TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) :
                 transactionCategories == YESSERCommon.TransactionCategories.Outbound ? TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) :
               transactionCategories == YESSERCommon.TransactionCategories.DraftOutbound ? TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
               : TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
            }).ToList();
        }

        public static List<MobileApi.Domain.TransactionType> LetterTypeMap(List<LetterType> letterTypes, YESSERCommon.TransactionCategories transactionCategories)
        {
            return letterTypes.Select(letterType => new MobileApi.Domain.TransactionType()
            {
                Id = letterType.Id,
                Text = letterType.Text != null ? letterType.Text : "",
                CategoryId = transactionCategories == YESSERCommon.TransactionCategories.Inbound ? TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
              : transactionCategories == YESSERCommon.TransactionCategories.Outbound ? TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
              : transactionCategories == YESSERCommon.TransactionCategories.DraftOutbound ? TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
                : TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList();
        }

        public static List<IncludedItemType> AttachementsTypeMap(List<YESSERDomain.AttachmentType> attachmentTypes, YESSERCommon.TransactionCategories transactionCategories)
        {
            return attachmentTypes.Select(attachmentType => new IncludedItemType()
            {
                Id = attachmentType.Id,
                Text = attachmentType.Text != null ? attachmentType.Text : "",
                CategoryId = transactionCategories == YESSERCommon.TransactionCategories.Inbound ? TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
             : transactionCategories == YESSERCommon.TransactionCategories.Outbound ? TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
             : transactionCategories == YESSERCommon.TransactionCategories.DraftOutbound ? TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
               : TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                IsArchivable = attachmentType.Archivable
            }).ToList();
        }

        public static List<MobileApi.Domain.AttachmentType> LookupAttachementsTypeMap(List<YESSERDomain.Lookup> lookupAttachmentTypes)
        {
            List<MobileApi.Domain.AttachmentType> attachmentTypes = lookupAttachmentTypes.Select(attachmentType => new MobileApi.Domain.AttachmentType()
            {
                Id = attachmentType.Id,
                Text = attachmentType.Text != null ? attachmentType.Text : "",
                CategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList();

            attachmentTypes.AddRange(lookupAttachmentTypes.Select(attachmentType => new MobileApi.Domain.AttachmentType()
            {
                Id = attachmentType.Id,
                Text = attachmentType.Text != null ? attachmentType.Text : "",
                CategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());


            attachmentTypes.AddRange(lookupAttachmentTypes.Select(attachmentType => new MobileApi.Domain.AttachmentType()
            {
                Id = attachmentType.Id,
                Text = attachmentType.Text != null ? attachmentType.Text : "",
                CategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());
            attachmentTypes.AddRange(lookupAttachmentTypes.Select(attachmentType => new MobileApi.Domain.AttachmentType()
            {
                Id = attachmentType.Id,
                Text = attachmentType.Text != null ? attachmentType.Text : "",
                CategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty)
            }).ToList());


            return attachmentTypes;
        }

        public static List<MobileApi.Domain.Tray> UserTraysMap(List<TrayDetailsInfo> trays)
        {
            return trays.Select(tray => new MobileApi.Domain.Tray()
            {
                TrayId = tray.Id,
                Name = tray.Id == (int)YESSERCommon.TrayType.InternalInboundCopies ? "النسخ الالكترونية" : tray.Name,//i but it hard coded cuz if i change the name in database will change in web also 
                Counter = tray.AllTransactionCount,
                AllowedActions = GetTrayAllowedActions(tray.Id)
            }).ToList();
        }

        private static List<string> GetTrayAllowedActions(int trayId)
        {
            switch (trayId)
            {
                case 99:
                case 100:
                case (int)YESSERCommon.TrayType.MyTransactions:
                    return new List<string>() { Actions.Assign.ToString(), Actions.Reject.ToString(), Actions.Save.ToString(), Actions.Update.ToString() };
                case (int)YESSERCommon.TrayType.DraftOutbound:
                    return new List<string>() { Actions.Update.ToString(), Actions.Assign.ToString() , Actions.Reject.ToString() };
                case (int)YESSERCommon.TrayType.OrgUnit:
                    return new List<string>() { Actions.AssignToSelf.ToString() };
                case (int)YESSERCommon.TrayType.Saved:
                    return new List<string>() { Actions.ReturnFromSaveTray.ToString() };
                case (int)YESSERCommon.TrayType.Manager:
                    return new List<string>() { Actions.Assign.ToString() };
                case (int)YESSERCommon.TrayType.SentTransactions:
                    return new List<string>() { Actions.Return.ToString() };
                case (int)YESSERCommon.TrayType.Copies:
                    return new List<string>() { Actions.SetCopyAsViewed.ToString(), Actions.Assign.ToString() };

                default:
                    return new List<string>();
            }
        }

        public enum Actions
        {
            Save,
            Assign,
            Return,
            Update,
            AssignToSelf,
            ReturnFromSaveTray,
            SetCopyAsViewed,
            Reject
        }

        internal static List<UserMobileExternalPartyDTO> ExternalPartiesMap(IList<ExternalParty> externalParties, string language)
        {
            return externalParties.Select(externalParty => new UserMobileExternalPartyDTO
            {
                Id = externalParty.Id,
                ParentId = externalParty.ParentId,
                Name = externalParty.LocalName,
                IsVirtual = externalParty.IsVirtual,
                UserDefinedId = externalParty.Number,
                Active = externalParty.IsActive,
                Persons = OrgUnitUserMap(externalParty.PartyManagers, externalParty.Id, language),
            }).ToList();
        }

        internal static List<MobileApi.Domain.Transaction> TransactionsMap(IList<YESSERDomain.Transaction> transactions, int trayId, string language)
        {
            if (transactions == null || !transactions.Any())
            {
                return new List<MobileApi.Domain.Transaction>();
            }
            List<MobileApi.Domain.Transaction> userMobileTransactions = new List<MobileApi.Domain.Transaction>();

            foreach (YESSERDomain.Transaction transaction in transactions)
            {
                MobileApi.Domain.Transaction userMobileTransaction = TransactionMap(transaction, trayId, language);

                userMobileTransactions.Add(userMobileTransaction);
            }

            return userMobileTransactions;
        }

        private static MobileApi.Domain.Transaction TransactionMap(YESSERDomain.Transaction transaction, int trayId, string language)
        {
            if (transaction == null)
            {
                return new MobileApi.Domain.Transaction();
            }
            string TransNumberRow = "";

            if (trayId == (int)YESSERCommon.TrayType.DraftOutbound)
            {
                if (transaction.ExternalParty != null)
                {
                    TransNumberRow = string.Format("{0} / {1}", transaction.Number, transaction.ExternalParty.LocalName);
                }
                else if (transaction.Entity != null)
                {
                    TransNumberRow = string.Format("{0} / {1}", transaction.Number, transaction.Entity.LocalName);
                }
                else
                {
                    TransNumberRow = string.Format("{0} - {1}", transaction.Number, transaction.OrgUnit.LocalName);
                }
            }
            else
            {
                TransNumberRow = string.Format("{0} - {1}", transaction.Number, transaction.OrgUnit.LocalName);
            }

            MobileApi.Domain.Transaction userMobileTransaction = new MobileApi.Domain.Transaction()
            {
                TransID = transaction.Id,
                TransNo = transaction.Number.ToString(),
                TransCategory = transaction.TransactionCategoryId,
                TransTitle = transaction.Subject,
                TransDate = transaction.DateH + " " + transaction.Date.ToShortTimeString(),
                Has_Supporting_Attachments = transaction.Attachments.Any(),
                TransNumberRow = TransNumberRow,
                TransFrom = string.Empty,
                FileSize = string.Empty,
                ReadOnly = false,
                TransSourceRow = transaction.TransactionType.Text,
                EntityName = transaction.OrgUnit.LocalName,
                PrivilegeName = transaction.Confidentiality.Code,
                OutboundDraft = transaction.IsDraft,
                Color = transaction.TransactionType.Color.Text.ToString(),
                IsCopy = transaction.Copies != null || (transaction.Copies != null && transaction.Copies.Count != 0) ? true : false,
                IsInternalOutbound = transaction.TransactionCategoryId == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),
                AllowedActions = GetTrayAllowedActions(trayId),
                SourceTray = transaction.SourceTray,
                IsAppointment = transaction.IsAppointment,
                IsDelayed = transaction.IsDelayed
            };

            return userMobileTransaction;
        }

        public static List<AssignTrackEntity> TransactionAssignmentHistoryMap(List<TransactionAssignmentHistory> transactionAssignmentHistories)
        {
            if (!transactionAssignmentHistories.Any())
            {
                return new List<AssignTrackEntity>();
            }

            return transactionAssignmentHistories.Select(h => new AssignTrackEntity
            {
                FromEntity = h.FromEntity.LocalName,
                FromPerson = h.FromUser.LocalName,
                ProcessName = h.Action?.LocalName,
                Remarks = h.Description,
                ToEntity = h.ToEntity.LocalName,
                ToPerson = h.ToUser?.LocalName,
                Date = h.DateH + " " + h.Date.ToShortTimeString()
            }).ToList();
        }

        internal static List<SearchTransactionDTO> SearchTransactionsMap(List<MobileSearchResult> mobileSearchResults, string language)
        {
            if (mobileSearchResults == null || !mobileSearchResults.Any())
            {
                return new List<SearchTransactionDTO>();
            }
            List<SearchTransactionDTO> searchTransactionDTOs = new List<SearchTransactionDTO>();

            foreach (MobileSearchResult mobileSearchResult in mobileSearchResults)
            {
                SearchTransactionDTO searchTransactionDTO = SearchTransactionMap(mobileSearchResult, language);

                searchTransactionDTOs.Add(searchTransactionDTO);
            }

            return searchTransactionDTOs;
        }

        internal static SearchTransactionDTO SearchTransactionMap(MobileSearchResult mobileSearchResult, string language)
        {
            if (mobileSearchResult == null)
            {
                return new SearchTransactionDTO();
            }

            SearchTransactionDTO searchTransactionDTO = new SearchTransactionDTO()
            {
                TransID = mobileSearchResult.TransID,
                TransNo = mobileSearchResult.TransNo.ToString(),
                TransCategory = mobileSearchResult.TransCategory,
                TransTitle = mobileSearchResult.TransTitle,
                TransDate = mobileSearchResult.TransDate,
                TransFrom = mobileSearchResult.TransFrom,
                ReadOnly = false,
                FileSize = mobileSearchResult.FileSize,
                Has_Supporting_Attachments = false,
                AllowedActions = new List<string>(),
                TransNumberRow = mobileSearchResult.TransNumberRow,
                EntityName = mobileSearchResult.EntityName,
                TransSourceRow = mobileSearchResult.TransSourceRow,
                PrivilegeName = mobileSearchResult.PrivilegeName,
                OutboundDraft = mobileSearchResult.OutboundDraft,
                IsInternalOutbound = mobileSearchResult.TransCategory == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty),

            };

            return searchTransactionDTO;
        }

        internal static TransactionDetailsDTO MapTransactionDetails(TransactionDetails transactionDetails)
        {
            TransactionDetailsDTO transactionDetailsDTO = new TransactionDetailsDTO()
            {
                Id = transactionDetails.Id,
                Date = transactionDetails.Date,
                HijriDate = transactionDetails.DateH + " " + transactionDetails.Date.ToShortTimeString(),
                Number = transactionDetails.Number
            };

            return transactionDetailsDTO;
        }

        internal static List<string> UserPrivilegesMap(IList<MCS.Domain.Permission> userPermissions, string language)
        {
            if (userPermissions == null || !userPermissions.Any())
            {
                return new List<string>();
            }
            List<string> userPermistions = new List<string>();

            foreach (MCS.Domain.Permission permission in userPermissions)
            {
                userPermistions.Add(permission.Code);
            }

            return userPermistions.Distinct().ToList();
        }
        internal static List<UserEntity> UserEntityMap(IList<OrgUnit> orgUnits, string language)
        {
            if (orgUnits == null || !orgUnits.Any())
            {
                return new List<UserEntity>();
            }
            List<UserEntity> userEntities = new List<UserEntity>();

            foreach (OrgUnit orgunit in orgUnits)
            {
                userEntities.Add(new UserEntity
                {

                    Id = orgunit.Id,
                    Text = orgunit.LocalName


                });
            }

            return userEntities.ToList();
        }

    }
}