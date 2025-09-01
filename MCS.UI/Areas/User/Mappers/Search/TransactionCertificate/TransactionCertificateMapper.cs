using MCS.DTO;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;

namespace MCS.UI.Areas.User.Mappers.Search.TransactionCertificate
{
    public static class TransactionCertificateMapper
    {
        public static TransactionCertificateVM Map(TransactionCertificateDTO transactionCertificateDTO)
        {
            if (transactionCertificateDTO != null)
            {
                TransactionCertificateVM transactionCertificateVM = new TransactionCertificateVM()
                { 
                    Date = transactionCertificateDTO.Date,
                    HijriDate = transactionCertificateDTO.HijriDate,
                    Id = transactionCertificateDTO.Id,
                    Links = TransactionCertificateLinkMapper.Map(transactionCertificateDTO.Links),
                    Number = transactionCertificateDTO.Number,
                    Source = transactionCertificateDTO.Source,
                    TransactionCategory = transactionCertificateDTO.TransactionCategory

                };
                return transactionCertificateVM;
            }
            return new TransactionCertificateVM();
        }
        public static TransactionCertificateDTO Map(TransactionCertificateVM transactionCertificateVM)
        {
            if (transactionCertificateVM != null)
            {
                TransactionCertificateDTO transactionCertificateDTO = new TransactionCertificateDTO()
                {
                    Date = transactionCertificateVM.Date,
                    HijriDate = transactionCertificateVM.HijriDate,
                    Id = transactionCertificateVM.Id,
                    Links = TransactionCertificateLinkMapper.Map(transactionCertificateVM.Links),
                    Number = transactionCertificateVM.Number,
                    Source = transactionCertificateVM.Source,
                    TransactionCategory = transactionCertificateVM.TransactionCategory

                };
                return transactionCertificateDTO;
            }
            return new TransactionCertificateDTO();
        }
    }
}