using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Localization.SupportClasses;

namespace MCS.Service.Mappers
{
    public class TransactionDetailsMapper
    {
        public static TransactionDetailsDTO Map(TransactionDetails transactionDetails)
        {
            if (transactionDetails != null)
            {
                TransactionDetailsDTO transactionDetailsDTO = new TransactionDetailsDTO()
                {
                    Date = transactionDetails.Date,
                    Id = transactionDetails.Id,
                    Number = transactionDetails.Number,
                    HijriDate = transactionDetails.DateH,
                };

                return transactionDetailsDTO;
            }
            return null;
        }

        public static TransactionDetails Map(TransactionDetailsDTO transactionDetailsDTO)
        {
            if (transactionDetailsDTO != null)
            {
                TransactionDetails transactionDetails = new TransactionDetails()
                {
                    Date = transactionDetailsDTO.Date,
                    Id = transactionDetailsDTO.Id,
                    Number = transactionDetailsDTO.Number,
                    DateH = transactionDetailsDTO.HijriDate,

                };

                return transactionDetails;
            }
            return null;
        }
        public static List<TransactionDetailsDTO> Map(List<Transaction> transactionList)
        {
            List<TransactionDetailsDTO> transactionDetailsDTOList = new List<TransactionDetailsDTO>();
            foreach (var transaction in transactionList)
            {
                transactionDetailsDTOList.Add(
                    Map(transaction)
                    );
            }
            return transactionDetailsDTOList;
        }
        public static TransactionDetailsDTO Map(Transaction transaction)
        {
            TransactionDetailsDTO transactionDetailsDTO = new TransactionDetailsDTO
            {
                Id = transaction.Id,
                Number = transaction.Number,
                TransactionCategoryId = transaction.TransactionCategoryId, //نوع المعاملة
                CreatedOn = transaction.CreatedOn,
                LetterTypeId = transaction.LetterTypeId, // نوع الخطاب
                PriorityId = transaction.PriorityId,
                ConfidentialityId = transaction.ConfidentialityId,
                HijriDate = transaction.DateH,
                Year=transaction.YearH,
                Confidentiality = transaction.Confidentiality != null ? transaction.Confidentiality.LocalName : "",
                Priority = transaction.Priority != null ? transaction.Priority.Text : "",
                Date = transaction.Date,
                privacyLevelId = transaction.PrivecyId,
                Privacy = transaction.Privecy != null ? transaction.Privecy.Text : "",
                TransactionType = transaction.TransactionType != null ? transaction.TransactionType.Text : "",
                InboundDateH = transaction.InboundDateH,
                ToOrgUnitId = transaction.OrgUnitId.ToString(),
                InboundNumber = transaction.DocumentNumber,
                ReminderDate = transaction?.RemindDate + transaction?.RemindDateH,
                //SubjectClassifications = transaction.SubjectClassifications
                Subject = transaction.Subject,
                LetterNumber = transaction.LetterNumber,
                FromOrgUnitId = transaction.ExternalPartyId.ToString(),
                FromOrgUnit = (transaction.ExternalParty != null) ? transaction.ExternalParty.LocalName : string.Empty,
                LetterType = transaction.LetterType != null ? transaction.LetterType.Text : null,
                ToUserId = transaction.ToUserId,
            };
            return transactionDetailsDTO;
        }

        public static List<TransactionDetailsDTO> Map(List<DashboardTransactionDetails> dashboardTransactionDetailsList)
        {
            List<TransactionDetailsDTO> transactionDetailsDTOList = new List<TransactionDetailsDTO>();

            foreach (var dashboardTransactionDetails in dashboardTransactionDetailsList)
            {
                transactionDetailsDTOList.Add(Map(dashboardTransactionDetails));
            }
            return transactionDetailsDTOList;
        }

        private static TransactionDetailsDTO Map(DashboardTransactionDetails dashboardTransactionDetails)
        {
            TransactionDetailsDTO transactionDetailsDTO = new TransactionDetailsDTO
            {
                Id = dashboardTransactionDetails.Id,
                Number = dashboardTransactionDetails.Number,
                Date = dashboardTransactionDetails.Date,
                HijriDate = dashboardTransactionDetails.DateH,
                LetterTypeId = dashboardTransactionDetails.LetterTypeId,
                LetterType = dashboardTransactionDetails.LetterType,
                PriorityId = dashboardTransactionDetails.PriorityId,
                Priority = dashboardTransactionDetails.Priority,
                ConfidentialityId = dashboardTransactionDetails.ConfidentialityId,
                Confidentiality = dashboardTransactionDetails.Confidentiality,
                Subject = dashboardTransactionDetails.Subject,
                TransactionType = dashboardTransactionDetails.TransactionType,
                CreatedOn = dashboardTransactionDetails.CreatedOn,
                Creator = dashboardTransactionDetails.Creator,
                CurrentUser = dashboardTransactionDetails.CurrentUser
            };
            return transactionDetailsDTO;
        }
    }
}