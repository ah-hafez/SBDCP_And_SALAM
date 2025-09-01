using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class SignedDeliveryReportMapper
    {
        public static List<SignedDeliveryReportVM> Map(IList<SignedDeliveryReportDTO> signedDeliveryReportDTOs)
        {
            int Number = 1;
            if (signedDeliveryReportDTOs == null || !signedDeliveryReportDTOs.Any())
            {
                return new List<SignedDeliveryReportVM>();
            }
            List<SignedDeliveryReportVM> signedDeliveryReportVMs = signedDeliveryReportDTOs
                .Select(b => new SignedDeliveryReportVM
                {
                    CreatedBy = b.CreatedBy,
                    CreatedOn = b.CreatedOn,
                    DocumentId = b.DocumentId,
                    Date = b.Date,
                    DateH = b.DateH,
                    Id = b.Id,
                    Number = b.NumberDelivery,

                }).ToList();
            return signedDeliveryReportVMs;
        }

    }
}