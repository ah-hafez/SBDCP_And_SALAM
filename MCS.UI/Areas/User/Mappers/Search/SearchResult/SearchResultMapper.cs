using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Encryption;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Search;
using MCS.UI.Areas.User.Models.File;
using System;

namespace MCS.UI.Areas.User.Mappers.Search
{
    public static class SearchResultMapper
    {

        public static List<InboundSearchResultVM> Map(List<InboundSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<InboundSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new InboundSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    DeliveryMethodId = searchResultDTO.DeliveryMethodId,
                    Encrypted = searchResultDTO.Encrypted,
                    DocumentNumber = searchResultDTO.DocumentNumber,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<InboundSearchResultVM>();
        }



        public static List<SearchICTransactionResultVM> Map(List<ICSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchICTransactionResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchICTransactionResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    MainDocId = searchResultDTO.MainDocId,
                    IsMain = searchResultDTO.IsMain,
                    GUID = searchResultDTO.GUID,
                    IsInIc = searchResultDTO.IsInIc,
                    IcName = searchResultDTO.IcName,
                    Description = searchResultDTO.Description,
                    OrderFileNumber = searchResultDTO.OrderFileNumber,
                    ModifiedUser = searchResultDTO.ModifiedUser,
                    FullClassificationName = searchResultDTO.FullClassificationName,
                    Part=searchResultDTO.Part





                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchICTransactionResultVM>();
        }




        private static TransactionAssignmentInfoVM Map(TransactionAssignmentDTO transactionAssignmentDTO)
        {
            if (transactionAssignmentDTO == null || transactionAssignmentDTO.ActionName == null)
            {
                return new TransactionAssignmentInfoVM();
            }

            TransactionAssignmentInfoVM transactionAssignmentInfoVM = new TransactionAssignmentInfoVM()
            {
                FromUser = transactionAssignmentDTO.FromUserName,
                FromEntity = transactionAssignmentDTO.FromOrgUnitName,
                Date = transactionAssignmentDTO.Date,
                Action = transactionAssignmentDTO.ActionName
            };
            return transactionAssignmentInfoVM;
        }

        public static List<OutboundInternalSearchResultVM> Map(List<OutboundInternalSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundInternalSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new OutboundInternalSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<OutboundInternalSearchResultVM>();
        }
        public static List<OutboundSearchResultVM> Map(List<OutboundSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundSearchResultVM> outboundSearchResultVMs = searchResultDTOs.Select(searchResultDTO => new OutboundSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    TotalCount = searchResultDTO.TotalCount,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    DeliveryMethodId = searchResultDTO.DeliveryMethodId,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,

                }).ToList();

