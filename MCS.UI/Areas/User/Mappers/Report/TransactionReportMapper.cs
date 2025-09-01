using System.Collections.Generic;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;
using MCS.UI.Helpers;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class TransactionReportMapper
    {
        public static List<TransactionGridResultVM> Map(List<TransactionReportResultDTO> transactionReportResultDTOs)
        {
            if (transactionReportResultDTOs == null)
            {
                return new List<TransactionGridResultVM>();
            }
            var result = new List<TransactionGridResultVM>();
            foreach (var item in transactionReportResultDTOs)
            {
                var newData = new TransactionGridResultVM
                {
                    TransactionId = item.TransactionId,
                    EncryptedId = AESEncrytDecry.Base64Encode(item.TransactionId.ToString()),
                    EncryptedIsDraft = item.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) ? AESEncrytDecry.Base64Encode(true.ToString().ToLower()) : AESEncrytDecry.Base64Encode(false.ToString().ToLower()),
                    TransactionTypeId = item.TransactionTypeId,
                    TransactionCategoryText = item.TransactionCategoryText,
                    OrgUnitText = item.OrgUnitText,
                    Date = item.Date,
                    DateText = DateHelper.DateCalendar(item.Date, SessionInfo.CultureShortName),
                    Number = item.Number,
                    TransactioDescription = item.TransactioDescription,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    LetterTypeText = item.LetterTypeText,
                    Remarks = item.Remarks,
                    DeliveryMethodText = item.DeliveryMethodText,
                    Subject = item.Subject,
                    FirstName = item.FirstName,
                    CivilID = item.CivilID,
                    MobileNumber = item.MobileNumber,
                    ExternalPartyText = item.ExternalPartyText,
                    InboundDateH = item.InboundDateH,
                    DocumentNumber = item.DocumentNumber,
                    OutBoundDate = item.OutBoundDate,
                    FromEntityText = item.FromEntityText,
                    FromUserText = item.FromUserText,
                    ToEntityText = item.ToEntityText,
                    ToUserText = item.ToUserText,
                    ToUserId = item.ToUserId,
                    CreatedOn = item.CreatedOn,
                    RemindDate = item.RemindDate,
                    TransactionTypeText = item.TransactionTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    NumberWithDate = item.NumberWithDate,
                    TransactionStatus = item.TransactionStatusText,
                    SavedReason = item.SavedReason,
                    DelayText = item.DelayText,
                    DelayedDaysCount = item.DelayedDaysCount,
                    AssignDate = item.AssignDate,
                    SignedByUserId = item.SignedByUserId,
                    SignedByUserText = item.SignedByUserText,
                    ConfidentialityId = item.ConfidentialityId,
                    LetterNumber = item.LetterNumber
                };
                result.Add(newData);
            }
            return result;
        }

        public static List<TaskGridResultVM> Map(List<TaskReportResultDTO> transactionReportResultDTOs)
        {
            if (transactionReportResultDTOs == null)
            {
                return new List<TaskGridResultVM>();
            }
            var result = new List<TaskGridResultVM>();
            foreach (var item in transactionReportResultDTOs)
            {
                var newData = new TaskGridResultVM
                {
                    TransactionId = item.TransactionId,
                    EncryptedId = AESEncrytDecry.Base64Encode(item.TransactionId.ToString()),
                    TransactionTypeId = item.TransactionTypeId,
                    TransactionCategoryText = item.TransactionCategoryText,
                    Date = item.Date,
                    DateText = DateHelper.DateCalendar(item.Date, SessionInfo.CultureShortName),
                    Number = item.Number,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    LetterTypeText = item.LetterTypeText,
                    FromEntityText = item.FromEntityText,
                    FromUserText = item.FromUserText,
                    ToEntityText = item.ToEntityText,
                    ToUserText = item.ToUserText,
                    ToUserId = item.ToUserId,
                    CreatedOn = item.CreatedOn,
                    RemindDate = item.RemindDate,
                    TransactionTypeText = item.TransactionTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    NumberWithDate = item.NumberWithDate,
                    TransactionStatus = item.TransactionStatusText
                };
                result.Add(newData);
            }
            return result;
        }



        public static List<SentTransactionGridResultVM> Map(List<SentTransactionReportResultDTO> transactionReportResultDTOs)
        {
            if (transactionReportResultDTOs == null)
            {
                return new List<SentTransactionGridResultVM>();
            }
            var result = new List<SentTransactionGridResultVM>();
            foreach (var item in transactionReportResultDTOs)
            {
                var newData = new SentTransactionGridResultVM
                {
                    TransactionId = item.TransactionId,
                    TransactionTypeId = item.TransactionTypeId,
                    TransactionCategoryText = item.TransactionCategoryText,
                    OrgUnitText = item.OrgUnitText,
                    AssignedDate = DateHelper.DateCalendar(item.AssignedDate, SessionInfo.CultureShortName),
                    TransactionDate = DateHelper.DateCalendar(item.TransactionDate, SessionInfo.CultureShortName),
                    Number = item.Number,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    Subject = item.Subject,
                    FromEntityText = item.FromEntityText,
                    ToEntityText = item.ToEntityText,
                    TransactionTypeText = item.TransactionTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    NumberWithDate = item.Number + "-" + DateHelper.DateCalendar(item.TransactionDate.AddDays(-1), SessionInfo.CultureShortName),
                    TransactionStatus = item.TransactionStatusText,
                    TransactionElcOwner = item.TransactionElcOwner,
                    TransactionPhysicalOwner = item.TransactionPhysicalOwner, 
                    SentStatus = item.Viewed ? "تم الاستلام" : "غير مستلمة",

    };
                result.Add(newData);
            }
            return result;
        }

        public static List<FollowupGridResultVM> Map(List<FollowupReportResultDTO> transactionReportResultDTOs)
        {
            if (transactionReportResultDTOs == null)
            {
                return new List<FollowupGridResultVM>();
            }
            var result = new List<FollowupGridResultVM>();
            foreach (var item in transactionReportResultDTOs)
            {
                var newData = new FollowupGridResultVM
                {
                    TransactionId = item.TransactionId,
                    TransactionCategoryText = item.TransactionCategoryText,
                    OrgUnitText = item.OrgUnitText,
                    Date = item.Date,
                    DateText = DateHelper.DateCalendar(item.Date.AddDays(-1), SessionInfo.CultureShortName),
                    Number = item.Number,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    LetterTypeText = item.LetterTypeText,
                    FromEntityText = item.FromEntityText,
                    FromUserText = item.FromUserText,
                    ToEntityText = item.ToEntityText,
                    ToUserText = item.ToUserText,
                    ToUserId = item.ToUserId,
                    CreatedOn = item.CreatedOn,
                    RemindDate = item.RemindDate,
                    TransactionTypeText = item.TransactionTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    NumberWithDate = item.NumberWithDate,
                    TransactionStatus = item.TransactionStatusText,
                    Subject = item.Subject,
                    GeneralExplanation = item.GeneralExplanation,
                };
                result.Add(newData);
            }
            return result;
        }


        public static List<PerformanceMeasurementGridResultVM> Map(List<PerformanceMeasurementReportResultDTO> transactionReportResultDTOs)
        {
            if (transactionReportResultDTOs == null)
            {
                return new List<PerformanceMeasurementGridResultVM>();
            }
            var result = new List<PerformanceMeasurementGridResultVM>();
            foreach (var item in transactionReportResultDTOs)
            {
                var newData = new PerformanceMeasurementGridResultVM();
                newData.OrgUnitsID = item.OrgUnitsID;
                newData.OrgUnitName = item.OrgUnitName;
                newData.UserProfilesID = item.UserProfilesID;
                newData.UserProfileName = item.UserProfileName;
                newData.OutboundCount = item.OutboundCount;
                newData.OutboundDraftCountCreated = item.OutboundDraftCountCreated;
                newData.OutboundDraftCountAssigned = item.OutboundDraftCountAssigned;
                newData.InboundCountCreated = item.InboundCountCreated;
                newData.InboundCountAssigned = item.InboundCountAssigned;
                newData.InternalOutboundCountCreated = item.InternalOutboundCountCreated;
                newData.InternalOutboundCountAssigned = item.InternalOutboundCountAssigned;
                newData.DelayedCount = item.DelayedCount;
                newData.FinishedCount = item.FinishedCount;
                newData.SavedCount = item.SavedCount;
                newData.AssignedCount = item.AssignedCount;
                newData.InProgressCount = item.InProgressCount;
                result.Add(newData);
            }
            return result;
        }
    }
}