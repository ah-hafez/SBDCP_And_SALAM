using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ReportMapper
    {
        public static SearchCriteriaTransactionReport Map(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            if (searchCriteriaTransactionReportDTO == null)
            {
                return new SearchCriteriaTransactionReport();
            }
            var result = new SearchCriteriaTransactionReport();
            result.Number = searchCriteriaTransactionReportDTO.Number;
            result.Subject = searchCriteriaTransactionReportDTO.Subject;
            result.TransactionCategoryId = searchCriteriaTransactionReportDTO.TransactionCategory;
            result.From = searchCriteriaTransactionReportDTO.From;
            result.To = searchCriteriaTransactionReportDTO.To;

            result.TransactionTypeId = searchCriteriaTransactionReportDTO.TransactionTypeId;
            result.IsAppointment = searchCriteriaTransactionReportDTO.IsAppointment;
            result.AppointmentDate = searchCriteriaTransactionReportDTO.AppointmentDate;
            result.PriorityLevelId = searchCriteriaTransactionReportDTO.PriorityLevelId;
            result.ConfidentialityLevelId = searchCriteriaTransactionReportDTO.ConfidentialityLevelId;
            result.LetterTypeId = searchCriteriaTransactionReportDTO.LetterTypeId;
            result.Remarks = searchCriteriaTransactionReportDTO.Remarks;
            result.DeliveryMethodId = searchCriteriaTransactionReportDTO.DeliveryMethodId;
            result.TransactionStatusId = searchCriteriaTransactionReportDTO.TransactionStatusId;

            result.FullName = searchCriteriaTransactionReportDTO.FullName;
            result.CivilID = searchCriteriaTransactionReportDTO.CivilID;
            result.MobileNumber = searchCriteriaTransactionReportDTO.MobileNumber;

            result.IsForIndividual = searchCriteriaTransactionReportDTO.IsForIndividual;
            result.DestinationId = searchCriteriaTransactionReportDTO.DestinationId;
            result.InboundDocumentNumber = searchCriteriaTransactionReportDTO.InboundDocumentNumber;
            result.InboundDateH = searchCriteriaTransactionReportDTO.InboundDateH;
            result.OutboundDateH = searchCriteriaTransactionReportDTO.OutboundDateH;

            result.FromOrgUnitId = searchCriteriaTransactionReportDTO.FromOrgUnitId;
            result.ToOrgUnitId = searchCriteriaTransactionReportDTO.ToOrgUnitId;
            result.FromEmployeeId = searchCriteriaTransactionReportDTO.FromEmployeeId;
            result.ToEmployeeId = searchCriteriaTransactionReportDTO.ToEmployeeId;

            result.CultureName = searchCriteriaTransactionReportDTO.CultureName;
            result.PageIndex = searchCriteriaTransactionReportDTO.PageIndex;
            result.PageSize = searchCriteriaTransactionReportDTO.PageSize;
            

            result.IsPrint = searchCriteriaTransactionReportDTO.IsPrint;
            result.TotalCount = searchCriteriaTransactionReportDTO.TotalCount;

            result.EntityId = searchCriteriaTransactionReportDTO.EntityId;
            result.UserId = searchCriteriaTransactionReportDTO.UserId;
            result.Level = searchCriteriaTransactionReportDTO.Level;
            return result;
        }

        public static List<TransactionReportResultDTO> Map(List<TransactionReportResult> transactionReportResult)
        {
            if (transactionReportResult == null)
            {
                return new List<TransactionReportResultDTO>();
            }
            var TransactionReportResultDTOs = new List<TransactionReportResultDTO>();
            foreach (var item in transactionReportResult)
            {
                var newData = new TransactionReportResultDTO
                {
                    TransactionId = item.TransactionId,
                    TransactionTypeId = item.TransactionTypeId,
                    OrgUnitText = item.OrgUnitText,
                    Date = item.Date,
                    Number = item.Number.Value,
                    TransactioDescription = item.TransactioDescription,
                    TransactionCategoryText = item.TransactionCategoryText,
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
                    ToUserId = item.ToUserId.HasValue ? item.ToUserId.Value : -1,
                    CreatedOn = item.CreatedOn,
                    TransactionTypeText = item.TransactionTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    RemindDate = item.RemindDate,
                    TransactionStatusText = item.TransactionStatusText,
                    SavedReason = item.SavedReason,
                    DelayText = item.DelayText,
                    NumberWithDate = (item.Number.HasValue ? item.Number.Value.ToString() : "0") + "-" + item.Date.ToShortDateString(),
                    DelayedDaysCount = item.DelayedDaysCount.ToString(),
                    AssignDate = item.AssignDate,
                    SignedByUserId = item.SignedByUserId,
                    SignedByUserText = item.SignedByUserText,
                    ConfidentialityId = item.ConfidentialityId,
                    LetterNumber = item.LetterNumber
                };
                TransactionReportResultDTOs.Add(newData);
            }
            return TransactionReportResultDTOs;
        }

        public static List<SentTransactionReportResultDTO> Map(List<SentTransactionReportResult> transactionReportResult)
        {
            if (transactionReportResult == null)
            {
                return new List<SentTransactionReportResultDTO>();
            }
            var TransactionReportResultDTOs = new List<SentTransactionReportResultDTO>();
            foreach (var item in transactionReportResult)
            {
                var newData = new SentTransactionReportResultDTO
                {
                    TransactionId = item.TransactionId,
                    TransactionTypeId = item.TransactionTypeId,
                    OrgUnitText = item.OrgUnitText,
                    Number = item.Number.Value,
                    TransactionCategoryText = item.TransactionCategoryText,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    TransactionTypeText = item.TransactionTypeText,
                    Subject = item.Subject,
                    FromEntityText = item.FromEntityText,
                    ToEntityText = item.ToEntityText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    TransactionStatusText = item.TransactionStatusText,
                    AssignedDate = item.AssignedDate,
                    TransactionDate = item.TransactionDate,
                    TransactionElcOwner = item.TransactionElcOwner != null ? item.TransactionElcOwner : "استقبال الإدارة",
                    TransactionPhysicalOwner = item.TransactionPhysicalOwner,
                    Viewed = item.Viewed,
                 
    };
                TransactionReportResultDTOs.Add(newData);
            }
            return TransactionReportResultDTOs;
        }

        public static List<TaskReportResultDTO> Map(List<TaskReportResult> transactionReportResult)
        {
            if (transactionReportResult == null)
            {
                return new List<TaskReportResultDTO>();
            }
            var TransactionReportResultDTOs = new List<TaskReportResultDTO>();
            foreach (var item in transactionReportResult)
            {
                var newData = new TaskReportResultDTO
                {
                    TransactionId = item.TransactionId,
                    TransactionTypeId = item.TransactionTypeId,

                    Date = item.Date,
                    Number = item.Number.Value,
                    TransactionCategoryText = item.TransactionCategoryText,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    LetterTypeText = item.TransactionTypeText,
                    FromEntityText = item.FromEntityText,
                    FromUserText = item.FromUserText,
                    ToEntityText = item.ToEntityText,
                    ToUserText = item.ToUserText,
                    ToUserId = item.ToUserId.HasValue ? item.ToUserId.Value : -1,
                    CreatedOn = item.CreatedOn,
                    TransactionTypeText = item.LetterTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    RemindDate = item.RemindDate,
                    TransactionStatusText = item.TransactionStatusText,
                    NumberWithDate = (item.Number.HasValue ? item.Number.Value.ToString() : "0") + "-" + item.Date.ToShortDateString()


                };
                TransactionReportResultDTOs.Add(newData);
            }
            return TransactionReportResultDTOs;
        }

        public static List<FollowupReportResultDTO> Map(List<FollowupReportResult> transactionReportResult)
        {
            if (transactionReportResult == null)
            {
                return new List<FollowupReportResultDTO>();
            }
            var TransactionReportResultDTOs = new List<FollowupReportResultDTO>();
            foreach (var item in transactionReportResult)
            {
                var newData = new FollowupReportResultDTO
                {
                    TransactionId = item.TransactionId,
                    OrgUnitText = item.OrgUnitText,
                    Date = item.Date,
                    Number = item.Number.Value,
                    TransactionCategoryText = item.TransactionCategoryText,
                    ConfidentialityText = item.ConfidentialityText,
                    PriorityText = item.PriorityText,
                    LetterTypeText = item.LetterTypeText,
                    FromEntityText = item.FromEntityText,
                    FromUserText = item.FromUserText,
                    ToEntityText = item.ToEntityText,
                    ToUserText = item.ToUserText,
                    ToUserId = item.ToUserId.HasValue ? item.ToUserId.Value : -1,
                    CreatedOn = item.CreatedOn,
                    TransactionTypeText = item.LetterTypeText,
                    TransactionCategoryId = item.TransactionCategoryId,
                    RemindDate = item.RemindDate,
                    TransactionStatusText = item.TransactionStatusText,
                    NumberWithDate = (item.Number.HasValue ? item.Number.Value.ToString() : "0") + "-" + item.Date.ToShortDateString(),
                    Subject = item.Subject,
                    GeneralExplanation = item.GeneralExplanation

                };
                TransactionReportResultDTOs.Add(newData);
            }
            return TransactionReportResultDTOs;
        }

        public static SearchCriteriaPerformanceMeasurementReport Map(SearchCriteriaPerformanceMeasurementDTO searchCriteriaTransactionReportDTO)
        {
            if (searchCriteriaTransactionReportDTO == null)
            {
                return new SearchCriteriaPerformanceMeasurementReport();
            }
            var result = new SearchCriteriaPerformanceMeasurementReport
            {
                ReportType = searchCriteriaTransactionReportDTO.ReportType,
                EmployeeId = searchCriteriaTransactionReportDTO.EmployeeId,
                OrgUnitId = searchCriteriaTransactionReportDTO.OrgUnitId,
                From = searchCriteriaTransactionReportDTO.From,
                To = searchCriteriaTransactionReportDTO.To,
                Level = searchCriteriaTransactionReportDTO.Level,

                LetterTypeId = searchCriteriaTransactionReportDTO.LetterTypeId,
                IsAppointment = searchCriteriaTransactionReportDTO.IsAppointment,
                AppointmentDate = searchCriteriaTransactionReportDTO.AppointmentDate,
                PriorityLevelId = searchCriteriaTransactionReportDTO.PriorityLevelId,
                ConfidentialityLevelId = searchCriteriaTransactionReportDTO.ConfidentialityLevelId,
                TransactionTypeId = searchCriteriaTransactionReportDTO.TransactionTypeId,
                Remarks = searchCriteriaTransactionReportDTO.Remarks,
                DeliveryMethodId = searchCriteriaTransactionReportDTO.DeliveryMethodId,

                CultureName = searchCriteriaTransactionReportDTO.CultureName,
                PageIndex = searchCriteriaTransactionReportDTO.PageIndex,
                PageSize = searchCriteriaTransactionReportDTO.PageSize,

                IsPrint = searchCriteriaTransactionReportDTO.IsPrint,
                TotalCount = searchCriteriaTransactionReportDTO.TotalCount
            };
            return result;
        }

        public static List<PerformanceMeasurementReportResultDTO> Map(List<PerformanceMeasurementReportResult> transactionReportResult)
        {
            if (transactionReportResult == null)
            {
                return new List<PerformanceMeasurementReportResultDTO>();
            }
            var TransactionReportResultDTOs = new List<PerformanceMeasurementReportResultDTO>();
            foreach (var item in transactionReportResult)
            {
                var newData = new PerformanceMeasurementReportResultDTO();
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
                TransactionReportResultDTOs.Add(newData);
            }
            return TransactionReportResultDTOs;
        }
    }
}