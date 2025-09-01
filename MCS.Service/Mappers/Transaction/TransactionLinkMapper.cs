using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionLinkMapper
    {

        public static TransactionLink Map(TransactionLinkDTO transactionLinkDTO, TransactionCategory transactionCategory)
        {
            TransactionLink link = new TransactionLink()
            {
                TypeId = transactionLinkDTO.LinkTypeId,
                ToTransactionId = transactionLinkDTO.TransactionId,
                Id = transactionLinkDTO.Id
            };

            return link;
        }

        public static TransactionLinkDTO Map(TransactionLink transactionLink)
        {
            if(transactionLink  == null)
            {

                return  new TransactionLinkDTO();
            }
            TransactionLinkDTO transactionLinkDTO = new TransactionLinkDTO()
            {
                Id = transactionLink.Id,
                TransactionId = transactionLink.ToTransaction.Id,
                LinkTypeId = transactionLink.TypeId,
                TransactionCategory = transactionLink.ToTransaction.TransactionCategoryId,
                DateH = transactionLink.ToTransaction.DateH,
                Date = transactionLink.ToTransaction.Date.ToShortDateString(),
                Year = transactionLink.ToTransaction.YearH,
                Subject = transactionLink.ToTransaction.Subject,
                ConfidentialityId = transactionLink.ToTransaction.ConfidentialityId,
                TransactionNumber = transactionLink.ToTransaction.Number.ToString(),
                TransactionCategoryName = transactionLink.ToTransaction.TransactionCategory.Text,
                OrgunitName = transactionLink.ToTransaction?.Assignments?.FirstOrDefault()?.ToEntity?.LocalName

            };

            return transactionLinkDTO;
        }

        public static List<TransactionLink> Map(IList<TransactionLinkDTO> transactionLinkDTOs, TransactionCategory transactionCategory)
        {
            List<TransactionLink> transactionLinks = new List<TransactionLink>();

            foreach (TransactionLinkDTO transactionLinkDTO in transactionLinkDTOs)
            {
                transactionLinks.Add(Map(transactionLinkDTO, transactionCategory));
            }

            return transactionLinks;
        }

        public static List<TransactionLinkDTO> Map(IList<TransactionLink> transactionLinks)
        {
            List<TransactionLinkDTO> transactionLinkDTOs = new List<TransactionLinkDTO>();

            foreach (TransactionLink transactionLink in transactionLinks)
            {
                transactionLinkDTOs.Add(Map(transactionLink));
            }

            return transactionLinkDTOs;
        }
    }
}