                return outboundSearchResultVMs;
            }
            return new List<OutboundSearchResultVM>();
        }
        public static List<OutboundDraftSearchResultVM> Map(List<OutboundDraftSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundDraftSearchResultVM> outboundSearchResultVMs = searchResultDTOs.Select(searchResultDTO => new OutboundDraftSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return outboundSearchResultVMs;
            }
            return new List<OutboundDraftSearchResultVM>();
        }
        public static List<SubjectSearchResultVM> Map(IList<SubjectSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SubjectSearchResultVM> searchResultVM = searchResultDTOs.Select(searchResultDTO => new SubjectSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,

                }).ToList();

                return searchResultVM;
            }
            return new List<SubjectSearchResultVM>();
        }
        public static List<BarcodeSearchResultVM> Map(IList<BarcodeSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<BarcodeSearchResultVM> searchResultVMs = searchResultDTOs.Select(searchResultDTO => new BarcodeSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    IsDeleted = searchResultDTO.IsDeleted,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,

                }).ToList();

                return searchResultVMs;
            }
            return new List<BarcodeSearchResultVM>();
        }
        public static List<EntitySearchResultVM> Map(List<EntitySearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<EntitySearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new EntitySearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<EntitySearchResultVM>();
        }
        public static List<CreatorSearchResultVM> Map(List<CreatorSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<CreatorSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new CreatorSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<CreatorSearchResultVM>();
        }

        public static List<AssignTransactionSearchResultVM> Map(List<AssignTransactionSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<AssignTransactionSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new AssignTransactionSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<AssignTransactionSearchResultVM>();
        }


        public static List<InboundSearchResultVM> MapBaseToInbound(List<BaseSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<InboundSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new InboundSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<InboundSearchResultVM>();
        }
        public static List<OutboundInternalSearchResultVM> MapBaseToOutboundInternal(List<BaseSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundInternalSearchResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new OutboundInternalSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<OutboundInternalSearchResultVM>();
        }
        public static List<OutboundSearchResultVM> MapBaseToOutbound(List<BaseSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundSearchResultVM> outboundSearchResultVMs = searchResultDTOs.Select(searchResultDTO => new OutboundSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    TotalCount = searchResultDTO.TotalCount,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    DeliveryMethodId = searchResultDTO.DeliveryMethodId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return outboundSearchResultVMs;
            }
            return new List<OutboundSearchResultVM>();
        }
        public static List<OutboundDraftSearchResultVM> MapBaseToOutboundDraft(List<BaseSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<OutboundDraftSearchResultVM> outboundSearchResultVMs = searchResultDTOs.Select(searchResultDTO => new OutboundDraftSearchResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return outboundSearchResultVMs;
            }
            return new List<OutboundDraftSearchResultVM>();
        }
        public static List<InquirySearchResultVM> Map(List<InquirySearchResultDTO> searchResultDTOs)
        {
            if (searchResultDTOs != null)
            {
                List<InquirySearchResultVM> inquirySearchResultVM = searchResultDTOs.Select(searchResultDTO => new InquirySearchResultVM
                {
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    ToEntity = searchResultDTO.ToEntity,
                    ToUser = searchResultDTO.ToUser,
                    ToUserId = searchResultDTO.ToUserID,
                    HasPermission = searchResultDTO.HasPermission,
                    TransactionTypeId = searchResultDTO.TransactionTypeId
                }).ToList();

                return inquirySearchResultVM;
            }
            return new List<InquirySearchResultVM>();
        }
        public static List<SearchCriteriaByNamesResultVM> Map(List<NamesSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByNamesResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByNamesResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByNamesResultVM>();
        }

        public static List<SearchCriteriaBySubjectLetterResultVM> Map(List<SubjectLetterSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaBySubjectLetterResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaBySubjectLetterResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaBySubjectLetterResultVM>();
        }

        public static List<SearchCriteriaByDailyResultVM> Map(List<DailySearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByDailyResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByDailyResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByDailyResultVM>();
        }
        public static List<SearchCriteriaByAssignmentNoteResultVM> Map(List<AssignmentNoteSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByAssignmentNoteResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByAssignmentNoteResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByAssignmentNoteResultVM>();
        }
        public static List<SearchCriteriaByManifestNumberResultVM> Map(List<ManifestNumberSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByManifestNumberResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByManifestNumberResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByManifestNumberResultVM>();
        }
        public static List<SearchCriteriaByMilitaryNumberOrIdentityResultVM> Map(List<MilitaryNumberOrIdentitySearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByMilitaryNumberOrIdentityResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByMilitaryNumberOrIdentityResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByMilitaryNumberOrIdentityResultVM>();
        }
        public static List<SearchCriteriaByTransactionNotsResultVM> Map(List<TransactionNotsSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByTransactionNotsResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByTransactionNotsResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByTransactionNotsResultVM>();
        }
        public static List<SearchCriteriaByElcEmployeeResultVM> Map(List<ELcEmployeeSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByElcEmployeeResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByElcEmployeeResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByElcEmployeeResultVM>();
        }
        public static List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM> Map(List<ExternalOutBoundOrManifestNumberSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByExternalOutBoundOrManifestNumberResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM>();
        }
        public static List<SearchCriteriaByCopyAssignemntResultVM> Map(List<CopyAssignemntSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByCopyAssignemntResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByCopyAssignemntResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByCopyAssignemntResultVM>();
        }

        public static List<SearchCriteriaByTransactionNumberResultVM> Map(List<TransactionNumberSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByTransactionNumberResultVM> inboundSearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByTransactionNumberResultVM
                {
                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Id = searchResultDTO.Id,
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ToEntityId = searchResultDTO.ToEntityId,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    Encrypted = searchResultDTO.Encrypted,
                }).ToList();

                return inboundSearchResultVM;
            }
            return new List<SearchCriteriaByTransactionNumberResultVM>();
        }

        public static List<SearchCriteriaByExternalPartyCopiesResultVM> Map(List<ExternalPartyCopiesSearchResultDTO> searchResultDTOs, bool HasPermissionSearch)
        {

            if (searchResultDTOs != null)
            {
                List<SearchCriteriaByExternalPartyCopiesResultVM> copySearchResultVM = searchResultDTOs.Select(searchResultDTO => new SearchCriteriaByExternalPartyCopiesResultVM
                {

                    ColorCode = searchResultDTO.ColorCode,
                    ConfidentialityName = searchResultDTO.ConfidentialityName,
                    Date = searchResultDTO.Date,
                    DateH = searchResultDTO.DateH,
                    Id = searchResultDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(searchResultDTO.Id.ToString()),
                    Number = searchResultDTO.Number,
                    OrgUnitName = searchResultDTO.OrgUnitName,
                    PartyName = searchResultDTO.PartyName,
                    PriorityName = searchResultDTO.PriorityName,
                    StatusName = searchResultDTO.StatusName,
                    Subject = searchResultDTO.Subject,
                    TransactionCategoryId = searchResultDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = searchResultDTO.TransactionCategoryId.ToString(),
                    TransactionCategoryName = searchResultDTO.TransactionCategoryName,
                    TransactionType = searchResultDTO.TransactionType,
                    WithArchiving = searchResultDTO.WithArchiving,
                    HasPermission = HasPermissionSearch ? true : searchResultDTO.HasPermission,
                    ToUserId = searchResultDTO.ToUserId,
                    StatusId = searchResultDTO.StatusId,
                    IsDeleted = searchResultDTO.IsDeleted,
                    TotalCount = searchResultDTO.TotalCount,
                    HasLinks = searchResultDTO.HasLinks,
                    ConfidentialityId = searchResultDTO.ConfidentialityId,
                    RemindDate = searchResultDTO.RemindDate,
                    RemindDateH = searchResultDTO.RemindDateH,
                    TransactionTypeId = searchResultDTO.TransactionTypeId,
                    TransactionAssignmentInfoVM = Map(searchResultDTO.TransactionAssignmentDTO),
                    ToEntityId = searchResultDTO.ExternalPartyId,
                    Encrypted = searchResultDTO.Encrypted
                }).ToList();

                return copySearchResultVM;
            }
            return new List<SearchCriteriaByExternalPartyCopiesResultVM>();
        }

    }
}