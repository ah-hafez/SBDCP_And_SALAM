using MCS.Framework.Encryption;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.File;

namespace MCS.UI.Areas.User.Mappers.File
{
    public static class TransactionDetailsInfoMapper
    {
        public static TransactionDetailsInfoVM Map(TransactionDetailsInfoDTO transactionDetailsInfoDTO)
        {
            if (transactionDetailsInfoDTO != null)
            {
                TransactionDetailsInfoVM transactionDetailsInfoVM = new TransactionDetailsInfoVM()
                {
                    ConfidentialityId = transactionDetailsInfoDTO.ConfidentialityId,
                    ConfidentialityName = transactionDetailsInfoDTO.ConfidentialityName,
                    Date = transactionDetailsInfoDTO.Date,
                    DateH = transactionDetailsInfoDTO.DateH,
                    DocumentNumber = transactionDetailsInfoDTO.DocumentNumber,
                    EntityName = transactionDetailsInfoDTO.EntityName,
                    ExternalPartyId = transactionDetailsInfoDTO.ExternalPartyId,
                    ExternalPartyManagerId = transactionDetailsInfoDTO.ExternalPartyManagerId,
                    ExternalPartyManagerName = transactionDetailsInfoDTO.ExternalPartyManagerName,
                    ExternalPartyName = transactionDetailsInfoDTO.ExternalPartyName,
                    Id = transactionDetailsInfoDTO.Id,
                    EncryptedId = AESEncrytDecry.Base64Encode(transactionDetailsInfoDTO.Id.ToString()),
                    IsLate = transactionDetailsInfoDTO.IsLate,
                    LetterTypeName = transactionDetailsInfoDTO.LetterTypeName,
                    Number = transactionDetailsInfoDTO.Number,
                    PriorityId = transactionDetailsInfoDTO.PriorityId,
                    PriorityName = transactionDetailsInfoDTO.PriorityName,
                    Remarks = transactionDetailsInfoDTO.Remarks,
                    RemindDate = transactionDetailsInfoDTO.RemindDate,
                    RemindDateH = transactionDetailsInfoDTO.RemindDateH,
                    SignedByOrgUnitId = transactionDetailsInfoDTO.SignedByOrgUnitId,
                    SignedByOrgUnitName = transactionDetailsInfoDTO.SignedByOrgUnitName,
                    SignedByUserId = transactionDetailsInfoDTO.SignedByUserId,
                    SignedByUserName = transactionDetailsInfoDTO.SignedByUserName,
                    TransactionTypeColorId = transactionDetailsInfoDTO.TransactionTypeColorId,
                    TransactionTypeId = transactionDetailsInfoDTO.TransactionTypeId,
                    TransactionTypeName = transactionDetailsInfoDTO.TransactionTypeName,
                    Status = transactionDetailsInfoDTO.Status,
                    StatusId = transactionDetailsInfoDTO.StatusId,
                    RejectionReason = transactionDetailsInfoDTO.RejectionReason,
                    Subject = transactionDetailsInfoDTO.Subject,
                    ToEntityName = transactionDetailsInfoDTO.ToEntityName,
                    ToUserName = transactionDetailsInfoDTO.ToUserName,
                    TransactionCategory = transactionDetailsInfoDTO.TransactionCategory,
                    TransactionCategoryId = transactionDetailsInfoDTO.TransactionCategoryId,
                    EncryptedTransactionCategoryId = AESEncrytDecry.Base64Encode(transactionDetailsInfoDTO.TransactionCategoryId.ToString()),
                    User = transactionDetailsInfoDTO.User,
                    UserId = transactionDetailsInfoDTO.UserId,
                    ToUserId = transactionDetailsInfoDTO.ToUserId,
                    Year = transactionDetailsInfoDTO.Year,
                    YasserRegistered = transactionDetailsInfoDTO.YasserRegistered,
                    AttachmentCount = transactionDetailsInfoDTO.AttachmentCount,
                    HasPermission = transactionDetailsInfoDTO.HasPermission,
                    SavedReason = transactionDetailsInfoDTO.SavedReason,
                    DeliveryMethodId = transactionDetailsInfoDTO.DeliveryMethodId,
                    TransactionPathId = transactionDetailsInfoDTO.TransactionPathId,
                    IsIndividual = transactionDetailsInfoDTO.IsIndividual,
                    DeliveryMethodName = transactionDetailsInfoDTO.DeliveryMethodName,
                    FollowupDate = transactionDetailsInfoDTO.FollowupDate,
                    FollowupDateH = transactionDetailsInfoDTO.FollowupDateH,
                    EncryptedIsDraft = transactionDetailsInfoDTO.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) ? AESEncrytDecry.Base64Encode(true.ToString()) : AESEncrytDecry.Base64Encode(false.ToString()),
                    HasLinks = transactionDetailsInfoDTO.HasLinks,
                    CopyStatus = transactionDetailsInfoDTO.StatusId,
                    PrivecyName = transactionDetailsInfoDTO.PrivecyName,
                    PrivecyId = transactionDetailsInfoDTO.PrivecyId,
                    isDeleted = transactionDetailsInfoDTO.isDeleted,
                    IsPresentationDraft= transactionDetailsInfoDTO.IsPresentationDraft,
                    IsElcOutBound = transactionDetailsInfoDTO.IsElcOutBound,
                    SpecialCopy = transactionDetailsInfoDTO.SpecialCopy,
                    IsBcc = transactionDetailsInfoDTO.IsBcc,
                    IsOpr = transactionDetailsInfoDTO.IsOpr,
                    OprEntityId = transactionDetailsInfoDTO.OprEntityId,
                    OprEntityName = transactionDetailsInfoDTO.OprEntityName,
                    isImportant = transactionDetailsInfoDTO.IsImportant,
                    TransactionCopyId = transactionDetailsInfoDTO.TransactionCopyId,
                    HasTask = transactionDetailsInfoDTO.HasTask,
                    Encrypted = transactionDetailsInfoDTO.Encrypted

                };

                return transactionDetailsInfoVM;
            }
            return new TransactionDetailsInfoVM();
        }

