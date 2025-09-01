using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Inbound;

namespace MCS.UI.Areas.User.Mappers.Transaction.Outbound
{
    public static class TransactionLinkMapper
    {
        public static List<TransactionLinkVM> Map(IList<TransactionLinkDTO> transactionLinkDTOs)
        {
            if (transactionLinkDTOs == null || !transactionLinkDTOs.Any())
            {
                return new List<TransactionLinkVM>();
            }
            List<TransactionLinkVM> transactionLinkVMs = transactionLinkDTOs
                .Select(transactionLinkDTO => new TransactionLinkVM()
                {
                    Id = transactionLinkDTO.Id,
                    HasPermission = HandleSubject(transactionLinkDTO.ConfidentialityId),
                    LinkTypeId = transactionLinkDTO.LinkTypeId,
                    LinkTypeName = transactionLinkDTO.LinkTypeName,
                    OrgUnitId = transactionLinkDTO.OrgUnitId,
                    TransactionId = transactionLinkDTO.TransactionId,
                    TransactionNumber = transactionLinkDTO.TransactionNumber,
                    Year = transactionLinkDTO.Year,
                    DateH = transactionLinkDTO.DateH,
                    Date = transactionLinkDTO.Date,
                    Subject = HandleSubject(transactionLinkDTO.ConfidentialityId) ? transactionLinkDTO.Subject : "****",
                    TransactionType = transactionLinkDTO.TransactionType,
                    TransactionCategory = transactionLinkDTO.TransactionCategory,
                    TransactionCategoryName = transactionLinkDTO.TransactionCategoryName,
                    OrgunitName= transactionLinkDTO.OrgunitName
                }).ToList();

            return transactionLinkVMs;
        }
        public static List<TransactionLinkVM> VipMap(IList<TransactionLinkDTO> transactionLinkDTOs)
        {
            if (transactionLinkDTOs == null || !transactionLinkDTOs.Any())
            {
                return new List<TransactionLinkVM>();
            }
            IList<LookupVM> Yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
            int linkCount = 0;
            List<TransactionLinkVM> transactionLinkVMs = transactionLinkDTOs
                .Select(transactionLinkDTO => new TransactionLinkVM()
                {
                    Id = transactionLinkDTO.Id,
                    HasPermission = HandleSubject(transactionLinkDTO.ConfidentialityId),
                    LinkTypeId = transactionLinkDTO.LinkTypeId,
                    LinkTypeName = transactionLinkDTO.LinkTypeName,
                    OrgUnitId = transactionLinkDTO.OrgUnitId,
                    TransactionId = transactionLinkDTO.TransactionId,
                    TransactionNumber = transactionLinkDTO.TransactionNumber,
                    Year = Yearlookups.Where(lo => lo.Text == transactionLinkDTO.Year.ToString()).FirstOrDefault().Id,
                    DateH = transactionLinkDTO.DateH,
                    Date = transactionLinkDTO.Date,
                    Subject = HandleSubject(transactionLinkDTO.ConfidentialityId) ? transactionLinkDTO.Subject : "****",
                    TransactionType = transactionLinkDTO.TransactionType,
                    TransactionCategory = transactionLinkDTO.TransactionCategory,
                    TransactionCategoryName = transactionLinkDTO.TransactionCategoryName,
                    YearDesc = transactionLinkDTO.Year,
                    Key = linkCount++
                }).ToList();

            return transactionLinkVMs;
        }
        public static bool HandleSubject(int confidentialityId)
        {

            bool isPermition = false;
            switch (confidentialityId)
            {
                case (int)Confedentiality.HandDelivered:
                    {
                        if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered))
                        {
                            isPermition = true;
                        }
                        break;
                    }
                case (int)Confedentiality.HighConfidential:
                    {
                        if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.ExtremlyConfidential))
                        {
                            isPermition = true;
                        }
                        break;
                    }
                case (int)Confedentiality.Secret:
                    {
                        if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.ExtremlyConfidential) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.Secret))
                        {
                            isPermition = true;
                        }
                        break;
                    }
                case (int)Confedentiality.Normal:
                    {
                        isPermition = true;
                        break;
                    }





            }
            return isPermition;
        }



        public static List<TransactionLinkDTO> Map(IList<TransactionLinkVM> transactionLinkVMs)
        {
            if (transactionLinkVMs == null || !transactionLinkVMs.Any())
            {
                return new List<TransactionLinkDTO>();
            }
            List<TransactionLinkDTO> transactionLinkDTOs = transactionLinkVMs
                .Select(transactionLinkVM => new TransactionLinkDTO()
                {
                    Id = transactionLinkVM.Id,
                    LinkTypeId = transactionLinkVM.LinkTypeId,
                    LinkTypeName = transactionLinkVM.LinkTypeName,
                    OrgUnitId = transactionLinkVM.OrgUnitId,
                    TransactionId = transactionLinkVM.TransactionId,
                    TransactionNumber = transactionLinkVM.TransactionNumber,
                    Year = transactionLinkVM.Year,
                    DateH = transactionLinkVM.DateH,
                    Date = transactionLinkVM.Date,
                    Subject = transactionLinkVM.Subject,
                    TransactionType = transactionLinkVM.TransactionType,
                    TransactionCategory = transactionLinkVM.TransactionCategory,
                    TransactionCategoryName = transactionLinkVM.TransactionCategoryName
                }).ToList();

            return transactionLinkDTOs;
        }
        public static TransactionLinkVM Map(TransactionLinkDTO transactionLinkDTO)
        {
            if (transactionLinkDTO != null)
            {
                TransactionLinkVM transactionLinkVM = new TransactionLinkVM()
                {
                    Id = transactionLinkDTO.Id,
                    LinkTypeId = transactionLinkDTO.LinkTypeId,
                    LinkTypeName = transactionLinkDTO.LinkTypeName,
                    OrgUnitId = transactionLinkDTO.OrgUnitId,
                    TransactionId = transactionLinkDTO.TransactionId,
                    TransactionNumber = transactionLinkDTO.TransactionNumber,
                    Year = transactionLinkDTO.Year,
                    DateH = transactionLinkDTO.DateH,
                    Date = transactionLinkDTO.Date,
                    Subject = transactionLinkDTO.Subject,
                    TransactionType = transactionLinkDTO.TransactionType,
                    TransactionCategory = transactionLinkDTO.TransactionCategory,
                    TransactionCategoryName = transactionLinkDTO.TransactionCategoryName

                };

                return transactionLinkVM;
            }
            return new TransactionLinkVM();
        }
        public static TransactionLinkDTO Map(TransactionLinkVM transactionLinkVM)
        {
            if (transactionLinkVM != null)
            {
                TransactionLinkDTO transactionLinkDTO = new TransactionLinkDTO()
                {
                    Id = transactionLinkVM.Id,
                    LinkTypeId = transactionLinkVM.LinkTypeId,
                    LinkTypeName = transactionLinkVM.LinkTypeName,
                    OrgUnitId = transactionLinkVM.OrgUnitId,
                    TransactionId = transactionLinkVM.TransactionId,
                    TransactionNumber = transactionLinkVM.TransactionNumber,
                    Year = transactionLinkVM.Year,
                    DateH = transactionLinkVM.DateH,
                    Date = transactionLinkVM.Date,
                    Subject = transactionLinkVM.Subject,
                    TransactionType = transactionLinkVM.TransactionType,
                    TransactionCategory = transactionLinkVM.TransactionCategory,
                    TransactionCategoryName = transactionLinkVM.TransactionCategoryName

                };

                return transactionLinkDTO;
            }
            return new TransactionLinkDTO();
        }
    }
}