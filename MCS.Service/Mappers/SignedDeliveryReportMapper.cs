using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class SignedDeliveryReportMapper
    {
        public static List<SignedDeliveryReportDTO> Map(IList<SignedDeliveryReport> signedDeliveryReports)
        {
            if (signedDeliveryReports == null || !signedDeliveryReports.Any())
            {
                return new List<SignedDeliveryReportDTO>();
            }

            List<SignedDeliveryReportDTO> signedDeliveryReportDTOs = new List<SignedDeliveryReportDTO>();

            foreach (SignedDeliveryReport signedDeliveryReport in signedDeliveryReports)
            {
                SignedDeliveryReportDTO signedDeliveryReportDTO = new SignedDeliveryReportDTO();
                signedDeliveryReportDTO.CreatedBy = signedDeliveryReport.CreatedBy;
                signedDeliveryReportDTO.CreatedOn = signedDeliveryReport.CreatedOn;
                signedDeliveryReportDTO.Date = signedDeliveryReport.Date;
                signedDeliveryReportDTO.DateH = signedDeliveryReport.DateH;
                signedDeliveryReportDTO.Id = signedDeliveryReport.Id;
                signedDeliveryReportDTO.DocumentId = signedDeliveryReport.DocumentId;
                signedDeliveryReportDTO.NumberDelivery = signedDeliveryReport.TransactionDeliveryReport != null ? signedDeliveryReport.TransactionDeliveryReport.Number : "";
                signedDeliveryReportDTO.Document = DocumentMapper.Map(signedDeliveryReport.Document);
                signedDeliveryReportDTOs.Add(signedDeliveryReportDTO);

            }

            return signedDeliveryReportDTOs;
        }
    }
}