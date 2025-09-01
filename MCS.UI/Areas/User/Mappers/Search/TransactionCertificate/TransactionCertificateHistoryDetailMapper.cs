using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search
{
    public static class TransactionCertificateHistoryDetailMapper
    {
        public static TransactionCertificateHistoryDetailVM Map(TransactionCertificateHistoryDetailDTO transactionCertificateHistoryDetailDTO)
        {
            if (transactionCertificateHistoryDetailDTO != null)
            {
                TransactionCertificateHistoryDetailVM transactionCertificateHistoryDetailVM = new TransactionCertificateHistoryDetailVM()
                { 
                    Date = transactionCertificateHistoryDetailDTO.Date,
                    DateH = transactionCertificateHistoryDetailDTO.DateH,
                    Description = transactionCertificateHistoryDetailDTO.Description
                };
                return transactionCertificateHistoryDetailVM;
            }
            return new TransactionCertificateHistoryDetailVM();
        }
        public static TransactionCertificateHistoryDetailDTO Map(TransactionCertificateHistoryDetailVM transactionCertificateHistoryDetailVM)
        {
            if (transactionCertificateHistoryDetailVM != null)
            {
                TransactionCertificateHistoryDetailDTO transactionCertificateHistoryDetailDTO = new TransactionCertificateHistoryDetailDTO()
                { 
                    Date = transactionCertificateHistoryDetailVM.Date,
                    DateH = transactionCertificateHistoryDetailVM.DateH,
                    Description = transactionCertificateHistoryDetailVM.Description
                };
                return transactionCertificateHistoryDetailDTO;
            }
            return new TransactionCertificateHistoryDetailDTO();
        }
        public static List<TransactionCertificateHistoryDetailDTO> Map(IList<TransactionCertificateHistoryDetailVM> transactionCertificateHistoryDetailVMs)
        {
            if (transactionCertificateHistoryDetailVMs == null || !transactionCertificateHistoryDetailVMs.Any())
            {
                return new List<TransactionCertificateHistoryDetailDTO>();
            }
            List<TransactionCertificateHistoryDetailDTO> transactionCertificateHistoryDetailDTOs = transactionCertificateHistoryDetailVMs
                .Select(transactionCertificateHistoryDetailDTO => new TransactionCertificateHistoryDetailDTO()
                { 
                    Date = transactionCertificateHistoryDetailDTO.Date,
                    DateH = transactionCertificateHistoryDetailDTO.DateH,
                    Description = transactionCertificateHistoryDetailDTO.Description
                }).ToList();
            return transactionCertificateHistoryDetailDTOs;
        }
        public static List<TransactionCertificateHistoryDetailVM> Map(IList<TransactionCertificateHistoryDetailDTO> transactionCertificateHistoryDetailDTOs)
        {
            if (transactionCertificateHistoryDetailDTOs == null || !transactionCertificateHistoryDetailDTOs.Any())
            {
                return new List<TransactionCertificateHistoryDetailVM>();
            }
            List<TransactionCertificateHistoryDetailVM> transactionCertificateHistoryDetailVMs = transactionCertificateHistoryDetailDTOs
                .Select(transactionCertificateHistoryDetailVM => new TransactionCertificateHistoryDetailVM()
                { 
                    Date = transactionCertificateHistoryDetailVM.Date,
                    DateH = transactionCertificateHistoryDetailVM.DateH,
                    Description = transactionCertificateHistoryDetailVM.Description
                }).ToList();
            return transactionCertificateHistoryDetailVMs;
        }
    }
}