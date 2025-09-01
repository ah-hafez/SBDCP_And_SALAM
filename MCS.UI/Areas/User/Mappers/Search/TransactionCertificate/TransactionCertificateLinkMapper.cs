using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search.TransactionCertificate
{
    public static class TransactionCertificateLinkMapper
    {
        public static TransactionCertificateLinkVM Map(TransactionCertificateLinkDTO transactionCertificateLinkDTO)
        {
            if (transactionCertificateLinkDTO != null)
            {
                TransactionCertificateLinkVM transactionCertificateLinkVM = new TransactionCertificateLinkVM()
                { 
                    LinkTypeId = transactionCertificateLinkDTO.LinkTypeId,
                    LinkTypeName = transactionCertificateLinkDTO.LinkTypeName,
                    OrgUnitId = transactionCertificateLinkDTO.OrgUnitId,
                    Transaction = TransactionCertificateMapper.Map(transactionCertificateLinkDTO.Transaction),
                    TransactionNumber = transactionCertificateLinkDTO.TransactionNumber,
                    Year = transactionCertificateLinkDTO.Year
                };
                return transactionCertificateLinkVM;
            }
            return new TransactionCertificateLinkVM();
        }
        public static TransactionCertificateLinkDTO Map(TransactionCertificateLinkVM transactionCertificateLinkVM)
        {
            if (transactionCertificateLinkVM != null)
            {
                TransactionCertificateLinkDTO transactionCertificateLinkDTO = new TransactionCertificateLinkDTO()
                { 
                    LinkTypeId = transactionCertificateLinkVM.LinkTypeId,
                    LinkTypeName = transactionCertificateLinkVM.LinkTypeName,
                    OrgUnitId = transactionCertificateLinkVM.OrgUnitId,
                    Transaction = TransactionCertificateMapper.Map(transactionCertificateLinkVM.Transaction),
                    TransactionNumber = transactionCertificateLinkVM.TransactionNumber,
                    Year = transactionCertificateLinkVM.Year
                };
                return transactionCertificateLinkDTO;
            }
            return new TransactionCertificateLinkDTO();
        }
        public static List<TransactionCertificateLinkVM> Map(IList<TransactionCertificateLinkDTO> transactionCertificateLinkDTOs)
        {
            if (transactionCertificateLinkDTOs == null || !transactionCertificateLinkDTOs.Any())
            {
                return new List<TransactionCertificateLinkVM>();
            }
            List<TransactionCertificateLinkVM> transactionCertificateLinkVMs = transactionCertificateLinkDTOs
                .Select(transactionCertificateLinkVM => new TransactionCertificateLinkVM()
                { 
                    LinkTypeId = transactionCertificateLinkVM.LinkTypeId,
                    LinkTypeName = transactionCertificateLinkVM.LinkTypeName,
                    OrgUnitId = transactionCertificateLinkVM.OrgUnitId,
                    Transaction = TransactionCertificateMapper.Map(transactionCertificateLinkVM.Transaction),
                    TransactionNumber = transactionCertificateLinkVM.TransactionNumber,
                    Year = transactionCertificateLinkVM.Year
                }).ToList();
            return transactionCertificateLinkVMs;
        }
        public static List<TransactionCertificateLinkDTO> Map(IList<TransactionCertificateLinkVM> transactionCertificateLinkVMs)
        {
            if (transactionCertificateLinkVMs == null || !transactionCertificateLinkVMs.Any())
            {
                return new List<TransactionCertificateLinkDTO>();
            }
            List<TransactionCertificateLinkDTO> transactionCertificateLinkDTOs = transactionCertificateLinkVMs
                .Select(transactionCertificateLinkDTO => new TransactionCertificateLinkDTO()
                {
                    LinkTypeId = transactionCertificateLinkDTO.LinkTypeId,
                    LinkTypeName = transactionCertificateLinkDTO.LinkTypeName,
                    OrgUnitId = transactionCertificateLinkDTO.OrgUnitId,
                    Transaction = TransactionCertificateMapper.Map(transactionCertificateLinkDTO.Transaction),
                    TransactionNumber = transactionCertificateLinkDTO.TransactionNumber,
                    Year = transactionCertificateLinkDTO.Year
                }).ToList();
            return transactionCertificateLinkDTOs;
        }
    }
}