        public static TransactionDetailsInfoDTO Map(TransactionDetailsInfoVM transactionDetailsInfoVM)
        {
            if (transactionDetailsInfoVM != null)
            {
                TransactionDetailsInfoDTO transactionDetailsInfoDTO = new TransactionDetailsInfoDTO()
                {
                    ConfidentialityId = transactionDetailsInfoVM.ConfidentialityId,
                    ConfidentialityName = transactionDetailsInfoVM.ConfidentialityName,
                    Date = transactionDetailsInfoVM.Date,
                    DateH = transactionDetailsInfoVM.DateH,
                    DocumentNumber = transactionDetailsInfoVM.DocumentNumber,
                    EntityName = transactionDetailsInfoVM.EntityName,
                    ExternalPartyId = transactionDetailsInfoVM.ExternalPartyId,
                    ExternalPartyManagerId = transactionDetailsInfoVM.ExternalPartyManagerId,
                    ExternalPartyManagerName = transactionDetailsInfoVM.ExternalPartyManagerName,
                    ExternalPartyName = transactionDetailsInfoVM.ExternalPartyName,
                    Id = transactionDetailsInfoVM.Id,
                    IsLate = transactionDetailsInfoVM.IsLate,
                    LetterTypeName = transactionDetailsInfoVM.LetterTypeName,
                    Number = transactionDetailsInfoVM.Number,
                    PriorityId = transactionDetailsInfoVM.PriorityId,
                    PriorityName = transactionDetailsInfoVM.PriorityName,
                    Remarks = transactionDetailsInfoVM.Remarks,
                    RemindDate = transactionDetailsInfoVM.RemindDate,
                    RemindDateH = transactionDetailsInfoVM.RemindDateH,
                    SignedByOrgUnitId = transactionDetailsInfoVM.SignedByOrgUnitId,
                    SignedByOrgUnitName = transactionDetailsInfoVM.SignedByOrgUnitName,
                    SignedByUserId = transactionDetailsInfoVM.SignedByUserId,
                    SignedByUserName = transactionDetailsInfoVM.SignedByUserName,
                    TransactionTypeColorId = transactionDetailsInfoVM.TransactionTypeColorId,
                    TransactionTypeId = transactionDetailsInfoVM.TransactionTypeId,
                    TransactionTypeName = transactionDetailsInfoVM.TransactionTypeName,
                    Status = transactionDetailsInfoVM.Status,
                    Subject = transactionDetailsInfoVM.Subject,
                    ToEntityName = transactionDetailsInfoVM.ToEntityName,
                    ToUserName = transactionDetailsInfoVM.ToUserName,
                    TransactionCategory = transactionDetailsInfoVM.TransactionCategory,
                    TransactionCategoryId = transactionDetailsInfoVM.TransactionCategoryId,
                    User = transactionDetailsInfoVM.User,
                    UserId = transactionDetailsInfoVM.UserId,
                    ToUserId = transactionDetailsInfoVM.ToUserId,
                    Year = transactionDetailsInfoVM.Year,
                    AttachmentCount = transactionDetailsInfoVM.AttachmentCount,
                    HasPermission = transactionDetailsInfoVM.HasPermission,
                    SavedReason = transactionDetailsInfoVM.SavedReason,
                    isDeleted = transactionDetailsInfoVM.isDeleted,
                };

                return transactionDetailsInfoDTO;
            }
            return new TransactionDetailsInfoDTO();
        }

    }
}