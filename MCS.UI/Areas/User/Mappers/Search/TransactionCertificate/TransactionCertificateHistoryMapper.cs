using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search.TransactionCertificate
{
    public static class TransactionCertificateHistoryMapper
    {
        public static TransactionCertificateHistoryVM Map(TransactionCertificateHistoryDTO transactionCertificateHistoryDTO)
        {
            if (transactionCertificateHistoryDTO != null)
            {
                TransactionCertificateHistoryVM transactionCertificateHistoryVM = new TransactionCertificateHistoryVM()
                { 
                    CertificateHistoryDetails = TransactionCertificateHistoryDetailMapper.Map(transactionCertificateHistoryDTO.CertificateHistoryDetails),
                    UserId = transactionCertificateHistoryDTO.UserId,
                    UserName = transactionCertificateHistoryDTO.UserName
                };
                return transactionCertificateHistoryVM;
            }
            return new TransactionCertificateHistoryVM();
        }
        public static TransactionCertificateHistoryDTO Map(TransactionCertificateHistoryVM transactionCertificateHistoryVM)
        {
            if (transactionCertificateHistoryVM != null)
            {
                TransactionCertificateHistoryDTO transactionCertificateHistoryDTO = new TransactionCertificateHistoryDTO()
                { 
                    CertificateHistoryDetails = TransactionCertificateHistoryDetailMapper.Map(transactionCertificateHistoryVM.CertificateHistoryDetails),
                    UserId = transactionCertificateHistoryVM.UserId,
                    UserName = transactionCertificateHistoryVM.UserName
                };
                return transactionCertificateHistoryDTO;
            }
            return new TransactionCertificateHistoryDTO();
        }
        public static List<TransactionCertificateHistoryDTO> Map(IList<TransactionCertificateHistoryVM> transactionCertificateHistoryVMs)
        {
            if (transactionCertificateHistoryVMs == null || !transactionCertificateHistoryVMs.Any())
            {
                return new List<TransactionCertificateHistoryDTO>();
            }
            List<TransactionCertificateHistoryDTO> transactionCertificateHistoryDTOs = transactionCertificateHistoryVMs
                .Select(transactionCertificateHistoryVM => new TransactionCertificateHistoryDTO()
                { 
                    CertificateHistoryDetails = TransactionCertificateHistoryDetailMapper.Map(transactionCertificateHistoryVM.CertificateHistoryDetails),
                    UserId = transactionCertificateHistoryVM.UserId,
                    UserName = transactionCertificateHistoryVM.UserName
                }).ToList();
            return transactionCertificateHistoryDTOs;
        }
        public static List<TransactionCertificateHistoryVM> Map(IList<TransactionCertificateHistoryDTO> transactionCertificateHistoryDTOs)
        {
            if (transactionCertificateHistoryDTOs == null || !transactionCertificateHistoryDTOs.Any())
            {
                return new List<TransactionCertificateHistoryVM>();
            }
            List<TransactionCertificateHistoryVM> transactionCertificateHistoryVMs = transactionCertificateHistoryDTOs
                .Select(transactionCertificateHistoryDTO => new TransactionCertificateHistoryVM()
                { 
                    CertificateHistoryDetails = TransactionCertificateHistoryDetailMapper.Map(transactionCertificateHistoryDTO.CertificateHistoryDetails),
                    UserId = transactionCertificateHistoryDTO.UserId,
                    UserName = transactionCertificateHistoryDTO.UserName
                }).ToList();
            return transactionCertificateHistoryVMs;
        }
    }
}