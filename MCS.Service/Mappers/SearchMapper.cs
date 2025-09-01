using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Service.Mappers
{
    public static class SearchMapper
    {
        public static List<SubjectSearchResultDTO> Map(IList<SubjectSearchResult> subjectSearchResults)
        {
            if (subjectSearchResults == null)
            {
                return new List<SubjectSearchResultDTO>();
            }

            List<SubjectSearchResultDTO> baseSearchResultDTOs = subjectSearchResults
                            .Select(subjectSearchResult => new SubjectSearchResultDTO
                            {
                                Id = subjectSearchResult.Id,
                                TransactionType = subjectSearchResult.TransactionType,
                                Number = subjectSearchResult.Number,
                                Subject = subjectSearchResult.Subject,
                                DateH = subjectSearchResult.DateH,
                                Date = subjectSearchResult.Date,
                                ConfidentialityName = subjectSearchResult.ConfidentialityName,
                                PriorityName = subjectSearchResult.PriorityName,
                                PartyName = subjectSearchResult.PartyName,
                                OrgUnitName = subjectSearchResult.OrgUnitName,
                                StatusName = subjectSearchResult.StatusName,
                                WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = subjectSearchResult.ColorCode,
                                TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                                TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                                HasPermission = subjectSearchResult.HasPermission,
                                ToUserId = subjectSearchResult.ToUserId,
                                IsDeleted = subjectSearchResult.IsDeleted,
                                StatusId = subjectSearchResult.StatusId,
                                TotalCount = subjectSearchResult.TotalCount,
                                HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = subjectSearchResult.ToEntityId,
                                Encrypted = subjectSearchResult.Encrypted,

                                //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                                //RemindDate = subjectSearchResult.RemindDate,
                                //RemindDateH = subjectSearchResult.RemindDateH,
                                //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                                //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                            }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<BarcodeSearchResultDTO> Map(IList<BarcodeSearchResult> subjectSearchResults)
        {
            if (subjectSearchResults == null)
            {
                return new List<BarcodeSearchResultDTO>();
            }

            List<BarcodeSearchResultDTO> baseSearchResultDTOs = subjectSearchResults
                            .Select(subjectSearchResult => new BarcodeSearchResultDTO
                            {
                                Id = subjectSearchResult.Id,
                                TransactionType = subjectSearchResult.TransactionType,
                                Number = subjectSearchResult.Number,
                                Subject = subjectSearchResult.Subject,
                                DateH = subjectSearchResult.DateH,
                                Date = subjectSearchResult.Date,
                                ConfidentialityName = subjectSearchResult.ConfidentialityName,
                                PriorityName = subjectSearchResult.PriorityName,
                                PartyName = subjectSearchResult.PartyName,
                                OrgUnitName = subjectSearchResult.OrgUnitName,
                                StatusName = subjectSearchResult.StatusName,
                                WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = subjectSearchResult.ColorCode,
                                TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                                TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                                HasPermission = subjectSearchResult.HasPermission,
                                IsDeleted = subjectSearchResult.IsDeleted,
                                StatusId = subjectSearchResult.StatusId,
                                ToUserId = subjectSearchResult.ToUserId,
                                TotalCount = subjectSearchResult.TotalCount,
                                HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = subjectSearchResult.ToEntityId,

                                //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                                //RemindDate = subjectSearchResult.RemindDate,
                                //RemindDateH = subjectSearchResult.RemindDateH,
                                //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                                //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)
                            }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<InboundSearchResultDTO> Map(IList<InboundSearchResult> inboundSearchResults)
        {
            if (inboundSearchResults == null)
            {
                return new List<InboundSearchResultDTO>();
            }

            List<InboundSearchResultDTO> inboundSearchResultDTOs = inboundSearchResults
                            .Select(baseSearchResult => new InboundSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                TotalCount = baseSearchResult.TotalCount,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                DeliveryMethodId = baseSearchResult.DeliveryMethodId,
                                Encrypted = baseSearchResult.Encrypted,
                                DocumentNumber = baseSearchResult.DocumentNumber,
                            }).ToList();

            return inboundSearchResultDTOs;
        }

        private static TransactionAssignmentDTO Map(TransactionAssignment transactionAssignment)
        {

            if (transactionAssignment == null || transactionAssignment.Action == null)
            {
                return new TransactionAssignmentDTO();
            }
            TransactionAssignmentDTO transactionAssignmentDTO = new TransactionAssignmentDTO()
            {
                FromUserName = transactionAssignment.FromUser.LocalName,
                FromOrgUnitName = transactionAssignment.FromEntity.LocalName,
                Date = transactionAssignment.Date.Date,
                ActionName = transactionAssignment.Action.LocalName,
            };
            return transactionAssignmentDTO;
        }

        public static List<OutboundInternalSearchResultDTO> Map(IList<OutboundInternalSearchResult> OutboundInternalSearchResults)
        {
            if (OutboundInternalSearchResults == null)
            {
                return new List<OutboundInternalSearchResultDTO>();
            }

            List<OutboundInternalSearchResultDTO> inboundSearchResultDTOs = OutboundInternalSearchResults
                            .Select(baseSearchResult => new OutboundInternalSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                TotalCount = baseSearchResult.TotalCount,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted,

                            }).ToList();

            return inboundSearchResultDTOs;
        }
        public static List<OutboundSearchResultDTO> Map(IList<OutboundSearchResult> outboundSearchResults)
        {
            if (outboundSearchResults == null)
            {
                return new List<OutboundSearchResultDTO>();
            }

            List<OutboundSearchResultDTO> outboundSearchResultDTOs = outboundSearchResults
                            .Select(baseSearchResult => new OutboundSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                TotalCount = baseSearchResult.TotalCount,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                DeliveryMethodId = baseSearchResult.DeliveryMethodId,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted,
                            }).ToList();

            return outboundSearchResultDTOs;
        }
        public static List<ExternalPartyCopiesSearchResultDTO> Map(IList<ExternalPartyCopiesSearchResult> externalPartyCopiesSearchResult)
        {
            if (externalPartyCopiesSearchResult == null)
            {
                return new List<ExternalPartyCopiesSearchResultDTO>();
            }

            List<ExternalPartyCopiesSearchResultDTO> externalPartyCopiesSearchResultDTOs = externalPartyCopiesSearchResult
                            .Select(baseSearchResult => new ExternalPartyCopiesSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                TotalCount = baseSearchResult.TotalCount,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ExternalPartyId = baseSearchResult.externalPartyId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted =     baseSearchResult.Encrypted,
                            }).ToList();

            return externalPartyCopiesSearchResultDTOs;
        }

        public static List<OutboundDraftSearchResultDTO> Map(IList<OutboundDraftSearchResult> outboundDraftSearchResults)
        {
            if (outboundDraftSearchResults == null)
            {
                return new List<OutboundDraftSearchResultDTO>();
            }

            List<OutboundDraftSearchResultDTO> outboundSearchResultDTOs = outboundDraftSearchResults
                            .Select(baseSearchResult => new OutboundDraftSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                TotalCount = baseSearchResult.TotalCount,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted,
                            }).ToList();

            return outboundSearchResultDTOs;
        }
        public static List<EntitySearchResultDTO> Map(IList<EntitySearchResult> SearchResults)
        {
            if (SearchResults == null)
            {
                return new List<EntitySearchResultDTO>();
            }

            List<EntitySearchResultDTO> SearchResultDTOs = SearchResults
                            .Select(baseSearchResult => new EntitySearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                TotalCount = baseSearchResult.TotalCount,
                                IsDeleted = baseSearchResult.IsDeleted,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted,
                            }).ToList();

            return SearchResultDTOs;
        }
        public static List<CreatorSearchResultDTO> Map(IList<CreatorSearchResult> SearchResults)
        {
            if (SearchResults == null)
            {
                return new List<CreatorSearchResultDTO>();
            }

            List<CreatorSearchResultDTO> SearchResultDTOs = SearchResults
                            .Select(baseSearchResult => new CreatorSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                TotalCount = baseSearchResult.TotalCount,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted,

                            }).ToList();

            return SearchResultDTOs;
        }

        public static List<AssignTransactionSearchResultDTO> Map(IList<AssignTransactionSearchResult> SearchResults)
        {
            if (SearchResults == null)
            {
                return new List<AssignTransactionSearchResultDTO>();
            }

            List<AssignTransactionSearchResultDTO> SearchResultDTOs = SearchResults
                            .Select(baseSearchResult => new AssignTransactionSearchResultDTO
                            {
                                Id = baseSearchResult.Id,
                                TransactionType = baseSearchResult.TransactionType,
                                Number = baseSearchResult.Number,
                                Subject = baseSearchResult.Subject,
                                DateH = baseSearchResult.DateH,
                                Date = baseSearchResult.Date,
                                ConfidentialityName = baseSearchResult.ConfidentialityName,
                                PriorityName = baseSearchResult.PriorityName,
                                PartyName = baseSearchResult.PartyName,
                                OrgUnitName = baseSearchResult.OrgUnitName,
                                StatusName = baseSearchResult.StatusName,
                                WithArchiving = baseSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = baseSearchResult.ColorCode,
                                TransactionCategoryName = baseSearchResult.TransactionCategoryName,
                                TransactionCategoryId = baseSearchResult.TransactionCategoryId,
                                HasPermission = baseSearchResult.HasPermission,
                                ToUserId = baseSearchResult.ToUserId,
                                StatusId = baseSearchResult.StatusId,
                                IsDeleted = baseSearchResult.IsDeleted,
                                TotalCount = baseSearchResult.TotalCount,
                                HasLinks = baseSearchResult.HasLinks == 1 ? true : false,
                                ToEntityId = baseSearchResult.ToEntityId,
                                ConfidentialityId = baseSearchResult.ConfidentialityId,
                                RemindDate = baseSearchResult.RemindDate,
                                RemindDateH = baseSearchResult.RemindDateH,
                                TransactionTypeId = baseSearchResult.TransactionTypeId,
                                TransactionAssignmentDTO = Map(baseSearchResult.TransactionAssignment),
                                Encrypted = baseSearchResult.Encrypted

                            }).ToList();

            return SearchResultDTOs;
        }
        public static List<BaseSearchResultDTO> Map(IList<BaseSearchResult> baseSearchResults)
        {
            if (baseSearchResults == null)
            {
                return new List<BaseSearchResultDTO>();
            }

            List<BaseSearchResultDTO> baseSearchResultDTOs = baseSearchResults
                            .Select(subjectSearchResult => new BaseSearchResultDTO
                            {
                                Id = subjectSearchResult.Id,
                                TransactionType = subjectSearchResult.TransactionType,
                                Number = subjectSearchResult.Number,
                                Subject = subjectSearchResult.Subject,
                                DateH = subjectSearchResult.DateH,
                                Date = subjectSearchResult.Date,
                                ConfidentialityName = subjectSearchResult.ConfidentialityName,
                                PriorityName = subjectSearchResult.PriorityName,
                                PartyName = subjectSearchResult.PartyName,
                                OrgUnitName = subjectSearchResult.OrgUnitName,
                                StatusName = subjectSearchResult.StatusName,
                                WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                                ColorCode = subjectSearchResult.ColorCode,
                                TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                                TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                                HasPermission = subjectSearchResult.HasPermission,
                                IsDeleted = subjectSearchResult.IsDeleted,
                                StatusId = subjectSearchResult.StatusId,
                                ToUserId = subjectSearchResult.ToUserId,
                                TotalCount = subjectSearchResult.TotalCount,
                                Encrypted   = subjectSearchResult.Encrypted,
                                // TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)
                            }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<InquirySearchResultDTO> Map(IList<Transaction> transaction)
        {
            if (transaction == null)
            {
                return null;
            }
            List<InquirySearchResultDTO> inquirySearchResultDTO = transaction
                            .Select(b => new InquirySearchResultDTO
                            {
                                Id = b.Id,
                                Number = b.Number,
                                StatusName = (b.Status != null) ? b.Status.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                                Subject = b.Subject,
                                ToEntity = b.Assignments[0].ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText(),
                                ToUser = b.Assignments[0].ToUser != null ? b.Assignments[0].ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                                ToUserID = b.Assignments[0].ToUser != null ? b.Assignments[0].ToUser.Id : -1,
                                ConfidentialityId = b.ConfidentialityId,
                                Weight = b.Confidentiality.Weight,
                                TransactionTypeId = b.TransactionTypeId ?? 0,
                                Encrypted = b.Encrypted
                            }).ToList();

            return inquirySearchResultDTO;
        }




        public static List<NamesSearchResultDTO> Map(IList<NamesSearchResult> namesSearchResults)
        {
            if (namesSearchResults == null)
            {
                return new List<NamesSearchResultDTO>();
            }

            List<NamesSearchResultDTO> baseSearchResultDTOs = namesSearchResults
                    .Select(subjectSearchResult => new NamesSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }

        public static List<DailySearchResultDTO> Map(IList<DailySearchResult> dailySearchResults)
        {
            if (dailySearchResults == null)
            {
                return new List<DailySearchResultDTO>();
            }

            List<DailySearchResultDTO> baseSearchResultDTOs = dailySearchResults
                    .Select(subjectSearchResult => new DailySearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted   = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }

        public static List<AssignmentNoteSearchResultDTO> Map(IList<AssignmentNoteSearchResult> assignmentNoteSearchResults)
        {
            if (assignmentNoteSearchResults == null)
            {
                return new List<AssignmentNoteSearchResultDTO>();
            }

            List<AssignmentNoteSearchResultDTO> baseSearchResultDTOs = assignmentNoteSearchResults
                    .Select(subjectSearchResult => new AssignmentNoteSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<ManifestNumberSearchResultDTO> Map(IList<ManifestNumberSearchResult> manifestNumberSearchResults)
        {
            if (manifestNumberSearchResults == null)
            {
                return new List<ManifestNumberSearchResultDTO>();
            }

            List<ManifestNumberSearchResultDTO> baseSearchResultDTOs = manifestNumberSearchResults
                    .Select(subjectSearchResult => new ManifestNumberSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<MilitaryNumberOrIdentitySearchResultDTO> Map(IList<MilitaryNumberOrIdentitySearchResult> militaryNumberOrIdentitySearchResults)
        {
            if (militaryNumberOrIdentitySearchResults == null)
            {
                return new List<MilitaryNumberOrIdentitySearchResultDTO>();
            }

            List<MilitaryNumberOrIdentitySearchResultDTO> baseSearchResultDTOs = militaryNumberOrIdentitySearchResults
                    .Select(subjectSearchResult => new MilitaryNumberOrIdentitySearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<TransactionNotsSearchResultDTO> Map(IList<TransactionNotsSearchResult> transactionNotsSearchResults)
        {
            if (transactionNotsSearchResults == null)
            {
                return new List<TransactionNotsSearchResultDTO>();
            }

            List<TransactionNotsSearchResultDTO> baseSearchResultDTOs = transactionNotsSearchResults
                    .Select(subjectSearchResult => new TransactionNotsSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<ELcEmployeeSearchResultDTO> Map(IList<ElcEmployeeSearchResult> elcEmployeeSearchResult)
        {
            if (elcEmployeeSearchResult == null)
            {
                return new List<ELcEmployeeSearchResultDTO>();
            }

            List<ELcEmployeeSearchResultDTO> baseSearchResultDTOs = elcEmployeeSearchResult
                    .Select(subjectSearchResult => new ELcEmployeeSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<ExternalOutBoundOrManifestNumberSearchResultDTO> Map(IList<ExternalOutBoundOrManifestNumberSearchResult> externalOutBoundOrManifestNumberSearchResult)
        {
            if (externalOutBoundOrManifestNumberSearchResult == null)
            {
                return new List<ExternalOutBoundOrManifestNumberSearchResultDTO>();
            }

            List<ExternalOutBoundOrManifestNumberSearchResultDTO> baseSearchResultDTOs = externalOutBoundOrManifestNumberSearchResult
                    .Select(subjectSearchResult => new ExternalOutBoundOrManifestNumberSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<CopyAssignemntSearchResultDTO> Map(IList<CopyAssignemntSearchResult> copyAssignemntSearchResult)
        {
            if (copyAssignemntSearchResult == null)
            {
                return new List<CopyAssignemntSearchResultDTO>();
            }

            List<CopyAssignemntSearchResultDTO> baseSearchResultDTOs = copyAssignemntSearchResult
                    .Select(subjectSearchResult => new CopyAssignemntSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }
        public static List<SubjectLetterSearchResultDTO> Map(IList<SubjectLetterSearchResult> subjectLetterSearchResult)
        {
            if (subjectLetterSearchResult == null)
            {
                return new List<SubjectLetterSearchResultDTO>();
            }

            List<SubjectLetterSearchResultDTO> baseSearchResultDTOs = subjectLetterSearchResult
                    .Select(subjectSearchResult => new SubjectLetterSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,
                        //ConfidentialityId = subjectSearchResult.ConfidentialityId,
                        //RemindDate = subjectSearchResult.RemindDate,
                        //RemindDateH = subjectSearchResult.RemindDateH,
                        //TransactionTypeId = subjectSearchResult.TransactionTypeId,
                        //TransactionAssignmentDTO = Map(subjectSearchResult.TransactionAssignment)

                    }).ToList();

            return baseSearchResultDTOs;
        }

        public static List<TransactionNumberSearchResultDTO> Map(IList<TransactionNumberSearchResult> transactionNumberSearchResults)
        {
            if (transactionNumberSearchResults == null)
            {
                return new List<TransactionNumberSearchResultDTO>();
            }

            List<TransactionNumberSearchResultDTO> baseSearchResultDTOs = transactionNumberSearchResults
                    .Select(subjectSearchResult => new TransactionNumberSearchResultDTO
                    {
                        Id = subjectSearchResult.Id,
                        TransactionType = subjectSearchResult.TransactionType,
                        Number = subjectSearchResult.Number,
                        Subject = subjectSearchResult.Subject,
                        DateH = subjectSearchResult.DateH,
                        Date = subjectSearchResult.Date,
                        ConfidentialityName = subjectSearchResult.ConfidentialityName,
                        PriorityName = subjectSearchResult.PriorityName,
                        PartyName = subjectSearchResult.PartyName,
                        OrgUnitName = subjectSearchResult.OrgUnitName,
                        StatusName = subjectSearchResult.StatusName,
                        WithArchiving = subjectSearchResult.WithArchiving == 1 ? true : false,
                        ColorCode = subjectSearchResult.ColorCode,
                        TransactionCategoryName = subjectSearchResult.TransactionCategoryName,
                        TransactionCategoryId = subjectSearchResult.TransactionCategoryId,
                        HasPermission = subjectSearchResult.HasPermission,
                        ToUserId = subjectSearchResult.ToUserId,
                        IsDeleted = subjectSearchResult.IsDeleted,
                        StatusId = subjectSearchResult.StatusId,
                        TotalCount = subjectSearchResult.TotalCount,
                        HasLinks = subjectSearchResult.HasLinks == 1 ? true : false,
                        ToEntityId = subjectSearchResult.ToEntityId,
                        Encrypted = subjectSearchResult.Encrypted,

                    }).ToList();

            return baseSearchResultDTOs;
        }


        public static List<ICSearchResultDTO> Map(IList<ICSearchResult> inboundOutboundLinkedNumberSearchResultList)
        {
            if (inboundOutboundLinkedNumberSearchResultList == null)
            {
                return new List<ICSearchResultDTO>();
            }

            List<ICSearchResultDTO> inboundOutboundLinkedNumberSearchResultDTOList = inboundOutboundLinkedNumberSearchResultList
                            .Select(s => new ICSearchResultDTO
                            {
                                Id = s.Id,
                                TransactionType = s.TransactionType,
                                Number = s.Number,
                                Subject = s.Subject,
                                DateH = s.DateH,
                                Date = s.Date,
                                ConfidentialityName = s.ConfidentialityName,
                                PriorityName = s.PriorityName,
                                PartyName = s.PartyName,
                                OrgUnitName = s.OrgUnitName,
                                StatusName = s.StatusName,
                                WithArchiving = s.WithArchiving == 1 ? true : false,
                                ColorCode = s.ColorCode,
                                TransactionCategoryName = s.TransactionCategoryName,
                                TransactionCategoryId = s.TransactionCategoryId,
                                HasPermission = s.HasPermission,
                                ToUserId = s.ToUserId,
                                IsDeleted = s.IsDeleted,
                                StatusId = s.StatusId,
                                TotalCount = s.TotalCount,
                                HasLinks = s.HasLinks == 1 ? true : false,
                                ToEntityId = s.ToEntityId,
                                MainDocId = s.MainDocId,
                                IsMain = s.IsMain,
                                ConfidentialityId = s.ConfidentialityId,
                                GUID = s.GUID,
                                IsInIc = s.IsInIc,
                                IcName = s.IcName,
                                Description = s.Description,
                                OrderFileNumber=s.OrderFileNumber
                                

                            }).ToList();

            return inboundOutboundLinkedNumberSearchResultDTOList;
        }




    }
}